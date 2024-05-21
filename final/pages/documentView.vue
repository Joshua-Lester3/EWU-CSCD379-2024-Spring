<template>
  <v-toolbar class="mt-5" height="50">
    <v-icon
      class="ml-5 mr-2"
      @click="$router.push('/')"
      :color="theme.global.name.value === 'dark' ? 'secondary' : ''"
      >mdi-book-open-blank-variant</v-icon
    >
    <v-btn density="compact"
      >File
      <v-menu activator="parent">
        <v-list>
          <v-list-item density="compact">
            <v-list-item-title @click="saveChanges">Save</v-list-item-title>
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
        </v-list>
      </v-menu>
    </v-btn>
  </v-toolbar>
  <v-row>
    <!-- <v-card
      elevation="5"
      tile
      min-height="450"
      height="auto"
      min-width="350"
      width="auto"
      color="primary"
      class="mx-auto my-15"> -->
    <v-container class="ma-10">
      <v-textarea
        v-model="content"
        :model-value="content"
        placeholder="Type something :)"
        variant="solo"
        auto-grow
        tile
        flat
        density="comfortable"
        elevation="0" />
    </v-container>
    <!-- </v-card> -->
  </v-row>
  <RhymDialog
    v-model:showModel="rhymDialog"
    v-model:content="content"
    @appendWord="word => appendWord(word)" />
</template>

<script setup lang="ts">
import Axios from 'axios';
import { useTheme } from 'vuetify';

const theme = useTheme();
const modelValue = defineModel<{ id: number }>();
const documentHeight = ref(0);
const documentWidth = ref(0);
const route = useRoute();
const content = ref('');
const title = ref('');
const rhymDialog = ref(false);
let documentId;
const userId = 1;

try {
  documentId = route.query.id as number;
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
