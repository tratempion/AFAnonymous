CREATE TABLE [dbo].[Banque] (
    [id]   UNIQUEIDENTIFIER CONSTRAINT [DF_Banque_id] DEFAULT (newid()) NOT NULL,
    [name] VARCHAR (50)     NOT NULL,
    [default_account] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_Banque] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [AK_Name_Banque] UNIQUE NONCLUSTERED ([name] ASC), 
    CONSTRAINT [FK_Banque_Compte] FOREIGN KEY (default_account) REFERENCES [Compte]([id])
);

