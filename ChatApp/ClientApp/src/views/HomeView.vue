<script setup lang="ts">
import axios from "axios";
import { ref, onMounted } from "vue";

import signalR from "../Store/useSignalR";
import Message from "../components/Message.vue";
const { online, startUp, sendMessage, events, removeMessage } = signalR();
const messages = ref<Message[]>()
const user = ref();

events.MessageReceived = (e) => {
  messages.value!.push(e)
};
events.MessageRemoved = (e) => {
  const index = messages.value?.findIndex(x => x.id == e)
  if (index != undefined)
    messages.value?.splice(index, 1);
}
onMounted(() => {
  startUp();

})

await axios.get('/api/FetchUserInfo').then(x => user.value = x.data);

await axios.get('/api/FetchMessages').then(x => messages.value = x.data as Message[]);
const tempMsg = ref('');
const sendMsg = async () => {
  await sendMessage(tempMsg.value)
  tempMsg.value = '';
}
const removeMsg = async (id: string) => {
  await removeMessage(id)
}
</script>

<template>
  <div class="grid">

    <div class="msgs-container">
      <Message @delete="removeMsg" :msg="msg" :newMsg="index > 0 ? msg.author == messages![index - 1].author! : true"
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
