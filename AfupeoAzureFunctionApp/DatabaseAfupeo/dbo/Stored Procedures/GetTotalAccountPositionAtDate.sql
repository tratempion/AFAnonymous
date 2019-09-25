create procedure [dbo].[GetTotalAccountPositionAtDate]
@date varchar(10),
@compte varchar(50)
as

declare @total_decaissement money;
declare @total_encaissement money;

set @total_decaissement=(
select  SUM(d.montant_ttc)
from Decaissement d 
join Compte c on c.id=d.id_compte
where d.date_valeur <=CONVERT(datetime, @date,103)
and d.facture_verifie=1
and c.numero=@compte);
set @total_encaissement=(
select  SUM(e.total_ttc)
from Encaissement e
join Compte c on c.id=e.id_compte
where e.date_reglement <=CONVERT(datetime, @date,103)
and c.numero=@compte);

select @date as date, coalesce(@total_encaissement,0)-coalesce(@total_decaissement,0) as position ;