CREATE procedure [dbo].[GetAllEncaissement]
as 
select 
e.id,
s.name as societe,
c.numero as compte,
b.name as banque,
total_ht,
total_ttc,
total_tva,
date_reglement,
m.name as mode_paiement,
transfert_compte,
date_ajout
from Encaissement e
join Compte c on c.id=e.id_compte
join Banque b on b.id=c.id_banque 
join Mode_paiement m on m.id=e.id_mode_paiement
join Societe s on s.id=e.id_societe