<template>
  <v-toolbar class="mt-5" height="50">
    <v-icon
      class="ml-5 mr-2"
      @click="$router.push('/')"
      :color="theme.global.name.value === 'dark' ? 'secondary' : ''"
      >mdi-book-open-blank-variant</v-icon
    >
    <v-row>
      <v-col cols="6">
        <v-text-field
          v-model="title"
          density="compact"
          variant="solo"
          class="mt-5"
          bg-color="secondary"
          flat />
      </v-col>
    </v-row>
  </v-toolbar>
  <v-toolbar color="primary" height="30">
    <v-btn density="compact"
      >File
      <v-menu activator="parent">
        <v-list>
          <v-list-item density="compact">
            <v-list-item-title @click="saveChanges">Save</v-list-item-title>
          </v-list-item>
          <v-list-item density="compact">
            <v-list-item-title @click="deleteDocument()"
              >Delete</v-list-item-title
            >
          </v-list-item>
        </v-list>
      </v-menu>
    </v-btn>
    <v-btn density="compact"
      >Rhym
      <v-menu activator="parent">
        <v-list>
          <v-list-item density="compact">
            <v-list-item-title @click="rhymDialog = !rhymDialog"
              >Rhyme, kid</v-list-item-title
            >
          </v-list-item>
          <v-list-item density="compact">
            <v-list-item-title
              @click="showRhymeSchemeWindow = !showRhymeSchemeWindow"
              >Show/hide rhyme scheme, kid</v-list-item-title
            >
          </v-list-item>
        </v-list>
      </v-menu>
    </v-btn>
  </v-toolbar>
  <v-progress-linear v-if="isBusy" color="secondary" indeterminate />
  <v-row>
    <v-window
      v-model="window"
      class="mx-auto my-15 pa-0"
      :show-arrows="showRhymeSchemeWindow ? 'hover' : false"
      @update:model-value="window === 1 ? getRhymeScheme() : 'hello'">
      <v-window-item>
        <v-card
          elevation="5"
          tile
          min-height="450"
          height="auto"
          min-width="500"
          width="auto"
          color="primary">
          <v-container class="mx-0">
            <v-textarea
              v-model="content"
              placeholder="Type something :)"
              variant="solo"
              tile
              flat
              density="comfortable"
              elevation="0"
              no-resize
              auto-grow />
          </v-container>
        </v-card>
      </v-window-item>
      <v-window-item>
        <v-card
          elevation="5"
          tile
          min-height="450"
          height="auto"
          min-width="500"
          width="auto"
          color="primary">
          <v-container class="mx-0">
            <v-textarea
              v-model="rhymeSchemeContent"
              variant="solo"
              tile
              flat
              density="comfortable"
              elevation="0"
              no-resize
              auto-grow
              disabled />
          </v-container>
        </v-card>
      </v-window-item>
    </v-window>
  </v-row>
  <RhymDialog
    v-model:showModel="rhymDialog"
    v-model:content="content"
    @appendWord="word => appendWord(word)" />
</template>

<script setup lang="ts">
import Axios from 'axios';
import { RhymeUtils } from '~/scripts/rhymeUtils';
import { useTheme } from 'vuetify';

const theme = useTheme();
const modelValue = defineModel<{ id: number }>();
const documentHeight = ref(0);
const documentWidth = ref(0);
const route = useRoute();
const router = useRouter();
const content = ref('');
const title = ref('');
const rhymDialog = ref(false);
let documentId: number;
const userId = 1;
const isBusy = ref(false);
const window = ref(0);
const showRhymeSchemeWindow = ref(false);
const rhymeSchemeContent = ref('');

const textAreaHeight = computed(() => {
  // count new lines
});

try {
  let stringId = route.query.id as string;
  documentId = parseInt(stringId);
  console.log(documentId);
  if (documentId < 0) {
    const url = 'document/addDocument';
    const response = await Axios.post(url, {
      UserId: userId,
      DocumentId: documentId,
      Title: 'Untitled',
      Content: '',
    });
    title.value = response.data.title;
    documentId = response.data.documentId;
  } else {
    const url = `document/getDocumentData?documentId=${documentId}`;
    const response = await Axios.get(url);
    title.value = response.data.title;
    content.value = response.data.content;
  }
} catch (error) {
  console.error('Error fetching selected word:', error);
}
async function saveChanges() {
  try {
    const url = 'document/addDocument';
    const response = await Axios.post(url, {
      UserId: userId,
      DocumentId: documentId,
      Title: title.value,
      Content: content.value,
    });
  } catch (error) {
    console.error('Error posting document information', error);
  }
}

function appendWord(word: string) {
  if (!content.value.endsWith(' ')) {
    word = ' ' + word;
  }
  content.value = content.value.concat(word.toLowerCase());
}

async function deleteDocument() {
  try {
    const url = `document/deleteDocument?documentId=${documentId}`;
    const response = await Axios.post(url, {});
    isBusy.value = true;
    setTimeout(() => {
      isBusy.value = false;
      router.push('/');
    }, 1000);
  } catch (error) {
    console.error('Error deleting document information', error);
  }
}

async function getRhymeScheme() {
  try {
    isBusy.value = true;
    const url = 'word/poemPronunciation';
    const response = await Axios.post(url, {
      content: content.value,
    });
    rhymeSchemeContent.value = response.data;
    isBusy.value = false;
  } catch (error) {
    console.error('Error getting rhyme scheme', error);
  }
}

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
