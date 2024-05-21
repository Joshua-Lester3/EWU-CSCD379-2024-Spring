<template>
  <v-bottom-sheet v-model="showModel">
    <v-card min-height="300" max-height="600">
      <v-card-title>Let's get to rhymin', kid</v-card-title>
      <v-btn
        v-if="showWords"
        v-for="word in perfectRhymes"
        :key="word"
        flat
        @click="
          $emit('appendWord', word);
          showModel = false;
        "
        >{{ word }}</v-btn
      >
      <v-btn-toggle v-model="algorithmType" color="red" rounded="0" group>
        <v-btn @click="algorithmType = AlgorithmType.PerfectRhyme">
          Perfect Rhyme
        </v-btn>
        <v-btn @click="algorithmType = AlgorithmType.Nothing"> Nothing </v-btn>
      </v-btn-toggle>
      <v-btn @click="runAlgorithm">Run</v-btn>
    </v-card>
  </v-bottom-sheet>
</template>

<script setup lang="ts">
import { AlgorithmType } from '~/scripts/rhymeUtils';
import Axios from 'axios';
import { RhymeUtils } from '~/scripts/rhymeUtils';

const emit = defineEmits(['appendWord']);

const showModel = defineModel<boolean>('showModel');
const contentModel = defineModel<string>('content');
const showWords = ref(false);
const perfectRhymes = ref<List<string>>([]);
const algorithmType = ref<AlgorithmType>(AlgorithmType.PerfectRhyme);
const rhymeUtils: RhymeUtils = new RhymeUtils();

function runAlgorithm() {
  switch (algorithmType.value) {
    case AlgorithmType.PerfectRhyme:
      getPerfectRhymeList();
      showWords.value = true;
      break;
    case AlgorithmType.Nothing:
      console.log('hello');
      break;
  }
}

async function getPerfectRhymeList() {
  try {
    const wordToRhyme = rhymeUtils.getWordToRhyme(contentModel.value);
    const url = `word/perfectRhyme?word=${wordToRhyme}`;
    const response = await Axios.get(url);
    perfectRhymes.value = response.data;
  } catch (error) {
    console.error('Error posting document information', error);
  }
}
</script>
