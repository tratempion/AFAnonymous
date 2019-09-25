USE [afupeoSQL]
GO
/****** Object:  Table [dbo].[Classification]    Script Date: 01/04/2019 11:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classification](
    [id] [uniqueidentifier] NOT NULL,
    [name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Classification] PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
,   CONSTRAINT AK_Name_Classification UNIQUE(name)   
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Compte]    Script Date: 01/04/2019 11:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compte](
    [id] [uniqueidentifier] NOT NULL,
    [id_banque] [uniqueidentifier] NOT NULL,
    [numero] [varchar](50) NOT NULL,
     CONSTRAINT AK_Name_Compte UNIQUE(numero)   ,

 CONSTRAINT [PK_Compte] PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Banque]    Script Date: 01/04/2019 11:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banque](
    [id] [uniqueidentifier] NOT NULL,
    [name] [varchar](50) NOT NULL,
     CONSTRAINT AK_Name_Banque UNIQUE(name)   ,

 CONSTRAINT [PK_Banque] PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Decaissment]    Script Date: 01/04/2019 11:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Decaissment](
    [id] [uniqueidentifier] NOT NULL,
    [operation] [varchar](50) NULL,
    [id_compte] [uniqueidentifier] NULL,
    [id_mode_paiement] [uniqueidentifier] NULL,
    [date_compta] [datetime2](7) NULL,
    [date_operation] [datetime2](7) NULL,
    [date_valeur] [datetime2](7) NULL,
    [montant_ttc] [money] NULL,
    [tva_deductible] [money] NULL,
    [montant_ht] [money] NULL,
    [versement_tva] [bit] NULL,
    [id_sous_classification] [uniqueidentifier] NULL,
    [details] [text] NULL,
    [facture_verifie] [bit] NULL,
    [mois_valeur] [tinyint] NULL,
    [transfert_compte] [money] NULL,
 CONSTRAINT [PK_Decaissment] PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Encaissement]    Script Date: 01/04/2019 11:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Encaissement](
    [id] [uniqueidentifier] NOT NULL,
    [id_piece] [uniqueidentifier] NULL,
    [date_piece] [datetime2](7) NULL,
    [id_societe] [uniqueidentifier] NULL,
    [id_compte] [uniqueidentifier] NULL,
    [total_ht] [money] NULL,
    [total_tva] [money] NULL,
    [total_ttc] [money] NULL,
    [date_reglement] [datetime2](7) NULL,
    [id_mode_paiement] [uniqueidentifier] NULL,
    [mois_reglement] [tinyint] NULL,
    [transfert_compte] [money] NULL,
 CONSTRAINT [PK_Encaissement] PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mode_paiement]    Script Date: 01/04/2019 11:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mode_paiement](
    [id] [uniqueidentifier] NOT NULL,
    [name] [varchar](50) NOT NULL,
     CONSTRAINT AK_Name_Mode_paiement UNIQUE(name),
 CONSTRAINT [PK_mode_paiement] PRIMARY KEY CLUSTERED
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Piece]    Script Date: 01/04/2019 11:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Piece](
    [id] [uniqueidentifier] NOT NULL,
    [name] [varchar](50) NOT NULL,
    [date] [datetime2](7) NOT NULL,
    [data] [varbinary](max) NOT NULL,
     CONSTRAINT AK_Name_Date_Piece UNIQUE(name,date),
 CONSTRAINT [PK_Piece] PRIMARY KEY CLUSTERED
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prevision]    Script Date: 01/04/2019 11:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prevision](
    [id] [uniqueidentifier] NOT NULL,
    [objectif] [money] NOT NULL,
    [periode] [date] NOT NULL,
    [id_sous_classification] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Societe]    Script Date: 01/04/2019 11:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Societe](
    [id] [uniqueidentifier] NOT NULL,
    [name] [varchar](10) NOT NULL,
     CONSTRAINT AK_Name_Societe UNIQUE(name),
 CONSTRAINT [PK_Societe] PRIMARY KEY CLUSTERED
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sous_classification]    Script Date: 01/04/2019 11:43:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sous_classification](
    [id] [uniqueidentifier] NOT NULL,
    [name] [varchar](100) NOT NULL,
    [id_classification] [uniqueidentifier] NOT NULL,
     CONSTRAINT AK_Name_id_Sous_classification UNIQUE(name,id)   ,
 CONSTRAINT [PK_sous_classification] PRIMARY KEY CLUSTERED 
(
    [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[Classification] ADD  CONSTRAINT [DF_Classification_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Compte] ADD  CONSTRAINT [DF_Compte_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Banque] ADD  CONSTRAINT [DF_Banque_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Decaissment] ADD  CONSTRAINT [DF_Decaissment_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Encaissement] ADD  CONSTRAINT [DF_Encaissement_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Mode_paiement] ADD  CONSTRAINT [DF_Mode_paiement_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Piece] ADD  CONSTRAINT [DF_Piece_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Prevision] ADD  CONSTRAINT [DF_Objectifs_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Societe] ADD  CONSTRAINT [DF_Societe_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[sous_classification] ADD  CONSTRAINT [DF_sous_classification_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Decaissment]  WITH CHECK ADD  CONSTRAINT [FK_Decaissment_Compte] FOREIGN KEY([id_compte])
REFERENCES [dbo].[Compte] ([id])
GO
ALTER TABLE [dbo].[Decaissment] CHECK CONSTRAINT [FK_Decaissment_Compte]
GO
ALTER TABLE [dbo].[Decaissment]  WITH CHECK ADD  CONSTRAINT [FK_Decaissment_Mode_paiement] FOREIGN KEY([id_mode_paiement])
REFERENCES [dbo].[Mode_paiement] ([id])
GO
ALTER TABLE [dbo].[Decaissment] CHECK CONSTRAINT [FK_Decaissment_Mode_paiement]
GO
ALTER TABLE [dbo].[Decaissment]  WITH CHECK ADD  CONSTRAINT [FK_Decaissment_sous_classification] FOREIGN KEY([id_sous_classification])
REFERENCES [dbo].[sous_classification] ([id])
GO
ALTER TABLE [dbo].[Decaissment] CHECK CONSTRAINT [FK_Decaissment_sous_classification]
GO
ALTER TABLE [dbo].[Encaissement]  WITH CHECK ADD  CONSTRAINT [FK_Encaissement_Compte] FOREIGN KEY([id_compte])
REFERENCES [dbo].[Compte] ([id])
GO
ALTER TABLE [dbo].[Compte]  WITH CHECK ADD  CONSTRAINT [FK_Compte_Banque] FOREIGN KEY([id_banque])
REFERENCES [dbo].[Banque] ([id])
GO
ALTER TABLE [dbo].[Encaissement] CHECK CONSTRAINT [FK_Encaissement_Compte]
GO
ALTER TABLE [dbo].[Encaissement]  WITH CHECK ADD  CONSTRAINT [FK_Encaissement_Mode_paiement] FOREIGN KEY([id_mode_paiement])
REFERENCES [dbo].[Mode_paiement] ([id])
GO
ALTER TABLE [dbo].[Encaissement] CHECK CONSTRAINT [FK_Encaissement_Mode_paiement]
GO
ALTER TABLE [dbo].[Encaissement]  WITH CHECK ADD  CONSTRAINT [FK_Encaissement_Piece] FOREIGN KEY([id_piece])
REFERENCES [dbo].[Piece] ([id])
GO
ALTER TABLE [dbo].[Encaissement] CHECK CONSTRAINT [FK_Encaissement_Piece]
GO
ALTER TABLE [dbo].[Encaissement]  WITH CHECK ADD  CONSTRAINT [FK_Encaissement_Societe] FOREIGN KEY([id_societe])
REFERENCES [dbo].[Societe] ([id])
GO
ALTER TABLE [dbo].[Encaissement] CHECK CONSTRAINT [FK_Encaissement_Societe]
GO
ALTER TABLE [dbo].[Prevision]  WITH CHECK ADD  CONSTRAINT [FK_Objectifs_sous_classification] FOREIGN KEY([id_sous_classification])
REFERENCES [dbo].[sous_classification] ([id])
GO
ALTER TABLE [dbo].[Prevision] CHECK CONSTRAINT [FK_Objectifs_sous_classification]
GO
ALTER TABLE [dbo].[sous_classification]  WITH CHECK ADD  CONSTRAINT [FK_sous_classification_classification] FOREIGN KEY([id_classification])
REFERENCES [dbo].[Classification] ([id])
GO
ALTER TABLE [dbo].[sous_classification] CHECK CONSTRAINT [FK_sous_classification_classification]
GO

IF OBJECT_ID ( 'GetDecaissment', 'P' ) IS NOT NULL   
    DROP PROCEDURE GetDecaissment;  
GO  
create procedure GetDecaissment as 
select Operation,
compte.numero as compte,
mode_paiement.name as mode_paiement,
date_compta,
date_operation,
date_valeur,
montant_ttc,
tva_deductible,
montant_ht,
versement_tva,
s.name as sous_classification,
classification.name as classification,
details,
facture_verifie,
mois_valeur,
transfert_compte 
from Decaissment d 
join compte on compte.id = d.id_compte 
join Mode_paiement on Mode_paiement.id = d.id_mode_paiement 
join sous_classification s on s.id = d.id_sous_classification 
join classification on classification.id = (select id from Classification where id=s.id_classification)
GO

IF OBJECT_ID ( 'GetUnvalidatedDecaissement', 'P' ) IS NOT NULL   
    DROP PROCEDURE GetUnvalidatedDecaissement;  
GO
create procedure GetUnvalidatedDecaissement as 
select Operation as operation,
compte.numero as compte,
mode_paiement.name as mode_paiement,
date_compta,
date_operation,
date_valeur,
montant_ttc
from Decaissment d 
join compte on compte.id = d.id_compte 
join Mode_paiement on Mode_paiement.id = d.id_mode_paiement

GO
IF OBJECT_ID ( 'GetAccountType', 'P' ) IS NOT NULL   
    DROP PROCEDURE GetAccountType;  
GO  
create procedure GetAccountType as 
select b.name as banque,
c.numero as compte
from Compte c 
join banque b on b.id = c.id_banque

GO
IF OBJECT_ID ( 'GetPaymentMethod', 'P' ) IS NOT NULL   
    DROP PROCEDURE GetPaymentMethod;  
GO  
create procedure GetPaymentMethod as 
select mp.name
from Mode_paiement mp

GO
IF OBJECT_ID ( 'saveLineDecaissement', 'P' ) IS NOT NULL   
    DROP PROCEDURE saveLineDecaissement;  
GO  
CREATE procedure [dbo].[saveLineDecaissement]
@operation varchar(50) =null,
@compte as varchar(10)=null,
@mode_paiement as varchar(10)=null,
@date_compta as datetime2(7)=null,
@date_ope as datetime2(7)=null,
@date_valeur as datetime2(7)=null,
@montant_ttc as money=null,
@tva_deductible as money=null,
@montant_ht as money=null,
@versement_tva as bit=null,
@sous_classification as varchar(100)=null,
@detail as text=null,
@verification as bit=null,
@mois_valeur as tinyint=null,
@transfert_compte as money = null
as 
begin try
    insert into Decaissment
    (operation,
    id_compte,
    id_mode_paiement,
    date_compta,
    date_operation,
    date_valeur,
    montant_ttc,
    tva_deductible,
    montant_ht,
    versement_tva,
    id_sous_classification,
    details,
    facture_verifie,
    mois_valeur,
    transfert_compte)
    values
    (@operation,
    (select id from Compte c  where c.numero like COALESCE (NULLIF(@compte, ''),'AUCUN')),
    (select id from Mode_paiement m  where m.name like COALESCE (NULLIF(@mode_paiement, ''),'AUCUN')),
    @date_compta,
    @date_ope,
    @date_valeur,
    @montant_ttc,
    @tva_deductible,
    @montant_ht,
    @versement_tva,
    (select id from sous_classification where name like COALESCE(NULLIF(@sous_classification, ''),'AUCUNE')),
    @detail,
    @verification,
    @mois_valeur,
    @transfert_compte);
    select 'OK' as retour;
end try
begin catch
    select ERROR_MESSAGE() as retour;
end catch
GO