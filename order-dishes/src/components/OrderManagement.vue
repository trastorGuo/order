 <template>
  <div id="app" style="height: 1000px;width: 100%;">
    <v-row no-gutters style="margin: 25px 15.5px 0 15.5px;text-align: center;">
      <v-col cols="6">
        <v-btn style="width:80%; " color="primary" rounded @click="closeAllOrder()">
          关闭所有订单
        </v-btn>
      </v-col>
      <v-col cols="6">
        <v-btn style="width:80%; " color="primary" rounded @click="clearPrint()">
          清空待打印
        </v-btn>
      </v-col>
    </v-row>
    <v-expansion-panels multiple style="padding: 15px !important;" v-model="panel">
      <v-expansion-panel style="margin-bottom: 10px !important;" v-for="(date,index) in allData" :key="index">
        <v-expansion-panel-header style="background: #28b89c;">{{ date.DATE }} <v-spacer></v-spacer>
          日营业额：${{date.DATE_PRICE}}</v-expansion-panel-header>
        <v-expansion-panel-content>
          <v-list two-line style="padding: 0;">
            <template v-for="(order,index) in date.ORDER">
              <v-list-item :key="order.ORDER_ID" style=" padding: 0!important;" @click="showOrder(order)">
                <v-list-item-avatar color="grey darken-1">
                  <p style="color:#fff;line-height: 40px;margin: 0;">
                    {{order.DESK_NUM }}桌
                  </p>
                </v-list-item-avatar>
                <v-list-item-content>
                  <v-list-item-title>${{order.ALL_PRICE }}</v-list-item-title>
                  <v-list-item-subtitle class="text-subtitle-1">
                    {{order.FOOD_ALL_NAME}}
                  </v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>
              <v-divider v-if="index< date.ORDER.length-1" :key="`divider-${order.ORDER_ID}`" inset></v-divider>
            </template>
          </v-list>
        </v-expansion-panel-content>
      </v-expansion-panel>
    </v-expansion-panels>
  </div>
</template>
 <style  scoped>
.v-expansion-panel-content__wrap {
  padding: 0 !important;
}
</style>
<script>
export default {
  data: () => ({
    allData: [],
    panel: [0],
    shopInfo: {},
  }),

  mounted() {
    this.shopInfo = this.$fw.getJsonInfo("shopInfo");
    this.getOrderData();
  },
  methods: {
    showOrder(order) {
      this.$fw.saveJsonInfo("curOrder", order);
      this.$router.push({ path: "/orderDetail" });
    },
    getOrderData() {
      let self = this;
      self
        .$http("get", "/api/Product/GetOrders?account=" + self.shopInfo.ACCOUNT)
        .then((response) => {
          if (!response.success) {
            self.$message.error(response.message.content);
            return;
          }
          response.data.data.forEach((element) => {
            var DATE_PRICE = 0;
            element.DATE = self.$moment(element.DATE).format("YYYY-MM-DD");
            element.ORDER.forEach((order) => {
              var FOOD_ALL_NAME = "";
              var ALL_PRICE = 0;
              order.FOODS.forEach((food) => {
                FOOD_ALL_NAME += food.FOOD_DETAIL_NAME + ".";
                ALL_PRICE += food.PRICE * food.QTY;
              });
              order.FOOD_ALL_NAME = FOOD_ALL_NAME;
              ALL_PRICE += order.COST * order.PERSON_NUM;
              order.ALL_PRICE = ALL_PRICE;
              DATE_PRICE += ALL_PRICE;
              order.ORDER_DATE = self
                .$moment(order.ORDER_DATE)
                .format("YYYY-MM-DD HH:MM:SS");
            });
            element.DATE_PRICE = DATE_PRICE;
          });
          self.allData = response.data.data;
        });
    },
    closeAllOrder() {
      let self = this;
      self.$dialog.confirm("确认关闭所有订单？").then(function (dialog) {
        self.$http("get", "api/Product/CloseAllOrders").then((response) => {
          if (!response.success) {
            self.$message.error(response.message.content);
            return;
          }
          self.$message.success("操作成功");
           self.getOrderData();
        });
      });
    },
    clearPrint() {
      let self = this;
      self.$dialog.confirm("确认清空待打印列表？").then(function (dialog) {
        self
          .$http(
            "get",
            "api/Printer/ClearPrintStatus?account=" + self.shopInfo.ACCOUNT
          )
          .then((response) => {
            if (!response.success) {
              self.$message.error(response.message.content);
              return;
            }
            self.$message.success("操作成功");
              self.getOrderData();
          });
      });
    },
  },
};
</script>

