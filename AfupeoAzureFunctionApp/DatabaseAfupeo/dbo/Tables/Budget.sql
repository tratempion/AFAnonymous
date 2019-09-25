CREATE TABLE [dbo].[Budget] (
    [id]                     UNIQUEIDENTIFIER CONSTRAINT [DF_Objectifs_id] DEFAULT (newid()) NOT NULL,
    [objectif]               MONEY            NOT NULL,
    [periode]                DATE             NOT NULL,
    [id_sous_classification] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_Objectifs_sous_classification] FOREIGN KEY ([id_sous_classification]) REFERENCES [dbo].[sous_classification] ([id])
);

