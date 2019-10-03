<template>
  <div>
    <v-app style="text-align:center">
      <div v-if="loading">
        <template>
          <div>
            <v-app>
              <v-layout align-center justify-center row fill-height>
                <v-flex md12>
                  <v-progress-circular :size="100" :width="17" color="amber" indeterminate></v-progress-circular>
                </v-flex>
              </v-layout>
            </v-app>
          </div>
        </template>
      </div>

      <div v-else>
        <v-layout v-resize="onResize" align-baseline="true">
          <v-card class="card--flex-toolbar" style=" width: 100%">
            <v-toolbar card dark color="amber" style="margin-top: -7px" v-if="isMobile">
              <v-btn @click="importData()" block large color="success">
                Importer des Données Excel
                <v-icon color="white" right>input</v-icon>
              </v-btn>
              <template v-slot:extension>
                <v-btn @click="addNew()" block large color="cyan">
                  Ajouter un nouvel Encaissement
                  <v-icon color="white" right>add</v-icon>
                </v-btn>
              </template>
            </v-toolbar>

            <v-toolbar card dark color="amber" v-else>
              <v-btn @click="importData()" block large color="success">
                Importer des Données Excel
                <v-icon color="white" right>input</v-icon>
              </v-btn>
              <v-spacer></v-spacer>
              <v-btn @click="addNew()" block large color="cyan">
                Ajouter un nouvel Encaissement
                <v-icon color="white" right>add</v-icon>
              </v-btn>
            </v-toolbar>

            <v-snackbar v-model="snackbar" :color="snackbarColor" :bottom="snackbarY==='bottom'" :left="snackbarX==='left'" :multi-line="snackbarMode==='multi-line'" :right="snackbarX==='right'" :timeout="3700" :top="snackbarY==='top'" :vertical="snackbarMode==='vertical'">
              {{ snackbarText }}
              <v-btn dark flat @click="snackbar=false">Fermer</v-btn>
            </v-snackbar>

            <v-card>
              <div>
                <v-dialog v-model="dialog" height="100%" :width="isMobile?'100%':'30%'">
                  <v-form ref="form" v-model="valid">
                    <v-card>
                      <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                        <span class="headline">{{ formTitle }}</span>
                      </v-card-title>
                      <v-card-text>
                        <v-container grid-list-md>
                          <v-flex>
                            <v-autocomplete v-model="editedItem.societe" :items="allSociety" label="Société" :rules="societyRules" required></v-autocomplete>
                          </v-flex>
                          <v-flex>
                            <v-autocomplete v-model="editedItem.banque" :items="allBank" v-on:change="onChangeBank" label="Banque" :rules="bankRules" required></v-autocomplete>
                          </v-flex>
                          <v-flex>
                            <v-autocomplete v-model="editedItem.compte" :items="allBankAccount" label="Compte" :rules="accountRules" required></v-autocomplete>
                          </v-flex>
                          <v-flex>
                            <v-autocomplete v-on:change="calculateHtAndTVA" v-model="editedItem.tvaAmount" :items="allTvaAmount" label="Taux de TVA" item-text="name" return-object :rules="tvaAmountRules" required></v-autocomplete>
                          </v-flex>
                          <v-flex>
                            <v-text-field v-on:change="calculateHtAndTVA" v-model="editedItem.total_ttc" label="Total TTC (€)" :rules="totalTTCRules" required></v-text-field>
                          </v-flex>
                          <v-flex>
                            <v-text-field v-model="editedItem.total_ht" label="Total HT (€)" disabled required></v-text-field>
                          </v-flex>
                          <v-flex>
                            <v-text-field v-model="editedItem.total_tva" label="Total TVA (€)" :rules="totalTVARules" disabled required></v-text-field>
                          </v-flex>
                          <v-flex>
                            <v-menu v-model="menuDateReglement" :close-on-content-click="false" :nudge-right="40" lazy transition="scale-transition" offset-y full-width min-width="290px">
                              <template v-slot:activator="{ on }">
                                <v-text-field v-model="editedItem.date_reglement" label="Date Règlement" prepend-icon="event" v-on="on" :rules="reglementDateRules" required></v-text-field>
                              </template>
                              <v-date-picker v-model="editedItem.date_reglement" @input="menuDateReglement = false"></v-date-picker>
                            </v-menu>
                          </v-flex>
                          <v-flex>
                            <v-autocomplete v-model="editedItem.mode_paiement" :items="allPaymentMode" label="Mode de Paiement" :rules="paymentMethodRules" required></v-autocomplete>
                          </v-flex>
                          <v-flex>
                            <v-checkbox v-model="editedItem.transfert_compte" label="Transfert Compte"></v-checkbox>
                          </v-flex>
                        </v-container>
                      </v-card-text>
                      <v-card-actions v-if="!isMobile">
                        <v-btn color="warning" block @click.native="close" large>Annuler</v-btn>
                        <v-btn color="success" block @click.native="save" :disabled="!valid" large>Sauvegarder</v-btn>
                      </v-card-actions>
                      <v-card-text v-else>
                        <v-container grid-list-md>
                          <v-layout wrap>
                            <v-flex>
                              <v-btn color="success" block @click.native="save" :disabled="!valid" large>Sauvegarder</v-btn>
                            </v-flex>
                            <v-flex>
                              <v-btn color="warning" block @click.native="close" large>Annuler</v-btn>
                            </v-flex>
                          </v-layout>
                        </v-container>
                      </v-card-text>
                    </v-card>
                  </v-form>
                </v-dialog>
              </div>
            </v-card>

            <v-card>
              <div>
                <v-dialog v-model="importExcel" height="100%">
                  <v-card>
                    <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                      <span class="headline">Import de Données Excel</span>
                    </v-card-title>
                    <v-card-text>
                      <v-container>
                        <v-textarea v-model="excelData" box auto-grow background-color="light-blue lighten-5" label="Coller vos lignes Excel ici" :rules="excelDataRules" required></v-textarea>
                        <v-alert :value="alertExcelData" type="error" transition="scale-transition">Vous devez rentrer au moins une ligne</v-alert>
                      </v-container>
                    </v-card-text>
                    <v-card-actions v-if="!isMobile">
                      <v-btn color="warning" block @click.native="closeImportExcel" large>Annuler</v-btn>
                      <v-btn color="success" block @click.native="validateExcel" large>Sauvegarder</v-btn>
                    </v-card-actions>
                    <v-card-text v-else>
                      <v-container grid-list-md>
                        <v-layout wrap>
                          <v-flex>
                            <v-btn color="success" block @click.native="validateExcel" large>Sauvegarder</v-btn>
                          </v-flex>
                          <v-flex>
                            <v-btn color="warning" block @click.native="closeImportExcel" large>Annuler</v-btn>
                          </v-flex>
                        </v-layout>
                      </v-container>
                    </v-card-text>
                  </v-card>
                </v-dialog>
              </div>
            </v-card>

            <v-card>
              
              <v-layout v-resize="onResize" column>

                <v-menu v-if="isMobile" block offset-y :nudge-left="170" :close-on-content-click="false">
      
                  <v-btn block slot="activator" color="amber" dark>
                    Trier <v-icon>swap_vert</v-icon>
                  </v-btn>
        
                  <v-list>
                    <v-list-tile v-for="item in headers" :key="item.value" @click="changeSort(item.value)">
                      <v-list-tile-title>
                        {{ item.text }}
                      <v-icon v-if="paginationMobile.sortBy === item.value">{{paginationMobile.descending ? 'arrow_downward':'arrow_upward'}}</v-icon>
                      </v-list-tile-title>
                    </v-list-tile>
                  </v-list>
            
                </v-menu>

              </v-layout>

                <v-text-field v-model="search" prepend-icon="search" label="Rechercher un mot clé dans tous le tableau" single-line hide-details></v-text-field>

                <v-data-table :headers="headers" :items="items" :search="search" :hide-headers="isMobile" :pagination.sync="pagination" :class="{mobile: isMobile}" v-if="!isMobile">
                  <template slot="items" slot-scope="props">

                    <tr>
                      <td class="justify-center layout px-0">
                        <v-btn icon small @click="editItem(props.item)" color="amber">
                          <v-icon color="white">edit</v-icon>
                        </v-btn>
                        <v-btn icon small @click="openDeleteDialog(props.item)" color="red">
                          <v-icon color="white">delete</v-icon>
                        </v-btn>
                      </td>
                      <td>{{ props.item.societe }}</td>
                      <td>{{ props.item.banque }}</td>
                      <td>{{ props.item.compte }}</td>
                      <td>{{ formatNumber(props.item.total_ht) }}</td>
                      <td>{{ formatNumber(props.item.total_tva) }}</td>
                      <td>{{ formatNumber(props.item.total_ttc) }}</td>
                      <td>{{ props.item.date_reglement }}</td>
                      <td>{{ props.item.mode_paiement }}</td>

                      <td v-if="props.item.transfert_compte">
                        <v-icon color="success">done</v-icon>
                      </td>
                      <td v-else>
                        <v-icon color="red">clear</v-icon>
                      </td>
                    </tr>

                  </template>
                </v-data-table>

                <div v-if="isMobile">
                  <v-data-table :headers="headers" :items="items" :search="search" :hide-headers="isMobile" :pagination.sync="paginationMobile" :class="{mobile: isMobile}">
                    <template slot="items" slot-scope="props">

                      <tr>
                        <td>
                          <ul class="flex-content">
                            <li class="flex-item" data-label="Modifier">
                              <v-btn block @click="editItem(props.item)" color="amber">
                                <v-icon color="white">edit</v-icon>
                              </v-btn>
                            </li>
                            <li class="flex-item" data-label="Supprimer">
                              <v-btn block @click="openDeleteDialog(props.item)" color="red">
                                <v-icon color="white">delete</v-icon>
                              </v-btn>
                            </li>

                            <li class="flex-item" data-label="Société" style="margin-top:25px">{{ props.item.societe }}</li>
                            <li class="flex-item" data-label="Banque" style="margin-top:25px">{{ props.item.banque }}</li>

                            <li class="flex-item" data-label="Compte">{{ props.item.compte }}</li>
                            <li class="flex-item" data-label="Total HT (€)">{{ formatNumber(props.item.total_ht) }}</li>
                            <li class="flex-item" data-label="Total TVA (€)">{{ formatNumber(props.item.total_tva) }}</li>
                            <li class="flex-item" data-label="Total TTC (€)">{{ formatNumber(props.item.total_ttc) }}</li>
                            <li class="flex-item" data-label="Date Règlement">{{ props.item.date_reglement }}</li>
                            <li class="flex-item" data-label="Mode de Paiement">{{ props.item.mode_paiement }}</li>

                            <li class="flex-item" data-label="Transfert Compte" v-if="props.item.transfert_compte">
                              <v-icon color="success">done</v-icon>
                            </li>
                            <li class="flex-item" data-label="Transfert Compte" v-else>
                              <v-icon color="red">clear</v-icon>
                            </li>
                          </ul>
                        </td>
                      </tr>

                    </template>
                  </v-data-table>
                </div>
            </v-card>
          </v-card>
        </v-layout>

        <v-layout row justify-center>
          <v-dialog v-model="dialogDelete" persistent max-width="500">
            <v-card>
              <v-toolbar card color="amber">
                <strong>Attention !</strong>
              </v-toolbar>
              <v-card-text>Etes vous sure de vouloir supprimer cette ligne ?</v-card-text>
              <v-card-actions>
                <v-btn block color="info" @click="dialogDelete=false">Annuler</v-btn>
                <v-spacer></v-spacer>
                <v-btn block color="error" @click="deleteItem">Oui</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-layout>
      </div>
    </v-app>
  </div>
</template>

<script>
const axios = require("axios");

export default {
  name: "writeEncaissement",

  data() {
    return {

      pagination: {

        sortBy: "societe",
        descending: false,
        rowsPerPage: -1
      },

      paginationMobile: {

        sortBy: "societe",
        descending: false,
        rowsPerPage: 5
      },

      dialog: false,
      importExcel: false,

      search: "",

      currentDay: 0,
      currentMonth: 0,
      currentYear: 0,

      dialogDelete: false,
      pendingDeleteItemIndex: -1,

      excelData: [],
      alertExcelData: false,

      editedIndex: -1,

      valid: false,

      societyRules: [v => !!v || "Vous devez rentrer une société"],

      bankRules: [v => !!v || "Vous devez sélectionner une banque"],

      accountRules: [v => !!v || "Vous devez sélectionner un compte"],

      tvaAmountRules: [v => !!v || "Vous devez sélectionner un taux de TVA"],

      totalTTCRules: [
        v => !!v || "Vous devez rentrer un total TTC",
        v =>
          (v && !isNaN(parseFloat(v)) && isFinite(v)) ||
          "Le total TTC doit être un chiffre",
        v =>
          (v && !this.hasMoreThanTwoDecimal(v)) ||
          "Le total TTC ne peux pas avoir plus de deux décimales",
        v =>
          (v && !this.isAboveMaxLenght(v)) ||
          "Le total TTC doit être plus petit que 1e22"
          
      ],

      totalTVARules: [
        v => !!v || "Rentrez un Total TTC et un Taux de TVA calculera automatiquement le total HT et TVA"
      ],

      reglementDateRules: [
        v => !!v || "Vous devez sélectionner un date de règlement",
        v =>
          (v &&
            !this.isDateAfterToday(
              new Date(
                parseInt(v.split("-")[0]),
                parseInt(v.split("-")[1] - 1),
                parseInt(v.split("-")[2])
              )
            )) ||
          "La date ne peux pas être dans le futur"
      ],

      paymentMethodRules: [
        v => !!v || "Vous devez sélectionner un mode de paiement"
      ],

      snackbar: false,
      snackbarText: "",
      snackbarColor: "",
      snackbarY: "top",
      snackbarX: null,
      snackbarMode: "",

      isMobile: false,
      loading: true,

      menuDateReglement: false,

      headers: [
        { text: "Action", value: "action", sortable: false },
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

      items: [],

      allSociety: [],

      allAccountType: [],

      allBank: [],
      allBankAccount: [],

      allTvaAmount: [
        {
          "name":"Remboursement Crédit TVA Etat : 100%",
          "percentage":100
        },
        {
          "name":"Taux Normal : 20%",
          "percentage":20
        },
        {
          "name":"Taux Réduit : 10%",
          "percentage":10
        },
        {
          "name":"Taux Réduit : 5,5%",
          "percentage":5.5
        },
        {
          "name":"Taux Particulier : 2,1%",
          "percentage":2.1
        }
      ],

      allPaymentMode: [],

      editedItem: {
        id: "",
        societe: "",
        banque: "",
        compte: "",
        tvaAmount: "",
        total_ht: "",
        total_ttc: "",
        total_tva: "",
        date_reglement: "",
        mode_paiement: "",
        transfert_compte: ""
      },

      defaultItem: {
        id: "",
        societe: "",
        banque: "",
        compte: "",
        tvaAmount: "",
        total_ht: "",
        total_ttc: "",
        total_tva: "",
        date_reglement: "",
        mode_paiement: "",
        transfert_compte: ""
      },

      excelDataRules: []
    };
  },

  created() {
    
    this.getItems();

    this.getAllSociete();

    this.getBanks();

    this.getPaiements();

    this.getCurrentDate();
  },

  computed: {
    formTitle() {
      return this.editedIndex === -1
        ? "Ajouter un Encaissement"
        : "Modifier un Encaissement";
    }
  },

  watch: {
    dialog(val) {
      val || this.close();

      this.$refs.form.resetValidation();
    },

    importExcel(val) {
      val || this.closeImportExcel();
    }
  },

  methods: {

    hasMoreThanTwoDecimal(val){

      val += "";

      var splitedTab = val.split(".");

      if(splitedTab.length > 1 && splitedTab[1].length > 2){

        return true;
      }

      return false;
    },

    isAboveMaxLenght(val){

      val += "";

      if(val.length<22){

        return false;
      }
      
      return true;
    },

    calculateHtAndTVA(){

      if(this.editedItem.total_ttc <= 99999999999999999999999){

        var tvaPercentage = this.editedItem.tvaAmount.percentage;

        if( this.editedItem.tvaAmount && 
          this.editedItem.tvaAmount != null &&
          this.editedItem.total_ttc && 
          this.editedItem.total_ttc != null &&
          this.editedItem.total_ttc > 0
        ){

          if(tvaPercentage==100){

            this.editedItem.total_tva = this.editedItem.total_ttc;

            this.editedItem.total_ht = 0;
          }
          else{

            this.editedItem.total_tva = this.formatNumber(Math.round( (this.editedItem.total_ttc * (tvaPercentage/100)) * 1000) / 1000).split(",").join("");

            this.editedItem.total_ht = this.formatNumber(Math.round( (this.editedItem.total_ttc - this.editedItem.total_tva ) * 1000) / 1000).split(",").join("");
          }
        }
      }

      this.$refs.form.validate();
    },
    
    setTotalHt(val) {
      if (
        this.editedItem.total_ttc &&
        this.editedItem.total_ttc != null &&
        this.editedItem.total_ttc > 0 &&
        this.editedItem.total_ttc - val > 0
      ) {
        this.editedItem.total_ht = this.editedItem.total_ttc - val;

        this.$refs.form.validate();
      }
    },

    setTotalTva(val) {
      if (
        this.editedItem.total_ttc &&
        this.editedItem.total_ttc != null &&
        this.editedItem.total_ttc > 0 &&
        this.editedItem.total_ttc - val > 0
      ) {
        this.editedItem.total_tva = this.editedItem.total_ttc - val;

        this.$refs.form.validate();
      }
    },

    getItems() {

      let context = this;

      var url = this.$executioEnvironment + "GetAllEncaissement";

      axios.get(url).then(function(response) {

          for (var key in response.data) {

            response.data[key].date_reglement = response.data[key].date_reglement.split("T")[0];

            response.data[key].tvaAmount = context.guessTvaAmount(response.data[key]);
          }

          context.items = response.data;
          context.loading = false;
        })
        .catch(function(error) {

          console.log(error);
        });
    },

    guessTvaAmount(item){

      for(var key in this.allTvaAmount){

        var calculatedTva = this.formatNumber(Math.round( (item.total_ttc * (this.allTvaAmount[key].percentage/100)) * 1000) / 1000).split(",").join("");

        if(item.total_tva == calculatedTva){

          return this.allTvaAmount[key];
        }
      }
    },

    getAllSociete() {
      let context = this;
      var urlAllSociety = this.$executioEnvironment + "GetAllSociete";

      axios
        .get(urlAllSociety)
        .then(function(response) {
          for (var key in response.data) {
            context.allSociety.push(response.data[key].name);
          }
        })
        .catch(function(error) {
          console.log(error);
        });
    },

    getBanks() {
      let context = this;
      var urlAccountType = this.$executioEnvironment + "getAccountType";

      axios
        .get(urlAccountType)
        .then(function(response) {
          context.allAccountType = response.data;

          for (var element in response.data) {
            context.allBank.push(element);
          }
        })
        .catch(function(error) {
          console.log(error);
        });
    },

    getPaiements() {
      let context = this;
      var urlAllPaymentMode = this.$executioEnvironment + "getPaymentMethod";

      axios
        .get(urlAllPaymentMode)
        .then(function(response) {
          for (var element in response.data) {
            context.allPaymentMode.push(response.data[element].name);
          }
        })
        .catch(function(error) {
          console.log(error);
        });
    },

    getCurrentDate() {
      var today = new Date();

      this.currentDay = parseInt(String(today.getDate()).padStart(2, "0"));
      this.currentMonth = parseInt(
        String(today.getMonth() + 1).padStart(2, "0")
      );
      this.currentYear = parseInt(today.getFullYear());
    },

    onResize() {
      
      if (window.innerWidth < 769) {

        this.isMobile = true;
      } 
      else {

        this.isMobile = false;
      }
    },

    changeSort(column) {

      console.log(column);

       if (this.paginationMobile.sortBy === column) {

         this.paginationMobile.descending = !this.paginationMobile.descending;
       } 
       else {
         
          this.paginationMobile.sortBy = column;
          this.paginationMobile.descending = false;
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

    onChangeBank: function() {
      var selectedBank = this.editedItem.banque;

      this.allBankAccount = this.allAccountType[selectedBank];
    },

    addNew() {
      this.dialog = true;
    },

    importData() {
      this.importExcel = true;

      this.validExcelData = false;

      this.excelData = [];
    },

    isDateAfterToday(date) {
      if (
        new Date(date.toDateString()) == new Date(new Date().toDateString())
      ) {
        return false;
      } else {
        return (
          new Date(date.toDateString()) > new Date(new Date().toDateString())
        );
      }
    },

    validateExcel: function() {
      if (this.excelData != "") {
        this.alertExcelData = false;

        this.saveImportExcel();
      } else {
        this.alertExcelData = true;
      }
    },

    saveEdit(item) {
      var url = this.$executioEnvironment + "EditEncaissement";

      var itemTransfertCompte = item.transfert_compte;

      if (itemTransfertCompte) {
        item.transfert_compte = 1;
      } else {
        item.transfert_compte = 0;
      }

      var stringifiedItem = JSON.stringify(item);

      axios
        .post(url, stringifiedItem, {
          headers: {
            Accept: "application/json",
            "Content-type": "application/json"
          }
        })
        .then(response => {
          console.log(response);

          (this.snackbar = true),
            (this.snackbarText = "Ligne bien modifié dans la base de données"),
            (this.snackbarColor = "success");

          this.getItems();
        })
        .catch(error => {
          console.log(error);

          (this.snackbar = true),
            (this.snackbarText =
              "Erreur lors de la Modification de la ligne dans la Base de Données"),
            (this.snackbarColor = "error");
        });

      this.getItems();
    },

    saveLine(item) {
      var url = this.$executioEnvironment + "SaveEncaissement";

      var itemTransfertCompte = item.transfert_compte;

      if (itemTransfertCompte) {
        item.transfert_compte = 1;
      } else {
        item.transfert_compte = 0;
      }

      var stringifiedItem = JSON.stringify(item);

      axios
        .post(url, stringifiedItem, {
          headers: {
            Accept: "application/json",
            "Content-type": "application/json"
          }
        })
        .then(response => {
          console.log(response);

          item.id = response.data[0].retour;

          (this.snackbar = true),
            (this.snackbarText = "Ligne Sauvegardée dans la Base de Données"),
            (this.snackbarColor = "success");
          this.getItems();
        })
        .catch(error => {
          console.log(error.response);

          console.log(error.response.data);

          (this.snackbar = true),
            (this.snackbarText =
              "Erreur lors de la Sauvegarde dans la Base de Données"),
            (this.snackbarColor = "error");
        });
    },

    editItem(item) {
      this.editedIndex = this.items.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;

      this.onChangeBank();
    },

    openDeleteDialog(item) {
      this.pendingDeleteItemIndex = this.items.indexOf(item);

      this.dialogDelete = true;
    },

    deleteItem() {
      var url = this.$executioEnvironment + "DeleteEncaissement";

      let id = this.items[this.pendingDeleteItemIndex].id;

      axios
        .post(url, JSON.parse('{"id":"' + id + '"}'), {
          headers: {
            Accept: "application/json",
            "Content-type": "application/json"
          }
        })
        .then(response => {
          console.log(response);

          (this.snackbar = true),
            (this.snackbarText = "Ligne supprimée de la base de données"),
            (this.snackbarColor = "success");
          this.getItems();
        })
        .catch(error => {
          console.log("erreur : " + error);

          (this.snackbar = true),
            (this.snackbarText =
              "Erreur lors de la suppression dans la Base de Données"),
            (this.snackbarColor = "error");
        });
      this.getItems();
      this.pendingDeleteItemIndex = -1;

      this.dialogDelete = false;
    },

    close() {
      this.dialog = false;

      setTimeout(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      }, 300);

      this.$refs.form.resetValidation();
    },

    save() {
      if (this.editedIndex > -1) {
        this.saveEdit(this.editedItem);
      } else {
        this.saveLine(this.editedItem);
      }

      this.close();
    },

    closeImportExcel() {
      this.importExcel = false;

      this.alertExcelData = false;
    },

    saveImportExcel() {
      var rows = this.excelData.split("\n");

      console.log(rows);

      for (var y in rows) {
        var row = rows[y];

        if (row != "") {
          var cells = row.split("\t");

          var newItem = {
            id: "",
            societe: "",
            banque: "",
            compte: "",
            total_ht: "",
            total_ttc: "",
            total_tva: "",
            date_reglement: "",
            mode_paiement: "",
            transfert_compte: ""
          };

          if (cells.length == 10) {
            //si le compte est précisé
            newItem.societe = cells[0];
            newItem.compte = cells[1];
            newItem.banque = cells[2];
            newItem.total_ht = cells[3];
            newItem.total_tva = cells[4];
            newItem.total_ttc = cells[5];
            newItem.date_reglement = cells[6];
            newItem.mode_paiement = cells[7];
            newItem.transfert_compte = cells[9];
          } else {
            newItem.societe = cells[0];
            newItem.banque = cells[1];
            newItem.total_ht = cells[2];
            newItem.total_tva = cells[3];
            newItem.total_ttc = cells[4];
            newItem.date_reglement = cells[5];
            newItem.mode_paiement = cells[6];
            newItem.transfert_compte = cells[8];
          }

          this.saveLine(newItem);
        }
      }

      this.closeImportExcel();
    }
  }
};
</script>
