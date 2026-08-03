<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import Header from '../components/Header.vue'
import Footer from '../components/Footer.vue'
import { useAuth } from '../composables/useAuth'

const items = ref([])
const name = ref('')
const description = ref('')
const menuDate = ref(new Date().toISOString().slice(0, 10))
const imageFile = ref(null)
const imageInputKey = ref(0)
const formError = ref('')
const formSuccess = ref('')
const submitting = ref(false)

const router = useRouter()
const { state, canEditMenu, logout } = useAuth()

function formatDate(isoDate) {
  // isoDate is "YYYY-MM-DD"
  const [y, m, d] = isoDate.split('-').map(Number)
  return new Date(y, m - 1, d).toLocaleDateString()
}

/**
 * API returns dishes sorted by name (each with many dates).
 * Admin wants a date-first view — reshape on the client:
 * group servings by date, then sort those groups chronologically.
 */
const menuByDate = computed(() => {
  const byDate = new Map()

  for (const item of items.value) {
    for (const date of item.menuDates ?? []) {
      if (!byDate.has(date)) {
        byDate.set(date, [])
      }
      byDate.get(date).push(item)
    }
  }

  return [...byDate.entries()]
    .sort(([a], [b]) => a.localeCompare(b))
    .map(([date, dishes]) => ({ date, dishes }))
})

function onImageChange(event) {
  const file = event.target.files?.[0] ?? null
  imageFile.value = file
}

async function loadItems() {
  const response = await fetch('/api/fooditem')
  items.value = await response.json()
}

async function onAddFood() {
  formError.value = ''
  formSuccess.value = ''
  submitting.value = true

  try {
    const formData = new FormData()
    formData.append('name', name.value)
    formData.append('description', description.value || '')
    formData.append('menuDate', menuDate.value)
    if (imageFile.value) {
      formData.append('image', imageFile.value)
    }

    const response = await fetch('/api/fooditem', {
      method: 'POST',
      credentials: 'include',
      body: formData,
    })

    const data = await response.json().catch(() => ({}))

    if (response.status === 401) {
      await router.push({ name: 'admin-login' })
      return
    }

    if (response.status === 403) {
      formError.value = 'You do not have permission to edit the menu.'
      return
    }

    if (!response.ok) {
      formError.value = data.message || 'Could not add food item.'
      return
    }

    formSuccess.value = `Added “${data.name}”.`
    name.value = ''
    description.value = ''
    menuDate.value = new Date().toISOString().slice(0, 10)
    imageFile.value = null
    imageInputKey.value += 1
    await loadItems()
  } finally {
    submitting.value = false
  }
}

async function onLogout() {
  await logout()
  await router.push({ name: 'admin-login' })
}

onMounted(loadItems)
</script>

<template>
  <main>
    <Header />
    <section class="admin-panel">
      <div class="admin-header-row">
        <div>
          <h1>Food Admin</h1>
          <p>
            Signed in as <strong>{{ state.username }}</strong>
            ({{ state.role }})
          </p>
        </div>
        <button type="button" class="secondary-btn" @click="onLogout">Sign out</button>
      </div>

      <div v-if="canEditMenu" class="admin-card">
        <h2>Add menu item</h2>
        <form class="admin-form" @submit.prevent="onAddFood">
          <label>
            Name
            <input v-model="name" type="text" required placeholder="Baked Alaska"/>
          </label>
          <label>
            Description
            <textarea v-model="description" rows="3" placeholder="Sounds fancy, tastes great."/>
          </label>
          <label>
            Menu date
            <input v-model="menuDate" type="date" required />
          </label>
          <label>
            Image
            <input
              :key="imageInputKey"
              type="file"
              accept=".jpg,.jpeg,.png,.webp,image/jpeg,image/png,image/webp"
              @change="onImageChange"
            />
          </label>
          <p class="admin-hint">Accepted formats: .jpg, .png, .webp</p>
          <p v-if="formError" class="form-error">{{ formError }}</p>
          <p v-if="formSuccess" class="form-success">{{ formSuccess }}</p>
          <button type="submit" :disabled="submitting">
            {{ submitting ? 'Saving…' : 'Add item' }}
          </button>
        </form>
      </div>

      <div v-else class="admin-card admin-notice">
        <h2>View only</h2>
        <p>
          Your account can view the menu admin area but cannot add or edit items.
          Ask a Food Admin for edit access.
        </p>
      </div>

      <div class="admin-card">
        <h2>Current menu by date</h2>
        <p v-if="menuByDate.length === 0" class="admin-hint">No menu dates yet.</p>
        <div v-for="group in menuByDate" :key="group.date" class="admin-date-group">
          <h3>{{ formatDate(group.date) }}</h3>
          <ul class="admin-menu-list">
            <li v-for="item in group.dishes" :key="`${group.date}-${item.foodItemId}`" class="admin-menu-item">
              <strong>{{ item.name }}</strong>
              <span v-if="item.description"> — {{ item.description }}</span>
            </li>
          </ul>
        </div>
      </div>
    </section>
  </main>
  <Footer />
</template>
