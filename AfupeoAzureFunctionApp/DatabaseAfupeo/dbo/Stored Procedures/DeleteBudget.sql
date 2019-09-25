CREATE procedure [dbo].[DeleteBudget]
@id uniqueidentifier
as 
begin 
	declare @id_verified uniqueidentifier;
	set @id_verified=(select id from Budget where id=@id)
	if @id_verified is null
		raiserror ('erreur : budget introuvable',16,1)
	delete from Budget where id= @id_verified;
	select 'OK' as result;
end