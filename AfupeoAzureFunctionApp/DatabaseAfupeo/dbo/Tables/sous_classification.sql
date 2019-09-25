CREATE TABLE [dbo].[sous_classification] (
    [id]                UNIQUEIDENTIFIER CONSTRAINT [DF_sous_classification_id] DEFAULT (newid()) NOT NULL,
    [name]              VARCHAR (100)    NOT NULL,
    [id_classification] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_sous_classification] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_sous_classification_classification] FOREIGN KEY ([id_classification]) REFERENCES [dbo].[Classification] ([id]),
    CONSTRAINT [AK_Name_id_Sous_classification] UNIQUE NONCLUSTERED ([name] ASC, [id] ASC)
);

