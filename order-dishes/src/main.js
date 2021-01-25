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
import Food from './components/common/food';
import VuejsDialog from 'vuejs-dialog';
// import VuejsDialogMixin from 'vuejs-dialog/dist/vuejs-dialog-mixin.min.js'; // only needed in custom components

import 'vuejs-dialog/dist/vuejs-dialog.min.css';
Vue.use(VuejsDialog, {
  okText: '确认',
  cancelText: '取消',
});
Vue.use(Food);
Vue.use(Message);
Vue.config.productionTip = false;
Vue.config.devtools = true;
Vue.prototype.$http = http;
Vue.prototype.$store = store;

router.beforeEach((to, from, next) => {
  /* 路由发生变化修改页面title */
  if (to.meta.title) {
    document.title = to.meta.title
  }
  next()
})
new Vue({
  el: '#app',
  router,
  store,
  components: {
    App
  },
  vuetify,
  template: '<App/>'
})
