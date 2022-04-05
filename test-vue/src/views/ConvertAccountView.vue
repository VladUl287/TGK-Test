<template>
  <div class="container mt-3">
    <form @submit.prevent="convertAccount">
      <div class="form-group">
        <label>Cчёт</label>
        <select class="form-select" v-model="form.accountNumber">
          <option
            v-for="account in accounts"
            :key="account.number"
            :value="account.number"
          >
            <p>{{ account.number }}</p>
          </option>
        </select>
      </div>
      <div class="form-group">
        <label>Валюта</label>
        <select class="form-select" v-model="form.currencyId">
          <option
            v-for="(currency, index) in cirrencies"
            :key="index"
            :value="currency.id"
          >
            <p>{{ currency.name }}</p>
          </option>
        </select>
      </div>
      <div class="form-group">
        <button type="submit" class="btn btn-success d-block mt-2">
          Конвертировать
        </button>
      </div>
    </form>
    <div v-for="(error, index) in errors" :key="index" class="mt-3">
      <div class="alert alert-danger alert-dismissible fade show" role="alert">
        {{ error }}
        <button
          type="button"
          class="btn-close"
          data-bs-dismiss="alert"
          aria-label="Close"
        ></button>
      </div>
    </div>
  </div>
</template>

<script>
import { computed, onMounted, ref } from "@vue/runtime-core";
import { useRoute, useRouter } from "vue-router";
import { useStore } from "vuex";

export default {
  setup() {
    const route = useRoute();
    const store = useStore();
    const router = useRouter();

    const form = ref({
      accountNumber: "",
      currencyId: 0,
    });
    const errors = ref([]);

    const accounts = computed(() => store.getters.StateAccounts);
    const cirrencies = computed(() => store.getters.StateCurrencies);

    onMounted(async () => {
      form.value.accountNumber = route.params.id;
      await store.dispatch("GetAccounts");
      await store.dispatch("GetCurrencies");
    });

    const convertAccount = async () => {
      try {
        errors.value = [];
        await store.dispatch("ConvertAccount", form.value);
        router.push("/");
      } catch {
        errors.value.push("Ошибка конвертации счёта.");
      }
    };

    return {
      form,
      errors,
      accounts,
      cirrencies,
      convertAccount,
    };
  },
};
</script>