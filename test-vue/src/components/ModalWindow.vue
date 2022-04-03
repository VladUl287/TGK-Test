<template>
  <div>
    <button
      type="button"
      class="btn btn-primary my-2"
      data-bs-toggle="modal"
      data-bs-target="#exampleModal"
    >
      Открыть лицевой счёт
    </button>

    <div
      class="modal fade"
      id="exampleModal"
      tabindex="-1"
      aria-labelledby="exampleModalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">
              Открыть лицевой счёт
            </h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Закрыть"
            ></button>
          </div>
          <div class="modal-body">
            <!-- <form>
            <div class="form-group">
              <input
                type="number"
                class="form-control"
                placeholder="сумма"
                v-model="form.value"
              />
            </div>

            <div class="form-group mt-2">
              <select v-model="form.currencyId" class="form-select">
                <option
                  v-for="currency of currencies"
                  :key="currency.id"
                  :value="currency.id"
                >
                  {{ currency.name }}
                </option>
              </select>
            </div>
          </form> -->
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-secondary"
              data-bs-dismiss="modal"
            >
              Закрыть
            </button>
            <!-- @click="createAccount" -->
            <button type="button" class="btn btn-primary">
              Открыть лицевой счёт
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {};
</script>