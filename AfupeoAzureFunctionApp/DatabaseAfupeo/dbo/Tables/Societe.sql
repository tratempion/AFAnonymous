CREATE TABLE [dbo].[Societe] (
    [id]   UNIQUEIDENTIFIER CONSTRAINT [DF_Societe_id] DEFAULT (newid()) NOT NULL,
    [name] VARCHAR (10)     NOT NULL,
    CONSTRAINT [PK_Societe] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [AK_Name_Societe] UNIQUE NONCLUSTERED ([name] ASC)
);

