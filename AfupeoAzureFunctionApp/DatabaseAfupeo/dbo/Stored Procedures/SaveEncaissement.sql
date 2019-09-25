CREATE PROCEDURE [dbo].[SaveEncaissement]
@societe varchar(50),
@compte varchar(50),
@banque varchar(50),
@total_ht money,
@total_tva money,
@total_ttc money,
@date_reglement varchar(50),
@mode_paiement varchar(50),
@transfert_compte money
as 
begin try
	declare @id_compte uniqueidentifier;
	set @id_compte = ( select (coalesce((select c.id from compte c join Banque b on c.id_banque=b.id where b.name like @banque and c.numero like @compte),
												 (select default_account from Banque where name like @banque))));
	insert into Encaissement ( id_societe,id_compte,total_ht,total_tva,total_ttc,date_reglement,id_mode_paiement,mois_reglement,transfert_compte,date_ajout)
	values(
	(select id from Societe where name like @societe),
	@id_compte,
	@total_ht,
	@total_tva,
	@total_ttc,
	CONVERT(datetime,@date_reglement,103),
	(select id from mode_paiement where name like @mode_paiement),
	(select month(CONVERT(datetime,@date_reglement,103))),
	@transfert_compte,
	getdate());
	select top 1 id from Encaissement order by date_ajout desc;
end try
begin catch
	begin
		select concat(ERROR_MESSAGE(),@date_reglement);
		throw;
	end
end catch