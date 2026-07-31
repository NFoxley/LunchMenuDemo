<script setup>
import { ref, onMounted } from 'vue'

const message = ref('Loading...')

onMounted(async () => {
  const response = await fetch('/api/menu')
  const data = await response.json()
  message.value = data.message
})
</script>

<template>
  <h1>{{ message }}</h1>
</template>
