USE [master]
GO

CREATE DATABASE [Sputnik DbSelect]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Sputnik DbSelect', FILENAME = N'<your path>\Sputnik DbSelect.mdf' , SIZE = 22848KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'Sputnik DbSelect_log"', FILENAME = N'<your path>\Sputnik DbSelect_log.ldf' , SIZE = 504KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO


USE [Sputnik DbSelect]
GO

CREATE TABLE [dbo].[Mandanten](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MDNr] [int] NULL,
	[MDName] [nvarchar](50) NULL,
	[MDPath] [nvarchar](255) NULL,
	[Deaktiviert] [bit] NULL,
	[DbName] [nvarchar](255) NULL,
	[DbConnectionstr] [nvarchar](500) NULL,
	[DbServerName] [nvarchar](255) NULL,
	[Customer_id] [nvarchar](255) NULL,
	[MDGroupNr] [int] NULL,
	[FileServerPath] [nvarchar](255) NULL,
 CONSTRAINT [PK_Mandanten] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[Mandanten]
           ([MDNr]
           ,[MDName]
           ,[MDPath]
           ,[Deaktiviert]
           ,[DbName]
           ,[DbConnectionstr]
           ,[DbServerName]
           ,[Customer_id]
           ,[MDGroupNr]
           ,[FileServerPath])
     VALUES
           (1
           ,'Demo MD'
           ,'<your path>\Sputnik Enterprise Server\MD11\'
           ,0
           ,'Sputnik TDemo'
           ,'Provider=SQLOLEDB.1;Password="password";Persist Security Info=True;User ID=username;Initial Catalog=dbname;Data Source=dbserver'
           ,'dbserver'
           ,NewID()
           ,1
           ,'<your path>\Sputnik Enterprise Server\MD11\'
		   )
GO




CREATE TABLE [dbo].[Aufgaben](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Aufgaben-Nr] [int] NULL,
	[Aufgaben-Betreff] [nvarchar](200) NULL,
	[Aufgaben-Beginn] [smalldatetime] NULL,
	[Aufgaben-Ende] [smalldatetime] NULL,
	[Ganztag] [bit] NOT NULL,
	[Errinerung] [bit] NOT NULL,
	[Errinern in] [smallint] NULL,
	[Aufgaben-Beschreibung] [nvarchar](1000) NULL,
	[Private] [bit] NOT NULL,
	[BenutzerNr] [int] NULL,
	[Benutzername] [nvarchar](70) NULL,
	[Erledigt] [bit] NULL,
	[Beginn Std] [int] NULL,
	[Beginn Min] [int] NULL,
	[End Std] [int] NULL,
	[End Min] [int] NULL,
	[Erledigt Am] [smalldatetime] NULL,
	[Erledigt Vom] [nvarchar](70) NULL,
	[Filialenname] [nvarchar](100) NULL,
	[Kontaktperson] [nvarchar](1000) NULL,
	[FirstRecNr] [int] NULL,
	[KontaktModul] [nvarchar](10) NULL,
	[SecRecNr] [int] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
 CONSTRAINT [PK_Aufgaben] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Aufgaben] ADD  DEFAULT (newid()) FOR [rowguid]
GO

CREATE TABLE [dbo].[Kandidaten](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](255) NULL,
	[MANr] [int] NULL,
	[MA_Guid] [nvarchar](100) NULL,
	[Berater] [nvarchar](255) NULL,
	[MA_Vorname] [nvarchar](255) NULL,
	[MA_Nachname] [nvarchar](255) NULL,
	[MA_Kanton] [nvarchar](255) NULL,
	[MA_Ort] [nvarchar](255) NULL,
	[MA_Beruf] [nvarchar](1000) NULL,
	[MA_Branche] [nvarchar](1000) NULL,
	[MASex] [nvarchar](10) NULL,
	[MA_EMail] [nvarchar](255) NULL,
	[MA_GebDat] [datetime] NULL,
	[MA_Language] [nvarchar](70) NULL,
	[MA_Nationality] [nvarchar](255) NULL,
	[BriefAnrede] [nvarchar](70) NULL,
	[AGB_WOS] [nvarchar](70) NULL,
	[Transfered_User] [nvarchar](100) NULL,
	[Transfered_On] [datetime] NULL,
 CONSTRAINT [PK_Kandidaten] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Kandidaten_Doc_Online](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MANr] [int] NULL,
	[ESNr] [int] NULL,
	[ESLohnNr] [int] NULL,
	[LONr] [int] NULL,
	[LogedUser_ID] [nvarchar](255) NULL,
	[Customer_ID] [nvarchar](255) NULL,
	[Customer_Name] [nvarchar](255) NULL,
	[Customer_Strasse] [nvarchar](255) NULL,
	[Customer_Ort] [nvarchar](255) NULL,
	[Customer_Telefon] [nvarchar](255) NULL,
	[Customer_eMail] [nvarchar](255) NULL,
	[Berater] [nvarchar](255) NULL,
	[MA_Vorname] [nvarchar](255) NULL,
	[MA_Nachname] [nvarchar](255) NULL,
	[MA_Filiale] [nvarchar](50) NULL,
	[MA_Kanton] [nvarchar](255) NULL,
	[MA_Ort] [nvarchar](255) NULL,
	[MASex] [nvarchar](10) NULL,
	[MAZivil] [nvarchar](50) NULL,
	[BriefAnrede] [nvarchar](70) NULL,
	[AGB_WOS] [nvarchar](70) NULL,
	[MA_Beruf] [nvarchar](1000) NULL,
	[MA_Branche] [nvarchar](1000) NULL,
	[MA_EMail] [nvarchar](255) NULL,
	[MA_GebDat] [datetime] NULL,
	[MA_Language] [nvarchar](70) NULL,
	[MA_Nationality] [nvarchar](255) NULL,
	[Transfered_User] [nvarchar](100) NULL,
	[Transfered_On] [datetime] NULL,
	[Owner_Guid] [nvarchar](100) NULL,
	[Doc_Guid] [nvarchar](100) NULL,
	[Doc_Art] [nvarchar](255) NULL,
	[Doc_Info] [nvarchar](255) NULL,
	[Result] [nvarchar](10) NULL,
	[User_Nachname] [nvarchar](70) NULL,
	[User_Vorname] [nvarchar](70) NULL,
	[User_Telefon] [nvarchar](70) NULL,
	[User_Telefax] [nvarchar](70) NULL,
	[User_eMail] [nvarchar](255) NULL,
	[DocFileName] [nvarchar](255) NULL,
	[DocScan] [varbinary](max) NULL,
	[LastNotification] [datetime] NULL,
	[RPNr] [int] NULL,
	[RPLNr] [int] NULL,
	[RPDocNr] [int] NULL,
	[MA_SSprache] [nvarchar](4000) NULL,
	[MA_MSprache] [nvarchar](4000) NULL,
	[MA_Eigenschaft] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Kandidaten_Doc_Online] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[Kunden_Doc_Online](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[KDNr] [int] NULL,
	[ZHDNr] [int] NULL,
	[ESNr] [int] NULL,
	[ESLohnNr] [int] NULL,
	[RPNr] [int] NULL,
	[RENr] [int] NULL,
	[LogedUser_ID] [nvarchar](255) NULL,
	[Customer_ID] [nvarchar](255) NULL,
	[KD_Name] [nvarchar](255) NULL,
	[ZHD_Vorname] [nvarchar](255) NULL,
	[ZHD_Nachname] [nvarchar](255) NULL,
	[KD_Filiale] [nvarchar](50) NULL,
	[KD_Kanton] [nvarchar](255) NULL,
	[KD_Ort] [nvarchar](255) NULL,
	[KD_AGB_WOS] [nvarchar](70) NULL,
	[ZHDSex] [nvarchar](10) NULL,
	[ZHD_BriefAnrede] [nvarchar](70) NULL,
	[KD_eMail] [nvarchar](255) NULL,
	[ZHD_eMail] [nvarchar](255) NULL,
	[KD_Guid] [nvarchar](100) NULL,
	[ZHD_Guid] [nvarchar](100) NULL,
	[Doc_Guid] [nvarchar](100) NULL,
	[Doc_Art] [nvarchar](255) NULL,
	[Doc_Info] [nvarchar](255) NULL,
	[Result] [nvarchar](10) NULL,
	[KD_Berater] [nvarchar](255) NULL,
	[ZHD_Berater] [nvarchar](255) NULL,
	[KD_Beruf] [nvarchar](1000) NULL,
	[KD_Branche] [nvarchar](1000) NULL,
	[ZHD_Beruf] [nvarchar](1000) NULL,
	[ZHD_Branche] [nvarchar](1000) NULL,
	[ZHD_AGB_WOS] [nvarchar](70) NULL,
	[ZHD_GebDat] [datetime] NULL,
	[Transfered_User] [nvarchar](100) NULL,
	[Transfered_On] [datetime] NULL,
	[User_Nachname] [nvarchar](70) NULL,
	[User_Vorname] [nvarchar](70) NULL,
	[User_Telefon] [nvarchar](70) NULL,
	[User_Telefax] [nvarchar](70) NULL,
	[User_eMail] [nvarchar](255) NULL,
	[GetResult] [tinyint] NULL,
	[Get_On] [datetime] NULL,
	[LastNotification] [datetime] NULL,
	[KD_Language] [nvarchar](70) NULL,
	[DocFileName] [nvarchar](255) NULL,
	[DocScan] [varbinary](max) NULL,
 CONSTRAINT [PK_Kunden_Doc_Online] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[MyTasks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RecNr] [int] NULL,
	[Betreff] [nvarchar](200) NULL,
	[Beginn] [smalldatetime] NULL,
	[Beschreibung] [nvarchar](4000) NULL,
	[Privat] [bit] NULL,
	[USNr] [int] NULL,
	[USGuid] [nvarchar](50) NULL,
	[UserName] [nvarchar](70) NULL,
	[BeginnStd] [int] NULL,
	[BeginnMin] [int] NULL,
	[Dauer] [nvarchar](70) NULL,
	[KontaktInfo] [nvarchar](4000) NULL,
	[KontaktRecNr] [int] NULL,
	[MDGuid] [nvarchar](50) NULL,
	[MDName] [nvarchar](70) NULL,
	[CreatedOn] [smalldatetime] NULL,
	[CreatedFrom] [nvarchar](70) NULL,
	[ChangedOn] [smalldatetime] NULL,
	[ChangedFrom] [nvarchar](70) NULL,
	[Result] [nvarchar](10) NULL,
 CONSTRAINT [PK_MyTasks] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[SPConnStr](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ForWhat] [nvarchar](50) NULL,
	[ConnStr] [nvarchar](2000) NULL,
	[Result] [nvarchar](50) NULL,
 CONSTRAINT [PK_SPConnStr] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[SpUpdateHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UpdateFileName] [nvarchar](1000) NULL,
	[FileDestPath] [nvarchar](255) NULL,
	[UpdateFileVersion] [nvarchar](255) NULL,
	[UpdateFileDate] [datetime] NULL,
	[UpdateFileTime] [nvarchar](255) NULL,
	[CreatedOn] [datetime] NULL,
	[ClientID] [nchar](255) NULL,
	[ServerID] [nchar](255) NULL,
	[Result] [nvarchar](255) NULL,
	[File_Guid] [nvarchar](255) NULL,
	[UpdateFileSize] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[tbl_UpdateInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UpdateVersion] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateDescription] [nvarchar](700) NULL,
	[UpdateSettingFile] [nvarchar](255) NULL,
 CONSTRAINT [PK__tbl_UpdateInfo__1387E197] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[tbl_UpdateViewedProtokoll](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RecID] [int] NULL,
	[UpdateFileName] [nvarchar](255) NULL,
	[UpdateFileDate] [datetime] NULL,
	[Username] [nvarchar](255) NULL,
	[UpdateViewed] [datetime] NULL,
 CONSTRAINT [PK_tbl_UpdateInfo_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[tblCustomerPayableServices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_Guid] [nvarchar](50) NULL,
	[User_Guid] [nvarchar](50) NULL,
	[ServiceName] [nvarchar](255) NULL,
	[UsedPoints] [nvarchar](50) NULL,
	[Servicedate] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
	[JobGuild] [nvarchar](255) NULL,
	[JobGuid] [nvarchar](255) NULL,
	[JobID] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[UserInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StationIP] [nvarchar](255) NULL,
	[StationName] [nvarchar](50) NULL,
	[MACAdress] [nvarchar](500) NULL,
	[FirstTimeLoged] [nvarchar](30) NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Loged_USer_ID] [nvarchar](200) NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [DF__UserInfo__rowgui__5BAD9CC8]  DEFAULT (newid()) FOR [rowguid]
GO



CREATE PROCEDURE [dbo].[List Mandant Data For Selecting Mandant]

AS

begin
SET NOCOUNT ON

SELECT  *
FROM    [Sputnik DbSelect].Dbo.Mandanten
WHERE   Deaktiviert = 0
ORDER BY mdnr;	

END

GO

/* ------------------ end of query --------------------------------------------- */
