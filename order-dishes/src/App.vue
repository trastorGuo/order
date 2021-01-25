<template>
  <v-app>
        <router-view></router-view>
  </v-app>
</template>

<script>
import Food from "./components/food";

export default {
  name: "App",
  components: {
    Food,
  },
};
</script>
