<template>
  <div id="app" style="height: 100%;width: 100%;">
    <div class="backgroud-img-div">
      <img src="http://cdn.trastor.cn/huoguo1.jpg" style="position: fixed;bottom: 0;width: 100%;" />
    </div>
    <div class="login-border-div ">
      <!-- <div style="color: white;text-align: center;">后台管理</div> -->
      <div class="login-border">
        <v-form ref="form" lazy-validation style="text-align: center;">
          <v-text-field v-model="name" label="账号" rounded required outlined dense></v-text-field>
          <v-text-field v-model="password" label="密码" rounded required type="password" outlined dense></v-text-field>
          <v-btn style="width:200px; " color="#73ce5f" rounded dark @click="login()">登陆</v-btn>
        </v-form>
      </div>
    </div>
  </div>
</template>
 
<script>
export default {
  name: "Login",
  data: () => ({
    name: "",
    password: "",
    testUrl: "",
    shopInfo: {},
  }),
  mounted() {
    let token = this.$fw.getToken();
    this.shopInfo = this.$fw.getJsonInfo("shopInfo");
    if ((token != null || token != "") && this.shopInfo.ACCOUNT != null)
      this.getShopInfo(this.shopInfo.ACCOUNT);
  },
  methods: {
    login: function () {
      let self = this;
      if (self.name == null || self.name == "") {
        self.$message.error("账号不能为空");
        return;
      }
      if (self.password == null || self.password == "") {
        self.$message.error("账号不能为空");
        return;
      }
      self
        .$http(
          "get",
          "/api/user/login?name=" +
            self.name +
            "&pwd=" +
            self.$md5(self.password)
        )
        .then((response) => {
          console.log(response);
          if (response != null) {
            if (!response.success) {
              self.$message.error(response.message.content);
              return;
            }
            self.$fw.saveToken(response.data);
            self.getShopInfo(self.name);
          }
        })
        .catch((err) => {
          self.$message.error("登陆失败:" + err);
          console.log(err.message);
        })
        .finally(() => {});
    },
    getShopInfo(ACCOUNT) {
      let self = this;
      self
        .$http("get", "/api/User/ShopInfo?account=" + ACCOUNT)
        .then((response) => {
          console.log(response);
          if (response != null) {
            if (!response.success) {
              self.$message.error(response.message.content);
              return;
            }
            self.$message.success("登陆成功！");
            self.$store.commit("mutationsChangeShopInfo", response.data);
            self.$fw.saveJsonInfo("shopInfo", response.data);
            self.$router.replace({
              path: "/shop/" + ACCOUNT + "/ShopInfo",
            });
          }
        });
    },
  },
};
</script>
