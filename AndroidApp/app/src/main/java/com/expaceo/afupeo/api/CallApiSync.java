package com.expaceo.afupeo.api;

import android.os.AsyncTask;
import android.util.Log;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class CallApiSync extends AsyncTask<URL, Integer, Long> {

    private String result;

    protected Long doInBackground(URL... urls) {

        try {

            URL url = urls[0];

            HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();

            try {

                BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));

                StringBuilder stringBuilder = new StringBuilder();

                String line;

                while ((line = bufferedReader.readLine()) != null) {

                    stringBuilder.append(line).append("\n");
                }

                bufferedReader.close();

                setResult(stringBuilder.toString());

                return new Long(117);

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


    public String getResult() {

        return result;
    }

    public void setResult(String result) {

        this.result = result;
    }
}
