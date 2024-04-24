<template>
  <v-container>
    <v-row v-for="index in 10" :key="index">
      <v-col cols="">
        <v-card height="200" width="150">{{ index }}</v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
