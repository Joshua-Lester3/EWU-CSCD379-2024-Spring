<template>
  <v-container class="justify-center">
    <v-btn @click="$router.push('/document')" icon="mdi-plus"></v-btn>
  </v-container>
  <v-container class="bg-surface-variant">
    <v-row>
      <v-col cols="4" v-for="n in 3" :key="n">
        <v-card height="200" width="150">{{ n }}</v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
