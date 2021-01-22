require('promise.prototype.finally').shim();
// �������������
axios.interceptors.request.use(function (config) {
  console.log("request:");
  console.log(config);
  return config;
}, function (error) {
  console.log("request error:");
  console.log(error);
  return Promise.reject(error);
});

// �����Ӧ������
axios.interceptors.response.use(function (response) {
  console.log("response:");
  console.log(response); 
  if (response.status ==401)
  {
    this.$message.error("�����µ�½");
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
 * ��װaxios��ͨ������
 * @param  {string}   method     get\post\put\delete
 * @param  {string}   url       ����Ľӿ�URL
 * @param  {object}   param     ���Ĳ�����û���򴫿ն���
 */
function http(method, url, param) {
  param = param && typeof param === 'object' ? param : {};
  let token = window.localStorage.getItem("accessToken");
  const config = {
    url: url,
    method: method,
    baseURL: 'http://www.trastor.cn:4396',
    // transformRequest: [function (param) {
    //   return Qs.stringify(param);
    // }],
    // headers: {
    //   'X-Requested-With': 'XMLHttpRequest',
    //   "accessToken": token == null ? "" : token,
    // },
    timeout: 30000,
  };

  // post����ʱ��Ҫ�趨Content-Type
  if (method === 'post') {
   // config.headers['Content-Type'] = 'application/json; charset=utf-8';
    config.data = param;
  } else if (method === 'get') {
    config.params = param;
  }
  return axios(config);
}

export {
  http
};
