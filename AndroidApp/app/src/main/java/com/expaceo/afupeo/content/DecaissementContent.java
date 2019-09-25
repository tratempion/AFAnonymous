package com.expaceo.afupeo.content;

import android.os.Build;
import android.os.StrictMode;

import com.expaceo.afupeo.api.CallApi;
import com.expaceo.afupeo.dto.Decaissement;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class DecaissementContent {

    public static final List<Decaissement> ITEMS = new ArrayList<>();

    public static final Map<String, Decaissement> ITEM_MAP = new HashMap<>();

    static {

        loadDecaissement();
    }

    private static void loadDecaissement() {

        if (Build.VERSION.SDK_INT > 9) {

            StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
            StrictMode.setThreadPolicy(policy);
        }

        CallApi callApi = new CallApi();

        callApi.setUrl("https://afupeo-staging.azurewebsites.net/api/GetValidatedDecaissements");

        ObjectMapper mapper = new ObjectMapper();

        ArrayList<Map<String, Object>> response = null;

        try {

            response = (ArrayList<Map<String, Object>>) mapper.readValue(callApi.doInBackground(), Object.class);
        }
            catch (Exception e) {

            e.printStackTrace();
        }

        for (Map<String, Object> oneObject : response) {

            Decaissement decaissement = new Decaissement();

            if(null != oneObject.get("operation")){

                decaissement.setOperation((String) oneObject.get("operation"));
            }

            if(null != oneObject.get("compte")){

                decaissement.setCompte((String) oneObject.get("compte"));
            }

            if(null != oneObject.get("banque")){

                decaissement.setBanque((String) oneObject.get("banque"));
            }

            if(null != oneObject.get("mode_paiement")){

                decaissement.setMode_paiement((String) oneObject.get("mode_paiement"));
            }

            if(null != oneObject.get("date_compta")){

                decaissement.setDate_compta((String) oneObject.get("date_compta"));
            }

            if(null != oneObject.get("date_operation")){

                decaissement.setDate_operation((String) oneObject.get("date_operation"));
            }

            if(null != oneObject.get("date_valeur")){

                decaissement.setDate_valeur((String) oneObject.get("date_valeur"));
            }

            if(null != oneObject.get("montant_ttc")){

                decaissement.setMontant_ttc((Double) oneObject.get("montant_ttc"));
            }

            if(null != oneObject.get("tva_deductible")){

                decaissement.setTva_deductible((Double) oneObject.get("tva_deductible"));
            }

            if(null != oneObject.get("montant_ht")){

                decaissement.setMontant_ht((Double) oneObject.get("montant_ht"));
            }

            if(null != oneObject.get("versement_tva")){

                decaissement.setVersement_tva((Boolean) oneObject.get("versement_tva"));
            }

            if(null != oneObject.get("sous_classification")){

                decaissement.setSous_classification((String) oneObject.get("sous_classification"));
            }

            if(null != oneObject.get("classification")){

                decaissement.setClassification((String) oneObject.get("classification"));
            }

            if(null != oneObject.get("details")){

                decaissement.setDetails((String) oneObject.get("details"));
            }

            if(null != oneObject.get("facture_verifie")){

                decaissement.setFacture_verifie((Boolean) oneObject.get("facture_verifie"));
            }

            if(null != oneObject.get("mois_valeur")){

                decaissement.setMois_valeur((Integer) oneObject.get("mois_valeur"));
            }

            if(null != oneObject.get("transfert_compte")){

                decaissement.setTransfert_compte((String) oneObject.get("transfert_compte"));
            }

            if(null != oneObject.get("date_ajout")){

                decaissement.setDate_ajout((String) oneObject.get("date_ajout"));
            }

            addDecaissement(decaissement);
        }
    }

    private static void addDecaissement(Decaissement decaissement) {

        ITEMS.add(decaissement);

        ITEM_MAP.put(decaissement.getId(), decaissement);
    }
}