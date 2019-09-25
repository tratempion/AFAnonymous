package com.expaceo.afupeo.fragment;

import android.content.Context;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;

import com.expaceo.afupeo.R;
import com.expaceo.afupeo.adapter.MyGetDecaissementRecyclerViewAdapter;
import com.expaceo.afupeo.content.DecaissementContent;
import com.expaceo.afupeo.dto.Decaissement;

public class GetDecaissementFragment extends Fragment {

    private static final String ARG_COLUMN_COUNT = "column-count";
    private int mColumnCount = 1;
    private OnListFragmentInteractionListener mListener;
    private MyGetDecaissementRecyclerViewAdapter adapter;
    private EditText filter;

    public GetDecaissementFragment() {

        // Required empty public constructor
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);

        if (getArguments() != null) {

            mColumnCount = getArguments().getInt(ARG_COLUMN_COUNT);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        View view = inflater.inflate(R.layout.fragment_getdecaissement_list, container, false);

        final Context context = view.getContext();

        RecyclerView recyclerView = (RecyclerView) view.findViewById(R.id.listDecaissement);

        if (mColumnCount <= 1) {

            recyclerView.setLayoutManager(new LinearLayoutManager(context));
        }
        else {

            recyclerView.setLayoutManager(new GridLayoutManager(context, mColumnCount));
        }

        adapter = new MyGetDecaissementRecyclerViewAdapter(DecaissementContent.ITEMS, mListener);

        recyclerView.setAdapter(adapter);

        filter = (EditText) view.findViewById(R.id.searchBar);

        filter.addTextChangedListener(new TextWatcher() {

            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

                (GetDecaissementFragment.this).adapter.getFilter().filter(s);
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });

        return view;
    }


    @Override
    public void onAttach(Context context) {

        super.onAttach(context);

        if (context instanceof OnListFragmentInteractionListener) {

            mListener = (OnListFragmentInteractionListener) context;
        }
        else {

            throw new RuntimeException(context.toString() + " must implement OnListFragmentInteractionListener");
        }
    }

    @Override
    public void onDetach() {

        super.onDetach();

        mListener = null;
    }

    public interface OnListFragmentInteractionListener {

        void onListFragmentInteraction(Decaissement item);
    }
}