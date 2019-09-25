<template>
  
  <div>
  
    <v-app>

    <v-content>

      <v-container fluid fill-height>
        
        <v-layout align-center justify-center row fill-height>

          <v-flex xs12 sm8 md4>

            <v-card class="elevation-12">

              <v-toolbar dark color="primary">
                <v-toolbar-title>Rentrez vos identifiants</v-toolbar-title>
              </v-toolbar>

              <v-card-text>
                <v-form>
                  <v-text-field prepend-icon="person" name="login" label="Identifiant" type="email"></v-text-field>
                  <v-text-field prepend-icon="vpn_key" name="password" label="Mot de Passe" id="password" type="password"></v-text-field>
                </v-form>
              </v-card-text>

              <v-card-actions>
                <v-btn block large color="primary">Connexion</v-btn>
              </v-card-actions>

            </v-card>

          </v-flex>

        </v-layout>    
      
      </v-container>

    </v-content>

    </v-app>
  
  </div>

</template>

<script>
export default {
  name: "login",
  data() {
    return {
      msg: "login"
    };
  }
};
</script>

<style scoped>
</style>
