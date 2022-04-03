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
    v-if="isLoggedIn"
  >
    <span class="fs-4">Навигация</span>
    <hr />
    <ul class="nav nav-pills flex-column mb-auto">
      <li class="nav-item">
        <router-link to="/" class="nav-link active" aria-current="page">
          <svg class="bi me-2" width="16" height="16">
            <use xlink:href="#home" />
          </svg>
          Счета
        </router-link>
      </li>
      <li>
        <router-link to="/reports" class="nav-link link-dark">
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
export default {
  name: "NavBar",
  computed: {
    isLoggedIn() {
      return this.$store.getters.isAuthenticated;
    },
  },
  methods: {
    async logout() {
      await this.$store.dispatch("Logout");
      this.$router.push("/login");
    },
  },
};
</script>