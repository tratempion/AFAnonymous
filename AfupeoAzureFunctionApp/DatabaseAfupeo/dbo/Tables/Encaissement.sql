CREATE TABLE [dbo].[Encaissement] (
    [id]               UNIQUEIDENTIFIER CONSTRAINT [DF_Encaissement_id] DEFAULT (newid()) NOT NULL,
    [id_piece]         UNIQUEIDENTIFIER NULL,
    [id_societe]       UNIQUEIDENTIFIER NULL,
    [id_compte]        UNIQUEIDENTIFIER NULL,
    [total_ht]         MONEY            NULL,
    [total_tva]        MONEY            NULL,
    [total_ttc]        MONEY            NULL,
    [date_reglement]   DATETIME2 (7)    NULL,
    [id_mode_paiement] UNIQUEIDENTIFIER NULL,
    [mois_reglement]   TINYINT          NULL,
    [transfert_compte] MONEY            NULL,
    [date_ajout] DATETIME2 NULL, 
    CONSTRAINT [PK_Encaissement] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Encaissement_Compte] FOREIGN KEY ([id_compte]) REFERENCES [dbo].[Compte] ([id]),
    CONSTRAINT [FK_Encaissement_Mode_paiement] FOREIGN KEY ([id_mode_paiement]) REFERENCES [dbo].[Mode_paiement] ([id]),
    CONSTRAINT [FK_Encaissement_Piece] FOREIGN KEY ([id_piece]) REFERENCES [dbo].[Piece] ([id]),
    CONSTRAINT [FK_Encaissement_Societe] FOREIGN KEY ([id_societe]) REFERENCES [dbo].[Societe] ([id])
);



