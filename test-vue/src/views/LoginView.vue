<template>
  <div class="container">
    <div class="row">
      <div class="col-md-5 mx-auto border rounded-3">
        <div class="col-md-12 my-3 text-center">
          <h2>Войти</h2>
        </div>
        <form @submit.prevent="submit">
          <div class="form-group">
            <input
              type="email"
              class="form-control"
              placeholder="Введите email"
              v-model="v$.form.email.$model"
              :class="{ 'is-invalid': v$.form.email.$errors.length }"
            />
            <div class="invalid-feedback" v-if="v$.form.email.$error">
              Некорректный email
            </div>
          </div>
          <div class="form-group mt-2">
            <input
              type="password"
              class="form-control"
              placeholder="Введите пароль"
              v-model="v$.form.password.$model"
              :class="{ 'is-invalid': v$.form.password.$errors.length }"
            />
            <div class="invalid-feedback" v-if="v$.form.password.$error">
              Пароль не менее 6-ти символов
            </div>
          </div>
          <div class="col-md-12 text-center mt-2">
            <button
              type="submit"
              class="btn btn-primary w-50"
              :disabled="v$.form.$invalid"
            >
              Войти
            </button>
          </div>
          <div class="col-md-12">
            <div class="login-or">
              <hr class="hr-or" />
              <p class="text-center">
                Нет аккаунта?
                <router-link to="/register">Заркгистрироваться</router-link>
              </p>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions } from "vuex";
import useVuelidate from "@vuelidate/core";
import { required, email, minLength } from "@vuelidate/validators";

export default {
  name: "LoginView",

  setup() {
    return { v$: useVuelidate() };
  },

  data() {
    return {
      form: {
        email: "",
        password: "",
      },
    };
  },

  methods: {
    ...mapActions(["Login"]),
    async submit() {
      try {
        await this.Login(this.form);
        // this.$router.push("/");
      } catch (error) {
        alert("Неверный логин или пароль.");
      }
    },
  },

  validations() {
    return {
      form: {
        email: {
          required,
          email,
        },
        password: {
          required,
          min: minLength(6),
        },
      },
    };
  },
};
</script>

<style scoped>
.container {
  margin-top: 5%;
  user-select: none;
}
</style>