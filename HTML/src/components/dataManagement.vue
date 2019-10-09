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

        <v-card>
          <div>
            <v-dialog v-model="dialogSociete" height="100%" :width="isMobile?'100%':'30%'">
              <v-form ref="formSociete" v-model="validSociete">
                <v-card>
                  <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                    <span class="headline">{{ societeFormTitle }}</span>
                  </v-card-title>
                  <v-card-text>
                    <v-container grid-list-md>
                      <v-flex>
                        <v-text-field v-model="editedSociete.name" label="Nom" :rules="societeRule" required></v-text-field>
                      </v-flex>
                    </v-container>
                  </v-card-text>
                  <v-card-actions v-if="!isMobile">
                    <v-btn color="warning" block @click.native="closeSociete" large>Annuler</v-btn>
                    <v-btn color="success" block @click.native="saveSociete" :disabled="!validSociete" large>Sauvegarder</v-btn>
                  </v-card-actions>
                  <v-card-text v-else>
                    <v-container grid-list-md>
                      <v-layout wrap>
                        <v-flex>
                          <v-btn color="success" block @click.native="saveSociete" :disabled="!validSociete" large>Sauvegarder</v-btn>
                        </v-flex>
                        <v-flex>
                          <v-btn color="warning" block @click.native="closeSociete" large>Annuler</v-btn>
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
            <v-dialog v-model="dialogModePaiement" height="100%" :width="isMobile?'100%':'30%'">
              <v-form ref="formModePaiement" v-model="validModePaiement">
                <v-card>
                  <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                    <span class="headline">{{ modePaiementFormTitle }}</span>
                  </v-card-title>
                  <v-card-text>
                    <v-container grid-list-md>
                      <v-flex>
                        <v-text-field v-model="editedModePaiement.name" label="Nom" :rules="modePaiementRule" required></v-text-field>
                      </v-flex>
                    </v-container>
                  </v-card-text>
                  <v-card-actions v-if="!isMobile">
                    <v-btn color="warning" block @click.native="closeModePaiement" large>Annuler</v-btn>
                    <v-btn color="success" block @click.native="saveModePaiement" :disabled="!validModePaiement" large>Sauvegarder</v-btn>
                  </v-card-actions>
                  <v-card-text v-else>
                    <v-container grid-list-md>
                      <v-layout wrap>
                        <v-flex>
                          <v-btn color="success" block @click.native="saveModePaiement" :disabled="!validModePaiement" large>Sauvegarder</v-btn>
                        </v-flex>
                        <v-flex>
                          <v-btn color="warning" block @click.native="closeModePaiement" large>Annuler</v-btn>
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
            <v-dialog v-model="dialogClassification" height="100%" :width="isMobile?'100%':'30%'">
              <v-form ref="formClassification" v-model="validClassification">
                <v-card>
                  <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                    <span class="headline">{{ classificationFormTitle }}</span>
                  </v-card-title>
                  <v-card-text>
                    <v-container grid-list-md>
                      <v-flex>
                        <v-text-field v-model="editedClassification.name" label="Nom" :rules="classificationRule" required></v-text-field>
                      </v-flex>
                    </v-container>
                  </v-card-text>
                  <v-card-actions v-if="!isMobile">
                    <v-btn color="warning" block @click.native="closeClassification" large>Annuler</v-btn>
                    <v-btn color="success" block @click.native="saveClassification" :disabled="!validClassification" large>Sauvegarder</v-btn>
                  </v-card-actions>
                  <v-card-text v-else>
                    <v-container grid-list-md>
                      <v-layout wrap>
                        <v-flex>
                          <v-btn color="success" block @click.native="saveClassification" :disabled="!validClassification" large>Sauvegarder</v-btn>
                        </v-flex>
                        <v-flex>
                          <v-btn color="warning" block @click.native="closeClassification" large>Annuler</v-btn>
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
            <v-dialog v-model="dialogBanque" height="100%" :width="isMobile?'100%':'30%'">
              <v-form ref="formBanque" v-model="validBanque">
                <v-card>
                  <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                    <span class="headline">{{ banqueFormTitle }}</span>
                  </v-card-title>
                  <v-card-text>
                    <v-container grid-list-md>
                      <v-flex>
                        <v-text-field v-model="editedBanque.name" label="Nom" :rules="banqueRule" required></v-text-field>
                      </v-flex>
                      <v-flex>
                        <v-autocomplete v-model="editedBanque.compte" :items="allBankAccount" item-text="numero" return-object label="Compte par défault" :rules="banqueRule" required></v-autocomplete>
                      </v-flex>
                    </v-container>
                  </v-card-text>
                  <v-card-actions v-if="!isMobile">
                    <v-btn color="warning" block @click.native="closeBanque" large>Annuler</v-btn>
                    <v-btn color="success" block @click.native="saveBanque" :disabled="!validBanque" large>Sauvegarder</v-btn>
                  </v-card-actions>
                  <v-card-text v-else>
                    <v-container grid-list-md>
                      <v-layout wrap>
                        <v-flex>
                          <v-btn color="success" block @click.native="saveBanque" :disabled="!validBanque" large>Sauvegarder</v-btn>
                        </v-flex>
                        <v-flex>
                          <v-btn color="warning" block @click.native="closeBanque" large>Annuler</v-btn>
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
            <v-dialog v-model="dialogCompte" height="100%" :width="isMobile?'100%':'30%'">
              <v-form ref="formCompte" v-model="validCompte">
                <v-card>
                  <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                    <span class="headline">{{ compteFormTitle }}</span>
                  </v-card-title>
                  <v-card-text>
                    <v-container grid-list-md>
                      <v-flex>
                        <v-text-field v-model="editedCompte.numero" label="Numero" :rules="compteRule" required></v-text-field>
                      </v-flex>
                    </v-container>
                  </v-card-text>
                  <v-card-actions v-if="!isMobile">
                    <v-btn color="warning" block @click.native="closeCompte" large>Annuler</v-btn>
                    <v-btn color="success" block @click.native="saveCompte" :disabled="!validCompte" large>Sauvegarder</v-btn>
                  </v-card-actions>
                  <v-card-text v-else>
                    <v-container grid-list-md>
                      <v-layout wrap>
                        <v-flex>
                          <v-btn color="success" block @click.native="saveCompte" :disabled="!validCompte" large>Sauvegarder</v-btn>
                        </v-flex>
                        <v-flex>
                          <v-btn color="warning" block @click.native="closeCompte" large>Annuler</v-btn>
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
            <v-dialog v-model="dialogSousClassification" height="100%" :width="isMobile?'100%':'30%'">
              <v-form ref="formSousClassification" v-model="validSousClassification">
                <v-card>
                  <v-card-title style="background-color:rgb(25, 118, 210); color:white">
                    <span class="headline">{{ sousClassificationFormTitle }}</span>
                  </v-card-title>
                  <v-card-text>
                    <v-container grid-list-md>
                      <v-flex>
                        <v-text-field v-model="editedSousClassification.name" label="Nom" :rules="sousClassificationRule" required></v-text-field>
                      </v-flex>
                      <v-flex>
                        <v-autocomplete v-model="editedSousClassification.classification" :items="allClassification" item-text="name" return-object label="Classification" :rules="sousClassificationRule" required></v-autocomplete>
                      </v-flex>
                    </v-container>
                  </v-card-text>
                  <v-card-actions v-if="!isMobile">
                    <v-btn color="warning" block @click.native="closeSousClassification" large>Annuler</v-btn>
                    <v-btn color="success" block @click.native="saveSousClassification" :disabled="!validSousClassification" large>Sauvegarder</v-btn>
                  </v-card-actions>
                  <v-card-text v-else>
                    <v-container grid-list-md>
                      <v-layout wrap>
                        <v-flex>
                          <v-btn color="success" block @click.native="saveSousClassification" :disabled="!validSousClassification" large>Sauvegarder</v-btn>
                        </v-flex>
                        <v-flex>
                          <v-btn color="warning" block @click.native="closeSousClassification" large>Annuler</v-btn>
                        </v-flex>
                      </v-layout>
                    </v-container>
                  </v-card-text>
                </v-card>
              </v-form>
            </v-dialog>
          </div>
        </v-card>

        <v-container fluid grid-list-md text-xs-center>
      
          <v-layout v-resize="onResize" row wrap style="margin-top: 3px" v-if="!isMobile">
            
            <v-flex xs6>

              <v-expansion-panel>

                <v-expansion-panel-content :key="0" class="cyan">
                
                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Société</strong>
                    </div>
                  </template>

                  <v-card dark>
                    
                    <v-layout row wrap>

                      <v-flex xs6>
                        <v-text-field v-model="searchSociete" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>
                      </v-flex>

                      <v-flex xs6>
                        <v-btn dark block color="light-green" @click="addNewSociete()">
                          <v-icon color="white">add</v-icon>Ajouter
                        </v-btn>
                      </v-flex>

                    </v-layout>

                    <v-data-table dark :headers="headersSociete" :items="allSociete" :search="searchSociete" :pagination.sync="paginationSociete">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editSociete(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteSociete(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.name }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>

                </v-expansion-panel-content>

                <v-expansion-panel-content :key="1" class="teal" style="margin-top: 17px">

                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Mode de Paiement</strong>
                    </div>
                  </template>

                  <v-card dark>
                    
                    <v-layout row wrap>

                      <v-flex xs6>
                        <v-text-field v-model="searchModePaiement" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>
                      </v-flex>

                      <v-flex xs6>
                        <v-btn dark block color="light-green" @click="addNewModePaiement()">
                          <v-icon color="white">add</v-icon>Ajouter
                        </v-btn>
                      </v-flex>

                    </v-layout>

                    <v-data-table dark :headers="headersModePaiement" :items="allModePaiement" :search="searchModePaiement" :pagination.sync="paginationModePaiement">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editModePaiement(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteModePaiement(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.name }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>
                  
                </v-expansion-panel-content>

                <v-expansion-panel-content :key="2" class="deep-purple" style="margin-top: 17px">

                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Classification</strong>
                    </div>
                  </template>

                  <v-card dark>
                    
                    <v-layout row wrap>

                      <v-flex xs6>
                        <v-text-field v-model="searchClassification" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>
                      </v-flex>

                      <v-flex xs6>
                        <v-btn dark block color="light-green" @click="addNewClassification()">
                          <v-icon color="white">add</v-icon>Ajouter
                        </v-btn>
                      </v-flex>

                    </v-layout>

                    <v-data-table dark :headers="headersClassification" :items="allClassification" :search="searchClassification" :pagination.sync="paginationClassification">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editClassification(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteClassification(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.name }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>
                  
                </v-expansion-panel-content>

              </v-expansion-panel>

            </v-flex>

            <v-flex xs6>

              <v-expansion-panel>

                <v-expansion-panel-content :key="4" class="lime">

                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Banque</strong>
                    </div>
                  </template>

                  <v-card dark>

                    <v-layout row wrap>

                      <v-flex xs6>
                        <v-text-field v-model="searchBank" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>
                      </v-flex>

                      <v-flex xs6>
                        <v-btn dark block color="light-green" @click="addNewBanque()">
                          <v-icon color="white">add</v-icon>Ajouter
                        </v-btn>
                      </v-flex>

                    </v-layout>

                    <v-data-table :headers="headersBank" :items="allBank" :search="searchBank" :pagination.sync="paginationBank">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editBanque(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteBanque(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.name }}</td>
                          <td>{{ props.item.compte.numero }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>

                </v-expansion-panel-content>

                <v-expansion-panel-content :key="5" class="amber darken-3" style="margin-top: 17px">

                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Compte</strong>
                    </div>
                  </template>

                  <v-card dark>
                    
                    <v-layout row wrap>

                      <v-flex xs6>
                        <v-text-field v-model="searchCompte" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>
                      </v-flex>

                      <v-flex xs6>
                        <v-btn dark block color="light-green" @click="addNewCompte()">
                          <v-icon color="white">add</v-icon>Ajouter
                        </v-btn>
                      </v-flex>

                    </v-layout>

                    <v-data-table dark :headers="headersCompte" :items="allBankAccount" :search="searchCompte" :pagination.sync="paginationCompte">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editCompte(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteCompte(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.numero }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>
                    
                </v-expansion-panel-content>

                <v-expansion-panel-content :key="6" class="red" style="margin-top: 17px">

                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Sous Classification</strong>
                    </div>
                  </template>

                  <v-card dark>
                    
                    <v-layout row wrap>

                      <v-flex xs6>
                        <v-text-field v-model="searchSousClassification" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>
                      </v-flex>

                      <v-flex xs6>
                        <v-btn dark block color="light-green" @click="addNewSousClassification()">
                          <v-icon color="white">add</v-icon>Ajouter
                        </v-btn>
                      </v-flex>

                    </v-layout>

                    <v-data-table dark :headers="headersSousClassification" :items="allSousClassification" :search="searchSousClassification" :pagination.sync="paginationSousClassification">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editSousClassification(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteSousClassification(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.name }}</td>
                          <td>{{ props.item.classification.name }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>
                    
                </v-expansion-panel-content>

              </v-expansion-panel>          

            </v-flex>

          </v-layout>

          <v-layout v-resize="onResize" row wrap style="margin-top: 3px" v-else>

            <v-expansion-panel>

              <v-expansion-panel-content :key="0" class="cyan">
                
                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Société</strong>
                    </div>
                  </template>

                  <v-card dark>

                    <v-btn dark block color="light-green" @click="addNewSociete()">
                      <v-icon color="white">add</v-icon>Ajouter
                    </v-btn>
                        
                    <v-text-field v-model="searchSociete" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>               

                    <v-data-table dark :headers="headersSociete" :items="allSociete" :search="searchSociete" :pagination.sync="paginationSociete">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editSociete(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteSociete(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.name }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>

                </v-expansion-panel-content>

                <v-expansion-panel-content :key="1" class="teal" style="margin-top: 17px">

                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Mode de Paiement</strong>
                    </div>
                  </template>

                  <v-card dark>

                    <v-btn dark block color="light-green" @click="addNewModePaiement()">
                      <v-icon color="white">add</v-icon>Ajouter
                    </v-btn>

                    <v-text-field v-model="searchModePaiement" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>              

                    <v-data-table dark :headers="headersModePaiement" :items="allModePaiement" :search="searchModePaiement" :pagination.sync="paginationModePaiement">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editModePaiement(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteModePaiement(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.name }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>
                  
                </v-expansion-panel-content>

                <v-expansion-panel-content :key="4" class="lime" style="margin-top: 17px">

                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Banque</strong>
                    </div>
                  </template>

                  <v-card dark>

                    <v-btn dark block color="light-green" @click="addNewBanque()">
                      <v-icon color="white">add</v-icon>Ajouter
                    </v-btn>

                    <v-text-field v-model="searchBank" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>              

                    <v-data-table :headers="headersBank" :items="allBank" :search="searchBank" :pagination.sync="paginationBank">

                      <template slot="items" slot-scope="props" v-if()>

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editBanque(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteBanque(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.name }}</td>
                          <td>{{ props.item.compte.numero }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>

                </v-expansion-panel-content>

                <v-expansion-panel-content :key="5" class="amber darken-3" style="margin-top: 17px">

                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Compte</strong>
                    </div>
                  </template>

                  <v-card dark>
                  
                    <v-btn dark block color="light-green" @click="addNewCompte()">
                      <v-icon color="white">add</v-icon>Ajouter
                    </v-btn>

                    <v-text-field v-model="searchCompte" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>                   

                    <v-data-table dark :headers="headersCompte" :items="allBankAccount" :search="searchCompte" :pagination.sync="paginationCompte">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editCompte(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteCompte(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.numero }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>
                    
                </v-expansion-panel-content>

                <v-expansion-panel-content :key="2" class="deep-purple" style="margin-top: 17px">

                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Classification</strong>
                    </div>
                  </template>

                  <v-card dark>

                    <v-btn dark block color="light-green" @click="addNewClassification()">
                      <v-icon color="white">add</v-icon>Ajouter
                    </v-btn>
                    
                    <v-text-field v-model="searchClassification" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>                     

                    <v-data-table dark :headers="headersClassification" :items="allClassification" :search="searchClassification" :pagination.sync="paginationClassification">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editClassification(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteClassification(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.name }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>
                  
                </v-expansion-panel-content>

                <v-expansion-panel-content :key="6" class="red" style="margin-top: 17px">

                  <template v-slot:header>
                    <div style="color: white">
                      <strong>Sous Classification</strong>
                    </div>
                  </template>

                  <v-card dark>

                    <v-btn dark block color="light-green" @click="addNewSousClassification()">
                      <v-icon color="white">add</v-icon>Ajouter
                    </v-btn>
                    
                    <v-text-field v-model="searchSousClassification" prepend-icon="search" label="Rechercher" single-line hide-details></v-text-field>

                    <v-data-table dark :headers="headersSousClassification" :items="allSousClassification" :search="searchSousClassification" :pagination.sync="paginationSousClassification">

                      <template slot="items" slot-scope="props">

                        <tr>

                          <td class="layout align-center px-0" style="margin-left: 2px">
                            <v-btn icon small color="amber" @click="editSousClassification(props.item)">
                              <v-icon color="white">edit</v-icon>
                            </v-btn>
                            <v-btn icon small color="red" @click="deleteSousClassification(props.item)">
                              <v-icon color="white">delete</v-icon>
                            </v-btn>
                          </td>

                          <td>{{ props.item.name }}</td>
                          <td>{{ props.item.classification.name }}</td>

                        </tr>

                      </template>

                    </v-data-table>

                  </v-card>
                    
                </v-expansion-panel-content>

            </v-expansion-panel>

          </v-layout>

        </v-container>
        
      </div>
    
    </v-app>
  </div>
</template>

<script>

const axios = require("axios");

export default {

  name: "dataManagement",

  data() {

    return {
      
      dialogSociete: false,
      validSociete: false,

      societeRule: [v => !!v || "Vous devez rentrer un nom de Société"],

      editedSociete: {
        id: "",
        name: ""
      },      

      defaultSociete: {
        id: "",
        name: ""
      },

      editedSocieteIndex: -1,

      dialogModePaiement: false,
      validModePaiement: false,

      modePaiementRule: [v => !!v || "Vous devez rentrer un nom de Mode de Paiement"],

      editedModePaiement: {
        id: "",
        name: ""
      },      

      defaultModePaiement: {
        id: "",
        name: ""
      },

      editedModePaiementIndex: -1,

      dialogClassification: false,
      validClassification: false,

      classificationRule: [v => !!v || "Vous devez rentrer un nom de Classification"],

      editedClassification: {
        id: "",
        name: ""
      },      

      defaultClassification: {
        id: "",
        name: ""
      },

      editedClassificationIndex: -1,

      dialogBanque: false,
      validBanque: false,

      banqueRule: [v => !!v || "Vous devez rentrer un nom de Banque"],

      editedBanque: {
        id: "",
        name: "",
        default_account: "",
        compte: ""
      },      

      defaultBanque: {
        id: "",
        name: "",
        default_account: "",
        compte: ""
      },

      editedBanqueIndex: -1,

      dialogCompte: false,
      validCompte: false,

      compteRule: [v => !!v || "Vous devez rentrer un nom de Compte"],

      editedCompte: {
        id: "",
        numero: ""
      },      

      defaultCompte: {
        id: "",
        numero: ""
      },

      editedCompteIndex: -1,

      dialogSousClassification: false,
      validSousClassification: false,

      sousClassificationRule: [v => !!v || "Vous devez rentrer un nom de Sous Classification"],

      editedSousClassification: {
        id: "",
        name: "",
        classification: ""
      },      

      defaultSousClassification: {
        id: "",
        name: "",
        classification: ""
      },

      editedSousClassificationIndex: -1,

      panelLeft: [true, false, false],
      panelRight: [true, false, false],

      paginationSociete: {

        sortBy: "name",
        rowsPerPage: 5
      },

      paginationModePaiement: {

        sortBy: "name",
        rowsPerPage: 5
      },

      paginationClassification: {

        sortBy: "name",
        rowsPerPage: 5
      },

      paginationBank: {

        sortBy: "name",
        rowsPerPage: 5
      },

      paginationCompte: {

        sortBy: "numero",
        rowsPerPage: 5
      },

      paginationSousClassification: {

        sortBy: "name",
        rowsPerPage: 5
      },

      headersSociete: [
        { text: "Action", value: "action", sortable: false },
        { text: "Nom", value: "name", sortable: true },
      ],

      headersModePaiement: [
        { text: "Action", value: "action", sortable: false },
        { text: "Nom", value: "name", sortable: true },
      ],

      headersClassification: [
        { text: "Action", value: "action", sortable: false },
        { text: "Nom", value: "name", sortable: true },
      ],

      headersBank: [
        { text: "Action", value: "action", sortable: false },
        { text: "Nom", value: "name", sortable: true },
        { text: "Compte par défault", value: "compte.numero", sortable: true }
      ],

      headersCompte: [
        { text: "Action", value: "action", sortable: false },
        { text: "Numero", value: "numero", sortable: true },
      ],

      headersSousClassification: [
        { text: "Action", value: "action", sortable: false },
        { text: "Nom", value: "name", sortable: true },
        { text: "Classification", value: "classification.name", sortable: true },
      ],

      searchSociete: "",
      searchModePaiement: "",
      searchClassification: "",

      searchBank: "",
      searchCompte: "",
      searchSousClassification: "",

      snackbar: false,
      snackbarText: "",
      snackbarColor: "",
      snackbarY: 'top',
      snackbarX: null,
      snackbarMode: '',

      isMobile: false,
      loading: true,
      
      allSociete: [],
      allModePaiement: [],
      allClassification: [],

      allBank: [],
      allBankAccount: [],
      allSousClassification: []
    };
  },

  created() {
    
    this.getBankAccount();

    this.getClassification();

    this.getSociete();
    
    this.getModePaiement();

    var context = this;

    setTimeout(function(){  

      context.getBanks();
       
      context.getSousClassification();
    }, 1117);

    if(this.allBank.length==0){

      this.getBanks();
    }

    if(this.allSousClassification.length==0){

      this.getSousClassification();
    }

    this.loading = false;
  },

  computed: {

    societeFormTitle() {

      return this.editedSocieteIndex === -1
        ? "Ajouter une Société"
        : "Modifier une Société";
    },

    modePaiementFormTitle() {

      return this.editedModePaiementIndex === -1
        ? "Ajouter un Mode de Paiement"
        : "Modifier un Mode de Paiement";
    },

    classificationFormTitle() {

      return this.editedClassificationIndex === -1
        ? "Ajouter une Classification"
        : "Modifier une Classification";
    },    

    banqueFormTitle() {

      return this.editedBanqueIndex === -1
        ? "Ajouter une Banque"
        : "Modifier une Banque";
    },            

    compteFormTitle() {

      return this.editedCompteIndex === -1
        ? "Ajouter une Compte"
        : "Modifier une Compte";
    },            

    sousClassificationFormTitle() {

      return this.editedSousClassificationIndex === -1
        ? "Ajouter une Sous Classification"
        : "Modifier une Sous Classification";
    }
  },

  watch: {

    dialogSociete(val) {

      val || this.closeSociete();

      this.$refs.formSociete.resetValidation();
    },
    
    dialogModePaiement(val) {

      val || this.closeModePaiement();

      this.$refs.formModePaiement.resetValidation();
    },

    dialogClassification(val) {

      val || this.closeClassification();

      this.$refs.formClassification.resetValidation();
    },

    dialogBanque(val) {

      val || this.closeBanque();

      this.$refs.formBanque.resetValidation();
    },

    dialogCompte(val) {

      val || this.closeCompte();

      this.$refs.formCompte.resetValidation();
    },

    dialogSousClassification(val) {

      val || this.closeSousClassification();

      this.$refs.formSousClassification.resetValidation();
    },
  },

  methods: {

    getSociete(){

      var urlBankAccountCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"societe",
        "command":"read"
      };

      axios.post(urlBankAccountCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        this.allSociete = response.data;
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la récupération des Sociétés dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    getModePaiement(){

      var urlBankAccountCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"mode_paiement",
        "command":"read"
      };

      axios.post(urlBankAccountCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        this.allModePaiement = response.data;
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la récupération des Modes de Paiement dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    getBankAndBankAccount: function() {

        getBankAccount().then(returnVal => {

          getBanks();
        });
    },

    getBankAccount(){

      var urlBankAccountCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"compte",
        "command":"read"
      };

      axios.post(urlBankAccountCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        this.allBankAccount = response.data;
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la récupération des Comptes dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    getBanks(){

      var urlBankCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"banque",
        "command":"read"
      };

      axios.post(urlBankCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{
           
        this.allBank = response.data;

        for(var key in this.allBank){

          this.allBank[key].compte = this.getBankAccountNumber(this.allBank[key].default_account);
        } 
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la récupération des Banques dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    getClassification(){

      var urlBankAccountCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"classification",
        "command":"read"
      };

      axios.post(urlBankAccountCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        this.allClassification = response.data;
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la récupération des Classifications dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    getSousClassification(){

      var urlBankAccountCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"sous_classification",
        "command":"read"
      };

      axios.post(urlBankAccountCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        this.allSousClassification = response.data;

        for(var key in this.allSousClassification){

          this.allSousClassification[key].classification = this.getClassificationName(this.allSousClassification[key].id_classification);
        }
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la récupération des Sous Classifications dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    getBankAccountNumber(searchedId){

      for(var key in this.allBankAccount){

        if(this.allBankAccount[key].id == searchedId){

          return this.allBankAccount[key];
        }
      }
    },

    getClassificationName(searchedId){

      for(var key in this.allClassification){

        if(this.allClassification[key].id == searchedId){

          return this.allClassification[key];
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
    
    //////////////////////////////////////////////////////////////////

    addNewSociete() {

      this.dialogSociete = true;
    },

    closeSociete() {

      this.dialogSociete = false;

      setTimeout(() => {

        this.editedSociete = Object.assign({}, this.defaultSociete);
        this.editedSocieteIndex = -1;
      }, 300);

      this.$refs.formSociete.resetValidation();
    },

    saveSociete() {

      if (this.editedSocieteIndex > -1) {

        this.saveEditSociete(this.editedSociete);
      } 
      else {

        this.saveNewSociete(this.editedSociete);
      }

      this.closeSociete();
    },    

    editSociete(item) {

      this.editedSocieteIndex = this.allSociete.indexOf(item);
      this.editedSociete = Object.assign({}, item);
      this.dialogSociete = true;
    },

    saveNewSociete(toSave) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"societe",
        "command":"create",
        "data":{
        
          "name": toSave.name
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Sauvegardée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getSociete();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la création de la Société dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeSociete();
    },

    saveEditSociete(toEdit) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"societe",
        "command":"update",
        "data":{
        
          "id": toEdit.id,
          "name": toEdit.name
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Modifiée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getSociete();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la modification de la Société dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeSociete();
    },    

    deleteSociete(toDelete) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"societe",
        "command":"delete",
        "data":{
        
          "id": toDelete.id
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Supprimée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getSociete();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la suppression de la Société dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    //////////////////////////////////////////////////////////////////

    addNewModePaiement() {

      this.dialogModePaiement = true;
    },

    closeModePaiement() {

      this.dialogModePaiement = false;

      setTimeout(() => {

        this.editedModePaiement = Object.assign({}, this.defaultModePaiement);
        this.editedModePaiementIndex = -1;
      }, 300);

      this.$refs.formModePaiement.resetValidation();
    },

    saveModePaiement() {

      if (this.editedModePaiementIndex > -1) {

        this.saveEditModePaiement(this.editedModePaiement);
      } 
      else {

        this.saveNewModePaiement(this.editedModePaiement);
      }

      this.closeModePaiement();
    },    

    editModePaiement(item) {

      this.editedModePaiementIndex = this.allModePaiement.indexOf(item);
      this.editedModePaiement = Object.assign({}, item);
      this.dialogModePaiement = true;
    },

    saveNewModePaiement(toSave) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"mode_paiement",
        "command":"create",
        "data":{
        
          "name": toSave.name
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Sauvegardée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getModePaiement();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la création du Mode de Paiement dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeModePaiement();
    },

    saveEditModePaiement(toEdit) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"mode_paiement",
        "command":"update",
        "data":{
        
          "id": toEdit.id,
          "name": toEdit.name
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Modifiée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getModePaiement();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la modification de la Mode de Paiement dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeModePaiement();
    },    

    deleteModePaiement(toDelete) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"mode_paiement",
        "command":"delete",
        "data":{
        
          "id": toDelete.id
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Supprimée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getModePaiement();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la suppression du Mode de Paiement dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    //////////////////////////////////////////////////////////////////

    addNewClassification() {

      this.dialogClassification = true;
    },

    closeClassification() {

      this.dialogClassification = false;

      setTimeout(() => {

        this.editedClassification = Object.assign({}, this.defaultClassification);
        this.editedClassificationIndex = -1;
      }, 300);

      this.$refs.formClassification.resetValidation();
    },

    saveClassification() {

      if (this.editedClassificationIndex > -1) {

        this.saveEditClassification(this.editedClassification);
      } 
      else {

        this.saveNewClassification(this.editedClassification);
      }

      this.closeClassification();
    },    

    editClassification(item) {

      this.editedClassificationIndex = this.allClassification.indexOf(item);
      this.editedClassification = Object.assign({}, item);
      this.dialogClassification = true;
    },

    saveNewClassification(toSave) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"classification",
        "command":"create",
        "data":{
        
          "name": toSave.name
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Sauvegardée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getClassification();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la création de la Classification dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeClassification();
    },

    saveEditClassification(toEdit) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"classification",
        "command":"update",
        "data":{
        
          "id": toEdit.id,
          "name": toEdit.name
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Modifiée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getClassification();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la modification de la Classification dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeClassification();
    },    

    deleteClassification(toDelete) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"classification",
        "command":"delete",
        "data":{
        
          "id": toDelete.id
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Supprimée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getClassification();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la suppression de la Classification dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    //////////////////////////////////////////////////////////////////

    addNewBanque() {

      this.dialogBanque = true;
    },

    closeBanque() {

      this.dialogBanque = false;

      setTimeout(() => {

        this.editedBanque = Object.assign({}, this.defaultBanque);
        this.editedBanqueIndex = -1;
      }, 300);

      this.$refs.formBanque.resetValidation();
    },

    saveBanque() {

      if (this.editedBanqueIndex > -1) {

        this.saveEditBanque(this.editedBanque);
      } 
      else {

        this.saveNewBanque(this.editedBanque);
      }

      this.closeBanque();
    },    

    editBanque(item) {

      this.editedBanqueIndex = this.allBank.indexOf(item);
      this.editedBanque = Object.assign({}, item);
      this.dialogBanque = true;
    },

    saveNewBanque(toSave) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"banque",
        "command":"create",
        "data":{
        
          "name": toSave.name,
          "default_account": toSave.compte.id
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Sauvegardée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getBanks();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la création de la Banque dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeBanque();
    },

    saveEditBanque(toEdit) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"banque",
        "command":"update",
        "data":{
        
          "id": toEdit.id,
          "name": toEdit.name,
          "default_account": toEdit.compte.id
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Modifiée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getBanks();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la modification de la Banque dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeBanque();
    },    

    deleteBanque(toDelete) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"banque",
        "command":"delete",
        "data":{
        
          "id": toDelete.id
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Supprimée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getBanks();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la suppression de la Banque dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    //////////////////////////////////////////////////////////////////

    addNewCompte() {

      this.dialogCompte = true;
    },

    closeCompte() {

      this.dialogCompte = false;

      setTimeout(() => {

        this.editedCompte = Object.assign({}, this.defaultCompte);
        this.editedCompteIndex = -1;
      }, 300);

      this.$refs.formCompte.resetValidation();
    },

    saveCompte() {

      if (this.editedCompteIndex > -1) {

        this.saveEditCompte(this.editedCompte);
      } 
      else {

        this.saveNewCompte(this.editedCompte);
      }

      this.closeCompte();
    },    

    editCompte(item) {

      this.editedCompteIndex = this.allBankAccount.indexOf(item);
      this.editedCompte = Object.assign({}, item);
      this.dialogCompte = true;
    },

    saveNewCompte(toSave) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"compte",
        "command":"create",
        "data":{
        
          "numero": toSave.numero
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Sauvegardée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getBankAccount();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la création du Compte dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeCompte();
    },

    saveEditCompte(toEdit) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"compte",
        "command":"update",
        "data":{
        
          "id": toEdit.id,
          "numero": toEdit.numero
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Modifiée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getBankAccount();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la modification du Compte dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeCompte();
    },    

    deleteCompte(toDelete) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"compte",
        "command":"delete",
        "data":{
        
          "id": toDelete.id
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Supprimée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getBankAccount();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la suppression du Compte dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    },

    //////////////////////////////////////////////////////////////////

    addNewSousClassification() {

      this.dialogSousClassification = true;
    },

    closeSousClassification() {

      this.dialogSousClassification = false;

      setTimeout(() => {

        this.editedSousClassification = Object.assign({}, this.defaultSousClassification);
        this.editedSousClassificationIndex = -1;
      }, 300);

      this.$refs.formSousClassification.resetValidation();
    },

    saveSousClassification() {

      if (this.editedSousClassificationIndex > -1) {

        this.saveEditSousClassification(this.editedSousClassification);
      } 
      else {

        this.saveNewSousClassification(this.editedSousClassification);
      }

      this.closeSousClassification();
    },    

    editSousClassification(item) {

      this.editedSousClassificationIndex = this.allSousClassification.indexOf(item);
      this.editedSousClassification = Object.assign({}, item);
      this.dialogSousClassification = true;
    },

    saveNewSousClassification(toSave) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      console.log(toSave);

      var data = {

        "asset":"sous_classification",
        "command":"create",
        "data":{
        
          "name": toSave.name,
          "id_classification": toSave.classification.id
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Sauvegardée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getSousClassification();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la création de la Sous Classification dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeSousClassification();
    },

    saveEditSousClassification(toEdit) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"sous_classification",
        "command":"update",
        "data":{
        
          "id": toEdit.id,
          "name": toEdit.name,
          "id_classification": toEdit.classification.id
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"            
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Modifiée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getSousClassification();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la modification de la Banque dans la Base de Données"),
          (this.snackbarColor = "error");
      });

      this.closeSousClassification();
    },    

    deleteSousClassification(toDelete) {

      var urlCrudAsset = this.$executioEnvironment+"CrudAsset";

      var data = {

        "asset":"sous_classification",
        "command":"delete",
        "data":{
        
          "id": toDelete.id
        }
      };

      axios.post(urlCrudAsset, data, {

        headers: {
          Accept: "application/json",
          "Content-type": "application/json"
        }
      })
      .then(response=>{

        console.log(response);

        (this.snackbar = true),
          (this.snackbarText = "Ligne Supprimée dans la Base de Données"),
            (this.snackbarColor = "success");

          this.getSousClassification();
      })
      .catch(error => {

        console.log(error);

        (this.snackbar = true),
          (this.snackbarText =
            "Erreur lors de la suppression de la Sous Classification dans la Base de Données"),
          (this.snackbarColor = "error");
      });
    }
  }
};
</script>