<template>
  <v-app>
    <v-container>
      <v-card min-height="300">
        <v-card-title> Home page </v-card-title>
        <v-btn
          class="ma-5"
          color="secondary"
          @click="$router.push('/documentHub')"
          >hub</v-btn
        >
      </v-card>
    </v-container>
  </v-app>
</template>
