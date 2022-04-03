<template>
  <div>
    <form @submit.prevent="transfer">
      <input type="text" name="fromAccount" v-model="form.fromAccountId" />
      <input type="text" name="toAccount" v-model="form.toAccountId" />
      <input type="text" name="value" v-model="form.value" />
      <button type="submit">Send</button>
    </form>
    <div v-for="(error, index) in errors" :key="index">
      <div>
        {{ error }}
      </div>
    </div>
  </div>
</template>

<script>
import { useRoute } from "vue-router";
import { mapActions } from "vuex";

export default {
  name: "TransferView",
  mounted() {
    const route = useRoute();
    this.form.fromAccountId = route.params.id;
  },

  data() {
    return {
      form: {
        fromAccountId: "",
        toAccountId: "",
        value: 0,
      },
      errors: [],
    };
  },

  methods: {
    ...mapActions(["Transfer"]),
    async transfer() {
      try {
        await this.Transfer(this.form);
      } catch {
        alert("error");
      }
    },
  },
};
</script>