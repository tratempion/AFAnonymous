package com.expaceo.afupeo.adapter;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Filter;
import android.widget.Filterable;
import android.widget.TextView;

import com.expaceo.afupeo.fragment.GetDecaissementFragment.OnListFragmentInteractionListener;
import com.expaceo.afupeo.R;
import com.expaceo.afupeo.dto.Decaissement;

import java.util.ArrayList;
import java.util.List;

public class MyGetDecaissementRecyclerViewAdapter extends RecyclerView.Adapter<MyGetDecaissementRecyclerViewAdapter.ViewHolder> implements Filterable {

    private List<Decaissement> mValues;
    private final List<Decaissement> mFullValues;
    private final OnListFragmentInteractionListener mListener;

    public MyGetDecaissementRecyclerViewAdapter(List<Decaissement> items, OnListFragmentInteractionListener listener) {

        mValues = items;
        mFullValues = new ArrayList<>(items);

        mListener = listener;
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.fragment_getdecaissement, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(final ViewHolder holder, int position) {

        holder.mItem = mValues.get(position);

        Decaissement decaissement = mValues.get(position);

        holder.mOperationView.setText(decaissement.getOperation());
        holder.mCompteView.setText(decaissement.getCompte());

        holder.mBanqueView.setText(decaissement.getBanque());
        holder.mModePaiementView.setText(decaissement.getMode_paiement());

        holder.mDateComptaView.setText(decaissement.getDate_compta());
        holder.mDateOperationView.setText(decaissement.getDate_operation());

        holder.mDateValeurView.setText(decaissement.getDate_valeur());
        holder.mMontantTTCView.setText(""+decaissement.getMontant_ttc());

        holder.mTVADeductibleView.setText(""+decaissement.getTva_deductible());
        holder.mMontantHTView.setText(""+decaissement.getMontant_ht());

        holder.mVersementTVAView.setText(""+decaissement.getVersement_tva());
        holder.mSousClassificationView.setText(decaissement.getSous_classification());

        holder.mClassificationView.setText(decaissement.getClassification());
        holder.mDetailsView.setText(decaissement.getDetails());

        holder.mFactureVerifieView.setText(""+decaissement.getFacture_verifie());
        holder.mMoisValeurView.setText(""+decaissement.getMois_valeur());

        holder.mTransfertCompteView.setText(decaissement.getTransfert_compte());
        holder.mDateAjoutView.setText(decaissement.getDate_ajout());

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

            List<Decaissement> filteredList = new ArrayList<>();

            if(null == constraint || constraint.length() == 0){

                filteredList.addAll(mFullValues);
            }
            else {

                String searchedPattern = constraint.toString().toLowerCase().trim();

                for (Decaissement decaissement : mFullValues){

                    if(decaissement.getOperation().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getCompte().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getBanque().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getMode_paiement().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getDate_compta().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getDate_operation().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getDate_valeur().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getMontant_ttc().toString().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getTva_deductible().toString().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getMontant_ht().toString().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getVersement_tva().toString().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getSous_classification().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getClassification().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getDetails().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getFacture_verifie().toString().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getMois_valeur().toString().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getTransfert_compte().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
                    }
                    else if(decaissement.getDate_ajout().toLowerCase().contains(searchedPattern)){

                        filteredList.add(decaissement);
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

        public final TextView mOperationView;
        public final TextView mCompteView;
        public final TextView mBanqueView;
        public final TextView mModePaiementView;
        public final TextView mDateComptaView;
        public final TextView mDateOperationView;
        public final TextView mDateValeurView;
        public final TextView mMontantTTCView;
        public final TextView mTVADeductibleView;
        public final TextView mMontantHTView;
        public final TextView mVersementTVAView;
        public final TextView mSousClassificationView;
        public final TextView mClassificationView;
        public final TextView mDetailsView;
        public final TextView mFactureVerifieView;
        public final TextView mMoisValeurView;
        public final TextView mTransfertCompteView;
        public final TextView mDateAjoutView;

        public Decaissement mItem;

        public ViewHolder(View view) {

            super(view);

            mView = view;

            mOperationView = (TextView) view.findViewById(R.id.operation);
            mCompteView = (TextView) view.findViewById(R.id.compte);

            mBanqueView = (TextView) view.findViewById(R.id.banque);
            mModePaiementView = (TextView) view.findViewById(R.id.modePaiement);

            mDateComptaView = (TextView) view.findViewById(R.id.dateCompta);
            mDateOperationView = (TextView) view.findViewById(R.id.dateOperation);

            mDateValeurView = (TextView) view.findViewById(R.id.dateValeur);
            mMontantTTCView = (TextView) view.findViewById(R.id.montantTTC);

            mTVADeductibleView = (TextView) view.findViewById(R.id.tvaDeductible);
            mMontantHTView = (TextView) view.findViewById(R.id.montantHT);

            mVersementTVAView = (TextView) view.findViewById(R.id.versementTva);
            mSousClassificationView = (TextView) view.findViewById(R.id.sousClassification);

            mClassificationView = (TextView) view.findViewById(R.id.classification);
            mDetailsView = (TextView) view.findViewById(R.id.details);

            mFactureVerifieView = (TextView) view.findViewById(R.id.factureVerifie);
            mMoisValeurView = (TextView) view.findViewById(R.id.moisValeur);

            mTransfertCompteView = (TextView) view.findViewById(R.id.transfertCompte);
            mDateAjoutView = (TextView) view.findViewById(R.id.dateAjout);
        }

        @Override
        public String toString() {
            return super.toString();
        }
    }
}