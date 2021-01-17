import Vue from 'vue'
import Router from 'vue-router'
import Order from '@/components/Order'
import Food from '@/components/Food'
import Login from '@/components/Login'
import Register from '@/components/Register'
import AddShop from '@/components/AddShop'


Vue.use(Router)

export default new Router({
  routes: [{
      path: '/order',
      name: 'Order',
      component: Order,
      meta: {
        title: "订单"
      }
    },
    {
      path: '/',
      name: 'Food',
      component: Food,
      meta: {
        title: "点菜"
      }
    },
    {
      path: '/login',
      name: 'Login',
      component: Login,
      meta: {
        title: "登陆"
      }
    },
    {
      path: '/Register',
      name: 'Register',
      component: Register,
      meta: {
        title: "注册店铺"
      }
    },
    {
      path: '/AddShop',
      name: 'AddShop',
      component: AddShop,
      meta: {
        title: "添加店铺"
      }
    }
  ]
})
