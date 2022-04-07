<template>
  <div class="card mb-2" v-for="account of accounts" :key="account.number">
    <AccountCard :account="account" />
  </div>
</template>
<script>
import AccountCard from '@/components/AccountCard.vue';
export default {
    props: {
        accounts: Array
    },
    components: {
        AccountCard
    }
};
</script>