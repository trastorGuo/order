class framwork {
  static saveShopInfo(info) {
    window.localStorage.setItem("shopInfo", JSON.stringify(info));
  }
  static getShopInfo() {
    var shopInfo = window.localStorage.getItem("shopInfo");
    if (shopInfo) {
      return JSON.parse(shopInfo);
    }
    return {};
  }
  static saveToken(token) {
    window.localStorage.setItem("accessToken", token);
  }
  static getToken() {
    var token = window.localStorage.getItem("accessToken");
    if (token) {
      return  token;
    }
    return "";
  }
  static clearLogin(){
      localStorage.clear();
  }
}
export default framwork;