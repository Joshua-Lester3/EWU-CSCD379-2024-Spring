<template>
  <NuxtLayout>
    <v-progress-linear v-if="isLoading" color="secondary" indeterminate />
    <v-card class="ma-10">
      <v-table>
        <thead>
          <tr>
            <th class="text-center">Word</th>
            <th class="text-center">Pronunciation</th>
            <th class="text-center">Edit</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(word, index) in words" :key="index">
            <td class="text-center">{{ word.word }}</td>
            <td class="text-center">{{ word.syllablesPronunciation }}</td>
            <td class="text-center">
              <v-btn>Holle</v-btn>
            </td>
          </tr>
        </tbody>
      </v-table>
      <v-pagination v-model="page" :length="4" rounded="circle"></v-pagination>
    </v-card>
  </NuxtLayout>
</template>

<script setup lang="ts">
import Axios from 'axios';

const isLoading = ref(true);
const countPerPage = ref(25);
const page = ref(1);
const words = ref<Array<WordDto>>([]);
const length = ref(0);

interface WordDto {
  word: string;
  phonemes: string[];
  syllablesPronunciation: string[];
  plainTextSyllables: string[];
}

onMounted(async () => {
  try {
    const url = `word/wordListPaginated?countPerPage=${
      countPerPage.value
    }&pageNumber=${page.value - 1}`;
    const response = await Axios.get(url);
    words.value = response.data.words;
    length.value = response.data.pages;
    isLoading.value = false;
  } catch (error) {
    console.error('Error deleting document information', error);
  }
});
</script>
