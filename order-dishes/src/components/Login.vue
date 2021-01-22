<template>
  <div id="app" style="height: 100%;width: 100%;">
    <div class="backgroud-img-div">
      <img  :src="testUrl" style="position: fixed;bottom: 0;width: 100%;" />
      <!-- <img src="https://gitee.com/trastor/picture/raw/master/1611238740(1).jpg" style="position: fixed;bottom: 0;width: 100%;" /> -->
      <!-- <img src="https://gitee.com/trastor/picture/raw/master/image-20210117195619570.png" style="position: fixed;bottom: 0;width: 100%;" /> -->
    </div>
    <div class="login-border-div ">
      <!-- <div style="color: white;text-align: center;">周洛御景山庄</div> -->
      <div class="login-border">
        <v-form ref="form" lazy-validation style="text-align: center;">
          <v-text-field v-model="name" label="账号1" required></v-text-field>
          <v-text-field v-model="password" label="密码" required type="password"></v-text-field>
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
    // testUrl: "https://gitee.com/trastor/picture/raw/master/huoguo.jpg",
  }),

  mounted() {
    let self = this;
    setTimeout(function(){
      self.testUrl = "https://gitee.com/trastor/picture/raw/master/huoguo.jpg";
    },500)
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
          "/api/user/login?name=" + self.name + "&pwd=" + self.password
        )
        .then((response) => {
          console.log(response);
          if (response != null) {
            if (!response.success) {
              self.$message.error(response.message.content);
              return;
            }
            self.$message.success("登陆成功！");
            window.localStorage.setItem("accessToken", response.data);
            self.$router.push({ path: "/food/"+self.name });
          }
        })
        .catch((err) => {
          self.$message.error("登陆失败:"+err);
          console.log(err.message);
        })
        .finally(() => {});
    },
  },
};
</script>
