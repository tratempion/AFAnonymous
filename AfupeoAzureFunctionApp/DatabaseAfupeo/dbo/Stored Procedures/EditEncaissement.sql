CREATE procedure [dbo].[EditEncaissement]
@id uniqueidentifier,
@societe varchar(50),
@compte varchar(50),
@banque varchar(50),
@total_ht money,
@total_tva money,
@total_ttc money,
@date_reglement varchar(10),
@mode_paiement varchar(50),
@transfert_compte money
as
declare @id_compte uniqueidentifier;
set @id_compte =  ( select (coalesce((select c.id from compte c join Banque b on c.id_banque=b.id where b.name like @banque and c.numero like @compte),
												(select default_account from Banque where name like @banque))));
update Encaissement
set 
id_societe =coalesce((select id from Societe where name like @societe),(select id_societe from encaissement where id=@id)),
id_compte =coalesce(@id_compte,(select id from compte where  numero like @id_compte),(select id_compte from encaissement where id=@id)),
id_mode_paiement =coalesce((select id from mode_paiement where name like @mode_paiement),(select id_mode_paiement from encaissement where id=@id)),
total_ht =coalesce(@total_ht,(select total_ht from encaissement where id=@id)),
total_tva =coalesce(@total_tva,(select total_tva from encaissement where id=@id)),
total_ttc =coalesce(@total_ttc,(select @total_ttc from encaissement where id=@id)),
date_reglement =coalesce(convert(datetime,@date_reglement,103),(select date_reglement from encaissement where id=@id)),
mois_reglement =coalesce(month(convert(datetime,@date_reglement,103)),(select mois_reglement from encaissement where id=@id)),
transfert_compte =coalesce(@transfert_compte,(select transfert_compte from encaissement where id=@id)),
date_ajout =getdate()
from encaissement 
where id=@id;
select 'OK' as retour;