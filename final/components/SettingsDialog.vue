<template>
  <v-dialog v-model="modelValue">
    <v-card class="ma-auto" min-width="400">
      <v-sheet color="secondary">
        <v-card-title>Settings</v-card-title>
      </v-sheet>
      <v-card-text>
        <v-divider class="my-4" />
        Light/Dark Mode:
        <v-btn
          color="secondary"
          @click="toggleTheme"
          icon="mdi-theme-light-dark" />
        <v-divider class="my-4" />
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import nuxtStorage from 'nuxt-storage';
import { useTheme } from 'vuetify';

const modelValue = defineModel<boolean>();
const theme = useTheme();

onMounted(() => {
  var defaultTheme = nuxtStorage.localStorage.getData('theme');
  theme.global.name.value = defaultTheme ?? 'dark';
});

function toggleTheme() {
  if (theme.global.name.value === 'dark') {
    theme.global.name.value = 'light';
  } else {
    theme.global.name.value = 'dark';
  }
  nuxtStorage.localStorage.setData('theme', theme.global.name.value);
}
</script>
