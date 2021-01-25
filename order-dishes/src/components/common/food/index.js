import foodComponent from './food1.vue'

const Food =  {
    install:function(Vue){
        Vue.component('food',foodComponent);
    }
}

export default Food;