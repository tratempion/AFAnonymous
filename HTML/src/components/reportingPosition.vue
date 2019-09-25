<template>
  
  <div>
    
    <v-app style="text-align:center">
  
      <v-container fluid grid-list-md text-xs-center>
  
        <v-layout row wrap style=" background-color: #3F51B5">
  
          <v-flex>

            <v-autocomplete dark v-model="yearInterval[0]" :items="selectAbleYear" label="Année de Début de L'interval" :rules="yearIntervalPastRules"/>

          </v-flex>

          <v-flex>

            <v-autocomplete dark v-model="yearInterval[1]" :items="selectAbleYear" label="Année de Fin de L'interval" :rules="yearIntervalFutureRules"/>

          </v-flex>

        </v-layout>

        <v-layout v-resize="onResize" row wrap style="margin-top: 3px" v-if="!isMobile">
               
          <v-flex xs6>

            <v-card>
              <v-card-title style="background-color: #FFC107; color: white">
                <div>
                  <strong>Évolution Total</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="YearLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="AreaChart" :data="chartDataYear" :options="chartOptionsYear" :events="chartEventsYear" ref="YearChart"/>

            </v-card>

          </v-flex>

          <v-flex xs6>

            <v-card>
              <v-card-title style="background-color: #03A9F4; color: white">
                <div>
                  <strong>Les Comptes</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="AccountLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="ColumnChart" :data="chartDataAccount" :options="chartOptionsAccount" :events="chartEventsAccount" ref="AccountChart"/>
            </v-card>
          </v-flex>

          <v-flex xs6>

            <v-card>
              <v-card-title style="background-color: #9C27B0; color: white">
                <div>
                  <strong>Évolution total en {{1900+this.selectedDate.getYear()}}</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="MonthLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="AreaChart" :data="chartDataMonth" :options="chartOptionsMonth" :events="chartEventsMonth" ref="MonthChart"/>
            </v-card>

          </v-flex>

          <v-flex xs6>

            <v-card>
              <v-card-title style="background-color: #8BC34A; color: white">
                <div>
                  <strong>Compte '{{this.selectedAccount}}' en {{1900+this.selectedDate.getYear()}}</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="AccountLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="AreaChart" :data="chartDataMonthAccount" :options="chartOptionsMonthAccount" :events="chartEventsAccountMonth" ref="MonthAccountChart"/>
            </v-card>

          </v-flex>

          <v-flex xs6>

            <v-card>
              <v-card-title style="background-color: #F44336; color: white">
                <div>
                  <strong>Évolution total en {{(this.MonthList[(this.selectedDate.getMonth())])+' '+ (1900+this.selectedDate.getYear())}}</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="DayLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="AreaChart" :data="chartDataDay" :options="chartOptionsDay" :events="chartEventsDay" ref="DayChart"/>
            </v-card>

          </v-flex>

          <v-flex xs6>

            <v-card>
              <v-card-title style="background-color: #009688; color: white">
                <div>
                  <strong>Compte '{{this.selectedAccount}}' en {{(this.MonthList[this.selectedAccountMonth-1])+' '+this.selectedDate.getFullYear()}}</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="AccountLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="AreaChart" :data="chartDataDayAccount" :options="chartOptionsDayAccount" ref="DayAccountChart"/>
            </v-card>

          </v-flex>

        </v-layout>

        <v-layout v-resize="onResize" row wrap style="margin-top: 3px" v-else>
               
          <v-flex xs12>

            <v-card>
              <v-card-title style="background-color: #FFC107; color: white">
                <div>
                  <strong>Évolution Total</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="YearLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="AreaChart" :data="chartDataYear" :options="chartOptionsYear" :events="chartEventsYear" ref="YearChart"/>

            </v-card>

          </v-flex>

          <v-flex xs12>

            <v-card>
              <v-card-title style="background-color: #9C27B0; color: white">
                <div>
                  <strong>Évolution total en {{1900+this.selectedDate.getYear()}}</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="MonthLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="AreaChart" :data="chartDataMonth" :options="chartOptionsMonth" :events="chartEventsMonth" ref="MonthChart"/>
            </v-card>

          </v-flex>

          <v-flex xs12>

            <v-card>
              <v-card-title style="background-color: #F44336; color: white">
                <div>
                  <strong>Évolution total en {{(this.MonthList[(this.selectedDate.getMonth())])+' '+ (1900+this.selectedDate.getYear())}}</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="DayLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="AreaChart" :data="chartDataDay" :options="chartOptionsDay" :events="chartEventsDay" ref="DayChart"/>
            </v-card>

          </v-flex>

          <v-flex xs12>

            <v-card>
              <v-card-title style="background-color: #03A9F4; color: white">
                <div>
                  <strong>Les Comptes</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="AccountLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="ColumnChart" :data="chartDataAccount" :options="chartOptionsAccount" :events="chartEventsAccount" ref="AccountChart"/>
            </v-card>
          </v-flex>

          <v-flex xs12>

            <v-card>
              <v-card-title style="background-color: #8BC34A; color: white">
                <div>
                  <strong>Compte '{{this.selectedAccount}}' en {{1900+this.selectedDate.getYear()}}</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="AccountLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="AreaChart" :data="chartDataMonthAccount" :options="chartOptionsMonthAccount" :events="chartEventsAccountMonth" ref="MonthAccountChart"/>
            </v-card>

          </v-flex>

          <v-flex xs12>

            <v-card>
              <v-card-title style="background-color: #009688; color: white">
                <div>
                  <strong>Compte '{{this.selectedAccount}}' en {{(this.MonthList[this.selectedAccountMonth-1])+' '+this.selectedDate.getFullYear()}}</strong>
                </div>
              </v-card-title>

              <trinity-rings-spinner v-if="AccountLoading" :animation-duration="500" :size="66" color="#FFC107" style="margin-left: auto;margin-right: auto;"/>

              <GChart v-else type="AreaChart" :data="chartDataDayAccount" :options="chartOptionsDayAccount" ref="DayAccountChart"/>
            </v-card>

          </v-flex>

        </v-layout>

      </v-container>

    </v-app>

  </div>

</template>
    

<script>

  import { GChart } from "vue-google-charts";
  import { TrinityRingsSpinner } from "epic-spinners";

  const axios = require("axios");

  export default {

    name: "reportingPosition",

    components: {

      GChart,
      TrinityRingsSpinner
    },

    data() {

      var that = this;

      return {

        hasError: false,
        MonthList:['Janvier','Fevrier','Mars','Avril','Mai','Juin','Juillet','Aout','Septembre','Octobre','Novembre','Decembre'],
        YearLoading: true,
        MonthLoading: true,
        DayLoading: true,
        AccountLoading: true,
        isMobile: false,

        chartDataYear: [["Annee", "Position"], [0, 0]],
        chartDataMonth: [["Mois", "Position"], [0, 0]],
        chartDataDay: [["jour", "Position"], [0, 0]],
        chartDataAccount: [["compte", 0]],
        chartDataMonthAccount: [["compte", 0]],
        chartDataDayAccount: [["compte", 0]],

        selectAbleYear: [],

        yearInterval: [],

        yearIntervalPastRules: [ v => this.yearInterval[0] <= this.yearInterval[1] || "L'année de Début doit être Anterieur à l'Année de Fin" ],

        yearIntervalFutureRules: [ v => this.yearInterval[0] <= this.yearInterval[1] || "L'année de Fin doit être Ulterieure à l'Année de Début"],

        selectedDate: null,
        selectedAccount: null,
        selectedAccountMonth: null,

        chartOptionsYear: {

          animation: {

            duration: 1000
          },

          legend: {

            position: "none"
          },

          lineWidth: 5,
          pointsVisible: true,
          colors: ["#FFC107"]
        },

        chartOptionsAccount: {

          isStacked: true,

          animation: {

            duration: 500
          },

          legend: {

            position: "none",
            colors: ["#03A9F4"]
          }
        },

        chartOptionsMonth: {

          animation: {

            duration: 500
          },

          legend: {

            position: "none"
          },

          lineWidth: 5,
          pointsVisible: true,
          colors: ["#9C27B0"]
        },

        chartOptionsMonthAccount: {

          isStacked: true,

          animation: {

            duration: 500
          },

          legend: {

            position: "none"
          },

          lineWidth: 5,
          pointsVisible: true,
          colors: ["#8BC34A"]
        },

        chartOptionsDay: {

          animation: {

            duration: 500
          },

          legend: {

            position: "none"
          },

          lineWidth: 5,
          pointsVisible: true,
          colors: ["#F44336"]
        },

        chartOptionsDayAccount: {

          isStacked: true,
          animation: {

            duration: 500
          },

          legend: {

            position: "none"
          },

          lineWidth: 5,
          pointsVisible: true,
          colors: ["#009688"]
        },

        chartEventsYear: {

          select: () => {

            if (that.$refs.YearChart.chartObject.getSelection()[0]) {

              that.selectedDate = new Date(
                "12/31/" +
                  (parseInt(that.yearInterval[0]) +
                    that.$refs.YearChart.chartObject.getSelection()[0].row)
              );
            }
          }
        },

        chartEventsMonth: {

          select: () => {

            if (that.$refs.MonthChart.chartObject.getSelection()[0]) {

              if (
                that.$refs.MonthChart.chartObject.getSelection()[0].row + 1 !=
                12
              ) {
                
                var dateTmp = new Date(
                  that.$refs.MonthChart.chartObject.getSelection()[0].row +
                    2 +
                    "/01/" +
                    (1900 + that.selectedDate.getYear())
                );

                dateTmp.setDate(dateTmp.getDate() - 1);

                that.selectedDate = dateTmp;
              } 
              else {

                that.selectedDate = new Date(
                  that.$refs.MonthChart.chartObject.getSelection()[0].row +
                    1 +
                    "/31/" +
                    (1900 + that.selectedDate.getYear())
                );
              }
            }
          }
        },

        chartEventsDay: {

          select: () => {

            if (that.$refs.DayChart.chartObject.getSelection()[0]) {

              that.selectedDate = new Date(
                that.selectedDate.getMonth() +
                  1 +
                  "/" +
                  (that.$refs.DayChart.chartObject.getSelection()[0].row + 1) +
                  "/" +
                  (1900 + that.selectedDate.getYear())
              );
            }
          }
        },

        chartEventsAccount: {

          select: () => {

            var selection = that.$refs.AccountChart.chartObject.getSelection()[0];

            if (
              selection &&
              !that.chartDataAccount[selection.row + 1][0].includes("Total")
            ) {

              var tmp =
                that.$refs.AccountChart.chartObject.getSelection()[0].row + 1;
              that.selectedAccount= that.chartDataAccount[tmp][0].split("-")[1].trim();
            }
          }
        },

        chartEventsAccountMonth: {

          select: () => {

            if (that.$refs.MonthAccountChart.chartObject.getSelection()[0]) {
              that.selectedAccountMonth =
                that.$refs.MonthAccountChart.chartObject.getSelection()[0].row +
                1;

              that.getDataAccountDay();
            }
          }
        }
      };
    },

    created() {

      this.getYear();
    },

    watch: {

      selectedAccount: function(old, newv) {

        this.selectedAccountMonth = 12;

        this.getDataAccountMonth();
      },

      yearInterval: function(newv, old) {

        if (
          parseInt(this.yearInterval[0]) > 1900 &&
          parseInt(this.yearInterval[0]) < 2100 &&
          parseInt(this.yearInterval[1]) < 2100 &&
          parseInt(this.yearInterval[1]) > 1900
        ) {

          if (this.yearInterval[0] >= this.yearInterval[1]) {

            this.yearInterval = old;
            this.hasError = true;
          } 
          else {

            this.hasError = false;

            this.getDataYear();
            var tmpDate = new Date(parseInt(this.yearInterval[1]) + 1, 0, 1);
            tmpDate.setDate(tmpDate.getDate() - 1);
            this.selectedDate = new Date(tmpDate);
          }
        }
      },

      selectedDate: function(newDate, oldDate) {

        if (oldDate == null || newDate.getYear() != oldDate.getYear()) {

          this.getDataMonths();
          this.getDataDay();
          this.getDataAccounts();

          //this.selectedAccount = null;
        } 
        else if (newDate.getMonth() != oldDate.getMonth()) {

          this.getDataDay();
        }

      }
    },

    mounted() {

      this.selectedDate = new Date();

      this.yearInterval = [
        1890 + this.selectedDate.getYear(),
        1900 + this.selectedDate.getYear()
      ];

      this.getDataYear();

      this.getDataAccounts();
    },

    methods: {

      getDataAccounts() {

        var that = this;

        var urlGetAllAccounts =
          this.$executioEnvironment +
          "GetAllAccountPositionAtDate?date=" +
          this.selectedDate.getDate() +
          "/" +
          (this.selectedDate.getMonth() + 1) +
          "/" +
          (1900 + this.selectedDate.getYear());

        axios
          .get(urlGetAllAccounts)
          .then(function(response) {
            that.chartDataAccount = [["Compte", "Position"]];
            var total = 0;
            response.data.forEach(compte => {
              that.chartDataAccount.push([compte.numero, compte.position]);
              total = total + compte.position;
            });

            that.chartDataAccount.push(["Total", total]);

            if (that.chartDataAccount.length > 1 && (that.selectedAccount == null || !that.selectedAccount)){

              that.selectedAccount=that.chartDataAccount[1][0].split("-")[1].trim();
            }  

            that.getDataAccountMonth();
            that.AccountLoading = false;
          })
          .catch(function(error) {
            console.log(error);
          });
      },    

      onResize() {

        if (window.innerWidth < 769) {

          this.isMobile = true;
        } 
        else {

          this.isMobile = false;
        }  
      },

      getYear() {

        var actualYear = new Date().getFullYear();

        for (var y = 2000; y <= actualYear + 10; y++) {

          this.selectAbleYear.push(y);
        }
      },

      getDataYear() {

        this.YearLoading = true;
        var that = this;
        var date_debut = "01/01/" + that.yearInterval[0];
        var date_fin = "31/12/" + that.yearInterval[1];

        var urlGetPostionYear =
          that.$executioEnvironment +
          "GetTotalPositionAtInterval" +
          "?date_debut=" +
          date_debut +
          "&date_fin=" +
          date_fin +
          "&mode=annee";

        axios
          .get(urlGetPostionYear)
          .then(function(response) {

            that.chartDataYear = [];
            that.chartDataYear.push(["Annee", "Position"]);
            response.data.forEach(element => {
              let currentYear = element.date.split("/")[2];
              that.chartDataYear.push([currentYear, parseInt(element.position)]);
            });

            that.YearLoading = false;
          })

          .catch(function(error) {

            console.log(error);
          });
      },

      getDataMonths() {

        var that = this;
        this.MonthLoading = true;
        var urlGetPostionMonth =
          that.$executioEnvironment +
          "GetTotalPositionAtInterval" +
          "?date_debut=01/01/" +
          (1900 + that.selectedDate.getYear()) +
          "&date_fin=31/12/" +
          (1900 + that.selectedDate.getYear()) +
          "&mode=mois";

        axios.get(urlGetPostionMonth).then(function(response) {

          that.chartDataMonth = [];
          that.chartDataMonth.push(["Mois", "Position"]);
          response.data.forEach(element => {
            let currentMonth = element.date.split("/")[1];
            that.chartDataMonth.push([currentMonth, parseInt(element.position)]);
            that.MonthLoading = false;
          });
        });
      },

      getDataDay() {

        var that = this;
        var date = "";
        if (this.selectedDate.getMonth() != 11) {

          date = new Date(
            this.selectedDate.getMonth() +
              2 +
              "/01/" +
              (1900 + this.selectedDate.getYear())
          );
          date.setDate(date.getDate() - 1);
        } 
        else {
          date = new Date(
            this.selectedDate.getMonth() +
              1 +
              "/31/" +
              (1900 + this.selectedDate.getYear())
          );
        }

        var urlGetPostionDays =
          that.$executioEnvironment +
          "GetTotalPositionAtInterval" +
          "?date_debut=01/" +
          (that.selectedDate.getMonth() + 1) +
          "/" +
          (1900 + that.selectedDate.getYear()) +
          "&date_fin=" +
          date.getDate() +
          "/" +
          (that.selectedDate.getMonth() + 1) +
          "/" +
          (1900 + that.selectedDate.getYear()) +
          "&mode=jours";

        axios.get(urlGetPostionDays).then(function(response) {

          that.chartDataDay = [];
          that.chartDataDay.push(["Jour", "Position"]);
          response.data.forEach(element => {
            let currentDay = element.date.split("/")[0];
            that.chartDataDay.push([currentDay, parseInt(element.position)]);
          });
          that.DayLoading = false;
        });
      },

      getDataAccountDay() {

        var that = this;
        var date = "";
        this.DayAccountLoading = true;

        if (this.selectedAccountMonth != 12) {

          date = new Date(
            this.selectedAccountMonth +
              1 +
              "/01/" +
              (1900 + this.selectedDate.getYear())
          );
          date.setDate(date.getDate() - 1);
        } 
        else {

          date = new Date(
            this.selectedAccountMonth +
              "/31/" +
              (1900 + this.selectedDate.getYear())
          );
        }

        var urlGetPostionDays =
          that.$executioEnvironment +
          "GetTotalPositionAtInterval" +
          "?date_debut=01/" +
          that.selectedAccountMonth +
          "/" +
          (1900 + that.selectedDate.getYear()) +
          "&date_fin=" +
          date.getDate() +
          "/" +
          that.selectedAccountMonth +
          "/" +
          (1900 + that.selectedDate.getYear()) +
          "&mode=jours" +
          "&compte=" +
          that.selectedAccount;

        axios.get(urlGetPostionDays).then(function(response) {

          that.chartDataDayAccount = [];
          that.chartDataDayAccount.push(["Jour", "Position"]);
          response.data.forEach(element => {
            let currentDay = element.date.split("/")[0];
            that.chartDataDayAccount.push([
              currentDay,
              parseInt(element.position)
            ]);
          });
          that.DayAccountLoading = false;
        });
      },

      getDataAccountMonth() {

        this.MonthAccountLoading = true;
        var that = this;

        var urlGetPostionMonthAccount =
          that.$executioEnvironment +
          "GetTotalPositionAtInterval" +
          "?date_debut=01/01/" +
          (1900 + that.selectedDate.getYear()) +
          "&date_fin=31/12/" +
          (1900 + that.selectedDate.getYear()) +
          "&mode=mois" +
          "&compte=" +
          that.selectedAccount;

        axios.get(urlGetPostionMonthAccount).then(function(response) {

          that.chartDataMonthAccount = [];
          that.chartDataMonthAccount.push(["Mois", "Position"]);
          response.data.forEach(element => {
            let currentMonth = element.date.split("/")[1];
            that.chartDataMonthAccount.push([
              currentMonth,
              parseInt(element.position)
            ]);
          });
          that.MonthAccountLoading = false;
        });
        
        this.getDataAccountDay();
      }
    }
  };

</script>

<style>

  svg > g > g:last-child {
    
    pointer-events: none;
  }

</style>