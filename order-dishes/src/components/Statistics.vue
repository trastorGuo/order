 <template>
  <div id="app" style=" width: 100%;">
    <template v-for="item in turnoverList">
      <v-card class="mx-auto" :key="item.ID" outlined style="margin:30px 30px 0 30px !important;">
        <v-list-item three-line>
          <v-fab-transition>
            <v-btn color="pink" dark absolute top left large style="top: -20px;height:80px;">
              <v-icon>{{item.ICON}}</v-icon>
            </v-btn>
          </v-fab-transition>
          <v-list-item-content style="text-align: right;">
            <v-list-item-subtitle>{{item.NAME}}</v-list-item-subtitle>
            <v-list-item-title class="headline mb-1">
              ￥{{item.TOTAL}}
            </v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-card>
    </template>
    <v-card class="mx-auto text-center" color="green" dark max-width="600" style="margin:30px 30px 0 30px !important;">
      <v-card-text>
        <v-sheet color="rgba(0, 0, 0, .12)">
          <v-sparkline :labels="labels" :value="value" color="rgba(255, 255, 255, .7)" height="100" padding="24"
            stroke-linecap="round" smooth>
            <!-- <template v-slot:label="item">
              ${{ item.value }}
            </template> -->
          </v-sparkline>
        </v-sheet>
      </v-card-text>
      <v-card-text style="margin:10px">
        <div class="text-h6">
          近一周日销售额
        </div>
      </v-card-text>
      <v-divider></v-divider>
      <!-- <v-card-actions class="justify-center">
        <v-btn block text>
          查看明细
        </v-btn>
      </v-card-actions> -->
    </v-card>
  </div>
</template>
 
<script>
export default {
  data: () => ({
    value: [],
    labels: [],
    turnoverList: [
      {
        ID: "1",
        ICON: "mdi-calendar-today",
        NAME: "今日营业额",
        TOTAL: "0",
      },
      {
        ID: "2",
        ICON: "mdi-calendar-month-outline",
        NAME: "本月营业额",
        TOTAL: "0",
      },
      {
        ID: "3",
        ICON: "mdi-chart-bar",
        NAME: "近半年营业额",
        TOTAL: "0",
      },
    ],
  }),

  mounted() {
    let self = this;
    self.$http("get", "/api/User/BusinessStatus").then((response) => {
      if(!response.success){
        self.$message.error(response.message.content);
        return;
      }
      self.turnoverList[0].TOTAL = response.data.salesToday;
      self.turnoverList[1].TOTAL = response.data.salesMonth;
      self.turnoverList[2].TOTAL = response.data.salesHalfYear;
      self.value = [];
      self.labels = [];
       response.data.week.forEach(element => {
         self.value.push(element.SALES);
         self.labels.push(self.$moment(element.DATE).format("DD")+"日");
       });
    });
  },
  methods: {},
};
</script>

