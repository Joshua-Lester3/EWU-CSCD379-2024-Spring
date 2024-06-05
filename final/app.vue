<template>
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>

<script setup lang="ts">
import { useTheme } from 'vuetify';
import nuxtStorage from 'nuxt-storage';

const theme = useTheme();

onMounted(() => {
  var defaultTheme = nuxtStorage.localStorage.getData('theme');
  theme.global.name.value = defaultTheme ?? 'dark';
});
</script>
