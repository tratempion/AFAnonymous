package com.expaceo.afupeo.dto;

public class Decaissement {

    private String id;
    private String operation;
    private String compte;
    private String banque;
    private String mode_paiement;
    private String date_compta;
    private String date_operation;
    private String date_valeur;
    private Double montant_ttc;
    private Double tva_deductible;
    private Double montant_ht;
    private Boolean versement_tva;
    private String sous_classification;
    private String classification;
    private String details;
    private Boolean facture_verifie;
    private Integer mois_valeur;
    private String transfert_compte;
    private String date_ajout;

    public Decaissement() {

        this.id = "";
        this.operation = "";
        this.compte = "";
        this.banque = "";
        this.mode_paiement = "";
        this.date_compta = "";
        this.date_operation = "";
        this.date_valeur = "";
        this.montant_ttc = 0.0;
        this.tva_deductible = 0.0;
        this.montant_ht = 0.0;
        this.versement_tva = false;
        this.sous_classification = "";
        this.classification = "";
        this.details = "";
        this.facture_verifie = false;
        this.mois_valeur = 0;
        this.transfert_compte = "";
        this.date_ajout = "";
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getOperation() {
        return operation;
    }

    public void setOperation(String operation) {
        this.operation = operation;
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

    public String getMode_paiement() {
        return mode_paiement;
    }

    public void setMode_paiement(String mode_paiement) {
        this.mode_paiement = mode_paiement;
    }

    public String getDate_compta() {
        return date_compta;
    }

    public void setDate_compta(String date_compta) {
        this.date_compta = date_compta;
    }

    public String getDate_operation() {
        return date_operation;
    }

    public void setDate_operation(String date_operation) {
        this.date_operation = date_operation;
    }

    public String getDate_valeur() {
        return date_valeur;
    }

    public void setDate_valeur(String date_valeur) {
        this.date_valeur = date_valeur;
    }

    public Double getMontant_ttc() {
        return montant_ttc;
    }

    public void setMontant_ttc(Double montant_ttc) {
        this.montant_ttc = montant_ttc;
    }

    public Double getTva_deductible() {
        return tva_deductible;
    }

    public void setTva_deductible(Double tva_deductible) {
        this.tva_deductible = tva_deductible;
    }

    public Double getMontant_ht() {
        return montant_ht;
    }

    public void setMontant_ht(Double montant_ht) {
        this.montant_ht = montant_ht;
    }

    public Boolean getVersement_tva() {
        return versement_tva;
    }

    public void setVersement_tva(Boolean versement_tva) {
        this.versement_tva = versement_tva;
    }

    public String getSous_classification() {
        return sous_classification;
    }

    public void setSous_classification(String sous_classification) {
        this.sous_classification = sous_classification;
    }

    public String getClassification() {
        return classification;
    }

    public void setClassification(String classification) {
        this.classification = classification;
    }

    public String getDetails() {
        return details;
    }

    public void setDetails(String details) {
        this.details = details;
    }

    public Boolean getFacture_verifie() {
        return facture_verifie;
    }

    public void setFacture_verifie(Boolean facture_verifie) {
        this.facture_verifie = facture_verifie;
    }

    public Integer getMois_valeur() {
        return mois_valeur;
    }

    public void setMois_valeur(Integer mois_valeur) {
        this.mois_valeur = mois_valeur;
    }

    public String getTransfert_compte() {
        return transfert_compte;
    }

    public void setTransfert_compte(String transfert_compte) {
        this.transfert_compte = transfert_compte;
    }

    public String getDate_ajout() {
        return date_ajout;
    }

    public void setDate_ajout(String date_ajout) {
        this.date_ajout = date_ajout;
    }
}