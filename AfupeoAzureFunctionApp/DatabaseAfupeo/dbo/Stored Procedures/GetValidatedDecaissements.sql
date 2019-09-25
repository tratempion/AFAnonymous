CREATE procedure [dbo].[GetValidatedDecaissements] as 
SELECT d.[id]
      ,[operation]
      ,c.numero as compte
	  ,b.name as banque
      ,m.name as mode_paiement
      ,[date_compta]
      ,[date_operation]
      ,[date_valeur]
      ,[montant_ttc]
      ,[tva_deductible]
      ,[montant_ht]
      ,[versement_tva]
      ,s.name as sous_classification
      ,cl.name as classification
	  ,[details]
      ,[facture_verifie]
      ,[mois_valeur]
      ,[transfert_compte]
      ,[date_ajout]
  FROM [dbo].[Decaissement] d
  join compte c on c.id =d.id_compte
  join Mode_paiement m on m.id = coalesce(d.id_mode_paiement,(select id from mode_paiement  where name='AUCUN') ) 
  join sous_classification s on s.id= coalesce(d.id_sous_classification,(select id from sous_classification  where name='AUCUNE') )
  join Classification cl on cl.id=coalesce(s.id_classification,(select id from classification where name='AUCUNE') )
  join Banque b on b.id=c.id_banque
  where d.facture_verifie=1 
