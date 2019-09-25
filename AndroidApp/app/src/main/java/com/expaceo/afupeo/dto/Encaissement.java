package com.expaceo.afupeo.dto;

public class Encaissement {

    private String id;
    private String societe;
    private String compte;
    private String banque;
    private Double total_ht;
    private Double total_ttc;
    private Double total_tva;
    private String date_reglement;
    private String mode_paiement;
    private Double transfert_compte;
    private String date_ajout;

    public Encaissement() {

        this.id = "";
        this.societe = "";
        this.compte = "";
        this.banque = "";
        this.total_ht = 0.0;
        this.total_ttc = 0.0;
        this.total_tva = 0.0;
        this.date_reglement = "";
        this.mode_paiement = "";
        this.transfert_compte = 0.0;
        this.date_ajout = "";
    }

    public String getId() {

        return id;
    }

    public void setId(String id) {

        this.id = id;
    }

    public String getSociete() {

        return societe;
    }

    public void setSociete(String societe) {

        this.societe = societe;
    }

    public String getCompte() {

        return compte;
    }

    public void setCompte(String compte) {

        this.compte = compte;
    }

    public String getBanque() {
        return banque;
    }

    public void setBanque(String banque) {

        this.banque = banque;
    }

    public Double getTotal_ht() {

        return total_ht;
    }

    public void setTotal_ht(Double total_ht) {

        this.total_ht = total_ht;
    }

    public Double getTotal_ttc() {

        return total_ttc;
    }

    public void setTotal_ttc(Double total_ttc) {

        this.total_ttc = total_ttc;
    }

    public Double getTotal_tva() {

        return total_tva;
    }

    public void setTotal_tva(Double total_tva) {

        this.total_tva = total_tva;
    }

    public String getDate_reglement() {

        return date_reglement;
    }

    public void setDate_reglement(String date_reglement) {

        this.date_reglement = date_reglement;
    }

    public String getMode_paiement() {

        return mode_paiement;
    }

    public void setMode_paiement(String mode_paiement) {

        this.mode_paiement = mode_paiement;
    }

    public Double getTransfert_compte() {

        return transfert_compte;
    }

    public void setTransfert_compte(Double transfert_compte) {

        this.transfert_compte = transfert_compte;
    }

    public String getDate_ajout() {

        return date_ajout;
    }

    public void setDate_ajout(String date_ajout) {

        this.date_ajout = date_ajout;
    }
}