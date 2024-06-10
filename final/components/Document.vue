<template>
  <DocumentToolbar
    v-model="title"
    @deleteDocument="deleteDocument"
    @saveChanges="saveChanges"
    @show-rhym-dialog="rhymDialog = !rhymDialog"
    @show-rhyme-scheme-window="showRhymeSchemeWindow = !showRhymeSchemeWindow"
    @showLinkDialog="showLinkDialog = true" />
  <v-progress-linear v-if="isBusy" color="secondary" indeterminate />
  <v-row>
    <v-window
      v-model="window"
      class="mx-auto my-15 pa-0"
      :show-arrows="showRhymeSchemeWindow ? 'hover' : false"
      @update:model-value="if (window === 1) getRhymeScheme();">
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
          max-width="450"
          width="auto"
          color="primary">
          <v-container class="mx-0">
            <template
              v-for="(word, outerIndex) in rhymeSchemeColorContent"
              :key="outerIndex">
              <template
                v-for="(syllable, innerIndex) in word.syllables"
                :key="innerIndex"
                ><span :class="`bg-${syllable.color}`">{{
                  syllable.syllable
                }}</span></template
              >
              <span>&ensp;</span>
            </template>
          </v-container>
        </v-card>
      </v-window-item>
    </v-window>
  </v-row>
  <RhymDialog
    v-model:showModel="rhymDialog"
    v-model:content="content"
    @appendWord="word => appendWord(word)" />
  <LinkDialog :documentId v-model="showLinkDialog" />
</template>

<script setup lang="ts">
import Axios from 'axios';
import { RhymeUtils, Word, Syllable } from '~/scripts/rhymeUtils';
import TokenService from '~/scripts/tokenService';

const modelValue = defineModel<string>({ required: false, default: '' });
const tokenService: Ref<TokenService> | undefined = inject('TOKEN');
const route = useRoute();
const router = useRouter();
const content = ref(modelValue.value);
const title = ref('');
const rhymDialog = ref(false);
let documentId: number;
const userId = computed(() => {
  return tokenService?.value.getGuid();
});
const isBusy = ref(false);
const window = ref(0);
const showRhymeSchemeWindow = ref(false);
const rhymeSchemeContent = ref('');
const rhymeSchemeColorContent = ref<Word[]>([]);
const utils = new RhymeUtils();
const showLinkDialog = ref(false);

try {
  let stringId = route.query.id as string;
  documentId = parseInt(stringId);
  console.log(documentId);
  if (documentId < 0) {
    const url = 'document/addDocument';
    const response = await Axios.post(url, {
      UserId: userId.value,
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
    await Axios.post(url, {
      UserId: userId.value,
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
    rhymeSchemeColorContent.value = await utils.runAlgorithm(
      response.data.pronunciation,
      response.data.poem
    );
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
