import axios from 'axios';

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
        let result = await axios.get('account/accounts')
        await commit('setAccounts', result.data);
    },
    async CreateAccount(_, form) {
        await axios.post('account/create', form)
    },
    async Transfer(_, form) {
        await axios.post('account/transfer', form)
    },
    async GetReports({ commit }) {
        let result = await axios.get('report')
        await commit('setReports', result.data);
    },
    async GetCurrencies({ commit }) {
        let result = await axios.get('currency')
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