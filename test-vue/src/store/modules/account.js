import axios from 'axios';

const state = {
    accounts: [],
    reports: []
};
const getters = {
    StateAccounts: state => state.accounts,
    StateReports: state => state.reports
};
const actions = {
    async GetAccounts({ commit }, userId) {
        let result = await axios.get(`account/${userId}`)
        await commit('setAccounts', result.data);
    },
    async CreateAccount(_, form) {
        await axios.post('account/create', form)
    },
    async Transfer(_, form) {
        await axios.post('account/transfer', form)
    },
    async GetReports({ commit }, userId) {
        let result = await axios.get(`report/${userId}`)
        await commit('setReports', result.data);
    },
};
const mutations = {
    setAccounts(state, accounts) {
        state.accounts = accounts;
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