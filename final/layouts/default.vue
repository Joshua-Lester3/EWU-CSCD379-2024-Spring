<template>
  <v-app>
    <v-app-bar>
      <v-app-bar-title class="cursor-pointer" @click="$router.push('/')">
        Notes
      </v-app-bar-title>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
    </v-app-bar>
    <v-navigation-drawer location="right" v-model="drawer">
      <v-list>
        <v-list-item @click="$router.push('/')"> Home </v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-main>
      <slot />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
const drawer = ref(false);
</script>
