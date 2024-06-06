<template>
  <v-toolbar height="50">
    <v-row>
      <v-col cols="6">
        <v-text-field
          v-model="modelValue"
          density="compact"
          variant="solo"
          class="mt-5 ml-1"
          bg-color="secondary"
          flat />
      </v-col>
    </v-row>
  </v-toolbar>
  <v-toolbar color="primary" height="30">
    <v-btn density="compact"
      >File
      <v-menu activator="parent">
        <v-list>
          <v-list-item density="compact">
            <v-list-item-title @click="$emit('saveChanges')"
              >Save</v-list-item-title
            >
          </v-list-item>
          <v-list-item density="compact">
            <v-list-item-title @click="$emit('deleteDocument')"
              >Delete</v-list-item-title
            >
          </v-list-item>
        </v-list>
      </v-menu>
    </v-btn>
    <v-btn density="compact"
      >Rhym
      <v-menu activator="parent">
        <v-list>
          <v-list-item density="compact">
            <v-list-item-title @click="$emit('showRhymDialog')"
              >Rhyme, kid</v-list-item-title
            >
          </v-list-item>
          <v-list-item density="compact">
            <v-list-item-title @click="$emit('showRhymeSchemeWindow')"
              >Show/hide rhyme scheme, kid</v-list-item-title
            >
          </v-list-item>
        </v-list>
      </v-menu>
    </v-btn>
  </v-toolbar>
</template>

<script setup lang="ts">
const modelValue = defineModel<string>();
defineEmits([
  'deleteDocument',
  'saveChanges',
  'showRhymeSchemeWindow',
  'showRhymDialog',
]);
</script>
