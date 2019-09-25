CREATE TABLE [dbo].[Decaissement] (
    [id]                     UNIQUEIDENTIFIER CONSTRAINT [DF_Decaissment_id] DEFAULT (newid()) NOT NULL,
    [operation]              VARCHAR (50)     NOT NULL,
    [id_compte]              UNIQUEIDENTIFIER NOT NULL,
    [id_mode_paiement]       UNIQUEIDENTIFIER NULL,
    [date_compta]            DATETIME2 (7)    NULL,
    [date_operation]         DATETIME2 (7)    NULL,
    [date_valeur]            DATETIME2 (7)    NULL,
    [montant_ttc]            MONEY            NULL,
    [tva_deductible]         MONEY            NULL,
    [montant_ht]             MONEY            NULL,
    [versement_tva]          BIT              NULL,
    [id_sous_classification] UNIQUEIDENTIFIER NULL,
    [details]                TEXT             NULL,
    [facture_verifie]        BIT              NOT NULL,
    [mois_valeur]            TINYINT          NULL,
    [transfert_compte]       MONEY            NULL,
    [date_ajout]             DATETIME2 (7)    CONSTRAINT [DF_Decaissment_date_ajout] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Decaissment] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Decaissment_Compte] FOREIGN KEY ([id_compte]) REFERENCES [dbo].[Compte] ([id]),
    CONSTRAINT [FK_Decaissment_Mode_paiement] FOREIGN KEY ([id_mode_paiement]) REFERENCES [dbo].[Mode_paiement] ([id]),
    CONSTRAINT [FK_Decaissment_sous_classification] FOREIGN KEY ([id_sous_classification]) REFERENCES [dbo].[sous_classification] ([id])
);

