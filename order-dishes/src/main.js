import Vue from 'vue';
import App from './App';
import router from './router';
import store from './store';
import vuetify from './plugins/vuetify';
import './assets/css/common.css';
import Message from 'vue-m-message';
import 'vue-m-message/dist/index.css';
import {
  http
} from './components/common/http';
import fw from './components/common/framwork';
import VuejsDialog from 'vuejs-dialog';
import * as qiniu from 'qiniu-js';
import FileUpload from 'vue-upload-component'
import 'vuejs-dialog/dist/vuejs-dialog.min.css';

Vue.component('file-upload', FileUpload);
Vue.use(VuejsDialog, {
  okText: '确认',
  cancelText: '取消',
});
Vue.use(Message);
Vue.config.productionTip = false;
Vue.config.devtools = true;
Vue.prototype.$http = http;
Vue.prototype.$store = store;
Vue.prototype.$fw = fw;

router.beforeEach((to, from, next) => {
  var token = window.localStorage.getItem("accessToken");
  if (to.meta.needLogin && to.name != "Login" && (token == null || token == "")) {
    next({
      name: "Login"
    });
  }
  /* 路由发生变化修改页面title */
  if (to.meta.title) {
    document.title = to.meta.title;
  }
  next();
})
new Vue({
  el: '#app',
  router,
  store,
  qiniu,
  components: {
    App
  },
  vuetify,
  template: '<App/>'
})
