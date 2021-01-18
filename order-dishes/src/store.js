// import Vue from 'vue';
// import Vuex from 'vuex';

Vue.use(Vuex);

const state = {
    foodList:{},
};

const mutations = {
    mutationsChangeFood(state, obj){
        return state.foodList = obj;
    }
}

export default new Vuex.Store({
    state,
    mutations
})