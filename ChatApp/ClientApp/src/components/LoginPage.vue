<script setup lang="ts">
import { useRouter } from 'vue-router';
import { computed, reactive, ref } from 'vue';
import axios from 'axios'
const router = useRouter();

const user = reactive<LoginRequest>({
  username: "",
  password: ""
})
const isValid = computed(() => {
  return user.username.length > 4 && user.password.length > 6
})
const errorMsg = ref("");
const submit = async () => {
  isLoading.value = true;
  const i = await axios.post("/api/signin", JSON.stringify(user), {
    headers: {
      "Content-Type": "application/json"
    },
    withCredentials: true
  }).then(function (x) {
    if (x.status == 200) {
      router.push('/');

    }
    console.log(x.status);

  }).catch(x => {
    errorMsg.value = x.response.data;
    console.log(x);

  });

  isLoading.value = false;

}

const isLoading = ref(false);
</script>

<template>
  <div class="form">
    <input v-model="user.username" placeholder="Username..." />
    <input type="password" v-model="user.password" placeholder="Password..." />
    <p v-show="errorMsg.length > 0">{{ errorMsg }}</p>
    <button @click="submit" :disabled="!isValid && isLoading">Sign In</button>

  </div>
</template>
<style scoped>
p {
  color: rgb(167, 46, 46);
  text-align: center;
}

.form {
  width: min-content;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  gap: 10px
}
</style>
