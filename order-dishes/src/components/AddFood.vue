<template>
  <div id="app" style=" width: 100%;">
    <v-card class="mx-auto" style="margin: 20px!important;">
      <v-card-text style="padding-bottom: 20px!important;">
        <v-form ref="form" lazy-validation style="padding: 20px;">
          <v-text-field outlined dense rounded v-model="type.TYPE_NAME" label="类别" disabled></v-text-field>
          <v-text-field outlined dense rounded v-model="food.FOOD_NAME" label="商品名称"></v-text-field>
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
            上传文件
          </file-upload>
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
      PRICE: 0,
      DETAIL_ID: "",
    },
    isAdd: true,
    type: {},
    hasDetail: false,
  }),
  mounted() {
    this.type = this.$store.state.curFood.type;
    if (
      this.$store.state.curFood.food != null &&
      this.$store.state.curFood.food != ""
    ) {
      this.isAdd = false;
      this.food = this.$store.state.curFood.food;
      if (this.food.FOOD_DETAIL.length == 1) {
        this.food.PRICE = this.food.FOOD_DETAIL[0].DETAIL_PRICE;
        this.food.DETAIL_ID = this.food.FOOD_DETAIL[0].DETAIL_ID;
        this.food.FOOD_DETAIL = [];
      } else if (this.food.FOOD_DETAIL.length > 1) {
        this.hasDetail = true;
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
        Type: self.type,
        Food: self.food,
      };
      if (!this.hasDetail) {
        if (data.Food.PRICE == "") {
          this.$message.error("价格不能为空");
          return;
        }
        6;
        data.Food.FOOD_DETAIL = [];
        data.Food.FOOD_DETAIL.push({
          DETAIL_NAME: null,
          DETAIL_ID: data.Food.DETAIL_ID,
          DETAIL_PRICE: data.Food.PRICE,
          Urls: [],
        });
      }

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
          self.$router.go(-1);
        });
    },
    changeHasDetail() {
      if (this.hasDetail) {
        this.food.FOOD_DETAIL.push({
          DETAIL_NAME: "",
          DETAIL_PRICE: 0,
          Urls: [],
        });
      } else {
        this.food.FOOD_DETAIL = [];
      }
    },
    addDetail() {
      this.food.FOOD_DETAIL.push({
        DETAIL_NAME: "",
        DETAIL_PRICE: 0,
        Urls: [],
      });
    },
    inputFile: function (newFile, oldFile) {
      // if (newFile && oldFile && !newFile.active && oldFile.active) {
      //   // 获得相应数据
      //   console.log("response", newFile.response);
      //   if (newFile.xhr) {
      //     //  获得响应状态码
      //     console.log("status", newFile.xhr.status);
      //   }
      //   //七牛
      //   const config = {
      //     useCdnDomain: true,
      //     region: "z2",
      //   };
      //   const observable = qiniu.upload(
      //     this.files[0],
      //     "111",
      //     token,
      //     putExtra,
      //     config
      //   );
      //   const subscription = observable.subscribe("trastor"); // 上传开始
      //   // or
      //   const subscription = observable.subscribe(next, error, complete); // 这样传参形式也可以
      //   subscription.unsubscribe(); // 上传取消
      // }
    },
    /**
     * Pretreatment
     * @param  Object|undefined   newFile   读写
     * @param  Object|undefined   oldFile   只读
     * @param  Function           prevent   阻止回调
     * @return undefined
     */
    inputFilter: function (newFile, oldFile, prevent) {
      if (newFile && !oldFile) {
        // 过滤不是图片后缀的文件
        if (!/\.(jpeg|jpe|jpg|gif|png|webp)$/i.test(newFile.name)) {
          this.$message.error("请上传图片！");
          return prevent();
        }
      }

      // 创建 blob 字段 用于图片预览
      newFile.blob = "";
      let URL = window.URL || window.webkitURL;
      if (URL && URL.createObjectURL) {
        newFile.blob = URL.createObjectURL(newFile.file);
      }
    },
  },
};
</script>
