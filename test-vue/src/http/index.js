import axios from 'axios';
import router from '@/router';
import store from '@/store';

const instance = axios.create({
    baseURL: 'https://localhost:7242/api/',
    withCredentials: true
});

instance.interceptors.request.use((config) => {
    if (store.getters.StateToken) {
        config.headers = {
            Authorization: 'Bearer ' + store.getters.StateToken
        };
    }
    return config;
});

let refresh = false;
instance.interceptors.response.use(undefined, async (error) => {
    const original = error.config;
    if (error.response.status === 401 && error.config && !refresh) {
        refresh = true;
        await store.dispatch('Refresh');
        return axios.request(original);
    } else if (error.response.status === 401 && refresh) {
        refresh = false;
        await store.dispatch('Logout')
        return router.push('/login')
    }
    refresh = false;
    return Promise.reject(error);
});

export default instance;