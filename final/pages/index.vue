<template>
  <v-container class="text-center">
    <v-btn
      size="70"
      @click="$router.push('/documentView')"
      icon="mdi-plus"></v-btn>
  </v-container>
  <v-container class="bg-surface-variant">
    <v-row>
      <v-col cols="4" v-for="n in sampleDocs" :key="n">
        <v-card
          @click="$router.push('/documentView')"
          height="200"
          width="150"
          >{{ n }}</v-card
        >
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
// get documents from database? from last open time
// loop through them in the v-col element v-for
//
// have each v-card link to documentView, connecting the id of
// each document to the documentView (somehow??), so the view knows
// document to open

const sampleDocs = ['1', '2', '3'];
</script>
