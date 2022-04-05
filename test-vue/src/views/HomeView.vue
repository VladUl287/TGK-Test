<template>
  <div class="container vh-100 overflow-auto">
    <ModalWindow
      id="createAccount"
      header="Открыть лицевой счёт"
      :currencies="currencies"
      @submit="createAccount"
    />

    <div class="card mb-2" v-for="account of accounts" :key="account.number">
      <h5 class="card-header">Номер счёта: {{ account.number }}</h5>
      <div class="card-body">
        <h5 class="card-title mb-3">
          Баланс: {{ account.value }} {{ account.currency.sign }}
        </h5>
        <router-link
          :to="{ name: 'TopUp', params: { id: account.number } }"
          class="btn btn-primary"
        >
          Пополнить
        </router-link>
        <router-link
          :to="{ name: 'Transfer', params: { id: account.number } }"
          class="btn btn-success mx-2"
        >
          Перевести
        </router-link>
        <router-link
          :to="{ name: 'Convert', params: { id: account.number } }"
          class="btn btn-primary"
        >
          Конвертировать
        </router-link>
      </div>
    </div>
  </div>
</template>
<script>
import { useStore } from "vuex";
import { computed, onMounted } from "@vue/runtime-core";
import ModalWindow from "@/components/ModalWindow.vue";
export default {
  setup() {
    const store = useStore();

    const accounts = computed(() => store.getters.StateAccounts);
    const currencies = computed(() => store.getters.StateCurrencies);

    onMounted(async () => {
      await store.dispatch("GetAccounts");
      await store.dispatch("GetCurrencies");
    });

    const createAccount = async (form) => {
      await store.dispatch("CreateAccount", form.value);
      await store.dispatch("GetAccounts");
    };

    return {
      accounts,
      currencies,
      createAccount,
    };
  },
  components: {
    ModalWindow,
  },
};
</script>
