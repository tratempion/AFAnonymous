CREATE procedure [dbo].[DeleteDecaissement]
@id uniqueidentifier
as
begin try

	if (select facture_verifie from decaissement where id=@id) = 0
		begin 
		delete from decaissement where id=@id;
		select 'OK' as retour;
		end
	else
		select 'Impossible de supprimer un decaissement validé ou inexistant' as retour;
end try
begin catch
	select ERROR_MESSAGE() as retour;
end catch