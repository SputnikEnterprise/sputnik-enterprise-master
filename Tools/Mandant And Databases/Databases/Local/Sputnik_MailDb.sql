USE [master]
GO


CREATE DATABASE [Sputnik_MailDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Sputnik_MailDb', FILENAME = N'<your path>\Sputnik_MailDb.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Sputnik_MailDb_log', FILENAME = N'<your path>\Sputnik_MailDb_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

USE [Sputnik_MailDb]
GO

CREATE TABLE [dbo].[Mail_FileScan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RecNr] [int] NULL,
	[Customer_ID] [nvarchar](255) NULL,
	[eMail_To] [nvarchar](255) NULL,
	[eMail_From] [nvarchar](255) NULL,
	[eMail_Subject] [nvarchar](max) NULL,
	[ScanFile] [varbinary](max) NULL,
	[FileName] [nvarchar](255) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](100) NULL,
	[Message_ID] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[Mail_Kontakte](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RecNr] [int] NULL,
	[Customer_ID] [nvarchar](255) NULL,
	[KDNr] [int] NULL,
	[KDZNr] [int] NULL,
	[MANr] [int] NULL,
	[eMail_To] [nvarchar](255) NULL,
	[eMail_From] [nvarchar](255) NULL,
	[eMail_Subject] [nvarchar](max) NULL,
	[eMail_Body] [nvarchar](max) NULL,
	[eMail_smtp] [nvarchar](70) NULL,
	[AsHtml] [bit] NULL,
	[AsTelefax] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](100) NULL,
	[Message_ID] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/* ------------------ end of query --------------------------------------------- */
