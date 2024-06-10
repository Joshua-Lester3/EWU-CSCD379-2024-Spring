<template>
  <v-progress-linear v-if="isLoading" color="secondary" indeterminate />
  <v-card class="ma-10">
    <v-row>
      <v-col cols="5">
        <v-select
          class="ma-5"
          width="50px"
          :items="[10, 25, 50, 100]"
          v-model="countPerPage"
          label="Words per page"
          @update:modelValue="setWords" />
      </v-col>
      <v-col class="ma-7" cols="3">
        <v-btn color="secondary" @click="showAddRhyme = true">Add Rhyme</v-btn>
      </v-col>
    </v-row>
    <v-table>
      <thead>
        <tr>
          <th class="text-center text-h6">Word</th>
          <th class="text-center text-h6">Pronunciation</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(word, index) in words" :key="index">
          <td class="text-center">{{ word?.word }}</td>
          <td class="text-center">{{ word?.syllablesPronunciation }}</td>
        </tr>
      </tbody>
    </v-table>
    <v-pagination
      v-model="page"
      :length
      rounded="circle"
      @update:modelValue="setWords" />
  </v-card>
  <AddRhyme v-model="showAddRhyme" />
</template>

<script setup lang="ts">
import Axios from 'axios';

const isLoading = ref(true);
const countPerPage = ref(25);
const page = ref(1);
const words = ref<Array<WordDto>>([]);
const length = ref(0);
const showAddRhyme = ref(false);

interface WordDto {
  word: string;
  phonemes: string[];
  syllablesPronunciation: string[];
  plainTextSyllables: string[];
}

onMounted(() => {
  setWords();
});

async function setWords() {
  try {
    const url = `word/wordListPaginated?countPerPage=${
      countPerPage.value
    }&pageNumber=${page.value - 1}`;
    const response = await Axios.get(url);
    words.value = response.data.words;
    length.value = response.data.pages;
    isLoading.value = false;
  } catch (error) {
    console.error('Error getting word information', error);
  }
}
</script>
