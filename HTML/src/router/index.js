import Router from 'vue-router'
import home from '../components/home.vue'
import login from '../components/login.vue'
import searchEncaissement from '../components/searchEncaissement.vue'
import searchDecaissement from '../components/searchDecaissement.vue'
import writEncaissement from '../components/writeEncaissement.vue'
import writeDecaissement from '../components/writeDecaissement.vue'
import budget from '../components/budget.vue'
import rapprochement from '../components/rapprochement.vue'
import reportingExecution from '../components/reportingExecution.vue'
import reportingPosition from '../components/reportingPosition.vue'
import dataManagement from '../components/dataManagement.vue'

export default new Router({
  routes: [
    {
      path: '/',
      name: '',
      component: home
    },
    {
      path: '/home',
      name: 'home',
      component: home
    },
    {
      path: '/login',
      name: 'login',
      component: login
    },
    {
      path: '/searchEncaissement',
      name: 'searchEncaissement',
      component: searchEncaissement
    },
    {
      path: '/searchDecaissement',
      name: 'searchDecaissement',
      component: searchDecaissement
    },
    {
      path: '/writeEncaissement',
      name: 'writeEncaissement',
      component: writEncaissement
    },
    {
      path: '/writeDecaissement',
      name: 'writeDecaissements',
      component: writeDecaissement
    },
    {
      path: '/budget',
      name: 'budget',
      component: budget
    },
    {
      path: '/rapprochement',
      name: 'rapprochement',
      component: rapprochement
    },
    {
      path: '/reportingExecution',
      name: 'reportingExecution',
      component: reportingExecution
    },
    {
      path: '/reportingPosition',
      name: 'reportingPosition',
      component: reportingPosition
    },
    {
      path: '/dataManagement',
      name: 'dataManagement',
      component: dataManagement
    }
  ]
})