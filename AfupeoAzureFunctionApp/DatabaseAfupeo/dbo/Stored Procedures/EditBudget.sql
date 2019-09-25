CREATE procedure [dbo].[EditBudget]
@id uniqueidentifier,
@id_sous_classification uniqueidentifier,
@budget money,
@mois_valeur tinyint,
@annee varchar(4)
as 
begin 
	declare @id_sous_classification_verifiee uniqueidentifier;
	set @id_sous_classification_verifiee=(select id from sous_classification where id=@id_sous_classification)
	if (@id_sous_classification_verifiee is null and @id_sous_classification is not null) 
		raiserror ('erreur : sous classification introuvable',16,1);
	update Budget set 
	id_sous_classification=coalesce(@id_sous_classification,(select id_sous_classification from Budget where id =@id)),
	objectif=coalesce(@budget,(select objectif from Budget where id =@id)),
	periode=coalesce(
					DATEFROMPARTS(@annee, @mois_valeur, '01'),
					DATEFROMPARTS(YEAR((select periode from Budget where id = @id)), @mois_valeur, '01'),
					DATEFROMPARTS(@annee, MONTH((select periode from Budget where id = @id)), '01'),
					(select periode from Budget where id =@id))
	WHERE id=@id;
	select 'OK' as result;
end