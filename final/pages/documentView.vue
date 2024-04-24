<template>
  <v-row>
    <v-card
      elevation="5"
      tile
      height="750"
      width="500"
      color="primary"
      class="mx-auto my-15">
      <Document />
    </v-card>
  </v-row>
</template>

<script setup lang="ts">
const modelValue = defineModel<{ id: number }>();
</script>
