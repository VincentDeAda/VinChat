<script setup lang="ts">
</script>
<template>
  <div class="loading">
    <h1 class="txt">Loading....</h1>
  </div>
</template>
<style scoped>
.loading {
  display: flex;

  align-items: center;
  justify-content: center;
  padding-bottom: 5%;
}

.txt {
  animation: grow 2s infinite both ease-in-out;
  user-select: none;
  font-size: 6.6rem;
}

@keyframes grow {
  0% {
    transform: scale(70%);

  }

  50% {
    transform: scale(60%);

  }

  100% {
    transform: scale(70%);

  }
}
</style>