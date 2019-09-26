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
                  <h1><strong>Totaux Décaissements Recherchés</strong></h1>
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
                  <h2><strong>Totaux Décaissements Recherchés</strong></h2>
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
              <v-text-field dark append-icon="search" label="Opération" @input="filterOperation"></v-text-field>
            </v-flex>

            <v-flex>
              <v-text-field dark append-icon="search" label="Banque" @input="filterBanque"></v-text-field>
            </v-flex>

            <v-flex>
              <v-text-field dark append-icon="search" label="Compte" @input="filterCompte"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Mode de Paiement" @input="filterModePaiement"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Date Compta" @input="filterDateCompta"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Date Opération" @input="filterDateOperation"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Date Valeur" @input="filterDateValeur"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Montant TTC" @input="filterMontantTTC"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="TVA Deductible" @input="filterTVADeductible"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Montant HT" @input="filterMontantHT"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Classification" @input="filterClassification"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Sous Classification" @input="filterSousClassification"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Details" @input="filterDetails"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Mois Valeur" @input="filterMoisValeur"></v-text-field>
            </v-flex>
    
            <v-flex>
              <v-text-field dark append-icon="search" label="Date Ajout" @input="filterDateAjout"></v-text-field>
            </v-flex>

          </v-layout>

          <v-layout v-resize="onResize" column>
                
            <v-menu v-if="isMobile" offset-y :nudge-left="170" :close-on-content-click="false">
    
              <v-btn block slot="activator" color="amber" dark>
                Trier <v-icon>swap_vert</v-icon>
              </v-btn>
    
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
        
            <v-data-table :headers="headers" :items="rows" :search="filters" :custom-filter="customFilter" :pagination.sync="pagination" :hide-headers="isMobile" :class="{mobile: isMobile}" v-if="!isMobile">
        
              <template slot="items" slot-scope="props">
        
                <tr>
        
                  <td>{{ props.item.operation }}</td>
                  <td>{{ props.item.banque }}</td>
                  <td>{{ props.item.compte }}</td>
                  <td>{{ props.item.mode_paiement }}</td>
                  <td>{{ props.item.date_compta }}</td>
                  <td>{{ props.item.date_operation }}</td>
                  <td>{{ props.item.date_valeur }}</td>
                  <td>{{ formatNumber(props.item.montant_ttc) }}</td>
                  <td>{{ formatNumber(props.item.tva_deductible) }}</td>
                  <td>{{ formatNumber(props.item.montant_ht) }}</td>

                  <td v-if="props.item.versement_tva"><v-icon color="success">done</v-icon></td>
                  <td v-else><v-icon color="red">clear</v-icon></td>

                  <td>{{ props.item.classification }}</td>
                  <td>{{ props.item.sous_classification }}</td>
                  <td>{{ props.item.details }}</td>

                  <td v-if="props.item.facture_verifie"><v-icon color="success">done</v-icon></td>
                  <td v-else><v-icon color="red">clear</v-icon></td>
                      
                  <td>{{ props.item.mois_valeur }}</td>

                  <td v-if="props.item.transfert_compte"><v-icon color="success">done</v-icon></td>
                  <td v-else><v-icon color="red">clear</v-icon></td>

                  <td>{{ props.item.date_ajout }}</td>

                </tr>

              </template>

              <v-alert slot="no-results" :value="true" color="error" icon="warning">
                Votre recherche n'a renvoyée aucun résultat !
              </v-alert>

            </v-data-table>

            <v-data-table :headers="headers" :items="rows" :search="search" :pagination.sync="paginationMobile" :hide-headers="isMobile" :class="{mobile: isMobile}" v-else>
        
              <template slot="items" slot-scope="props">
        
                <tr>

                  <td>

                    <ul class="flex-content">

                      <li class="flex-item" data-label="Opération">{{ props.item.operation }}</li>
                      <li class="flex-item" data-label="Banque">{{ props.item.banque }}</li>
                      <li class="flex-item" data-label="Compte">{{ props.item.compte }}</li>
                      <li class="flex-item" data-label="Mode de Paiement">{{ props.item.mode_paiement }}</li>
                      <li class="flex-item" data-label="Date Comptabilité">{{ props.item.date_compta }}</li>
                      <li class="flex-item" data-label="Date Opération">{{ props.item.date_operation }}</li>
                      <li class="flex-item" data-label="Date Valeur">{{ props.item.date_valeur }}</li>
                      <li class="flex-item" data-label="Montant TTC (€)">{{ formatNumber(props.item.montant_ttc) }}</li>
                      <li class="flex-item" data-label="TVA Deductible (€)">{{ formatNumber(props.item.tva_deductible) }}</li>
                      <li class="flex-item" data-label="Montant HT (€)">{{ formatNumber(props.item.montant_ht) }}</li>

                      <li class="flex-item" data-label="Versement TVA" v-if="props.item.versement_tva"><v-icon color="success">done</v-icon></li>
                      <li class="flex-item" data-label="Versement TVA" v-else><v-icon color="red">clear</v-icon></li>
                      
                      <li class="flex-item" data-label="Classification">{{ props.item.classification }}</li>
                      <li class="flex-item" data-label="Sous Classification">{{ props.item.sous_classification }}</li>
                      <li class="flex-item" data-label="Details">{{ props.item.details }}</li>

                      <li class="flex-item" data-label="Facture Verifiée" v-if="props.item.facture_verifie"><v-icon color="success">done</v-icon></li>
                      <li class="flex-item" data-label="Facture Verifiée" v-else><v-icon color="red">clear</v-icon></li>

                      <li class="flex-item" data-label="Mois Valeur">{{ props.item.mois_valeur }}</li>
                            
                      <li class="flex-item" data-label="Transfert Compte" v-if="props.item.transfert_compte"><v-icon color="success">done</v-icon></li>
                      <li class="flex-item" data-label="Transfert Compte" v-else><v-icon color="red">clear</v-icon></li>

                      <li class="flex-item" data-label="Date d'Ajout">{{ props.item.date_ajout }}</li>

                    </ul>

                  </td>

                </tr>

              </template>

              <v-alert slot="no-results" :value="true" color="error" icon="warning">
                Votre recherche n'a renvoyée aucun résultat !
              </v-alert>

            </v-data-table>

          </v-layout>     

        </div>   

      </div>

    </v-app>

  </div>
  
</template>

<script>
import { format } from 'path';

const axios = require('axios');

export default {

  name: "searchDecaissement",

  data() {

    return {

      pagination: {

        sortBy: "date_ajout",
        descending: true,
        rowsPerPage: -1
      },

      paginationMobile: {

        sortBy: "date_ajout",
        descending: true,
        rowsPerPage: 5
      },

      selected: [],      
      search: "",
      isMobile: false,
      rows: null,
      loading: true,
      errored: false,

      headers: [
        { text: "Opération", value: "operation", sortable: true },
        { text: "Banque", value: "banque", sortable: true },
        { text: "Compte", value: "compte", sortable: true },
        { text: "Mode de Paiement", value: "mode_paiement", sortable: true },
        { text: "Date Comptabilite", value: "date_compta", sortable: true },
        { text: "Date Opération", value: "date_operation", sortable: true },
        { text: "Date Valeur", value: "date_valeur", sortable: true },
        { text: "Montant TTC (€)", value: "montant_ttc", sortable: true },
        { text: "TVA Deductible (€)", value: "tva_deductible", sortable: true },
        { text: "Montant HT (€)", value: "montant_ht", sortable: true },
        { text: "Versement TVA", value: "versement_tva", sortable: true },
        { text: "Classification", value: "classification", sortable: true },
        { text: "Sous Classification", value: "sous_classification", sortable: true },
        { text: "Details", value: "details", sortable: true },
        { text: "Facture Verifiée", value: "facture_verifie", sortable: true },
        { text: "Mois Valeur", value: "mois_valeur", sortable: true },
        { text: "Transfert Compte", value: "transfert_compte", sortable: true },
        { text: "Date d'Ajout", value: "date_ajout", sortable: true }
      ],

      filters: {

        operation: '',
        banque: '',
        compte: '',
        mode_paiement: '',
        date_compta: '',
        date_operation: '',
        date_valeur: '',
        montant_ttc: '',
        tva_deductible: '',
        montant_ht: '',
        versement_tva: '',
        classification: '',
        sous_classification: '',
        details: '',
        facture_verifie: '',
        mois_valeur: '',
        transfert_compte: '',
        date_ajout: ''
      },

      totalTTC: 0,
      totalTVA: 0,
      totalHT: 0
    };
  },

  created() {

    this.getItems();
  },

  methods: {
    
    getItems(){

      let context = this;

      var url = this.$executioEnvironment+"GetValidatedDecaissements";

      axios.get(url).then(function (response) {        
        
        for(var key in response.data) {

          response.data[key].date_compta = response.data[key].date_compta.split("T")[0];
          response.data[key].date_operation = response.data[key].date_operation.split("T")[0];
          response.data[key].date_valeur = response.data[key].date_valeur.split("T")[0];
          response.data[key].date_ajout = response.data[key].date_ajout.split("T")[0];
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

    toggleAll() {

      if (this.selected.length) {

       this.selected = [];
      } 
      else {

        this.selected = this.rows.slice();
      }
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

        customFilter.registerFilter('operation', function (searchedOperation, items) {

          if (searchedOperation.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.operation)
            return item.operation.toLowerCase().includes(searchedOperation.toLowerCase());

          }, searchedOperation);          

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

        customFilter.registerFilter('mode_paiement', function (searchedModePaiement, items) {

          if (searchedModePaiement.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.mode_paiement)
            return item.mode_paiement.toLowerCase().includes(searchedModePaiement.toLowerCase());

          }, searchedModePaiement);

        });

        customFilter.registerFilter('date_compta', function (searchedDateCompta, items) {

          if (searchedDateCompta.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.date_compta)
            return item.date_compta.toLowerCase().includes(searchedDateCompta.toLowerCase());

          }, searchedDateCompta);

        });

        customFilter.registerFilter('date_operation', function (searchedDateOperation, items) {

          if (searchedDateOperation.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.date_operation)
            return item.date_operation.toLowerCase().includes(searchedDateOperation.toLowerCase());

          }, searchedDateOperation);

        });

        customFilter.registerFilter('date_valeur', function (searchedDateValeur, items) {

          if (searchedDateValeur.trim() === ''){
            
            return items;
          }

          return items.filter(item => {
  
            if(null!=item.date_valeur)
            return item.date_valeur.toLowerCase().includes(searchedDateValeur.toLowerCase());

          }, searchedDateValeur);

        });

        customFilter.registerFilter('montant_ttc', function (searchedMontantTTC, items) {

          if (searchedMontantTTC.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.montant_ttc)
            return item.montant_ttc.toString().includes(searchedMontantTTC.toString());

          }, searchedMontantTTC);

        });

        customFilter.registerFilter('tva_deductible', function (searchedTVADeductible, items) {

          if (searchedTVADeductible.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.tva_deductible)
            return item.tva_deductible.toString().includes(searchedTVADeductible.toString());

          }, searchedTVADeductible);

        });

        customFilter.registerFilter('montant_ht', function (searchedMontantHT, items) {

          if (searchedMontantHT.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.montant_ht)
            return item.montant_ht.toString().includes(searchedMontantHT.toString());

          }, searchedMontantHT);

        });

        customFilter.registerFilter('versement_tva', function (searchedVersementTVA, items) {

          if (searchedVersementTVA.trim() === ''){
            
            return items;
          }

          return items.filter(item => {
            
            if(null!=item.versement_tva)
            return item.versement_tva.toLowerCase().includes(searchedVersementTVA.toLowerCase());

          }, searchedVersementTVA);

        });

        customFilter.registerFilter('classification', function (searchedClassification, items) {

          if (searchedClassification.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.classification)
            return item.classification.toLowerCase().includes(searchedClassification.toLowerCase());

          }, searchedClassification);

        });

        customFilter.registerFilter('sous_classification', function (searchedSousClassification, items) {

          if (searchedSousClassification.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.sous_classification)
            return item.sous_classification.toLowerCase().includes(searchedSousClassification.toLowerCase());

          }, searchedSousClassification);

        });

        customFilter.registerFilter('details', function (searchedDetails, items) {

          if (searchedDetails.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.details)
            return item.details.toLowerCase().includes(searchedDetails.toLowerCase());

          }, searchedDetails);

        });

        customFilter.registerFilter('facture_verifie', function (searchedFactureVerifie, items) {

          if (searchedFactureVerifie.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.facture_verifie)
            return item.facture_verifie.toLowerCase().includes(searchedFactureVerifie.toLowerCase());

          }, searchedFactureVerifie);

        });

        customFilter.registerFilter('mois_valeur', function (searchedMoisValeur, items) {

          if (searchedMoisValeur.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.mois_valeur)
            return item.mois_valeur.toString().includes(searchedMoisValeur.toString());

          }, searchedMoisValeur);

        });

        customFilter.registerFilter('transfert_compte', function (searchedTransfertCompte, items) {

          if (searchedTransfertCompte.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.transfert_compte)
            return item.transfert_compte.toLowerCase().includes(searchedTransfertCompte.toLowerCase());

          }, searchedTransfertCompte);

        });

        customFilter.registerFilter('date_ajout', function (searchedDateAjout, items) {

          if (searchedDateAjout.trim() === ''){
            
            return items;
          }

          return items.filter(item => {

            if(null!=item.date_ajout)
            return item.date_ajout.toLowerCase().includes(searchedDateAjout.toLowerCase());

          }, searchedDateAjout);

        });

        var filteredItems = customFilter.runFilters();

        this.calculateTotal(filteredItems);

        return filteredItems;
      },
      
      filterOperation(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {operation: val});
      },

      filterBanque(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {banque: val});
      },

      filterCompte(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {compte: val});
      },

      filterModePaiement(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {mode_paiement: val});
      },

      filterDateCompta(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {date_compta: val});
      },

      filterDateOperation(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {date_operation: val});
      },

      filterDateValeur(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {date_valeur: val});
      },

      filterMontantTTC(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {montant_ttc: val});
      },

      filterTVADeductible(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {tva_deductible: val});
      },

      filterMontantHT(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {montant_ht: val});
      },

      filterVersementTVA(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {versement_tva: val});
      },

      filterClassification(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {classification: val});
      },

      filterSousClassification(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {sous_classification: val});
      },

      filterDetails(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {details: val});
      },

      filterFactureVerifie(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {facture_verifie: val});
      },

      filterMoisValeur(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {mois_valeur: val});
      },

      filterTransfertCompte(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {transfert_compte: val});
      },

      filterDateAjout(val) {

        this.filters = this.$MultiFilters.updateFilters(this.filters, {date_ajout: val});
      },

      calculateTotal(filteredItems){

        this.totalTTC = 0;
        this.totalTVA = 0;
        this.totalHT = 0;

        for(var key in filteredItems){          

          this.totalTTC += filteredItems[key].montant_ttc;
          this.totalTVA += filteredItems[key].tva_deductible;
          this.totalHT += filteredItems[key].montant_ht;
        }
      }
  }
};
</script>