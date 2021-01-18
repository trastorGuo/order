<template>
  <div id="app" style="height: 100%;width: 100%;">
    <div class="backgroud-img-div">
      <img src="../assets/huoguo.jpg" style="position: fixed;bottom: 0;width: 100%;" />
    </div>
    <div class="main-border-div ">
      <!-- <div style="color: white;text-align: center;">周洛御景山庄</div> -->
      <div class="main-border">
        <v-form ref="form" lazy-validation style="text-align: center;">
          <v-text-field v-model="form.NAME" label="店铺名" required></v-text-field>
          <v-text-field v-model="form.TEL" label="联系方式" :rules="telRules" required></v-text-field>
          <v-text-field v-model="form.ADDRESS" label="地址" required></v-text-field>
          <v-text-field v-model="form.ACCOUNT" label="账号" :rules="AccountRules" autocomplete="off" required></v-text-field>
          <v-text-field v-model="form.PASSWORD" label="密码" :rules="AccountRules" required type="password" autocomplete="new-password">
          </v-text-field>
          <v-text-field v-model="form.PASSWORD2" label="再输入一次密码" :rules="pw2Rules" required type="password" autocomplete="new-password">
          </v-text-field>
          <v-btn style="width:200px; " color="#73ce5f" rounded dark @click="Register()">新增</v-btn>
        </v-form>
      </div>
    </div>
  </div>
</template>
 
<script>
import axios from "axios";
export default {
  name: "Login",
  data: () => ({
    form: {
      NAME: "",
      TEL: "",
      ADDRESS: "",
      ACCOUNT: "",
      PASSWORD: "",
      PASSWORD2: "",
    },
      telRules: [
        value => {
          const pattern = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
          return pattern.test(value) || '无效的手机号码'
        },
      ],
      AccountRules: [
        value => {
          const pattern = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,}$/;
          return pattern.test(value) || '长度需大于6位，由数字和字符组成'
        },
      ],
 
  }),

  mounted() {
    // document.getElementsByTagName("html")[0].className="body-bg1";
    // axios
    //   .get("http://localhost:59703/order/food/Get")
    //   .then((response) => console.log(response));
  },
  methods: {
    Register: function () {
      let self = this;
      if(self.form.NAME == ""){
        
      }
    },
  },
};
</script>
