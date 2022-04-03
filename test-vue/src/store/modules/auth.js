import axios from 'axios';

const state = {
    user: null
};
const getters = {
    isAuthenticated: state => !!state.user,
    StateUser: state => state.user,
};
const actions = {
    async Register(_, form) {
        const user = new FormData();
        user.append("email", form.email);
        user.append("password", form.password);
        await axios.post('auth/register', user)
    },
    async Login({ commit }, form) {
        const user = new FormData();
        user.append("email", form.email);
        user.append("password", form.password);
        let result = await axios.post('auth/login', user);
        await commit('setUser', result.data)
    },
    async Logout({ commit }) {
        commit('logout')
        await axios.post('auth/logout')
    }
};
const mutations = {
    setUser(state, user) {
        state.user = user
    },
    logout(state) {
        state.user = null
    },
};

export default {
    state,
    getters,
    actions,
    mutations
};