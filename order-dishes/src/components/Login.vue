<template>
  <div id="app" style="height: 100%;width: 100%;">
    <div class="backgroud-img-div">
      <img src="../assets/huoguo.jpg" style="position: fixed;bottom: 0;width: 100%;" />
    </div>
    <div class="login-border-div ">
        <!-- <div style="color: white;text-align: center;">周洛御景山庄</div> -->
      <div  class="login-border">
        <v-form ref="form" lazy-validation style="text-align: center;">
          <v-text-field v-model="name" label="账号" required  ></v-text-field>
          <v-text-field v-model="password" label="密码" required type="password" ></v-text-field>
          <v-btn style="width:200px; " color="#73ce5f" rounded  dark @click="login()">登陆</v-btn>
        </v-form>
      </div>
    </div>
  </div>
</template>
 
<script>
// import axios from "axios";

export default {
  name: "Login",
  data: () => ({
    name: "",
    password: "",
  }),

  mounted() {
    // document.getElementsByTagName("html")[0].className="body-bg1";
    // axios
    //   .get("http://localhost:59703/order/food/Get")
    //   .then((response) => console.log(response));
  },
  methods: {
    login:function(){
      this.$message.success(`success这是一条消息`)
    }
  },
};
</script>
