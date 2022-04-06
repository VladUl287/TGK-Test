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

instance.interceptors.response.use(undefined, async (error) => {
    if (error.response.status === 401 && error.config && !error.config._isRetry) {
        try {
            await store.dispatch('Refresh');
            return instance.request(error.config);
        } catch {
            await store.dispatch('Refresh');
            router.push('/login');
        }
    }
    return Promise.reject(error);
});

export default instance;