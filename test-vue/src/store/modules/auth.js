import instance from '@/http';
import axios from 'axios';

const state = {
    email: null,
    access_token: null
};

const getters = {
    isAuthenticated: state => !!state.access_token,
    StateUser: state => state.email,
    StateToken: state => state.access_token,
};

const actions = {
    async Register(_, form) {
        var user = getFormData(form);
        await instance.post('auth/register', user)
    },
    async Login({ commit }, form) {
        var user = getFormData(form);
        let result = await instance.post('auth/login', user);
        await commit('setAuth', result.data);
    },
    async Logout({ commit }) {
        await commit('logout')
        await instance.post('auth/logout')
    },
    async Refresh({ commit }) {
        let result = await axios.post(instance.defaults.baseURL + 'auth/refresh', {}, {
            withCredentials: true
        });
        await commit('setAuth', result.data);
    }
};

const mutations = {
    setAuth(state, data) {
        state.email = data.email
        state.access_token = data.accessToken
    },
    logout(state) {
        state.email = null
        state.access_token = null
    },
};

export default {
    state,
    getters,
    actions,
    mutations
};

const getFormData = (form) => {
    const formData = new FormData();
    for (const [key, value] of Object.entries(form)) {
        formData.append(key, value);
    }
    return formData;
}