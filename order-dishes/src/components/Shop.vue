<template>
  <v-card class="overflow-hidden" style="height:100%">
    <v-app-bar dark src="http://cdn.trastor.cn/xingkong.jpg" scroll-target="#scrolling-techniques-4" color="primary">
      <!-- <v-app-bar-nav-icon></v-app-bar-nav-icon> -->
      <v-toolbar-title>{{shopInfo.NAME}}</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-app-bar-nav-icon class="text-decoration-underline" v-if="shopInfo.IS_ADMIN"
        @click="$router.replace({path:'/Register'})">注册</v-app-bar-nav-icon>
      <v-app-bar-nav-icon style="margin:0 10px;" class="text-decoration-underline" @click="isShowChangePw = true">修改密码
      </v-app-bar-nav-icon>
      <v-app-bar-nav-icon style="margin:0 10px;" class="text-decoration-underline" @click="reLogin()">退出登录
      </v-app-bar-nav-icon>
      <template v-slot:extension>
        <v-tabs v-model="tab" align-with-title>
          <v-tabs-slider color="primary"></v-tabs-slider>
          <v-tab :to="'/Shop/'+shopInfo.ACCOUNT+'/ShopInfo'">店铺信息</v-tab>
          <v-tab :to="'/Shop/'+shopInfo.ACCOUNT+'/FoodManagement'"> 菜单管理</v-tab>
          <v-tab :to="'/Shop/'+shopInfo.ACCOUNT+'/Statistics'">营业统计</v-tab>
          <v-tab :to="'/Shop/'+shopInfo.ACCOUNT+'/OrderManagement'">订单管理</v-tab>
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
    <v-dialog persistent v-model="isShowChangePw">
      <template>
        <v-card>
          <v-toolbar color="primary" dark>修改密码</v-toolbar>
          <v-card-text>
            <v-text-field style="margin-right: 16px;" v-model="shopInfo.ACCOUNT" label="账号" disabled
              prepend-icon="mdi-account-supervisor"></v-text-field>
            <v-text-field style="margin-right: 16px;" v-model="oldPassword" label="原密码" type="password"
              prepend-icon="mdi-account-supervisor"></v-text-field>
            <v-text-field style="margin-right: 16px;" v-model="newPassword" label="新密码" type="password"
              prepend-icon="mdi-account-supervisor"></v-text-field>
            <v-text-field style="margin-right: 16px;" v-model="newPassword2" label="新密码再输一次" type="password"
              prepend-icon="mdi-account-supervisor"></v-text-field>
          </v-card-text>
          <v-card-actions class="justify-end">
            <v-row>
              <v-col cols="6">
                <v-btn block @click="changePw()" color="primary">修改 </v-btn>
              </v-col>
              <v-col cols="6">
                <v-btn block @click="cancelChange()" color="primary">取消 </v-btn>
              </v-col>
            </v-row>

          </v-card-actions>
        </v-card>
      </template>
    </v-dialog>

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
    oldPassword: "",
    newPassword: "",
    newPassword2: "",
    isShowChangePw: false,
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
    reLogin() {
      this.$fw.clearLogin();
      this.$router.replace({ path: "/login" });
    },
    changePw() {
      let self = this;
      if (self.newPassword != self.newPassword2) {
        self.$message.error("输入的两次新密码不一致");
        return;
      }
      if (self.oldPassword == self.newPassword) {
        self.$message.error("新密码不能与旧密码相同");
        return;
      }
      self
        .$http("post", "/api/user/ModifiefPassword", {
          username: self.shopInfo.ACCOUNT,
          passwordbefore: self.$md5(self.oldPassword),
          passwordafter: self.$md5(self.newPassword),
        })
        .then((response) => {
          if (!response.success) {
            self.$message.error(response.message.content);
            return;
          }
          self.$message.success("修改成功,请重新登陆");
          self.reLogin();
        });
    },
    cancelChange() {
      (this.oldPassword = ""),
        (this.newPassword = ""),
        (this.newPassword2 = ""),
        (this.isShowChangePw = false);
    },
  },
};
</script>