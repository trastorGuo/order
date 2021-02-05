<template>
  <div id="app" style=" width: 100%;">
    <v-card class="mx-auto" style="margin: 20px!important;">
      <v-card-text style="padding-bottom: 20px!important;">
        <v-form ref="form" lazy-validation style="padding: 20px;">
          <v-text-field outlined dense rounded v-model="type.TYPE_NAME" label="类别" disabled></v-text-field>
          <v-text-field outlined dense rounded v-model="food.FOOD_NAME" label="商品名称"></v-text-field>
          <v-switch style="padding:0;margin:0;" v-model="isBigInventory" label="无限库存" inset>
          </v-switch>
          <v-text-field outlined dense rounded v-if="!isBigInventory" v-model="food.INVENTORY" type="number" label="库存">
          </v-text-field>
          <v-switch style="padding:0;margin:0;" v-model="food.VISIBLE" label="是否展示" inset>
          </v-switch>
          <v-switch style="padding:0;margin:0;" @change="changeHasDetail()" v-model="hasDetail" label="是否有多项价格" inset>
          </v-switch>
          <v-text-field outlined dense rounded v-if="!hasDetail" v-model="food.PRICE" label="价格" type="number">
          </v-text-field>
          <v-row v-for="(detail,index) in food.FOOD_DETAIL" :key="detail.index">
            <v-col cols="6">
              <v-text-field rounded outlined dense v-model="detail.DETAIL_NAME" label="项名称"></v-text-field>
            </v-col>
            <v-col cols="5">
              <v-text-field rounded outlined dense v-model="detail.DETAIL_PRICE" label="项价格" type="number">
              </v-text-field>
            </v-col>
            <v-col cols="1" style="line-height: 40px;">
              <v-icon v-if="index > 1" color="primary" @click="food.FOOD_DETAIL.splice(index,1)">
                mdi-delete-outline
              </v-icon>
              <v-icon v-if="index == 0" color="primary" @click="addDetail()">
                mdi-plus-circle-outline
              </v-icon>
            </v-col>
          </v-row>
          <file-upload ref="upload" v-model="files" @input-file="inputFile" @input-filter="inputFilter">
            <v-icon large>mdi-camera</v-icon>
            上传图片
          </file-upload>
          <v-overlay :value="isLoading">
            <v-progress-circular indeterminate size="64"></v-progress-circular>
          </v-overlay>
          <v-img style="width: 100px; height: 100px;margin: 0 auto;margin-bottom:10px;overflow: visible;"
            v-for="(URL,index) in food.Urls" :src="URL.URL" :key="URL.URL">
            <v-fab-transition>
              <v-btn style="right: 0;" fab x-small absolute top right @click="deleteImg(food,index)">
                <v-icon>mdi-close</v-icon>
              </v-btn>
            </v-fab-transition>
          </v-img>
          <!-- <button v-show="!$refs.upload || !$refs.upload.active" @click.prevent="$refs.upload.active = true"
            type="button">开始上传</button>
          <button v-show="$refs.upload && $refs.upload.active" @click.prevent="$refs.upload.active = false"
            type="button">停止上传</button> -->
          <v-btn style="width:100%;margin:8px 0; " color="#73ce5f" rounded dark @click="Save()">
            保存
          </v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </div>
</template>
 <<style  scoped>
 .col{
   padding:0;
 }
 </style>
<script>
export default {
  data: () => ({
    files: [],
    food: {
      FOOD_NAME: "",
      FOOD_DETAIL: [],
      Urls: [],
      PRICE: null,
      VISIBLE: true,
      INVENTORY: null,
      DETAIL_ID: "",
    },
    isAdd: true,
    type: {},
    hasDetail: false,
    isLoading: false,
    isBigInventory: true,
  }),
  mounted() {
    let curFood = this.$fw.getJsonInfo("curFood");
    this.type = curFood.type;
    if (
      curFood.food != null &&
      curFood.food != ""
    ) {
      this.isAdd = false;
      this.food = curFood.food;
      if (this.food.FOOD_DETAIL.length == 1) {
        this.food.PRICE = this.food.FOOD_DETAIL[0].DETAIL_PRICE;
        this.food.DETAIL_ID = this.food.FOOD_DETAIL[0].DETAIL_ID;
        this.food.FOOD_DETAIL = [];
        this.food.VISIBLE = this.food.VISIBLE == "Y" ? true : false;
      } else if (this.food.FOOD_DETAIL.length > 1) {
        this.hasDetail = true;
      }
      if (this.food.INVENTORY != -1) {
        this.isBigInventory = false;
      }
    }
  },
  methods: {
    Save() {
      let self = this;
      if (self.food.FOOD_NAME == "") {
        this.$message.error("商品名称不能为空");
        return;
      }
      for (let i = 0; i < self.food.FOOD_DETAIL.length; i++) {
        if (self.food.FOOD_DETAIL[i].DETAIL_PRICE === "") {
          this.$message.error("项价格不能为空");
          return;
        }
        if (self.food.FOOD_DETAIL[i].DETAIL_NAME === "") {
          this.$message.error("项名称不能为空");
          return;
        }
      }
      var data = {
        Type: JSON.parse(JSON.stringify(self.type)),
        Food: JSON.parse(JSON.stringify(self.food)),
      };
      if (!this.hasDetail) {
        if (data.Food.PRICE == "" || data.Food.PRICE ==null) {
          this.$message.error("价格不能为空");
          return;
        }
        data.Food.FOOD_DETAIL = [];
        data.Food.FOOD_DETAIL.push({
          DETAIL_NAME: "",
          DETAIL_ID: data.Food.DETAIL_ID,
          DETAIL_PRICE: data.Food.PRICE,
          Urls: [],
        });
      }
      data.Type.FOODS = [];
      data.Food.VISIBLE = data.Food.VISIBLE ? "Y" : "N";
      data.Food.INVENTORY = self.isBigInventory ? -1 : data.Food.INVENTORY;
      self
        .$http(
          "post",
          "/api/Product/" + (self.isAdd ? "AddProduct" : "EditProduct"),
          data
        )
        .then((response) => {
          if (!response.success) {
            self.$message.error(response.message.content);
            return;
          }
          self.$message.success(self.isAdd ? "商品新增成功" : "商品修改成功");
          // self.$router.replace({path:""});
          self.$router.go(-1);
        });
    },
    changeHasDetail() {
      if (this.hasDetail) {
        this.food.FOOD_DETAIL.push({
          DETAIL_NAME: "",
          DETAIL_PRICE: "",
          Urls: [],
        });
      } else {
        this.food.FOOD_DETAIL = [];
      }
    },
    addDetail() {
      this.food.FOOD_DETAIL.push({
        DETAIL_NAME: "",
        DETAIL_PRICE: "",
        Urls: [],
      });
    },
    inputFile: function (newFile, oldFile) {
      let self = this;
      self.isLoading = true;
      self.$http("get", "/api/Qiniu/GetToken").then((response) => {
        var formData = new FormData();
        formData.append("file", newFile.file);
        formData.append("token", response.data);
        axios({
          headers: {
            "content-type": "multipart/form-data",
          },
          method: "post",
          url: "http://up-z2.qiniup.com",
          data: formData,
        }).then(function (response) {
          self.isLoading = false;
          if (response != null && response.key != null) {
            self.food.Urls.push({
              URL: "http://cdn.trastor.cn/" + response.key,
            });
          }
        });
      });
    },
    inputFilter: function (newFile, oldFile, prevent) {
      if (newFile && !oldFile) {
        // 过滤不是图片后缀的文件
        if (!/\.(jpeg|jpe|jpg|gif|png|webp)$/i.test(newFile.name)) {
          this.$message.error("请上传图片！");
          return prevent();
        }
      }
    },
    deleteImg(food, index) {
      food.Urls.splice(index, 1);
    },
  },
};
</script>
