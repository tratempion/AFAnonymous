<template>
  
  <div>
  
    <v-app>

        <v-snackbar v-model="snackbar" :color="snackbarColor" :bottom="snackbarY==='bottom'" :left="snackbarX==='left'" :multi-line="snackbarMode==='multi-line'" :right="snackbarX==='right'" :timeout="3700" :top="snackbarY==='top'" :vertical="snackbarMode==='vertical'">
            {{ snackbarText }}
            <v-btn dark flat @click="snackbar=false">Fermer</v-btn>
        </v-snackbar>

        <v-container fluid grid-list-md>

            <v-layout row wrap style=" background-color: #00897B">
                
                <v-flex>
                    <v-autocomplete dark v-model="selectedYear" :items="selectAbleYear" v-on:change="refreshBudget" label="Année"></v-autocomplete>   
                </v-flex>
                <v-flex>
                    <v-autocomplete dark v-model="selectedClassification" :items="allClassification" v-on:change="refreshBudget" label="Classification"></v-autocomplete>
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
                        Sélectionnez une année et une classification afin d'avoir accès aux budgets
                    </v-alert>

                </v-layout>

                <v-layout v-resize="onResize" align-baseline="true" row wrap v-else>
        
                    <template>

                        <v-expansion-panel dark>

                            <v-expansion-panel-content v-for="(item, i) in 12" :key="i" class="teal lighten-2" style="margin-top: 3px">

                                <template v-slot:header>
                                    <div>{{monthName[i]}}</div>
                                </template>

                                <template v-slot:actions>
                                    <div :key="reRenderKey">Total : {{ formatNumber(eachMonthTotal[i]) }}€</div>
                                </template>

                                <v-card light>

                                    <v-data-table :headers="headers" :items="yearBudget[i]" :hide-headers="isMobile" :class="{mobile: isMobile}" hide-actions>

                                        <template v-slot:items="props">
                                            <tr v-if="!isMobile">
                                                <td>
                                                    <v-btn icon small @click="editItem(i, props.item)" color="amber">
                                                        <v-icon color="white">edit</v-icon>
                                                    </v-btn>
                                                    <v-btn icon small @click="openDeleteDialog(props.item)" color="red">
                                                        <v-icon color="white">delete</v-icon>
                                                    </v-btn>
                                                </td>
                                                <td>{{ props.item.sous_classification }}</td>
                                                <td>{{ formatNumber(props.item.budget) }}</td>
                                            </tr>
                                            <tr v-else>
                                                <td>
                                                    <ul class="flex-content">

                                                        <li class="flex-item" data-label="Modifier">
                                                            <v-btn block @click="editItem(i, props.item)" color="amber">
                                                                <v-icon color="white">edit</v-icon>
                                                            </v-btn>
                                                        </li>
                                                        <li class="flex-item" data-label="Supprimer">
                                                            <v-btn block @click="openDeleteDialog(props.item)" color="red">
                                                                <v-icon color="white">delete</v-icon>
                                                            </v-btn>
                                                        </li>

                                                        <li class="flex-item" data-label="Sous Classification" style="margin-top:25px">{{ props.item.sous_classification }}</li>
                                                        <li class="flex-item" data-label="Budget" style="margin-top:25px;margin-bottom:10px;">{{ formatNumber(props.item.budget) }}</li>
                                                        
                                                    </ul>
                                                </td>
                                            </tr>
                                        </template>

                                        <template v-slot:no-data>
                                            Il n'y à pas de budget rentré pour ce mois
                                        </template>
                                        
                                        <template v-slot:footer style="padding-left: 10px">
                                            <v-btn dark @click="addNewBudget(i)" color="light-green" v-if="!isMobile">
                                                <v-icon color="white">add</v-icon>
                                                Ajouter
                                            </v-btn>
                                            <v-btn dark block @click="addNewBudget(i)" color="light-green" v-else>
                                                <v-icon color="white">add</v-icon>
                                                Ajouter
                                            </v-btn>
                                        </template>

                                    </v-data-table>

                                </v-card>
                                
                            </v-expansion-panel-content>

                        </v-expansion-panel>

                    </template>

                </v-layout>

            </div>

        </v-container>
  
        <v-layout row justify-center>

            <v-dialog v-model="dialog" height="100%">

                <v-form ref="form" v-model="valid">

                  <v-card>
                    <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                      <span class="headline">{{ formTitle }}</span>
                    </v-card-title>
                    <v-card-text>
                      <v-container grid-list-md>
                        <v-layout wrap>
                          <v-flex>
                            <v-autocomplete v-model="editedBudget.sous_classification" :items="availableUnderClassification" label="Sous Classification" :rules="sousClassificationRules" required></v-autocomplete>
                          </v-flex>
                          <v-flex>
                            <v-text-field v-model="editedBudget.budget" label="Budget" :rules="budgetRules" required></v-text-field>
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

            <v-dialog v-model="dialogDelete" persistent max-width="500">
                <v-card>

                    <v-toolbar card color="amber">
                        <strong>Attention !</strong>
                    </v-toolbar>
                    
                    <v-card-text>Etes vous sure de vouloir supprimer ce budget ?</v-card-text>

                    <v-card-actions>

                        <v-btn block color="info" @click="dialogDelete=false">Annuler</v-btn>
                    
                        <v-spacer></v-spacer>
                    
                        <v-btn block color="error" @click="deleteBudget">Oui</v-btn>
                    
                    </v-card-actions>

                </v-card>
            </v-dialog>

        </v-layout>

    </v-app>
  
  </div>

</template>

<script>

const axios = require("axios");

export default {

    name: "budget",

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
            selectedClassification: "",
            
            allClassification : [],
            allClassificationType: [],

            availableUnderClassification: [],

            yearBudget: [],

            monthName: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Aout", "Septembre", "Octobre", "Novembre", "Décembre"],

            eachMonthTotal: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],

            reRenderKey: 0,
            
            headers: [
                
                { text: "Action", value: "action", sortable: false },
                { text: "Sous Classification", value: "sous_classification", sortable: true },
                { text: "Budget", value: "budget", sortable: true },
            ],
            
            addedBudgetMonth: -1,
            editedBudgetMonth: -1,


            editionMode: false,

            dialog: false,
            valid: false,

            anneeRules: [
                v => !!v || 'Vous devez rentrer une année',
                v => (v && !isNaN(parseFloat(v)) && isFinite(v)) || 'L\'année doit être un chiffre',
            ],

            sousClassificationRules: [
                v => !!v || 'Vous devez selectioner une sous classification'
            ],

            budgetRules: [
                v => !!v || 'Vous devez rentrer un budget',
                v => (v && !isNaN(parseFloat(v)) && isFinite(v)) || 'Le budget doit être un chiffre'
            ],

            editedBudget: {

                annee: "",
                mois_valeur: "",
                id_sous_classification: "",
                sous_classification: "",
                budget: ""
            },

            defaultBudget: {

                annee: "",
                mois_valeur: "",
                id_sous_classification: "",
                sous_classification: "",
                budget: ""
            },

            dialogDelete: false,
            pendingDeleteBudget: ""
        };
    },

    created() {
        
        this.getYear();

        this.getClassification();
        this.selectedYear=new Date().getFullYear();
    },

    watch: {

        dialog(val) {

            val || this.close();

            this.$refs.form.resetValidation(); 
        },
    },

    computed: {

        formTitle() {

        return this.editionMode === false
            ? "Ajouter un Budget"
            : "Modifier un Budget";
        }
    },

    methods: {

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

        getYear(){
    
            var actualYear = new Date().getFullYear();

            for(var y=2000; y <= (actualYear +10); y++){

                this.selectAbleYear.push(y);
            }
        },

        getBudget(){

            let context=this;
      
            var urlAccountType = this.$executioEnvironment+"GetBudget";

            return axios.get(urlAccountType, {

                params: {

                    "annee": context.selectedYear,
                    "classification": context.selectedClassification
                }

            }).then(function (response) {

                context.yearBudget = JSON.parse(response.data);
            })
            .catch(function (error) {
        
                console.log(error);
            });
        },

        refreshBudget: function () {

            if(null != this.selectedYear && undefined != this.selectedYear && this.selectedYear != ""){
                
                if(null != this.selectedClassification && undefined != this.selectedClassification && this.selectedClassification != ""){
    
                    this.loading = true;

                    this.getBudget().then((returnVal) => {
   
                        this.calculateAllMonthTotal();

                        this.showable = true;

                        this.reRenderKey += 1;

                        this.loading = false;
                    });
                }
            }
        },

        calculateAllMonthTotal(){

            for(var month in this.yearBudget){

                var allMonthUnderClassification = this.yearBudget[month];

                this.eachMonthTotal[month] = 0;

                for(var key in allMonthUnderClassification){

                    this.eachMonthTotal[month] += parseFloat(allMonthUnderClassification[key].budget);
                }
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
        
        addNewBudget(month) {
            
            this.$refs.form.resetValidation();

            this.addedBudgetMonth = month;

            this.makeAvailableUnderClassificationList(month);

            this.dialog = true;
        },

        editItem(month, item) {

            this.$refs.form.resetValidation();

            this.editionMode = true;

            this.editedBudgetMonth = month;

            this.editedBudget = Object.assign({}, item);

            this.makeAvailableUnderClassificationList(month);

            this.availableUnderClassification.push(item.sous_classification);

            this.dialog = true;
        },
        
        makeAvailableUnderClassificationList(month){

            this.availableUnderClassification = [];

            var allUnderClassificationForSelectedClassification = this.allClassificationType[this.selectedClassification];

            for(var key in allUnderClassificationForSelectedClassification){

                var underClassification = allUnderClassificationForSelectedClassification[key].name;

                var isAvailable = true;

                for(var key in this.yearBudget[month]){

                    var usedUnderClassification = this.yearBudget[month][key].sous_classification;

                    if(underClassification==usedUnderClassification){

                        isAvailable = false;
                    }                    
                }

                if(isAvailable){

                    this.availableUnderClassification.push(underClassification);
                }
            }
        },        

        save() {      

            if (this.editionMode) {

                this.saveEdit(this.editedBudget);
            } 
            else {
                
                this.saveLine(this.editedBudget);
            }

            this.close();
        },

        saveLine(item) {

            var url = this.$executioEnvironment+"SaveBudget";

            item.annee = this.selectedYear;
            item.mois_valeur = (this.addedBudgetMonth)+1;
            
            item.id_sous_classification = this.findUnderClassificationId();

            var stringifiedBudget = JSON.stringify(item);
            
            axios.post(url, stringifiedBudget, {
                
                headers: {
                Accept: "application/json",
                "Content-type": "application/json"
                }
            })
            .then(response => {

                console.log(response);

                this.snackbar = true;

                this.snackbarText = "Budget Sauvegardée dans la Base de Données";
                this.snackbarColor = "success";

                this.getBudget().then((returnVal) => {
   
                        this.calculateAllMonthTotal();

                    });  
            })
            .catch(error => {

                console.log(error.response);

                this.snackbar = true;

                this.snackbarText = "Erreur lors de la Sauvegarde dans la Base de Données";
                this.snackbarColor = "error";
            });
        },        

        saveEdit(item){

            var url = this.$executioEnvironment+"EditBudget";

            item.annee = this.selectedYear;
            item.mois_valeur = (this.editedBudgetMonth)+1;
            
            item.id_sous_classification = this.findUnderClassificationId();

            var stringifiedBudget = JSON.stringify(item);
            
            axios.post(url, stringifiedBudget, {
                
                headers: {
                
                    Accept: "application/json",
                    "Content-type": "application/json"
                }                
            }).then(response=>{

                console.log(response);

                this.snackbar = true,
                this.snackbarText = "Budget bien modifié dans la base de données",
                this.snackbarColor = "success"

                this.getBudget().then((returnVal) => {
   
                        this.calculateAllMonthTotal();

                    });            
            })
            .catch(error => {

                console.log(error);

                this.snackbar = true,
                this.snackbarText = "Erreur lors de la Modification de la ligne dans la Base de Données",
                this.snackbarColor = "error"
            });
        },

        findUnderClassificationId(){

            var underClassificationList = this.allClassificationType[this.selectedClassification];

            for(var key in underClassificationList){

                var underClassification = underClassificationList[key];

                if(underClassification.name == this.editedBudget.sous_classification){

                    return underClassification.id;
                }
            }
        },

        close() {

            this.dialog = false;

            setTimeout(() => {

                this.editedBudget = Object.assign({}, this.defaultBudget);

                this.editionMode = false;

            }, 300);

            this.$refs.form.resetValidation();
        },

        openDeleteDialog(item) {

            this.pendingDeleteBudget = item;

            this.dialogDelete = true;
        },

        deleteBudget() {

            var urlDeleteBudget = this.$executioEnvironment+"DeleteBudget";

            var budget = this.pendingDeleteBudget;

            var stringifiedBudget = JSON.stringify(budget);
            
            axios.post(urlDeleteBudget, stringifiedBudget, {
                
                headers: {
                    
                    Accept: "application/json",
                    "Content-type": "application/json"
                }
            })
            .then(response => {

                console.log(response);

                this.snackbar = true,
                this.snackbarText = "Budget supprimé de la base de données",
                this.snackbarColor = "success"

                this.getBudget().then((returnVal) => {
   
                        this.calculateAllMonthTotal();

                    });  
            })
            .catch(error => {

                console.log('erreur : '+error);

                this.snackbar = true,
                this.snackbarText = "Erreur lors de la suppression dans la Base de Données",
                this.snackbarColor = "error"
            });
            
            this.pendingDeleteBudget = "";

            this.dialogDelete = false;
        },
    }  
};
</script>

<style scoped>

</style>