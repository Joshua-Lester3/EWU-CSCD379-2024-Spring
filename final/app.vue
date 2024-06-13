<template>
  <v-app>
    <v-app-bar
      class="elevation-0"
      :color="theme.global.name.value === 'dark' ? '' : 'secondary'">
      <template v-slot:prepend>
        <v-icon
          class="ml-5 mr-2"
          @click="$router.push('/')"
          :color="theme.global.name.value === 'dark' ? 'secondary' : ''"
          >mdi-book-open-blank-variant</v-icon
        >
        <v-app-bar-title class="cursor-pointer" @click="$router.push('/')"
          >Rhyme
        </v-app-bar-title></template
      >
      <v-btn @click="signInDialog = true">{{
        tokenService.isLoggedIn() ? tokenService.getUserName() : 'Log In'
      }}</v-btn>
      <v-app-bar-nav-icon
        icon="mdi-cog"
        class="mr-2"
        @click="settingsDialog = !settingsDialog" />

      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
    </v-app-bar>
    <v-navigation-drawer location="right" width="175" v-model="drawer">
      <v-list>
        <v-list-item @click="$router.push('/about')">
          <v-icon>mdi-information-slab-box-outline</v-icon> About
        </v-list-item>
        <v-list-item @click="$router.push('/')">
          <v-icon>mdi-home</v-icon> Home
        </v-list-item>
        <v-list-item @click="$router.push('/wordList')"
          ><v-icon> mdi-book-open-blank-variant-outline</v-icon> Rhyme
          Editor</v-list-item
        >
      </v-list>
    </v-navigation-drawer>
    <v-main>
      <NuxtPage />
    </v-main>
    <SettingsDialog v-model="settingsDialog" />
    <SignInDialog v-model="signInDialog" />
  </v-app>
</template>

<script setup lang="ts">
import nuxtStorage from 'nuxt-storage';
import { useTheme } from 'vuetify';
import TokenService from './scripts/tokenService';

const tokenService = ref(new TokenService());
const theme = useTheme();
const drawer = ref(false);
const settingsDialog = ref(false);
const signInDialog = ref<boolean>(!tokenService.value.isLoggedIn());
provide('TOKEN', tokenService);

onMounted(() => {
  var defaultTheme = nuxtStorage.localStorage.getData('theme');
  theme.global.name.value = defaultTheme ?? 'dark';
});
</script>
