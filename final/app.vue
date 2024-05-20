<template>
  <NuxtPage />
</template>

<script setup lang="ts">
import { useTheme } from 'vuetify';

const theme = useTheme();

onMounted(() => {
  var defaultTheme = nuxtStorage.localStorage.getData('theme');
  theme.global.name.value = defaultTheme ?? 'dark';
});
</script>
