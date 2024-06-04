<template>
  <v-app>
    <v-app-bar :color="theme.global.name.value === 'dark' ? '' : 'secondary'">
      <template v-slot:prepend>
        <v-icon
          class="ml-5 mr-2"
          @click="$router.push('/')"
          :color="theme.global.name.value === 'dark' ? 'secondary' : ''"
          >mdi-book-open-blank-variant</v-icon
        >
        <v-app-bar-title class="cursor-pointer" @click="$router.push('/')"
          >Rhym
        </v-app-bar-title></template
      >
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
    </v-app-bar>
    <v-navigation-drawer location="right" width="175" v-model="drawer">
      <v-list>
        <v-list-item @click="$router.push('/documentHub')">
          <v-icon>mdi-home</v-icon> Hub
        </v-list-item>
        <v-list-item @click="settingsDialog = !settingsDialog"
          ><v-icon> mdi-cog</v-icon> Settings</v-list-item
        >
        <v-list-item @click="$router.push('/wordList')"
          ><v-icon> mdi-book-open-blank-variant-outline</v-icon>
          Editor</v-list-item
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
import { useTheme } from 'vuetify';

const theme = useTheme();
const drawer = ref(false);
const settingsDialog = ref(false);
</script>
