CREATE PROCEDURE [dbo].[DeleteEncaissement]
@id uniqueidentifier
as 
begin try
	delete from Encaissement where id = @id;
	select 'OK' as retour;
end try
begin catch
	select ERROR_MESSAGE() as retour;
end catch