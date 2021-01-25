require('promise.prototype.finally').shim();
// 添加请求拦截器
axios.interceptors.request.use(function (config) {
  console.log("request:");
  console.log(config);

  let token = window.localStorage.getItem("accessToken")
  if (token) {
    //将token放到请求头发送给服务器,将tokenkey放在请求头中
    config.headers.authorization = token;
  }
  return config;
}, function (error) {
  console.log("request error:");
  console.log(error);
  return Promise.reject(error);
});

// 添加响应拦截器
axios.interceptors.response.use(function (response) {
  console.log("response:");
  console.log(response); 
  if (response.status ==401)
  {
    this.$message.error("请重新登陆");
     this.$router.push({
       path: "/food/" + self.name
     });
    return null;
  }
  return response.data;
}, function (error) {
  return Promise.reject(error);
});

/**
 * 封装axios的通用请求
 * @param  {string}   method     get\post\put\delete
 * @param  {string}   url       请求的接口URL
 * @param  {object}   param     传的参数，没有则传空对象
 */
function http(method, url, param) {
  param = param && typeof param === 'object' ? param : {};
  let token = window.localStorage.getItem("accessToken");
  const config = {
    url: url,
    method: method,
    baseURL: 'http://www.trastor.cn:4396',
    timeout: 30000,
  };
  // post请求时需要设定Content-Type
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
