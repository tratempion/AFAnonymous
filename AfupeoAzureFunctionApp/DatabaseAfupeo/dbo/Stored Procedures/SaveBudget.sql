CREATE procedure [dbo].[SaveBudget]
@mois_valeur varchar(2),
@annee varchar(4),
@id_sous_classification uniqueidentifier,
@budget money
as 
begin try
	declare @id_sous_calssification_verifiee uniqueidentifier;
	set @id_sous_calssification_verifiee=(select id from sous_classification where id = @id_sous_classification);
	if(@id_sous_calssification_verifiee is null)
		raiserror ('erreur : sous classification introuvable',16,1);
	insert into Budget(id_sous_classification,periode,objectif)
	values(@id_sous_calssification_verifiee, DATEFROMPARTS(@annee, @mois_valeur, '01'),@budget);
	select 'OK' as result;
end try
begin catch
	select ERROR_MESSAGE();
end catch