<template>
  <div class="container mt-5">
    <div class="row">
      <div class="col-md-5 mx-auto border rounded-3">
        <div class="col-md-12 my-3 text-center">
          <h2>Войти</h2>
        </div>
        <form @submit.prevent="submit" class="user-select-none">
          <div class="form-group">
            <input
              type="email"
              class="form-control"
              placeholder="Введите email"
              v-model="validate.form.email.$model"
              :class="{ 'is-invalid': validate.form.email.$errors.length }"
            />
            <div class="invalid-feedback" v-if="validate.form.email.$error">
              Некорректный email
            </div>
          </div>
          <div class="form-group mt-2">
            <input
              type="password"
              class="form-control"
              placeholder="Введите пароль"
              v-model="validate.form.password.$model"
              :class="{ 'is-invalid': validate.form.password.$errors.length }"
            />
            <div class="invalid-feedback" v-if="validate.form.password.$error">
              Пароль не менее 6-ти символов
            </div>
          </div>
          <div class="col-md-12 text-center mt-2">
            <button
              type="submit"
              class="btn btn-primary w-50"
              :disabled="validate.form.$invalid"
            >
              Войти
            </button>
          </div>
          <div class="col-md-12">
            <div class="login-or">
              <hr class="hr-or" />
              <p class="text-center">
                Нет аккаунта?
                <router-link to="/register">Зарегистрироваться</router-link>
              </p>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { useStore } from "vuex";
import useVuelidate from "@vuelidate/core";
import { required, email, minLength } from "@vuelidate/validators";
import { ref } from "@vue/reactivity";
import { useRouter } from "vue-router";

export default {
  setup() {
    const validate = useVuelidate();
    const router = useRouter();
    const store = useStore();

    const form = ref({
      email: "",
      password: "",
    });

    const submit = async () => {
      try {
        await store.dispatch("Login", form.value);
        router.push("/");
      } catch (error) {
        alert("Неверный логин или пароль.");
      }
    };

    return {
      form,
      validate,
      submit,
    };
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