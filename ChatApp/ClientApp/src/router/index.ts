import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import NotFound from '../views/NotFoundView.vue'
import LoginView from '../views/LoginView.vue'
import Cookie from 'universal-cookie'
const cookie = new Cookie();
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      beforeEnter: async (to, from, next) => {
        const isAuth = await fetch('/api/verify', {
          credentials: 'include'
        }).then(x => x.status == 200 ? true : false).catch(x => x);
        console.log(isAuth);

        if (isAuth) {
          next();
        }
        else {
          next({ name: "login" })
        }
      }
    },
    {
      path: "/login",
      name: "login",
      component: LoginView
    },
    {
      path: "/:pathMatch(.*)",
      component: NotFound
    }
  ]
})

export default router
