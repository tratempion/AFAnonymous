package com.expaceo.afupeo;

import android.net.Uri;
import android.os.Bundle;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;

import com.expaceo.afupeo.dto.Decaissement;
import com.expaceo.afupeo.dto.Encaissement;
import com.expaceo.afupeo.fragment.GetDecaissementFragment;
import com.expaceo.afupeo.fragment.GetEncaissementFragment;
import com.expaceo.afupeo.fragment.MainFragment;
import com.expaceo.afupeo.fragment.ReportingExecutionFragment;

public class MainActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener,
        GetEncaissementFragment.OnListFragmentInteractionListener,
        GetDecaissementFragment.OnListFragmentInteractionListener,
        ReportingExecutionFragment.OnFragmentInteractionListener {

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);

        getSupportFragmentManager().beginTransaction().replace(R.id.fragment_container, new MainFragment()).commit();
    }

    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {

        int id = item.getItemId();

        return super.onOptionsItemSelected(item);
    }

    @Override
    public boolean onNavigationItemSelected(MenuItem item) {

        int id = item.getItemId();

        if (id == R.id.nav_search_income) {

            getSupportFragmentManager().beginTransaction().replace(R.id.fragment_container, new GetEncaissementFragment()).commit();

        } else if (id == R.id.nav_search_outcome) {

            getSupportFragmentManager().beginTransaction().replace(R.id.fragment_container, new GetDecaissementFragment()).commit();

        } else if (id == R.id.nav_reporting) {

            getSupportFragmentManager().beginTransaction().replace(R.id.fragment_container, new ReportingExecutionFragment()).commit();
        }

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

    @Override
    public void onListFragmentInteraction(Encaissement item) {

    }

    @Override
    public void onListFragmentInteraction(Decaissement item) {

    }

    @Override
    public void onFragmentInteraction(Uri uri) {

    }
}