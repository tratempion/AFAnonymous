create procedure [dbo].[GetObjectifYear]
@annee varchar(4)
as
select c.name as classif, s.name as sous_classif,b.objectif,d.operation,d.montant_ttc
from Classification c
join sous_classification s on s.id_classification=c.id
left merge join 
	(select sum(objectif) as objectif,id_sous_classification from Budget where YEAR(Budget.periode)=@annee group by id_sous_classification) as b  
	on b.id_sous_classification=s.id
left merge join Decaissement d on d.id_sous_classification=s.id and facture_verifie=1 and YEAR(d.date_valeur)=@annee

order by sous_classif desc