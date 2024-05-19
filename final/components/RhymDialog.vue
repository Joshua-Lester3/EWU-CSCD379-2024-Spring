<template>
  <v-bottom-sheet v-model="showModel">
    <v-card title="Let's get to rhymin', kid"> </v-card>
  </v-bottom-sheet>
</template>

<script setup lang="ts">
const showModel = defineModel<boolean>();
</script>
