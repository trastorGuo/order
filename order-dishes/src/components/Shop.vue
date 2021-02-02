<template>
  <v-card class="overflow-hidden" style="height:100%">
    <v-app-bar dark src="http://cdn.trastor.cn/xingkong.jpg" scroll-target="#scrolling-techniques-4" color="primary">
      <!-- <v-app-bar-nav-icon></v-app-bar-nav-icon> -->
      <v-toolbar-title>{{shopInfo.NAME}}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-app-bar-nav-icon class="text-decoration-underline" v-if="shopInfo.IS_ADMIN" @click="$router.replace({path:'/Register'})">注册</v-app-bar-nav-icon>
       <v-app-bar-nav-icon style="margin:0 5px;"  class="text-decoration-underline" @click="reLogin()">退出登录</v-app-bar-nav-icon>
      <template v-slot:extension>
        <v-tabs v-model="tab" align-with-title>
          <v-tabs-slider color="primary"></v-tabs-slider>
          <v-tab :to="'/Shop/'+shopInfo.ACCOUNT+'/ShopInfo'" replace>店铺信息</v-tab>
          <v-tab :to="'/Shop/'+shopInfo.ACCOUNT+'/FoodManagement'"> 菜单管理</v-tab>
          <v-tab :to="'/Shop/'+shopInfo.ACCOUNT+'/OrderManagement'" replace>订单管理</v-tab>
        </v-tabs>
      </template>
    </v-app-bar>
    <!-- <v-navigation-drawer v-model="drawer" absolute temporary color="primary">
      <v-list nav dense>
        <v-list-item-group v-model="group" active-class="deep-purple--text text--accent-4">
          <v-list-item>
            <v-list-item-title>Foo</v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title>Bar</v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title>Fizz</v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title>Buzz</v-list-item-title>
          </v-list-item>
        </v-list-item-group>
      </v-list>
    </v-navigation-drawer> -->
    <v-card-text>
      <v-sheet style="padding-top:10px;" id="scrolling-techniques-4" class="overflow-y-auto"
        :max-height="bodyHeight + 'px'">
        <router-view></router-view>
      </v-sheet>
    </v-card-text>
  </v-card>
</template>


<script>
export default {
  name: "Shop",
  data: () => ({
    shopInfo: {},
    drawer: false,
    group: null,
    account: "",
    tab: null,
    bodyHeight: 600,
  }),

  watch: {
    group() {
      this.drawer = false;
    },
  },
  mounted() {
    this.bodyHeight = document.documentElement.clientHeight - 100;
    this.shopInfo = JSON.parse(window.localStorage.getItem("shopInfo"));
  },
  methods: {
    reLogin(){
      this.$fw.clearLogin();
      this.$router.push({path:'/login'});
    }
  },
};
</script>