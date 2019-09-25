CREATE procedure [dbo].[GetAllAccountsPositionAtDate]

@date  varchar(50)

as

select numero, coalesce(SUM(amount),0) as position from (
select CONCAT(b.name,' - ',c.numero) as numero,(0-d.montant_ttc) as amount
from Compte c
join Banque b on b.id=c.id_banque
left join Decaissement d on d.id_compte=c.id and d.date_valeur<=CONVERT(datetime,@date,103) and facture_verifie=1
union all

select CONCAT(b.name,' - ',c.numero) as numero,e.total_ttc as amount
from Compte c
join Banque b on b.id=c.id_banque
left join Encaissement e on e.id_compte=c.id and e.date_reglement<=CONVERT(datetime,@date,103)) x
group by numero