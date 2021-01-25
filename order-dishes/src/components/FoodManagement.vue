<template>
  <v-container style="height: 100%">
    <v-card style="height: 100%;min-height:1000px;">
      <v-tabs vertical style="height: 100%">
        <template v-for="item in types">
          <v-tab :key="item.TYPE_ID">{{ item.TYPE_NAME }}</v-tab>
          <v-tab-item :key="'item' + item.TYPE_ID">
            <template  v-for="food in item.FOODS">
              <v-card flat :key="food.FOOD_ID">
                <v-card-text>
                  <v-card outlined>
                    <v-list-item three-line style="padding: 0 0 0 16px">
                      <v-list-item-avatar tile size="75">
                        <v-img :src="food.Urls[0].URL"></v-img>
                      </v-list-item-avatar>
                      <v-list-item-content>
                        <p>{{ food.FOOD_NAME }}</p>
                        <v-chip-group mandatory v-show="food.FOOD_DETAIL.length > 1" active-class="primary--text"
                          @change="ChangeFoodDetailSelect(food,$event)" column v-model="food.SelectDetailIndex">
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
                          <v-icon color="primary" @click="EditFood(true)">
                            mdi-pencil-outline</v-icon>
                          <v-spacer></v-spacer>
                          <!-- <v-btn v-if="food.NUM > 0" icon>
                            <v-icon dense color="primary" @click="changeNum(food, -1)">
                              mdi-minus-circle-outline</v-icon>
                          </v-btn>
                          <div style="width: 30px" v-if="food.NUM > 0">
                            <v-text-field type="number" readonly v-model="food.NUM" style="padding-top: 0" />
                          </div>
                          <v-btn icon>
                            <v-icon dense color="primary" @click="changeNum(food, 1)">
                              mdi-plus-circle-outline
                            </v-icon>
                          </v-btn> -->
                        </div>
                      </v-list-item-content>
                    </v-list-item>
                  </v-card>
                </v-card-text>
              </v-card>
            </template>
            <div style="text-align: center;margin: 10px 0;">
              <v-icon color="primary" @click="EditFood(false)">
                mdi-plus-circle-outline
              </v-icon>
            </div>
          </v-tab-item>
        </template>
        <div key="addTypeId" @click="isShowAddType = true">
            <v-icon color="primary">
              mdi-plus-circle-outline
            </v-icon>
          </div>
      </v-tabs>
    </v-card>
    <v-dialog persistent v-model="isShowAddType">
      <template>
        <v-card>
          <v-toolbar color="primary" dark>请输入类别名称</v-toolbar>
          <v-card-text>
            <v-text-field style="margin-right: 16px;" prepend-icon="mdi-account-supervisor" v-model="newTypeName"
              color="purple darken-2" required></v-text-field>
          </v-card-text>
          <v-card-actions class="justify-end">
            <v-row>
              <v-col cols="6">
                <v-btn block @click="AddType" color="primary" :disabled="newTypeName==''">新增 </v-btn>
              </v-col>
              <v-col cols="6">
                <v-btn block @click="isShowAddType = false" color="primary">取消 </v-btn>
              </v-col>
            </v-row>
          </v-card-actions>
        </v-card>
      </template>
    </v-dialog>
  </v-container>
</template>
<script>
export default {
  data: () => ({
    shopName: "",
    account: "",
    types: [],
    isShowAddType: false,
    newTypeName: "",
  }),
  mounted() {
    let self = this;
    self.urlParam = this.$route.params;
    if (self.urlParam.account != null) {
      self.account = self.urlParam.account;
    }
    self.GetFoodData();
  },
  methods: {
    AddType: function () {
      let self = this;
      self.isShowAddType = false;
      var data = {
        ICON: "",
        TYPE_NAME: self.newTypeName,
      };
      self.$http("post", "/api/Product/AddType", data).then((response) => {
        self.$message.success("类别新增成功");
        self.GetFoodData();
        self.newTypeName = "";
      });
    },
    GetFoodData: function () {
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
            self.shopName = response.data.SHOP_NAME;
            self.types = response.data.TYPES;
            self.types.forEach((type) => {
              type.FOODS.forEach((food) => {
                food.NUM = 0;
                food.SelectDetailIndex = 0;
                food.SelectDetail = food.FOOD_DETAIL[0];
              });
            });
          }
        })
        .catch((err) => {
          console.log(err.message);
        });
    },
    EditFood: function (isEdit) {},
    ChangeFoodDetailSelect: function (food, index) {
      if (typeof index != "undefined") {
        food.SelectDetail = food.FOOD_DETAIL[index];
      } else {
        food.SelectDetail = null;
      }
      this.$forceUpdate();
    },
  },
};
</script>