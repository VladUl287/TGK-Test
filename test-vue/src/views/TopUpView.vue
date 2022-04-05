<template>
  <div class="container mt-3">
    <form @submit.prevent="topUpAccount">
      <div class="form-group">
        <label>Пополнить счёт</label>
        <select class="form-select" v-model="form.accountNumber">
          <option
            v-for="(account, index) in accounts"
            :key="index"
            :value="account.number"
          >
            <p>{{ account.number }}</p>
          </option>
        </select>
      </div>
      <div class="form-group">
        <label>Сумма</label>
        <input
          type="number"
          name="value"
          class="form-control"
          v-model="form.value"
        />
      </div>
      <div class="form-group">
        <button type="submit" class="btn btn-success d-block mt-2">
          Пополнить
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
      value: 0,
    });
    const errors = ref([]);

    const accounts = computed(() => store.getters.StateAccounts);

    onMounted(async () => {
      form.value.accountNumber = route.params.id;
      await store.dispatch("GetAccounts");
    });

    const topUpAccount = async () => {
      try {
        errors.value = [];
        await store.dispatch("TopUpAccount", form.value);
        router.push("/");
      } catch {
        errors.value.push("Ошибка пополнения счёта.");
      }
    };

    return {
      form,
      errors,
      accounts,
      topUpAccount,
    };
  },
};
</script>