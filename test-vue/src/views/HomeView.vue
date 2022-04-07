<template>
  <div class="container vh-100 overflow-auto">
    <ModalWindow
      id="createAccount"
      header="Открыть лицевой счёт"
      :currencies="currencies"
      @submit="createAccount"
    />

    <AccountCardList :accounts="accounts" />
  </div>
</template>
<script>
import { useStore } from "vuex";
import { computed, onMounted } from "@vue/runtime-core";
import ModalWindow from "@/components/ModalWindow.vue";
import AccountCardList from "@/components/AccountCardList.vue";
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
    AccountCardList,
  },
};
</script>
