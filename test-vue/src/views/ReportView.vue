<template>
  <div class="container vh-100 overflow-auto pt-2">
    <div class="mb-2">
      <div class="mb-1">
        <label>Номер счёта</label>
        <select class="form-select" v-model="filter.accountNumber">
          <option
            v-for="(account, index) in accounts"
            :key="index"
            :value="account.number"
          >
            <p>{{ account.number }}</p>
          </option>
        </select>
      </div>
      <div class="mb-2">
        <label>Валюта</label>
        <select class="form-select" v-model="filter.currencyId">
          <option
            v-for="(currency, index) in currencies"
            :key="index"
            :value="currency.id"
          >
            <p>{{ currency.name }}</p>
          </option>
        </select>
      </div>
      <div class="mb-2 d-flex align-items-center">
        <p class="my-0 me-1">с</p>
        <input type="date" class="form-control" v-model="filter.startDate" />
        <p class="my-0 ms-3 me-1">по</p>
        <input type="date" class="form-control" v-model="filter.endDate" />
      </div>
      <div class="mb-1 text-end">
        <button class="btn btn-primary" @click="tooggle">
          {{ isGraphic ? "Отчёты" : "График" }}
        </button>
        <button class="btn btn-success mx-1" @click="reset">
          Сбрость фильтры
        </button>
        <button class="btn btn-success" @click="search">Поиск</button>
      </div>
    </div>
    <div v-if="!isGraphic">
      <div class="card mb-2" v-for="report of reports" :key="report.id">
        <div class="card-header">Отчёт: #{{ report.id }}</div>
        <div class="card-body">
          <h5 class="card-title text-success" v-if="report.credited">
            +{{ report.transferValue }} {{ report.currency.sign }}
          </h5>
          <h5 class="card-title text-danger" v-else>
            -{{ report.transferValue }} {{ report.currency.sign }}
          </h5>
          <p class="card-text">Счёт: {{ report.personalAccountId }}</p>
        </div>
        <div class="card-footer text-muted">
          {{ format_date(report.dateTransfer) }}
        </div>
      </div>
    </div>
    <div v-else>
      <LineChart :chartData="testData" :options="options" />
    </div>
  </div>
</template>

<script>
import moment from "moment";
import { computed, defineComponent, onMounted, ref } from "vue";
import { LineChart } from "vue-chart-3";
import { Chart, registerables } from "chart.js";
import { useStore } from "vuex";

Chart.register(...registerables);

export default defineComponent({
  components: { LineChart },
  setup() {
    const store = useStore();

    const filter = ref({
      accountNumber: null,
      currencyId: null,
      startDate: null,
      endDate: null,
    });

    const isGraphic = ref(false);

    let loading = ref(true);
    const testData = ref({
      labels: [],
      datasets: [],
    });

    const options = ref({
      responsive: true,
      plugins: {
        legend: {
          position: "top",
        },
        title: {
          display: true,
          text: "Chart.js Doughnut Chart",
        },
      },
    });

    const reports = computed(() => store.getters.StateReports);
    const accounts = computed(() => store.getters.StateAccounts);
    const currencies = computed(() => store.getters.StateCurrencies);

    onMounted(async () => {
      await store.dispatch("GetAccounts");
      await store.dispatch("GetCurrencies");
      filter.value.accountNumber = accounts.value[0].number;
      await store.dispatch("GetReports", filter);
      testData.value = {
        labels: reports.value.map((e) => format_date(e.dateTransfer)),
        datasets: [
          {
            data: reports.value.map((e) => e.accountValue),
          },
        ],
      };
      loading.value = false;
    });

    const format_date = (value) => {
      if (value) {
        return moment(String(value)).locale("ru").format("HH:MM DD.MM.YYYY");
      }
    };

    const reset = () => {
      filter.value.currencyId = null;
      filter.value.startDate = null;
      filter.value.endDate = null;
    };

    const search = async () => {
      await store.dispatch("GetReports", filter.value);
      testData.value = {
        labels: reports.value.map((e) => format_date(e.dateTransfer)),
        datasets: [
          {
            data: reports.value.map((e) => e.accountValue),
          },
        ],
      };
    };

    const tooggle = () => {
      isGraphic.value = !isGraphic.value;
    };

    return {
      search,
      reset,
      reports,
      filter,
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
});
</script>