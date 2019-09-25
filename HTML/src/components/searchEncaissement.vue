<template>

  <div>

    <v-app>

      <div v-if="errored">
        <p>Une erreur est survenue lors de la récupération des données !</p>
      </div>

      <div v-else>
        
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

        <div v-else>                

          <v-layout align-space-around column>

            <v-alert style="width: 100%" :value="true" type="info" v-if="!isMobile">

              <v-layout align-center justify-space-between row fill-height>

                <v-flex>
                  <h1><strong>Totaux Encaissements Recherchés</strong></h1>
                </v-flex>                  

                <v-flex>
                  <h1><strong>Total TTC =</strong> {{ formatNumber(parseFloat(totalTTC)) }}  €</h1>
                </v-flex>

                <v-flex>
                  <h1><strong>Total TVA =</strong> {{ formatNumber(parseFloat(totalTVA)) }} €</h1>
                </v-flex>

                <v-flex>
                  <h1><strong>Total HT =</strong> {{ formatNumber(parseFloat(totalHT)) }} €</h1>
                </v-flex>

              </v-layout>

            </v-alert>

            <v-alert style="width: 100%" :value="true" type="info" v-else>

              <v-layout align-center justify-space-between column fill-height>

                <v-flex>
                  <h2><strong>Totaux Encaissements Recherchés</strong></h2>
                </v-flex>                  

                <v-flex>
                  <h2><strong>Total TTC =</strong> {{ formatNumber(parseFloat(totalTTC)) }}  €</h2>
                </v-flex>

                <v-flex>
                  <h2><strong>Total TVA =</strong> {{ formatNumber(parseFloat(totalTVA)) }} €</h2>
                </v-flex>

                <v-flex>
                  <h2><strong>Total HT =</strong> {{ formatNumber(parseFloat(totalHT)) }} €</h2>
                </v-flex>

              </v-layout>

            </v-alert>

          </v-layout>        

          <v-layout row wrap style=" background-color: #8BC34A" v-if="!isMobile">

            <v-flex>
              <v-text-field dark append-icon="search" label="Société" @input="filterSociete"></v-text-field>
            </v-flex>
            <v-flex>
              <v-text-field dark append-icon="search" label="Banque" @input="filterBanque"></v-text-field>
            </v-flex>
            <v-flex>
              <v-text-field dark append-icon="search" label="Compte" @input="filterCompte"></v-text-field>
            </v-flex>
            <v-flex>
              <v-text-field dark append-icon="search" label="Total HT (€)" @input="filterTotalHT"></v-text-field>
            </v-flex>
            <v-flex>
              <v-text-field dark append-icon="search" label="Total TTC (€)" @input="filterTotalTTC"></v-text-field>
            </v-flex>
            <v-flex>
              <v-text-field dark append-icon="search" label="Total TVA (€)" @input="filterTotalTVA"></v-text-field>
            </v-flex>
            <v-flex>
              <v-text-field dark append-icon="search" label="Date Règlement" @input="filterDateReglement"></v-text-field>
            </v-flex>
            <v-flex>
              <v-text-field dark append-icon="search" label="Mode de Paiement" @input="filterModePaiement"></v-text-field>
            </v-flex>

          </v-layout>

          <v-layout v-resize="onResize" column>
              
            <v-menu v-if="isMobile" offset-y :nudge-left="170" :close-on-content-click="false">

              <v-btn block slot="activator" color="amber" dark>
                Trier <v-icon>swap_vert</v-icon>
              </v-btn>>
            
              <v-list>
                <v-list-tile v-for="item in headers" :key="item.value" @click="changeSort(item.value)">
                  <v-list-tile-title>
                    {{ item.text }}
                    <v-icon v-if="pagination.sortBy === item.value">{{pagination.descending ? 'arrow_downward':'arrow_upward'}}</v-icon>
                  </v-list-tile-title>
                </v-list-tile>
              </v-list>
            
            </v-menu>
            
            <v-text-field v-model="search" prepend-icon="search" label="Rechercher un mot clé dans tous le tableau" single-line hide-details v-if="isMobile"/>
            
            <v-data-table :headers="headers" :items="rows" :search="filters" :custom-filter="customFilter" :pagination.sync="pagination" :hide-headers="isMobile" :class="{mobile: isMobile}"  v-if="!isMobile">
            
              <template slot="items" slot-scope="props">
              
                <tr>
                
                  <td>{{ props.item.societe }}</td>
                  <td>{{ props.item.banque }}</td>
                  <td>{{ props.item.compte }}</td>
                  <td>{{ formatNumber(props.item.total_ht) }}</td>
                  <td>{{ formatNumber(props.item.total_tva) }}</td>
                  <td>{{ formatNumber(props.item.total_ttc) }}</td>
                  <td>{{ props.item.date_reglement }}</td>
                  <td>{{ props.item.mode_paiement }}</td>

                  <td v-if="props.item.transfert_compte"><v-icon color="success">done</v-icon></td>
                  <td v-else><v-icon color="red">clear</v-icon></td>
                
                </tr>
      
              </template>
    
              <v-alert slot="no-results" :value="true" color="error" icon="warning">
                Votre recherche  n'a renvoyée aucun résultat !
              </v-alert>
    
            </v-data-table>

            <v-data-table :headers="headers" :items="rows" :search="search" :pagination.sync="pagination" :hide-headers="isMobile" :class="{mobile: isMobile}" v-else>
            
              <template slot="items" slot-scope="props">
                             
                <tr>
                
                  <td>
                
                    <ul class="flex-content">
                
                      <li class="flex-item" data-label="Société" style="margin-top:25px">{{ props.item.societe }}</li>
                      <li class="flex-item" data-label="Banque" style="margin-top:25px">{{ props.item.banque }}</li>

                      <li class="flex-item" data-label="Compte">{{ props.item.compte }}</li>
                      <li class="flex-item" data-label="Total HT (€)">{{ formatNumber(props.item.total_ht) }}</li>
                      <li class="flex-item" data-label="Total TVA (€)">{{ formatNumber(props.item.total_tva) }}</li>
                      <li class="flex-item" data-label="Total TTC (€)">{{ formatNumber(props.item.total_ttc) }}</li>
                      <li class="flex-item" data-label="Date Règlement">{{ props.item.date_reglement }}</li>
                      <li class="flex-item" data-label="Mode de Paiement">{{ props.item.mode_paiement }}</li>

                      <li class="flex-item" data-label="Transfert Compte" v-if="props.item.transfert_compte"><v-icon color="success">done</v-icon></li>
                      <li class="flex-item" data-label="Transfert Compte" v-else><v-icon color="red">clear</v-icon></li>

                    </ul>

                  </td>

                </tr>
      
              </template>
    
              <v-alert slot="no-results" :value="true" color="error" icon="warning">
                Votre recherche  n'a renvoyée aucun résultat !
              </v-alert>
    
            </v-data-table>

          </v-layout>            

        </div>
              
      </div>

    </v-app>

  </div>
  
</template>

<script>

const axios = require('axios');

export default {

  name: "searchEncaissement",

  data() {

    return {

      pagination: {

        sortBy: "societe",
        rowsPerPage: -1
      },

      selected: [],      
      search: "",
      isMobile: false,
      rows: null,
      loading: true,
      errored: false,

      headers: [
        { text: "Société", value: "societe", sortable: true },
        { text: "Banque", value: "banque", sortable: true },
        { text: "Compte", value: "compte", sortable: true },
        { text: "Total HT (€)", value: "total_ht", sortable: true },
        { text: "Total TVA (€)", value: "total_tva", sortable: true },
        { text: "Total TTC (€)", value: "total_ttc", sortable: true },
        { text: "Date Règlement", value: "date_reglement", sortable: true },
        { text: "Mode de Paiement", value: "mode_paiement", sortable: true },
        { text: "Transfert Compte", value: "transfert_compte", sortable: true }
      ],

      filters: {

        societe: "",
        banque: "",
        compte: "",
        total_ht: "",
        total_ttc: "",
        total_tva: "",
        date_reglement: "",
        mode_paiement: ""
      },

      totalHT: 0,
      totalTTC: 0,
      totalTVA: 0
    };
  },

  created() {

    this.getItems();
  },
  methods: {
    
    getItems(){

      let context = this;

      var url = this.$executioEnvironment+"GetAllEncaissement";

      axios.get(url).then(function (response) {

        for(var key in response.data) {

          response.data[key].date_reglement = response.data[key].date_reglement.split("T")[0];
        }
                
        context.rows = response.data;
        context.loading = false;
      })
      .catch(function (error) {
        
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

    toggleAll() {

      if (this.selected.length) {

       this.selected = [];
      } 
      else {

        this.selected = this.rows.slice();
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

            formatedNumber += ".";

            p=0;
          }

          p++;
        }

        formatedNumber = formatedNumber.split("").reverse().join("");

        if(null!=decimal && undefined!=decimal && decimal.length!=0){
          
          if(decimal.length>2){

            decimal = decimal.substring(0, 2);
          }

          formatedNumber += "," + decimal;
        }
      }

      return formatedNumber;
    },

    changeSort(column) {

      console.log(column);

       if (this.pagination.sortBy === column) {

         this.pagination.descending = !this.pagination.descending;
       } 
       else {
         
          this.pagination.sortBy = column;
          this.pagination.descending = false;
       }
     },

     customFilter(items, filters, filter, headers) {
       
        const customFilter = new this.$MultiFilters(items, filters, filter, headers);

        customFilter.registerFilter('societe', function (searchedSociete, items) {

          if (searchedSociete.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.societe)
            return item.societe.toLowerCase().includes(searchedSociete.toLowerCase());

          }, searchedSociete);          

        });

        customFilter.registerFilter('banque', function (searchedBanque, items) {

          if (searchedBanque.trim() === ''){
            
            return items;
          }

          return items.filter(item => {
  
            if(null!=item.banque)
            return item.banque.toLowerCase().includes(searchedBanque.toLowerCase());

          }, searchedBanque);

        });

        customFilter.registerFilter('compte', function (searchedCompte, items) {

          if (searchedCompte.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.compte)
            return item.compte.toLowerCase().includes(searchedCompte.toLowerCase());

          }, searchedCompte);

        });

        customFilter.registerFilter('total_ht', function (searchedTotalHT, items) {

          if (searchedTotalHT.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.total_ht)
            return item.total_ht.toString().includes(searchedTotalHT.toLowerCase());

          }, searchedTotalHT);          

        });

        customFilter.registerFilter('total_ttc', function (searchedTotalTTC, items) {

          if (searchedTotalTTC.trim() === ''){
            
            return items;
          }

          return items.filter(item => {
            
            if(null!=item.total_ttc)
            return item.total_ttc.toString().includes(searchedTotalTTC.toLowerCase());

          }, searchedTotalTTC);          

        });

        customFilter.registerFilter('total_tva', function (searchedTotalTVA, items) {

          if (searchedTotalTVA.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.total_tva)
            return item.total_tva.toString().includes(searchedTotalTVA.toLowerCase());

          }, searchedTotalTVA);          

        });

        customFilter.registerFilter('date_reglement', function (searchedDateReglement, items) {

          if (searchedDateReglement.trim() === ''){
            
            return items;
          }

          return items.filter(item => {
            
            if(null!=item.date_reglement)
            return item.date_reglement.toLowerCase().includes(searchedDateReglement.toLowerCase());

          }, searchedDateReglement);          

        });

        customFilter.registerFilter('mode_paiement', function (searchedModePaiement, items) {

          if (searchedModePaiement.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.mode_paiement)
            return item.mode_paiement.toLowerCase().includes(searchedModePaiement.toLowerCase());

          }, searchedModePaiement);

        });

        var filteredItems = customFilter.runFilters();

        this.calculateTotal(filteredItems);

        return filteredItems;
      },
      
      filterSociete(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {societe: val});
      },

      filterBanque(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {banque: val});
      },

      filterCompte(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {compte: val});
      },
      
      filterTotalHT(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {total_ht: val});
      },

      filterTotalTTC(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {total_ttc: val});
      },

      filterTotalTVA(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {total_tva: val});
      },

      filterDateReglement(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {date_reglement: val});
      },

      filterModePaiement(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {mode_paiement: val});
      },      

      calculateTotal(filteredItems){

        this.totalHT = 0;
        this.totalTTC = 0;
        this.totalTVA = 0;

        for(var key in filteredItems){          

          this.totalHT += filteredItems[key].total_ht;
          this.totalTTC += filteredItems[key].total_ttc;
          this.totalTVA += filteredItems[key].total_tva;
        }
      }
  }
};
</script>