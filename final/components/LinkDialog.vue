<template>
  <v-dialog v-model="modelValue" @update:modelValue="closeDialog">
    <v-card>
      <v-sheet color="secondary">
        <v-card-title>Share the link with your friends!</v-card-title>
      </v-sheet>
      <v-alert v-if="copied" tile type="success"
        >Successfully copied to clipboard!</v-alert
      >
      <v-card-text>
        <v-text-field
          class="shrink"
          style="width: 350px"
          variant="outlined"
          readonly
          append-inner-icon="mdi-content-copy"
          @click:append-inner="copyToClipboard"
          v-model="url" />
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
const modelValue = defineModel<boolean>();
const props = defineProps({
  documentId: { type: Number, required: true },
});
const url = `website.net/documentView?id=${props.documentId}`;
const copied = ref(false);

function copyToClipboard() {
  navigator.clipboard.writeText(url);
  copied.value = true;
}

function closeDialog() {
  setTimeout(() => {
    copied.value = false;
  }, 500);
}
</script>
