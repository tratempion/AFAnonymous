import Vue from 'vue';
import App from './App.vue';
import Router from 'vue-router';
import router from './router';
import MultiFiltersPlugin from './plugins/MultiFilters'

import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'
import BootstrapVue from 'bootstrap-vue/dist/bootstrap-vue.esm';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';

import en from 'vuetify/es5/locale/en'
import fr from 'vuetify/es5/locale/fr'
Vue.use(BootstrapVue);

Vue.use(Router);

// Prod
//Vue.prototype.$executioEnvironment = 'https://afupeo.azurewebsites.net/api/';

// Staging
//Vue.prototype.$executioEnvironment = 'https://afupeo-staging.azurewebsites.net/api/';
 
// Localhost
Vue.prototype.$executioEnvironment = 'http://localhost:7071/api/';

Vue.use(Vuetify, {

  lang: {
    
    locales: { en, fr },
    current: 'fr'
  }
});
Vue.use(MultiFiltersPlugin);

new Vue({

  el: '#app',
  render: h => h(App),
  router: router
});