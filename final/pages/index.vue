<template>
  <v-app>
    <v-container class="text-center">
      <v-btn
        size="70"
        elevation="2"
        @click="$router.push('/documentView')"
        icon="mdi-plus"></v-btn>
    </v-container>
    <v-container>
      <v-row>
        <v-col cols="4" v-for="n in sampleDocs" :key="n">
          <v-card
            @click="$router.push('/documentView')"
            height="200"
            width="150"
            elevation="2"
            >{{ n }}</v-card
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
const theme = useTheme();
const sampleDocs = ['1', '2', '3'];
</script>
