import { createApp } from 'vue'
import App from './App.vue'
import router from './router';
import store from './store';
import axios from 'axios';
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"

axios.defaults.withCredentials = true;
axios.defaults.baseURL = 'https://localhost:7242/api/';

axios.interceptors.response.use(undefined, (error) => {
    if (error) {
        const originalRequest = error.config;
        if (error.response.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            store.dispatch('Logout')
            return router.push('/login')
        }
        return Promise.reject(error);
    }
})

createApp(App).use(store).use(router).mount('#app')