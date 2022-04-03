<template>
  <div class="container vh-100 overflow-auto pt-2">
    <div class="card mb-2" v-for="report of reports" :key="report.id">
      <div class="card-header">Перевод: {{ report.id }}</div>
      <div class="card-body">
        <h5 class="card-title">
          {{ report.value }} {{ report.currency.sign }}
        </h5>
        <p class="card-text">
          Отправлено со счёта: {{ report.toPersonalAccountId }}
        </p>
        <p class="card-text">
          Cчёт зачисления: {{ report.toPersonalAccountId }}
        </p>
      </div>
      <div class="card-footer text-muted">
        {{ format_date(report.dateTransfer) }}
      </div>
    </div>
  </div>
</template>

<script>
import moment from "moment";
import { useStore } from "vuex";
import { computed, onMounted } from "@vue/runtime-core";

export default {
  setup() {
    const store = useStore();

    const reports = computed(() => store.getters.StateReports);

    onMounted(async () => {
      await store.dispatch("GetReports");
    });

    const format_date = (value) => {
      if (value) {
        return moment(String(value)).locale("ru").format("MM/DD/YYYY hh:mm");
      }
    };

    return {
      reports,
      format_date,
    };
  },
};
</script>