<template>
  <div id="app" style="height: 100%;width: 100%;">
    <div class="backgroud-img-div">
      <img src="https://gitee.com/trastor/picture/raw/master/huoguo.jpg"
        style="position: fixed;bottom: 0;width: 100%;" />
    </div>
    <div class="login-border-div" style="top: 10%;">
      <!-- <div style="color: white;text-align: center;">周洛御景山庄</div> -->
      <div class="login-border">
        <v-form ref="form" lazy-validation style="text-align: center;">
          <v-text-field v-model="form.NAME" label="店铺名称" rounded required outlined dense :rules="requiredRules">
          </v-text-field>
          <v-text-field v-model="form.ADDRESS" label="地址" rounded required outlined dense :rules="requiredRules">
          </v-text-field>
          <v-text-field v-model="form.TEL" label="联系方式" rounded required outlined dense :rules="telRules">
          </v-text-field>
          <v-text-field v-model="form.PRINTER" label="打印机" rounded required outlined dense></v-text-field>
          <v-text-field v-model="form.ACCOUNT" label="账号" rounded required outlined dense :rules="accountRules">
          </v-text-field>
          <v-text-field v-model="form.PASSWORD" label="密码" rounded required type="password" outlined dense :rules="accountRules">
          </v-text-field>
          <v-text-field v-model="form.PASSWORD2" label="再输一次密码" rounded required type="password" outlined dense>
          </v-text-field>
          <v-btn style="width:200px; " color="#73ce5f" rounded dark @click="register()">注册</v-btn>
        </v-form>
      </div>
    </div>
  </div>
</template>
 
<script>
// import axios from "axios";
export default {
  name: "Register",
  data: () => ({
    form: {
      NAME: "",
      ADDRESS: "",
      TEL: "",
      PRINTER: "",
      ACCOUNT: "",
      PASSWORD: "",
      PASSWORD2: "",
    },
    requiredRules: [(v) => !!v || "必填"],
    telRules: [
      (value) => {
        const pattern = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
        return pattern.test(value) || "无效的手机号码";
      },
    ],
    accountRules: [
      (value) => {
        const pattern = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,}$/;
        return pattern.test(value) || "长度需大于5位，由数字和字符组成";
      },
    ],
  }),

  mounted() {},
  methods: {
    register: function () {
      let self = this;
      if (!self.$refs.form.validate()) {
        self.$message.error("请提交正确数据");
        return;
      }
      if (self.form.PASSWORD != self.form.PASSWORD2) {
        self.$message.error("两次密码不相同，请再次确认");
        return;
      }
      self
        .$http("post", "/api/User/AddShop", self.form)
        .then((responseInfo) => {
          console.log(responseInfo);
          if (!responseInfo.success) {
            self.$message.error(responseInfo.message.content);
            return;
          }
          self.$message.success("店铺[" + self.form.NAME + "]注册成功,欢迎您");
          self.$fw.clearLogin();
          self.$router.replace({
            path: "/login",
          });
        });
    },
  },
};
</script>
