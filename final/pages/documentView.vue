<template>
  <v-row>
    <v-card
      elevation="5"
      tile
      min-height="450"
      height="auto"
      min-width="350"
      width="auto"
      color="primary"
      class="mx-auto my-15">
      <Document />
    </v-card>
  </v-row>
</template>

<script setup lang="ts">
const modelValue = defineModel<{ id: number }>();
const documentHeight = ref(0);
const documentWidth = ref(0);

// function updateSize() {
//   if (true) {
//     documentHeight.value = 450;
//     documentWidth.value = 330;
//   } else if (display.sm.value) {
//     documentHeight.value = 1000;
//     documentWidth.value = 750;
//   } else {
//     documentHeight.value = 1250;
//     documentWidth.value = 750s;
//   }
// }
</script>
