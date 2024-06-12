<template>
  <v-alert v-if="error" type="error"> Could not find other note's URL </v-alert>
  <v-app>
    <v-toolbar height="50" color="background">
      <v-row>
        <v-col cols="7"> </v-col>
        <v-col cols="5">
          <v-text-field
            label="Other note's URL"
            density="compact"
            variant="solo"
            class="mt-5 mr-1"
            bg-color="secondary"
            type="error"
            flat
            v-model="otherDocumentUrl"
            append-inner-icon="mdi-vector-line"
            @click:append-inner="openOtherDocument" />
        </v-col>
      </v-row>
    </v-toolbar>
    <v-container class="align-center d-flex justify-center">
      <v-card
        height="200"
        width="150"
        class="align-center d-flex justify-center mt-2 mb-6"
        @click="$router.push('/documentView?id=-1')">
        <v-icon class="mb-5">mdi-plus</v-icon>
      </v-card>
    </v-container>
    <v-container>
      <v-row>
        <v-col
          class="my-4"
          align="center"
          cols="12"
          sm="12"
          md="6"
          lg="4"
          v-for="document in documents"
          :key="document.documentId">
          <v-card
            @click="$router.push(`/documentView?id=${document.documentId}`)"
            elevation="2"
            :height="300"
            class="align-center d-flex justify-center"
            ><p class="mb-5">
              {{ document.title }}
            </p></v-card
          >
        </v-col>
      </v-row>
    </v-container>
  </v-app>
</template>

<script setup lang="ts">
// get documents from database? from last open time
// loop through them in the v-col element v-for
//
// have each v-card link to documentView, connecting the id of
// each document to the documentView (somehow??), so the view knows
// document to open
import { useDisplay } from 'vuetify';
import Axios from 'axios';
import TokenService from '~/scripts/tokenService';

const display = useDisplay();
const router = useRouter();
const documents = ref<Document[]>();
const tokenService: Ref<TokenService> | undefined = inject('TOKEN');
const otherDocumentUrl = ref('');
const error = ref(false);

interface Document {
  documentId: number;
  title: string;
}

const getHeight = computed(() => {
  if (display.sm) {
    return 300;
  }
  if (display.md) {
    return 200;
  }
  if (display.lg) {
    return 300;
  }
});

onMounted(async () => {
  const guid = tokenService?.value.getGuid();
  if (guid !== '') {
    console.log(guid);
    try {
      const url = `document/getDocumentList?userId=${guid}`;
      const response = await Axios.get(url);
      documents.value = response.data;
    } catch (error) {
      console.error('Error fetching selected word:', error);
    }
  }
});

function openOtherDocument() {
  const array = otherDocumentUrl.value.split('=');
  const documentId = parseInt(array[array.length - 1]);
  if (Number.isNaN(documentId)) {
    error.value = true;
  } else {
    error.value = false;
    router.push(`/documentView?id=${documentId}`);
  }
}
</script>
