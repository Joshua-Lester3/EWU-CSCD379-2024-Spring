<template>
  <v-app>
    <v-container class="text-center">
      <v-btn
        size="70"
        elevation="2"
        @click="$router.push('/documentView?id=-1')"
        icon="mdi-plus"></v-btn>
    </v-container>
    <v-container>
      <v-row>
        <v-col
          class="my-4"
          align="center"
          cols="4"
          v-for="document in documents"
          :key="document.documentId">
          <v-card
            @click="$router.push('/documentView')"
            height="200"
            width="150"
            elevation="2"
            >{{ document.title }}</v-card
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
import { useTheme } from 'vuetify';
import Axios from 'axios';

const theme = useTheme();
const documents = ref<Document[]>();

interface Document {
  documentId: number;
  title: string;
}

try {
  const url = `document/getDocumentList?userId=1`;
  const response = await Axios.get(url);
  documents.value = response.data;
} catch (error) {
  console.error('Error fetching selected word:', error);
}
</script>
