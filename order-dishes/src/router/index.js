// import Vue from 'vue'
// import Router from 'vue-router'
// import Order from '@/components/Order'
// import Food from '@/components/Food'
// import Login from '@/components/Login'
// import Register from '@/components/Register'
// import AddShop from '@/components/AddShop'


Vue.use(VueRouter)

export default new VueRouter({
  routes: [{
      path: '/order',
      name: 'Order',
      component: resolve => require(['@/components/Order'], resolve),
      meta: {
        title: "订单"
      }
    },
    {
      path: '/',
      name: 'Food',
      component: resolve => require(['@/components/Food'], resolve),
      meta: {
        title: "点菜"
      }
    },
    {
      path: '/login',
      name: 'Login',
      component: resolve => require(['@/components/Login'], resolve),
      meta: {
        title: "登陆"
      }
    },
    {
      path: '/Register',
      name: 'Register',
      component: resolve => require(['@/components/Register'], resolve), 
      meta: {
        title: "注册店铺"
      }
    },
    {
      path: '/AddShop',
      name: 'AddShop',
      component: resolve => require(['@/components/AddShop'], resolve),
      meta: {
        title: "添加店铺"
      }
    }
  ]
})
