<script setup lang="ts">
import signalR from "../Store/useSignalR";
import credentials from "../Store/useCredentials";
import axios from "axios";
const { isAuth, verify } = credentials();
const { online } = signalR();
const logOut = async () => {
  await axios.post("/api/signout", { withCredentials: true }).catch(x => x)
  verify();
  window.location.reload()
}
</script>

<template>
  <nav>
    <h1>VinChat</h1>
    <div class="nav-items">
      <h1>Online:{{ online }}</h1>
      <button v-if="isAuth" @click="logOut">Log Out</button>
    </div>
  </nav>
</template>


<style scoped>
nav {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-direction: row;
  padding: 5px;
  border-bottom: 1px solid;
}

h1 {
  user-select: none;
}

.nav-items {
  display: flex;
  flex-direction: row;
  gap: 10px;
}
</style>
