<template>
  <v-app>
        <router-view></router-view>
  </v-app>
</template>

<script>
import Food from "./components/Food";

export default {
  name: "App",
  components: {
    Food,
  },
};
</script>
