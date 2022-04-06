import instance from '@/http';

const state = {
    accounts: [],
    reports: [],
    currencies: []
};
const getters = {
    StateAccounts: state => state.accounts,
    StateReports: state => state.reports,
    StateCurrencies: state => state.currencies
};
const actions = {
    async GetAccounts({ commit }) {
        let result = await instance.get('account/accounts')
        await commit('setAccounts', result.data);
    },
    async CreateAccount(_, form) {
        await instance.post('account/create', form)
    },
    async Transfer(_, form) {
        await instance.post('account/transfer', form)
    },
    async ConvertAccount(_, form) {
        await instance.post('account/convert', form)
    },
    async TopUpAccount(_, form) {
        await instance.post('account/topup', form)
    },
    async GetReports({ commit }, filter) {
        let query = '?';
        if (filter) {
            for (const [key, value] of Object.entries(filter)) {
                if (value) {
                    query += `${key}=${value}&`;
                }
            }
        }
        let result = await instance.get('report' + query)
        await commit('setReports', result.data);
    },
    async GetCurrencies({ commit }) {
        let result = await instance.get('currency')
        await commit('setCurrencies', result.data);
    },
};
const mutations = {
    setAccounts(state, accounts) {
        state.accounts = accounts;
    },
    setCurrencies(state, currencies) {
        state.currencies = currencies;
    },
    setReports(state, reports) {
        state.reports = reports;
    }
};

export default {
    state,
    getters,
    actions,
    mutations
};