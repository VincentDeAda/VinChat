<script setup lang="ts">
import axios from "axios";
import { ref, onMounted, onBeforeMount, onUnmounted } from "vue";

import signalR from "../Store/useSignalR";
import Message from "../components/Message.vue";
const { online, startUp, sendMessage, events, removeMessage } = signalR();
const messages = ref<Message[]>(new Array<Message>())
const user = ref();

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
  if (e.shiftKey && !shiftKey.value) {
    shiftKey.value = true;
  }
}
onMounted(async () => {
  await startUp();
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
</script>

<template>
  <div class="grid">

    <div @scroll="onScroll" class="msgs-container" ref="scroll">
      <Message :isShifting="shiftKey" :key="msg.id" @delete="removeMsg" :msg="msg" :newMsg="isNew(index, msg)"
        v-for="msg, index in messages" />

    </div>

    <div class="inputContainer">
      <input v-model="tempMsg" @keypress.enter="sendMsg" type="text" placeholder="your msgs">
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

.inputContainer {
  padding: 5px;
}

.inputContainer {
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
