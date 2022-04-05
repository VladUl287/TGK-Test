<template>
  <div class="container-fluid">
    <div class="row flex-nowrap">
      <NavBar v-if="isLoggedIn" />
      <div class="col p-0">
        <router-view />
      </div>
    </div>
  </div>
</template>

<script>
import NavBar from "@/components/NavBar.vue";
import { computed } from "@vue/runtime-core";
import { useStore } from "vuex";
export default {
  setup() {
    const store = useStore();

    const isLoggedIn = computed(() => store.getters.isAuthenticated);

    return {
      isLoggedIn,
    };
  },
  components: {
    NavBar,
  },
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
</style>
