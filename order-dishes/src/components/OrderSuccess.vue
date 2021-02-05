<template>
  <div id="app">
    <div style=" text-align:center;margin:20px;">
      <p>{{shopName}} {{descNum}} {{personNum}}人</p>
      <p class="display-1 text--primary">
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
        <template v-for="food in itemsCar">
          <v-list-item :key="food.SelectDetail.DETAIL_ID" v-if="food.NUM > 0">
            <v-list-item-avatar tile size="40">
              <v-img v-if="food.Urls.length > 0" :src="food.Urls[0].URL"></v-img>
              <v-img v-else style="background: lightgrey;">
                <div style="width:100%;line-height: 40px;">
                  {{ food.FOOD_NAME }}
                </div>
              </v-img>
            </v-list-item-avatar>
            <!-- <v-list-item-avatar tile size="40">
              <v-img :src="food.Urls[0].URL"></v-img>
            </v-list-item-avatar> -->
            <v-list-item-content>
              <v-list-item-title v-if="food.FOOD_DETAIL.length>1">
                {{food.FOOD_NAME}} - {{food.SelectDetail.DETAIL_NAME}}
              </v-list-item-title>
              <v-list-item-title v-else>
                {{food.FOOD_NAME}}
              </v-list-item-title>
            </v-list-item-content>
            <!-- <v-spacer></v-spacer> -->
            <v-list-item-content>
              <v-list-item-title style="text-align: center;" color="primary"> ${{food.SelectDetail.DETAIL_PRICE}} ×
                {{food.NUM }} </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-divider></v-divider>
        </template>
      </v-card-text>

    </v-card>
    <div style="text-align:center;padding:8px">
      <v-row>
        <v-col cols="6">
          <v-btn style="width:80%" color="primary" @click="toFood()">
            继续点餐
          </v-btn>
        </v-col>
        <v-col cols="6">
          <v-btn style="width:80%" color="primary" @click="toFood()">
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
    personNum: 0,
    itemsCar: [],
    descNum: 0,
  }),
  mounted() {
    this.itemsCar = this.$store.state.itemsCar;
    this.personNum = this.$store.state.personNum;
    this.shopName = this.$store.state.shopName;
    this.descNum = this.$store.state.descNum;
  },
  methods: {
    toFood() {
      this.$router.push({
        path:
          "/food/" +
          this.$store.state.account +
          "/" +
          this.$store.state.descNum,
      });
    },
  },
};
</script>
