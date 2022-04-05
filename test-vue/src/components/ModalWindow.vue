<template>
  <div class="d-inline">
    <button
      type="button"
      class="btn btn-primary my-2"
      data-bs-toggle="modal"
      v-bind:data-bs-target="'#' + id"
    >
      {{header}}
    </button>

    <div
      class="modal fade"
      v-bind:id="id"
      tabindex="-1"
      aria-labelledby="exampleModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">
              {{header}}
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
                  type="number"
                  class="form-control"
                  placeholder="сумма"
                  v-model="form.value"
                />
              </div>

              <div class="form-group mt-2">
                <select class="form-select" v-model="form.currencyId">
                  <option
                    v-for="currency of currencies"
                    :key="currency.id"
                    :value="currency.id"
                  >
                    {{ currency.name }}
                  </option>
                </select>
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
              @click="submit"
            >
              {{header}}
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from "@vue/reactivity";
export default {
  props: {
    id: {
      type: String,
      required: true
    },
    header: {
      type: String,
      required: true
    },
    currencies: {
      type: Array,
      required: true
    },
  },
  setup(_, { emit }) {
    const form = ref({
      value: 0,
      currencyId: 0,
    });

    const submit = () => {
      emit("submit", form);
    };

    return {
      form,
      submit,
    };
  },
};
</script>