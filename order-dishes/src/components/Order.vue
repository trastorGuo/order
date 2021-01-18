<template>
  <div id="app">
    这是结算页面
  </div>
</template>

<script>
import axios from 'axios';
export default {
  name: "Order",
  data: () => ({
    SHOP_NAME: "周洛御景山庄",
    tab: null,
    PERSON_QTY: 2,
    num: 10,
    sheet: false,
    dialog: false,
    foodList: [
      {
        title: "农家小炒",
        foods: [
          {
            ID: "1",
            NUM: 0,
            PRICE: 38,
            NAME: "辣椒炒肉",
          },
          {
            ID: "2",
            NUM: 0,
            PRICE: 38,
            NAME: "辣椒炒肉",
          },
          {
            ID: "3",
            NUM: 0,
            PRICE: 38,
            NAME: "辣椒炒肉",
          },
          {
            ID: "4",
            NUM: 0,
            PRICE: 38,
            NAME: "辣椒炒肉",
          },
          {
            ID: "5",
            NUM: 0,
            PRICE: 38,
            NAME: "辣椒炒肉",
          },
          {
            ID: "6",
            NUM: 0,
            PRICE: 38,
            NAME: "辣椒炒肉",
          },
          {
            ID: "7",
            NUM: 0,
            PRICE: 38,
            NAME: "辣椒炒肉",
          },
        ],
      },
      {
        title: "特色蒸菜",
        foods: [
          {
            ID: "21",
            NUM: 0,
            PRICE: 38,
            NAME: "腊味合蒸",
          },
        ],
      },
      {
        title: "营养炖汤",
        foods: [
          {
            ID: "31",
            NUM: 0,
            PRICE: 38,
            NAME: "羊肉炖粉皮",
          },
        ],
      },
    ],

    items: [
      {
        src: "https://cdn.vuetifyjs.com/images/carousel/squirrel.jpg",
      },
      {
        src: "https://cdn.vuetifyjs.com/images/carousel/sky.jpg",
      },
      {
        src: "https://cdn.vuetifyjs.com/images/carousel/bird.jpg",
      },
      {
        src: "https://cdn.vuetifyjs.com/images/carousel/planet.jpg",
      },
    ],
    itemsCar: [],
    carNum: 0,
  }),
  watch: {
    sheet: function (newValue, oldValue) {
      this.carNum = 0;
      this.foodList.forEach((element) => {
        element.foods.forEach((food) => {
          if (food.NUM > 0) this.carNum++;
        });
      });
    },
  },
  mounted() {
    // axios
    //   .get("http://localhost:59703/order/food/Get")
    //   .then((response) => console.log(response));
  },
  methods: {
    showCar: function () {
      this.sheet = true;
      this.itemsCar = [];
      this.foodList.forEach((element) => {
        element.foods.forEach((food) => {
          if (food.NUM > 0) this.itemsCar.push(food);
        });
      });
    },
    changeNum: function (food, num) {
      food.NUM += num;
      this.carNum = 0;
      this.foodList.forEach((element) => {
        element.foods.forEach((food) => {
          if (food.NUM > 0) this.carNum++;
        });
      });
    },
    SelectPersonQty: function () {
      this.dialog = true;
    },
  },
};
</script>
