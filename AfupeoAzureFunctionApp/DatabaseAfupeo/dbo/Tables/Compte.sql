CREATE TABLE [dbo].[Compte] (
    [id]        UNIQUEIDENTIFIER CONSTRAINT [DF_Compte_id] DEFAULT (newid()) NOT NULL,
    [id_banque] UNIQUEIDENTIFIER NULL,
    [numero]    VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_Compte] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Compte_Banque] FOREIGN KEY ([id_banque]) REFERENCES [dbo].[Banque] ([id]),
    CONSTRAINT [AK_Name_Compte] UNIQUE NONCLUSTERED ([numero] ASC)
);



