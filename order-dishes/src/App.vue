<template>
  <v-app>
        <router-view></router-view>
  </v-app>
</template>
<style  >
.container {
  padding: 3px;
}
.v-tab {
  padding: 0 !important;
}
.v-card__subtitle,
.v-card__text,
.v-card__title {
  padding: 0 !important;
}
.v-input input {
  text-align: center;
}
</style>
<script>
import Order from "./components/Order";

export default {
  name: "App",
  components: {
    Order,
  },
};
</script>
