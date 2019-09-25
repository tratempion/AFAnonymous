CREATE TABLE [dbo].[Mode_paiement] (
    [id]   UNIQUEIDENTIFIER CONSTRAINT [DF_Mode_paiement_id] DEFAULT (newid()) NOT NULL,
    [name] VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_mode_paiement] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [AK_Name_Mode_paiement] UNIQUE NONCLUSTERED ([name] ASC)
);

