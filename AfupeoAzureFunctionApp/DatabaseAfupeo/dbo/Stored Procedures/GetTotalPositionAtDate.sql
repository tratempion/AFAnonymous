CREATE procedure [dbo].[GetTotalPositionAtDate]
@date varchar(10)
as
declare @total_decaissement money;
declare @total_encaissement money;

set @total_decaissement=(
select  SUM(d.montant_ttc)
from Decaissement d 
where d.date_valeur <=CONVERT(datetime, @date,103)
and d.facture_verifie=1);
set @total_encaissement=(
select  SUM(e.total_ttc)
from Encaissement e
where e.date_reglement <=CONVERT(datetime, @date,103));

select @date as date, coalesce(@total_encaissement,0)-coalesce(@total_decaissement,0) as position ;