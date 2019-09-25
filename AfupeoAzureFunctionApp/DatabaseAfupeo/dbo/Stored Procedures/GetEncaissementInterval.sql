CREATE procedure [dbo].[GetEncaissementInterval]
@debut_interval varchar(50),
@fin_interval varchar(50)
as 
select * from Encaissement where date_ajout between CONVERT(datetime2(7),@debut_interval,103) and CONVERT(datetime2(7),@fin_interval,103)