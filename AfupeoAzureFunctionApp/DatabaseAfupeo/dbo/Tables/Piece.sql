CREATE TABLE [dbo].[Piece] (
    [id]   UNIQUEIDENTIFIER CONSTRAINT [DF_Piece_id] DEFAULT (newid()) NOT NULL,
    [name] VARCHAR (50)     NOT NULL,
    [date] DATETIME2 (7)    NULL,
    [data] VARBINARY (MAX)  NULL,
    CONSTRAINT [PK_Piece] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [AK_Name_Date_Piece] UNIQUE NONCLUSTERED ([name] ASC, [date] ASC)
);

