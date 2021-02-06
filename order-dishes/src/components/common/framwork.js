class framwork {

  // 数据获取
  static saveJsonInfo(name, info) {
    window.localStorage.setItem(name, JSON.stringify(info));
  }
  static getJsonInfo(name) {
    var info = window.localStorage.getItem(name);
    if (info) {
      return JSON.parse(info);
    }
    return {};
  }
  //token 操作
  static saveToken(token) {
    window.localStorage.setItem("accessToken", token);
  }
  static getToken() {
    var token = window.localStorage.getItem("accessToken");
    if (token) {
      return token;
    }
    return "";
  }
  static clearLogin() {
    localStorage.clear();
  }
}
export default framwork;
