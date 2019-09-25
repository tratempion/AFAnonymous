<template>
  <div>
    <v-app style="text-align:center">

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
      
        <v-layout v-resize="onResize" align-baseline="true">
          <v-card style="width: 100%; margin-top: -7px;">
            
            <v-snackbar v-model="snackbar" :color="snackbarColor" :bottom="snackbarY==='bottom'" :left="snackbarX==='left'" :multi-line="snackbarMode==='multi-line'" :right="snackbarX==='right'" :timeout="3700" :top="snackbarY==='top'" :vertical="snackbarMode==='vertical'">
              {{ snackbarText }}
              <v-btn dark flat @click="snackbar=false">Fermer</v-btn>
            </v-snackbar>

            <v-card>
              <div>
                <v-dialog v-model="dialog" height="100%">

                  <v-form ref="form" v-model="valid">

                    <v-card>
                      <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                        <span class="headline">Effectuer le rapprochement</span>
                      </v-card-title>
                      <v-card-text>
                        <v-container grid-list-md>
                          <v-layout :key="reRenderKey" wrap>
                            <v-flex>
                              <v-autocomplete v-model="selectedItem.classification" :items="allClassification" v-on:change="onChangeClassification" label="Classification" :rules="classificationRules" required></v-autocomplete>
                            </v-flex>
                            <v-flex>
                              <v-autocomplete v-model="selectedItem.sous_classification" :items="allUnderClassification" label="Sous Classification" :rules="sousClassificationRules" required></v-autocomplete>
                            </v-flex>
                            <v-flex>
                              <v-text-field v-on:change="setTvaDeductible" v-model="selectedItem.tva_deductible" :rules="tvaDeductibleRules" label="TVA deductible" required></v-text-field>
                            </v-flex>
                            <v-flex>
                              <v-text-field v-on:change="setMontantHt" v-model="selectedItem.montant_ht" :rules="montantHTRules" label="Montant HT" required></v-text-field>
                            </v-flex>
                            <v-flex>
                              <v-checkbox v-model="selectedItem.versement_tva" label="Versement TVA"></v-checkbox>
                            </v-flex>
                          </v-layout>
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
                <v-dialog v-model="dialogEdit" height="100%">

                  <v-form ref="formEdit" v-model="validEdit">

                    <v-card>
                      <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                        <span class="headline">Modifier un Décaissement</span>
                      </v-card-title>
                      <v-card-text>
                        <v-container grid-list-md>
                          <v-layout wrap>
                            <v-flex> 
                              <v-text-field v-model="editedItem.operation" label="Opération" :rules="operationRules" required></v-text-field>
                            </v-flex>
                            <v-flex>
                              <v-autocomplete v-model="editedItem.banque" :items="allBank" v-on:change="onChangeBank" label="Banque" :rules="bankRules" required></v-autocomplete>
                            </v-flex>
                            <v-flex>
                              <v-autocomplete v-model="editedItem.compte" :items="allBankAccount" label="Compte" :rules="accountRules" required></v-autocomplete>
                            </v-flex>
                            <v-flex>
                              <v-autocomplete v-model="editedItem.mode_paiement" :items="allPaymentMode" label="Mode de Paiement" :rules="paymentMethodRules" required></v-autocomplete>
                            </v-flex>
                            <v-flex>
                              <v-menu v-model="menuDateComptabilite" :close-on-content-click="false" :nudge-right="40" lazy transition="scale-transition" offset-y full-width min-width="290px">
                                <template v-slot:activator="{ on }">
                                  <v-text-field v-model="editedItem.date_compta" label="Date Comptabilité" prepend-icon="event" v-on="on" :rules="comptaDateRules" required></v-text-field>
                                </template>
                                <v-date-picker v-model="editedItem.date_compta" @input="menuDateComptabilite = false"></v-date-picker>
                              </v-menu>
                            </v-flex>
                            <v-flex>
                              <v-menu v-model="menuDateOperation" :close-on-content-click="false" :nudge-right="40" lazy transition="scale-transition" offset-y full-width min-width="290px">
                                <template v-slot:activator="{ on }">
                                  <v-text-field v-model="editedItem.date_operation" label="Date Opération" prepend-icon="event" v-on="on" :rules="operationDateRules" required></v-text-field>
                                </template>
                                <v-date-picker v-model="editedItem.date_operation" @input="menuDateOperation = false"></v-date-picker>
                              </v-menu>
                            </v-flex>
                            <v-flex>
                              <v-menu v-model="menuDateValeur" :close-on-content-click="false" :nudge-right="40" lazy transition="scale-transition" offset-y full-width min-width="290px">
                                <template v-slot:activator="{ on }">
                                  <v-text-field v-model="editedItem.date_valeur" label="Date Valeur" prepend-icon="event" v-on="on" :rules="valueDateRules" required></v-text-field>
                                </template>
                                <v-date-picker v-model="editedItem.date_valeur" @input="menuDateValeur = false"></v-date-picker>
                              </v-menu>
                            </v-flex>
                            <v-flex>
                              <v-text-field v-model="editedItem.montant_ttc" label="Montant TTC (€)" :rules="amountRules" required></v-text-field>
                            </v-flex>
                          </v-layout>
                        </v-container>
                      </v-card-text>
                      <v-card-actions v-if="!isMobile">
                        <v-btn color="warning" block @click.native="closeEdit" large>Annuler</v-btn>
                        <v-btn color="success" block @click.native="saveEdit" :disabled="!validEdit" large>Sauvegarder</v-btn>
                      </v-card-actions>
                      <v-card-text v-else>
                        <v-container grid-list-md>
                          <v-layout wrap>
                            <v-flex>
                              <v-btn color="success" block @click.native="saveEdit" :disabled="!validEdit" large>Sauvegarder</v-btn>
                            </v-flex>
                            <v-flex>
                              <v-btn color="warning" block @click.native="closeEdit" large>Annuler</v-btn>
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
              <v-data-table :headers="headers" :items="items" :hide-headers="isMobile" :pagination.sync="pagination" :class="{mobile: isMobile}">
                <template slot="items" slot-scope="props">
                  <tr v-if="!isMobile">
                    <td class="justify-center layout px-0">
                      <v-btn icon small @click="editItem(props.item)" color="amber">
                        <v-icon color="white">edit</v-icon>
                      </v-btn>
                      <v-btn icon small @click="rapprocherItem(props.item)" color="purple">
                        <v-icon color="white">compare_arrows</v-icon>
                      </v-btn>
                    </td>
                    <td>{{ props.item.operation }}</td>
                    <td>{{ props.item.banque }}</td>
                    <td>{{ props.item.compte }}</td>
                    <td>{{ props.item.mode_paiement }}</td>
                    <td>{{ props.item.date_compta }}</td>
                    <td>{{ props.item.date_operation }}</td>
                    <td>{{ props.item.date_valeur }}</td>
                    <td>{{ formatNumber(props.item.montant_ttc) }}</td>
                  </tr>
                  <tr v-else>
                    <td>
                      <ul class="flex-content">
                        <li class="flex-item" data-label="Modifier">
                          <v-btn block @click="editItem(props.item)" color="amber">
                            <v-icon color="white">edit</v-icon>
                          </v-btn>
                        </li>
                        <li class="flex-item" data-label="Rapprocher">
                          <v-btn block @click="rapprocherItem(props.item)" color="purple">
                            <v-icon color="white">compare_arrows</v-icon>
                          </v-btn>
                        </li>

                        <li class="flex-item" data-label="Opération" style="margin-top:25px">{{ props.item.operation }}</li>
                        <li class="flex-item" data-label="Banque" style="margin-top:25px">{{ props.item.banque }}</li>
                        
                        <li class="flex-item" data-label="Compte">{{ props.item.compte }}</li>
                        <li class="flex-item" data-label="Mode de Paiement">{{ props.item.mode_paiement }}</li>
                        <li class="flex-item" data-label="Date Comptabilité">{{ props.item.date_compta }}</li>
                        <li class="flex-item" data-label="Date Opération">{{ props.item.date_operation }}</li>
                        <li class="flex-item" data-label="Date Valeur">{{ props.item.date_valeur }}</li>
                        <li class="flex-item" data-label="Montant TTC (€)">{{ formatNumber(props.item.montant_ttc) }}</li>
                      </ul>
                    </td>
                  </tr>
                </template>
              </v-data-table>
            </v-card>
          </v-card>
        </v-layout>
        
      </div>
    
    </v-app>
  </div>
</template>

<script>

const axios = require("axios");

export default {

  name: "rapprochement",

  data() {

    return {

      pagination: {

        rowsPerPage: -1
      },

      dialog: false,

      dialogDelete: false,
      pendingDeleteItemIndex: -1,

      currentDay: 0,
      currentMonth: 0,
      currentYear: 0,

      reRenderKey: 0,

      valid: false,
      
      classificationRules: [        
        v => !!v || 'Vous devez selectioner une classification'
      ],

      sousClassificationRules: [
        v => !!v || 'Vous devez selectioner une sous classification'
      ],

      moisValeurRules: [
        v => !!v || 'Vous devez rentrer un mois de valeur',
        v => (v && !isNaN(parseFloat(v)) && isFinite(v)) || 'Le Mois doit être un chiffre',
        v => (v >= 1 && v <= 12) || 'Le Mois doit être un compris entre 1 et 12'
      ],

      montantHTRules: [
        v => !!v || 'Vous devez rentrer un montant HT',
        v => (v && !isNaN(parseFloat(v)) && isFinite(v)) || 'Le montant HT doit être un chiffre',
        v => (v && (parseInt(this.selectedItem.montant_ht) <= parseInt(this.selectedItem.montant_ttc))) || 'Le montant HT ne peut pas être suppérieur au montant TTC'
      ],

      tvaDeductibleRules: [
        v => !!v || 'Vous devez rentrer un montant de TVA deductible',
        v => (v && !isNaN(parseFloat(v)) && isFinite(v)) || 'Le montant de TVA deductible doit être un chiffre',
        v => (v && (parseInt(this.selectedItem.tva_deductible) <= parseInt(this.selectedItem.montant_ttc))) || 'Le montant HT ne peut pas être suppérieur au montant TTC'
      ],

      snackbar: false,
      snackbarText: "",
      snackbarColor: "",
      snackbarY: 'top',
      snackbarX: null,
      snackbarMode: '',

      isMobile: false,
      loading: true,

      headers: [
        { text: "Action", value: "action", sortable: false },
        { text: "Opération", value: "operation", sortable: true },
        { text: "Banque", value: "banque", sortable: true },
        { text: "Compte", value: "compte", sortable: true },
        { text: "Mode de Paiement", value: "mode_paiement", sortable: true },
        { text: "Date Comptabilité", value: "date_compta", sortable: true },
        { text: "Date Opération", value: "date_operation", sortable: true },
        { text: "Date Valeur", value: "date_valeur", sortable: true },
        { text: "Montant TTC (€)", value: "montant_ttc", sortable: true }
      ],

      items: [],      

      selectedItem: {
        id:"",
        operation: "",
        banque: "",
        compte: "",
        mode_paiement: "",
        date_compta: "",
        date_operation: "",
        date_valeur: "",
        montant_ttc: "",
        classification: "",
        sous_classification: "",
        mois_valeur: "",
        montant_ht:"",
        tva_deductible:"",
        versement_tva: false
      },

      allClassification : [],
      allClassificationType: [],

      allUnderClassification: [],
      
      dialogEdit: false,
      validEdit: false,      
      
      operationRules: [        
        v => !!v || 'Vous devez rentrer un numero d\'opération'
      ],

      bankRules: [
        v => !!v || 'Vous devez selectioner une banque'
      ],

      accountRules: [
        v => !!v || 'Vous devez selectioner un compte'
      ],

      paymentMethodRules: [
        v => !!v || 'Vous devez selectioner un mode de paiement'
      ],

      comptaDateRules: [
        v => !!v || 'Vous devez selectioner un date de comptabilité',
        v => (v && !(this.isDateAfterToday(new Date(parseInt(v.split("-")[0]), parseInt(v.split("-")[1]-1) ,parseInt(v.split("-")[2]))))) || 'La date ne peux pas être dans le futur'
      ],

      operationDateRules: [
        v => !!v || 'Vous devez selectioner une date d\'opération',
        v => (v && !(this.isDateAfterToday(new Date(parseInt(v.split("-")[0]), parseInt(v.split("-")[1]-1) ,parseInt(v.split("-")[2]))))) || 'La date ne peux pas être dans le futur'
      ],

      valueDateRules: [
        v => !!v || 'Vous devez selectioner une date de valeur',
        v => (v && !(this.isDateAfterToday(new Date(parseInt(v.split("-")[0]), parseInt(v.split("-")[1]-1) ,parseInt(v.split("-")[2]))))) || 'La date ne peux pas être dans le futur'
      ],

      amountRules: [
        v => !!v || 'Vous devez rentrer un montant',
        v => (v && !isNaN(parseFloat(v)) && isFinite(v)) || 'Le montant doit être un chiffre'
      ],
      
      menuDateComptabilite: false,
      menuDateOperation: false,
      menuDateValeur: false,

      allBank: [],
      allBankAccount: [],

      allPaymentMode: [],

      editedItem: {

        id:"",
        operation: "",
        banque: "",
        compte: "",
        mode_paiement: "",
        date_compta: "",
        date_operation: "",
        date_valeur: "",
        montant_ttc: "",
        classification: "",
        sous_classification: "",
        mois_valeur: "",
        montant_ht:"",
        tva_deductible:"",        
        versement_tva: false
      },

      defaultItem: {

        id:"",
        operation: "",
        banque: "",
        compte: "",
        mode_paiement: "",
        date_compta: "",
        date_operation: "",
        date_valeur: "",
        montant_ttc: "",
        classification: "",
        sous_classification: "",
        mois_valeur: "",
        montant_ht:"",
        tva_deductible:"",
        versement_tva: false
      }
    };
  },

  created() {

    this.getItems();

    this.getClassification();

    this.getBanks();

    this.getPaiements();

    this.getCurrentDate();    
  },

  watch: {

    dialog(val) {

      if(val){

        this.selectedItem.montant_ht = this.selectedItem.montant_ttc;
        this.selectedItem.tva_deductible = "0";

        this.reRenderKey += 1;
      }

      val || this.close();
      
      this.$refs.form.resetValidation();
    },

    dialogEdit(val) {

      val || this.close();
      
      this.$refs.formEdit.resetValidation();
    }
  },

  methods: {

    isDateAfterToday(date) {

      if(new Date(date.toDateString()) == new Date(new Date().toDateString())){

        return false;
      }
      else{

        return new Date(date.toDateString()) > new Date(new Date().toDateString());
      }
    },

    setTvaDeductible(val){

      if(this.selectedItem.montant_ttc-val>=0) {

        this.selectedItem.montant_ht = this.selectedItem.montant_ttc-val;

        this.reRenderKey += 1;
      }
    },

    setMontantHt(val){

      if(this.selectedItem.montant_ttc-val>=0) {
        
        this.selectedItem.tva_deductible = this.selectedItem.montant_ttc-val;
                        
        this.reRenderKey += 1;
      }   
    },

    getItems(){

      let context = this;

      var url = this.$executioEnvironment+"getDraftDecaissements";

      axios.get(url).then(function (response) {

        for(var key in response.data) {

          response.data[key].date_valeur = response.data[key].date_valeur.split("T")[0];
          response.data[key].date_compta = response.data[key].date_compta.split("T")[0];
          response.data[key].date_operation = response.data[key].date_operation.split("T")[0];
        }
        
        context.items = response.data;
        context.loading = false;
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

    getBanks(){

      let context=this;
      var urlAccountType = this.$executioEnvironment+"getAccountType";

      axios.get(urlAccountType).then(function (response) {
        
        context.allAccountType = response.data;

        for(var element in response.data) {

          context.allBank.push(element);
        }
      })
      .catch(function (error) {
        
        console.log(error);
      });
    },
    
    getPaiements(){

      let context=this;
      var urlAllPaymentMode = this.$executioEnvironment+"getPaymentMethod";

      axios.get(urlAllPaymentMode).then(function (response) {
        
        for(var element in response.data) {

          context.allPaymentMode.push(response.data[element].name);
        }
      })
      .catch(function (error) {
        
        console.log(error);
      });
    },    

    getCurrentDate(){

      var today = new Date();

      this.currentDay = parseInt(String(today.getDate()).padStart(2, '0'));
      this.currentMonth = parseInt(String(today.getMonth() + 1).padStart(2, '0'));
      this.currentYear = parseInt(today.getFullYear());
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
      else {

        formatedNumber = "0";
      }

      return formatedNumber;
    },

    onChangeClassification: function () {

      this.allUnderClassification = [];
      
      var selectedClassification = this.selectedItem.classification;

      var allUnderClassificationForSelectedClassification = this.allClassificationType[selectedClassification];

      for(var key in allUnderClassificationForSelectedClassification){

        this.allUnderClassification.push(allUnderClassificationForSelectedClassification[key].name);
      }
    },

    onChangeBank: function () {
      
      var selectedBank = this.editedItem.banque;

      this.allBankAccount = this.allAccountType[selectedBank];
    },

    rapprocherLine(item) {

      item.sous_classification = this.findUnderClassificationId(item);

      var url = this.$executioEnvironment+"ValidateDecaissement";

      if(null==item.versement_tva || undefined==item.versement_tva) {

        item.versement_tva = false;
      }

      var stringifiedItem = JSON.stringify({

        "id_decaissement":item.id,
        "id_sous_classification":item.sous_classification,
        "mois_valeur":item.mois_valeur,
        "tva_deductible":item.tva_deductible,
        "montant_ht":item.montant_ht,
        "versement_tva":item.versement_tva
      });

      axios.post(url, stringifiedItem, {
        
        headers: {
          Accept: "application/json",
          "Content-type": "application/json"
        }
      })
      .then(response => {

        console.log(response);

        this.snackbar = true,
        this.snackbarText = "Ligne Sauvegardée dans la Base de Données",
        this.snackbarColor = "success"

        this.getItems();
      })
      .catch(error => {

        console.log(error.response.data);

        this.snackbar = true,
        this.snackbarText = "Erreur lors de la Sauvegarde dans la Base de Données",
        this.snackbarColor = "error"
      });
    },

    findUnderClassificationId(item){

      for(var classification in this.allClassificationType) {

        if(classification == item.classification){

          var underClassificationList = this.allClassificationType[classification];

          for(var key in underClassificationList){

            var underClassification = underClassificationList[key];

            if(underClassification.name == item.sous_classification){

              return underClassification.id;
            }
          }
        }
      }
    },   

    saveEditedItem(item){

      var url = this.$executioEnvironment+"EditDecaissement";

      console.log(item);

      var stringifiedItem = JSON.stringify(item);
      
      axios.post(url, stringifiedItem, {
        
        headers: {
          Accept: "application/json",
          "Content-type": "application/json"
        }
      }).then(response=>{

        console.log(response);

        this.snackbar = true,
        this.snackbarText = "Ligne bien modifié dans la base de données",
        this.snackbarColor = "success"

        this.getItems();
      })
      .catch(error => {

        console.log(error);

        this.snackbar = true,
        this.snackbarText = "Erreur lors de la Modification de la ligne dans la Base de Données",
        this.snackbarColor = "error"
      });
    },

    rapprocherItem(item) {

      this.selectedItem = this.items.indexOf(item);
      this.selectedItem = Object.assign({}, item);
      this.selectedItem.mois_valeur=this.selectedItem.date_valeur.split('-')[1].replace('0','')
      this.dialog = true;

      this.onChangeClassification();
    },

    editItem(item) {

      this.editedItem = Object.assign({}, item);

      this.dialogEdit = true;

      this.onChangeBank();
    },

    close() {

      this.dialog = false;

      setTimeout(() => {

        this.selectedItem = Object.assign({}, this.defaultItem);
        this.selectedIndex = -1;
      }, 300);

      this.allUnderClassification = [];

      this.$refs.form.resetValidation();
    },   

    closeEdit() {

      this.dialogEdit = false;

      setTimeout(() => {

        this.editedItem = Object.assign({}, this.defaultItem);

      }, 300);

      this.$refs.formEdit.resetValidation();
    },

    save() {      

      this.rapprocherLine(this.selectedItem);      

      this.close();
    },

    saveEdit() {      

      this.saveEditedItem(this.editedItem);      

      this.closeEdit();
    }
  }
};
</script>