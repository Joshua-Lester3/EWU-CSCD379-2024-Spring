<template>
  <Document />
  <v-container>
    <v-text-field
      class="mt-5"
      label="Bar"
      @keyup.stop
      @keyup.enter="sendBar"
      v-model="bar"
      @click:append-inner="sendBar"
      append-inner-icon="mdi-send" />
  </v-container>
</template>

<script setup lang="ts">
import SignalRService from '~/scripts/signalRService';

const bar = ref<string>('');
const signalRService = new SignalRService();

function sendBar() {
  signalRService.sendBar(bar.value);
  bar.value = '';
}
</script>
