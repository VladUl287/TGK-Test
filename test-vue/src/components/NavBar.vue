<template>
  <div
    class="
      d-flex
      flex-column flex-shrink-0
      p-3
      bg-light
      vh-100
      user-select-none
    "
    style="width: 300px"
  >
    <span class="fs-4">Навигация</span>
    <hr />
    <ul class="nav nav-pills flex-column mb-auto">
      <li class="nav-item">
        <router-link to="/" class="nav-link" active-class="active">
          <svg class="bi me-2" width="16" height="16">
            <use xlink:href="#home" />
          </svg>
          Счета
        </router-link>
      </li>
      <li>
        <router-link to="/reports" class="nav-link" active-class="active">
          <svg class="bi me-2" width="16" height="16">
            <use xlink:href="#speedometer2" />
          </svg>
          История
        </router-link>
      </li>
    </ul>
    <hr />
    <div class="dropdown">
      <span
        href="#"
        class="
          d-flex
          align-items-center
          link-dark
          text-decoration-none
          dropdown-toggle
          w-100
        "
        id="dropdownUser2"
        data-bs-toggle="dropdown"
        aria-expanded="false"
      >
        <strong>email@mail.ru</strong>
      </span>
      <ul
        class="dropdown-menu text-small shadow"
        aria-labelledby="dropdownUser2"
      >
        <li><a class="dropdown-item" href="#">Профиль</a></li>
        <li><hr class="dropdown-divider" /></li>
        <li><button class="dropdown-item" @click="logout">Выйти</button></li>
      </ul>
    </div>
  </div>
</template>

<script>
import { useRouter } from 'vue-router';
import { useStore } from 'vuex';
export default {
  setup() {
    const store = useStore();
    const router = useRouter();

    const logout = async () => {
      await store.dispatch("Logout");
      router.push("/login");
    };

    return {
      logout,
    };
  },
};
</script>