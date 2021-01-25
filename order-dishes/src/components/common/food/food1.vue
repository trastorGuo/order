<template>
  <div>
    <v-tabs vertical>
      <template v-for="item in types">
        <v-tab :key="item.TYPE_ID">{{ item.TYPE_NAME }}</v-tab>
        <v-tab-item :key="'item' + item.TYPE_ID">
          <template v-for="food in item.FOODS">
            <v-card flat :key="food.FOOD_ID">
              <v-card-text>
                <v-card outlined>
                  <v-list-item three-line style="padding: 0 0 0 16px">
                    <v-list-item-avatar tile size="75">
                      <v-img :src="food.Urls[0].URL"></v-img>
                    </v-list-item-avatar>
                    <v-list-item-content>
                      <p>{{ food.FOOD_NAME }}</p>
                      <v-chip-group mandatory v-show="food.FOOD_DETAIL.length > 1" active-class="primary--text"
                        @change="ChangeFoodDetailSelect(food,$event)" column v-model="food.SelectDetailIndex">
                        <template v-for="detail in food.FOOD_DETAIL">
                          <v-chip x-small :key="detail.DETAIL_ID">
                            {{detail.DETAIL_NAME}}
                          </v-chip>
                        </template>
                      </v-chip-group>
                      <div class="d-flex" style="height: 40px">
                        <p style="line-height: 40px;" class="primary--text">
                          ${{ food.SelectDetail.DETAIL_PRICE}}</p>
                        <v-spacer></v-spacer>
                        <v-btn v-if="food.NUM > 0" icon>
                          <v-icon dense color="primary" @click="changeNum(food, -1)">
                            mdi-minus-circle-outline</v-icon>
                        </v-btn>
                        <div style="width: 30px" v-if="food.NUM > 0">
                          <v-text-field type="number" readonly v-model="food.NUM" style="padding-top: 0" />
                        </div>
                        <v-btn icon>
                          <v-icon dense color="primary" @click="changeNum(food, 1)">
                            mdi-plus-circle-outline
                          </v-icon>
                        </v-btn>
                      </div>
                    </v-list-item-content>
                  </v-list-item>
                </v-card>
              </v-card-text>
            </v-card>
          </template>
        </v-tab-item>
      </template>
    </v-tabs>
  </div>
</template>
<script>
export default {
  props: ["types","carNum"],
  methods: {

  },
};
</script>