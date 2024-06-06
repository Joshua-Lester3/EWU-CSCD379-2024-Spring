<template>
  <v-dialog v-model="modelValue" @update:model-value="added = false">
    <v-card v-if="!added" class="ma-auto" min-width="500" min-height="500">
      <v-sheet :color="notAdded ? 'error' : 'secondary'">
        <v-row>
          <v-col cols="9">
            <v-text-field
              class="largeWord ml-5"
              variant="plain"
              v-model="word"
              :disabled="props.word?.trim() === ''"></v-text-field>
          </v-col>
          <v-col>
            <v-btn class="mt-6" @click="saveRhyme">Save</v-btn>
          </v-col>
        </v-row>
      </v-sheet>
      <v-sheet v-if="notAdded" color="error">
        <v-card-text
          >Word was not added. Check formatting and dictionary for duplicate
          entry.</v-card-text
        >
      </v-sheet>
      <v-progress-linear v-if="isLoading" color="secondary" indeterminate />

      <v-row>
        <v-col>
          <v-responsive max-width="125" class="mt-4 ml-4"
            ><p class="mb-2">Pronunciation (syllables):</p>
            <v-text-field
              v-for="(syllable, index) in syllablesPronunciation"
              :modelValue="syllable"></v-text-field>
            <v-text-field v-model="pronunciationAdded"></v-text-field>
            <v-btn
              class="ml-8 mb-4"
              icon="mdi-plus"
              color="secondary"
              @click="appendPronunciationValue()"></v-btn
          ></v-responsive>
        </v-col>
        <v-col>
          <v-responsive max-width="125" class="mt-4 ml-4">
            <p class="mb-2">Plain text (syllables):</p>
            <v-text-field
              v-for="(syllable, index) in plainTextSyllables"
              :modelValue="syllable"></v-text-field>
            <v-text-field v-model="plainTextAdded"></v-text-field>
            <v-btn
              class="ml-8 mb-4"
              icon="mdi-plus"
              color="secondary"
              @click="appendPlainTextValue()"></v-btn
          ></v-responsive>
        </v-col>
      </v-row>
    </v-card>
    <v-card v-else>
      <v-sheet color="success">
        <v-card-title>Word successfully added!</v-card-title>
      </v-sheet>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import Axios from 'axios';

const props = defineProps<{
  word?: string;
  syllablesPronunciation?: string[];
  plainTextSyllables?: string[];
}>();
const modelValue = defineModel<boolean>();
const word = ref('');
const syllablesPronunciation = ref<Array<string>>([]);
const pronunciationAdded = ref('');
const plainTextSyllables = ref<Array<string>>([]);
const plainTextAdded = ref('');
const isLoading = ref(false);
const notAdded = ref(false);
const added = ref(false);

onMounted(() => {
  if (props.word) {
    word.value = props.word;
  }
  if (props.syllablesPronunciation) {
    syllablesPronunciation.value = props.syllablesPronunciation;
  }
  if (props.plainTextSyllables) {
    plainTextSyllables.value = props.plainTextSyllables;
  }
});

function appendPronunciationValue() {
  if (pronunciationAdded.value.trim()) {
    syllablesPronunciation.value.push(pronunciationAdded.value);
    pronunciationAdded.value = '';
  }
}

function appendPlainTextValue() {
  if (plainTextAdded.value.trim()) {
    plainTextSyllables.value.push(plainTextAdded.value);
    plainTextAdded.value = '';
  }
}

async function saveRhyme() {
  try {
    if (notAdded.value) {
      notAdded.value = false;
    }
    if (pronunciationAdded.value.trim() !== '') {
      appendPronunciationValue();
    }
    if (plainTextAdded.value.trim() !== '') {
      appendPlainTextValue();
    }
    isLoading.value = true;
    const url = 'word/addWord';
    debugger;
    const response = await Axios.post(url, {
      word: word.value,
      syllablesPronunciation: syllablesPronunciation.value,
      plainTextSyllables: plainTextSyllables.value,
    });
    isLoading.value = false;
    if (response.data) {
      added.value = true;
    } else {
      notAdded.value = true;
    }
  } catch (error) {
    console.error('Error saving word information', error);
    isLoading.value = false;
  }
}
</script>

<style scoped>
.largeWord :deep(.v-field__input) {
  font-size: 1.6em;
}
</style>
