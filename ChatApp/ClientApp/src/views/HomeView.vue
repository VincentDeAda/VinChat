<script setup lang="ts">
import axios from "axios";
import { ref, onMounted } from "vue";
import signalR from "../Store/useSignalR";
const { online, startUp, sendMessage, events, removeMessage } = signalR();
const msgs = ref<Message[]>();
const user = ref();

events.MessageReceived = (e) => {
  msgs.value!.push(e)
};
events.MessageRemoved = (e) => {
  const index = msgs.value?.findIndex(x => x.id == e)
  if (index != undefined)
    msgs.value?.splice(index, 1);
}
onMounted(() => {
  startUp();

})

await axios.get('/api/FetchUserInfo').then(x => user.value = x.data);

await axios.get('/api/FetchMessages').then(x => msgs.value = x.data as Message[]);
const tempMsg = ref('');
const sendMsg = async () => {
  await sendMessage(tempMsg.value)
  tempMsg.value;
}
const removeMsg = async (id: string) => {
  await removeMessage(id)
}

// await new Promise<void>(r => setTimeout(() => r(), 30000000));
</script>

<template>
  <div class="grid">

    <div class="msgs-container">
      <div v-for="msg in msgs" :key="msg.id"><span class="author">{{ msg.author.username }}</span>: <span
          class="messageContent">{{
            msg.message }}</span>
        <button v-if="msg.author.id == user.id" @click="removeMsg(msg.id)">Remove</button>
      </div>

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


  padding: 10px 20px;
  display: flex;
  flex-direction: column;
  gap: 5px;
  overflow-y: scroll;
}
</style>
