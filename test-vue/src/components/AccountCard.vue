<template>
  <h5 class="card-header">Номер счёта: {{ account.number }}</h5>
  <div class="card-body">
    <h5 class="card-title mb-3">
      Баланс: {{ account.value }} {{ account.currency.sign }}
    </h5>
    <router-link
      :to="{ name: 'TopUp', params: { id: account.number } }"
      class="btn btn-primary me-2"
    >
      Пополнить
    </router-link>
    <router-link
      :to="{ name: 'Transfer', params: { id: account.number } }"
      class="btn btn-success me-2"
    >
      Перевести
    </router-link>
    <router-link
      :to="{ name: 'Convert', params: { id: account.number } }"
      class="btn btn-primary me-2"
    >
      Конвертировать
    </router-link>
  </div>
</template>
<script>
export default {
  props: {
    account: Object,
  },
};
</script>