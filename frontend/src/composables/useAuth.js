import { reactive, computed } from 'vue'

const state = reactive({
  loaded: false,
  authenticated: false,
  username: null,
  role: null,
  canEditMenu: false,
})

async function fetchMe() {
  const response = await fetch('/api/auth/me', { credentials: 'include' })
  const data = await response.json()

  state.loaded = true
  state.authenticated = !!data.authenticated
  state.username = data.username ?? null
  state.role = data.role ?? null
  state.canEditMenu = !!data.canEditMenu

  return data
}

async function login(username, password) {
  const response = await fetch('/api/auth/login', {
    method: 'POST',
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ username, password }),
  })

  const data = await response.json().catch(() => ({}))

  if (!response.ok) {
    throw new Error(data.message || 'Login failed.')
  }

  state.loaded = true
  state.authenticated = true
  state.username = data.username
  state.role = data.role
  state.canEditMenu = !!data.canEditMenu

  return data
}

async function logout() {
  await fetch('/api/auth/logout', {
    method: 'POST',
    credentials: 'include',
  })

  state.authenticated = false
  state.username = null
  state.role = null
  state.canEditMenu = false
}

export function useAuth() {
  return {
    state,
    isAuthenticated: computed(() => state.authenticated),
    canEditMenu: computed(() => state.canEditMenu),
    fetchMe,
    login,
    logout,
  }
}
