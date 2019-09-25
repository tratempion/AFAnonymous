CREATE procedure [dbo].[EditDecaissement]
@id uniqueidentifier,
@operation varchar(50),
@compte varchar(50),
@banque varchar(50),
@mode_paiement varchar(50),
@date_compta varchar(10),
@date_operation varchar(10),
@date_valeur varchar(10),
@montant_ttc money,
@tva_deductible money,
@montant_ht money,
@versement_tva bit,
@id_sous_classification uniqueidentifier,
@details text,
@facture_verifie bit,
@mois_valeur tinyint,
@transfert_compte money
as
BEGIN TRY
	update Decaissement
	set 
	operation =coalesce(@operation,(select operation from decaissement where id=@id)),
	id_compte =coalesce((select id from compte where numero like @compte),(select default_account from banque where name like @banque),(select id_compte from decaissement where id=@id)),
	id_mode_paiement =coalesce((select id from mode_paiement where name like @mode_paiement),(select id_mode_paiement from decaissement where id=@id)),
	date_compta =coalesce(convert(datetime,@date_compta,103),(select date_compta from decaissement where id=@id)),
	date_operation =coalesce(convert(datetime,@date_operation,103),(select date_operation from decaissement where id=@id)),
	date_valeur =coalesce(convert(datetime,@date_valeur,103),(select date_valeur from decaissement where id=@id)),
	montant_ttc =coalesce(@montant_ttc,(select montant_ttc from decaissement where id=@id)),
	tva_deductible =coalesce(@tva_deductible,(select tva_deductible from decaissement where id=@id)),
	montant_ht =coalesce(@montant_ht,(select montant_ht from decaissement where id=@id)),
	versement_tva =coalesce(@versement_tva,(select versement_tva from decaissement where id=@id)),
	id_sous_classification =coalesce(@id_sous_classification,(select id_sous_classification from decaissement where id=@id)),
	details =coalesce(@details,(select details from decaissement where id=@id)),
	facture_verifie =coalesce(@facture_verifie,(select facture_verifie from decaissement where id=@id)),
	mois_valeur =coalesce(@mois_valeur,(select mois_valeur from decaissement where id=@id)),
	transfert_compte =coalesce(@transfert_compte,(select transfert_compte from decaissement where id=@id)),
	date_ajout =getdate()
	from Decaissement 
	where id=@id;
	select 'OK' as retour;
END TRY
BEGIN CATCH
	SELECT ERROR_MESSAGE();
END CATCH