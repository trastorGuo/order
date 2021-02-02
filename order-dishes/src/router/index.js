// import Vue from 'vue'
// import Router from 'vue-router'
// import Order from '@/components/Order'
// import Food from '@/components/Food'
// import Login from '@/components/Login'
// import Register from '@/components/Register'
// import AddShop from '@/components/AddShop'


Vue.use(VueRouter)
Vue.config.devtools = true;
export default new VueRouter({
  routes: [{
      path: '/OrderSuccess',
      name: 'OrderSuccess',
      component: resolve => require(['@/components/OrderSuccess'], resolve),
      meta: {
        title: "订单",
        needLogin: false,
      },
    },
    {
      path: '/food/:account/:descnum',
      name: 'food',
      component: resolve => require(['@/components/food'], resolve),
      meta: {
        title: "点菜",
        needLogin: false,
      },
    },
    {
      path: '/login',
      name: 'Login',
      component: resolve => require(['@/components/Login'], resolve),
      meta: {
        title: "登陆",
        needLogin: true,
      },
    },
    {
      path: '/Register',
      name: 'Register',
      component: resolve => require(['@/components/Register'], resolve),
      meta: {
        title: "注册店铺",
        needLogin: true,
      },
    },
    {
      path: '/AddFood',
      name: 'AddFood',
      component: resolve => require(['@/components/AddFood'], resolve),
      meta: {
        title: "商品",
        needLogin: true,
      },
    },
    {
      path: '/Shop/:account',
      name: 'Shop',
      component: resolve => require(['@/components/Shop'], resolve),
      meta: {
        title: "我的店铺",
        needLogin: true,
      },
      children: [{
        path: 'ShopInfo',
        name: 'ShopInfo',
        component: resolve => require(['@/components/ShopInfo'], resolve),
        meta: {
          title: "店铺信息",
          needLogin: true,
        },
      }, {
        path: 'FoodManagement',
        name: 'FoodManagement',
        component: resolve => require(['@/components/foodManagement'], resolve),
        meta: {
          title: "菜单管理",
          needLogin: true,
        },
      }, {
        path: 'OrderManagement',
        name: 'OrderManagement',
        component: resolve => require(['@/components/OrderManagement'], resolve),
        meta: {
          title: "订单管理",
          needLogin: true,
        },
      }]
    }
  ],
})
