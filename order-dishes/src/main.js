 
import Vue from 'vue'
import App from './App'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify';
// import vuetify from 'vuetify';
import './assets/css/common.css'  
import Message from 'vue-m-message';
import 'vue-m-message/dist/index.css'

//引入
import VueLazyload from 'vue-lazyload'
//注册
Vue.use(VueLazyload)
Vue.config.devtools = true;

Vue.use(Message);
Vue.config.productionTip = false;
 import {
   http
 } from './components/common/http';
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
  components: { App },
  vuetify,
  template: '<App/>'
})

