import { createRouter, createWebHistory } from 'vue-router'
import store from '../store';
import HomeView from '../views/HomeView.vue'
import RegisterView from '../views/RegisterView.vue'
import LoginView from '../views/LoginView.vue'
import TransferView from '../views/TransferView.vue'
import ReportView from '../views/ReportView.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: HomeView,
    meta: { requiresAuth: true }
  },
  {
    path: '/register',
    name: "Register",
    component: RegisterView,
    meta: { guest: true },
  },
  {
    path: '/login',
    name: "Login",
    component: LoginView,
    meta: { guest: true },
  },
  {
    path: '/transfer/:id',
    name: "Transfer",
    component: TransferView,
    meta: { requiresAuth: true },
  },
  {
    path: '/reports',
    name: "Reports",
    component: ReportView,
    meta: { requiresAuth: true },
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (store.getters.isAuthenticated) {
      next()
      return
    }
    next('/login')
  } else {
    next()
  }
})

router.beforeEach((to, from, next) => {
  if (to.matched.some((record) => record.meta.guest)) {
    if (store.getters.isAuthenticated) {
      next("/");
      return;
    }
    next();
  } else {
    next();
  }
});

export default router