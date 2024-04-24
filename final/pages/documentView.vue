<template>
  <v-row>
    <v-card
      elevation="5"
      tile
      :height="documentHeight"
      :width="documentWidth"
      color="primary"
      class="mx-auto my-15">
      <Document />
    </v-card>
  </v-row>
</template>

<script setup lang="ts">
import { useDisplay } from 'vuetify';

const modelValue = defineModel<{ id: number }>();
const display = useDisplay();
const documentHeight = ref(0);
const documentWidth = ref(0);

watch([display.xs, display.sm, display.md], () => {
  updateSize();
});

function updateSize() {
  if (display.xs.value) {
    documentHeight.value = 450;
    documentWidth.value = 330;
  } else if (display.sm.value) {
    documentHeight.value = 1000;
    documentWidth.value = 750;
  } else {
    documentHeight.value = 1250;
    documentWidth.value = 940;
  }
}

onMounted(() => {
  updateSize();
});
</script>
