CREATE procedure [dbo].[GetDraftDecaissements] as 
select
d.id,
operation,
banque.name as banque,
compte.numero as compte,
mode_paiement.name as mode_paiement,
date_compta,
date_operation,
date_valeur,
montant_ttc
from Decaissement d 
join compte on compte.id = d.id_compte 
join banque on banque.id=compte.id_banque
join Mode_paiement on Mode_paiement.id = coalesce(d.id_mode_paiement,(select id from mode_paiement  where name='AUCUN') )
where facture_verifie=0
order by d.date_ajout 