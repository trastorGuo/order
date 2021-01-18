 
import Vue from 'vue'
import App from './App'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify';
// import 'roboto-fontface/css/roboto/roboto-fontface.css'
// // import '@mdi/font/css/materialdesignicons.css'
import './assets/css/common.css'  
Vue.config.productionTip = false;
 

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

