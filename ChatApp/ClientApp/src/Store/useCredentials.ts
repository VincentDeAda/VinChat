import axios from "axios";
import { ref } from "vue";

const isAuth = ref(false);
const credentials = ref<UserInfo>({ id: "", username: "" });
const verify = async () => {
  await axios('/api/verify', {
    withCredentials: true
  }).then(x => isAuth.value = x.status == 200).catch(x => x);
}
const fetchUserInfo = async () => {
  await axios.get('/api/FetchUserInfo').then(x => credentials.value = x.data).catch(x => x);
}
export default function () {
  return {
    isAuth, credentials,

    verify, fetchUserInfo
  }
}