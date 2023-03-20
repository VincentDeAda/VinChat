<script setup lang="ts">
import signalR from "../Store/useSignalR";

const { online } = signalR();
</script>

<template>
  <nav>
    <h1>VinChat</h1>
    <h1>Online:{{ online }}</h1>
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
</style>
