CREATE TABLE [dbo].[Classification] (
    [id]   UNIQUEIDENTIFIER CONSTRAINT [DF_Classification_id] DEFAULT (newid()) NOT NULL,
    [name] VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_Classification] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [AK_Name_Classification] UNIQUE NONCLUSTERED ([name] ASC)
);

