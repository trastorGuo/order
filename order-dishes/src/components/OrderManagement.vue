 <template>
  <div id="app" style="height: 1000px;width: 100%;">
    <v-row>
      <v-col v-for="card in cards" :key="card" cols="12">
        <v-card>
          <v-subheader>{{ card }}</v-subheader>

          <v-list two-line>
            <template v-for="n in 6">
              <!-- <v-divider></v-divider> -->
              <v-list-item :key="n">
                <v-list-item-avatar color="grey darken-1">
                </v-list-item-avatar>

                <v-list-item-content>
                  <v-list-item-title>第 {{ n }} 桌</v-list-item-title>
                  <v-list-item-subtitle>
                    辣椒炒肉、农家一碗香、炖土鸡...
                  </v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>
              <v-divider v-if="n !== 6" :key="`divider-${n}`" inset></v-divider>
            </template>
          </v-list>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>
 
<script>
export default {
  data: () => ({
    cards:['2019-11-11','2020-1-1'],
    form: {
      NAME: "",
      TEL: "",
      ADDRESS: "",
      ACCOUNT: "",
      PASSWORD: "",
      PASSWORD2: "",
    },
  }),

  mounted() {
  },
  methods: {
  },
};
</script>

