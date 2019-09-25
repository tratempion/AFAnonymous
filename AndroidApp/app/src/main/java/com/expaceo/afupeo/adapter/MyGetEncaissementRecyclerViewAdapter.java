package com.expaceo.afupeo.adapter;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Filter;
import android.widget.Filterable;
import android.widget.TextView;

import com.expaceo.afupeo.fragment.GetEncaissementFragment.OnListFragmentInteractionListener;
import com.expaceo.afupeo.R;
import com.expaceo.afupeo.dto.Encaissement;

import java.util.ArrayList;
import java.util.List;

public class MyGetEncaissementRecyclerViewAdapter extends RecyclerView.Adapter<MyGetEncaissementRecyclerViewAdapter.ViewHolder> implements Filterable {

    private List<Encaissement> mValues;
    private final List<Encaissement> mFullValues;
    private final OnListFragmentInteractionListener mListener;

    public MyGetEncaissementRecyclerViewAdapter(List<Encaissement> items, OnListFragmentInteractionListener listener) {

        mValues = items;
        mFullValues = new ArrayList<>(items);

        mListener = listener;
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.fragment_getencaissement, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(final ViewHolder holder, int position) {

        holder.mItem = mValues.get(position);

        Encaissement encaissement = mValues.get(position);

        holder.mSocieteView.setText(encaissement.getSociete());
        holder.mCompteView.setText(encaissement.getCompte());

        holder.mBanqueView.setText(encaissement.getBanque());
        holder.mTotalHTView.setText(""+encaissement.getTotal_ht());

        holder.mTotalTTCView.setText(""+encaissement.getTotal_ttc());
        holder.mTotalTVAView.setText(""+encaissement.getTotal_tva());

        holder.mDateReglementView.setText(encaissement.getDate_reglement());
        holder.mModePaiementView.setText(encaissement.getMode_paiement());

        holder.mTransfertCompteView.setText(""+encaissement.getTransfert_compte());
        holder.mDateAjoutView.setText(encaissement.getDate_ajout());

        holder.mView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                if (null != mListener) {

                    mListener.onListFragmentInteraction(holder.mItem);
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        return mValues.size();
    }

    @Override
    public Filter getFilter() {
        return customFilter;
    }

    private Filter customFilter = new Filter() {
        @Override
        protected FilterResults performFiltering(CharSequence constraint) {

            List<Encaissement> filteredList = new ArrayList<>();

            if(null == constraint || constraint.length() == 0){

                filteredList.addAll(mFullValues);
            }
            else {

                String searchedPattern = constraint.toString().toLowerCase().trim();

                for (Encaissement encaissement : mFullValues) {

                    if(encaissement.getSociete().toLowerCase().contains(searchedPattern)){

                        filteredList.add(encaissement);
                    }
                    else if(encaissement.getCompte().toLowerCase().contains(searchedPattern)){

                        filteredList.add(encaissement);
                    }
                    else if(encaissement.getBanque().toLowerCase().contains(searchedPattern)){

                        filteredList.add(encaissement);
                    }
                    else if(encaissement.getTotal_ht().toString().toLowerCase().contains(searchedPattern)){

                        filteredList.add(encaissement);
                    }
                    else if(encaissement.getTotal_ttc().toString().toLowerCase().contains(searchedPattern)){

                        filteredList.add(encaissement);
                    }
                    else if(encaissement.getTotal_tva().toString().toLowerCase().contains(searchedPattern)){

                        filteredList.add(encaissement);
                    }
                    else if(encaissement.getDate_reglement().toLowerCase().contains(searchedPattern)){

                        filteredList.add(encaissement);
                    }
                    else if(encaissement.getMode_paiement().toLowerCase().contains(searchedPattern)){

                        filteredList.add(encaissement);
                    }
                    else if(encaissement.getTransfert_compte().toString().toLowerCase().contains(searchedPattern)){

                        filteredList.add(encaissement);
                    }
                    else if(encaissement.getDate_ajout().toLowerCase().contains(searchedPattern)){

                        filteredList.add(encaissement);
                    }
                }
            }

            FilterResults filterResults = new FilterResults();

            filterResults.values = filteredList;

            return filterResults;
        }

        @Override
        protected void publishResults(CharSequence constraint, FilterResults results) {

            mValues.clear();
            mValues.addAll((List) results.values);
            notifyDataSetChanged();
        }
    };

    public class ViewHolder extends RecyclerView.ViewHolder {

        public final View mView;

        public final TextView mSocieteView;
        public final TextView mCompteView;
        public final TextView mBanqueView;
        public final TextView mTotalHTView;
        public final TextView mTotalTTCView;
        public final TextView mTotalTVAView;
        public final TextView mDateReglementView;
        public final TextView mModePaiementView;
        public final TextView mTransfertCompteView;
        public final TextView mDateAjoutView;

        public Encaissement mItem;

        public ViewHolder(View view) {

            super(view);

            mView = view;

            mSocieteView = (TextView) view.findViewById(R.id.societe);
            mCompteView = (TextView) view.findViewById(R.id.compte);

            mBanqueView = (TextView) view.findViewById(R.id.banque);
            mTotalHTView = (TextView) view.findViewById(R.id.totalHT);

            mTotalTTCView = (TextView) view.findViewById(R.id.totalTTC);
            mTotalTVAView = (TextView) view.findViewById(R.id.totalTVA);

            mDateReglementView = (TextView) view.findViewById(R.id.dateReglement);
            mModePaiementView = (TextView) view.findViewById(R.id.modePaiement);

            mTransfertCompteView = (TextView) view.findViewById(R.id.transfertCompte);
            mDateAjoutView = (TextView) view.findViewById(R.id.dateAjout);
        }

        @Override
        public String toString() {


            return super.toString();
        }
    }
}