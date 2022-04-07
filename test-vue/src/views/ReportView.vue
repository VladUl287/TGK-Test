<template>
  <div class="container vh-100 overflow-auto pt-2" v-if="!loading">
    <div class="mb-2">
      <ReportFilters
        :accounts="accounts"
        :currencies="currencies"
        :isGraphic="isGraphic"
        @search="search"
        @tooggle="tooggle"
      />
    </div>
    <div v-if="!isGraphic">
      <div class="card mb-2" v-for="report of reports.slice().reverse()" :key="report.id">
        <ReportCard :report="report" :format_date="format_date" />
      </div>
    </div>
    <div v-else>
      <LineChart :chartData="testData" :options="options" />
    </div>
  </div>
  <div class="d-flex justify-content-center align-items-center h-100" v-else>
    <div class="spinner-border text-primary" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
</template>

<script>
import moment from "moment";
import { useStore } from "vuex";
import { LineChart } from "vue-chart-3";
import { Chart, registerables } from "chart.js";
import { computed, defineComponent, onMounted, ref } from "vue";
import ReportFilters from "@/components/ReportFilters.vue";
import ReportCard from "@/components/ReportCard.vue";

Chart.register(...registerables);

export default defineComponent({
  setup() {
    const store = useStore();
    const isGraphic = ref(true);

    const loading = ref(true);

    const testData = ref({
      labels: [],
      datasets: [],
    });
    const options = ref({
      responsive: true,
      plugins: {
        legend: {
          display: false,
        },
      },
      scales: {
        x: {
          display: true,
          title: {
            display: true,
            text: "Дата и время",
          },
        },
        y: {
          display: true,
          title: {
            display: true,
            text: "Баланс",
          },
        },
      },
    });

    const reports = computed(() => store.getters.StateReports);
    const accounts = computed(() => store.getters.StateAccounts);
    const currencies = computed(() => store.getters.StateCurrencies);

    onMounted(async () => {
      await store.dispatch("GetAccounts");
      await store.dispatch("GetCurrencies");
      if (accounts.value.length > 0) {
        await store.dispatch("GetReports", {
          accountNumber: accounts.value[0].number,
          currencyId: accounts.value[0].currencyId,
        });
      }
      loading.value = false;
      setupGraphic();
    });

    const down = (ctx, value) =>
      ctx.p0.parsed.y > ctx.p1.parsed.y ? value : undefined;

    const setupGraphic = () => {
      testData.value = {
        labels: reports.value.map((e) => format_date(e.dateTransfer)),
        datasets: [
          {
            data: reports.value.map((e) => e.accountValue),
            borderColor: "#3e95cd",
            segment: {
              borderColor: (ctx) => down(ctx, "rgb(192,75,75)"),
            },
          },
        ],
      };
    };

    const format_date = (value) => {
      if (value) {
        return moment(String(value)).locale("ru").format("HH:MM DD.MM.YYYY");
      }
    };

    const search = async (filter) => {
      await store.dispatch("GetReports", filter.value);
      setupGraphic();
    };

    const tooggle = () => {
      isGraphic.value = !isGraphic.value;
    };

    return {
      search,
      reports,
      options,
      loading,
      testData,
      accounts,
      isGraphic,
      currencies,
      format_date,
      tooggle,
    };
  },
  components: {
    ReportFilters,
    ReportCard,
    LineChart,
  },
});
</script>