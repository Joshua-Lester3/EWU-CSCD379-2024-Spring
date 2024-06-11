<template>
  <v-dialog persistent v-model="modelValue" width="400">
    <v-card>
      <v-sheet color="error">
        <v-card-title>This document is not shared </v-card-title>
      </v-sheet>
      <v-card-text>
        <p>Please contact the owner of the document to access it.</p>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
const modelValue = defineModel<boolean>();
</script>
