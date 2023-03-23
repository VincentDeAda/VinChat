<script setup lang="ts">
import { defineProps, ref, computed } from 'vue';
const emits = defineEmits(['delete', 'edit'])
const showActions = ref(false)

const { isShifting } = defineProps<{ msg: Message, newMsg: boolean, isShifting: boolean }>()
const isHover = ref(false);
const show = computed(() => isHover && isShifting);


const toggle = ref<HTMLElement>();
</script>
<template >
  <div @mouseenter="isHover = true" @mouseleave="isHover = false" class="message">
    <h4 v-if="newMsg">{{ msg.author.username }}:</h4>
    <div class="content">{{ msg.message }}</div>
    <div class="dropdown">
      <button v-show="isHover && isShifting" @click.stop="showActions = !showActions" class="icon">...</button>
      <div ref="toggle" :class="{ showOptions: show }" class="options">
        <button @click="emits('delete', msg.id)">Delete</button>
        <button @click="emits('edit')">Edit</button>
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



.dropdown {
  visibility: hidden;
  position: absolute;
  top: 0;
  right: 10px;
}

.message:hover .dropdown {
  visibility: visible;

}

.options {
  visibility: hidden;
  position: absolute;
  top: 0;
  right: 0;
}


.showOptions {
  visibility: visible;
  padding: 10px 20px;

}
</style>
