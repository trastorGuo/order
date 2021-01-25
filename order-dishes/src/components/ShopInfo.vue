<template>
  <div id="app" style="height: 1000px;width: 100%;">
    <div>
      <div class="main-border">
        <v-form ref="form" lazy-validation style="text-align: center;">
          <v-btn v-if="isEdit" style="width:200px;margin:8px 0; " color="#73ce5f" rounded dark @click="isEdit=false">
            <v-icon dark>
              mdi-pencil-outline
            </v-icon>编辑
          </v-btn>
          <v-btn v-else style="width:200px;margin:8px 0; " color="#73ce5f" rounded dark @click="Save()">
            <v-icon dark>
              mdi-pencil-outline
            </v-icon>保存
          </v-btn>
          <v-text-field v-model="shopInfo.NAME" label="店铺名称" required :disabled="isEdit"></v-text-field>
          <v-text-field v-model="shopInfo.ACCOUNT" label="店铺账号" required disabled></v-text-field>
          <v-text-field v-model="shopInfo.ADDRESS" label="店铺地址" required :disabled="isEdit"></v-text-field>
          <v-text-field v-model="shopInfo.TEL" label="联系方式" :rules="telRules" required :disabled="isEdit">
          </v-text-field>
          <v-expansion-panels multiple>
            <v-expansion-panel>
              <v-expansion-panel-header>桌号</v-expansion-panel-header>
              <v-expansion-panel-content>
                <div class="pa-4">
                  <v-chip-group active-class="primary--text" column>
                    <v-chip v-for="desk in shopInfo.DeskList" :key="desk.ID" close @click:close="DeleteDesk()">
                      {{ desk.DESK_COUNT }}
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
        </v-form>
      </div>
    </div>
    <v-dialog persistent v-model="isShowAddDesk">
      <template>
        <v-card>
          <v-toolbar color="primary" dark>新增桌号</v-toolbar>
          <v-card-text>
            <v-text-field style="margin-right: 16px;" v-model="newName" label="桌号"
              prepend-icon="mdi-account-supervisor"></v-text-field>
            <v-text-field style="margin-right: 16px;" v-model="newDesc" label="描述"
              prepend-icon="mdi-account-supervisor"></v-text-field>
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
 
<script>
export default {
  data: () => ({
    shopInfo: {
      NAME: "",
      ADDRESS: "",
      ACCOUNT: "",
      PASSWORD: "",
      TEL: "",
      URLS: [],
    },
    isEdit: true,
    isShowAddDesk: false,
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
              self.$router.go(0);
            });
        });
    },
    DeleteDesk() {
      let self = this;
      self.$dialog
        .confirm("确认删除此分类？")
        .then(function (dialog) {
          console.log("Clicked on proceed");
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
      self.$http("post", "/api/Product/AddDesk", data).then((response) => {
        self.newTypeName = "";
        self.isShowAddDesk = false;
        self.$message.success("桌号新增成功");
        self.shopInfo.DeskList = response.data;
      });
    },
  },
};
</script>
