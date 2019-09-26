<template>

  <div>
  
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,500,700,900|Material+Icons" rel="stylesheet">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no, minimal-ui">

    <v-app v-resize="onResize">
    
      <v-toolbar dark color="primary" fixed v-if="!isMobile">
      
        <v-toolbar-items>
        
          <router-link class="white--text" :to="'/home'" tag="button">
            <v-btn block style="height:100%" flat>
              <v-toolbar-title>
                AFUPEO
              </v-toolbar-title>
            </v-btn>
          </router-link>

          <v-menu open-on-hover full-width offset-y transition="scale-transition">
            <template v-slot:activator="{ on }" > 
                <v-btn flat class="white--text"   v-on="on">
                  Recherche<v-icon dark>arrow_drop_down</v-icon>
                </v-btn>
            </template>
            <v-list style="background: #E1F5FE">
              <v-list-tile>
                <router-link :to="'/searchEncaissement'" tag="button">
                  <v-btn flat>Encaissement</v-btn>
                </router-link>  
              </v-list-tile>
              <v-list-tile>
                <router-link :to="'/searchDecaissement'" tag="button">
                  <v-btn flat>Decaissement</v-btn>
                </router-link>     
              </v-list-tile>
            </v-list>
          </v-menu>
        
          <v-menu open-on-hover full-width offset-y transition="scale-transition">
            <template v-slot:activator="{ on }">
                <v-btn flat class="white--text"   v-on="on">
                  Saisie<v-icon dark>arrow_drop_down</v-icon>
                </v-btn>
            </template>
            <v-list style="background: #E1F5FE">
              <v-list-tile>
                <router-link :to="'/writeEncaissement'" tag="button">
                  <v-btn flat>Encaissement</v-btn>
                </router-link>
              </v-list-tile>
              <v-list-tile>
                <router-link :to="'/writeDecaissement'" tag="button">
                  <v-btn flat>Decaissement</v-btn>
                </router-link>      
              </v-list-tile>
            </v-list>
          </v-menu>
          
          <router-link class="white--text" :to="'/budget'" tag="button">
            <v-btn style="height:100%" flat>Budget</v-btn>
          </router-link>
          
          <router-link class="white--text" :to="'/rapprochement'" tag="button">
            <v-btn style="height:100%" flat>Rapprochement</v-btn>
          </router-link>         
        
          <v-menu open-on-hover full-width offset-y transition="scale-transition">
            <template v-slot:activator="{ on }">
                <v-btn flat class="white--text"   v-on="on">
                  Reporting<v-icon dark>arrow_drop_down</v-icon>
                </v-btn>
            </template>
            <v-list style="background: #E1F5FE">
              <v-list-tile>
                <router-link :to="'/reportingPosition'" tag="button">
                  <v-btn flat>Evolution Position</v-btn>
                </router-link>
              </v-list-tile>
              <v-list-tile>
                <router-link :to="'/reportingExecution'" tag="button">
                  <v-btn flat>Suivi d'Execution</v-btn>
                </router-link>      
              </v-list-tile>
            </v-list>
          </v-menu>          
          
          <router-link class="white--text" :to="'/admin'" tag="button">
            <v-btn style="height:100%" flat>Administration</v-btn>
          </router-link>
          
        </v-toolbar-items>

        <v-spacer></v-spacer>
        
        <v-toolbar-items>
  
          <v-menu open-on-hover full-width offset-y transition="scale-transition">
            <template v-slot:activator="{ on }">
                <v-btn flat class="white--text" color="primary" v-on="on" style="text-transform: none !important;">
                  {{connectedUser}}<v-icon dark>arrow_drop_down</v-icon>
                </v-btn>
            </template>
            <v-list style="background: #E1F5FE">
              <v-list-tile>
                  <v-btn flat :href="redirectURL">
                    Se Déconnecter
                  </v-btn>
              </v-list-tile>
            </v-list>
          </v-menu>

        </v-toolbar-items>

      </v-toolbar>

      <v-toolbar dark color="primary" fixed v-else>

        <v-toolbar-side-icon @click.stop="toggleMenu"></v-toolbar-side-icon>

        <router-link class="white--text" :to="'/home'" tag="button">
          <v-btn block style="height:100%" flat>
            <v-toolbar-title>
              AFUPEO
            </v-toolbar-title>
          </v-btn>
        </router-link>

        <v-spacer></v-spacer>

        <v-menu class="hidden-md-and-up">
        <v-toolbar-side-icon slot="activator"><v-icon>account_circle</v-icon></v-toolbar-side-icon>
        <v-list>
          <v-list-tile>
            <div align-center style="color: #2196F3; width:100%">
              <strong>{{connectedUser}}</strong>
            </div>            
          </v-list-tile>
          <v-list-tile>
            <router-link :to="'/login'" tag="button">
                <v-btn flat>Se Déconnecter</v-btn>
            </router-link>
          </v-list-tile>   
        </v-list>
      </v-menu>

      </v-toolbar>

      <v-navigation-drawer dark app fixed v-model="showMenu">

        <v-list>        
          
          <v-list-tile :to="'/home'" tag="button">
            <v-list-tile-action>
              <v-icon>account_balance</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title>
                Accueil
              </v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>

          <v-list-group no-action prepend-icon="search">
            <template v-slot:activator>
              <v-list-tile>
                <v-list-tile-content>
                  <v-list-tile-title>Recherche</v-list-tile-title>
                </v-list-tile-content>
              </v-list-tile>
            </template>
            <v-list-tile :to="'/searchEncaissement'">
              Encaissement
            </v-list-tile>
            <v-list-tile :to="'/searchDecaissement'">
              Decaissement
            </v-list-tile>
          </v-list-group>

          <v-list-group no-action prepend-icon="create">
            <template v-slot:activator>
              <v-list-tile>
                <v-list-tile-content>
                  <v-list-tile-title>Saisie</v-list-tile-title>
                </v-list-tile-content>
              </v-list-tile>
            </template>
            <v-list-tile :to="'/writeEncaissement'">
              Encaissement
            </v-list-tile>
            <v-list-tile :to="'/writeDecaissement'">
              Decaissement
            </v-list-tile>
          </v-list-group>
          
          <v-list-tile :to="'/budget'" tag="button">
            <v-list-tile-action>
              <v-icon>attach_money</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title>
                Budget
              </v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
          
          <v-list-tile :to="'/rapprochement'" tag="button">
            <v-list-tile-action>
              <v-icon>compare_arrows</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title>
                Rapprochement
              </v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>         

          <v-list-group no-action prepend-icon="create">
            <template v-slot:activator>
              <v-list-tile>
                <v-list-tile-content>
                  <v-list-tile-title>Reporting</v-list-tile-title>
                </v-list-tile-content>
              </v-list-tile>
            </template>
            <v-list-tile :to="'/reportingPosition'">
              Evolution Position
            </v-list-tile>
            <v-list-tile :to="'/reportingExecution'">
              Suivi d'Execution
            </v-list-tile>
          </v-list-group>     
          
          <v-list-tile :to="'/admin'" tag="button">
            <v-list-tile-action>
              <v-icon>build</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title>
                Administration
              </v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>    

        </v-list>

      </v-navigation-drawer>

      <router-view style ="padding-top:63px"></router-view>

    </v-app>

  </div>

</template>

<script>
import axios from 'axios'
export default {


  data() {

    return {

      connectedUser: "",
      selected: "home",
      isMobile: false,
      showMenu: false,
      redirectURL:""
    };
  },

  created:function(){

    var microsoftLogin = "https://login.microsoftonline.com/4f8d29ed-f993-4bfe-909a-a14f917c33a0/oauth2/authorize?response_type=code+id_token&redirect_uri=https%3A%2F%2Fafupeo-staging.azurewebsites.net%2F.auth%2Flogin%2Faad%2Fcallback&client_id=f86bc9fa-75ca-4fd1-89e5-d8bc0ab16227&scope=openid+profile+email&response_mode=form_post&nonce=eb8c12820d054c65ae9d4ccb805a4d31_20190629180909&state=redir%3D%252F";
    
    this.redirectURL = "https://login.microsoftonline.com/4f8d29ed-f993-4bfe-909a-a14f917c33a0/oauth2/logout";
    
    //this.redirectURL=("https://login.microsoftonline.com/4f8d29ed-f993-4bfe-909a-a14f917c33a0/oauth2/logout?post_logout_redirect_uri="+this.$executioEnvironment).split('/api/')[0];
    
    let that=this;
    
    this.selected=this.$route.name;
    
    var url=this.$executioEnvironment+"GetUsername";
    
    axios.get(url).then(
      
      function(response){

        var firstName = response.data.split('.')[0];
        var lastName = response.data.split('.')[1].split('@')[0];

        firstName = firstName.toLowerCase();

        firstName = firstName.charAt(0).toUpperCase() + firstName.substring(1, firstName.length);
    
        lastName = lastName.toUpperCase();

        that.connectedUser = firstName + " " + lastName;
      }
    )
  },

  methods: {

    onResize() {

      if (window.innerWidth < 769) {
        
        this.isMobile = true;
      }
      else {
        
        this.isMobile = false;

        this.showMenu = false;
      }
    },

    toggleMenu() {      

      this.showMenu = !this.showMenu;      
    }
  }
};
</script>

<style>

#app {
  
  font-size: 18px;
  font-family: "Roboto", sans-serif;
  text-align: center;
}

.mobile {
  
  color: #333;
}
    
@media screen and (max-width: 768px) {
        
  .mobile table.v-table tr {
  
    max-width: 100%;
    position: relative;
    display: block;
  }
    
  .mobile table.v-table tr:nth-child(odd) {

    border-left: 6px solid gold;
  }
    
  .mobile table.v-table tr:nth-child(even) {
    
    border-left: 6px solid cyan;
  }

  .mobile table.v-table tr td {
  
    display: flex;
    width: 100%;
    border-bottom: 1px solid #f5f5f5;
    height: auto;
    padding: 10px;
  }

  .mobile table.v-table tr td ul li:before {
  
    content: attr(data-label);
    padding-right: .5em;
    text-align: left;
    display: block;
    color: #999;
  }
    
  .v-datatable__actions__select {

    width: 50%;
    margin: 0px;
    justify-content: flex-start;
  }
    
  .mobile .theme--light.v-table tbody tr:hover:not(.v-datatable__expand-row) {
  
    background: transparent;
  }
}
  
.flex-content {
 
  padding: 0;
  margin: 0;
  list-style: none;
  display: flex;
  flex-wrap: wrap;
  width: 100%;
}

.flex-item {
  
  padding: 5px;
  width: 50%;
  height: 40px;
	font-weight: bold;
}

</style>