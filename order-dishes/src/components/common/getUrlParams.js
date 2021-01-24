// 获取参数
export function GetUrlParam(name) {
  var url = window.location.href;

  let params = url.substr(url.lastIndexOf("?") + 1).split("&");
  for (let i = 0; i < params.length; i++) {
    let param = params[i];
    let key = param.split("=")[0];
    let value = param.split("=")[1];
    if (key === name) {
      return value;
    }
  }
}