<template>
  <div class="container vh-100 d-flex align-items-center">
    <div class="row w-100">
      <div class="col-md-5 mx-auto border rounded-3">
        <div class="col-md-12 my-3 text-center">
          <h2>Регистрация</h2>
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
              v-if="!loading"
            >
              Зарегистрироваться
            </button>
            <button class="btn btn-primary" type="button" disabled v-else>
              <span
                class="spinner-grow spinner-grow-sm"
                role="status"
                aria-hidden="true"
              ></span>
              Загрузка...
            </button>
          </div>
          <div class="col-md-12">
            <div class="login-or">
              <hr class="hr-or" />
              <p class="text-center">
                Уже есть аккаунт?
                <router-link to="/login">Войти</router-link>
              </p>
            </div>
          </div>
        </form>
        <div class="alert alert-danger" role="alert" v-if="result">
          {{ result }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import useVuelidate from "@vuelidate/core";
import { required, email, minLength } from "@vuelidate/validators";
import { useRouter } from "vue-router";
import { useStore } from "vuex";
import { ref } from "@vue/reactivity";

export default {
  setup() {
    const v$ = useVuelidate();
    const router = useRouter();
    const store = useStore();

    const form = ref({
      email: "",
      password: "",
    });

    const result = ref(null);
    const loading = ref(false);

    const submit = async () => {
      try {
        loading.value = true;
        await store.dispatch("Register", form.value);
        router.push("/login");
      } catch (error) {
        result.value = error.response.data.message;
      } finally {
        loading.value = false;
      }
    };

    return {
      v$,
      form,
      result,
      loading,
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