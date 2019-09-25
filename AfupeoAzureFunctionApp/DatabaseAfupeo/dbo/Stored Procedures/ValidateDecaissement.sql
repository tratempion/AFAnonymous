CREATE procedure [dbo].[ValidateDecaissement]
@id_sous_classification varchar(50) ,
@id_decaissement varchar(50),
@mois_valeur tinyint,
@tva_deductible money,
@montant_ht money,
@versement_tva bit
as 
begin try
	begin
		declare @id_sous_classification_verified uniqueidentifier;
		set @id_sous_classification_verified= (select id from sous_classification where id = @id_sous_classification);
		if(@id_sous_classification_verified is null ) 
			begin 
				raiserror ('erreur : sous classification introuvable',16,1);
			end
		declare @id_decaissement_verified uniqueidentifier;
		set @id_decaissement_verified= (select id from Decaissement where id = @id_decaissement);
		if(@id_decaissement_verified is null ) 
			begin 
				raiserror ('erreur : decaissement introuvable',16,1);
			end
		update Decaissement set id_sous_classification= CONVERT(uniqueidentifier,@id_sous_classification_verified),
								facture_verifie=1,
								mois_valeur=@mois_valeur,
								tva_deductible=@tva_deductible,
								montant_ht=@montant_ht,
								versement_tva=@versement_tva
							WHERE id = convert(uniqueidentifier,@id_decaissement_verified);
		select 'OK' as result;
	end
end try
begin catch
	select ERROR_MESSAGE();
end catch