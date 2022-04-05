import Vuex from 'vuex';
import auth from './modules/auth';
import account from './modules/account';
import createPersistedState from "vuex-persistedstate";

export default new Vuex.Store({
    modules: {
        auth,
        account
    },
    plugins: [createPersistedState({
        paths: ['auth']
    })]
});