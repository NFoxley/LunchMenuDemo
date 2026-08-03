import { createRouter, createWebHistory } from 'vue-router'
import { useAuth } from '../composables/useAuth'
import HomeView from '../views/HomeView.vue'
import AdminLoginView from '../views/AdminLoginView.vue'
import AdminDashboardView from '../views/AdminDashboardView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/admin/login',
      name: 'admin-login',
      component: AdminLoginView,
      meta: { guestOnly: true },
    },
    {
      path: '/admin',
      name: 'admin',
      component: AdminDashboardView,
      meta: { requiresAuth: true },
    },
  ],
})

router.beforeEach(async (to) => {
  const { state, fetchMe } = useAuth()

  if (!state.loaded) {
    await fetchMe()
  }

  if (to.meta.requiresAuth && !state.authenticated) {
    return { name: 'admin-login', query: { redirect: to.fullPath } }
  }

  if (to.meta.guestOnly && state.authenticated) {
    return { name: 'admin' }
  }

  return true
})

export default router
