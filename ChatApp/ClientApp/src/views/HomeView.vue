<script setup lang="ts">
import axios from "axios";
import { ref, onMounted, onBeforeMount, onUnmounted, provide } from "vue";

import signalR from "../Store/useSignalR";
import Message from "../components/Message.vue";
const { online, startUp, sendMessage, events, removeMessage, updateMessage } = signalR();
const currentEdit = ref<Message>()
const messages = ref<Message[]>(new Array<Message>())
const user = ref();
provide('UserInfo', user)
let reachedEnd = false;
events.MessageReceived = (e) => {
  messages.value.push(e)

};
const fetchMoreMessages = async () => {
  axios.get(`/api/FetchMessages?skip=${messages.value.length}`).then(x => {
    if (x.status == 204) {
      reachedEnd = true;
      return;
    }
    const data = Array.from(x.data);
    if (data.length < 50)
      reachedEnd = true;
    data.forEach(message => messages.value.unshift(message as Message));
  });
}


events.MessageUpdated = (e) => {
  const index = messages.value.findIndex(x => x.id = e.id);
  if (index == -1)
    return;
  messages.value[index].message = e.message;
  messages.value[index].lastEditDate = e.lastEditDate;
  messages.value[index].isEdited = true;
}
events.MessageRemoved = (e) => {
  const index = messages.value?.findIndex(x => x.id == e)
  if (index != undefined)
    messages.value?.splice(index, 1);
}
const shiftKey = ref(false);

onBeforeMount(() => {
  axios.get('/api/FetchUserInfo').then(x => user.value = x.data);
  axios.get('/api/FetchMessages').then(x => Array.from((x.data)).forEach(y => messages.value.push(y as Message)));
})
const watchShiftDown = (e: KeyboardEvent) => {
  if (e.shiftKey && !shiftKey.value) {
    shiftKey.value = true;
  }
}
const watchShiftUp = (e: KeyboardEvent) => {
  if (e.key == 'Shift' && shiftKey.value) {
    shiftKey.value = false;
  }
}
onMounted(async () => {
  await startUp();
  if (scroll.value!.children.length > 1)
    scroll.value?.children[scroll.value?.children.length - 1].scrollIntoView();
  document.addEventListener('keydown', watchShiftDown);
  document.addEventListener('keyup', watchShiftUp);
})
onUnmounted(() => {
  document.removeEventListener('keydown', watchShiftDown);
  document.removeEventListener('keyup', watchShiftUp);
})


const tempMsg = ref('');
const sendMsg = async () => {
  await sendMessage(tempMsg.value)
  tempMsg.value = '';
}
const removeMsg = async (id: string) => {
  await removeMessage(id)
}
const updateMsg = async () => {
  await updateMessage(currentEdit.value!.id, tempMsg.value)
  tempMsg.value = '';
  currentEdit.value = undefined;
}
const requestEdit = (message: Message) => {
  currentEdit.value = message;
  tempMsg.value = message.message;
}
const scroll = ref<HTMLElement>()

const onScroll = async (e: Event) => {
  if (reachedEnd) return;
  const item = (e.target as HTMLDivElement)
  if (item.scrollTop < 20)
    await fetchMoreMessages();
}
const isNew = (index: number, msg: Message) => {

  if (index == 0)
    return true;

  return msg.author.id != messages.value[index - 1].author.id;
}
const enterListener = async () => {
  if (currentEdit.value != undefined) {
    await updateMsg()
  }
  else
    await sendMsg()
}
</script>

<template>
  <div class="grid">

    <div @scroll="onScroll" class="msgs-container" ref="scroll">

      <Message @edit="requestEdit" :isShifting="shiftKey" :key="msg.id" @delete="removeMsg" :msg="msg"
        :newMsg="isNew(index, msg)" v-for="msg, index in messages" />

    </div>

    <div class="inputContainer">
      <div v-if="currentEdit != undefined" class="edit">Edt Mode</div>
      <input v-model="tempMsg" @keydown.esc="currentEdit = undefined" @keypress.enter="enterListener" type="text"
        placeholder="your msgs">
    </div>

  </div>
</template>
<style scoped>
.grid {
  display: grid;
  grid-template-rows: 9.5fr 1fr;
  gap: 5px;
  overflow: hidden;
}

.edit {
  width: 100%;
  background-color: var(--darker-darker);
  position: absolute;
  top: 0;
  transform: translateY(-100%);
}

.inputContainer {
  padding: 5px;
}

.inputContainer {
  position: relative;
  min-height: min-content;
  max-height: 230px;
}

input {
  height: 100%;
  width: 100%;
  border-radius: 0;
}

.author {
  font-weight: 600;
}

.msgs-container {


  display: flex;
  flex-direction: column;
  gap: 5px;
  overflow-y: scroll;
}
</style>
