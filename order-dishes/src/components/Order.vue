<template>
  <div id="app">
    <div style=" text-align:center;margin:20px;">
      <p>{{shopName}} {{descNum}}  {{personNum}}人</p>
      <p class="display-1 text--primary">
        <v-icon color="primary" large>
          mdi-check-circle
        </v-icon>下单成功！
      </p>
      <div class="text--primary">
        商家正在备菜中....
      </div>
    </div>
    <v-card class="mx-auto" max-width="344">
      <v-card-text>

        <template v-for="food in itemsCar">
          <v-list-item :key="food.SelectDetail.DETAIL_ID" v-if="food.NUM > 0">
            <v-list-item-avatar tile size="40">
              <v-img :src="food.Urls[0].URL"></v-img>
            </v-list-item-avatar>
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
      <!-- <v-card-actions>
     
      </v-card-actions> -->
    </v-card>
    <div style="text-align:center;padding:8px">
      <v-btn style="width:100%" color="primary" @click="toFood()">
        继续点餐
      </v-btn>
    </div>
  </div>
</template>
<script>
export default {
  name: "Order",
  data: () => ({
    shopName: "",
    personNum: 0,
    itemsCar:[],
    descNum:0,
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
