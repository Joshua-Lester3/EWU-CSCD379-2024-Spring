<template>
  <v-bottom-sheet v-model="showModel">
    <v-card height="200">
      <v-card-title>Let's get to rhymin', kid</v-card-title>
      <v-btn v-if="showWords" v-for="index in 10" :key="index" flat>{{
        index
      }}</v-btn>
      <v-btn-toggle v-model="algorithmType" color="red" rounded="0" group>
        <v-btn :value="AlgorithmType.PerfectRhyme"> Perfect Rhyme </v-btn>
        <v-btn :value="AlgorithmType.Nothing"> Nothing </v-btn>
      </v-btn-toggle>
      <v-btn @click="runAlgorithm">Run</v-btn>
    </v-card>
  </v-bottom-sheet>
</template>

<script setup lang="ts">
const showModel = defineModel<boolean>();
const showWords = ref(false);
const algorithmType = ref<AlgorithmType>(AlgorithmType.PerfectRhyme);

enum AlgorithmType {
  PerfectRhyme,
  Nothing,
}

function runAlgorithm() {}
</script>
