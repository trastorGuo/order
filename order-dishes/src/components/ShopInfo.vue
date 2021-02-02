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
            <v-text-field v-model="shopInfo.PrinterCode" label="打印机号" outlined dense :disabled="isEdit"></v-text-field>
            <v-expansion-panels multiple v-model="panel">
              <v-expansion-panel>
                <v-expansion-panel-header>桌号</v-expansion-panel-header>
                <v-expansion-panel-content>
                  <div class="pa-4">
                    <v-chip-group active-class="primary--text" column>
                      <v-chip draggable v-for="desk in shopInfo.DeskList" :key="desk.ID" :close="!isEdit"
                        @click="showQr(desk)" @click:close="DeleteDesk()">
                        <!-- {{ desk.DeskCount}}({{desk.DescDesc}}) -->
                        {{ desk.DeskCount}}
                      </v-chip>
                      <v-chip key="tagAdd" @click="isShowAddDesk=true">
                        <v-icon>
                          mdi-plus-circle-outline
                        </v-icon>
                      </v-chip>
                    </v-chip-group>
                  </div>
                </v-expansion-panel-content>
              </v-expansion-panel>
            </v-expansion-panels>
            <div class="qrcode" ref="qrCodeUrl" style="margin-top: 10px;"></div>
            <div class="qrcodesave" v-show="showDownload">
              <span @click.stop="downloadE">
                <v-icon>
                  mdi-download
                </v-icon>下载
              </span>
            </div>
          </v-form>
        </v-card-text>
      </v-card>
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
import QRCode from "qrcodejs2";
export default {
  data: () => ({
    shopInfo: {
      NAME: "",
      ADDRESS: "",
      ACCOUNT: "",
      PASSWORD: "",
      TEL: "",
      URLS: [],
      DeskList: [],
    },
    panel: [0],
    isEdit: true,
    showDownload: false,
    isShowAddDesk: false,
    curDesk: null,
    newName: "",
    newDesc: "",
    telRules: [
      (value) => {
        const pattern = /^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/;
        return pattern.test(value) || "无效的手机号码";
      },
    ],
    // AccountRules: [
    //   (value) => {
    //     const pattern = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,}$/;
    //     return pattern.test(value) || "长度需大于6位，由数字和字符组成";
    //   },
    // ],
  }),
  mounted() {
    this.shopInfo = JSON.parse(window.localStorage.getItem("shopInfo"));
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
    DeleteDesk() {
      let self = this;
      self.$dialog
        .confirm("确认删除此桌号？")
        .then(function (dialog) {
          self.$message.error("暂时咩做，莫急");
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
          self.newTypeName = "";
          self.isShowAddDesk = false;
          self.$message.success("桌号新增成功");
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
      this.$refs.qrCodeUrl.innerHTML = "";
      var text =
        document.location.host +
        "/#/food/" +
        this.shopInfo.ACCOUNT +
        "/" +
        desk.DeskCount;
      var qrcode = new QRCode(this.$refs.qrCodeUrl, {
        text: text, // 需要转换为二维码的内容
        width: 150,
        height: 150,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.H,
      });
      this.showDownload = true;
      this.curDesk = desk;
    },
    downloadE() {
      //下载
      var canvasData = this.$refs.qrCodeUrl.getElementsByTagName("canvas");
      var a = document.createElement("a");
      var event = new MouseEvent("click"); // 创建一个单击事件
      a.href = canvasData[0].toDataURL("image/png");
      a.download = this.shopInfo.NAME + "-" + this.curDesk.DeskCount;
      a.dispatchEvent(event); // 触发a的单击事件
    },
  },
};
</script>
