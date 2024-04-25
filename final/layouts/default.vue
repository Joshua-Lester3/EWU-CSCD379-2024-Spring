<template>
  <v-app>
    <v-app-bar color="secondary" elevation="2">
      <template v-slot:prepend>
        <v-app-bar-title class="ml-6 cursor-pointer" @click="$router.push('/')">
          Notes
        </v-app-bar-title></template
      >
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
    </v-app-bar>
    <v-navigation-drawer location="right" width="150" v-model="drawer">
      <v-list>
        <v-list-item @click="$router.push('/')">
          <v-icon>mdi-home</v-icon> Home
        </v-list-item>
        <v-list-item @click="settingsDialog = !settingsDialog"
          ><v-icon> mdi-cog</v-icon> Settings</v-list-item
        >
      </v-list>
    </v-navigation-drawer>
    <SettingsDialog v-model="settingsDialog" />
    <v-main>
      <slot />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
const drawer = ref(false);
const settingsDialog = ref(false);
</script>
