<template>
  <div id="app">
    <v-card>
      <v-app-bar fixed color="white" elevate-on-scroll>
        <v-app-bar-nav-icon></v-app-bar-nav-icon>
        <v-toolbar-title>周洛御景山庄</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-toolbar-title>
          <v-btn @click="SelectPersonQty()" color="primary">
            {{ PERSON_QTY }}人
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
            <v-main>
              <v-container fluid style="height: 100%">
                <v-card style="height: 100%">
                  <v-tabs vertical>
                    <template v-for="item in foodList">
                      <v-tab :key="item.TYPE_ID">{{ item.TYPE_NAME }}</v-tab>
                      <v-tab-item :key="'item' + item.TYPE_ID" transition="slide-x-transition">
                        <template v-for="food in item.FOODS">
                          <v-card flat :key="food.FOOD_ID">
                            <v-card-text>
                              <v-card outlined>
                                <v-list-item three-line style="padding: 0 0 0 16px">
                                  <v-list-item-avatar tile size="75">
                                    <v-img :src="food[0].FOOD_DETAIL[0].Urls[0].URL"></v-img>
                                  </v-list-item-avatar>
                                  <v-list-item-content>
                                    <p>{{ food.FOOD_NAME }}</p>
                                    <div class="d-flex" style="height: 40px">
                                      <p class="red--text">${{ food[0].DETAIL_PRICE }}</p>
                                      <v-spacer></v-spacer>
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
                                    </div>
                                  </v-list-item-content>
                                </v-list-item>
                              </v-card>
                            </v-card-text>
                          </v-card>
                        </template>
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
              <v-btn to="/food" color="primary" style="float: right;" class="ml-auto" elevation="1">结算</v-btn>
              <!-- <v-btn @change="showCar" color="primary" style="float: right;" class="ml-auto" elevation="1">结算</v-btn> -->
              <v-bottom-sheet v-model="sheet" scrollable>
                <v-sheet class="text-center">
                  <v-card>
                    <v-toolbar color="primary">
                      <v-app-bar-nav-icon></v-app-bar-nav-icon>
                      <v-toolbar-title>购物车</v-toolbar-title>
                      <v-spacer></v-spacer>
                      <v-btn @click="sheet = !sheet" icon>
                        <v-icon>mdi-close-circle-outline</v-icon>
                      </v-btn>
                    </v-toolbar>
                    <v-card-text style="height: 400px; overflow: scroll">
                      <v-list dense>
                        <template v-for="item in itemsCar">
                          <v-list-item :key="item.ID" v-if="item.NUM > 0">
                            <v-list-item-avatar tile size="40">
                              <v-img src="https://cdn.vuetifyjs.com/images/john.png"></v-img>
                            </v-list-item-avatar>
                            <v-list-item-content>
                              <v-list-item-title v-text="item.NAME"></v-list-item-title>
                            </v-list-item-content>
                            <v-spacer></v-spacer>
                            <v-list-item-content>
                              <v-list-item-action style="margin: 0">
                                <div class="d-flex">
                                  <v-btn icon v-if="item.NUM > 0">
                                    <v-icon color="primary" @click="item.NUM -= 1">mdi-minus-circle-outline</v-icon>
                                  </v-btn>
                                  <div style="width: 30px" v-if="item.NUM > 0">
                                    <v-text-field type="number" readonly v-model="item.NUM"
                                      style="padding-top: 0; text-align: center" />
                                  </div>
                                  <v-btn icon>
                                    <v-icon color="primary" @click="item.NUM += 1">
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
                    <v-btn color="primary" @click="sheet = !sheet">
                      选好了
                    </v-btn>
                  </v-row>
                </v-sheet>
              </v-bottom-sheet>
            </v-footer>
          </v-app>
        </v-container>
      </v-sheet>
    </v-card>

    <v-dialog persistent v-model="dialog" transition="dialog-top-transition">
      <template v-slot:default="dialog">
        <v-card>
          <v-toolbar color="primary" dark>请输入用餐人数</v-toolbar>
          <v-card-text>
            <v-text-field prepend-icon="mdi-account-supervisor" :rules="[ value => (value >= 0) || '需大于0',]"
              type="number" v-model="PERSON_QTY" color="purple darken-2" required></v-text-field>
          </v-card-text>
          <v-card-actions class="justify-end">
            <v-btn block @click="dialog.value = false" color="primary" :disabled="PERSON_QTY==''||PERSON_QTY<0">确认
            </v-btn>
          </v-card-actions>
        </v-card>
      </template>
    </v-dialog>
  </div>
</template>
 
<script>
// import axios from "axios";
export default {
  name: "Food",
  data: () => ({
    SHOP_NAME: "周洛御景山庄",
    tab: null,
    PERSON_QTY: 2,
    num: 10,
    sheet: false,
    dialog: false,
    foodList: [
      {
        TYPE_ID: "164D146E775E49E7B627F8FEFCAEE19D",
        ICON: null,
        TYPE_NAME: "类别1",
        FOODS: [
          {
            FOOD_ID: "ACC0C23408464E5F991E4A4088003CFD",
            FOOD_TAG: null,
            FOOD_NAME: "食物A",
            FOOD_IMG_ID: null,
            FOOD_DETAIL: [
              {
                DETAIL_ID: "5D622FEC91D7436FAC0A7A2A274D1D52",
                DETAIL_NAME: "食物A_1",
                DETAIL_DESC: "无",
                DETAIL_PRICE: 100,
                Urls: [
                  {
                    URL:
                      "https://www.baidu.com/img/PCtm_d9c8750bed0b3c7d089fa7d55720d6cf.png",
                  },
                ],
              },
            ],
          },
        ],
      },
    ],
    items: [
      // {
      //   src: "https://cdn.vuetifyjs.com/images/carousel/squirrel.jpg",
      // },
      // {
      //   src: "https://cdn.vuetifyjs.com/images/carousel/sky.jpg",
      // },
      // {
      //   src: "https://cdn.vuetifyjs.com/images/carousel/bird.jpg",
      // },
      // {
      //   src: "https://cdn.vuetifyjs.com/images/carousel/planet.jpg",
      // },
    ],
    itemsCar: [],
    carNum: 0,
  }),
  watch: {
    sheet: function (newValue, oldValue) {
      this.carNum = 0;
      this.foodList.forEach((element) => {
        element.foods.forEach((food) => {
          if (food.NUM > 0) this.carNum++;
        });
      });
    },
  },
  mounted() {
    let self = this;
    document.title = this.SHOP_NAME;
    axios
      .get(
        "http://www.trastor.cn:4396/api/product/GetProductList?account=test01"
      )
      .then((response) => {
        console.log(response);
        if (response.data.success == true) {
          self.foodList = response.data.data;
          self.SHOP_NAME = response.data.SHOP_NAME;
          self.foodList = response.data.data.TYPES;
        }
      });
  },
  methods: {
    showCar: function () {
      this.sheet = true;
      this.itemsCar = [];
      this.foodList.forEach((element) => {
        element.foods.forEach((food) => {
          if (food.NUM > 0) this.itemsCar.push(food);
        });
      });
      this.$store.commit("mutationsChangeFood", this.itemsCar);
    },
    changeNum: function (food, num) {
      food.NUM += num;
      this.carNum = 0;
      this.foodList.forEach((element) => {
        element.foods.forEach((food) => {
          if (food.NUM > 0) this.carNum++;
        });
      });
    },
    SelectPersonQty: function () {
      this.dialog = true;
    },
  },
};
</script>
