<template>
  <div>
      <div class="card mb-2" v-for="report of reports" :key="report.id">
        <div class="card-header">
          Перевод {{ report.toPersonalAccountId }}
        </div>
        <div class="card-body">
          <h5 class="card-title">{{ report.value }} {{ report.currency == 0 ? "Руб." : "Усл.ед" }}</h5>
          <p class="card-text">
            Отправлено со счёта: {{report.personalAccountId}}
          </p>
        </div>
        <div class="card-footer text-muted">{{report.dateTransfer}}</div>
      </div>
  </div>
</template>

<script>
import { mapActions } from "vuex";
export default {
  name: "ReportView",
  computed: {
    reports() {
      return this.$store.getters.StateReports;
    },
  },

  mounted() {
    let userId = this.$store.getters.StateUser.id;
    this.GetReports(userId);
  },

  methods: {
    ...mapActions(["GetReports"]),
  },
};
</script>

<style>
</style>