package com.expaceo.afupeo.api;

import android.os.AsyncTask;
import android.util.Log;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class CallApi extends AsyncTask<Void, Void, String> {

    private String url;

    public String doInBackground(Void... urls) {

        try {

            URL url = new URL(getUrl());

            HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();

            try {

                BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));

                StringBuilder stringBuilder = new StringBuilder();

                String line;

                while ((line = bufferedReader.readLine()) != null) {

                    stringBuilder.append(line).append("\n");
                }

                bufferedReader.close();

                return stringBuilder.toString();

            }
            finally{

                urlConnection.disconnect();
            }
        }
        catch(Exception e) {

            Log.e("ERROR", e.getMessage(), e);

            return null;
        }
    }

    public String getUrl() {

        return url;
    }

    public void setUrl(String url) {

        this.url = url;
    }
}