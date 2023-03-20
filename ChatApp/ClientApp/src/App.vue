<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import Loading from './components/Loading.vue';
import Navbar from './components/NavBar.vue';


</script>

<template>
  <div class="wrapper">
    <header>
      <Navbar />
    </header>

    <RouterView v-slot="{ Component }">
      <template v-if="Component">
        <Transition mode="out-in">
          <KeepAlive>
            <Suspense>
              <component :is="Component"></component>
              <template #fallback>
                <Loading />
              </template>
            </Suspense>
          </KeepAlive>
        </Transition>
      </template>
    </RouterView>
  </div>
</template>


<style scoped>
.wrapper {
  height: 100vh;
  width: 100vw;
  display: grid;
  grid-template-rows: 0.5fr 9.5fr;

}
</style>
