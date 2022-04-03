<template>
  <!-- <div class="d-flex flex-column flex-shrink-0 p-3 user-select-none" style="width: 280px">
    <span class="fs-4">Разделы</span>
    <hr />
    <ul class="nav nav-pills flex-column mb-auto">
      <li class="nav-item">
        <a href="#" class="nav-link active" aria-current="page"> Счета </a>
      </li>
      <li>
        <a href="#" class="nav-link link-dark"> История </a>
      </li>
    </ul>
  </div> -->
  <div class="container">
    <button
      type="button"
      class="btn btn-primary mb-2"
      data-bs-toggle="modal"
      data-bs-target="#exampleModal"
    >
      Открыть лицевой счёт
    </button>

    <!-- Модальное окно -->
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
              Заголовок модального окна
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
                  type="email"
                  class="form-control"
                  placeholder="сумма"
                  v-model="form.value"
                />
              </div>
              <div class="form-group mt-2">
                <input
                  type="password"
                  class="form-control"
                  placeholder="валюта"
                  v-model="form.currency"
                />
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

    <div class="card mb-2" v-for="account of accounts" :key="account.number">
      <h5 class="card-header">{{ account.number }}</h5>
      <div class="card-body">
        <h5 class="card-title">{{ account.value }}</h5>
        <p class="card-text">
          With supporting text below as a natural lead-in to additional content.
        </p>
        <router-link
          :to="{ name: 'Transfer', params: { id: account.number } }"
          class="btn btn-primary"
          >Перевести</router-link
        >
      </div>
    </div>
  </div>
</template>
<script>
import { mapActions } from "vuex";

export default {
  name: "HomeView",

  data() {
    return {
      form: {
        value: "",
        currency: "",
        userId: 0,
      },
    };
  },

  computed: {
    accounts() {
      return this.$store.getters.StateAccounts;
    },
  },

  async mounted() {
    let userId = this.$store.getters.StateUser.id;
    await this.GetAccounts(userId);
  },

  methods: {
    ...mapActions(["GetAccounts", "CreateAccount"]),
    async createAccount() {
      this.form.userId = this.$store.getters.StateUser.id;
      this.form.value = +this.form.value;
      this.form.currency = +this.form.currency;
      await this.CreateAccount(this.form);
      let userId = this.$store.getters.StateUser.id;
      this.GetAccounts(userId);
    },
  },
};
</script>
