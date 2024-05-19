<template>
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
      <Document />
    </v-container>
    <!-- </v-card> -->
  </v-row>
</template>

<script setup lang="ts">
import Axios from 'axios';
const modelValue = defineModel<{ id: number }>();
const documentHeight = ref(0);
const documentWidth = ref(0);
const route = useRoute();
const content = ref('');
const title = ref('');

try {
  let id = route.query.id as number;
  console.log(id);
  debugger;
  if (id < 0) {
    const url = 'document/addDocument';
    const response = await Axios.post(url, {
      UserId: 1,
      Title: 'Untitled',
      Content: '',
    });
    title.value = response.data.title;
  } else {
    const url = `document/getDocumentData?documentId=${id}`;
    const response = await Axios.get(url);
    content.value = response.data.content;
  }
} catch (error) {
  console.error('Error fetching selected word:', error);
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
