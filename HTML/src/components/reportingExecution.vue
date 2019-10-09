<template>

  <div>

    <v-app>

      <v-snackbar v-model="snackbar" :color="snackbarColor" :bottom="snackbarY==='bottom'" :left="snackbarX==='left'" :multi-line="snackbarMode==='multi-line'" :right="snackbarX==='right'" :timeout="3700" :top="snackbarY==='top'" :vertical="snackbarMode==='vertical'">
        {{ snackbarText }}
        <v-btn dark flat @click="snackbar=false">Fermer</v-btn>
      </v-snackbar>

      <v-container fluid grid-list-md>

        <v-layout row wrap style=" background-color: #FFA726">
                
          <v-flex>

            <v-autocomplete dark v-model="selectedYear" :items="selectAbleYear" v-on:change="refreshBudgetBilan" label="Année" ></v-autocomplete>
          
          </v-flex>
  
        </v-layout>

      <div v-if="loading">

        <template>
          <div>
            <v-app>
              <v-layout align-center justify-center row fill-height>
                <v-flex md12>
                  <v-progress-circular :size="100" :width="17" color="amber" indeterminate>
                  </v-progress-circular>
                </v-flex>
              </v-layout>
            </v-app>    
          </div>
        </template>

      </div>

      <div style="margin-top: 4px;" v-else>

        <v-layout align-space-around column v-if="!showable">

          <v-alert style="width: 100%" :value="true" type="info">
            Sélectionnez une année afin d'avoir accès au bilan
          </v-alert>

        </v-layout>

        <v-layout v-resize="onResize" align-baseline="true" row wrap v-else>

          <v-btn v-if="!isMobile" outline block large round color="error" v-on:click="exportPDF()">
            Exporter au Format PDF 
            <v-icon right dark>description</v-icon>
          </v-btn>

          <v-btn v-else outline block large round color="error" v-on:click="exportPDF()">
            Exporter au Format PDF 
            <v-icon right dark>description</v-icon>
          </v-btn>

          <v-alert style="width: 100%" :value="true" :type="bilanAnnuelType" v-if="!isMobile">

            <v-layout align-center justify-space-between row fill-height>

              <v-flex>
                <h1><strong>Objectif :</strong> {{ formatNumber(objectifAnnuel) }} €</h1>
              </v-flex>

              <v-flex>
                <h1><strong>Mouvement :</strong> {{ formatNumber(decaissementAnnuel) }} €</h1>
              </v-flex>

              <v-flex>
                <h1><strong>Différentiel :</strong> {{ bilanAnnuel }} €</h1>
              </v-flex>

            </v-layout>

          </v-alert>

          <v-alert style="width: 100%" :value="true" :type="bilanAnnuelType" v-else>

            <v-layout align-center justify-space-between column fill-height>

              <v-flex>
                <h2><strong>Objectif :</strong> {{ formatNumber(objectifAnnuel) }} €</h2>
              </v-flex>

              <v-flex>
                <h2><strong>Mouvement :</strong> {{ formatNumber(decaissementAnnuel) }} €</h2>
              </v-flex>

              <v-flex>
                <h2><strong>Différentiel :</strong> {{ bilanAnnuel }} €</h2>
              </v-flex>

            </v-layout>

          </v-alert>
    
          <template>

            <v-expansion-panel dark>

              <v-expansion-panel-content v-for="(item, i) in allClassification.length" :key="i" :class="eachClassificationColor[allClassification[i]]" style="margin-top: 3px">

                <template v-slot:header>
                  <div>{{ formatClassificationName(allClassification[i]) }}</div>
                </template>

                <template v-slot:actions>

                  <v-layout align-center justify-space-around row fill-height v-if="!isMobile">

                    <v-flex>
                      <div style="width: 300px" :key="reRenderKey"><strong>Objectif :</strong> {{ formatNumber(eachClassificationTotalObjectif[allClassification[i]]) }} €</div>
                    </v-flex>

                    <v-flex>
                      <div style="width: 300px" :key="reRenderKey"><strong>Mouvement :</strong> {{ formatNumber(eachClassificationTotalDecaissement[allClassification[i]]) }} €</div>
                    </v-flex>

                    <v-flex>
                      <div style="width: 300px" :key="reRenderKey"><strong>Différentiel :</strong> {{ formatNumber(eachClassificationBilan[allClassification[i]]) }} €</div>
                    </v-flex>

                  </v-layout>

                  <v-layout align-center justify-space-around column fill-height v-else>

                    <v-flex>
                      <div :key="reRenderKey"><strong>Objectif :</strong> {{ formatNumber(eachClassificationTotalObjectif[allClassification[i]]) }} €</div>
                    </v-flex>

                    <v-flex>
                      <div :key="reRenderKey"><strong>Mouvement :</strong> {{ formatNumber(eachClassificationTotalDecaissement[allClassification[i]]) }} €</div>
                    </v-flex>

                    <v-flex>
                      <div :key="reRenderKey"><strong>Différentiel :</strong> {{ formatNumber(eachClassificationBilan[allClassification[i]]) }} €</div>
                    </v-flex>

                  </v-layout>

                </template>

                <v-expansion-panel dark>

                  <v-expansion-panel-content v-for="(item, j) in allClassificationType[allClassification[i]].length" :key="j" :class="eachUnderClassificationColor[allClassification[i]+allClassificationType[allClassification[i]][j].name]" style="margin-top: 1px">

                    <template v-slot:header>
                      <div>{{ allClassificationType[allClassification[i]][j].name }}</div>
                    </template>

                    <template v-slot:actions>

                      <v-layout align-center justify-space-around row fill-height v-if="!isMobile">

                        <v-flex>
                          <div style="width: 250px" :key="reRenderKey"><strong>Objectif :</strong> {{ formatNumber(parseFloat(budgetBilanData[allClassification[i]][allClassificationType[allClassification[i]][j].name].objectif)) }} €</div>
                        </v-flex>

                        <v-flex>
                          <div style="width: 250px" :key="reRenderKey"><strong>Mouvement :</strong> {{ formatNumber(parseFloat(eachUnderClassificationTotal[allClassification[i]+"/"+allClassificationType[allClassification[i]][j].name])) }} €</div>
                        </v-flex>

                      </v-layout>                      

                      <v-layout align-center justify-space-around column fill-height v-else>

                        <v-flex>
                          <div :key="reRenderKey"><strong>Objectif :</strong> {{ formatNumber(parseFloat(budgetBilanData[allClassification[i]][allClassificationType[allClassification[i]][j].name].objectif)) }} €</div>
                        </v-flex>

                        <v-flex>
                          <div :key="reRenderKey"><strong>Mouvement :</strong> {{ formatNumber(parseFloat(eachUnderClassificationTotal[allClassification[i]+"/"+allClassificationType[allClassification[i]][j].name])) }} €</div>
                        </v-flex>

                      </v-layout>
                      
                    </template>

                    <v-card light>

                      <v-data-table :headers="headers" :items="budgetBilanData[allClassification[i]][allClassificationType[allClassification[i]][j].name].decaissements" :hide-headers="isMobile" :class="{mobile: isMobile}" hide-actions>

                        <template v-slot:items="props">

                          <tr v-if="!isMobile">
                            <td>{{ props.item.operation }}</td>
                            <td>{{ formatNumber(parseFloat(props.item.montant_ttc)) }}</td>
                          </tr>

                          <tr v-else>
                            <td>
                              <ul class="flex-content">
                                <li class="flex-item" data-label="Opération" style="margin-top:25px">{{ props.item.operation }}</li>
                                <li class="flex-item" data-label="Montant TTC (€)" style="margin-top:25px">{{ formatNumber(parseFloat(props.item.montant_ttc)) }}</li>                                                        
                              </ul>
                            </td>
                          </tr>

                        </template>

                        <template v-slot:no-data>
                          Il n'y à pas de decaissement rentré pour ce mois
                        </template>

                      </v-data-table>

                    </v-card>
                                  
                  </v-expansion-panel-content>

                </v-expansion-panel>
                           
              </v-expansion-panel-content>

            </v-expansion-panel>

          </template>

        </v-layout>

      </div>
           
      </v-container>

    </v-app>

  </div>

</template>

<script>

const axios = require("axios");

export default {

  name: "reportingExecution",

  data() {

    return {

      isMobile: false,

      loading: false,

      snackbar: false,
      snackbarText: "",
      snackbarColor: "",
      snackbarY: 'top',
      snackbarX: null,
      snackbarMode: '',
      
      showable: false,

      selectAbleYear: [],

      selectedYear: "",

      budgetBilanData: [],

      allClassification : [],
      allClassificationType: [],
 
      reRenderKey: 0,
      
      headers: [
        
        { text: "Opération", value: "operation", sortable: true },
        { text: "Montant TTC (€)", value: "montant_ttc", sortable: true },
      ],

      bilanAnnuel: 0,
      bilanAnnuelType: "success",
      objectifAnnuel: 0,
      decaissementAnnuel: 0,
     
      eachClassificationColor: {},
      eachUnderClassificationColor: {},

      eachClassificationBilan: {},
      eachClassificationTotalObjectif: {},
      eachClassificationTotalDecaissement: {},

      eachUnderClassificationTotal: {}
    };
  }, 

  created() {
    
    this.getYear();
    
    this.getClassification();
    this.selectedYear=(1900+new Date().getYear());
    this.refreshBudgetBilan();
  },
  
  methods: {
    exportPDF(){
      window.open(this.$executioEnvironment+'GeneratePDF?annee='+this.selectedYear);
    },

    getYear(){
    
      var actualYear = new Date().getFullYear();

      for(var y=2000; y <= (actualYear +10); y++){

        this.selectAbleYear.push(y);
      }
    },
    
    getBudgetBilanData(){

      let context = this;
      
      var urlBudgetBilan = this.$executioEnvironment+"GetBudgetBilan";

      return axios.get(urlBudgetBilan, {

        params: {

          "annee": context.selectedYear,
        }

      }).then(function (response) {

        context.budgetBilanData = response.data;        

        console.log(context.budgetBilanData);
      })
      .catch(function (error) {
        
        console.log(error);
      });
    },   

    getClassification(){

      let context=this;
      
      var urlAccountType = this.$executioEnvironment+"getClassification";
       
      axios.get(urlAccountType).then(function (response) {
      
        context.allClassificationType = response.data;

        for(var element in response.data) {

          context.allClassification.push(element);
        }
      })
      .catch(function (error) {
        
        console.log(error);
      });
    },

    refreshBudgetBilan: function () {

      if(null != this.selectedYear && undefined != this.selectedYear && this.selectedYear != ""){
                
        this.loading = true;

        this.getBudgetBilanData().then((returnVal) => {

          this.bilanAnnuel = 0;
          this.objectifAnnuel = 0;
          this.decaissementAnnuel = 0;

          this.calculateAllClassificationTotal();

          this.calculateBilanAnnuel();
              
          this.showable = true;

          this.reRenderKey += 1;
          
          this.loading = false;
        });
      }
    },

    calculateAllClassificationTotal(){

      for(var classificationKey in this.budgetBilanData){

        var classification = this.budgetBilanData[classificationKey];

        this.eachClassificationBilan[classificationKey] = 0;
        this.eachClassificationTotalObjectif[classificationKey] = 0;
        this.eachClassificationTotalDecaissement[classificationKey] = 0;

        for(var underClassificationKey in classification){
          
          var underClassification = classification[underClassificationKey];
          
          this.eachClassificationBilan[classificationKey] += parseFloat(underClassification.objectif);
          this.eachClassificationTotalObjectif[classificationKey] += parseFloat(underClassification.objectif);

          this.objectifAnnuel += parseFloat(underClassification.objectif);
          
          this.eachUnderClassificationTotal[classificationKey+"/"+underClassificationKey] = 0;

          for(var decaissementKey in underClassification.decaissements){

            var decaissement = underClassification.decaissements[decaissementKey];

            if(null != decaissement.operation && null != decaissement.montant_ttc){

              this.eachClassificationBilan[classificationKey] -= parseFloat(decaissement.montant_ttc);
              this.eachClassificationTotalDecaissement[classificationKey] += parseFloat(decaissement.montant_ttc);

              this.decaissementAnnuel += parseFloat(decaissement.montant_ttc);
              
              this.eachUnderClassificationTotal[classificationKey+"/"+underClassificationKey] += parseFloat(decaissement.montant_ttc);
            }
          }

          if(this.eachUnderClassificationTotal[classificationKey+"/"+underClassificationKey] < underClassification.objectif){

            this.eachUnderClassificationColor[classificationKey+underClassificationKey] = "light-green lighten-2";
          }
          else if(this.eachUnderClassificationTotal[classificationKey+"/"+underClassificationKey] > underClassification.objectif){

            this.eachUnderClassificationColor[classificationKey+underClassificationKey] = "red lighten-2";
          }
          else if(this.eachUnderClassificationTotal[classificationKey+"/"+underClassificationKey] == 0){

            this.eachUnderClassificationColor[classificationKey+underClassificationKey] = "grey lighten-1";         
          }
        }

        if(this.eachClassificationBilan[classificationKey] > 0){

          this.eachClassificationColor[classificationKey] = "light-green";
        }
        else if(this.eachClassificationBilan[classificationKey] < 0){

          this.eachClassificationColor[classificationKey] = "red";
        }
        else if(this.eachClassificationBilan[classificationKey] == 0){

          this.eachClassificationColor[classificationKey] = "grey darken-1";         
        }
      }
    },

    calculateBilanAnnuel(){

      this.bilanAnnuel = 0;
      this.bilanAnnuelType = "success";

      for(var key in this.eachClassificationBilan){

        this.bilanAnnuel += this.eachClassificationBilan[key];
      }      

      if(this.bilanAnnuel>0){

        var formatedBilanAnnuel = this.formatNumber(this.bilanAnnuel);

        this.bilanAnnuel = "+ " + formatedBilanAnnuel;
      }
      else if(this.bilanAnnuel<0){

        var formatedBilanAnnuel = this.formatNumber(this.bilanAnnuel.toString().split('-')[1] );

        this.bilanAnnuel = "- " + formatedBilanAnnuel;

        this.bilanAnnuelType = "error";
      }
    },

    onResize() {

      if(window.innerWidth < 769) {

        this.isMobile = true;
      }
      else {

        this.isMobile = false;
      }
    },    

    formatNumber(number){

            if(null!=number && undefined!=number && ""!=number){

                number += "";

                var splited = number.split(".");

                var number = splited[0];

                var decimal = splited[1];

                var formatedNumber = "";

                var p = 1;

                for(var i=number.length-1; i >= 0; i--){

                    formatedNumber += number.charAt(i);

                    if(p==3 && i>0){

                        formatedNumber += ",";

                        p=0;
                    }

                    p++;
                }

                formatedNumber = formatedNumber.split("").reverse().join("");

                if(null!=decimal && undefined!=decimal && decimal.length!=0){
                
                    if(decimal.length>2){

                        decimal = decimal.substring(0, 2);
                    }
                    else if(decimal.length==1){

                        decimal += "0";
                    }

                    formatedNumber += "." + decimal;
                }
                else{

                    formatedNumber += ".00";
                }
            }
            else {

                formatedNumber = "0.00";
            }

            return formatedNumber;
        },

    formatClassificationName(classificationName){

      var splited = classificationName.split("_");

      var formatedClassificationName = "";

      for(var part in splited){

        if(part!=0){

          formatedClassificationName += " ";
        }

        formatedClassificationName += splited[part];
      }

      return formatedClassificationName;
    }
  }
}
</script>

<style scoped>

</style>