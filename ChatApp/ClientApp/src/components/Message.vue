<script setup lang="ts">
import { computed } from '@vue/reactivity';
import { defineProps, ref, watch } from 'vue';
const emits = defineEmits(['delete', 'edit'])
const showActions = ref(false)
const editMode = ref(false);
const { isShifting, msg } = defineProps<{ msg: Message, newMsg: boolean, isShifting: boolean, isAuthor: boolean,index:number }>()
const isHover = ref(false);
const editedMessage = ref(msg.message)
watch(() => editMode, () => editedMessage.value = msg.message);
const submitEdit = () => {
  emits('edit', msg, editedMessage.value)
  editMode.value = false;

}
const splittedMsg = computed(() => msg.message.split('\n'));
</script>
<template >
  <div @mouseenter="isHover = true" @mouseleave="isHover = false" class="message">
    <div class="divider" v-if="newMsg &&index!=0 "></div>
    <h4 v-if="newMsg"> {{ msg.author.username }}:</h4>
    <div v-if="!editMode" class="content"> <span v-for="line, index in splittedMsg">{{ line }} <span class="edit tooltip"
          v-if="msg.isEdited && index == splittedMsg.length - 1">edited <span class="tooltip-text">{{
            msg.lastEditDate!.toString()
          }}</span></span>

      </span>
    </div>
    <textarea @keydown.esc="editMode = false" @keypress.enter.exact="submitEdit" v-else type="text"
      v-model="editedMessage"></textarea>
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

.divider {
  height: 0;
  width: 70%;
  margin: auto;
  border-top: 1px solid;
  opacity: 50%;
}

textarea {
  width: 95%;
  height: max-content;
}

.message:hover {
  background-color: var(--darker-darker);
}

.content {
  display: flex;
  flex-direction: column;
  padding: 2px 10px;

}

.icon {
  user-select: none;
}

h4 {
  font-weight: bold;
}

.edit {
  user-select: none;
  font-size: 0.5rem;
  font-style: italic;
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
