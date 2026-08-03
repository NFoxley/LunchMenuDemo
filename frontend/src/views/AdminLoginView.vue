<script setup>
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import Header from '../components/Header.vue'
import Footer from '../components/Footer.vue'
import { useAuth } from '../composables/useAuth'

const username = ref('')
const password = ref('')
const error = ref('')
const submitting = ref(false)

const route = useRoute()
const router = useRouter()
const { login } = useAuth()

async function onSubmit() {
  error.value = ''
  submitting.value = true

  try {
    await login(username.value.trim(), password.value)
    const redirect = typeof route.query.redirect === 'string' ? route.query.redirect : '/admin'
    await router.push(redirect)
  } catch (err) {
    error.value = err.message || 'Login failed.'
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <main>
    <Header />
    <section class="admin-panel">
      <h1>Food Admin Login</h1>
      <p class="admin-hint">
        Demo accounts: <code>foodadmin1</code> / <code>admin123</code> (can edit),
        <code>foodstaff1</code> / <code>staff123</code> (view only).
      </p>

      <form class="admin-form" @submit.prevent="onSubmit">
        <label>
          Username
          <input v-model="username" type="text" autocomplete="username" required id="username"/>
        </label>
        <label>
          Password
          <input v-model="password" type="password" autocomplete="current-password" required id="password"/>
        </label>
        <p v-if="error" class="form-error">{{ error }}</p>
        <button type="submit" :disabled="submitting">
          {{ submitting ? 'Signing in…' : 'Sign in' }}
        </button>
      </form>
    </section>
  </main>
  <Footer />
</template>
