<script setup lang="ts">
import axios from 'axios';
import { computed, reactive, ref } from 'vue';
import { useRouter } from 'vue-router';
const router = useRouter();

const user = reactive<RegisterRequest>({
  username: "",
  email: "",
  password: ""
})
const isValid = computed(() => {
  return user.username.length > 4 && user.password.length > 6 && repeatedPassword.value == user.password && user.email.length > 5
});

const errorMsg = ref("");
const submit = async () => {
  isLoading.value = true;
  const i = await axios.post("/api/signup", JSON.stringify(user), {
    headers: {
      "Content-Type": "application/json"
    },
    withCredentials: true
  }).then(function (x) {
    if (x.status == 200) {
      alert('Account Created');

    }
    console.log(x.status);

  }).catch(x => {
    errorMsg.value = x.response.data;
    console.log(x);

  });

  isLoading.value = false;

}

const isLoading = ref(false);
const repeatedPassword = ref('');
</script>

<template>
  <div class="form">

    <input v-model="user.username" placeholder="Username..." />
    <input type="email" v-model="user.email" placeholder="Email..." />
    <input type="password" v-model="user.password" placeholder="Password..." />
    <input type="password" v-model="repeatedPassword" placeholder="Repeat Password..." />
    <p v-show="errorMsg.length > 0">{{ errorMsg }}</p>
    <button @click="submit" :disabled="!isValid && isLoading">Register</button>

  </div>
</template>
<style scoped>
.form {
  width: min-content;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  gap: 10px
}
</style>
