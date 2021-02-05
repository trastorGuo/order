<template>
  <div id="app">
    <v-card>
      <v-app-bar fixed color="primary" elevate-on-scroll>
        <v-app-bar-nav-icon color="white">
          <!-- <v-icon color="white">mdi-bowl-mix</v-icon> -->
        </v-app-bar-nav-icon>
        <v-toolbar-title style="color: white;">{{shopName +'(第'+descNum+'桌)'}}</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-toolbar-title>
          <v-btn @click="SelectPersonQty()" color="primary">
            {{ personNum }}人
          </v-btn>
        </v-toolbar-title>
      </v-app-bar>
      <v-sheet style="padding-top: 50px;">
        <v-container>
          <v-app>
            <v-carousel v-show="items.length > 0" height="180px" cycle :interval="3000" hide-delimiters
              :show-arrows="false">
              <v-carousel-item v-for="(item, i) in items" :key="i" :src="item.src"></v-carousel-item>
            </v-carousel>
            <v-main style="padding-top:5px;">
              <v-container fluid style="height: 100%">
                <v-card style="height: 100%">
                  <v-tabs vertical>
                    <template v-for="item in types">
                      <v-tab :key="item.TYPE_ID">{{ item.TYPE_NAME }}</v-tab>
                      <v-tab-item :key="'item' + item.TYPE_ID">
                        <template v-for="food in item.FOODS">
                          <template v-if="food.VISIBLE=='Y'">
                            <v-card flat :key="food.FOOD_ID">
                              <v-card-text>
                                <v-card outlined>
                                  <v-list-item three-line style="padding: 0 0 0 16px">
                                    <v-list-item-avatar tile size="75"  @click="showImg(food.Urls)">
                                      <v-img v-if="food.Urls.length > 0" :src="food.Urls[0].URL"></v-img>
                                      <!-- <v-img v-if="food.Urls[0].URL!=null" :src="food.Urls[0].URL"></v-img> -->
                                      <v-img v-else style="background: lightgrey;">
                                        <div style="width:100%;line-height: 75px;">
                                          {{ food.FOOD_NAME }}
                                        </div>
                                      </v-img>
                                    </v-list-item-avatar>
                                    <v-list-item-content>
                                      <p>{{ food.FOOD_NAME }}</p>
                                      <v-chip-group mandatory v-show="food.FOOD_DETAIL.length > 1"
                                        active-class="primary--text" @change="ChangeFoodDetailSelect(food,$event)"
                                        column v-model="food.SelectDetailIndex">
                                        <template v-for="detail in food.FOOD_DETAIL">
                                          <v-chip x-small :key="detail.DETAIL_ID">
                                            {{detail.DETAIL_NAME}}
                                          </v-chip>
                                        </template>
                                      </v-chip-group>
                                      <div class="d-flex" style="height: 40px">
                                        <p style="line-height: 40px;" class="primary--text">
                                          ${{ food.SelectDetail.DETAIL_PRICE}}</p>
                                        <v-spacer></v-spacer>
                                        <div v-if="food.INVENTORY==0" style="margin-right: 20px;">
                                          <v-avatar color="white" size="50">
                                            <span class="grey--text headline">售罄</span>
                                          </v-avatar>
                                        </div>
                                        <template v-else>
                                          <v-btn v-if="food.NUM > 0" icon>
                                            <v-icon dense color="primary" @click="changeNum(food, -1)">
                                              mdi-minus-circle-outline</v-icon>
                                          </v-btn>
                                          <div style="width: 30px" v-if="food.NUM > 0">
                                            <v-text-field type="number" readonly v-model="food.NUM"
                                              style="padding-top: 0" />
                                          </div>
                                          <v-btn icon>
                                            <v-icon dense color="primary" @click="changeNum(food, 1)">
                                              mdi-plus-circle-outline
                                            </v-icon>
                                          </v-btn>
                                        </template>

                                      </div>
                                    </v-list-item-content>
                                  </v-list-item>
                                </v-card>
                              </v-card-text>
                            </v-card>
                          </template>
                          </template?>
                      </v-tab-item>
                    </template>
                  </v-tabs>
                </v-card>
              </v-container>
            </v-main>
            <v-footer app class="d-flex" style="margin-bottom: 0 !important">
              <v-badge color="primary" :content="carNum" outlined :value="carNum > 0">
                <v-icon size="34" color="primary" @click="showCar()" content="6">
                  mdi-cart-outline
                </v-icon>
              </v-badge>
              <v-btn @click="showCar()" :disabled="carNum <= 0" color="primary" style="float: right;" class="ml-auto"
                elevation="1">结算</v-btn>
              <v-bottom-sheet v-model="sheet" scrollable>
                <v-sheet class="text-center">
                  <v-card>
                    <v-toolbar color="primary">
                      <v-app-bar-nav-icon>
                        <v-icon color="white">
                          mdi-cart-outline
                        </v-icon>
                      </v-app-bar-nav-icon>
                      <v-toolbar-title style="color: white;">购物车</v-toolbar-title>
                      <v-spacer></v-spacer>
                      <v-btn @click="sheet = !sheet" icon>
                        <v-icon color="white">mdi-close-circle-outline</v-icon>
                      </v-btn>
                    </v-toolbar>
                    <v-card-text style="height: 400px; overflow: scroll">
                      <v-list dense>
                        <template v-for="food in itemsCar">
                          <v-list-item :key="food.FOOD_ID" v-if="food.NUM > 0">
                            <v-list-item-avatar tile size="40">
                              <v-img v-if="food.Urls.length > 0" :src="food.Urls[0].URL"></v-img>
                              <!-- <v-img v-if="food.Urls[0].URL!=null" :src="food.Urls[0].URL"></v-img> -->
                              <v-img v-else style="background: lightgrey;">
                                <div style="width:100%;line-height: 40px;">
                                  {{ food.FOOD_NAME }}
                                </div>
                              </v-img>
                            </v-list-item-avatar>
                            <v-list-item-content>
                              <v-list-item-title>
                                {{food.FOOD_NAME}}
                              </v-list-item-title>
                              <v-list-item-subtitle v-if="food.FOOD_DETAIL.length > 1">
                                {{food.SelectDetail.DETAIL_NAME }}</v-list-item-subtitle>
                            </v-list-item-content>
                            <v-spacer></v-spacer>
                            <v-list-item-content>
                              <v-list-item-action style="margin: 0">
                                <div class="d-flex">
                                  <v-btn icon v-if="food.NUM > 0">
                                    <v-icon color="primary" @click="changeCarNum(food, -1)">mdi-minus-circle-outline
                                    </v-icon>
                                  </v-btn>
                                  <div style="width: 30px" v-if="food.NUM > 0">
                                    <v-text-field type="number" readonly v-model="food.NUM"
                                      style="padding-top: 0; text-align: center" />
                                  </div>
                                  <v-btn icon>
                                    <v-icon color="primary" @click="changeCarNum(food, +1)">
                                      mdi-plus-circle-outline
                                    </v-icon>
                                  </v-btn>
                                </div>
                              </v-list-item-action>
                            </v-list-item-content>
                          </v-list-item>
                          <v-divider></v-divider>
                        </template>
                      </v-list>
                    </v-card-text>
                  </v-card>
                  <v-row style="margin: 8px 0px" align="center" justify="space-around">
                    <v-btn color="primary" @click="sheet = !sheet">
                      再选选
                    </v-btn>
                    <v-btn color="primary" @click="commit()" :disabled="itemsCar.length <= 0">
                      下单
                    </v-btn>
                  </v-row>
                </v-sheet>
              </v-bottom-sheet>
            </v-footer>
          </v-app>
        </v-container>
      </v-sheet>
    </v-card>

    <v-dialog persistent v-model="dialog">
      <template v-slot:default="dialog">
        <v-card>
          <v-toolbar color="primary" dark>请输入用餐人数</v-toolbar>
          <v-card-text>
            <v-text-field style="margin-right: 16px;" prepend-icon="mdi-account-supervisor"
              :rules="[ value => (value > 0) || '需大于0',]" type="number" v-model="personNum" color="purple darken-2"
              required></v-text-field>
          </v-card-text>
          <v-card-actions class="justify-end">
            <v-row>
              <!-- <v-col cols="6">
                <v-btn block @click="dialog.value = false" color="primary" :disabled="personNum==''||personNum<0">加菜
                </v-btn>
              </v-col>
              <v-col cols="6">
                <v-btn block @click="dialog.value = false" color="primary" :disabled="personNum==''||personNum<0">新开一单
                </v-btn>
              </v-col> -->
               <v-col cols="12">
                <v-btn block @click="dialog.value = false" color="primary" :disabled="personNum==''||personNum<0">确认
                </v-btn>
              </v-col>
            </v-row>
          </v-card-actions>
        </v-card>
      </template>
    </v-dialog>
    <v-overlay :value="overlay" style="text-align:center">
      <v-img @click="overlay = false" style="width: 25em;" :src="curImg"></v-img>
      <v-btn outlined large style="margin-top:10px;" fab @click="overlay = false">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-overlay>
  </div>
</template>
 
<script>
export default {
  name: "food",
  data: () => ({
    shopName: "",
    account: "",
    descNum: "0",
    urlParam: "",
    tab: null,
    personNum: 0,
    sheet: false,
    dialog: true,
    types: [],
    items: [],
    itemsCar: [],
    carNum: 0,
    curOrderId:"",
     curImg: "",
    overlay: false,
  }),
  watch: {
    sheet: function (newValue, oldValue) {
      this.carNum = 0;
      this.types.forEach((type) => {
        type.FOODS.forEach((food) => {
          if (food.NUM > 0) this.carNum++;
        });
      });
    },
    shopName: function (newValue, oldValue) {
      document.title = newValue;
      this.$store.commit("mutationsChangeShopName", newValue);
    },
    personNum: function (newValue, oldValue) {
      this.$store.commit("mutationsChangePersonNum", newValue);
      this.$data.personNum = Number(newValue);
    },
    descNum: function (newValue, oldValue) {
      this.$store.commit("mutationsChangeDescNum", newValue);
    },
    account: function (newValue, oldValue) {
      this.$store.commit("mutationsChangeAccount", newValue);
    },
  },
  mounted() {
    let self = this;
    self.urlParam = this.$route.params;
    if (self.urlParam.account != null) {
      self.account = self.urlParam.account;
      self.descNum = self.urlParam.descnum;
      self.personNum = self.urlParam.personNum;
    }
    if (self.personNum) {
      //加菜
      self.dialog = false;
    }
    self
      .$http(
        "get",
        "/api/Product/DeskIsFree?desckNum=" +
          self.descNum +
          "&shopAcount=" +
          self.account
      )
      .then((response) => {
        if (!response.success) {
          self.$message.error(response.message.content);
          return;
        }
        if (response.data != true) {
          //存数据
          self.curOrderId = response.data;
          self.$fw.saveJsonInfo("curOrderId", self.curOrderId);
          if (!self.personNum) {
            self.$router.replace({
              path: "/OrderSuccess" + "/" + self.account + "/" + self.descNum,
            });
          }
          else{
            self.personNum = "";
          }
        }
      });
    self.getFoodData();
    // this.$vuetify.theme.themes.light.primary = "#000";//修改主题颜色
  },
  methods: {
     showImg(Urls) {
      if (Urls.length > 0) {
        this.curImg = Urls[0].URL;
        this.overlay = true;
      }
    },
    showCar: function () {
      let self = this;
      self.sheet = true;
      self.itemsCar = [];
      self.types.forEach((type) => {
        type.FOODS.forEach((food) => {
          if (food.NUM > 0) self.itemsCar.push(food);
        });
      });
      this.$store.commit("mutationsChangeCar", this.itemsCar);
    },
    getFoodData() {
      let self = this;
      self
        .$http("get", "/api/product/GetProductList?account=" + self.account)
        .then((response) => {
          console.log(response);
          if (response != null) {
            if (!response.success) {
              self.$message.error(response.message.content);
              return;
            }
            self.types = [];
            response.data.TYPES.forEach((type) => {
              if (
                type.FOODS.length > 0 &&
                type.FOODS.filter((x) => x.VISIBLE == "Y").length > 0
              ) {
                type.FOODS.forEach((food) => {
                  food.NUM = 0;
                  food.SelectDetailIndex = 0;
                  food.SelectDetail = food.FOOD_DETAIL[0];
                });
                self.types.push(type);
              }
            });
            self.shopName = response.data.SHOP_NAME;
          }
        })
        .catch((err) => {
          console.log(err.message);
        });
    },
    changeNum: function (food, num) {
      let self = this;
      food.NUM += num;
      self.carNum = 0;
      self.types.forEach((type) => {
        type.FOODS.forEach((food) => {
          if (food.NUM > 0) self.carNum++;
        });
      });
    },
    changeCarNum: function (food, num) {
      let self = this;
      food.NUM += num;
      this.$forceUpdate();
    },
    SelectPersonQty: function () {
      this.dialog = true;
    },
    ChangeFoodDetailSelect: function (food, index) {
      if (typeof index != "undefined") {
        food.SelectDetail = food.FOOD_DETAIL[index];
      } else {
        food.SelectDetail = null;
      }
      this.$forceUpdate();
    },
    commit: function () {
      let self = this;
      var foods = [];
      self.itemsCar.forEach((x) => {
        foods.push({
          DETAIL_ID: x.SelectDetail.DETAIL_ID,
          NUM: x.NUM,
        });
      });
      var data = {
        Foods: foods,
        OrderId: self.curOrderId,
        Account: self.account,
        User: "god",
        IsPrint: "Y",
        DescNum: self.descNum,
        PersonNum: self.personNum,
      };
      self
        .$http("post", "/api/Product/PlaceAnOrder", data)
        .then((response) => {
          if (!response.success) {
            self.$message.error(response.message.content);
            return;
          }
           self.$fw.saveJsonInfo("curOrderId",response.data.OrderId);
          self.$message.success("您已成功下单");
          self.$router.replace({
            path: "/OrderSuccess" + "/" + self.account + "/" + self.descNum,
          });
        })
        .catch((err) => {
          self.$message.success(err);
          console.log(err.message);
        });
    },
  },
};
</script>
