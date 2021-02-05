<template>
  <div id="app">
    <div style=" text-align:center;margin:20px;">
      <p>{{shopName}} 第{{descNum}}桌 {{personNum}}人</p>
      <p class="display-1 text--primary" style="font-size: 2em !important;margin:30px 0;">
        <v-icon color="primary" large>
          mdi-check-circle
        </v-icon>下单成功，坐等开吃
      </p>
      <div class="text--primary">
        菜品正在制作中....
      </div>
    </div>
    <v-card class="mx-auto" max-width="344">
      <v-card-text>
        <template v-for="food in order.FOODS">
          <v-list-item :key="food.FOOD_DETAIL_NAME">
            <v-list-item-avatar tile size="40">
              <v-img v-if="food.URLS.length > 0" :src="food.URLS[0]"></v-img>
              <v-img v-else style="background: lightgrey;">
                <div style="width:100%;line-height: 40px;">
                  {{ food.FOOD_DETAIL_NAME }}
                </div>
              </v-img>
            </v-list-item-avatar>
            <v-list-item-content>
              <v-list-item-title>
                {{food.FOOD_DETAIL_NAME}}
              </v-list-item-title>
            </v-list-item-content>
            <v-list-item-content>
              <v-list-item-title style="text-align: center;" color="primary"> ${{food.PRICE}} ×
                {{food.QTY }} </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-divider></v-divider>
        </template>
        <v-list-item v-if="order.COST > 0" >
          <v-list-item-avatar  >
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title>
              {{order.CAPITATION}}
            </v-list-item-title>
          </v-list-item-content>
          <v-list-item-content>
            <v-list-item-title style="text-align: center;" color="primary"> ${{order.COST}} ×
              {{food.PERSON_NUM }} </v-list-item-title>
          </v-list-item-content>
        </v-list-item>

      </v-card-text>
    </v-card>
    <div style="text-align:center;padding:8px">
      <v-row>
        <v-col cols="6">
          <v-btn style="width:80%" color="primary" :to="{path:'/food/'+account+'/'+descNum+'/'+personNum}">
            加菜
          </v-btn>
        </v-col>
        <v-col cols="6">
          <v-btn style="width:80%" color="primary" @click="closeOrder()">
            新开一单
          </v-btn>
        </v-col>
      </v-row>

    </div>
  </div>
</template>
<script>
export default {
  name: "Order",
  data: () => ({
    shopName: "",
    account: "",
    descNum: "",
    personNum: "",
    curOrderId: "",
    curOrderId: "",
    order: {},
    urlParam: {},
  }),
  mounted() {
    // this.itemsCar = this.$store.state.itemsCar;
    // this.personNum = this.$store.state.personNum;
    // this.shopName = this.$store.state.shopName;
    // this.descNum = this.$store.state.descNum;

    let self = this;
    self.urlParam = this.$route.params;
    if (self.urlParam.account != null) {
      self.account = self.urlParam.account;
      self.descNum = self.urlParam.descnum;
    }

    self.curOrderId = self.$fw.getJsonInfo("curOrderId");
    if (self.curOrderId == null || self.curOrderId == "") {
      self.$message.error("未找到相关相关订单数据，请重新扫描二维码点菜");
      return;
    }
    self
      .$http(
        "get",
        "/api/Product/GetOrders?account=" +
          self.account +
          "&id=" +
          self.curOrderId
      )
      .then((response) => {
        if (response != null) {
          if (!response.success) {
            self.$message.error(response.message.content);
            return;
          }
          try {
            if (response.data.data[0].ORDER.length != 1) {
              self.$message.error(
                "未找到相关相关订单数据，请重新扫描二维码点菜"
              );
              return;
            }
            self.order = response.data.data[0].ORDER[0];
            self.shopName = response.data.name;
            self.personNum = self.order.PERSON_NUM;
          } catch (err) {
            self.$message.error("未找到相关相关订单数据，请重新扫描二维码点菜");
          }
        }
      });
  },
  methods: {
    closeOrder() {
      let self = this;
      self
        .$http("get", "/api/Product/CloseOrder?orderId=" + self.curOrderId)
        .then((response) => {
          if (!response.success) {
            self.$message.error(response.message.content);
            return;
          }
          self.$router.push({
            path: "/food/" + self.account + "/" + self.descNum,
          });
        });
    },
  },
};
</script>
