require('promise.prototype.finally').shim();
import Message from 'vue-m-message';
import fw from '../common/framwork';
// 添加请求拦截器
axios.interceptors.request.use(function (config) {
  console.log("request:");
  console.log(config);
  let token = fw.getToken();
  if (token) {
    config.headers.authorization = token;
  }
  return config;
}, function (error) {
  console.log(error);
  return Promise.reject(error);
});

// 添加响应拦截器
axios.interceptors.response.use(function (response) {
  console.log(response);
  return response.data;
}, function (error) {
    if (error.status == 401) {
      Message.error("登陆已失效，请重新登陆");
      fw.clearLogin();
      this.$router.push({
        path: "/login"
      });
      return null;
    }
    Message.error(error.message);
  return Promise.reject(error);
});

function http(method, url, param) {
  param = param && typeof param === 'object' ? param : {};
  const config = {
    url: url,
    method: method,
    baseURL: 'http://thankful.top:7700/',
    timeout: 30000,
  };
  if (method === 'post') {
    config.data = param;
  } else if (method === 'get') {
    config.params = param;
  }
  return axios(config);
}
export {
  http
};
