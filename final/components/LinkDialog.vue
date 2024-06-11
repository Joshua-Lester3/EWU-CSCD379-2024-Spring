<template>
  <v-dialog v-model="modelValue" @update:modelValue="closeDialog" width="400">
    <v-card>
      <v-sheet color="secondary">
        <v-card-title>Share the link with your friends!</v-card-title>
      </v-sheet>
      <v-alert v-if="copied" tile type="success"
        >Successfully copied to clipboard!</v-alert
      >
      <v-card-text>
        <v-row>
          <v-col cols="6">
            <p>Toggle to share with friends</p>
          </v-col>
          <v-col>
            <v-switch
              color="secondary"
              v-model="switchModelValue"
              @update:model-value="$emit('toggleShared', switchModelValue)" />
          </v-col>
        </v-row>
        <v-text-field
          label="copy me :)"
          class="shrink"
          style="width: 350px"
          variant="outlined"
          readonly
          :disabled="!switchModelValue"
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
  shared: { type: Boolean, required: true },
});
const emits = defineEmits(['toggleShared']);
const url = `website.net/documentView?id=${props.documentId}`;
const copied = ref(false);
const switchModelValue = ref(props.shared);

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
