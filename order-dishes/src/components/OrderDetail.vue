<template>
  <div id="app" style="height: 1000px;width: 100%;">
    <div>
      <v-row no-gutters style="margin: 25px 15.5px 0 15.5px;text-align: center;">
        <v-col cols="6">
          <v-btn v-if="order.IS_CLOSE=='N'" style="width:80%; " color="primary" rounded @click="closeOrder()">
            关闭
          </v-btn>
          <v-btn v-else style="width:80%;" color="primary" rounded @click="delOrder()">
            删除
          </v-btn>
        </v-col>
        <v-col cols="6">
          <v-btn style="width:80%; " color="primary" rounded @click="rePrint()">
            补打
          </v-btn>
        </v-col>
      </v-row>
      <v-card class="mx-auto" max-width="344">
        <v-card-text style="padding: 15px!important;margin-top:15px;">
          <v-form ref="form" lazy-validation style="text-align: center;margin:20px;">
            <v-text-field v-model="order.ORDER_ID" label="订单号" outlined dense disabled></v-text-field>
            <v-text-field v-model="order.DESC_NUM" label="桌号" outlined dense disabled></v-text-field>
            <v-text-field v-model="order.PERSON_NUM" label="用餐人数" outlined dense disabled></v-text-field>
            <v-text-field v-model="order.ORDER_DATE" label="下单时间" outlined dense disabled></v-text-field>
          </v-form>
        </v-card-text>
      </v-card>
      <v-card class="mx-auto" max-width="344">
        <v-card-text style="padding: 15px!important;margin-top:15px;">
          <template v-for="(food,index) in order.FOODS">
            <v-list-item :key="index">
              <v-list-item-avatar tile size="40">
                <v-img v-if="food.URLS.length > 0" :src="food.URLS[0]"></v-img>
                <v-img v-else style="background: lightgrey;">
                  <div style="width:100%;line-height: 40px;font-size: 12px;">
                    {{ food.FOOD_DETAIL_NAME }}
                  </div>
                </v-img>
              </v-list-item-avatar>
              <v-list-item-content>
                <v-list-item-title>
                  {{food.FOOD_DETAIL_NAME}}
                </v-list-item-title>
              </v-list-item-content>
              <!-- <v-spacer></v-spacer> -->
              <v-list-item-content>
                <v-list-item-title style="text-align: right;" color="primary"> ${{food.PRICE}} ×
                  {{food.QTY }} </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-divider></v-divider>
          </template>
          <v-list-item v-if="order.COST > 0">
            <v-list-item-avatar>
            </v-list-item-avatar>
            <v-list-item-content>
              <v-list-item-title>
                {{order.CAPITATION}}
              </v-list-item-title>
            </v-list-item-content>
            <v-list-item-content>
              <v-list-item-title style="text-align: right;" color="primary">
                ${{order.COST}} × {{order.PERSON_NUM}}
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item>
            <v-list-item-avatar>
            </v-list-item-avatar>
            <v-list-item-content>
              <v-list-item-title>
                共计：
              </v-list-item-title>
            </v-list-item-content>
            <v-list-item-content>
              <v-list-item-title style="text-align: right;" color="primary">
                ${{order.ALL_PRICE}}
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>

        </v-card-text>
      </v-card>
    </div>
  </div>
</template>
<script>
export default {
  data: () => ({
    order: {},
  }),
  mounted() {
    this.order = this.$fw.getJsonInfo("curOrder");
  },
  methods: {
    closeOrder(desk) {
      let self = this;
      self.$dialog.confirm("确认关闭该订单？").then(function (dialog) {
        self
          .$http(
            "get",
            "/api/Product/CloseOrder?orderId=" + self.order.ORDER_ID
          )
          .then((response) => {
            if (!response.success) {
              self.$message.error(response.message.content);
              return;
            }
            self.$message.success("操作成功");
            self.$router.go(-1);
          });
      });
    },
    rePrint(desk) {
      let self = this;
      self.$dialog
        .confirm("确认补打该订单(请确保打印机已开启并成功联网)？")
        .then(function (dialog) {
          self
            .$http("get", "/api/Product/Reprint?orderId=" + self.order.ORDER_ID)
            .then((response) => {
              if (!response.success) {
                self.$message.error(response.message.content);
                return;
              }
              self.$message.success("已补打");
              self.$router.go(-1);
            });
        });
    },
    delOrder(desk) {
      let self = this;
      self.$dialog
        .confirm("确认删除该订单(不可恢复)？")
        .then(function (dialog) {
          self
            .$http("get", "/api/Product/DeleteOrder?id=" + self.order.ORDER_ID)
            .then((response) => {
              if (!response.success) {
                self.$message.error(response.message.content);
                return;
              }
              self.$message.success("已删除");
              self.$router.go(-1);
            });
        });
    },
    // getOrderInfo() {
    //   let self = this;
    //   self
    //     .$http("get", "/api/Product/GetOrders?id=" + self.order.ORDER_ID)
    //     .then((response) => {
    //       if (!response.success) {
    //         self.$message.error(response.message.content);
    //         return;
    //       }
    //       if (response.data[0].ORDER.length == 1) {
    //         self.order = response.data[0].ORDER[0];
    //       }
    //     });
    // },
  },
};
</script>
