<template>
  <div id="app" style="height: 1000px;width: 100%;">
    <div>
      <v-card class="mx-auto" max-width="344">
        <v-card-text style="padding: 15px!important;margin-top:15px;">
          <v-form ref="form" lazy-validation style="text-align: center;margin:20px;">
            <v-fab-transition v-if="isEdit">
              <v-btn @click="isEdit=false" color="primary" fab dark small absolute top right>
                <v-icon>mdi-pencil-outline</v-icon>
              </v-btn>
            </v-fab-transition>
            <!-- <v-btn v-if="isEdit" style="width:200px;margin:8px 0; " color="#73ce5f" rounded dark @click="isEdit=false">
              <v-icon>
                mdi-pencil-outline
              </v-icon>编辑
            </v-btn> -->
            <v-row v-else no-gutters style="height: 50px;">
              <v-col cols="6">
                <v-btn style="width:80%; " color=" #73ce5f" rounded @click="isEdit=true">
                  取消
                </v-btn>
              </v-col>
              <v-col cols="6">
                <v-btn style="width:80%;  " color="#73ce5f" rounded @click="Save()">
                  保存
                </v-btn>
              </v-col>
            </v-row>
            <v-text-field v-model="shopInfo.NAME" label="店铺名称" outlined dense :disabled="isEdit"></v-text-field>
            <v-text-field v-model="shopInfo.ACCOUNT" label="店铺账号" outlined dense disabled></v-text-field>
            <v-text-field v-model="shopInfo.ADDRESS" label="店铺地址" outlined dense :disabled="isEdit"></v-text-field>
            <v-text-field v-model="shopInfo.TEL" label="联系方式" :rules="telRules" outlined dense :disabled="isEdit">
            </v-text-field>
            <v-text-field v-model="shopInfo.PRINTER" label="打印机号" outlined dense :disabled="isEdit"></v-text-field>
            <v-text-field v-model="shopInfo.CAPITATION" label="茶位费描述" outlined dense :disabled="isEdit"></v-text-field>
            <v-text-field v-model="shopInfo.COST" label="茶位费(元/人)" outlined dense :disabled="isEdit" type="number">
            </v-text-field>
          </v-form>
        </v-card-text>
      </v-card>
      <v-expansion-panels multiple v-model="panel" style="padding: 15px!important;margin-top:15px;">
        <v-expansion-panel>
          <v-expansion-panel-header>桌号管理</v-expansion-panel-header>
          <v-expansion-panel-content>
            <div class="pa-4">
              <v-chip-group active-class="primary--text" column>
                <v-chip key="tagAdd" @click="isShowAddDesk=true">
                  <v-icon small>
                    mdi-plus-circle-outline
                  </v-icon>
                </v-chip>
                <v-chip key="tagEdit" @click="isDelDesk=!isDelDesk">
                  <v-icon small>
                    mdi-pencil-outline
                  </v-icon>
                </v-chip>
                <v-chip draggable v-for="desk in shopInfo.DeskList" :key="desk.ID" :close="isDelDesk"
                  @click="showQr(desk)" @click:close="DeleteDesk(desk)">
                  <!-- {{ desk.DeskCount}}({{desk.DescDesc}}) -->
                  {{ desk.DeskCount}}
                </v-chip>
              </v-chip-group>
            </div>
            <div style="text-align: center;" v-show="showDownload">
              <div id="newImg" v-show="showOldQr">
                <vue-qr :text="qrSrc" :size="qrSize" />
                <div style="text-align:center">{{qrInfo}}</div>
              </div>
              <div v-show="!showOldQr">
                <img style="width:100%;" :src="qrCodeUrl" />
                <span @click="setImage()">
                  <v-icon>
                    mdi-download
                  </v-icon>长按二维码保存
                </span>
              </div>
            </div>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
      <!-- <v-expansion-panels multiple v-model="panel" style="padding: 15px!important;margin-top:15px;">
        <v-expansion-panel>
          <v-expansion-panel-header>配置项</v-expansion-panel-header>
          <v-expansion-panel-content>
            <div class="pa-4">
              <v-icon color="primary" @click="addDetail()">
                mdi-plus-circle-outline
              </v-icon>
              <v-row v-for="(PARAM,index) in shopInfo.PARAMS" :key="PARAM.index">
                <v-col cols="6">
                  <v-text-field rounded outlined dense v-model="PARAM.PARAM_NAME" label="名称"></v-text-field>
                </v-col>
                <v-col cols="5">
                  <v-text-field rounded outlined dense v-model="PARAM.PARAM_VALUE" label="值" type="number">
                  </v-text-field>
                </v-col>
                <v-col cols="1" style="line-height: 40px;">
                  <v-icon v-if="index > 1" color="primary" @click="shopInfo.PARAMS.splice(index,1)">
                    mdi-delete-outline
                  </v-icon>
                </v-col>
              </v-row>
            </div>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels> -->
    </div>

    <v-dialog persistent v-model="isShowAddDesk">
      <template>
        <v-card>
          <v-toolbar color="primary" dark>新增桌号</v-toolbar>
          <v-card-text>
            <v-text-field style="margin-right: 16px;" v-model="newName" label="桌号"
              prepend-icon="mdi-account-supervisor"></v-text-field>
            <!-- <v-text-field style="margin-right: 16px;" v-model="newDesc" label="描述"
              prepend-icon="mdi-account-supervisor"></v-text-field> -->
          </v-card-text>
          <v-card-actions class="justify-end">
            <v-row>
              <v-col cols="6">
                <v-btn block @click="AddDesk" color="primary" :disabled="newName==''">新增 </v-btn>
              </v-col>
              <v-col cols="6">
                <v-btn block @click="isShowAddDesk = false" color="primary">取消 </v-btn>
              </v-col>
            </v-row>

          </v-card-actions>
        </v-card>
      </template>
    </v-dialog>

  </div>
</template>
 <style   scoped>
.qrcode {
  display: inline-block;
  background-color: #fff;
  padding: 6px;
  box-sizing: border-box;
}
</style>
<script>
import vueQr from "vue-qr";
import html2canvas from "html2canvas";

export default {
  components: {
    vueQr,
  },
  data: () => ({
    qrSize: 200,
    qrInfo: "",
    qrCodeUrl: "",
    showOldQr: true,
    shopInfo: {
      NAME: "",
      ADDRESS: "",
      ACCOUNT: "",
      PASSWORD: "",
      PRINTER: "",
      COST: 0,
      CAPITATION: "茶位费",
      TEL: "",
      URLS: [],
      DeskList: [],
    },
    qrSrc: "ssss",
    panel: [0],
    isEdit: true,
    isDelDesk: false,
    showDownload: false,
    isShowAddDesk: false,
    isShowChangePw: false,
    curDesk: null,
    newName: "",
    newDesc: "",

    telRules: [
      (value) => {
        const pattern = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
        return pattern.test(value) || "无效的手机号码";
      },
    ],
  }),
  mounted() {
    // this.shopInfo = JSON.parse(window.localStorage.getItem("shopInfo"));
    this.shopInfo = this.$fw.getJsonInfo("shopInfo");
  },
  methods: {
    Save() {
      let self = this;
      self
        .$http("post", "/api/User/EditShop", self.shopInfo)
        .then((response) => {
          if (!response.success) {
            self.$message.error(response.message.content);
            return;
          }
          self.$message.success("店铺信息修改成功");
          self.isEdit = true;
          self.updatePageShopInfo();
        });
    },
    DeleteDesk(desk) {
      let self = this;
      self.$dialog
        .confirm("确认删除此桌号？")
        .then(function (dialog) {
          self
            .$http("get", "/api/Product/DeleteDesk?descNum=" + desk.DeskCount)
            .then((response) => {
              if (!response.success) {
                self.$message.error(response.message.content);
                return;
              }
              self.$message.success("删除成功");
              self.updatePageShopInfo();
            });
        })
        .catch(function () {
          console.log("Clicked on cancel");
        });
    },
    AddDesk() {
      let self = this;
      var data = {
        deskNum: self.newName,
        deskDesc: self.newDesc,
      };
      self
        .$http(
          "get",
          "/api/Product/AddOrEditDesk?deskNum=" +
            self.newName +
            "&deskDesc=" +
            self.newDesc
        )
        .then((response) => {
          if (!response.success) {
            self.$message.error(response.message.content);
            return;
          }
          self.newName = "";
          self.isShowAddDesk = false;
          self.$message.success(response.data);
          // self.shopInfo.DeskList = response.data;
          self.updatePageShopInfo();
        });
    },
    updatePageShopInfo() {
      let self = this;
      self
        .$http("get", "/api/User/ShopInfo?account=" + self.shopInfo.ACCOUNT)
        .then((responseInfo) => {
          console.log(responseInfo);
          if (!responseInfo.success) {
            self.$message.error(responseInfo.message.content);
            return;
          }
          self.$store.commit("mutationsChangeShopInfo", responseInfo.data);
          window.localStorage.setItem(
            "shopInfo",
            JSON.stringify(responseInfo.data)
          );
          self.shopInfo = responseInfo.data;
        });
    },
    showQr(desk) {
      this.qrSrc =
        "http://" +
        document.location.host +
        "/#/food/" +
        this.shopInfo.ACCOUNT +
        "/" +
        desk.DeskCount;
      this.qrInfo = this.shopInfo.NAME + "——第" + desk.DeskCount + "桌";
      this.showDownload = true;
      this.curDesk = desk;
      let self = this;
      self.showOldQr = true;
      setTimeout(() => {
        var opts = { useCORS: true };
        html2canvas(document.getElementById("newImg"), opts).then(function (
          canvas
        ) {
          var imgUri = canvas.toDataURL("image/jpeg", 2); // 获取生成的图片的url
          self.qrCodeUrl = imgUri;
          self.showOldQr = false;
        });
      }, 10);
    },
  },
};
</script>
