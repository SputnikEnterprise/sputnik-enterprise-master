USE [master]
GO

CREATE DATABASE [Sputnik ScanJobs]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Sputnik ScanJobs', FILENAME = N'<your path>\Sputnik ScanJobs.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Sputnik ScanJobs_log', FILENAME = N'<your path>\Sputnik ScanJobs_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

USE [Sputnik ScanJobs]
GO

CREATE TABLE [dbo].[RP.ScannedFileContent](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MDName] [nvarchar](255) NULL,
	[MDGuid] [nvarchar](255) NULL,
	[KW] [tinyint] NULL,
	[Monday] [smalldatetime] NULL,
	[Sunday] [smalldatetime] NULL,
	[RPNr] [int] NULL,
	[RPLID] [int] NULL,
	[ESNr] [int] NULL,
	[MANr] [int] NULL,
	[KDNr] [int] NULL,
	[FoundedCodeValue] [nvarchar](255) NULL,
	[MAName] [nvarchar](255) NULL,
	[Firma1] [nvarchar](255) NULL,
	[Scan_MD] [varbinary](max) NULL,
	[Scan_Zuslagen] [varbinary](max) NULL,
	[Scan_Std] [varbinary](max) NULL,
	[Scan_Woche] [varbinary](max) NULL,
	[Scan_MA] [varbinary](max) NULL,
	[Scan_KD] [varbinary](max) NULL,
	[Scan_Unterschrift] [varbinary](max) NULL,
	[Scan_Nr] [varbinary](max) NULL,
	[Scan_Komplett] [varbinary](max) NULL,
	[IsValid] [tinyint] NULL,
	[ImportedFileGuid] [nvarchar](50) NULL,
	[File_ScannedOn] [smalldatetime] NULL,
	[CheckedOn] [smalldatetime] NULL,
	[RPMonth] [tinyint] NULL,
	[ModulNumber] [int] NULL,
	[RecordNumber] [int] NULL,
	[DocumentCategoryNumber] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[RP.ScannedFiles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[USNr] [int] NULL,
	[MDGuid] [nvarchar](50) NULL,
	[UserGuid] [nvarchar](50) NULL,
	[ImportedFileGuid] [nvarchar](50) NULL,
	[FileContent] [varbinary](max) NULL,
	[File_ScannedOn] [smalldatetime] NULL,
	[CheckedOn] [smalldatetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[tblMySetting](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_Name] [nvarchar](70) NULL,
	[RP_Guid] [nvarchar](50) NULL,
	[Recipients] [nvarchar](255) NULL,
	[bccAddresses] [nvarchar](255) NULL,
	[MailSender] [nvarchar](255) NULL,
	[MailUserName] [nvarchar](255) NULL,
	[MailPassword] [nvarchar](255) NULL,
	[SmtpServer] [nvarchar](255) NULL,
	[SmtpPort] [int] NULL,
 CONSTRAINT [PK_tblMySetting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/* ------------------ end of query --------------------------------------------- */
