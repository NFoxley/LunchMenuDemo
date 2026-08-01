<!-- <script setup>
import { ref, onMounted } from 'vue'

const message = ref('Loading...')

onMounted(async () => {
  const response = await fetch('/api/fooditem')
  const data = await response.json()
  message.value = data
})
</script>

<template>
  <h1>{{ message.name }}</h1>
  <p>{{ message.description }}</p>
</template> -->
<script setup>
import { ref, onMounted } from 'vue'
// import style from './style.css'
import Header from './components/Header.vue'
import Footer from './components/Footer.vue'

const items = ref([])

onMounted(async () => {
  const response = await fetch('/api/fooditem')
  items.value = await response.json()  // expect an array
})
</script>

<template>
  <main>
    <Header />
    <div v-for="item in items" :key="item.foodItemId" class="food-list">
      <h1>{{ item.name }}</h1>
      <h2>{{ item.description }}</h2>
      <img :src="item.imageUrl" />
    </div>    
  </main>
  <Footer />
</template>