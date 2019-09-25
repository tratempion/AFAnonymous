CREATE procedure [dbo].[GetClassification]
as 
select 
c.id as id_classification,
c.name classification,
s.id as id_sous_classification,
s.name as sous_classification
from classification c
join sous_classification s on c.id= s.id_classification
order by c.id desc