package com.expaceo.afupeo.content;

import android.os.Build;
import android.os.StrictMode;

import com.expaceo.afupeo.api.CallApi;
import com.expaceo.afupeo.dto.Encaissement;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class EncaissementContent {

    public static final List<Encaissement> ITEMS = new ArrayList<Encaissement>();

    public static final Map<String, Encaissement> ITEM_MAP = new HashMap<String, Encaissement>();

    static {

        loadEncaissement();
    }

    private static void loadEncaissement() {

        if (Build.VERSION.SDK_INT > 9) {

            StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
            StrictMode.setThreadPolicy(policy);
        }

        CallApi callApi = new CallApi();

        callApi.setUrl("https://afupeo-staging.azurewebsites.net/api/GetAllEncaissement");

        ObjectMapper mapper = new ObjectMapper();

        ArrayList<Map<String, Object>> response = null;

        try {

            response = (ArrayList<Map<String, Object>>) mapper.readValue(callApi.doInBackground(), Object.class);
        }
            catch (Exception e) {

            e.printStackTrace();
        }

        for (Map<String, Object> oneObject : response) {

            Encaissement encaissement = new Encaissement();

            if(null != oneObject.get("societe")){

                encaissement.setSociete((String) oneObject.get("societe"));
            }

            if(null != oneObject.get("compte")){

                encaissement.setCompte((String) oneObject.get("compte"));
            }

            if(null != oneObject.get("banque")){

                encaissement.setBanque((String) oneObject.get("banque"));
            }

            if(null != oneObject.get("date_reglement")){

                encaissement.setDate_reglement((String) oneObject.get("date_reglement"));
            }

            if(null != oneObject.get("mode_paiement")){

                encaissement.setMode_paiement((String) oneObject.get("mode_paiement"));
            }

            if(null != oneObject.get("date_ajout")){

                encaissement.setDate_ajout((String) oneObject.get("date_ajout"));
            }

            if(null != oneObject.get("total_ht")){

                encaissement.setTotal_ht((Double) oneObject.get("total_ht"));
            }

            if(null != oneObject.get("total_ttc")){

                encaissement.setTotal_ttc((Double) oneObject.get("total_ttc"));
            }

            if(null != oneObject.get("total_tva")){

                encaissement.setTotal_tva((Double) oneObject.get("total_tva"));
            }

            if(null != oneObject.get("transfert_compte")){

                encaissement.setTransfert_compte((Double) oneObject.get("transfert_compte"));
            }

            addEncaissement(encaissement);
        }
    }

    private static void addEncaissement(Encaissement encaissement) {

        ITEMS.add(encaissement);

        ITEM_MAP.put(encaissement.getId(), encaissement);
    }
}