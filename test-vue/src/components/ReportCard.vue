<template>
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
</template>
<script>
export default {
  props: {
    report: Object,
    format_date: Function,
  },
};
</script>