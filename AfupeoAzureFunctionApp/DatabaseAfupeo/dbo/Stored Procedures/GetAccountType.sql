CREATE procedure [dbo].[GetAccountType] as 
select 
b.name as banque,
c.numero as compte
from Compte c 
join banque b on b.id = c.id_banque
order by b.name,c.numero