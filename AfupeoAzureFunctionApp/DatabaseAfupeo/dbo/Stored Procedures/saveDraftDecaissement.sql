CREATE procedure [dbo].[saveDraftDecaissement]
@operation varchar(50) ,
@compte  varchar(50),
@banque  varchar(50),
@mode_paiement  varchar(10),
@date_compta  varchar(50),
@date_operation  varchar(50),
@date_valeur  varchar(50),
@montant_ttc  money
as 
SET DATEFORMAT dmy;  

declare @result varchar(50)=''
if (trim(@compte) =  '' or @compte is null)

	insert into Decaissement
	(operation,
	id_compte,
	id_mode_paiement,
	date_compta,
	date_operation,
	date_valeur,
	montant_ttc,
	facture_verifie)

	values

	(@operation,
	(select default_account from Banque b  where b.name like @banque),
	(select id from Mode_paiement m  where m.name like COALESCE (NULLIF(@mode_paiement, ''),'AUCUN')),
		convert(datetime,@date_compta,103),
		convert(datetime,@date_operation,103),
		convert(datetime,@date_valeur,103),
	@montant_ttc,
	0);
else
	insert into Decaissement
	(operation,
	id_compte,
	id_mode_paiement,
	date_compta,
	date_operation,
	date_valeur,
	montant_ttc,
	facture_verifie)

	values

	(@operation,
	(select id from Compte c  where c.numero like @compte),
	(select id from Mode_paiement m  where m.name like COALESCE (NULLIF(@mode_paiement, ''),'AUCUN')),
	@date_compta,
	@date_operation,
	@date_valeur,
	@montant_ttc,
	0);
select top 1 id as retour from decaissement order by date_ajout desc;