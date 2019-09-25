CREATE PROCEDURE [dbo].[EditPiece]
@id uniqueidentifier ,
@data_piece varbinary(MAX),
@name varchar(50)
as 
update Piece 
set data=(coalesce(@data_piece,(select data from Piece where id = @id ))),
name=coalesce(@name,(select name from Piece where id = @id))