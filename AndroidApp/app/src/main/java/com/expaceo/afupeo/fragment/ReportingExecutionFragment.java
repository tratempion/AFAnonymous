package com.expaceo.afupeo.fragment;

import android.content.Context;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.StrictMode;
import android.support.v4.app.Fragment;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.expaceo.afupeo.R;
import com.expaceo.afupeo.api.CallApiSync;
import com.expaceo.afupeo.formatter.MyValueFormatter;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.github.mikephil.charting.charts.BarChart;
import com.github.mikephil.charting.components.Legend;
import com.github.mikephil.charting.components.XAxis;
import com.github.mikephil.charting.components.YAxis;
import com.github.mikephil.charting.data.BarData;
import com.github.mikephil.charting.data.BarDataSet;
import com.github.mikephil.charting.data.BarEntry;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.formatter.IndexAxisValueFormatter;
import com.github.mikephil.charting.highlight.Highlight;
import com.github.mikephil.charting.interfaces.datasets.IBarDataSet;
import com.github.mikephil.charting.listener.OnChartValueSelectedListener;
import com.github.mikephil.charting.utils.ColorTemplate;

import java.net.URL;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Map;

public class ReportingExecutionFragment extends Fragment {

    private OnChartValueSelectedListener mListenerChartValue;

    private Spinner mSpinner;
    private List<String> mSpinnerData;

    private BarChart chart;

    private TextView textViewYear;

    private Float bilanAnnuel;

    private float maxValue;

    public ReportingExecutionFragment() {

        // Required empty public constructor
    }

    public static ReportingExecutionFragment newInstance() {

        ReportingExecutionFragment fragment = new ReportingExecutionFragment();

        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        View view = inflater.inflate(R.layout.fragment_reporting_execution, container, false);

        super.onCreate(savedInstanceState);

        Calendar now = Calendar.getInstance();

        Integer currentYear = now.get(Calendar.YEAR);

        textViewYear = view.findViewById(R.id.textViewYear);

        initSpinner(view, currentYear);

        Map<String, Map<String, Map<String, Object>>> data = loadData(currentYear);

        chart = view.findViewById(R.id.chart1);
        chart.setOnChartValueSelectedListener(mListenerChartValue);

        chart.getDescription().setEnabled(false);

        chart.setMaxVisibleValueCount(17);

        chart.setPinchZoom(false);

        chart.setDrawGridBackground(false);
        chart.setDrawBarShadow(false);
        chart.setDrawValueAboveBar(false);
        chart.setHighlightFullBarEnabled(false);

        setChartData(data);

        setTextViewYear();

        YAxis leftAxis = chart.getAxisLeft();
        leftAxis.setValueFormatter(new MyValueFormatter(""));
        leftAxis.setAxisMinimum(-maxValue);
        chart.getAxisRight().setEnabled(false);

        Legend l = chart.getLegend();

        l.setVerticalAlignment(Legend.LegendVerticalAlignment.TOP);
        l.setHorizontalAlignment(Legend.LegendHorizontalAlignment.RIGHT);
        l.setOrientation(Legend.LegendOrientation.HORIZONTAL);
        l.setDrawInside(false);
        l.setFormSize(8f);
        l.setFormToTextSpace(4f);
        l.setXEntrySpace(6f);

        final ArrayList<String> xLabel = new ArrayList<>();

        for (Map.Entry<String, Map<String, Map<String, Object>>> pair : data.entrySet()) {

            String label = pair.getKey();

            if(label.length()>12){

                label = label.substring(0,12);
                label += "..";
            }

            xLabel.add(label);
        }

        IndexAxisValueFormatter indexAxisValueFormatter = new IndexAxisValueFormatter(xLabel);

        XAxis xAxis = chart.getXAxis();
        xAxis.setPosition(XAxis.XAxisPosition.BOTTOM);
        xAxis.setDrawGridLines(false);

        xAxis.setLabelCount(12);

        xAxis.setLabelRotationAngle(90);
        xAxis.setValueFormatter(indexAxisValueFormatter);

        return view;
    }

    private void initSpinner(View view, Integer currentYear){

        final List<String> myArraySpinner = new ArrayList<>();

        for(int i = 2000; i <= currentYear+10; i++) {

            myArraySpinner.add("" + i);
        }

        mSpinnerData = myArraySpinner;

        Spinner mySpinner = view.findViewById(R.id.spinner);

        mSpinner = mySpinner;

        ArrayAdapter<String> spinnerArrayAdapter = new ArrayAdapter<>(view.getContext(), android.R.layout.simple_spinner_item, myArraySpinner);
        spinnerArrayAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        mySpinner.setAdapter(spinnerArrayAdapter);

        mSpinner.setSelection(currentYear-2000);

        mySpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {

            @Override
            public void onItemSelected(AdapterView<?> parentView, View selectedItemView, int myPosition, long myID) {

                Integer selectedYear = Integer.parseInt(myArraySpinner.get(myPosition));

                Map<String, Map<String, Map<String, Object>>> data = loadData(selectedYear);

                setChartData(data);

                setTextViewYear();
            }

            @Override
            public void onNothingSelected(AdapterView<?> parentView) {
                // your code here
            }
        });
    }

    private Map<String, Map<String, Map<String, Object>>> loadData(Integer year){

        if (Build.VERSION.SDK_INT > 9) {

            StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
            StrictMode.setThreadPolicy(policy);
        }

        ObjectMapper mapper = new ObjectMapper();

        Map<String, Map<String, Map<String, Object>>> response = null;

        try {

            URL url = new URL("https://afupeo.azurewebsites.net/api/GetBudgetBilan?annee="+year);

            CallApiSync callApiSync = new CallApiSync();

            callApiSync.execute(url).get();

            response = (Map<String, Map<String, Map<String, Object>>>) mapper.readValue(callApiSync.getResult(), Object.class);

        }
        catch (Exception e){

            e.printStackTrace();
        }

        return response;
    }

    private void setChartData(Map<String, Map<String, Map<String, Object>>> data){

        ArrayList<BarEntry> values = new ArrayList<>();

        int i = 0;

        bilanAnnuel = Float.valueOf(0);

        for (Map.Entry<String, Map<String, Map<String, Object>>> classification : data.entrySet()) {

            Float classificationObjectif = Float.valueOf(0);

            Float classificationDepenses = Float.valueOf(0);

            String labelClassification = classification.getKey();

            Map<String, Map<String, Object>> sousClassificationMap = data.get(labelClassification);

            for (Map.Entry<String, Map<String, Object>> sousClassification : sousClassificationMap.entrySet()) {

                Map<String, Object> sousClassificationValue = sousClassification.getValue();

                classificationObjectif += Float.parseFloat((String)sousClassificationValue.get("objectif"));

                List<Map<String, String>> decaissementList = (List<Map<String, String>>) sousClassificationValue.get("decaissements");

                for(Map<String, String> decaissement : decaissementList){

                    classificationDepenses += Float.parseFloat(decaissement.get("montant_ttc"));
                }
            }

            if(classificationObjectif > maxValue){

                maxValue = classificationObjectif;
            }

            if(classificationDepenses > maxValue){

                maxValue = classificationDepenses;
            }

            bilanAnnuel += classificationObjectif;
            bilanAnnuel -= classificationDepenses;

            classificationDepenses = -(classificationDepenses);

            values.add(new BarEntry(i, new float[]{classificationObjectif, classificationDepenses}, getResources().getDrawable(R.drawable.star)));

            i++;
        }

        maxValue = maxValue * (float) 1.01;

        BarDataSet dataSet;

        if (chart.getData() != null && chart.getData().getDataSetCount() > 0) {

            dataSet = (BarDataSet) chart.getData().getDataSetByIndex(0);
            dataSet.setValues(values);
            chart.getData().notifyDataChanged();
            chart.notifyDataSetChanged();
        }
        else {

            dataSet = new BarDataSet(values, "");
            dataSet.setDrawIcons(false);
            dataSet.setColors(getColors());
            dataSet.setStackLabels(new String[]{"Budget", "Depense"});

            ArrayList<IBarDataSet> dataSets = new ArrayList<>();
            dataSets.add(dataSet);

            BarData barData = new BarData(dataSets);

            barData.setDrawValues(false);

            chart.setData(barData);
        }

        chart.setFitBars(true);
        chart.invalidate();
    }

    private void setTextViewYear(){

        String bilanAnnuelString = "Bilan Annuel =";

        if(bilanAnnuel>0) {

            bilanAnnuelString += " + " + bilanAnnuel;

            textViewYear.setBackgroundColor(getColors()[0]);
        }
        else if(bilanAnnuel<0){

            String ba = bilanAnnuel.toString();

            bilanAnnuelString += " - " + ba.substring(1);

            textViewYear.setBackgroundColor(getColors()[1]);
        }
        else if(bilanAnnuel==0){

            bilanAnnuelString += " " + bilanAnnuel;

            textViewYear.setBackgroundColor(-12303292);
        }

        textViewYear.setText(bilanAnnuelString);
    }

    @Override
    public void onAttach(final Context context) {

        super.onAttach(context);

        if (context instanceof OnFragmentInteractionListener) {

            mListenerChartValue = new OnChartValueSelectedListener() {

                @Override
                public void onValueSelected(Entry e, Highlight h) {

                    BarEntry entry = (BarEntry) e;

                    String value = "" + entry.getYVals()[h.getStackIndex()];

                    Toast toast = Toast.makeText(context, value, Toast.LENGTH_SHORT);
                    toast.setGravity(Gravity.TOP| Gravity.CENTER_HORIZONTAL, 0, 117);
                    toast.show();
                }

                @Override
                public void onNothingSelected() {

                }
            };
        }
        else {

            throw new RuntimeException(context.toString() + " must implement OnFragmentInteractionListener");
        }
    }

    @Override
    public void onDetach() {

        super.onDetach();

        mListenerChartValue = null;
    }

    private int[] getColors() {

        int[] materialColors = new int[3];

        System.arraycopy(ColorTemplate.MATERIAL_COLORS, 0, materialColors, 0, 3);

        int[] colors = new int[2];

        colors[0] = materialColors[0];
        colors[1] = materialColors[2];

        return colors;
    }

    public interface OnFragmentInteractionListener {

        void onFragmentInteraction(Uri uri);
    }
}