<script setup lang="ts">
import { defineProps, ref, watch } from 'vue';
const emits = defineEmits(['delete', 'edit'])
const showActions = ref(false)
const editMode = ref(false);
const { isShifting, msg } = defineProps<{ msg: Message, newMsg: boolean, isShifting: boolean, isAuthor: boolean }>()
const isHover = ref(false);
const editedMessage = ref(msg.message)
watch(() => editMode, () => editedMessage.value = msg.message);
const submitEdit = () => {
  emits('edit', msg, editedMessage.value)
  editMode.value = false;

}
</script>
<template >
  <div @mouseenter="isHover = true" @mouseleave="isHover = false" class="message">
    <h4 v-if="newMsg"> {{ msg.author.username }}:</h4>
    <div v-if="!editMode" class="content"> {{ msg.message }}</div>
    <input @keydown.esc="editMode = false" @keypress.enter="submitEdit" v-else type="text" v-model="editedMessage">
    <div class="dropdown">
      <div v-if="isAuthor" v-show="(isShifting && isHover) || editMode" class="options">
        <button v-if="!editMode" @click="emits('delete', msg.id)">Delete</button>
        <button v-else @click="submitEdit">Edit</button>

        <button v-if="!editMode" @click="editMode = true">Edit</button>
        <button v-else @click="editMode = false">Cancel</button>
      </div>
    </div>

  </div>
</template>
<style scoped>
.message {
  position: relative;
  padding: 5px;
}

.message:hover {
  background-color: var(--darker-darker);
}

.content {
  padding: 5px 10px;

}

.icon {
  user-select: none;
}

h4 {
  font-weight: bold;
}



/* .dropdown {
  visibility: hidden;
  position: absolute;
  top: 0;
  right: 10px;
} */

.message:hover .dropdown {
  visibility: visible;

}

/* .options {
  visibility: hidden;
  position: absolute;
  top: 0;
  right: 0;
} */


.showOptions {
  visibility: visible;
  padding: 10px 20px;

}
</style>
