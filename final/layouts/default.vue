<template>
  <v-app>
    <v-app-bar>
      <v-app-bar-title> Notes </v-app-bar-title>
      <v-btn v-slot:append @click="$router.push('/document')">Doc</v-btn>
    </v-app-bar>
    <v-main>
      <slot />
    </v-main>
  </v-app>
</template>

<script setup lang="ts"></script>
