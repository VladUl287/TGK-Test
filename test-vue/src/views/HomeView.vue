<template>
  <div class="container vh-100 overflow-auto">
    <button
      type="button"
      class="btn btn-primary my-2"
      data-bs-toggle="modal"
      data-bs-target="#exampleModal"
    >
      Открыть лицевой счёт
    </button>

    <div
      class="modal fade"
      id="exampleModal"
      tabindex="-1"
      aria-labelledby="exampleModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">
              Открыть лицевой счёт
            </h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Закрыть"
            ></button>
          </div>
          <div class="modal-body">
            <form>
              <div class="form-group">
                <input
                  type="number"
                  class="form-control"
                  placeholder="сумма"
                  v-model="form.value"
                />
              </div>

              <div class="form-group mt-2">
                <select v-model="form.currencyId" class="form-select">
                  <option
                    v-for="currency of currencies"
                    :key="currency.id"
                    :value="currency.id"
                  >
                    {{ currency.name }}
                  </option>
                </select>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-secondary"
              data-bs-dismiss="modal"
            >
              Закрыть
            </button>
            <button
              type="button"
              class="btn btn-primary"
              @click="createAccount"
            >
              Открыть счёт
            </button>
          </div>
        </div>
      </div>
    </div>
    <!-- <ModalWindow header='Открыть лицевой счёт' /> -->

    <div class="card mb-2" v-for="account of accounts" :key="account.number">
      <h5 class="card-header">Номер счёта: {{ account.number }}</h5>
      <div class="card-body">
        <h5 class="card-title mb-3">
          Баланс: {{ account.value }} {{ account.currency.sign }}
        </h5>
        <button class="btn btn-primary">Пополнить</button>
        <router-link
          :to="{ name: 'Transfer', params: { id: account.number } }"
          class="btn btn-success mx-2"
        >
          Перевести
        </router-link>
      </div>
    </div>
  </div>
</template>
<script>
import { useStore } from "vuex";
import { computed, onMounted, ref } from "@vue/runtime-core";

export default {
  setup() {
    const store = useStore();

    const form = ref({
      value: 0,
      currencyId: 1,
    });

    const accounts = computed(() => store.getters.StateAccounts);
    const currencies = computed(() => store.getters.StateCurrencies);

    onMounted(async () => {
      await store.dispatch("GetAccounts");
      await store.dispatch("GetCurrencies");
    });

    const createAccount = async () => {
      await store.dispatch("CreateAccount", form.value);
      await store.dispatch("GetAccounts");
    };

    return {
      form,
      accounts,
      currencies,
      createAccount,
    };
  },

  // components: {
  //   ModalWindow,
  // },
};
</script>
