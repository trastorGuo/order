<template>
  <v-container style="height: 100%">
    <v-card style="height: 100%;min-height:1000px;">
      <v-tabs vertical style="height: 100%" v-model="selectedType">
        <template v-for="item in types">
          <v-tab :key="item.TYPE_ID">{{ item.TYPE_NAME }}</v-tab>
          <v-tab-item :key="'item' + item.TYPE_ID">
            <template v-for="food in item.FOODS">
              <v-card flat :key="food.FOOD_ID">
                <v-card-text>
                  <v-card outlined>
                    <v-list-item three-line style="padding: 0 0 0 16px">
                      <v-list-item-avatar tile size="75" @click="showImg(food.Urls)">
                        <v-img v-if="food.Urls.length > 0" :src="food.Urls[0].URL"></v-img>
                        <v-img v-else style="background: lightgrey;">
                          <div style="width:100%;line-height: 75px;">
                            {{ food.FOOD_NAME }}
                          </div>
                        </v-img>
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
                          <v-icon color="primary" @click="EditFood(food)">
                            mdi-pencil-outline</v-icon>
                          <v-spacer></v-spacer>
                          <v-icon color="primary" @click="DelFood(food)">
                            mdi-delete-outline
                          </v-icon>
                          <v-spacer></v-spacer>
                        </div>
                      </v-list-item-content>
                    </v-list-item>
                  </v-card>
                </v-card-text>
              </v-card>
            </template>
            <div style="text-align: center;margin: 10px 0;">
              <v-icon color="primary" @click="EditFood(null)">
                mdi-plus-circle-outline
              </v-icon>
              <v-icon style="margin-left:10px;" v-if="item.FOODS.length==0" color="primary" @click="DelType(item)">
                mdi-delete-outline
              </v-icon>
            </div>
          </v-tab-item>
        </template>
        <div key="addTypeId" @click="isShowAddType = true" style="text-align:center;">
          <v-icon color="primary">
            mdi-plus-circle-outline
          </v-icon>
        </div>
      </v-tabs>
    </v-card>
    <v-dialog persistent v-model="isShowAddType">
      <template>
        <v-card>
          <v-toolbar color="primary" dark>类别</v-toolbar>
          <v-card-text>
            <v-text-field style="margin:  10px 16px 0 16px;" v-model="newTypeName" label="类别名称" outlined dense>
            </v-text-field>
            <v-text-field style="margin:  0px 16px ;" type="number" v-model="newSeq" label="序号" outlined dense>
            </v-text-field>
          </v-card-text>
          <v-card-actions class="justify-end">
            <v-row>
              <v-col cols="6">
                <v-btn block @click="AddType" color="primary" :disabled="newTypeName==''">确认 </v-btn>
              </v-col>
              <v-col cols="6">
                <v-btn block @click="isShowAddType = false" color="primary">取消 </v-btn>
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
  </v-container>
</template>
<script>
export default {
  data: () => ({
    curImg: "",
    overlay: false,
    selectedType: null,
    shopName: "",
    account: "",
    types: [],
    isShowAddType: false,
    newTypeName: "",
    newSeq: "",
  }),
  mounted() {
    let self = this;
    self.urlParam = this.$route.params;
    if (self.urlParam.account != null) {
      self.account = self.urlParam.account;
    }
    self.GetFoodData();
  },
  watch: {
    selectedType(newValue, oldValue) {
      this.$store.commit("mutationsChangeSelectedType", newValue);
    },
  },
  methods: {
    AddType() {
      let self = this;
      self.isShowAddType = false;
      var data = {
        SEQ: self.newSeq,
        ICON: "",
        TYPE_NAME: self.newTypeName,
      };
      self.$http("post", "/api/Product/AddType", data).then((response) => {
        self.newTypeName = "";
        if (!response.success) {
          self.$message.error(response.message.content);
          return;
        }
        self.$message.success("类别新增成功");
        self.GetFoodData();
      });
    },
    GetFoodData() {
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
            self.newSeq = self.types.length + 1;
            if (self.$store.state.selectedType > 0) {
              self.selectedType = self.$store.state.selectedType;
            }
          }
        })
        .catch((err) => {
          console.log(err.message);
        });
    },
    EditFood(food) {
      this.$fw.saveJsonInfo("curFood", {
        type: this.types[this.selectedType],
        food: food == null ? "" : food,
      });
      this.$router.push({
        path: "/AddFood",
      });
    },
    ChangeFoodDetailSelect(food, index) {
      if (typeof index != "undefined") {
        food.SelectDetail = food.FOOD_DETAIL[index];
      } else {
        food.SelectDetail = null;
      }
      this.$forceUpdate();
    },
    DelFood(food) {
      let self = this;
      self.$dialog
        .confirm("确认删除商品[" + food.FOOD_NAME + "]？")
        .then(function (dialog) {
          self
            .$http("get", "/api/Product/DeleteProduct?id=" + food.FOOD_ID)
            .then((response) => {
              if (!response.success) {
                self.$message.error(response.message.content);
                return;
              }
              self.$message.success("删除成功");
              self.GetFoodData();
            });
        });
    },
    DelType(type) {
      let self = this;
      if (type.FOODS.length > 0) {
        self.$message.error("如需删除分类，请先将该分类下的商品全部删除再操作");
      }
      self.$dialog
        .confirm("确认删除分类[" + type.TYPE_NAME + "]？")
        .then(function (dialog) {
          self
            .$http("get", "/api/Product/DeleteType?id=" + type.TYPE_ID)
            .then((response) => {
              if (!response.success) {
                self.$message.error(response.message.content);
                return;
              }
              self.$message.success("删除成功");
              self.GetFoodData();
            });
        });
    },
    showImg(Urls) {
      if (Urls.length > 0) {
        this.curImg = Urls[0].URL;
        this.overlay = true;
      }
    },
  },
};
</script>