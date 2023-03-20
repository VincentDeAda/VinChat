<script setup lang="ts">
import LoginPage from "../components/LoginPage.vue"
import RegisterPage from "../components/RegisterPage.vue"
import { ref, watch } from 'vue';
const toLogin = ref(true);
const toLeft = ref(true)
const filled = ref(false);
let isAnimating = ref(false);
function playFill() {
  isAnimating.value = true;
  filled.value = true;
  ///Value Has to be 50% of the total animation length
  setTimeout(() => toLeft.value = !toLeft.value, 250)
  setTimeout(() => {
    filled.value = false;
    isAnimating.value = false;
  }, 500)


}
watch(toLogin, (newVal) => {
  playFill();
});
</script>

<template>
  <div class="container">
    <div class="mode">
      <button :disabled="isAnimating" @click="toLogin = true">Login</button>
      <button :disabled="isAnimating" @click="toLogin = false">Sign Up</button>
      <div :class="{ fill: filled, left: toLeft, right: !toLeft }" class="bar"></div>
    </div>

    <div class="content">
      <div class="info">
        <h1>VinChat</h1>
        <p>Lorem ipsum dolor sit amet consectetur adipisicing elit. Obcaecati necessitatibus saepe distinctio porro!
          Necessitatibus iste sed alias corporis blanditiis numquam eveniet aliquam beatae, illum cupiditate est quidem
          quae
          provident exercitationem!</p>
      </div>
      <div class="info">
        <Transition name="table" appear enter>

          <LoginPage v-if="toLogin" />
          <RegisterPage v-else />
        </Transition>
      </div>
    </div>
  </div>
</template>
<style scoped>
.container {
  display: flex;
  flex-direction: column;
  align-items: center;
  widows: 100vw;
  height: 90vh;
}

.mode {
  position: relative;
}

.bar {
  transition: all 0.5s ease-in-out;
  position: absolute;
  bottom: 0;
  width: 50%;
  border-bottom: 2px solid;
}

.right {
  right: 0;
}

.left {
  left: 0;
}

.fill {
  animation: fill 0.5s ease-in-out both;
}

@keyframes fill {
  0% {
    width: 50%;
  }

  50% {
    width: 100%;

  }

  100% {
    width: 50%;
  }
}

.content {
  margin: 10px;
  padding-top: 10px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-bottom: 30vh;
  gap: 10px;
  width: 80%;
  height: 100%;
}

input {
  width: 33px !important;
}

.info {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  min-width: 50%;
  min-height: 70%;
  border-radius: 5px;
}


.table-enter-active {
  transition: all 0.5s ease-in-out;

}

.info p {
  width: 75%;
  text-align: center;
}

@media screen and (max-width: 700px) {
  .content {
    flex-direction: column;
  }
}

.table-enter-from,
.table-leave-to {
  opacity: 0;
  position: fixed;
  transform: translateX(60px);

}
</style>