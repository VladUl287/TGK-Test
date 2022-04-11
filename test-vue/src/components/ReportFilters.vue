<template>
  <div>
    <div class="mb-1">
      <label>Номер счёта</label>
      <select class="form-select" v-model="filter.accountNumber">
        <option
          v-for="(account, index) in accounts"
          :key="index"
          :value="account.number"
        >
          <p>{{ account.number }}</p>
        </option>
      </select>
    </div>
    <div class="mb-2">
      <label>Валюта</label>
      <select class="form-select" v-model="filter.currencyId">
        <option
          v-for="(currency, index) in currencies"
          :key="index"
          :value="currency.id"
        >
          <p>{{ currency.name }}</p>
        </option>
      </select>
    </div>
    <div class="mb-2 d-flex align-items-center">
      <p class="my-0 me-1">с</p>
      <input type="date" class="form-control" v-model="filter.startDate" />
      <p class="my-0 ms-3 me-1">по</p>
      <input type="date" class="form-control" v-model="filter.endDate" />
    </div>
    <div class="mb-1 text-end">
      <button class="btn btn-primary" @click="tooggle">
        {{ isGraphic ? "Отчёты" : "График" }}
      </button>
      <button class="btn btn-success mx-1" @click="reset">
        Сбрость фильтры
      </button>
      <button class="btn btn-success" @click="search">Поиск</button>
    </div>
  </div>
</template>
<script>
import { ref } from "@vue/reactivity";
import { onMounted } from "@vue/runtime-core";
export default {
  props: {
    isGraphic: Boolean,
    accounts: Array,
    currencies: Array,
  },
  setup(props, { emit }) {
    const filter = ref({
      accountNumber: null,
      currencyId: 1,
      startDate: null,
      endDate: null,
    });

    onMounted(() => {
      if(props.accounts.length > 0) {
        filter.value.accountNumber = props.accounts[0].number;
        filter.value.currencyId = props.accounts[0].currencyId;
      }
    });

    const reset = () => {
      filter.value.currencyId = 1;
      filter.value.startDate = null;
      filter.value.endDate = null;
    };

    const search = () => {
      emit("search", filter);
    };

    const tooggle = () => {
      emit("tooggle");
    };

    return {
      filter,
      reset,
      search,
      tooggle,
    };
  },
};
</script>