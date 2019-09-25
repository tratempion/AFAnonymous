CREATE procedure [dbo].[GetBudget] 
@classification varchar(50),
@year varchar(4)
as 
select p.id as id_budget ,p.objectif as budget,month(p.periode) as mois_valeur,s.id as id_sous_classification,s.name as sous_classification
from Budget p
join sous_classification s on s.id=p.id_sous_classification
join classification c on c.id=s.id_classification
where c.name=@classification
and YEAR(p.periode)=@year