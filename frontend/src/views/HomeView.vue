<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import Header from '../components/Header.vue'
import Footer from '../components/Footer.vue'

const items = ref([])
const dayOffset = ref(0) // 0 = today; allowed range -3 .. +3

function toIsoDate(date) {
  const y = date.getFullYear()
  const m = String(date.getMonth() + 1).padStart(2, '0')
  const d = String(date.getDate()).padStart(2, '0')
  return `${y}-${m}-${d}`
}

function addDays(base, days) {
  const next = new Date(base.getFullYear(), base.getMonth(), base.getDate())
  next.setDate(next.getDate() + days)
  return next
}

const today = new Date()
const selectedDate = computed(() => addDays(today, dayOffset.value))
const selectedIso = computed(() => toIsoDate(selectedDate.value))
const selectedLabel = computed(() => selectedDate.value.toLocaleDateString())

const canGoPrev = computed(() => dayOffset.value > -3)
const canGoNext = computed(() => dayOffset.value < 3)

async function loadItems() {
  const response = await fetch(`/api/fooditem?date=${selectedIso.value}`)
  items.value = await response.json()
}

function goPrev() {
  if (canGoPrev.value) dayOffset.value -= 1
}

function goNext() {
  if (canGoNext.value) dayOffset.value += 1
}

watch(selectedIso, loadItems)
onMounted(loadItems)
</script>

<template>
  <main>
    <Header />
    <section class="menu-items">
      <div class="menu-date-nav">
        <button
          type="button"
          class="menu-date-btn"
          :disabled="!canGoPrev"
          aria-label="Previous day"
          @click="goPrev"
        >
          ‹
        </button>
        <h1>Lunch Menu: {{ selectedLabel }}</h1>
        <button
          type="button"
          class="menu-date-btn"
          :disabled="!canGoNext"
          aria-label="Next day"
          @click="goNext"
        >
          ›
        </button>
      </div>
      <p v-if="items.length === 0" class="admin-hint">No dishes scheduled for this day.</p>
      <div v-for="item in items" :key="item.foodItemId" class="food-list">
        <div class="food-item-text">
          <h2>{{ item.name }}</h2>
          <p>{{ item.description }}</p>
        </div>
        <img v-if="item.imageUrl" :src="item.imageUrl" :alt="item.name" />
      </div>
    </section>
  </main>
  <Footer />
</template>
