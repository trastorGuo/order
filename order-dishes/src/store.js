// import Vue from 'vue';
// import Vuex from 'vuex';

Vue.use(Vuex);

const state = {
  itemsCar: [],
  shopName: "",
  descNum: "",
  personNum: 0,
};

const mutations = {
  mutationsChangeCar(state, obj) {
    return state.itemsCar = obj;
  },
  mutationsChangeDescNum(state, obj) {
    return state.descNum = obj;
  },
  mutationsChangePersonNum(state, obj) {
    return state.personNum = obj;
  },
  mutationsChangeShopName(state, obj) {
    return state.shopName = obj;
  },
}

export default new Vuex.Store({
  state,
  mutations
})
