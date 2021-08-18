USE [master]
GO

CREATE DATABASE [SpContract]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'spContract', FILENAME = N'<your path>\SpContract.mdf' , SIZE = 56812544KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'spContract_log', FILENAME = N'<your path>\SpContract_log.ldf' , SIZE = 768KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO


USE [SpContract]
GO

/* ------------------ create tables ------------------ */

CREATE TABLE [dbo].[MySetting](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Userkey] [NVARCHAR](255) NULL,
	[Passwort] [NVARCHAR](70) NULL,
	[eMail] [NVARCHAR](255) NULL,
	[Customer_Name] [NVARCHAR](70) NULL,
	[KD_ZHD] [NVARCHAR](70) NULL,
	[ModulName] [NVARCHAR](50) NULL,
	[Vak_Guid] [NVARCHAR](255) NULL,
	[MA_Guid] [NVARCHAR](255) NULL,
	[KD_Guid] [NVARCHAR](255) NULL,
	[DP_Guid] [NVARCHAR](255) NULL,
	[Verleih_Guid] [NVARCHAR](255) NULL,
	[Customer_Logo] [VARBINARY](MAX) NULL,
	[Customer_Ort] [NVARCHAR](255) NULL,
	[Customer_Telefon] [NVARCHAR](70) NULL,
	[Customer_Telefax] [NVARCHAR](70) NULL,
	[Customer_eMail] [NVARCHAR](255) NULL,
	[Customer_AGB] [VARBINARY](MAX) NULL,
	[Customer_Strasse] [NVARCHAR](70) NULL,
	[Customer_Homepage] [NVARCHAR](255) NULL,
	[Customer_cssFile] [NVARCHAR](255) NULL,
	[PrintVerleih] [TINYINT] NULL,
	[Customer_AGBFest] [VARBINARY](MAX) NULL,
	[Customer_AGBSonst] [VARBINARY](MAX) NULL,
	[Customer_AGBFest_I] [VARBINARY](MAX) NULL,
	[Customer_AGBSonst_I] [VARBINARY](MAX) NULL,
	[Customer_AGBFest_F] [VARBINARY](MAX) NULL,
	[Customer_AGBSonst_F] [VARBINARY](MAX) NULL,
	[Customer_AGBFest_E] [VARBINARY](MAX) NULL,
	[Customer_AGBSonst_E] [VARBINARY](MAX) NULL,
	[Customer_AGB_I] [VARBINARY](MAX) NULL,
	[Customer_AGB_F] [VARBINARY](MAX) NULL,
	[Customer_AGB_E] [VARBINARY](MAX) NULL,
	[Rahmenvertrag] [VARBINARY](MAX) NULL,
	[Rahmenvertrag_I] [VARBINARY](MAX) NULL,
	[Rahmenvertrag_F] [VARBINARY](MAX) NULL,
	[Rahmenvertrag_E] [VARBINARY](MAX) NULL,
	[AutoNotification] [TINYINT] NULL,
	[Visible_Candidate_Fields] [VARCHAR](255) NULL,
	[Visible_Vacancy_Fields] [VARCHAR](256) NULL,
	[Autonotification_MA] [BIT] NULL,
	[Autonotification_KD] [BIT] NULL,
	[GAVUnia] [NVARCHAR](255) NULL,
	[TplFilename] [NVARCHAR](255) NULL,
	[Customer_ID] [NVARCHAR](50) NULL,
	[WOS_Guid] [NVARCHAR](50) NULL,
	[ISKDAllowed] [BIT] NULL,
	[ISMAAllowed] [BIT] NULL,
	[ISVakAllowed] [BIT] NULL,
 CONSTRAINT [PK_SMS_Setting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[Customer_Users](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[User_ID] [NVARCHAR](255) NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[Customer_Name] [NVARCHAR](255) NULL,
	[User_Sex] [NVARCHAR](20) NULL,
	[User_Vorname] [NVARCHAR](70) NULL,
	[User_Nachname] [NVARCHAR](70) NULL,
	[User_Initial] [NVARCHAR](50) NULL,
	[User_Filiale] [NVARCHAR](50) NULL,
	[User_Telefon] [NVARCHAR](255) NULL,
	[User_Telefax] [NVARCHAR](255) NULL,
	[User_eMail] [NVARCHAR](255) NULL,
	[User_Homepage] [NVARCHAR](255) NULL,
	[CreatedOn] [SMALLDATETIME] NULL,
	[User_Picture] [VARBINARY](MAX) NULL,
	[User_Sign] [VARBINARY](MAX) NULL,
	[Result] [NVARCHAR](10) NULL,
	[WOS_Guid] [NVARCHAR](50) NULL,
 CONSTRAINT [PK_Customer_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[kandidaten](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[MANr] [INT] NULL,
	[MA_Guid] [NVARCHAR](100) NULL,
	[Berater] [NVARCHAR](255) NULL,
	[MA_Vorname] [NVARCHAR](255) NULL,
	[MA_Nachname] [NVARCHAR](255) NULL,
	[MA_Kanton] [NVARCHAR](255) NULL,
	[MA_Ort] [NVARCHAR](255) NULL,
	[MA_Beruf] [NVARCHAR](1000) NULL,
	[MA_Branche] [NVARCHAR](1000) NULL,
	[MASex] [NVARCHAR](10) NULL,
	[MA_EMail] [NVARCHAR](255) NULL,
	[MA_GebDat] [DATETIME] NULL,
	[MA_Language] [NVARCHAR](70) NULL,
	[MA_Nationality] [NVARCHAR](255) NULL,
	[BriefAnrede] [NVARCHAR](70) NULL,
	[AGB_WOS] [NVARCHAR](70) NULL,
	[Transfered_User] [NVARCHAR](100) NULL,
	[Transfered_On] [DATETIME] NULL,
	[WOS_Guid] [NVARCHAR](50) NULL,
 CONSTRAINT [PK_kandidaten] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Kandidaten_Doc_Online](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[MANr] [INT] NULL,
	[ESNr] [INT] NULL,
	[ESLohnNr] [INT] NULL,
	[LONr] [INT] NULL,
	[LogedUser_ID] [NVARCHAR](255) NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[Customer_Name] [NVARCHAR](255) NULL,
	[Customer_Strasse] [NVARCHAR](255) NULL,
	[Customer_Ort] [NVARCHAR](255) NULL,
	[Customer_Telefon] [NVARCHAR](255) NULL,
	[Customer_eMail] [NVARCHAR](255) NULL,
	[Berater] [NVARCHAR](255) NULL,
	[MA_Vorname] [NVARCHAR](255) NULL,
	[MA_Nachname] [NVARCHAR](255) NULL,
	[MA_Filiale] [NVARCHAR](255) NULL,
	[MA_Kanton] [NVARCHAR](255) NULL,
	[MA_Ort] [NVARCHAR](255) NULL,
	[MASex] [NVARCHAR](10) NULL,
	[MAZivil] [NVARCHAR](50) NULL,
	[BriefAnrede] [NVARCHAR](70) NULL,
	[AGB_WOS] [NVARCHAR](70) NULL,
	[MA_Beruf] [NVARCHAR](1000) NULL,
	[MA_Branche] [NVARCHAR](1000) NULL,
	[MA_EMail] [NVARCHAR](255) NULL,
	[MA_GebDat] [DATETIME] NULL,
	[MA_Language] [NVARCHAR](70) NULL,
	[MA_Nationality] [NVARCHAR](255) NULL,
	[Transfered_User] [NVARCHAR](100) NULL,
	[Transfered_On] [DATETIME] NULL,
	[Owner_Guid] [NVARCHAR](100) NULL,
	[Doc_Guid] [NVARCHAR](100) NULL,
	[Doc_Art] [NVARCHAR](255) NULL,
	[Doc_Info] [NVARCHAR](255) NULL,
	[Result] [NVARCHAR](10) NULL,
	[User_Nachname] [NVARCHAR](70) NULL,
	[User_Vorname] [NVARCHAR](70) NULL,
	[User_Telefon] [NVARCHAR](70) NULL,
	[User_Telefax] [NVARCHAR](70) NULL,
	[User_eMail] [NVARCHAR](70) NULL,
	[DocFileName] [NVARCHAR](255) NULL,
	[DocScan] [VARBINARY](MAX) NULL,
	[LastNotification] [DATETIME] NULL,
	[RPNr] [INT] NULL,
	[RPLNr] [INT] NULL,
	[RPDocNr] [INT] NULL,
	[WOS_Guid] [NVARCHAR](50) NULL,
 CONSTRAINT [PK_Kandidaten_Doc_Online] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[Kandidaten_Online](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[MANr] [INT] NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[Customer_Name] [NVARCHAR](255) NULL,
	[Customer_Strasse] [NVARCHAR](255) NULL,
	[Customer_Ort] [NVARCHAR](255) NULL,
	[Customer_Telefon] [NVARCHAR](255) NULL,
	[Customer_eMail] [NVARCHAR](255) NULL,
	[Berater] [NVARCHAR](255) NULL,
	[MA_Vorname] [NVARCHAR](255) NULL,
	[MA_Nachname] [NVARCHAR](255) NULL,
	[MA_Filiale] [NVARCHAR](50) NULL,
	[MA_Kanton] [NVARCHAR](255) NULL,
	[MA_Ort] [NVARCHAR](255) NULL,
	[MA_Kontakt] [NVARCHAR](50) NULL,
	[MA_State1] [NVARCHAR](50) NULL,
	[MA_State2] [NVARCHAR](50) NULL,
	[MA_Beruf] [NVARCHAR](1000) NULL,
	[JobProzent] [NVARCHAR](255) NULL,
	[MAGebDat] [DATETIME] NULL,
	[MASex] [NVARCHAR](10) NULL,
	[MAZivil] [NVARCHAR](50) NULL,
	[MAFSchein] [NVARCHAR](50) NULL,
	[MAAuto] [NVARCHAR](50) NULL,
	[MANationality] [NVARCHAR](70) NULL,
	[Bewillig] [NVARCHAR](50) NULL,
	[BriefAnrede] [NVARCHAR](70) NULL,
	[MA_Res1] [NVARCHAR](255) NULL,
	[MA_Res2] [NVARCHAR](255) NULL,
	[MA_Res3] [NVARCHAR](255) NULL,
	[MA_Res4] [NVARCHAR](255) NULL,
	[Transfered_User] [NVARCHAR](100) NULL,
	[Transfered_On] [DATETIME] NULL,
	[Transfered_Guid] [NVARCHAR](100) NULL,
	[Result] [NVARCHAR](10) NULL,
	[User_Nachname] [NVARCHAR](70) NULL,
	[User_Vorname] [NVARCHAR](70) NULL,
	[User_Telefon] [NVARCHAR](70) NULL,
	[User_Telefax] [NVARCHAR](70) NULL,
	[User_eMail] [NVARCHAR](255) NULL,
	[MA_SSprache] [NVARCHAR](4000) NULL,
	[MA_MSprache] [NVARCHAR](4000) NULL,
	[MA_Eigenschaft] [NVARCHAR](4000) NULL,
	[MA_Res5] [NVARCHAR](255) NULL,
	[Advisor_ID] [NVARCHAR](50) NULL,
	[AssignedCustomer_ID] [NVARCHAR](50) NULL,
	[Branches] [NVARCHAR](MAX) NULL,
	[WOS_Guid] [NVARCHAR](50) NULL,
	[Mainlanguage] [NVARCHAR](70) NULL,
	[ShowAsAvailable] [BIT] NULL,
	[MA_PLZ] [NVARCHAR](50) NULL,
	[DesiredWagesOld] [DECIMAL](8, 2) NULL,
	[DesiredWagesNew] [DECIMAL](8, 2) NULL,
	[DesiredWagesInMonth] [DECIMAL](8, 2) NULL,
	[DesiredWagesInHour] [DECIMAL](8, 2) NULL,
 CONSTRAINT [PK_Kandidaten_Online] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[KD_Vakanzen](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[VakNr] [INT] NULL,
	[KDNr] [INT] NULL,
	[KDZHDNr] [INT] NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[Customer_Name] [NVARCHAR](255) NULL,
	[Customer_Strasse] [NVARCHAR](255) NULL,
	[Customer_Ort] [NVARCHAR](255) NULL,
	[Customer_Telefon] [NVARCHAR](255) NULL,
	[Customer_eMail] [NVARCHAR](255) NULL,
	[Berater] [NVARCHAR](20) NULL,
	[Filiale] [NVARCHAR](50) NULL,
	[VakKontakt] [NVARCHAR](255) NULL,
	[VakState] [NVARCHAR](50) NULL,
	[Bezeichnung] [NVARCHAR](255) NULL,
	[Slogan] [NVARCHAR](255) NULL,
	[Gruppe] [NVARCHAR](255) NULL,
	[ExistLink] [BIT] NULL,
	[VakLink] [NVARCHAR](255) NULL,
	[Beginn] [NVARCHAR](255) NULL,
	[JobProzent] [NVARCHAR](255) NULL,
	[Anstellung] [NVARCHAR](100) NULL,
	[Dauer] [NVARCHAR](255) NULL,
	[MAAge] [NVARCHAR](100) NULL,
	[MASex] [NVARCHAR](10) NULL,
	[MAZivil] [NVARCHAR](50) NULL,
	[MALohn] [NVARCHAR](70) NULL,
	[Jobtime] [NVARCHAR](100) NULL,
	[JobOrt] [NVARCHAR](70) NULL,
	[MAFSchein] [NVARCHAR](50) NULL,
	[MAAuto] [NVARCHAR](50) NULL,
	[MANationality] [NVARCHAR](70) NULL,
	[IEExport] [BIT] NULL,
	[KDBeschreibung] [NVARCHAR](4000) NULL,
	[KDBietet] [NVARCHAR](4000) NULL,
	[SBeschreibung] [NVARCHAR](4000) NULL,
	[Reserve1] [NVARCHAR](4000) NULL,
	[Taetigkeit] [NVARCHAR](4000) NULL,
	[Anforderung] [NVARCHAR](4000) NULL,
	[Reserve2] [NVARCHAR](4000) NULL,
	[Reserve3] [NVARCHAR](4000) NULL,
	[Ausbildung] [NVARCHAR](4000) NULL,
	[Weiterbildung] [NVARCHAR](4000) NULL,
	[SKennt] [NVARCHAR](4000) NULL,
	[EDVKennt] [NVARCHAR](4000) NULL,
	[Branchen] [NVARCHAR](4000) NULL,
	[CreatedOn] [SMALLDATETIME] NULL,
	[CreatedFrom] [NVARCHAR](100) NULL,
	[ChangedOn] [SMALLDATETIME] NULL,
	[ChangedFrom] [NVARCHAR](100) NULL,
	[Transfered_User] [NVARCHAR](100) NULL,
	[Transfered_On] [DATETIME] NULL,
	[Transfered_Guid] [NVARCHAR](100) NULL,
	[Result] [NVARCHAR](10) NULL,
	[Vak_Region] [NVARCHAR](255) NULL,
	[Vak_Kanton] [NVARCHAR](255) NULL,
	[User_Nachname_OLD] [NVARCHAR](70) NULL,
	[User_Vorname_OLD] [NVARCHAR](70) NULL,
	[User_Telefon_OLD] [NVARCHAR](70) NULL,
	[User_Telefax_OLD] [NVARCHAR](70) NULL,
	[User_eMail_OLD] [NVARCHAR](70) NULL,
	[MSprachen] [NVARCHAR](4000) NULL,
	[SSprachen] [NVARCHAR](4000) NULL,
	[Qualifikation] [NVARCHAR](4000) NULL,
	[SQualifikation] [NVARCHAR](4000) NULL,
	[User_Guid] [NVARCHAR](50) NULL,
	[_KDBeschreibung] [NVARCHAR](4000) NULL,
	[_Taetigkeit] [NVARCHAR](4000) NULL,
	[_Anforderung] [NVARCHAR](4000) NULL,
	[_KDBietet] [NVARCHAR](4000) NULL,
	[JobPLZ] [NVARCHAR](10) NULL,
	[_Reserve1] [NVARCHAR](4000) NULL,
	[_Reserve2] [NVARCHAR](4000) NULL,
	[_Reserve3] [NVARCHAR](4000) NULL,
	[_Weiterbildung] [NVARCHAR](4000) NULL,
	[_SBeschreibung] [NVARCHAR](4000) NULL,
	[_SKennt] [NVARCHAR](4000) NULL,
	[_EDVKennt] [NVARCHAR](4000) NULL,
	[_Ausbildung] [NVARCHAR](4000) NULL,
	[Job_Categories] [NVARCHAR](4000) NULL,
	[Job_Disciplines] [NVARCHAR](4000) NULL,
	[Job_Position] [NVARCHAR](4000) NULL,
	[TitelForSearch] [NVARCHAR](255) NULL,
	[ShortDescription] [NVARCHAR](255) NULL,
	[SBNNumber] [INT] NULL,
	[SubGroup] [NVARCHAR](255) NULL,
	[WOS_Guid] [NVARCHAR](50) NULL,
	[JobChannelPriority] [BIT] NULL,
 CONSTRAINT [PK_KD_Vakanzen] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[Kunden](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[KDNr] [INT] NULL,
	[KD_Guid] [NVARCHAR](100) NULL,
	[KD_Name] [NVARCHAR](255) NULL,
	[KD_Berater] [NVARCHAR](255) NULL,
	[KD_Kanton] [NVARCHAR](50) NULL,
	[KD_Ort] [NVARCHAR](255) NULL,
	[KD_Beruf] [NVARCHAR](1000) NULL,
	[KD_Branche] [NVARCHAR](1000) NULL,
	[KD_eMail] [NVARCHAR](255) NULL,
	[KD_Language] [NVARCHAR](70) NULL,
	[KD_AGB_WOS] [NVARCHAR](70) NULL,
	[ZHDNr] [INT] NULL,
	[ZHD_Guid] [NVARCHAR](100) NULL,
	[ZHD_Vorname] [NVARCHAR](255) NULL,
	[ZHD_Nachname] [NVARCHAR](255) NULL,
	[Zhd_Beruf] [NVARCHAR](1000) NULL,
	[Zhd_Branche] [NVARCHAR](1000) NULL,
	[ZHDSex] [NVARCHAR](10) NULL,
	[Zhd_GebDat] [DATETIME] NULL,
	[Zhd_BriefAnrede] [NVARCHAR](70) NULL,
	[ZHD_eMail] [NVARCHAR](70) NULL,
	[Zhd_Berater] [NVARCHAR](255) NULL,
	[ZHD_AGB_WOS] [NVARCHAR](70) NULL,
	[Transfered_User] [NVARCHAR](100) NULL,
	[Transfered_On] [DATETIME] NULL,
	[EnableContractLink] [BIT] NULL,
	[WOS_Guid] [NVARCHAR](50) NULL,
 CONSTRAINT [PK_Kunden] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Kunden_Doc_Online](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[KDNr] [INT] NULL,
	[ZHDNr] [INT] NULL,
	[ESNr] [INT] NULL,
	[ESLohnNr] [INT] NULL,
	[RPNr] [INT] NULL,
	[RENr] [INT] NULL,
	[LogedUser_ID] [NVARCHAR](255) NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[KD_Name] [NVARCHAR](255) NULL,
	[ZHD_Vorname] [NVARCHAR](255) NULL,
	[ZHD_Nachname] [NVARCHAR](255) NULL,
	[KD_Filiale] [NVARCHAR](255) NULL,
	[KD_Kanton] [NVARCHAR](255) NULL,
	[KD_Ort] [NVARCHAR](255) NULL,
	[KD_AGB_WOS] [NVARCHAR](70) NULL,
	[ZHDSex] [NVARCHAR](10) NULL,
	[ZHD_BriefAnrede] [NVARCHAR](70) NULL,
	[KD_eMail] [NVARCHAR](255) NULL,
	[ZHD_eMail] [NVARCHAR](255) NULL,
	[KD_Guid] [NVARCHAR](100) NULL,
	[ZHD_Guid] [NVARCHAR](100) NULL,
	[Doc_Guid] [NVARCHAR](100) NULL,
	[Doc_Art] [NVARCHAR](255) NULL,
	[Doc_Info] [NVARCHAR](255) NULL,
	[Result] [NVARCHAR](10) NULL,
	[KD_Berater] [NVARCHAR](255) NULL,
	[ZHD_Berater] [NVARCHAR](255) NULL,
	[KD_Beruf] [NVARCHAR](1000) NULL,
	[KD_Branche] [NVARCHAR](1000) NULL,
	[ZHD_Beruf] [NVARCHAR](1000) NULL,
	[ZHD_Branche] [NVARCHAR](1000) NULL,
	[ZHD_AGB_WOS] [NVARCHAR](70) NULL,
	[ZHD_GebDat] [DATETIME] NULL,
	[Transfered_User] [NVARCHAR](100) NULL,
	[Transfered_On] [DATETIME] NULL,
	[User_Nachname] [NVARCHAR](70) NULL,
	[User_Vorname] [NVARCHAR](70) NULL,
	[User_Telefon] [NVARCHAR](70) NULL,
	[User_Telefax] [NVARCHAR](70) NULL,
	[User_eMail] [NVARCHAR](255) NULL,
	[GetResult] [TINYINT] NULL,
	[Get_On] [DATETIME] NULL,
	[LastNotification] [DATETIME] NULL,
	[KD_Language] [NVARCHAR](70) NULL,
	[DocFileName] [NVARCHAR](255) NULL,
	[DocScan] [VARBINARY](MAX) NULL,
	[ProposeNr] [INT] NULL,
	[WOS_Guid] [NVARCHAR](50) NULL,
	[FK_StateID] [INT] NULL,
 CONSTRAINT [PK_Kunden_Doc_Online] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[Kunden_ZHD](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[KDNr] [INT] NULL,
	[KD_Guid] [NVARCHAR](100) NULL,
	[ZHDNr] [INT] NULL,
	[ZHD_Guid] [NVARCHAR](100) NULL,
	[ZHD_Vorname] [NVARCHAR](255) NULL,
	[ZHD_Nachname] [NVARCHAR](255) NULL,
	[Zhd_Beruf] [NVARCHAR](1000) NULL,
	[Zhd_Branche] [NVARCHAR](1000) NULL,
	[ZHDSex] [NVARCHAR](10) NULL,
	[Zhd_GebDat] [DATETIME] NULL,
	[Zhd_BriefAnrede] [NVARCHAR](70) NULL,
	[ZHD_eMail] [NVARCHAR](70) NULL,
	[Zhd_Berater] [NVARCHAR](255) NULL,
	[ZHD_AGB_WOS] [NVARCHAR](70) NULL,
	[Transfered_User] [NVARCHAR](100) NULL,
	[Transfered_On] [DATETIME] NULL,
	[WOS_Guid] [NVARCHAR](50) NULL,
 CONSTRAINT [PK_Kunden_ZHD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[MailNotification](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[Customer_Name] [NVARCHAR](255) NULL,
	[Customer_Ort] [NVARCHAR](255) NULL,
	[MailFrom] [NVARCHAR](255) NULL,
	[MailTo] [NVARCHAR](255) NULL,
	[Result] [NVARCHAR](255) NULL,
	[CreatedOn] [DATETIME] NULL,
	[SUBJECT] [NVARCHAR](500) NULL,
	[Body] [NVARCHAR](4000) NULL,
	[DocLink] [NVARCHAR](500) NULL,
	[Recipient_Guid] [NVARCHAR](50) NULL,
 CONSTRAINT [PK_MailNotification] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[SP_ModulUsage](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[ModulName] [NVARCHAR](50) NULL,
	[ModulVersion] [NVARCHAR](255) NULL,
	[UserID] [NVARCHAR](255) NULL,
	[Answer] [NVARCHAR](4000) NULL,
	[RequestParam] [NVARCHAR](4000) NULL,
	[IsWebService] [BIT] NULL,
	[CreatedOn] [DATETIME] NULL,
 CONSTRAINT [PK_SP_ModulUsage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[Tab_ModulUsage](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[ModulName] [NVARCHAR](50) NULL,
	[UseCount] [INT] NULL,
	[UseDate] [DATETIME] NULL,
	[MachineID] [NVARCHAR](255) NULL,
	[ModulParameter] [NVARCHAR](4000) NULL,
	[IsWebService] [BIT] NULL,
 CONSTRAINT [PK_Tab_ModulUsage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Available_Employee_ApplicationData](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[WOS_Guid] [NVARCHAR](255) NULL,
	[EmployeeNr] [INT] NULL,
	[Employee_Guid] [NVARCHAR](255) NULL,
	[LL_Name] [NVARCHAR](255) NULL,
	[ApplicationReserve0] [NVARCHAR](MAX) NULL,
	[ApplicationReserve1] [NVARCHAR](MAX) NULL,
	[ApplicationReserve2] [NVARCHAR](MAX) NULL,
	[ApplicationReserve3] [NVARCHAR](MAX) NULL,
	[ApplicationReserve4] [NVARCHAR](MAX) NULL,
	[ApplicationReserve5] [NVARCHAR](MAX) NULL,
	[ApplicationReserve6] [NVARCHAR](MAX) NULL,
	[ApplicationReserve7] [NVARCHAR](MAX) NULL,
	[ApplicationReserve8] [NVARCHAR](MAX) NULL,
	[ApplicationReserve9] [NVARCHAR](MAX) NULL,
	[ApplicationReserve10] [NVARCHAR](MAX) NULL,
	[ApplicationReserve11] [NVARCHAR](MAX) NULL,
	[ApplicationReserve12] [NVARCHAR](MAX) NULL,
	[ApplicationReserve13] [NVARCHAR](MAX) NULL,
	[ApplicationReserve14] [NVARCHAR](MAX) NULL,
	[ApplicationReserve15] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf0] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf1] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf2] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf3] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf4] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf5] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf6] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf7] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf8] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf9] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf10] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf11] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf12] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf13] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf14] [NVARCHAR](MAX) NULL,
	[ApplicationReserveRtf15] [NVARCHAR](MAX) NULL,
	[CreatedOn] [DATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
 CONSTRAINT [PK_tbl_Available_Employee_ApplicationData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Customer_WOSDocument_State](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[WOS_ID] [NVARCHAR](255) NULL,
	[EmployeeNr] [INT] NULL,
	[CustomerNr] [INT] NULL,
	[ZHDNr] [INT] NULL,
	[ProposeNr] [INT] NULL,
	[ESNr] [INT] NULL,
	[ESLohnNr] [INT] NULL,
	[ReportNr] [INT] NULL,
	[InvoiceNr] [INT] NULL,
	[KD_Guid] [NVARCHAR](100) NULL,
	[ZHD_Guid] [NVARCHAR](100) NULL,
	[Doc_Guid] [NVARCHAR](100) NULL,
	[Doc_Art] [NVARCHAR](255) NULL,
	[Doc_Info] [NVARCHAR](255) NULL,
	[Employee_Advisor] [NVARCHAR](255) NULL,
	[Customer_Advisor] [NVARCHAR](255) NULL,
	[ZHD_Advisor] [NVARCHAR](255) NULL,
	[Transfered_User] [NVARCHAR](100) NULL,
	[Transfered_On] [DATETIME] NULL,
	[GetResult] [INT] NULL,
	[Get_On] [DATETIME] NULL,
	[ViewedResult] [INT] NULL,
	[Viewed_On] [DATETIME] NULL,
	[LastNotification] [DATETIME] NULL,
	[Customer_Feedback] [NVARCHAR](MAX) NULL,
	[Customer_Feedback_On] [DATETIME] NULL,
	[NotifyAdvisor] [BIT] NULL,
 CONSTRAINT [PK_tbl_Customer_WOSDocument_State] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Employee_Online_Template_Document](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[WOS_Guid] [NVARCHAR](255) NULL,
	[EmployeeNr] [INT] NULL,
	[ScanDoc] [VARBINARY](MAX) NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
	[CreatedOn] [DATETIME] NULL,
	[GetResult] [INT] NULL,
	[Get_On] [DATETIME] NULL,
	[ViewedResult] [INT] NULL,
	[Viewed_On] [DATETIME] NULL,
 CONSTRAINT [PK_tbl_Employee_Online_Template_Document] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tblJobCHPlattform](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_Guid] [NVARCHAR](50) NULL,
	[User_Guid] [NVARCHAR](50) NULL,
	[VakNr] [INT] NULL,
	[OrganisationID] [INT] NULL,
	[InseratID] [INT] NULL,
	[Vorspann] [NVARCHAR](4000) NULL,
	[Beruf] [NVARCHAR](255) NULL,
	[Text_Taetigkeit] [NVARCHAR](4000) NULL,
	[Text_Anforderung] [NVARCHAR](4000) NULL,
	[Text_WirBieten] [NVARCHAR](4000) NULL,
	[PLZ] [NVARCHAR](10) NULL,
	[Ort] [NVARCHAR](255) NULL,
	[Kontakt] [NVARCHAR](255) NULL,
	[EMail] [NVARCHAR](50) NULL,
	[URL] [NVARCHAR](100) NULL,
	[Direkt_URL] [NVARCHAR](255) NULL,
	[Direkt_URL_Post_Args] [NVARCHAR](255) NULL,
	[StartDate] [DATETIME] NULL,
	[EndDate] [DATETIME] NULL,
	[Titel] [NVARCHAR](255) NULL,
	[Anriss] [NVARCHAR](255) NULL,
	[Firma] [NVARCHAR](255) NULL,
	[Anstellungsart] [NVARCHAR](255) NULL,
	[Our_URL] [NVARCHAR](255) NULL,
	[Anstellungsgrad_Von_Bis] [NVARCHAR](50) NULL,
	[RubrikID] [NVARCHAR](500) NULL,
	[Position] [NVARCHAR](255) NULL,
	[Branche] [NVARCHAR](255) NULL,
	[Sprache] [NVARCHAR](2) NULL,
	[Region] [NVARCHAR](255) NULL,
	[Alter_Von_Bis] [NVARCHAR](50) NULL,
	[Sprachkenntniss_Kandidat] [NVARCHAR](255) NULL,
	[Sprachkenntniss_Niveau] [NVARCHAR](255) NULL,
	[Bildungsniveau] [NVARCHAR](255) NULL,
	[Berufserfahrung] [NVARCHAR](255) NULL,
	[Berufserfahrung_Position] [NVARCHAR](255) NULL,
	[Lyout] [INT] NULL,
	[Logo] [INT] NULL,
	[Bewerben_URL] [NVARCHAR](255) NULL,
	[Angebot] [INT] NULL,
	[Xing_Poster_URL] [NVARCHAR](255) NULL,
	[Xing_Company_Profile_URL] [NVARCHAR](255) NULL,
	[Xing_Company_Is_Poc] [BIT] NULL,
	[CreatedOn] [DATETIME] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tblOstJobCHPlattform](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_Guid] [NVARCHAR](50) NULL,
	[User_Guid] [NVARCHAR](50) NULL,
	[VakNr] [INT] NULL,
	[JobVersion] [NVARCHAR](1) NULL,
	[Company] [NVARCHAR](255) NULL,
	[Title] [NVARCHAR](255) NULL,
	[Workplace_Country] [NVARCHAR](255) NULL,
	[Workplace_Zip] [NVARCHAR](50) NULL,
	[Workplace_City] [NVARCHAR](255) NULL,
	[Company_Description] [NVARCHAR](4000) NULL,
	[WirBieten] [NVARCHAR](4000) NULL,
	[Anforderungen] [NVARCHAR](4000) NULL,
	[Aufgabe] [NVARCHAR](4000) NULL,
	[Contact] [NVARCHAR](4000) NULL,
	[Description_url] [NVARCHAR](255) NULL,
	[Application_url] [NVARCHAR](255) NULL,
	[Publication_ostjob_ch] [BIT] NULL,
	[Publication_westjob_at] [BIT] NULL,
	[Publication_nicejob_de] [BIT] NULL,
	[Publication_zentraljob_ch] [BIT] NULL,
	[Publication_minisite] [BIT] NULL,
	[Apprenticeship] [BIT] NULL,
	[Template] [NVARCHAR](50) NULL,
	[CreatedOn] [DATETIME] NULL,
	[IsOnline] [BIT] NULL,
	[StartDate] [DATETIME] NULL,
	[EndDate] [DATETIME] NULL,
	[KeyWords] [NVARCHAR](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tblVacancyDisciplinesMatch](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_Guid] [NVARCHAR](50) NULL,
	[DisciplinesJobs] [NVARCHAR](255) NULL,
	[DisciplinesMatch] [NVARCHAR](255) NULL,
	[CreatedOn] [DATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tblVacancyJobExperience](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_Guid] [NVARCHAR](50) NULL,
	[VakNr] [INT] NULL,
	[Berufgruppe] [NVARCHAR](255) NULL,
	[BerufErfahrung] [NVARCHAR](255) NULL,
	[BerufPosition] [NVARCHAR](255) NULL,
	[CreatedOn] [DATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
	[WOS_Guid] [NVARCHAR](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tblVacancyRegionMatch](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_Guid] [NVARCHAR](50) NULL,
	[RegionJobs] [NVARCHAR](255) NULL,
	[RegionMatch] [NVARCHAR](255) NULL,
	[CreatedOn] [DATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



/* ------------------- drop and create functions ------------------------------*/

BEGIN TRY DROP FUNCTION [dbo].[SplitMyString] END TRY BEGIN CATCH END CATCH;
Go
CREATE FUNCTION [dbo].[SplitMyString]
(
    @StringToSplit VARCHAR(2048),
    @Separator VARCHAR(128)
)
RETURNS TABLE
AS
RETURN WITH indices
       AS (SELECT 0 S,
                  1 E
           UNION ALL
           SELECT E,
                  CHARINDEX(@Separator, @StringToSplit, E) + LEN(@Separator)
           FROM indices
           WHERE E > S)
SELECT SUBSTRING(   @StringToSplit,
                    S,
                    CASE
                        WHEN E > LEN(@Separator) THEN
                            E - S - LEN(@Separator)
                        ELSE
                            LEN(@StringToSplit) - S + 1
                    END
                ) String,
       S StartIndex
FROM indices
WHERE S > 0;
GO


BEGIN TRY DROP FUNCTION [dbo].[Get JobDisciplines for each VacNumber] END TRY BEGIN CATCH END CATCH;
Go
CREATE FUNCTION  [dbo].[Get JobDisciplines for each VacNumber]
(
	-- Add the parameters for the function here
	@vaknr INT,
	@customer_ID NVARCHAR(50)
)
RETURNS NVARCHAR(4000)
AS
BEGIN

DECLARE @result NVARCHAR(4000) = ''
DECLARE	@JobDisipline nvarchar(4000) = ''
--SELECT * FROM _vak

SELECT @JobDisipline = ISNULL(@JobDisipline + '#', '')
                + dbo.tblVacancyJobExperience.BerufErfahrung
        FROM    dbo.tblVacancyJobExperience
		WHERE Vaknr = @vaknr AND Customer_Guid = @customer_ID

        SET @JobDisipline = ISNULL(@JobDisipline, '')
		SET @result = @JobDisipline

	return @result

END
GO


BEGIN TRY DROP FUNCTION [dbo].[Get JobDisciplinesMatch for each VacNumber] END TRY BEGIN CATCH END CATCH;
Go
CREATE FUNCTION [dbo].[Get JobDisciplinesMatch for each VacNumber]
(
	@vaknr INT,
	@customer_ID NVARCHAR(50)
)
RETURNS NVARCHAR(4000)
AS
BEGIN

DECLARE @result NVARCHAR(4000) = ''
DECLARE	@JobDiscipline nvarchar(4000) = ''
DECLARE	@JobDisciplineMatch nvarchar(4000) = ''
DECLARE @JobDiscipline1 NVARCHAR(2000) = ''
DECLARE @JobDiscipline2 NVARCHAR(2000) = ''
DECLARE @JobDiscipline1Match NVARCHAR(2000) = ''
DECLARE @JobDiscipline2Match NVARCHAR(2000) = ''
DECLARE @Matching INT


SELECT @JobDiscipline = ISNULL(@JobDiscipline + '#', '')
                + dbo.tblVacancyJobExperience.BerufErfahrung
        FROM    dbo.tblVacancyJobExperience
		WHERE Vaknr = @vaknr AND Customer_Guid = @customer_ID

        SET @JobDiscipline = ISNULL(@JobDiscipline, '')
		SET @result = @JobDiscipline

SET @JobDiscipline = SUBSTRING(@JobDiscipline, 2, 2000)

SET @Matching = (SELECT COUNT(*) FROM tblVacancyDisciplinesMatch WHERE Customer_Guid = @customer_ID)

SET  @JobDiscipline1 = CASE 
	WHEN @Matching >0 THEN
		CASE 
				WHEN 
					Patindex('%#%',@JobDiscipline) > 0
				THEN
					Left(@JobDiscipline, Patindex('%#%', @JobDiscipline) - 1)
			ELSE
				@JobDiscipline
		END
	ELSE ''
END 


SET @JobDiscipline2= CASE 
	WHEN @Matching >0 THEN
		CASE 
				WHEN 
					Patindex('%#%',@JobDiscipline) > 0
				THEN
					Right(@JobDiscipline, Len(@JobDiscipline) - Patindex('%#%', @JobDiscipline))
			ELSE
				''
		END
	ELSE ''
END 

SELECT @JobDiscipline1Match = vdm.DisciplinesMatch
	FROM dbo.tblVacancyDisciplinesMatch vdm
	WHERE vdm.DisciplinesJobs LIKE @JobDiscipline1 AND vdm.Customer_Guid = @customer_ID

SELECT @JobDiscipline2Match = vdm.DisciplinesMatch
	FROM dbo.tblVacancyDisciplinesMatch vdm
	WHERE vdm.DisciplinesJobs LIKE @JobDiscipline2 AND vdm.Customer_Guid = @customer_ID

SET @JobDisciplineMatch = CASE
		WHEN
			((@JobDiscipline1Match <> @JobDiscipline2Match) AND (@JobDiscipline2Match <> ''))
		THEN      
			@JobDiscipline1Match ++ '#' ++ @JobDiscipline2Match
		ELSE
			@JobDiscipline1Match      
	END  

SET @result = @JobDisciplineMatch

	return @result

END
GO


BEGIN TRY DROP FUNCTION [dbo].[Get JobRegionMatch for each VacNumber] END TRY BEGIN CATCH END CATCH;
Go
CREATE FUNCTION [dbo].[Get JobRegionMatch for each VacNumber]
(
	@vaknr INT,
	@Vak_Region NVARCHAR(250),
	@customer_ID NVARCHAR(50)
)
RETURNS NVARCHAR(4000)
AS
BEGIN

DECLARE @result NVARCHAR(4000) = ''
DECLARE	@VakRegionMatch nvarchar(4000) = ''
DECLARE @VakRegion1 NVARCHAR(2000) = ''
DECLARE @VakRegion2 NVARCHAR(2000) = ''
DECLARE @VakRegion1Match NVARCHAR(2000) = ''
DECLARE @VakRegion2Match NVARCHAR(2000) = ''
DECLARE @Matching INT

SET @Matching = (SELECT COUNT(*) FROM tblVacancyRegionMatch WHERE Customer_Guid = @customer_ID)

SET @VakRegion1 = CASE 
	WHEN @Matching >0 THEN
		CASE 
				WHEN 
					Patindex('%#%',@Vak_Region) > 0
				THEN
					Left(@Vak_Region, Patindex('%#%', @Vak_Region) - 1)
			ELSE
				@Vak_Region
		END
	ELSE ''
END 


SET @VakRegion2 = CASE 
	WHEN @Matching >0 THEN
		CASE 
				WHEN 
					Patindex('%#%',@Vak_Region) > 0
				THEN
					Right(@Vak_Region, Len(@Vak_Region) - Patindex('%#%', @Vak_Region))
			ELSE
				''
		END
	ELSE ''
END 

SELECT @VakRegion1Match = vrm.RegionMatch
	FROM dbo.tblVacancyRegionMatch vrm
	WHERE vrm.RegionJobs LIKE @VakRegion1 AND vrm.Customer_Guid = @customer_ID

SELECT @VakRegion2Match = vrm.RegionMatch
	FROM dbo.tblVacancyRegionMatch vrm
	WHERE vrm.RegionJobs LIKE @VakRegion2 AND vrm.Customer_Guid = @customer_ID

SET @VakRegionMatch = CASE
		WHEN
			((@VakRegion1Match <> @VakRegion2Match) AND (@VakRegion2Match <> ''))
		THEN      
			@VakRegion1Match ++ '#' ++ @VakRegion2Match
		ELSE
			@VakRegion1Match      
	END  

SET @result = @VakRegionMatch

RETURN @result
				
END
GO


BEGIN TRY DROP FUNCTION [dbo].[ufsReadfileAsString] END TRY BEGIN CATCH END CATCH;
Go
CREATE FUNCTION [dbo].[ufsReadfileAsString]
(
@Path VARCHAR(255),
@Filename VARCHAR(100)
)
RETURNS 
 Varchar(max)
AS
BEGIN

DECLARE  @objFileSystem int
        ,@objTextStream int,
		@objErrorObject int,
		@strErrorMessage Varchar(1000),
	    @Command varchar(1000),
		@Chunk Varchar(8000),
		@String varchar(max),
	    @hr int,
		@YesOrNo int

Select @String=''
select @strErrorMessage='opening the File System Object'
EXECUTE @hr = sp_OACreate  'Scripting.FileSystemObject' , @objFileSystem OUT


if @HR=0 Select @objErrorObject=@objFileSystem, @strErrorMessage='Opening file "'+@path+'\'+@filename+'"',@command=@path+'\'+@filename

if @HR=0 execute @hr = sp_OAMethod   @objFileSystem  , 'OpenTextFile'
	, @objTextStream OUT, @command,1,false,0--for reading, FormatASCII

WHILE @hr=0
	BEGIN
	if @HR=0 Select @objErrorObject=@objTextStream, 
		@strErrorMessage='finding out if there is more to read in "'+@filename+'"'
	if @HR=0 execute @hr = sp_OAGetProperty @objTextStream, 'AtEndOfStream', @YesOrNo OUTPUT

	IF @YesOrNo<>0  break
	if @HR=0 Select @objErrorObject=@objTextStream, 
		@strErrorMessage='reading from the output file "'+@filename+'"'
	if @HR=0 execute @hr = sp_OAMethod  @objTextStream, 'Read', @chunk OUTPUT,4000
	SELECT @String=@string+@chunk
	end
if @HR=0 Select @objErrorObject=@objTextStream, 
	@strErrorMessage='closing the output file "'+@filename+'"'
if @HR=0 execute @hr = sp_OAMethod  @objTextStream, 'Close'


if @hr<>0
	begin
	Declare 
		@Source varchar(255),
		@Description Varchar(255),
		@Helpfile Varchar(255),
		@HelpID int
	
	EXECUTE sp_OAGetErrorInfo  @objErrorObject, 
		@source output,@Description output,@Helpfile output,@HelpID output
	Select @strErrorMessage='Error whilst '
			+coalesce(@strErrorMessage,'doing something')
			+', '+coalesce(@Description,'')
	select @String=@strErrorMessage
	end
EXECUTE  sp_OADestroy @objTextStream
	
	RETURN @string
END
GO



/*---------------- create stored procedures --------------------------- */

CREATE PROCEDURE [dbo].[List Vak-Titel]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KD_Vakanzen.Bezeichnung 
	FROM KD_Vakanzen
	WHERE (KD_Vakanzen.WOS_Guid = @UserID)
	And (KD_Vakanzen.Bezeichnung <> '' Or KD_Vakanzen.Bezeichnung Is Not Null)
	Group By KD_Vakanzen.Bezeichnung
	ORDER BY KD_Vakanzen.Bezeichnung

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Vak-Region]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KD_Vakanzen.Vak_Region 
	FROM KD_Vakanzen
	WHERE (KD_Vakanzen.WOS_Guid = @UserID)
	And (KD_Vakanzen.Vak_Region <> '' Or KD_Vakanzen.Vak_Region Is Not Null)
	Group By KD_Vakanzen.Vak_Region
	ORDER BY KD_Vakanzen.Vak_Region

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Vak-Filiale]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KD_Vakanzen.Filiale
	FROM KD_Vakanzen
	WHERE (KD_Vakanzen.WOS_Guid = @UserID)
	And (KD_Vakanzen.Filiale <> '' Or KD_Vakanzen.Filiale Is Not Null)
	Group By KD_Vakanzen.Filiale
	ORDER BY KD_Vakanzen.Filiale

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Vak-Ort]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KD_Vakanzen.JobOrt
	FROM KD_Vakanzen
	WHERE (KD_Vakanzen.WOS_Guid = @UserID)
	And (KD_Vakanzen.JobOrt <> '' Or KD_Vakanzen.JobOrt Is Not Null)
	Group By KD_Vakanzen.JobOrt
	ORDER BY KD_Vakanzen.JobOrt

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Get Vakrec]
	@UserID nvarchar(255) = '',
	@Beruf nvarchar(255) = '',
	@Ort nvarchar(255) = '',
	@Kanton nvarchar(255) = '', 
	@Region nvarchar(255) = '', 
	@Filiale nvarchar(255) = '',
	@SortString nvarchar(255) = 'KD_Vakanzen.Transfered_On DESC'

AS
if @SortString = '' 
	begin
	 set @SortString = 'KD_Vakanzen.Transfered_On DESC'
	end
BEGIN
	SET NOCOUNT ON
	Declare @SQL nvarchar(2000)
	
	Set @SQL = 'SELECT KD_Vakanzen.* 
	FROM KD_Vakanzen 
	WHERE (KD_Vakanzen.Customer_ID = ''' + @UserID + ''') 
	And (KD_Vakanzen.Bezeichnung Like ''%' + @Beruf + '%'' Or '''	+ @Beruf +	''' = '''') 
	And (KD_Vakanzen.JobOrt Like ''%' + @Ort  + '%'' Or '''	+ @Ort + ''' = '''') 
	And (KD_Vakanzen.Vak_Kanton = '''	+ @Kanton + ''' Or '''	+ @Kanton + ''' = '''') 
	And (KD_Vakanzen.Vak_Region Like ''%'	+ @Region + '%'' Or '''	+ @Region + ''' = '''') 
	And (KD_Vakanzen.Filiale = ''' + @Filiale + ''' Or ''' + @Filiale + ''' = '''') 
	ORDER BY ' + @Sortstring 
	
	EXEC (@sql)
	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[List Vak-Kanton]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KD_Vakanzen.Vak_Kanton
	FROM KD_Vakanzen
	WHERE (KD_Vakanzen.WOS_Guid = @UserID) 
	And (KD_Vakanzen.Vak_Kanton <> '' Or KD_Vakanzen.Vak_Kanton Is Not Null)
	Group By KD_Vakanzen.Vak_Kanton
	ORDER BY KD_Vakanzen.Vak_Kanton

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected Vak-Rec]
	@UserID nvarchar(255) = '',
	@Transfered_Guid nvarchar(255) = '',
	@VakNr int = 0

AS
BEGIN
	SET NOCOUNT ON
	
	Delete KD_Vakanzen 
	WHERE (KD_Vakanzen.Customer_ID = @UserID)
	And (KD_Vakanzen.Transfered_Guid = @Transfered_Guid)
	And (KD_Vakanzen.VakNr = @VakNr)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Get Vakrec_1]
	@UserID nvarchar(255) = '',
	@Beruf nvarchar(255) = '',
	@Ort nvarchar(255) = '',
	@Kanton nvarchar(255) = '', 
	@Region nvarchar(255) = '', 
	@Filiale nvarchar(255) = '',
	@SortString nvarchar(255) = 'KD_Vakanzen.Transfered_On DESC'

AS
BEGIN
	SET NOCOUNT ON
	Declare @SQL nvarchar(2000)
	
	Set @SQL = 'SELECT KD_Vakanzen.* 
	FROM KD_Vakanzen 
	WHERE (KD_Vakanzen.Customer_ID = ''' + @UserID + ''') 
	And (KD_Vakanzen.Bezeichnung = ''' + @Beruf + ''' Or '''	+ @Beruf +	''' = '''') 
	And (KD_Vakanzen.JobOrt = '''	+ @Ort  + ''' Or '''	+ @Ort +	''' = '''') 
	And (KD_Vakanzen.Vak_Kanton = '''	+ @Kanton + ''' Or '''	+ @Kanton + ''' = '''') 
	And (KD_Vakanzen.Vak_Region = '''	+ @Region + ''' Or '''	+ @Region + ''' = '''') 
	And (KD_Vakanzen.Filiale = ''' + @Filiale + ''' Or ''' + @Filiale + ''' = '''') 
	ORDER BY ' + @Sortstring 
	
	EXEC (@sql)
	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete Selected MA-Rec]
	@UserID nvarchar(255) = '',
	@Transfered_Guid nvarchar(255) = '',
	@MANr int = 0

AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kandidaten_Online 
	WHERE (Kandidaten_Online.Customer_ID = @UserID)
	And (Kandidaten_Online.MANr = @MANr)

	if Not Exists(Select MANr From Kandidaten_Doc_Online Where Customer_ID = @UserID And MANr = @MANr)
	Begin
		Delete Kandidaten WHERE (Kandidaten.Customer_ID = @UserID) And (MANr = @MANr)
	end

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Get MArec]
	@UserID nvarchar(255) = '',
	@Beruf nvarchar(255) = '',
	@Ort nvarchar(255) = '',
	@Kanton nvarchar(255) = '', 
	@Filiale nvarchar(255) = '',
	@SortString nvarchar(255) = 'MA.Transfered_On DESC'

AS
BEGIN
	SET NOCOUNT ON
	Declare @SQL nvarchar(2000)
	
	Set @SQL = 'SELECT MA.* 
	FROM Kandidaten_Online MA
	WHERE (MA.Customer_ID = ''' + @UserID + ''') 
	And (MA.MA_Beruf Like ''%' + @Beruf + '%'' Or '''	+ @Beruf +	''' = '''') 
	And (MA.MA_Ort Like ''%' + @Ort  + '%'' Or '''	+ @Ort + ''' = '''') 
	And (MA.MA_Kanton = '''	+ @Kanton + ''' Or '''	+ @Kanton + ''' = '''') 
	And (MA.MA_Filiale = ''' + @Filiale + ''' Or ''' + @Filiale + ''' = '''') 
	ORDER BY ' + @Sortstring 
	
	EXEC (@sql)
--	select @sql
	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[List MA-Kanton]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT Kandidaten_Online.MA_Kanton
	FROM Kandidaten_Online
	WHERE (Kandidaten_Online.Customer_ID = @UserID) 
	And Kandidaten_Online.MA_Kanton <> '' Or Kandidaten_Online.MA_Kanton Is Not Null
	Group By Kandidaten_Online.MA_Kanton
	ORDER BY Kandidaten_Online.MA_Kanton

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[List MA-Ort]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT Kandidaten_Online.MA_Ort
	FROM Kandidaten_Online
	WHERE (Kandidaten_Online.Customer_ID = @UserID)
	And Kandidaten_Online.MA_Ort <> '' Or Kandidaten_Online.MA_Ort Is Not Null
	Group By Kandidaten_Online.MA_Ort
	ORDER BY Kandidaten_Online.MA_Ort

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create PROCEDURE [dbo].[List MA-Filiale]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT Kandidaten_Online.MA_Filiale
	FROM Kandidaten_Online
	WHERE (Kandidaten_Online.Customer_ID = @UserID)
	And Kandidaten_Online.MA_Filiale <> '' Or Kandidaten_Online.MA_Filiale Is Not Null
	Group By Kandidaten_Online.MA_Filiale
	ORDER BY Kandidaten_Online.MA_Filiale

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[List MA-Berufe]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT Kandidaten_Online.MA_Beruf 
	FROM Kandidaten_Online
	WHERE (Kandidaten_Online.Customer_ID = @UserID)
	And Kandidaten_Online.MA_Beruf <> '' Or Kandidaten_Online.MA_Beruf Is Not Null
	Group By Kandidaten_Online.MA_Beruf
	ORDER BY Kandidaten_Online.MA_Beruf

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected MADoc With DocGuid]
	@UserID nvarchar(255) = '',
	@MA_Guid nvarchar(255) = '',
	@MANr int = 0,
	@Doc_Guid nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kandidaten_Doc_Online 
	WHERE (Kandidaten_Doc_Online.Customer_ID = @UserID)
	And (Kandidaten_Doc_Online.Owner_Guid = @MA_Guid)
	And (Kandidaten_Doc_Online.MANr = @MANr)
	And (Kandidaten_Doc_Online.Doc_Guid = @Doc_Guid)
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected MADoc With DocInfo]
	@UserID nvarchar(255) = '',
	@MA_Guid nvarchar(255) = '',
	@MANr int = 0,
	@Doc_Art nvarchar(255) = '', 
	@Doc_Info nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kandidaten_Doc_Online 
	WHERE (Kandidaten_Doc_Online.Customer_ID = @UserID)
	And (Kandidaten_Doc_Online.Owner_Guid = @MA_Guid)
	And (Kandidaten_Doc_Online.MANr = @MANr)
	And (Kandidaten_Doc_Online.Doc_Art = @Doc_Art)
	And (Kandidaten_Doc_Online.Doc_Info = @Doc_Info)
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[List MADoc Nach Art]
	@Owner_Guid nvarchar(255) = '',
	@Doc_Art nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT TOP 1000 ID, Doc_info, convert(varchar(10), Transfered_On,104) As Transfered_On 
        FROM Kandidaten_Doc_Online WHERE Owner_Guid = @Owner_Guid And DOC_ART = @Doc_Art 

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[List MADoc Grouped Nach Art]
	@Owner_Guid nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT TOP 1000 Doc_Art, Count(*) As AnzRec
        FROM Kandidaten_Doc_Online WHERE Owner_Guid = @Owner_Guid Group by Doc_Art Order By Doc_Art

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected MAESDoc]
	@UserID nvarchar(255) = '',
	@MA_Guid nvarchar(255) = '',
	@MANr int = 0,
	@ESNr int = 0
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kandidaten_Doc_Online 
	WHERE (Kandidaten_Doc_Online.Customer_ID = @UserID)
	And (Kandidaten_Doc_Online.Owner_Guid = @MA_Guid)
	And (Kandidaten_Doc_Online.MANr = @MANr)
	And (Kandidaten_Doc_Online.ESNr = @ESNr)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected MALODoc]
	@UserID nvarchar(255) = '',
	@MA_Guid nvarchar(255) = '',
	@MANr int = 0,
	@LONr int = 0
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kandidaten_Doc_Online 
	WHERE (Kandidaten_Doc_Online.Customer_ID = @UserID)
	And (Kandidaten_Doc_Online.Owner_Guid = @MA_Guid)
	And (Kandidaten_Doc_Online.MANr = @MANr)
	And (Kandidaten_Doc_Online.LONr = @LONr)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected AllMADoc]
	@UserID nvarchar(255) = '',
	@MA_Guid nvarchar(255) = '',
	@MANr int = 0
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kandidaten_Doc_Online 
	WHERE (Kandidaten_Doc_Online.Customer_ID = @UserID)
	And (Kandidaten_Doc_Online.Owner_Guid = @MA_Guid)
	And (Kandidaten_Doc_Online.MANr = @MANr)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected KDDoc With DocGuid]
	@UserID nvarchar(255) = '',
	@KD_Guid nvarchar(255) = '',
	@KDNr int = 0,
	@Doc_Guid nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden_Doc_Online 
	WHERE (Kunden_Doc_Online.Customer_ID = @UserID)
	And (Kunden_Doc_Online.KD_Guid = @KD_Guid)
	And (Kunden_Doc_Online.KDNr = @KDNr)
	And (Kunden_Doc_Online.Doc_Guid = @Doc_Guid)
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected KDDoc With DocInfo]
	@UserID nvarchar(255) = '',
	@KD_Guid nvarchar(255) = '',
	@KDNr int = 0,
	@Doc_Art nvarchar(255) = '', 
	@Doc_Info nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden_Doc_Online 
	WHERE (Kunden_Doc_Online.Customer_ID = @UserID)
	And (Kunden_Doc_Online.KD_Guid = @KD_Guid)
	And (Kunden_Doc_Online.KDNr = @KDNr)
	And (Kunden_Doc_Online.Doc_Art = @Doc_Art)
	And (Kunden_Doc_Online.Doc_Info = @Doc_Info)
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[Delete Selected KDESDoc]
	@UserID nvarchar(255) = '',
	@KD_Guid nvarchar(255) = '',
	@KDNr int = 0,
	@ESNr int = 0
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden_Doc_Online 
	WHERE (Kunden_Doc_Online.Customer_ID = @UserID)
	And (Kunden_Doc_Online.KD_Guid = @KD_Guid)
	And (Kunden_Doc_Online.KDNr = @KDNr)
	And (Kunden_Doc_Online.ESNr = @ESNr)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create PROCEDURE [dbo].[Delete Selected KDRPDoc]
	@UserID nvarchar(255) = '',
	@KD_Guid nvarchar(255) = '',
	@KDNr int = 0,
	@RPNr int = 0
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden_Doc_Online 
	WHERE (Kunden_Doc_Online.Customer_ID = @UserID)
	And (Kunden_Doc_Online.KD_Guid = @KD_Guid)
	And (Kunden_Doc_Online.KDNr = @KDNr)
	And (Kunden_Doc_Online.RPNr = @RPNr)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create PROCEDURE [dbo].[Delete Selected KDREDoc]
	@UserID nvarchar(255) = '',
	@KD_Guid nvarchar(255) = '',
	@KDNr int = 0,
	@RENr int = 0
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden_Doc_Online 
	WHERE (Kunden_Doc_Online.Customer_ID = @UserID)
	And (Kunden_Doc_Online.KD_Guid = @KD_Guid)
	And (Kunden_Doc_Online.KDNr = @KDNr)
	And (Kunden_Doc_Online.RENr = @RENr)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[Delete Selected AllKDDoc]
	@UserID nvarchar(255) = '',
	@KD_Guid nvarchar(255) = '',
	@KDNr int = 0
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden_Doc_Online 
	WHERE (Kunden_Doc_Online.Customer_ID = @UserID)
	And (Kunden_Doc_Online.KD_Guid = @KD_Guid)
	And (Kunden_Doc_Online.KDNr = @KDNr)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected Doc With Guid]
	@UserID nvarchar(255) = '',
	@Doc_Guid nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kandidaten_Doc_Online 
	WHERE (Kandidaten_Doc_Online.Customer_ID = @UserID)
	And (Kandidaten_Doc_Online.Doc_Guid = @Doc_Guid)

	Delete Kunden_Doc_Online 
	WHERE (Kunden_Doc_Online.Customer_ID = @UserID)
	And (Kunden_Doc_Online.Doc_Guid = @Doc_Guid)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[Delete Selected MA-Rec From Kandidaten]
	@UserID nvarchar(255) = '',
	@Transfered_Guid nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kandidaten
	WHERE (Kandidaten.Customer_ID = @UserID)
	And (Kandidaten.MA_Guid = @Transfered_Guid)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Refresh DocCount With UserID]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
begin try Drop Table #NewCount_1 end Try begin Catch end catch
begin try Drop Table #NewCount_2 end Try begin Catch end catch
begin try Drop Table #NewCount_3 end Try begin Catch end catch

begin try Drop Table #NewCount_MA_ end Try begin Catch end catch
begin try Drop Table #NewCount_MA end Try begin Catch end catch
begin try Drop Table #NewCount_KD end Try begin Catch end catch
begin try Drop Table #NewCount_Vak end Try begin Catch end catch
begin try Drop Table #NewCount_Verleih end Try begin Catch end catch

Select Count(MADoc.Customer_ID) As AnzMADoc, MADoc.Customer_ID, 'MADoc' As DbName, 'Test-Account                                              ' As Customer_Name 
	Into #NewCount_MA_
	From Kandidaten_Doc_Online MADoc 
	WHERE (MADoc.Customer_ID = @UserID Or @UserID = '') 
	Group By MADoc.Customer_ID
select * into #NewCount_MA From #NewCount_MA_
Truncate Table #NewCount_MA

Insert Into #NewCount_MA Select #NewCount_MA_.AnzMADoc, #NewCount_MA_.Customer_ID, #NewCount_MA_.DbName, MySetting.Customer_Name
	From #NewCount_MA_
	Left Join MySetting On #NewCount_MA_.Customer_ID = MySetting.KD_Guid
	Order By MySetting.Customer_Name

Select Count(KDDoc.Customer_ID) As AnzKDDoc, KDDoc.Customer_ID, 'KDDoc' As DbName , 'Test-Account                                               ' As Customer_Name
	Into #NewCount_KD 
	From Kunden_Doc_Online KDDoc 
	WHERE (KDDoc.Customer_ID = @UserID Or @UserID = '') 
	Group By KDDoc.Customer_ID

Insert Into #NewCount_MA Select #NewCount_KD.AnzKDDoc,#NewCount_KD.Customer_ID, #NewCount_KD.DbName, MySetting.Customer_Name
	From #NewCount_KD 
	Left Join MySetting On #NewCount_KD.Customer_ID = MySetting.KD_Guid
	Order By MySetting.Customer_Name

Select Count(KDVak.Customer_ID) As AnzVakDoc, KDVak.Customer_ID, 'KDVak' As DbName , 'Test-Account                                                   ' As Customer_Name
	Into #NewCount_Vak
	From KD_Vakanzen KDVak
	WHERE (KDVak.Customer_ID = @UserID Or @UserID = '') 
	Group By KDVak.Customer_ID

Insert Into #NewCount_MA Select Isnull(#NewCount_Vak.AnzVakDoc, 0), IsNull(#NewCount_Vak.Customer_ID, ''), isnull(#NewCount_Vak.DbName, ''), isnull(MySetting.Customer_Name,'')
	From #NewCount_Vak 
	Left Join MySetting On #NewCount_Vak.Customer_ID = MySetting.Vak_Guid
	Order By MySetting.Customer_Name

Select #NewCount_MA.* 
	From #NewCount_MA 
	Order By #NewCount_MA.Customer_Name, DBName

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get MADocrec2ShowOnClient]
	@UserID nvarchar(255) = '',
	@MANr nvarchar(10) = '',
	@DocArt nvarchar(255) = '',
	@TransferedOn nvarchar(255) = '',
	@Operator nvarchar(2) = '>=',
	@SortString nvarchar(255) = 'MADoc.Transfered_On DESC'

AS
BEGIN
	SET NOCOUNT ON
	Declare @SQL nvarchar(2000)
	
	Set @SQL = 'SELECT MADoc.ID, MADoc.MANr, MADoc.Customer_ID, 
	MADoc.MA_Vorname, MADoc.MA_Nachname, 
	MADoc.Doc_Art, MADoc.Transfered_On, 
	MADoc.Transfered_User, MADoc.Owner_Guid, MADoc.Doc_Guid, MADoc.Doc_Info, MADoc.DocFilename 
	FROM Kandidaten_Doc_Online MADoc
	WHERE (MADoc.Customer_ID = ''' + @UserID + ''') 
	And (MADoc.Doc_Art = ''' + @DocArt + ''' Or ''' + @DocArt + ''' = '''') 
	And (MADoc.MANr = ''' + @MANr + ''' Or ''' + @MANr + ''' = ''0'' Or  ''' + @MANr + ''' = '''') 
	And (convert(varchar, MADoc.Transfered_On, 104) ' + 
	@Operator + ' ''' + @TransferedOn + ''' Or ''' + @TransferedOn + ''' = '''') ORDER BY ' + @Sortstring 
	
	EXEC (@sql)
	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get KDDocrec2ShowOnClient]
	@UserID nvarchar(255) = '',
	@KDNr nvarchar(255) = '',
	@DocArt nvarchar(255) = '',
	@TransferedOn nvarchar(255) = '',
	@Operator nvarchar(2) = '>=',
	@SortString nvarchar(255) = 'KDDoc.Transfered_On DESC'

AS
BEGIN
	SET NOCOUNT ON
	Declare @SQL nvarchar(2000)
	
	Set @SQL = 'SELECT KDDoc.ID, KDDoc.KDNr, KDDoc.Customer_ID, 
	KD.KD_Name, '''' As KD_Name, '''' As ZHD_Vorname, 
	KDDoc.Doc_Art, KDDoc.Transfered_On, KDDoc.GetResult, KDDoc.Get_On, 
	KDDoc.Transfered_User, KDDoc.KD_Guid, KDDoc.ZHD_Guid, KDDoc.Doc_Guid, KDDoc.Doc_Info, KDDoc.DocFilename 
	FROM Kunden_Doc_Online KDDoc Left Join Kunden KD On KDDoc.Customer_ID = KD.Customer_ID 
	And KDDoc.KD_Guid = KD.KD_Guid 
	WHERE (KDDoc.Customer_ID = ''' + @UserID + ''') 
	And (KDDoc.Doc_Art = ''' + @DocArt + ''' Or ''' + @DocArt + ''' = '''') 
	And (KDDoc.KDNr = ''' + @KDNr + ''' Or ''' + @KDNr + ''' = ''0'' Or  ''' + @KDNr + ''' = '''') 
	And (convert(varchar, KDDoc.Transfered_On, 104) ' + @Operator + '''' + @TransferedOn + ''' Or ''' + @TransferedOn + ''' = '''') 
	ORDER BY ' + @Sortstring 

	
	EXEC (@sql)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete Selected KD-Rec From Kunden]
	@UserID nvarchar(255) = '',
	@KD_Guid nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden
	WHERE (Kunden.Customer_ID = @UserID)
	And (Kunden.KD_Guid = @KD_Guid)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Change Selected KDGuid With NewGuid]
	@UserID nvarchar(255) = '',
	@OldKD_Guid nvarchar(255) = '',
	@NewKD_Guid nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON

	Update Kunden_Doc_Online 
	Set Kunden_Doc_Online.KD_Guid = @NewKD_Guid 
	WHERE (Kunden_Doc_Online.Customer_ID = @UserID)
	And (Kunden_Doc_Online.KD_Guid = @OldKD_Guid)
	
	Update Kunden
	Set Kunden.KD_Guid = @NewKD_Guid 
	WHERE (Kunden.Customer_ID = @UserID)
	And (Kunden.KD_Guid = @OldKD_Guid)

	Update Kunden_ZHD 
	Set Kunden_ZHD.KD_Guid = @NewKD_Guid 
	WHERE (Kunden_ZHD.Customer_ID = @UserID)
	And (Kunden_ZHD.KD_Guid = @OldKD_Guid)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Change Selected KDZHDGuid With NewGuid]
	@UserID nvarchar(255) = '',
	@KD_Guid nvarchar(255) = '',
	@OldKDZHD_Guid nvarchar(255) = '',
	@NewKDZHD_Guid nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON

	Update Kunden_Doc_Online 
	Set Kunden_Doc_Online.ZHD_Guid = @NewKDZHD_Guid 
	WHERE (Kunden_Doc_Online.Customer_ID = @UserID)
	And (Kunden_Doc_Online.KD_Guid = @KD_Guid)
	And (Kunden_Doc_Online.ZHD_Guid = @OldKDZHD_Guid)
	
	Update Kunden_ZHD 
	Set Kunden_ZHD.ZHD_Guid = @NewKDZHD_Guid 
	WHERE (Kunden_ZHD.Customer_ID = @UserID)
	And (Kunden_ZHD.KD_Guid = @KD_Guid)
	And (Kunden_ZHD.ZHD_Guid = @OldKDZHD_Guid)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected KDZHD-Rec From Kunden ZHD]
	@UserID nvarchar(255) = '',
	@KD_Guid nvarchar(255) = '',
	@ZHD_Guid nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden_ZHD
	WHERE (Kunden_ZHD.Customer_ID = @UserID)
	And (Kunden_ZHD.KD_Guid = @KD_Guid)
	And (Kunden_ZHD.ZHD_Guid = @ZHD_Guid)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Change Selected MAGuid With NewGuid]
	@UserID nvarchar(255) = '',
	@OldMA_Guid nvarchar(255) = '',
	@NewMA_Guid nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON

	Update Kandidaten_Doc_Online 
	Set Kandidaten_Doc_Online.Owner_Guid = @NewMA_Guid 
	WHERE (Kandidaten_Doc_Online.Customer_ID = @UserID)
	And (Kandidaten_Doc_Online.Owner_Guid = @OldMa_Guid)
	
	Update Kandidaten 
	Set Kandidaten.MA_Guid = @NewMA_Guid 
	WHERE (Kandidaten.Customer_ID = @UserID)
	And (Kandidaten.MA_Guid = @OldMA_Guid)

	Update Kandidaten_Online  
	Set Kandidaten_Online.Transfered_Guid = @NewMA_Guid 
	WHERE (Kandidaten_Online.Customer_ID = @UserID)
	And (Kandidaten_Online.Transfered_Guid = @OldMA_Guid)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected KD-Rec From ALL KD_Db]
	@UserID nvarchar(255) = '',
	@KD_Guid nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden
	WHERE (Kunden.Customer_ID = @UserID)
	And (Kunden.KD_Guid = @KD_Guid)

	Delete Kunden_ZHD
	WHERE (Kunden_ZHD.Customer_ID = @UserID)
	And (Kunden_ZHD.KD_Guid = @KD_Guid)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Vak-Gruppe]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KD_Vakanzen.Gruppe
	FROM KD_Vakanzen
	WHERE (KD_Vakanzen.WOS_Guid = @UserID) 
	And (KD_Vakanzen.Gruppe <> '' Or KD_Vakanzen.Gruppe Is Not Null)
	Group By KD_Vakanzen.Gruppe
	ORDER BY KD_Vakanzen.Gruppe
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Get Vakrec With Gruppe]
	@UserID nvarchar(255) = '',
	@Beruf nvarchar(255) = '',
	@Ort nvarchar(255) = '',
	@Kanton nvarchar(255) = '', 
	@Region nvarchar(255) = '', 
	@Filiale nvarchar(255) = '',
	@Gruppe nvarchar(255) = '',
	@SortString nvarchar(255) = 'KD_Vakanzen.Transfered_On DESC'

AS
if @SortString = '' 
	begin
	 set @SortString = 'KD_Vakanzen.Transfered_On DESC'
	end
BEGIN
	SET NOCOUNT ON
	Declare @SQL nvarchar(2000)
	
	Set @SQL = 'SELECT KD_Vakanzen.* 
	FROM KD_Vakanzen 
	WHERE (KD_Vakanzen.Customer_ID = ''' + @UserID + ''') 
	And (KD_Vakanzen.Bezeichnung Like ''%' + @Beruf + '%'' Or '''	+ @Beruf +	''' = '''') 
	And (KD_Vakanzen.JobOrt Like ''%' + @Ort  + '%'' Or '''	+ @Ort + ''' = '''') 
	And (KD_Vakanzen.Vak_Kanton = '''	+ @Kanton + ''' Or '''	+ @Kanton + ''' = '''') 
	And (KD_Vakanzen.Vak_Region Like ''%'	+ @Region + '%'' Or '''	+ @Region + ''' = '''') 
	And (KD_Vakanzen.Filiale = ''' + @Filiale + ''' Or ''' + @Filiale + ''' = '''') 
	And (KD_Vakanzen.Gruppe = ''' + @Gruppe + ''' Or ''' + @Gruppe + ''' = '''') 
	ORDER BY ' + @Sortstring 
	
	EXEC (@sql)
	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Get UserData From Customer_Users]
	@Customer_ID nvarchar(50) = '',
	@User_ID nvarchar(50) = ''

AS
BEGIN
	SET NOCOUNT ON
	Declare @SQL nvarchar(2000)
	Select [USER_ID] From Customer_Users Where Customer_ID = @Customer_ID And [USER_ID] = @User_ID
	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get Vakrec By ID]
	@UserID nvarchar(255) = '',
	@RecID nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON
-- Neu 25.04.2012
	Declare @SQL nvarchar(2000)
	
	Set @SQL = 'SELECT v.*, IsNull(u.User_Vorname, v.User_Vorname_old) As User_Vorname, ' +
				'IsNull(u.User_Nachname, v.User_Nachname_old) As User_Nachname, u.User_picture, u.User_Sign, ' +
				'isnull(u.User_Telefon, v.User_Telefon_old) As User_Telefon, ' +
				'isnull(u.User_Telefax, v.User_Telefax_old) As User_Telefax, ' +
				'isnull(u.User_eMail, v.User_eMail_old) As User_eMail, ' +
				'isnull(u.User_Sex, '''') As User_salutation, ' +
				'MyS.Customer_Ort As CustomerPLZOrt ' +
				'FROM KD_Vakanzen v Left Join Customer_Users u On v.User_Guid = u.[User_ID] ' +
				'And v.Customer_ID = u.[Customer_ID] ' +
				'Left Join MySetting MyS On v.Customer_ID = MyS.Vak_Guid ' +
				'WHERE (v.Customer_ID = ''' + @UserID + ''') And (v.ID = ''' + @RecID + ''')'
	
	EXEC (@SQL)

end
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get Vakrec By ID_1]
	@UserID nvarchar(255) = '',
	@RecID nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON
-- Neu 25.04.2012
	Declare @SQL nvarchar(2000)
	
	Set @SQL = 'SELECT v.*, IsNull(u.User_Vorname, v.User_Vorname_old) As User_Vorname, ' +
				'IsNull(u.User_Nachname, v.User_Nachname_old) As User_Nachname, u.User_picture, u.User_Sign, ' +
				'isnull(u.User_Telefon, v.User_Telefon_old) As User_Telefon, ' +
				'isnull(u.User_Telefax, v.User_Telefax_old) As User_Telefax, ' +
				'isnull(u.User_eMail, v.User_eMail_old) As User_eMail, ' +
				'MyS.Customer_Ort As CustomerPLZOrt ' +
				'FROM KD_Vakanzen v Left Join Customer_Users u On v.User_Guid = u.[User_ID] ' +
				'And v.Customer_ID = u.[Customer_ID] ' +
				'Left Join MySetting MyS On U.Customer_ID = MyS.Vak_Guid ' +
				'WHERE (v.Customer_ID = ''' + @UserID + ''') And (v.ID = ''' + @RecID + ''')'
	
	EXEC (@SQL)

end
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Vak-Branchen]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KD_Vakanzen.Branchen 
	FROM KD_Vakanzen
	WHERE (KD_Vakanzen.WOS_Guid = @UserID)
	And (KD_Vakanzen.Branchen <> '' Or KD_Vakanzen.Branchen Is Not Null)
	Group By KD_Vakanzen.Branchen
	ORDER BY KD_Vakanzen.Branchen

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get Vakrec With Gruppe And Branchen]
	@UserID nvarchar(255) = '',
	@Beruf nvarchar(255) = '',
	@Ort nvarchar(255) = '',
	@Kanton nvarchar(255) = '', 
	@Region nvarchar(255) = '', 
	@Filiale nvarchar(255) = '',
	@Gruppe nvarchar(255) = '',
	@Branchen nvarchar(255) = '',
	@SortString nvarchar(255) = 'KD_Vakanzen.Transfered_On DESC'

AS
if @SortString = '' 
	begin
	 set @SortString = 'KD_Vakanzen.Transfered_On DESC'
	end
BEGIN
	SET NOCOUNT ON
	Declare @SQL nvarchar(2000)
	
	Set @SQL = 'SELECT KD_Vakanzen.* 
	FROM KD_Vakanzen 
	WHERE (KD_Vakanzen.Customer_ID = ''' + @UserID + ''') 
	And (KD_Vakanzen.Bezeichnung Like ''%' + @Beruf + '%'' Or '''	+ @Beruf +	''' = '''') 
	And (KD_Vakanzen.JobOrt Like ''%' + @Ort  + '%'' Or '''	+ @Ort + ''' = '''') 
	And (KD_Vakanzen.Vak_Kanton = '''	+ @Kanton + ''' Or '''	+ @Kanton + ''' = '''') 
	And (KD_Vakanzen.Vak_Region Like ''%'	+ @Region + '%'' Or '''	+ @Region + ''' = '''') 
	And (KD_Vakanzen.Filiale = ''' + @Filiale + ''' Or ''' + @Filiale + ''' = '''') 
	And (KD_Vakanzen.Gruppe = ''' + @Gruppe + ''' Or ''' + @Gruppe + ''' = '''') 
	And (KD_Vakanzen.Branchen Like ''%' + @Branchen + '%'' Or ''' + @Branchen + ''' = '''') 
	ORDER BY ' + @Sortstring 
	
	EXEC (@sql)
--select @sql 	

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Get Vakrec For Listing]
	@UserID nvarchar(255) = '',
	@Beruf nvarchar(255) = '',
	@Ort nvarchar(255) = '',
	@Kanton nvarchar(255) = '', 
	@Region nvarchar(255) = '', 
	@Filiale nvarchar(255) = '',
	@SortString nvarchar(255) = 'KD_Vakanzen.Transfered_On DESC'

AS
if @SortString = '' 
	begin
	 set @SortString = 'KD_Vakanzen.Transfered_On DESC'
	end
BEGIN
	SET NOCOUNT ON
	Declare @SQL nvarchar(2000)
	
	Set @SQL = 'SELECT KD_Vakanzen.* 
	FROM KD_Vakanzen 
	WHERE (KD_Vakanzen.Customer_ID = ''' + @UserID + ''') 
	And (KD_Vakanzen.Bezeichnung Like ''%' + @Beruf + '%'' Or '''	+ @Beruf +	''' = '''') 
	And (KD_Vakanzen.JobOrt Like ''%' + @Ort  + '%'' Or '''	+ @Ort + ''' = '''') 
	And (KD_Vakanzen.Vak_Kanton = '''	+ @Kanton + ''' Or '''	+ @Kanton + ''' = '''') 
	And (KD_Vakanzen.Vak_Region Like ''%'	+ @Region + '%'' Or '''	+ @Region + ''' = '''') 
	And (KD_Vakanzen.Filiale = ''' + @Filiale + ''' Or ''' + @Filiale + ''' = '''') 
	ORDER BY ' + @Sortstring 
	
	EXEC (@sql)
	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected KD-Rec From Kunden With KDNr]
	@UserID nvarchar(255) = '',
	@KDNr int = 0
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden
	WHERE (Kunden.Customer_ID = @UserID)
	And (Kunden.KDNr = @KDNr)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[Delete Selected KDZHD-Rec From Kunden ZHD With ZHDNr]
	@UserID nvarchar(255) = '',
	@KDNr int = 0,
	@ZHDNr int = 0
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kunden_ZHD
	WHERE (Kunden_ZHD.Customer_ID = @UserID)
	And (Kunden_ZHD.KDNr = @kdnr)
	And (Kunden_ZHD.ZHDNr = @zhdnr)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Create New Vacancy For OstJob.CH]
	@Customer_Guid NVARCHAR(50),
	@User_Guid NVARCHAR(50),
	@VakNr INT,
	@JobVersion NVARCHAR(1),
	@Company NVARCHAR(255),
	@Title NVARCHAR(255),
	@Workplace_Country NVARCHAR(255),
	@Workplace_Zip NVARCHAR(50),
	@Workplace_City NVARCHAR(255),
	@Company_Description NVARCHAR(255),
	@Aufgabe NVARCHAR(4000),
	@Anforderungen NVARCHAR(4000),
	@WirBieten NVARCHAR(4000),
	@Contact NVARCHAR(4000),
	@Description_url NVARCHAR(255),
	@Application_url NVARCHAR(255),
	@Apprenticeship BIT,
	@Template NVARCHAR(10),
	@Keywords NVARCHAR(255),

	@StartDate datetime ,
	@EndDate Datetime ,
	
	@IsOnline BIT,
	@Publication_ostjob_ch BIT,
	@Publication_westjob_at BIT,
	@Publication_nicejob_de BIT,
	@Publication_zentraljob_ch  BIT,
	@Publication_minisite BIT
AS

BEGIN
SET NOCOUNT ON

DELETE dbo.tblOstJobCHPlattform WHERE Customer_Guid = @Customer_Guid AND VakNr = @VakNr;

IF @EndDate < CONVERT(DATETIME, CONVERT(NVARCHAR(10), GETDATE(), 104), 104)
BEGIN
	SET @IsOnline = 0

END

IF @IsOnline = 1
	BEGIN

	INSERT INTO tblOstJobCHPlattform (
		Customer_Guid,
		User_Guid,
		VakNr,
		JobVersion,
		Company,
		Title,
		Workplace_Country,
		Workplace_Zip,
		Workplace_City,
		Company_Description,
		Aufgabe,
		Anforderungen,
		WirBieten,
		Contact,
		Description_url,
		Application_url,
		Apprenticeship,
		Template,
		KeyWords,
		CreatedOn,

		StartDate ,
		EndDate ,

		IsOnline,
		Publication_ostjob_ch,
		Publication_westjob_at,
		Publication_nicejob_de,
		Publication_zentraljob_ch,
		Publication_minisite
		)
		VALUES 
		(
		@Customer_Guid,
		@User_Guid,
		@VakNr,
		@JobVersion,
		@Company,
		@Title,
		@Workplace_Country,
		@Workplace_Zip,
		@Workplace_City,
		@Company_Description,
		@Aufgabe,
		@Anforderungen,
		@WirBieten,
		@Contact,
		@Description_url,
		@Application_url,
		@Apprenticeship,
		@Template,
		@Keywords,
		GETDATE(),

		@StartDate ,
		@EndDate ,

		@IsOnline,
		@Publication_ostjob_ch,
		@Publication_westjob_at,
		@Publication_nicejob_de,
		@Publication_zentraljob_ch,
		@Publication_minisite
		)

	END
 
 DELETE tblOstJobCHPlattform
 WHERE  ( EndDate < CONVERT(DATETIME, CONVERT(NVARCHAR(10), GETDATE(), 104), 104) )
        AND Customer_Guid = @Customer_Guid
  
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Create New Vacancy For Jobs.CH]
	@Customer_Guid NVARCHAR(255) ,
	@User_Guid NVARCHAR(255),
	@VakNr INT,

	@OrganisationID NVARCHAR(255),
	@InseratID INT,
	@Vorspann  NVARCHAR(4000),
	@Beruf  NVARCHAR(255),
	@Taetigkeit  NVARCHAR(4000),
	@Anforderung  NVARCHAR(4000),
	@WirBieten  NVARCHAR(4000),
	@PLZ  NVARCHAR(255),
	@Ort  NVARCHAR(255),
	@Kontakt  NVARCHAR(255),
	@EMail  NVARCHAR(255),
	@URL  NVARCHAR(255),
	@Direkt_URL  NVARCHAR(255),
	@Direkt_URL_Post_Args  NVARCHAR(255),

	@StartDate datetime ,
	@EndDate Datetime ,

	@Titel  NVARCHAR(255),
	@Anriss  NVARCHAR(150),
	@Firma  NVARCHAR(255),
	@Anstellungsart  NVARCHAR(255),
	@Our_URL  NVARCHAR(255),
	@Anstellungsgrad_von_bis  NVARCHAR(255),
	@RubrikID  NVARCHAR(255),
	@Position  NVARCHAR(255),
	@Branche  NVARCHAR(255),
	@Sprache  NVARCHAR(255),
	@Region  NVARCHAR(255),

	@Alter_Von_bis  NVARCHAR(255),

	@Sprachkenntniss_Kandidat  NVARCHAR(255),
	@Sprachkenntniss_Niveau  NVARCHAR(255),

	@Bildungsniveau  NVARCHAR(255),
	@Berufserfahrung  NVARCHAR(255),
	@Berufserfahrung_Position  NVARCHAR(255),
	@Lyout  NVARCHAR(255),
	@Logo  NVARCHAR(255),
	@Bewerben_URL  NVARCHAR(255),
	@Angebot  NVARCHAR(255),

	@Xing_Poster_URL  NVARCHAR(255),
	@Xing_Company_Profile_URL  NVARCHAR(255),
	@Xing_Company_Is_Poc NVARCHAR(255), 
	@SetOnline bit

AS

BEGIN
SET NOCOUNT ON

DELETE dbo.tblJobCHPlattform WHERE Customer_Guid = @Customer_Guid AND VakNr = @VakNr;

IF @EndDate < CONVERT(DATETIME, CONVERT(NVARCHAR(10), GETDATE(), 104), 104)
BEGIN
	SET @SetOnline = 0

END

IF @SetOnline = 1
BEGIN
	INSERT INTO tblJobCHPlattform (
		Customer_Guid,
		User_Guid,
		VakNr,
		OrganisationID,
		InseratID ,
		Vorspann ,
		Beruf ,
		Text_Taetigkeit ,
		Text_Anforderung ,
		Text_Wirbieten ,
		PLZ ,
		Ort ,
		Kontakt ,
		EMail ,
		URL ,
		Direkt_URL ,
		Direkt_URL_Post_Args ,

		StartDate ,
		EndDate ,

		Titel ,
		Anriss ,
		Firma ,
		Anstellungsart ,
		Our_URL ,
		Anstellungsgrad_von_bis ,
		RubrikID ,
		Position ,
		Branche ,
		Sprache ,
		Region ,

		Alter_Von_bis ,

		Sprachkenntniss_Kandidat ,
		Sprachkenntniss_Niveau ,

		Bildungsniveau ,
		Berufserfahrung ,
		Berufserfahrung_Position ,
		Lyout ,
		Logo ,
		Bewerben_URL ,
		Angebot ,

		Xing_Poster_URL,
		Xing_Company_Profile_URL ,
		Xing_Company_Is_Poc,
		CreatedOn 
		)
		VALUES 
		(
		@Customer_Guid,
		@User_Guid,
		@VakNr,
		@OrganisationID,
		@InseratID ,
		@Vorspann ,
		@Beruf ,
		@Taetigkeit ,
		@Anforderung ,
		@Wirbieten ,
		@PLZ ,
		@Ort ,
		@Kontakt ,
		@EMail ,
		@URL ,
		@Direkt_URL ,
		@Direkt_URL_Post_Args ,

		@StartDate ,
		@EndDate ,

		@Titel ,
		@Anriss ,
		@Firma ,
		@Anstellungsart ,
		@Our_URL ,
		@Anstellungsgrad_von_bis ,
		@RubrikID ,
		@Position ,
		@Branche ,
		@Sprache ,
		@Region ,

		@Alter_Von_bis ,

		@Sprachkenntniss_Kandidat ,
		@Sprachkenntniss_Niveau ,

		@Bildungsniveau ,
		@Berufserfahrung ,
		@Berufserfahrung_Position ,
		@Lyout ,
		@Logo ,
		@Bewerben_URL ,
		@Angebot ,

		@Xing_Poster_URL,
		@Xing_Company_Profile_URL ,
		@Xing_Company_Is_Poc,
		GETDATE() 
		)
END

DELETE  tblJobCHPlattform
WHERE   ( EndDate < CONVERT(DATETIME, CONVERT(NVARCHAR(10), GETDATE(), 104), 104) ) AND Customer_Guid = @Customer_Guid

END


GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Create New Vacancy For Intern]
	@Customer_Guid NVARCHAR(255) ,
	@User_Guid NVARCHAR(255),

	@Customer_Name nvarchar(255),
	@Customer_strasse nvarchar(255),
	@Customer_plz nvarchar(255),
	@Customer_ort nvarchar(255),
	@Customer_land nvarchar(255),
	@Customer_telefon nvarchar(255),
	@Customer_telefax nvarchar(255),
	@Customer_email nvarchar(255),
	@Customer_homepage nvarchar(255),

	@User_Vorname nvarchar(255),
	@User_Nachname nvarchar(255),
	@User_telefon nvarchar(255),
	@User_telefax nvarchar(255),
	@User_email nvarchar(255),
	@User_homepage nvarchar(255),
	@User_Anrede nvarchar(50),
	@User_Initial nvarchar(50),
	@User_Filiale nvarchar(50),

	@Loged_UserName nvarchar(255),
	@Loged_UserGuid nvarchar(255),


	@VakNr INT,
	@KDNr INT,
	@KDZHDNr INT,

	@Berater NVARCHAR(255),
	@Filiale nvarchar(255),
	@vakKontakt  NVARCHAR(4000),
	@UserKontakt  NVARCHAR(255),
	@VakState  NVARCHAR(4000),
	@Bezeichnung  NVARCHAR(4000),
	@slogan  NVARCHAR(4000),
	@Gruppe NVARCHAR(4000),

	@Vaklink NVARCHAR(4000),
	@Beginn  NVARCHAR(4000),
	@jobprozent  NVARCHAR(4000),
	@anstellung NVARCHAR(4000),

	@Dauer NVARCHAR(4000),
	@MAAge  NVARCHAR(4000),
	@MASex  NVARCHAR(4000),
	@MAZivil  NVARCHAR(4000),

	@MALohn  NVARCHAR(4000),
	@JobTime  NVARCHAR(4000),
	@JobOrt NVARCHAR(4000),
	@JobPLZ  NVARCHAR(4000),

	@Jobs_Vorspann NVARCHAR(4000),
	@_Jobs_Vorspann NVARCHAR(4000),

	@Jobs_Aufgabe NVARCHAR(4000),
	@_Jobs_Aufgabe NVARCHAR(4000),

	@Jobs_Anforderung NVARCHAR(4000),
	@_Jobs_Anforderung NVARCHAR(4000),

	@Jobs_WirBieten NVARCHAR(4000),
	@_Jobs_WirBieten NVARCHAR(4000),

	@Branchen NVARCHAR(255),
	@MSprache NVARCHAR(255),
	@Region NVARCHAR(255),
	@Vak_Kanton NVARCHAR(2),
	@Qualifikation NVARCHAR(255),

	@User_Picture image,
	@User_Sign image,

	@SetOnline bit

AS

BEGIN
SET NOCOUNT ON

DELETE dbo.KD_Vakanzen WHERE Customer_ID = @Customer_Guid AND VakNr = @VakNr;
DELETE dbo.Customer_Users WHERE [USER_ID] = @User_Guid ;

Insert Into Customer_Users (
		Customer_ID  ,
		Customer_Name  ,
		[User_ID] ,
		User_Sex,
		User_Vorname,
		User_Nachname,
		User_Initial,
		User_Filiale,
		User_Telefon,
		User_Telefax,
		User_eMail,
		User_Homepage,
		User_Picture,
		User_Sign,
		CreatedOn
	)
		Values
	(
		@Customer_Guid  ,
		@Customer_Name  ,
		@User_Guid ,
		@User_Anrede,
		@User_Vorname,
		@User_Nachname,
		@User_Initial,
		@User_Filiale,
		@User_Telefon,
		@User_Telefax,
		@User_eMail,
		@User_Homepage,
		@User_Picture,
		@User_Sign,
		getdate()
	)
		

IF @SetOnline = 1
BEGIN
	INSERT INTO KD_Vakanzen (
		Customer_ID  ,
		User_Guid ,
		Customer_Name ,
		Customer_Strasse ,
		Customer_Ort ,
		Customer_Telefon ,
		Customer_eMail ,

		VakNr ,
		KDNr ,
		KDZHDNr ,

		Berater ,
		Filiale ,
		vakKontakt  ,
		VakState  ,
		Bezeichnung  ,
		slogan  ,
		Gruppe ,

		Vaklink ,
		Beginn  ,
		jobprozent  ,
		anstellung ,

		Dauer ,
		MAAge  ,
		MASex  ,
		MAZivil  ,

		MALohn  ,
		JobTime  ,
		JobOrt ,
		JobPLZ ,

		KDBeschreibung ,
		_KDBeschreibung ,

		Taetigkeit ,
		_Taetigkeit ,

		Anforderung ,
		_Anforderung ,

		KDBietet ,
		_KDBietet ,

		Branchen ,
		Vak_Region ,
		Vak_Kanton ,

		MSprachen ,
		Qualifikation ,
		CreatedOn ,
		Transfered_On 
		)

		VALUES 
		(
		@Customer_Guid  ,
		@User_Guid ,
		@Customer_name ,
		@Customer_strasse ,
		@Customer_Ort ,
		@Customer_Telefon ,
		@Customer_eMail ,

		@VakNr ,
		@KDNr ,
		@KDZHDNr ,

		@Berater ,
		@Filiale ,
		@UserKontakt  ,
		@VakState  ,
		@Bezeichnung  ,
		@slogan  ,
		@Gruppe ,

		@Vaklink ,
		@Beginn  ,
		@jobprozent  ,
		@anstellung ,

		@Dauer ,
		@MAAge  ,
		@MASex  ,
		@MAZivil  ,

		@MALohn  ,
		@JobTime  ,
		@JobOrt ,
		@JobPLZ  ,

		@Jobs_Vorspann ,
		@_Jobs_Vorspann ,

		@Jobs_Aufgabe ,
		@_Jobs_Aufgabe ,

		@Jobs_Anforderung ,
		@_Jobs_Anforderung ,

		@Jobs_WirBieten ,
		@_Jobs_WirBieten ,

		@Branchen ,
		@MSprache ,
		@Region ,
		@Vak_Kanton ,
		@Qualifikation ,

		GETDATE() , 
		GETDATE() 
		)
END

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[List Vacancy Job Categories]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT Berufgruppe
	FROM tblVacancyJobExperience
	WHERE (WOS_Guid = @UserID)
	And (Berufgruppe <> '' Or Berufgruppe Is Not Null)
	Group By Berufgruppe 
	ORDER BY Berufgruppe

END

GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [dbo].[List Vacancy Job Disciplines]
	@UserID nvarchar(255) = '',
	@JobCategories nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON
	
	SELECT BerufErfahrung
	FROM tblVacancyJobExperience
	WHERE (WOS_Guid = @UserID)
	And (@JobCategories = '' OR Berufgruppe = @JobCategories)
	And (BerufErfahrung <> '' Or BerufErfahrung Is Not Null)
	Group By BerufErfahrung 
	ORDER BY BerufErfahrung 

END


GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Delete Vakanzen JobCH]

AS
BEGIN
	SET NOCOUNT ON
	
Delete tblJobCHPlattform
where EndDate < DATEADD(day, 0, CURRENT_TIMESTAMP)

END

GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Clean All Vacancies With EndDate]
AS 
    BEGIN
        SET NOCOUNT ON

        DELETE  tblJobCHPlattform
        WHERE   ( EndDate < CONVERT(DATETIME, CONVERT(NVARCHAR(10), GETDATE(), 104), 104) ) ;

        DELETE  tblOstJobCHPlattform
        WHERE   ( EndDate < CONVERT(DATETIME, CONVERT(NVARCHAR(10), GETDATE(), 104), 104) );

        DELETE  dbo.tblVacancyJobExperience
        WHERE   Vaknr NOT IN (
                SELECT  Vaknr
                FROM    dbo.KD_Vakanzen
                WHERE   dbo.tblVacancyJobExperience.vaknr = dbo.KD_Vakanzen.Vaknr
                        AND dbo.tblVacancyJobExperience.WOS_Guid = dbo.KD_Vakanzen.WOS_Guid );


    END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete Assigned Customer Document With DocGuid]
  @CustomerID NVARCHAR(50) ,
  @WOSGuid NVARCHAR(50) ,
	@DocumentArt NVARCHAR(50) ,
  @DocumentGuid NVARCHAR(255)
AS
    BEGIN
        SET NOCOUNT ON;
	
DECLARE @stateID INT = 0
SELECT @stateID = ISNULL(FK_StateID, 0) FROM dbo.Kunden_Doc_Online         
				WHERE   ( Kunden_Doc_Online.WOS_Guid = @CustomerID OR Kunden_Doc_Online.WOS_Guid = @WOSGuid )
                AND ( Kunden_Doc_Online.Doc_Art = @DocumentArt )
                AND ( Kunden_Doc_Online.Doc_Guid = @DocumentGuid )

        DELETE  dbo.tbl_Customer_WOSDocument_State
        WHERE   ( WOS_ID = @WOSGuid )
                AND ( ID = @stateID );

        DELETE  Kunden_Doc_Online
        WHERE   ( Kunden_Doc_Online.WOS_Guid = @CustomerID OR Kunden_Doc_Online.WOS_Guid = @WOSGuid )
                AND ( Kunden_Doc_Online.Doc_Art = @DocumentArt )
                AND ( Kunden_Doc_Online.Doc_Guid = @DocumentGuid );

    END;
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get Employee Data For sending Notifications]
	@Guid nvarchar(255) = '',
	@USGuid nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON

Select MA.Customer_ID, MA.MA_Nachname, MA.MA_Vorname, MA.MA_EMail As Reciever, MA.MA_Language As Language, MA.BriefAnrede MA_BriefAnrede, 
				'' ZHD_Nachname, '' Zhd_BriefAnrede, '' ZHDSex,
				IsNull((US.User_Vorname + ' ' + US.User_Nachname), '') As Berater, 
				IsNull(SetDb.KD_Guid, '') As Customer_ID, 
				IsNull(US.Customer_Name, SetDb.Customer_Name) As Customer_Name, 
				IsNull(SetDb.Customer_Ort, SetDb.Customer_Ort) As Customer_Ort, 
				ISNULL(SetDb.Customer_Strasse, IsNull(SetDb.Customer_Strasse, '')) As Customer_Strasse, 
				(CASE
					WHEN ISNULL(US.User_Telefon, '') = '' THEN SetDb.Customer_Telefon
					ELSE 
                    US.User_Telefon                
				END) User_Telefon,
				(CASE
					WHEN ISNULL(US.USer_Telefax, '') = '' THEN SetDb.Customer_Telefax
					ELSE 
                    US.USer_Telefax                
				END) USer_Telefax,
				(CASE
					WHEN ISNULL(US.User_eMail, '') = '' THEN SetDb.Customer_eMail
					ELSE 
                    US.User_eMail                
				END) User_eMail,
				(CASE
					WHEN ISNULL(US.User_Homepage, '') = '' THEN SetDb.Customer_Homepage
					ELSE 
                    US.User_Homepage                
				END) User_Homepage

				From Kandidaten MA 
				left Join MySetting SetDb On MA.Customer_ID = SetDb.MA_Guid 
				left Join Customer_Users US On MA.Customer_ID = US.Customer_ID 
				Where Ma.MA_eMail <> '' And MA.MA_Guid = @Guid  And US.User_ID = @USGuid	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get Customer Data For sending Notifications]
	@Guid nvarchar(255) = '',
	@USGuid nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON

Select KD.Customer_ID, KD.KD_Name, KD.KD_eMail As Reciever, KD.KD_Language As Language, 
				'' ZHD_Nachname, '' Zhd_BriefAnrede, '' ZHDSex,
				'' MA_Nachname, '' MA_BriefAnrede, 
				ISNULL((US.User_Vorname + ' ' + US.User_Nachname), '') As Berater, 
				IsNull(SetDb.KD_Guid, '') As Customer_ID, 
				IsNull(US.Customer_Name, SetDb.Customer_Name) As Customer_Name, 
				IsNull(SetDb.Customer_Ort, SetDb.Customer_Ort) As Customer_Ort, 
				ISNULL(SetDb.Customer_Strasse, IsNull(SetDb.Customer_Strasse, '')) As Customer_Strasse, 
				(CASE
					WHEN ISNULL(US.User_Telefon, '') = '' THEN SetDb.Customer_Telefon
					ELSE 
                    US.User_Telefon                
				END) User_Telefon,
				(CASE
					WHEN ISNULL(US.USer_Telefax, '') = '' THEN SetDb.Customer_Telefax
					ELSE 
                    US.USer_Telefax                
				END) USer_Telefax,
				(CASE
					WHEN ISNULL(US.User_eMail, '') = '' THEN SetDb.Customer_eMail
					ELSE 
                    US.User_eMail                
				END) User_eMail,
				(CASE
					WHEN ISNULL(US.User_Homepage, '') = '' THEN SetDb.Customer_Homepage
					ELSE 
                    US.User_Homepage                
				END) User_Homepage
				From kunden KD 
				left Join MySetting SetDb On KD.Customer_ID = SetDb.KD_Guid 
				left Join Customer_Users US On KD.Customer_ID = US.Customer_ID 
				Where KD.KD_eMail <> '' And KD.KD_Guid = @Guid And US.User_ID = @USGuid
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get Customer Responsible Data For sending Notifications]
	@Guid nvarchar(255) = '',
	@USGuid nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON

Select KDZ.ZHD_Nachname, KDZ.ZHD_Vorname, KDZ.ZHD_eMail As Reciever, KD.Customer_ID, KD.KD_Language As Language, 
				kdz.Zhd_BriefAnrede, kdz.ZHDSex,
				'' MA_Nachname, '' MA_BriefAnrede, 
				IsNull((US.User_Vorname + ' ' + US.User_Nachname), '') As Berater, 
				IsNull(SetDb.KD_Guid, '') As Customer_ID, 
				IsNull(US.Customer_Name, SetDb.Customer_Name) As Customer_Name, 
				IsNull(SetDb.Customer_Ort, SetDb.Customer_Ort) As Customer_Ort, 
				IsNull(SetDb.Customer_Strasse, IsNull(SetDb.Customer_Strasse, '')) As Customer_Strasse, 
				(CASE
					WHEN ISNULL(US.User_Telefon, '') = '' THEN SetDb.Customer_Telefon
					ELSE 
                    US.User_Telefon                
				END) User_Telefon,
				(CASE
					WHEN ISNULL(US.USer_Telefax, '') = '' THEN SetDb.Customer_Telefax
					ELSE 
                    US.USer_Telefax                
				END) USer_Telefax,
				(CASE
					WHEN ISNULL(US.User_eMail, '') = '' THEN SetDb.Customer_eMail
					ELSE 
                    US.User_eMail                
				END) User_eMail,
				(CASE
					WHEN ISNULL(US.User_Homepage, '') = '' THEN SetDb.Customer_Homepage
					ELSE 
                    US.User_Homepage                
				END) User_Homepage
				From Kunden_ZHD KDZ 
				Left Join MySetting SetDb On KDZ.Customer_ID = SetDb.KD_Guid 
				left Join Kunden KD On KDz.Customer_ID = KD.Customer_ID And KDz.KD_Guid = KD.KD_Guid 
				left Join Customer_Users US On KDZ.Customer_ID = US.Customer_ID 
				Where KDZ.ZHD_eMail <> '' And KDZ.ZHD_Guid = @Guid
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Employee WOS Document Data]
	@WOSGuid nvarchar(255) = '',
	@USGuid nvarchar(255) = '',
	@Jahr int = 2016,
	@Monat INT = 0
AS
BEGIN
	SET NOCOUNT ON
	
SELECT  MA.ID ,
        MA.Customer_ID ,
        MA.MANr ,
        MA.LONr ,
        MA.RPNr ,
        MA.ESNr ,
        MA.MA_Vorname ,
        MA.MA_Nachname ,
        MA.Transfered_On ,
        MA.Transfered_User ,
        MA.LastNotification ,
        MA.Doc_Art ,
        MA.Doc_Info ,
        MA.Owner_Guid ,
        MA.Doc_Guid
FROM    [SpContract].dbo.Kandidaten_Doc_Online MA
WHERE   MA.Customer_ID = @WOSGuid
        AND ( @USGuid = ''
              OR MA.LogedUser_ID = @USGuid
            )
        AND ( @Jahr = 0
              OR YEAR(MA.Transfered_On) = @Jahr
            )
        AND ( @Monat = 0
              OR MONTH(MA.Transfered_On) = @Monat
            )
ORDER BY Transfered_On;

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List WOS Notification Data]
	@WOSGuid nvarchar(255),
	@Jahr int = 2016,
	@Monat INT = 0
AS
BEGIN
	SET NOCOUNT ON
	
SELECT  N.ID ,
        N.Customer_ID ,
        N.MailFrom ,
        N.MailTo ,
        N.Result ,
        N.[SUBJECT] ,
        N.Body MailBody ,
        N.DocLink ,
        N.Recipient_Guid ,
        N.CreatedOn
FROM    [SpContract].dbo.MailNotification N
WHERE   N.Customer_ID = @WOSGuid
        AND ( @Jahr = 0
              OR YEAR(N.CreatedOn) = @Jahr
            )
        AND ( @Monat = 0
              OR MONTH(N.CreatedOn) = @Monat
            )
ORDER BY N.CreatedOn DESC;

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get Assigned Employee WOS Document Data]
	@WOSGuid nvarchar(255) = '',
	@RecID INT
AS
BEGIN
	SET NOCOUNT ON
	
SELECT  MA.ID ,
        MA.Customer_ID ,
        MA.MANr ,
        MA.LONr ,
        MA.RPNr ,
        MA.ESNr ,
        MA.MA_Vorname ,
        MA.MA_Nachname ,
        MA.Transfered_On ,
        MA.Transfered_User ,
        MA.LastNotification ,
        MA.Doc_Art ,
        MA.Doc_Info ,
        MA.Owner_Guid ,
        MA.Doc_Guid ,
        MA.DocScan ScanContent
FROM    [SpContract].dbo.Kandidaten_Doc_Online MA
WHERE   MA.Customer_ID = @WOSGuid
        AND MA.ID = @RecID;

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get Assigned Vacancy Data By Vacancynumber]
    @UserID NVARCHAR(255) = '' ,
    @vakNumber INT 
AS 
    BEGIN

        SET NOCOUNT ON

        DECLARE @JobCategorie NVARCHAR(1000)
        DECLARE @JobDiscipline NVARCHAR(1000)
        DECLARE @JobPosition NVARCHAR(1000)

-- Beruf-Gruppe
        SELECT  @JobCategorie = ISNULL(@JobCategorie + '#', '')
                + dbo.tblVacancyJobExperience.Berufgruppe
        FROM    dbo.tblVacancyJobExperience
        WHERE   WOS_Guid = @UserID
                AND VakNr = @vakNumber

        SET @JobCategorie = ISNULL(@JobCategorie, '')

-- Beruf-Erfahrung
        SELECT  @JobDiscipline = ISNULL(@JobDiscipline + '#', '')
                + dbo.tblVacancyJobExperience.BerufErfahrung
        FROM    dbo.tblVacancyJobExperience
        WHERE   WOS_Guid = @UserID
                AND VakNr = @vakNumber

        SET @JobDiscipline = ISNULL(@JobDiscipline, '')

-- Beruf-Position
        SELECT  @JobPosition = ISNULL(@JobPosition + '#', '')
                + dbo.tblVacancyJobExperience.BerufPosition
        FROM    dbo.tblVacancyJobExperience
        WHERE   WOS_Guid = @UserID
                AND VakNr = @vakNumber

        SET @JobPosition = ISNULL(@JobPosition, '')

-- Ganzen Datensatz zurückgeben
        SELECT  
	v.[ID] ,
	v.[VakNr] ,
	v.[KDNr] ,
	v.[KDZHDNr] ,
	v.[Customer_ID] ,
	v.[Customer_Name] ,
	v.[Customer_Strasse] ,
	v.[Customer_Ort] ,
	v.[Customer_Telefon] ,
	v.[Customer_eMail] ,
	v.[Berater] ,
	v.[Filiale] ,
	v.[VakKontakt] ,
	v.[VakState] ,
	v.[Bezeichnung] ,
	v.[Slogan] ,
	v.[Gruppe] ,
	v.[SubGroup] ,
	v.[ExistLink] ,
	v.[VakLink] ,
	v.[Beginn] ,
	v.[JobProzent] ,
	v.[Anstellung] ,
	v.[Dauer] ,
	v.[MAAge] ,
	v.[MASex] ,
	v.[MAZivil] ,
	v.[MALohn] ,
	v.[Jobtime] ,
	v.[JobOrt] ,
	v.[MAFSchein] ,
	v.[MAAuto] ,
	v.[MANationality] ,
	v.[IEExport] ,
	v.[KDBeschreibung] ,
	v.[KDBietet] ,
	v.[SBeschreibung] ,
	v.[Reserve1] ,
	v.[Taetigkeit] ,
	v.[Anforderung] ,
	v.[Reserve2] ,
	v.[Reserve3] ,
	v.[Ausbildung] ,
	v.[Weiterbildung] ,
	v.[SKennt] ,
	v.[EDVKennt] ,
	v.[Branchen] ,
	v.[CreatedOn], 
	v.[CreatedFrom] ,
	v.[ChangedOn] ,
	v.[ChangedFrom] ,
	v.[Transfered_User] ,
	v.[Transfered_On] ,
	v.[Transfered_Guid] ,
	v.[Result] ,
	v.[Vak_Region] ,
	v.[Vak_Kanton] ,
	v.[MSprachen] ,
	v.[SSprachen] ,
	v.[Qualifikation] ,
	v.[SQualifikation] ,
	v.[User_Guid] ,
	v.[_KDBeschreibung] ,
	v.[_Taetigkeit] ,
	v.[_Anforderung] ,
	v.[_KDBietet] ,
	v.[JobPLZ] ,
	v.[_Reserve1] ,
	v.[_Reserve2] ,
	v.[_Reserve3] ,
	v.[_Weiterbildung] ,
	v.[_SBeschreibung] ,
	v.[_SKennt] ,
	v.[_EDVKennt] ,
	v.[_Ausbildung] ,
	v.[TitelForSearch] ,
	v.[ShortDescription] ,

        @JobCategorie AS Job_Categories ,
        @JobDiscipline AS Job_Disciplines ,
        @JobPosition AS Job_Position, 

	ISNULL((SELECT TOP 1 User_Sex FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterSex,
	ISNULL((SELECT TOP 1 User_Vorname FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterVorname,
	ISNULL((SELECT TOP 1 User_Nachname FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterNachname,
	ISNULL((SELECT TOP 1 User_EMail FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterEMail,
	ISNULL((SELECT TOP 1 User_Telefon FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterTelefon

	, (SELECT TOP 1 User_Picture FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid) AS BeraterPicture

        FROM    KD_Vakanzen V
        WHERE   ( V.WOS_Guid = @UserID )
                AND ( VakNr = @vakNumber ) 
    END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Create New Employee Document For WOS]
    @Customer_ID NVARCHAR(50),
	@WOSGuid nvarchar(50),
	@MANr INT,
    @ESNr INT ,
    @ESLohnNr INT ,
    @LONr INT ,
    @RPNr INT ,
    @RPLNr INT ,
    @RPDocNr INT ,

    @LogedUser_Guid NVARCHAR(50),
    @Berater NVARCHAR(255),
    @MA_Vorname NVARCHAR(255),
    @MA_Nachname NVARCHAR(255),
    @MA_Filiale NVARCHAR(255),
    @MA_Postfach NVARCHAR(255),
    @MA_Strasse NVARCHAR(255),
    @MA_PLZ NVARCHAR(255),
    @MA_Ort NVARCHAR(255),
    @MASex NVARCHAR(255),
    @MAZivil NVARCHAR(255),
    
	@BriefAnrede NVARCHAR(255),
    @AGB_WOS NVARCHAR(255),
    @MA_Beruf NVARCHAR(4000),
    @MA_Branche NVARCHAR(4000),
    @MA_EMail NVARCHAR(255),
    @MA_GebDat DATE ,
    @MA_Language NVARCHAR(255),
    @MA_Nationality NVARCHAR(255),
    @Transfered_User NVARCHAR(255),
    @Owner_Guid NVARCHAR(255),

    @MA_FSchein NVARCHAR(255),
    @MA_Auto NVARCHAR(255),
    @MA_Kontakt NVARCHAR(255),
    @MA_State1 NVARCHAR(255),
    @MA_State2 NVARCHAR(255),
    @MA_Eigenschaft NVARCHAR(255),
    @MA_SSprache NVARCHAR(255),
    @MA_MSprache NVARCHAR(255),
    @AHV_Nr NVARCHAR(255),
    @MA_Canton NVARCHAR(255),

    @Doc_Guid NVARCHAR(255),
    @Doc_Art NVARCHAR(255),
    @Doc_Info NVARCHAR(255),
    @Result NVARCHAR(255),

    @US_Nachname NVARCHAR(255),
    @US_Vorname NVARCHAR(255),
    @US_Telefon NVARCHAR(255),
    @US_Telefax NVARCHAR(255),
    @US_eMail NVARCHAR(255),
    @DocFileName NVARCHAR(255),
	@DocScan varbinary(MAX),

	@User_Initial NVARCHAR(4000), 
	@User_Sex NVARCHAR(4000), 
	@User_Filiale  NVARCHAR(4000), 
	@User_Picture varbinary(MAX), 
	@User_Sign varbinary(MAX) ,
	@SignTransferedDocument BIT ,

	@NewId int OUTPUT


AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

DECLARE @Customer_Name NVARCHAR(255) 
DECLARE @Customer_Ort NVARCHAR(255) 
DECLARE @Customer_Strasse NVARCHAR(255) 
DECLARE @Customer_Telefon NVARCHAR(255) 
DECLARE @Customer_Telefax NVARCHAR(255) 
DECLARE @Customer_EMail NVARCHAR(255) 
DECLARE @notificationDate DATETIME

IF @SignTransferedDocument = 1 
BEGIN
	SET @notificationDate = GETDATE()
END


SELECT TOP 1 @Customer_Name = ISNULL(customer_name, 'is not defined!!!')
	,@Customer_Ort = ISNULL(Customer_Ort, 'is not defined!!!')
	,@Customer_Strasse = ISNULL(Customer_Strasse, 'is not defined!!!')
	,@Customer_Telefon = ISNULL(Customer_Telefon, 'is not defined!!!')
	,@Customer_Telefax = ISNULL(Customer_Telefax, 'is not defined!!!')
	,@Customer_EMail = ISNULL(Customer_EMail, 'is not defined!!!')
	FROM dbo.MySetting WHERE WOS_Guid = @WOSGuid


DELETE dbo.Customer_Users WHERE WOS_Guid = @WOSGuid AND [User_ID] = @LogedUser_Guid;
DELETE dbo.kandidaten WHERE WOS_Guid = @WOSGuid AND MANr = @MANr
DELETE dbo.Kandidaten_Doc_Online WHERE WOS_Guid = @WOSGuid AND Doc_Guid = @Doc_Guid;

Insert Into Customer_Users (
					[User_ID], 
					Customer_ID, 
					WOS_Guid, 
					Customer_Name, 
					User_Initial, 
					User_Sex, 
					User_Vorname, 
					User_Nachname, 
					User_Telefon, 
					User_Telefax, 
					User_eMail, 
					User_Filiale, 
					User_Picture, 
					User_Sign, 
					CreatedOn 
					) 
					VALUES 
					(
					@LogedUser_Guid, 
					@Customer_ID, 
					@WOSGuid, 
					@Customer_Name, 
					@User_Initial, 
					@User_Sex, 
					@US_Vorname, 
					@US_Nachname, 
					@US_Telefon, 
					@US_Telefax, 
					@US_eMail, 
					@User_Filiale, 
					@User_Picture, 
					@User_Sign, 
					GETDATE()
					) 

INSERT Into dbo.kandidaten
        ( Customer_ID ,
          WOS_Guid ,
          MANr ,
          MA_Guid ,
          Berater ,
          MA_Vorname ,
          MA_Nachname ,
          MA_Kanton ,
          MA_Ort ,
          MA_Beruf ,
          MA_Branche ,
          MASex ,
          MA_EMail ,
          MA_GebDat ,
          MA_Language ,
          MA_Nationality ,
          BriefAnrede ,
          AGB_WOS ,
          Transfered_User ,
          Transfered_On
        )
VALUES  ( @Customer_ID , -- Customer_ID - nvarchar(255)
		  @WOSGuid, 
          @MANr , -- MANr - int
          @Owner_Guid , -- MA_Guid - nvarchar(100)
          @Berater , -- Berater - nvarchar(255)
          @MA_Vorname , -- MA_Vorname - nvarchar(255)
          @MA_Nachname , -- MA_Nachname - nvarchar(255)
          @MA_Canton , -- MA_Kanton - nvarchar(255)
          @MA_Ort , -- MA_Ort - nvarchar(255)
          @MA_Beruf , -- MA_Beruf - nvarchar(1000)
          @MA_Branche , -- MA_Branche - nvarchar(1000)
          @MASex , -- MASex - nvarchar(10)
          @MA_EMail , -- MA_EMail - nvarchar(255)
          @MA_GebDat , -- MA_GebDat - datetime
          @MA_Language , -- MA_Language - nvarchar(70)
          @MA_Nationality , -- MA_Nationality - nvarchar(255)
          @BriefAnrede , -- BriefAnrede - nvarchar(70)
          @AGB_WOS , -- AGB_WOS - nvarchar(70)
          @Transfered_User , -- Transfered_User - nvarchar(100)
          GETDATE()  -- Transfered_On - datetime
        )

INSERT INTO dbo.Kandidaten_Doc_Online
        ( MANr ,
          ESNr ,
          ESLohnNr ,
          LONr ,
          LogedUser_ID ,
          Customer_ID ,
          WOS_Guid ,
          Customer_Name ,
          Customer_Strasse ,
          Customer_Ort ,
          Customer_Telefon ,
          Customer_eMail ,
          Berater ,
          MA_Vorname ,
          MA_Nachname ,
          MA_Filiale ,
          MA_Kanton ,
          MA_Ort ,
          MASex ,
          MAZivil ,
          BriefAnrede ,
          AGB_WOS ,
          MA_Beruf ,
          MA_Branche ,
          MA_EMail ,
          MA_GebDat ,
          MA_Language ,
          MA_Nationality ,
          Transfered_User ,
          Transfered_On ,
          Owner_Guid ,
          Doc_Guid ,
          Doc_Art ,
          Doc_Info ,
          Result ,
          User_Nachname ,
          User_Vorname ,
          User_Telefon ,
          User_Telefax ,
          User_eMail ,
          DocFileName ,
          DocScan ,
          RPNr ,
          RPLNr ,
          RPDocNr,
		  LastNotification
        )
VALUES  ( @MANr , -- MANr - int
          @ESNr , -- ESNr - int
          @ESLohnNr , -- ESLohnNr - int
          @LONr , -- LONr - int
          @LogedUser_Guid , -- LogedUser_ID - nvarchar(255)
          @Customer_ID , -- Customer_ID - nvarchar(255)
		  @WOSGuid, 
          @Customer_Name , -- Customer_Name - nvarchar(255)
          @Customer_Strasse , -- Customer_Strasse - nvarchar(255)
          @Customer_Ort , -- Customer_Ort - nvarchar(255)
          @Customer_Telefon , -- Customer_Telefon - nvarchar(255)
          @Customer_eMail , -- Customer_eMail - nvarchar(255)
          @Berater , -- Berater - nvarchar(255)
          @MA_Vorname , -- MA_Vorname - nvarchar(255)
          @MA_Nachname , -- MA_Nachname - nvarchar(255)
          @MA_Filiale , -- MA_Filiale - nvarchar(50)
          @MA_Canton , -- MA_Kanton - nvarchar(255)
          @MA_Ort , -- MA_Ort - nvarchar(255)
          @MASex , -- MASex - nvarchar(10)
          @MAZivil , -- MAZivil - nvarchar(50)
          @BriefAnrede , -- BriefAnrede - nvarchar(70)
          @AGB_WOS , -- AGB_WOS - nvarchar(70)
          @MA_Beruf , -- MA_Beruf - nvarchar(1000)
          @MA_Branche , -- MA_Branche - nvarchar(1000)
          @MA_EMail , -- MA_EMail - nvarchar(255)
          @MA_GebDat , -- MA_GebDat - datetime
          @MA_Language , -- MA_Language - nvarchar(70)
          @MA_Nationality , -- MA_Nationality - nvarchar(255)
          @Transfered_User , -- Transfered_User - nvarchar(100)
          GETDATE() , -- Transfered_On - datetime
          @Owner_Guid , -- Owner_Guid - nvarchar(100)
          @Doc_Guid , -- Doc_Guid - nvarchar(100)
          @Doc_Art , -- Doc_Art - nvarchar(255)
          @Doc_Info , -- Doc_Info - nvarchar(255)
          @Result , -- Result - nvarchar(10)
          @US_Nachname , -- User_Nachname - nvarchar(70)
          @US_Vorname , -- User_Vorname - nvarchar(70)
          @US_Telefon , -- User_Telefon - nvarchar(70)
          @US_Telefax , -- User_Telefax - nvarchar(70)
          @US_eMail , -- User_eMail - nvarchar(70)
          @DocFileName , -- DocFileName - nvarchar(255)
          @DocScan , -- DocScan - varbinary(max)
          @RPNr , -- RPNr - int
          @RPLNr , -- RPLNr - int
          @RPDocNr , -- RPDocNr - int
		  @notificationDate
		)

		SET @NewId = SCOPE_IDENTITY() 
			

		IF @StartTranCount = 0 COMMIT TRAN
		
	END TRY
	BEGIN CATCH
		IF @StartTranCount = 0 AND @@trancount > 0
		BEGIN
			ROLLBACK TRAN
			DECLARE @message NVARCHAR(MAX)
			DECLARE @state INT
			SELECT @message = ERROR_MESSAGE(), @state = ERROR_STATE()
			RAISERROR (@message, 11, @state)
		END

END CATCH

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete Selected Employee Document]
	@UserID nvarchar(255) = '',
	@MA_Guid nvarchar(255) = '',
	@MANr int = 0,
	@Doc_Guid nvarchar(255) = '', 
	@Doc_Art nvarchar(255) = '', 
	@Doc_Info nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	Delete Kandidaten_Doc_Online 
	WHERE (Kandidaten_Doc_Online.Customer_ID = @UserID OR Kandidaten_Doc_Online.WOS_Guid = @UserID)
	And (Kandidaten_Doc_Online.Owner_Guid = @MA_Guid)
	And (Kandidaten_Doc_Online.MANr = @MANr)
	And (@Doc_Guid = '' OR Kandidaten_Doc_Online.Doc_Guid = @Doc_Guid)
	And (Kandidaten_Doc_Online.Doc_Art = @Doc_Art)
	And (Kandidaten_Doc_Online.Doc_Info = @Doc_Info)
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Update WOS Customer User Data]
	@LogedUser_Guid NVARCHAR(255),
	@WOSCustomerID NVARCHAR(255) ,
	@US_Nachname NVARCHAR(4000),
	@US_Vorname NVARCHAR(4000),
	@US_Telefon  NVARCHAR(4000),
	@US_Telefax  NVARCHAR(4000),
	@US_eMail NVARCHAR(4000),
	@Customer_Name NVARCHAR(4000), 
	@User_Initial NVARCHAR(4000), 
	@User_Sex NVARCHAR(4000), 
	@User_Filiale  NVARCHAR(4000)

AS

BEGIN
SET NOCOUNT ON


UPDATE dbo.Customer_Users SET 
					Customer_Name = @Customer_Name, 
					User_Vorname = @US_Vorname, 
					User_Nachname = @US_Nachname, 
					User_Telefon = @US_Telefon, 
					User_Telefax = @US_Telefax, 
					User_eMail = @US_eMail, 
					User_Filiale = @User_Filiale
					WHERE  [User_ID] = @LogedUser_Guid
					AND Customer_ID = @WOSCustomerID


END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Vak-SubGruppe]
	@UserID nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KD_Vakanzen.SubGroup
	FROM KD_Vakanzen
	WHERE (KD_Vakanzen.WOS_Guid = @UserID) 
	And (KD_Vakanzen.SubGroup <> '' Or KD_Vakanzen.SubGroup Is Not Null)
	Group By KD_Vakanzen.SubGroup
	ORDER BY KD_Vakanzen.SubGroup
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Vacancy SubGruppe For Assigned Group]
	@UserID nvarchar(255) = '',
	@Group nvarchar(255) = ''
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT V.SubGroup
	FROM dbo.KD_Vakanzen V
	WHERE (V.Customer_ID = @UserID) 
	And (V.Gruppe = @Group)
	And (ISNULL(V.SubGroup, '') <> '' Or V.SubGroup Is Not Null)
	Group By V.SubGroup
	ORDER BY V.SubGroup
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Available Candidates]
	@WOSGuid nvarchar(255),
	@Beruf nvarchar(255) = '',
	@Ort nvarchar(255) = '',
	@Kanton nvarchar(255) = '', 
	@Filiale nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON

	SET @Beruf = '%' + @Beruf + '%'
	SET @Ort = '%' + @Ort + '%'
	SET @Kanton = '%' + @Kanton + '%'
	SET @Filiale = '%' + @Filiale + '%'

	SELECT MA.ID,
           MA.MANr,
           MA.Customer_ID,
           MA.Customer_Name,
           MA.Customer_Strasse,
           MA.Customer_Ort,
           MA.Customer_Telefon,
           MA.Customer_eMail,
           MA.Berater,
           MA.MA_Vorname,
           MA.MA_Nachname,
           MA.MA_Filiale,
           MA.MA_Kanton,
           MA.MA_Ort,
           MA.MA_Kontakt,
           MA.MA_State1,
           MA.MA_State2,
           MA.MA_Beruf,
           MA.JobProzent,
           MA.MAGebDat,
           MA.MASex,
           MA.MAZivil,
           MA.MAFSchein,
           MA.MAAuto,
           MA.MANationality,
           MA.Bewillig,
           MA.BriefAnrede,
           MA.MA_Res1,
           MA.MA_Res2,
           MA.MA_Res3,
           MA.MA_Res4,
MA.MA_Res5,
           MA.Transfered_User,
           MA.Transfered_On,
           MA.Transfered_Guid,
           MA.Result,
           MA.User_Nachname,
           MA.User_Vorname,
           MA.User_Telefon,
           MA.User_Telefax,
           MA.User_eMail,
           MA.MA_SSprache,
           MA.MA_MSprache,
           MA.MA_Eigenschaft,
MA.Advisor_ID,
MA.AssignedCustomer_ID
	FROM dbo.Kandidaten_Online MA
	WHERE (MA.Customer_ID =  @WOSGuid) 
	And (MA.MA_Beruf Like @Beruf Or @Beruf = '') 
	And (MA.MA_Ort Like @Ort  Or @Ort = '') 
	And (MA.MA_Kanton = @Kanton Or @Kanton = '') 
	And (MA.MA_Filiale = @Filiale Or @Filiale = '') 
	ORDER BY MA.Transfered_On DESC
	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[List Vacancy Job Positions]
	@UserID nvarchar(255) = '',
	@JobCategories nvarchar(255) = '',
	@Jobdisciplines nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON
	
	SELECT BerufPosition
	FROM dbo.tblVacancyJobExperience
	WHERE (WOS_Guid = @UserID)
	And (@JobCategories = '' OR Berufgruppe = @JobCategories)
	And (@Jobdisciplines = '' OR BerufErfahrung = @Jobdisciplines)
	And (ISNULL(BerufPosition, '') <> '')
	Group By BerufPosition 
	ORDER BY BerufPosition 

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get Vakrec By ID Filtered]
    @UserID NVARCHAR(50) = '' ,
    @RecID INT = 0
AS 
    BEGIN

        SET NOCOUNT ON

        DECLARE @JobCategorie NVARCHAR(1000)
        DECLARE @JobDiscipline NVARCHAR(1000)
        DECLARE @JobPosition NVARCHAR(1000)

-- Beruf-Gruppe
        SELECT  @JobCategorie = ISNULL(@JobCategorie + '#', '')
                + dbo.tblVacancyJobExperience.Berufgruppe
        FROM    dbo.tblVacancyJobExperience
        WHERE   WOS_Guid = @UserID
                AND VakNr = ISNULL(( SELECT Vaknr
                                     FROM   dbo.KD_Vakanzen
                                     WHERE  WOS_Guid = @UserID
                                            AND ID = @RecID
                                   ), 0)
        SET @JobCategorie = ISNULL(@JobCategorie, '')

-- Beruf-Erfahrung
        SELECT  @JobDiscipline = ISNULL(@JobDiscipline + '#', '')
                + dbo.tblVacancyJobExperience.BerufErfahrung
        FROM    dbo.tblVacancyJobExperience
        WHERE   WOS_Guid = @UserID
                AND VakNr = ISNULL(( SELECT Vaknr
                                     FROM   dbo.KD_Vakanzen
                                     WHERE  WOS_Guid = @UserID
                                            AND ID = @RecID
                                   ), 0)
        SET @JobDiscipline = ISNULL(@JobDiscipline, '')

-- Beruf-Position
        SELECT  @JobPosition = ISNULL(@JobPosition + '#', '')
                + dbo.tblVacancyJobExperience.BerufPosition
        FROM    dbo.tblVacancyJobExperience
        WHERE   WOS_Guid = @UserID
                AND VakNr = ISNULL(( SELECT Vaknr
                                     FROM   dbo.KD_Vakanzen
                                     WHERE  WOS_Guid = @UserID
                                            AND ID = @RecID
                                   ), 0)
        SET @JobPosition = ISNULL(@JobPosition, '')

-- Ganzen Datensatz zurückgeben
        SELECT  
	v.[ID] ,
	v.[VakNr] ,
	v.[KDNr] ,
	v.[KDZHDNr] ,
	v.[Customer_ID] ,
	v.[Customer_Name] ,
	v.[Customer_Strasse] ,
	v.[Customer_Ort] ,
	v.[Customer_Telefon] ,
	v.[Customer_eMail] ,
	v.[Berater] ,
	v.[Filiale] ,
	v.[VakKontakt] ,
	v.[VakState] ,
	v.[Bezeichnung] ,
	v.[Slogan] ,
	v.[Gruppe] ,
	v.[SubGroup] ,
	v.[ExistLink] ,
	v.[JobChannelPriority] ,
	v.[VakLink] ,
	v.[Beginn] ,
	v.[JobProzent] ,
	v.[Anstellung] ,
	v.[Dauer] ,
	v.[MAAge] ,
	v.[MASex] ,
	v.[MAZivil] ,
	v.[MALohn] ,
	v.[Jobtime] ,
	v.[JobOrt] ,
	v.[MAFSchein] ,
	v.[MAAuto] ,
	v.[MANationality] ,
	v.[IEExport] ,
	v.[KDBeschreibung] ,
	v.[KDBietet] ,
	v.[SBeschreibung] ,
	v.[Reserve1] ,
	v.[Taetigkeit] ,
	v.[Anforderung] ,
	v.[Reserve2] ,
	v.[Reserve3] ,
	v.[Ausbildung] ,
	v.[Weiterbildung] ,
	v.[SKennt] ,
	v.[EDVKennt] ,
	v.[Branchen] ,
	v.[CreatedOn], 
	v.[CreatedFrom] ,
	v.[ChangedOn] ,
	v.[ChangedFrom] ,
	v.[Transfered_User] ,
	v.[Transfered_On] ,
	v.[Transfered_Guid] ,
	v.[Result] ,
	v.[Vak_Region] ,
	v.[Vak_Kanton] ,
	v.[MSprachen] ,
	v.[SSprachen] ,
	v.[Qualifikation] ,
	v.[SQualifikation] ,
	v.[User_Guid] ,
	v.[_KDBeschreibung] ,
	v.[_Taetigkeit] ,
	v.[_Anforderung] ,
	v.[_KDBietet] ,
	v.[JobPLZ] ,
	v.[_Reserve1] ,
	v.[_Reserve2] ,
	v.[_Reserve3] ,
	v.[_Weiterbildung] ,
	v.[_SBeschreibung] ,
	v.[_SKennt] ,
	v.[_EDVKennt] ,
	v.[_Ausbildung] ,
	v.[TitelForSearch] ,
	v.[ShortDescription] ,

        @JobCategorie AS Job_Categories ,
        @JobDiscipline AS Job_Disciplines ,
        @JobPosition AS Job_Position, 

	ISNULL((SELECT TOP 1 User_Sex FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterSex,
	ISNULL((SELECT TOP 1 User_Vorname FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterVorname,
	ISNULL((SELECT TOP 1 User_Nachname FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterNachname,
	ISNULL((SELECT TOP 1 User_EMail FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterEMail,
	ISNULL((SELECT TOP 1 User_Telefon FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterTelefon
	
	, (SELECT TOP 1 User_Picture FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid) AS BeraterPicture

        FROM    dbo.KD_Vakanzen V
        WHERE   ( V.WOS_Guid = @UserID )
                AND ( V.ID = @RecID ) 
    END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Transfered Vacancy Data From WOS]
	@Customer_ID nvarchar(50),
	@WOSGuid nvarchar(50) ,
	@KDNumber INT,
	@VacancyNumber INT

AS
BEGIN

SET NOCOUNT ON
	
SELECT V.ID,
       V.VakNr,
       V.KDNr,
       V.KDZHDNr,
       V.Customer_ID,
       V.Customer_Name,
       V.Customer_Strasse,
       V.Customer_Ort,
       V.Customer_Telefon,
       V.Customer_eMail,
       V.Berater,
       V.Filiale,
       V.VakKontakt,
       V.VakState,
       V.Bezeichnung,
       V.Slogan,
       V.Gruppe,
       V.ExistLink,
       V.VakLink,
       V.Beginn,
       V.JobProzent,
       V.Anstellung,
       V.Dauer,
       V.MAAge,
       V.MASex,
       V.MAZivil,
       V.MALohn,
       V.Jobtime,
       V.JobOrt,
       V.MAFSchein,
       V.MAAuto,
       V.MANationality,
       V.IEExport,
       V.KDBeschreibung,
       V.KDBietet,
       V.SBeschreibung,
       V.Reserve1,
       V.Taetigkeit,
       V.Anforderung,
       V.Reserve2,
       V.Reserve3,
       V.Ausbildung,
       V.Weiterbildung,
       V.SKennt,
       V.EDVKennt,
       V.Branchen,
       V.CreatedOn,
       V.CreatedFrom,
       V.ChangedOn,
       V.ChangedFrom,
       V.Transfered_User,
       V.Transfered_On,
       V.Transfered_Guid,
       V.Result,
       V.Vak_Region,
       V.Vak_Kanton,
       V.MSprachen,
       V.SSprachen,
       V.Qualifikation,
       V.SQualifikation,
       V.User_Guid,
       V._KDBeschreibung,
       V._Taetigkeit,
       V._Anforderung,
       V._KDBietet,
       V.JobPLZ,
       V._Reserve1,
       V._Reserve2,
       V._Reserve3,
       V._Weiterbildung,
       V._SBeschreibung,
       V._SKennt,
       V._EDVKennt,
       V._Ausbildung,
       V.Job_Categories,
       V.Job_Disciplines,
       V.Job_Position,
       V.TitelForSearch,
       V.ShortDescription,
       V.SBNNumber,
       V.SubGroup,
       V.WOS_Guid,
       V.JobChannelPriority 
,	ISNULL((SELECT TOP 1 User_Sex FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterSex
,	ISNULL((SELECT TOP 1 User_Vorname FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterVorname
,	ISNULL((SELECT TOP 1 User_Nachname FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterNachname
,	ISNULL((SELECT TOP 1 User_EMail FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterEMail
,	ISNULL((SELECT TOP 1 User_Telefon FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterTelefon

,	(SELECT TOP 1 User_Picture FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid) AS BeraterPicture
,
CONVERT(BIT, (CASE
	WHEN ISNULL( (SELECT TOP 1 ISNULL(J.ID, 0) FROM dbo.tblJobCHPlattform J WHERE J.VakNr = V.VakNr AND J.Customer_Guid = @Customer_ID), 0) > 0 THEN 1 ELSE 0
END
) ) JobCHOnline
, 
CONVERT(BIT, (CASE 
	WHEN ISNULL( (SELECT TOP 1 ISNULL(O.ID, 0) FROM dbo.tblOstJobCHPlattform O WHERE O.VakNr = V.VakNr AND O.Customer_Guid = @Customer_ID), 0) > 0 THEN 1 ELSE 0
END
) ) OstJobOnline

FROM dbo.KD_Vakanzen V
WHERE 
	(V.Customer_ID = @Customer_ID) 
	AND (V.WOS_Guid = @WOSGuid) 
	AND (ISNULL(@KDNumber, 0) = 0 OR V.KDNr = @KDNumber)
	AND (ISNULL(@VacancyNumber, 0) = 0 OR V.VakNr = @VacancyNumber)
	
  ORDER BY V.Transfered_On DESC

END;
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete Assigned Vacancy From WOS]
	@Customer_ID NVARCHAR(50),
	@WOSGuid NVARCHAR(50),

	@VakNr INT

AS

BEGIN
SET NOCOUNT ON

DELETE dbo.KD_Vakanzen WHERE Customer_ID = @Customer_ID AND WOS_Guid = @WOSGuid AND VakNr = @VakNr;
DELETE dbo.tblVacancyJobExperience Where Customer_Guid = @Customer_ID AND WOS_Guid = @WOSGuid AND VakNr = @VakNr;

DELETE dbo.tblJobCHPlattform Where Customer_Guid = @Customer_ID And VakNr = @VakNr;
Delete dbo.tblOstJobCHPlattform Where Customer_Guid = @Customer_ID And VakNr = @VakNr;

END

GO


SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[Create Assigned Available Employee For WOS]
		@Customer_ID NVARCHAR(50),
		@WOSGuid NVARCHAR(50),
		@EmployeeNumber INT,
		@Employee_Advisor nvarchar(255), 
		@Employee_Filiale nvarchar(255), 
		@Employee_Canton nvarchar(255), 
		@Postcode nvarchar(50), 
		@Location nvarchar(255), 
		@Firstname nvarchar(255), 
		@Lastname nvarchar(255), 
		@HowContact nvarchar(255), 
		@FirstState nvarchar(255), 
		@SecondState nvarchar(255), 
		@Qualification nvarchar(255), 
		@Branches nvarchar(255), 
		@JobProzent nvarchar(255), 
		@BirthDate datetime, 
		@Gender nvarchar(255), 
		@Civilstate nvarchar(255), 
		@FSchein nvarchar(255), 
		@Auto nvarchar(255), 
		@Nationality nvarchar(255), 
		@Permission nvarchar(255), 
		@Salutation nvarchar(255), 

		@MA_Res1 nvarchar(255), 
		@MA_Res2 nvarchar(255), 
		@MA_Res3 nvarchar(255), 
		@MA_Res4 nvarchar(255), 
		@MA_Res5 nvarchar(255), 

		@LL_Name nvarchar(255), 
		@ApplicationReserve0 nvarchar(MAX), 
		@ApplicationReserve1 nvarchar(MAX), 
		@ApplicationReserve2 nvarchar(MAX ), 
		@ApplicationReserve3 nvarchar(MAX), 
		@ApplicationReserve4 nvarchar(MAX), 
		@ApplicationReserve5 nvarchar(MAX), 
		@ApplicationReserve6 nvarchar(MAX), 
		@ApplicationReserve7 nvarchar(MAX), 
		@ApplicationReserve8 nvarchar(MAX), 
		@ApplicationReserve9 nvarchar(MAX), 
		@ApplicationReserve10 nvarchar(MAX), 
		@ApplicationReserve11 nvarchar(MAX), 
		@ApplicationReserve12 nvarchar(MAX), 
		@ApplicationReserve13 nvarchar(MAX), 
		@ApplicationReserve14 nvarchar(MAX), 
		@ApplicationReserve15 nvarchar(MAX), 

		@ApplicationReserveRtf0 nvarchar(MAX), 
		@ApplicationReserveRtf1 nvarchar(MAX), 
		@ApplicationReserveRtf2 nvarchar(MAX ), 
		@ApplicationReserveRtf3 nvarchar(MAX), 
		@ApplicationReserveRtf4 nvarchar(MAX), 
		@ApplicationReserveRtf5 nvarchar(MAX), 
		@ApplicationReserveRtf6 nvarchar(MAX), 
		@ApplicationReserveRtf7 nvarchar(MAX), 
		@ApplicationReserveRtf8 nvarchar(MAX), 
		@ApplicationReserveRtf9 nvarchar(MAX), 
		@ApplicationReserveRtf10 nvarchar(MAX), 
		@ApplicationReserveRtf11 nvarchar(MAX), 
		@ApplicationReserveRtf12 nvarchar(MAX), 
		@ApplicationReserveRtf13 nvarchar(MAX), 
		@ApplicationReserveRtf14 nvarchar(MAX), 
		@ApplicationReserveRtf15 nvarchar(MAX), 

		@MainLanguage nvarchar(70), 
		@MA_MLanguage nvarchar(255), 
		@MA_SLanguage nvarchar(255), 
		@MA_Eigenschaft nvarchar(255), 
		@Transfer_UserID nvarchar(255),
		@Transfered_Guid nvarchar(50),
		@Advisor_ID NVARCHAR(50),

		@DesiredWagesOld DECIMAL(8, 2),
		@DesiredWagesNew DECIMAL(8, 2),
		@DesiredWagesInMonth DECIMAL(8, 2),
		@DesiredWagesInHour DECIMAL(8, 2),

		@NewId int OUTPUT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int
	DECLARE @NextApplicationNr int
	DECLARE @NextApplicantNr int
	DECLARE @ShowEmployeeAsAvailable BIT = 0

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

SELECT @ShowEmployeeAsAvailable = ISNULL(ShowAsAvailable, 0) FROM dbo.Kandidaten_Online WHERE WOS_Guid = @WOSGuid AND Customer_ID = @Customer_ID AND MANr = @EmployeeNumber AND ShowAsAvailable = 1 ;
DELETE dbo.Kandidaten_Online WHERE WOS_Guid = @WOSGuid AND Customer_ID = @Customer_ID AND MANr = @EmployeeNumber ;
DELETE dbo.tbl_Available_Employee_ApplicationData WHERE WOS_Guid = @WOSGuid AND Customer_ID = @Customer_ID AND EmployeeNr = @EmployeeNumber ;
DELETE dbo.[tbl_Employee_Online_Template_Document] WHERE WOS_Guid = @WOSGuid AND Customer_ID = @Customer_ID AND EmployeeNr = @EmployeeNumber ;

Insert Into dbo.Kandidaten_Online (
		MANr, 
		WOS_Guid, 
		AssignedCustomer_ID, 
		Customer_ID, 
		Advisor_ID, 
		Berater, 
		MA_Filiale, 
		MA_Kanton, 
		MA_PLZ, 
		MA_Ort, 
		MA_Vorname, 
		MA_Nachname, 
		MA_Kontakt, 
		MA_State1, 
		MA_State2, 
		Branches, 
		MA_Beruf, 
		JobProzent, 
		MAGebDat, 
		MASex, 
		MAZivil, 
		MAFSchein, 
		MAAuto, 
		MANationality, 
		Bewillig, 
		BriefAnrede, 
		MA_Res1, 
		MA_Res2, 
		MA_Res3, 
		MA_Res4, 
		MA_Res5, 
		MainLanguage, 
		MA_MSprache, 
		MA_SSprache, 
		MA_Eigenschaft,
		ShowAsAvailable,
		DesiredWagesOld,
		DesiredWagesNew,
		DesiredWagesInMonth,
		DesiredWagesInHour,
		Transfered_User ,
		Transfered_Guid ,	
		Transfered_On	
		) 
		VALUES 
		(
		@EmployeeNumber, 
		@WOSGuid, 
		@Customer_ID, 
		@Customer_ID, 
		@Advisor_ID, 
		@Employee_Advisor, 
		@Employee_Filiale, 
		@Employee_Canton, 
		@Postcode, 
		@Location, 
		@Firstname, 
		@Lastname, 
		@HowContact, 
		@FirstState, 
		@SecondState, 
		@Branches, 
		@Qualification, 
		@JobProzent, 
		@BirthDate, 
		@Gender, 
		@Civilstate, 
		@FSchein, 
		@Auto, 
		@Nationality, 
		@Permission, 
		@Salutation, 
		@MA_Res1, 
		@MA_Res2, 
		@MA_Res3, 
		@MA_Res4, 
		@MA_Res5, 
		@MainLanguage, 
		@MA_MLanguage, 
		@MA_SLanguage, 
		@MA_Eigenschaft,
		@ShowEmployeeAsAvailable ,
		@DesiredWagesOld,
		@DesiredWagesNew,
		@DesiredWagesInMonth,
		@DesiredWagesInHour,
		@Transfer_UserID ,
		@Transfered_Guid,
		GETDATE()
		)

		SET @NewId = SCOPE_IDENTITY()

INSERT INTO dbo.tbl_Available_Employee_ApplicationData
(
    Customer_ID,
    WOS_Guid,
    EmployeeNr,
    LL_Name,
    Employee_Guid,
    ApplicationReserve0,
    ApplicationReserve1,
    ApplicationReserve2,
    ApplicationReserve3,
    ApplicationReserve4,
    ApplicationReserve5,
    ApplicationReserve6,
    ApplicationReserve7,
    ApplicationReserve8,
    ApplicationReserve9,
    ApplicationReserve10,
    ApplicationReserve11,
    ApplicationReserve12,
    ApplicationReserve13,
    ApplicationReserve14,
    ApplicationReserve15,
    ApplicationReserveRtf0,
    ApplicationReserveRtf1,
    ApplicationReserveRtf2,
    ApplicationReserveRtf3,
    ApplicationReserveRtf4,
    ApplicationReserveRtf5,
    ApplicationReserveRtf6,
    ApplicationReserveRtf7,
    ApplicationReserveRtf8,
    ApplicationReserveRtf9,
    ApplicationReserveRtf10,
    ApplicationReserveRtf11,
    ApplicationReserveRtf12,
    ApplicationReserveRtf13,
    ApplicationReserveRtf14,
    ApplicationReserveRtf15,
    CreatedOn,
    CreatedFrom
)
VALUES
(   @Customer_ID,       -- Customer_ID - nvarchar(255)
    @WOSGuid,       -- WOS_ID - nvarchar(255)
    @EmployeeNumber,         -- EmployeeNr - int
    @LL_Name,       -- LL_Name - nvarchar(255)
    @Transfered_Guid,       -- Employee_Guid - nvarchar(255)
    @ApplicationReserve0,       -- ApplicationReserve0 - nvarchar(max)
    @ApplicationReserve1,       -- ApplicationReserve1 - nvarchar(max)
    @ApplicationReserve2,       -- ApplicationReserve2 - nvarchar(max)
    @ApplicationReserve3,       -- ApplicationReserve3 - nvarchar(max)
    @ApplicationReserve4,       -- ApplicationReserve4 - nvarchar(max)
    @ApplicationReserve5,       -- ApplicationReserve5 - nvarchar(max)
    @ApplicationReserve6,       -- ApplicationReserve6 - nvarchar(max)
    @ApplicationReserve7,       -- ApplicationReserve7 - nvarchar(max)
    @ApplicationReserve8,       -- ApplicationReserve8 - nvarchar(max)
    @ApplicationReserve9,       -- ApplicationReserve9 - nvarchar(max)
    @ApplicationReserve10,       -- ApplicationReserve10 - nvarchar(max)
    @ApplicationReserve11,       -- ApplicationReserve11 - nvarchar(max)
    @ApplicationReserve12,       -- ApplicationReserve12 - nvarchar(max)
    @ApplicationReserve13,       -- ApplicationReserve13 - nvarchar(max)
    @ApplicationReserve14,       -- ApplicationReserve14 - nvarchar(max)
    @ApplicationReserve15,       -- ApplicationReserve15 - nvarchar(max)
    @ApplicationReserveRtf0,       -- ApplicationReserveRtf0 - nvarchar(max)
    @ApplicationReserveRtf1,       -- ApplicationReserveRtf1 - nvarchar(max)
    @ApplicationReserveRtf2,       -- ApplicationReserveRtf2 - nvarchar(max)
    @ApplicationReserveRtf3,       -- ApplicationReserveRtf3 - nvarchar(max)
    @ApplicationReserveRtf4,       -- ApplicationReserveRtf4 - nvarchar(max)
    @ApplicationReserveRtf5,       -- ApplicationReserveRtf5 - nvarchar(max)
    @ApplicationReserveRtf6,       -- ApplicationReserveRtf6 - nvarchar(max)
    @ApplicationReserveRtf7,       -- ApplicationReserveRtf7 - nvarchar(max)
    @ApplicationReserveRtf8,       -- ApplicationReserveRtf8 - nvarchar(max)
    @ApplicationReserveRtf9,       -- ApplicationReserveRtf9 - nvarchar(max)
    @ApplicationReserveRtf10,       -- ApplicationReserveRtf10 - nvarchar(max)
    @ApplicationReserveRtf11,       -- ApplicationReserveRtf11 - nvarchar(max)
    @ApplicationReserveRtf12,       -- ApplicationReserveRtf12 - nvarchar(max)
    @ApplicationReserveRtf13,       -- ApplicationReserveRtf13 - nvarchar(max)
    @ApplicationReserveRtf14,       -- ApplicationReserveRtf14 - nvarchar(max)
    @ApplicationReserveRtf15,       -- ApplicationReserveRtf15 - nvarchar(max)
    GETDATE(), -- CreatedOn - datetime
    @Transfer_UserID        -- CreatedFrom - nvarchar(255)
    )								
			
		IF @StartTranCount = 0 COMMIT TRAN
		
	END TRY
	BEGIN CATCH
		IF @StartTranCount = 0 AND @@trancount > 0
		BEGIN
			ROLLBACK TRAN
			DECLARE @message NVARCHAR(MAX)
			DECLARE @state INT
			SELECT @message = ERROR_MESSAGE(), @state = ERROR_STATE()
			RAISERROR (@message, 11, @state)
		END

END CATCH

END;
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--[List Available Employee For WOS] 'WOS-2AC6-4f9f-839F-1E4E9D6D9E2A'
CREATE PROCEDURE [dbo].[List Available Employee For WOS]
	@WOSGuid nvarchar(255),
	@Beruf nvarchar(255) = '',
	@Ort nvarchar(255) = '',
	@Kanton nvarchar(255) = '', 
	@Filiale nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON

	SELECT MA.ID,
           MA.MANr,
           MA.Customer_ID,
           MA.WOS_Guid,
		   (
		   CASE 
		   WHEN ISNULL(MA.Customer_Name, '') = '' THEN (SELECT TOP 1 U.Customer_Name FROM dbo.MySetting U WHERE U.WOS_Guid = @WOSGuid) ELSE MA.Customer_Name
		   END ) Customer_Name,
		   (
		   CASE 
		   WHEN ISNULL(MA.Customer_Strasse, '') = '' THEN (SELECT TOP 1 U.Customer_Strasse FROM dbo.MySetting U WHERE U.WOS_Guid = @WOSGuid) ELSE MA.Customer_Strasse
		   END ) Customer_Strasse,
		   (
		   CASE 
		   WHEN ISNULL(MA.Customer_Ort, '') = '' THEN (SELECT TOP 1 U.Customer_Ort FROM dbo.MySetting U WHERE U.WOS_Guid = @WOSGuid) ELSE MA.Customer_Ort
		   END ) Customer_Ort,
		   (
		   CASE 
		   WHEN ISNULL(MA.Customer_Telefon, '') = '' THEN (SELECT TOP 1 U.Customer_Telefon FROM dbo.MySetting U WHERE U.WOS_Guid = @WOSGuid) ELSE MA.Customer_Telefon
		   END ) Customer_Telefon,
		   (
		   CASE 
		   WHEN ISNULL(MA.Customer_eMail, '') = '' THEN (SELECT TOP 1 U.Customer_eMail FROM dbo.MySetting U WHERE U.WOS_Guid = @WOSGuid) ELSE MA.Customer_eMail
		   END ) Customer_eMail,

           MA.Berater,
           MA.MA_Vorname,
           MA.MA_Nachname,
           MA.MA_Filiale,
           MA.MA_Kanton,
           MA.MA_PLZ,
            MA.MA_Ort,
          MA.MA_Kontakt,
           MA.MA_State1,
           MA.MA_State2,
           MA.Branches,
           MA.MA_Beruf,
           MA.JobProzent,
           MA.MAGebDat,
           MA.MASex,
           MA.MAZivil,
           MA.MAFSchein,
           MA.MAAuto,
           MA.MANationality,
           MA.Bewillig,
           MA.BriefAnrede,
           MA.MA_Res1,
           MA.MA_Res2,
           MA.MA_Res3,
           MA.MA_Res4,
		   MA.MA_Res5,
		   MA.Transfered_User,
           MA.Transfered_On,
           MA.Transfered_Guid,
           
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.User_Nachname ELSE (SELECT TOP 1 U.User_Nachname FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_Nachname,
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.User_Vorname ELSE (SELECT TOP 1 U.User_Vorname FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_Vorname,
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.User_Telefon ELSE (SELECT TOP 1 U.User_Telefon FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_Telefon,
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.User_Telefax ELSE (SELECT TOP 1 U.User_Telefax FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_Telefax,
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.User_eMail ELSE (SELECT TOP 1 U.User_eMail FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_eMail,
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.Berater ELSE (SELECT TOP 1 U.User_Initial FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_Initial,
		   MA.MainLanguage,
           MA.MA_SSprache,
           MA.MA_MSprache,
           MA.MA_Eigenschaft,
MA.Advisor_ID,
CONVERT(BIT, ISNULL((SELECT TOP (1) ID FROM Dbo.[tbl_Employee_Online_Template_Document] MT WHERE MT.WOS_Guid = @WOSGuid AND MT.EmployeeNr = MA.MANr ), 0)) AS ExistsTemplate,
MA.AssignedCustomer_ID,
      MA.DesiredWagesOld,
      MA.DesiredWagesNew,
      MA.DesiredWagesInMonth,
      MA.DesiredWagesInHour
	FROM dbo.Kandidaten_Online MA
	WHERE (MA.WOS_Guid =  @WOSGuid) 
	AND EXISTS (SELECT TOP 1 AEmployee.EmployeeNr FROM dbo.tbl_Available_Employee_ApplicationData AEmployee WHERE AEmployee.WOS_Guid = @WOSGuid AND AEmployee.EmployeeNr = MA.MANr)
	And (MA.MA_Beruf Like @Beruf Or @Beruf = '') 
	And (MA.MA_Ort Like @Ort  Or @Ort = '') 
	And (MA.MA_Kanton = @Kanton Or @Kanton = '') 
	And (MA.MA_Filiale = @Filiale Or @Filiale = '') 
	ORDER BY MA.Transfered_On DESC
	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Assigned Available Employee Data]
	@WOSGuid nvarchar(50),
	@Customer_ID nvarchar(50) = '',
	@EmployeeNumber INT

AS
BEGIN
	SET NOCOUNT ON

	SELECT MA.ID,
           MA.MANr,
           MA.Customer_ID,
           MA.WOS_Guid,
CONVERT(BIT, ISNULL((SELECT TOP (1) ID FROM Dbo.[tbl_Employee_Online_Template_Document] MT WHERE MT.WOS_Guid = @WOSGuid AND MT.EmployeeNr = MA.MANr ), 0)) AS ExistsTemplate,
		   (
		   CASE 
		   WHEN ISNULL(MA.Customer_Name, '') = '' THEN (SELECT TOP 1 U.Customer_Name FROM dbo.MySetting U WHERE U.WOS_Guid = @WOSGuid) ELSE MA.Customer_Name
		   END ) Customer_Name,
		   (
		   CASE 
		   WHEN ISNULL(MA.Customer_Strasse, '') = '' THEN (SELECT TOP 1 U.Customer_Strasse FROM dbo.MySetting U WHERE U.WOS_Guid = @WOSGuid) ELSE MA.Customer_Strasse
		   END ) Customer_Strasse,
		   (
		   CASE 
		   WHEN ISNULL(MA.Customer_Ort, '') = '' THEN (SELECT TOP 1 U.Customer_Ort FROM dbo.MySetting U WHERE U.WOS_Guid = @WOSGuid) ELSE MA.Customer_Ort
		   END ) Customer_Ort,
		   (
		   CASE 
		   WHEN ISNULL(MA.Customer_Telefon, '') = '' THEN (SELECT TOP 1 U.Customer_Telefon FROM dbo.MySetting U WHERE U.WOS_Guid = @WOSGuid) ELSE MA.Customer_Telefon
		   END ) Customer_Telefon,
		   (
		   CASE 
		   WHEN ISNULL(MA.Customer_eMail, '') = '' THEN (SELECT TOP 1 U.Customer_eMail FROM dbo.MySetting U WHERE U.WOS_Guid = @WOSGuid) ELSE MA.Customer_eMail
		   END ) Customer_eMail,

           MA.Berater,
           MA.MA_Vorname,
           MA.MA_Nachname,
           MA.MA_Filiale,
           MA.MA_Kanton,
           MA.MA_Ort,
           MA.MA_Kontakt,
           MA.MA_State1,
           MA.MA_State2,
           MA.Branches,
           MA.MA_Beruf,
           MA.JobProzent,
           MA.MAGebDat,
           MA.MASex,
           MA.MAZivil,
           MA.MAFSchein,
           MA.MAAuto,
           MA.MANationality,
           MA.Bewillig,
           MA.BriefAnrede,
           MA.MA_Res1,
           MA.MA_Res2,
           MA.MA_Res3,
           MA.MA_Res4,
		   MA.MA_Res5,
		   MA.Transfered_User,
           MA.Transfered_On,
           MA.Transfered_Guid,
           
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.User_Nachname ELSE (SELECT TOP 1 U.User_Nachname FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_Nachname,
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.User_Vorname ELSE (SELECT TOP 1 U.User_Vorname FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_Vorname,
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.User_Telefon ELSE (SELECT TOP 1 U.User_Telefon FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_Telefon,
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.User_Telefax ELSE (SELECT TOP 1 U.User_Telefax FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_Telefax,
		   (
		   CASE 
		   WHEN ISNULL(MA.Advisor_ID, '') = '' THEN MA.User_eMail ELSE (SELECT TOP 1 U.User_eMail FROM dbo.Customer_Users U WHERE U.WOS_Guid = @WOSGuid AND U.User_ID = MA.Advisor_ID) 
		   END ) User_eMail,
		   MA.MainLanguage,
      MA.MA_SSprache,
      MA.MA_MSprache,
      MA.MA_Eigenschaft,
			MA.Advisor_ID,
			MA.AssignedCustomer_ID,
      MA.DesiredWagesOld,
      MA.DesiredWagesNew,
      MA.DesiredWagesInMonth,
      MA.DesiredWagesInHour
	FROM dbo.Kandidaten_Online MA
	WHERE MA.WOS_Guid =  @WOSGuid
	And MA.MANr = @employeeNumber  
	ORDER BY MA.Transfered_On DESC
	
END;
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Assigned Available Employee Application Data]
	@WOSGuid nvarchar(50),
	@Customer_ID nvarchar(50) = '',
	@EmployeeNumber INT


AS
BEGIN
	SET NOCOUNT ON

	SELECT ID,
           [EmployeeNr],
           [WOS_Guid],

           [ApplicationReserve0],
           [ApplicationReserve1],
           [ApplicationReserve2],
           [ApplicationReserve3],
           [ApplicationReserve4],
           [ApplicationReserve5],
           [ApplicationReserve6],
           [ApplicationReserve7],
           [ApplicationReserve8],
           [ApplicationReserve9],
           [ApplicationReserve10],
           [ApplicationReserve11],
           [ApplicationReserve12],
           [ApplicationReserve13],
           [ApplicationReserve14],
           [ApplicationReserve15],

           [ApplicationReserveRtf0],
           [ApplicationReserveRtf1],
           [ApplicationReserveRtf2],
           [ApplicationReserveRtf3],
           [ApplicationReserveRtf4],
           [ApplicationReserveRtf5],
           [ApplicationReserveRtf6],
           [ApplicationReserveRtf7],
           [ApplicationReserveRtf8],
           [ApplicationReserveRtf9],
           [ApplicationReserveRtf10],
           [ApplicationReserveRtf11],
           [ApplicationReserveRtf12],
           [ApplicationReserveRtf13],
           [ApplicationReserveRtf14],
           [ApplicationReserveRtf15],

           CreatedOn,
           CreatedFrom
           
	FROM dbo.[tbl_Available_Employee_ApplicationData] 
	WHERE WOS_Guid =  @WOSGuid
	And EmployeeNr = @employeeNumber  
	ORDER BY CreatedOn DESC
	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Delete Assigned Available Employee From WOS]
		@Customer_ID NVARCHAR(50),
		@WOSGuid NVARCHAR(50),
		@EmployeeNumber INT,
		@UserID NVARCHAR(50),
		@Transfered_Guid NVARCHAR(255) 

AS
BEGIN
	SET NOCOUNT ON
	
	Delete dbo.[tbl_Employee_Online_Template_Document] WHERE WOS_Guid = @WOSGuid
		And Customer_ID = @Customer_ID
		And EmployeeNr = @EmployeeNumber

	Delete dbo.Kandidaten_Online WHERE (Kandidaten_Online.WOS_Guid = @WOSGuid)
		And (Kandidaten_Online.Transfered_Guid = @Transfered_Guid)
		And (Kandidaten_Online.MANr = @EmployeeNumber)

	Delete dbo.[tbl_Available_Employee_ApplicationData] WHERE (WOS_Guid = @WOSGuid) And (EmployeeNr = @EmployeeNumber);
	IF NOT EXISTS
	(
			SELECT TOP (1)
						 MANr
			FROM dbo.Kandidaten_Doc_Online
			WHERE WOS_Guid = @WOSGuid
						AND MANr = @EmployeeNumber
						AND Owner_Guid = @Transfered_Guid
	)
	BEGIN
			DELETE dbo.kandidaten
			WHERE (WOS_Guid = @WOSGuid)
						AND (MANr = @EmployeeNumber);
	END

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Customer WOS Document Data]
	@WOSGuid nvarchar(255) = '',
	@USGuid nvarchar(255) = '',
	@Jahr int = 2016,
	@Monat INT = 0
AS
BEGIN
	SET NOCOUNT ON
	
SELECT  KD.ID ,
        KD.Customer_ID ,
        KD.ZHDNr ,
        KD.KDNr ,
        KD.RENr ,
        KD.RPNr ,
        KD.ESNr ,
        CONVERT(INT, KD.GetResult) GetResult ,
        KD.Get_On ,
        KD.KD_Name AS Firma ,
        KD.ZHD_Vorname ZFirstName ,
        KD.ZHD_Nachname ZLastName ,
        KD.Transfered_On ,
        KD.Transfered_User ,
        KD.LastNotification ,
        KD.Doc_Art ,
        KD.Doc_Info ,
        KD.KD_Guid ,
        KD.ZHD_Guid ,
        KD.Doc_Guid
FROM    [SpContract].dbo.Kunden_Doc_Online KD
WHERE   KD.WOS_Guid = @WOSGuid
        AND ( @USGuid = ''
              OR KD.LogedUser_ID = @USGuid
            )
        AND ( @Jahr = 0
              OR YEAR(KD.Transfered_On) = @Jahr
            )
        AND ( @Monat = 0
              OR MONTH(KD.Transfered_On) = @Monat
            )
ORDER BY Transfered_On;

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Get Assigned Customer WOS Document Data]
	@WOSGuid nvarchar(255),
	@RecID INT
AS
BEGIN
	SET NOCOUNT ON
	
SELECT  KD.ID ,
        KD.Customer_ID ,
        KD.ZHDNr ,
        KD.KDNr ,
        KD.RENr ,
        KD.RPNr ,
        KD.ESNr ,
        CONVERT(INT, KD.GetResult) GetResult ,
        KD.Get_On ,
        KD.KD_Name AS Firma ,
        KD.ZHD_Vorname ZFirstName ,
        KD.ZHD_Nachname ZLastName ,
        KD.Transfered_On ,
        KD.Transfered_User ,
        KD.LastNotification ,
        KD.Doc_Art ,
        KD.Doc_Info ,
        KD.KD_Guid ,
        KD.ZHD_Guid ,
        KD.Doc_Guid ,
        KD.DocScan ScanContent
FROM    [SpContract].dbo.Kunden_Doc_Online KD
WHERE   KD.Customer_ID = @WOSGuid
        AND KD.ID = @RecID;

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Assigned Customer Data From WOS]
		@Customer_ID NVARCHAR(50),
		@WOSGuid NVARCHAR(50),
		@CustomerNumber INT,
		@modulGuid NVARCHAR(50) 

AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KDDoc.ID,
           KDDoc.KDNr,
           KDDoc.ZHDNr,
           KDDoc.ESNr,
           KDDoc.ESLohnNr,
           KDDoc.RPNr,
           KDDoc.RENr,
           KDDoc.LogedUser_ID,
           KDDoc.Customer_ID,
           KDDoc.KD_Name,
           KDDoc.ZHD_Vorname,
           KDDoc.ZHD_Nachname,
           KDDoc.KD_Filiale,
           KDDoc.KD_Kanton,
           KDDoc.KD_Ort,
           KDDoc.KD_AGB_WOS,
           KDDoc.ZHDSex,
           KDDoc.ZHD_BriefAnrede,
           KDDoc.KD_eMail,
           KDDoc.ZHD_eMail,
           KDDoc.KD_Guid,
           KDDoc.ZHD_Guid,
           KDDoc.Doc_Guid,
           KDDoc.Doc_Art,
           KDDoc.Doc_Info,
           KDDoc.KD_Berater,
           KDDoc.ZHD_Berater,
           KDDoc.KD_Beruf,
           KDDoc.KD_Branche,
           KDDoc.ZHD_Beruf,
           KDDoc.ZHD_Branche,
           KDDoc.ZHD_AGB_WOS,
           KDDoc.ZHD_GebDat,
           KDDoc.Transfered_User,
           KDDoc.Transfered_On,
           CONVERT(INT, KDDoc.GetResult) GetResult,
           KDDoc.LastNotification,
           KDDoc.ProposeNr,
           KDDoc.FK_StateID,
           KDDoc.WOS_Guid ,
           DocState.Get_On,
           DocState.Viewed_On,
           DocState.ViewedResult,
           DocState.Customer_Feedback,
		   DocState.Customer_Feedback_On,
		   DocState.NotifyAdvisor
		   FROM dbo.Kunden_Doc_Online KDDoc
		   LEFT JOIN dbo.tbl_Customer_WOSDocument_State DocState 
		   ON KDDoc.WOS_Guid = DocState.WOS_ID AND KDDoc.KDNr = DocState.CustomerNr AND KDDoc.FK_StateID = DocState.ID 
		   WHERE (KDDoc.WOS_Guid = @WOSGuid)
		And (KDDoc.Doc_Guid = @modulGuid)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Create New Customer Document For WOS]
	@Customer_ID NVARCHAR(255) ,
	@WOSGuid NVARCHAR(50),
	@MANr INT,
	@KDNr INT,
	@ZHDNr INT,
	@ESNr INT,
	@ESLohnNr INT,
	@RPNr INT,
	@RENr INT,
	@ProposeNr INT,
	@LogedUser_Guid NVARCHAR(255),
	@KD_Name nvarchar(255),
	@ZHD_Vorname nvarchar(255),
	@ZHD_Nachname nvarchar(255),
	@KD_Filiale nvarchar(255),
	@KD_Postfach nvarchar(255),
	@KD_Strasse nvarchar(255),
	@KD_PLZ nvarchar(255),
	@KD_Kanton nvarchar(255),
	@KD_Ort nvarchar(255),
	@KD_AGB_Wos nvarchar(255),
	@ZHDSex nvarchar(255),
	@ZHD_Briefanrede nvarchar(255),
	@DoNotShowContractInWOS BIT ,
	@KD_EMail nvarchar(255),
	@KD_Guid nvarchar(255),
	@ZHD_Guid nvarchar(255),
	@Doc_Guid nvarchar(50),
	@Doc_Art nvarchar(50),
	@Doc_Info nvarchar(50),
	@Result nvarchar(255),
	@KD_Berater nvarchar(255),
	@ZHD_Berater NVARCHAR(255),
	@KD_Beruf nvarchar(255),
	@KD_Branche  NVARCHAR(4000),
	@ZHD_Beruf  NVARCHAR(255),
	@ZHD_Branche  NVARCHAR(4000),
	@ZHD_AGB_WOS  NVARCHAR(4000),
	@TransferedUser NVARCHAR(4000),
	@US_Nachname NVARCHAR(4000),
	@US_Vorname NVARCHAR(4000),
	@US_Telefon  NVARCHAR(4000),
	@US_Telefax  NVARCHAR(4000),
	@US_eMail NVARCHAR(4000),
	@KD_Language  NVARCHAR(4000),
	@ZHD_EMail  NVARCHAR(4000),
	@DocFilename  NVARCHAR(4000),
	@DocScan varbinary(MAX),

	@Customer_Name NVARCHAR(4000), 
	@User_Initial NVARCHAR(4000), 
	@User_Sex NVARCHAR(4000), 
	@User_Filiale  NVARCHAR(4000), 
	@User_Picture varbinary(MAX), 
	@User_Sign varbinary(MAX),
	@SignTransferedDocument BIT ,

	@NewId int OUTPUT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

DELETE dbo.Customer_Users WHERE WOS_Guid = @WOSGuid AND [User_ID] = @LogedUser_Guid;
DELETE dbo.Kunden WHERE WOS_Guid = @WOSGuid AND kdnr = @KDNr
DELETE dbo.Kunden_ZHD WHERE WOS_Guid = @WOSGuid AND kdnr = @KDNr AND ZHDNr = @ZHDNr
DELETE dbo.Kunden_Doc_Online WHERE WOS_Guid = @WOSGuid AND Doc_Guid = @Doc_Guid;
DELETE dbo.[tbl_Customer_WOSDocument_State] WHERE [WOS_ID] = @WOSGuid AND Doc_Guid = @Doc_Guid;

DECLARE @notificationDate DATETIME

IF @SignTransferedDocument = 1 
BEGIN
	SET @notificationDate = GETDATE()
END

IF ISNULL(@DoNotShowContractInWOS, 0) = 0 
BEGIN
SET @DoNotShowContractInWOS = 1
END
else
BEGIN
SET @DoNotShowContractInWOS = 0
END


Insert Into dbo.Customer_Users (
					[User_ID], 
					Customer_ID, 
					WOS_Guid, 
					Customer_Name, 
					User_Initial, 
					User_Sex, 
					User_Vorname, 
					User_Nachname, 
					User_Telefon, 
					User_Telefax, 
					User_eMail, 
					User_Filiale, 
					User_Picture, 
					User_Sign, 
					CreatedOn 
					) 
					VALUES 
					(
					@LogedUser_Guid, 
					@WOSGuid, 
					@WOSGuid, 
					@Customer_Name, 
					@User_Initial, 
					@User_Sex, 
					@US_Vorname, 
					@US_Nachname, 
					@US_Telefon, 
					@US_Telefax, 
					@US_eMail, 
					@User_Filiale, 
					@User_Picture, 
					@User_Sign, 
					GETDATE()
					) 

INSERT Into dbo.Kunden
        ( Customer_ID ,
          WOS_Guid ,
          KDNr ,
          KD_Guid ,
          KD_Name ,
          KD_Berater ,
          KD_Kanton ,
          KD_Ort ,
          KD_Beruf ,
          KD_Branche ,
          KD_eMail ,
          KD_Language ,
          KD_AGB_WOS ,
          Transfered_User ,
          Transfered_On ,
          EnableContractLink
        )
VALUES  ( @WOSGuid , -- Customer_ID - nvarchar(255)
          @WOSGuid ,
          @KDNr , -- KDNr - int
          @KD_Guid , -- KD_Guid - nvarchar(100)
          @KD_Name , -- KD_Name - nvarchar(255)
          @KD_Berater , -- KD_Berater - nvarchar(255)
          @KD_Kanton , -- KD_Kanton - nvarchar(50)
          @KD_Ort , -- KD_Ort - nvarchar(255)
          @KD_Beruf , -- KD_Beruf - nvarchar(1000)
          @KD_Branche , -- KD_Branche - nvarchar(1000)
          @KD_EMail , -- KD_eMail - nvarchar(70)
          @KD_Language , -- KD_Language - nvarchar(70)
          @KD_AGB_Wos , -- KD_AGB_WOS - nvarchar(70)
          @LogedUser_Guid , -- Transfered_User - nvarchar(100)
          GETDATE() ,  -- Transfered_On - datetime
		  @DoNotShowContractInWOS
        )

IF @ZHD_Guid <> ''
BEGIN
	INSERT INTO dbo.Kunden_ZHD
			( Customer_ID ,
	          WOS_Guid ,
			  KDNr ,
			  KD_Guid ,
			  ZHDNr ,
			  ZHD_Guid ,
			  ZHD_Vorname ,
			  ZHD_Nachname ,
			  Zhd_Beruf ,
			  Zhd_Branche ,
			  ZHDSex ,
			  Zhd_GebDat ,
			  Zhd_BriefAnrede ,
			  ZHD_eMail ,
			  Zhd_Berater ,
			  ZHD_AGB_WOS ,
			  Transfered_User ,
			  Transfered_On
			)
	VALUES  ( @WOSGuid , -- Customer_ID - nvarchar(255)
	          @WOSGuid ,
			  @KDNr , -- KDNr - int
			  @KD_Guid , -- KD_Guid - nvarchar(100)
			  @ZHDNr , -- ZHDNr - int
			  @ZHD_Guid , -- ZHD_Guid - nvarchar(100)
			  @ZHD_Vorname , -- ZHD_Vorname - nvarchar(255)
			  @ZHD_Nachname , -- ZHD_Nachname - nvarchar(255)
			  @ZHD_Beruf , -- Zhd_Beruf - nvarchar(1000)
			  @ZHD_Branche , -- Zhd_Branche - nvarchar(1000)
			  @ZHDSex , -- ZHDSex - nvarchar(10)
			  GETDATE() , -- Zhd_GebDat - datetime
			  @ZHD_Briefanrede , -- Zhd_BriefAnrede - nvarchar(70)
			  @ZHD_EMail , -- ZHD_eMail - nvarchar(70)
			  @ZHD_Berater , -- Zhd_Berater - nvarchar(255)
			  @ZHD_AGB_WOS , -- ZHD_AGB_WOS - nvarchar(70)
			  @TransferedUser , -- Transfered_User - nvarchar(100)
			  GETDATE()  -- Transfered_On - datetime
			)
END


		SET @NewId = 0

INSERT INTO dbo.[tbl_Customer_WOSDocument_State]
(
    [Customer_ID],
    [WOS_ID],
    [EmployeeNr],
    [CustomerNr],
    [ZHDNr],
    [ProposeNr],
    [ESNr],
    [ESLohnNr],
    [ReportNr],
    [InvoiceNr],
    [KD_Guid],
    [ZHD_Guid],
    [Doc_Guid],
    [Doc_Art],
    [Doc_Info],
    [Employee_Advisor],
    [Customer_Advisor],
    [ZHD_Advisor],
    [Transfered_User],
    [Transfered_On],
    [GetResult],
    [Get_On],
    [ViewedResult],
    [Viewed_On],
    [LastNotification],
    [Customer_Feedback]
)
VALUES
(   @WOSGuid ,       -- Customer_ID - nvarchar(255)
    @WOSGuid,       -- WOS_ID - nvarchar(255)
    @MANr,         -- EmployeeNr - int
    @KDNr,         -- CustomerNr - int
    @ZHDNr,         -- ZHDNr - int
    @ProposeNr,         -- ProposeNr - int
    @ESNr,         -- ESNr - int
    @ESLohnNr,         -- ESLohnNr - int
    @RPNr,         -- ReportNr - int
    @RENr,         -- InvoiceNr - int
    @KD_Guid,       -- KD_Guid - nvarchar(100)
    @ZHD_Guid,       -- ZHD_Guid - nvarchar(100)
    @Doc_Guid,       -- Doc_Guid - nvarchar(100)
    @Doc_Art,       -- Doc_Art - nvarchar(255)
    @Doc_Info,       -- Doc_Info - nvarchar(255)
    NULL,       -- Employee_Advisor - nvarchar(255)
    @KD_Berater,       -- Customer_Advisor - nvarchar(255)
    @ZHD_Berater,       -- ZHD_Advisor - nvarchar(255)
    @TransferedUser,       -- Transfered_User - nvarchar(100)
    GETDATE(), -- Transfered_On - datetime
    NULL,         -- GetResult - tinyint
    NULL, -- Get_On - datetime
    NULL,         -- ViewedResult - tinyint
    NULL, -- Viewed_On - datetime
    NULL, -- LastNotification - datetime
    NULL        -- Customer_Feedback - nvarchar(max)
    );

		SET @NewId = SCOPE_IDENTITY() 


Insert Into Kunden_Doc_Online (
	FK_StateID ,
	Customer_ID ,
	WOS_Guid ,
	KDNr ,
	ZHDNr ,
	ESNr ,
	ESLohnNr ,
	RPNr ,
	RENr ,
	ProposeNr ,
	LogedUser_ID ,
	KD_Name ,
	ZHD_Vorname ,
	ZHD_Nachname ,
	KD_Filiale ,
	KD_Kanton ,
	KD_Ort ,
	KD_AGB_Wos ,
	ZHDSex ,
	ZHD_Briefanrede ,
	KD_EMail ,
	KD_Guid ,
	ZHD_Guid ,
	Doc_Guid ,
	Doc_Art ,
	Doc_Info ,
	Result ,
	KD_Berater ,
	ZHD_Berater ,
	KD_Beruf ,
	KD_Branche  ,
	ZHD_Beruf  ,
	ZHD_Branche  ,
	ZHD_AGB_WOS  ,
	Transfered_User ,
	Transfered_On  ,
	User_Nachname,
	User_Vorname ,
	User_Telefon ,
	User_Telefax ,
	User_eMail ,
	KD_Language  ,
	DocFilename  ,
	LastNotification ,
	DocScan
	)
		Values
	(
	@NewId ,
	@Customer_ID ,
	@WOSGuid ,
	@KDNr ,
	@ZHDNr ,
	@ESNr ,
	@ESLohnNr ,
	@RPNr ,
	@RENr ,
	@ProposeNr ,
	@LogedUser_Guid ,
	@KD_Name ,
	@ZHD_Vorname ,
	@ZHD_Nachname ,
	@KD_Filiale ,
	@KD_Kanton ,
	@KD_Ort ,
	@KD_AGB_Wos ,
	@ZHDSex ,
	@ZHD_Briefanrede ,
	@KD_EMail ,
	@KD_Guid ,
	@ZHD_Guid ,
	@Doc_Guid ,
	@Doc_Art ,
	@Doc_Info ,
	@Result ,
	@KD_Berater ,
	@ZHD_Berater ,
	@KD_Beruf ,
	@KD_Branche  ,
	@ZHD_Beruf  ,
	@ZHD_Branche  ,
	@ZHD_AGB_WOS  ,
	@TransferedUser ,
	GETDATE()  ,
	@US_Nachname ,
	@US_Vorname ,
	@US_Telefon ,
	@US_Telefax ,
	@US_eMail ,
	@KD_Language  ,
	@DocFilename  ,
	@notificationDate ,
	@DocScan 
	);

			
		IF @StartTranCount = 0 COMMIT TRAN
		
	END TRY
	BEGIN CATCH
		IF @StartTranCount = 0 AND @@trancount > 0
		BEGIN
			ROLLBACK TRAN
			DECLARE @message NVARCHAR(MAX)
			DECLARE @state INT
			SELECT @message = ERROR_MESSAGE(), @state = ERROR_STATE()
			RAISERROR (@message, 11, @state)
		END

END CATCH

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Assigned Vacancy Data By Vacancynumber]
    @CustomerID NVARCHAR(50) = '' ,
    @WOS_Guid NVARCHAR(50) = '' ,
    @vakNumber INT 
AS 
    BEGIN

        SET NOCOUNT ON

        DECLARE @JobCategorie NVARCHAR(1000)
        DECLARE @JobDiscipline NVARCHAR(1000)
        DECLARE @JobPosition NVARCHAR(1000)

-- Beruf-Gruppe
        SELECT  @JobCategorie = ISNULL(@JobCategorie + '#', '')
                + dbo.tblVacancyJobExperience.Berufgruppe
        FROM    dbo.tblVacancyJobExperience
        WHERE   WOS_Guid = @WOS_Guid
                AND VakNr = @vakNumber
        SET @JobCategorie = ISNULL(@JobCategorie, '')

-- Beruf-Erfahrung
        SELECT  @JobDiscipline = ISNULL(@JobDiscipline + '#', '')
                + dbo.tblVacancyJobExperience.BerufErfahrung
        FROM    dbo.tblVacancyJobExperience
        WHERE   WOS_Guid = @WOS_Guid
                AND VakNr = @vakNumber
        SET @JobDiscipline = ISNULL(@JobDiscipline, '')

-- Beruf-Position
        SELECT  @JobPosition = ISNULL(@JobPosition + '#', '')
                + dbo.tblVacancyJobExperience.BerufPosition
        FROM    dbo.tblVacancyJobExperience
        WHERE   WOS_Guid = @WOS_Guid
                AND VakNr = @vakNumber
        SET @JobPosition = ISNULL(@JobPosition, '')

-- Ganzen Datensatz zurückgeben
        SELECT  
	v.[ID] ,
	v.[VakNr] ,
	v.[KDNr] ,
	v.[KDZHDNr] ,
	v.[Customer_ID] ,
	v.[Customer_Name] ,
	v.[Customer_Strasse] ,
	v.[Customer_Ort] ,
	v.[Customer_Telefon] ,
	v.[Customer_eMail] ,
	v.[Berater] ,
	v.[Filiale] ,
	v.[VakKontakt] ,
	v.[VakState] ,
	v.[Bezeichnung] ,
	v.[Slogan] ,
	v.[Gruppe] ,
	v.[SubGroup] ,
	v.[ExistLink] ,
	v.[JobChannelPriority] ,
	v.[VakLink] ,
	v.[Beginn] ,
	v.[JobProzent] ,
	v.[Anstellung] ,
	v.[Dauer] ,
	v.[MAAge] ,
	v.[MASex] ,
	v.[MAZivil] ,
	v.[MALohn] ,
	v.[Jobtime] ,
	v.[JobOrt] ,
	v.[MAFSchein] ,
	v.[MAAuto] ,
	v.[MANationality] ,
	v.[IEExport] ,
	v.[KDBeschreibung] ,
	v.[KDBietet] ,
	v.[SBeschreibung] ,
	v.[Reserve1] ,
	v.[Taetigkeit] ,
	v.[Anforderung] ,
	v.[Reserve2] ,
	v.[Reserve3] ,
	v.[Ausbildung] ,
	v.[Weiterbildung] ,
	v.[SKennt] ,
	v.[EDVKennt] ,
	v.[Branchen] ,
	v.[CreatedOn], 
	v.[CreatedFrom] ,
	v.[ChangedOn] ,
	v.[ChangedFrom] ,
	v.[Transfered_User] ,
	v.[Transfered_On] ,
	v.[Transfered_Guid] ,
	v.[Result] ,
	v.[Vak_Region] ,
	v.[Vak_Kanton] ,
	v.[MSprachen] ,
	v.[SSprachen] ,
	v.[Qualifikation] ,
	v.[SQualifikation] ,
	v.[User_Guid] ,
	v.[_KDBeschreibung] ,
	v.[_Taetigkeit] ,
	v.[_Anforderung] ,
	v.[_KDBietet] ,
	v.[JobPLZ] ,
	v.[_Reserve1] ,
	v.[_Reserve2] ,
	v.[_Reserve3] ,
	v.[_Weiterbildung] ,
	v.[_SBeschreibung] ,
	v.[_SKennt] ,
	v.[_EDVKennt] ,
	v.[_Ausbildung] ,
	v.[TitelForSearch] ,
	v.[ShortDescription] ,

        @JobCategorie AS Job_Categories ,
        @JobDiscipline AS Job_Disciplines ,
        @JobPosition AS Job_Position, 

	ISNULL((SELECT TOP 1 User_Sex FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterSex,
	ISNULL((SELECT TOP 1 User_Vorname FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterVorname,
	ISNULL((SELECT TOP 1 User_Nachname FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterNachname,
	ISNULL((SELECT TOP 1 User_EMail FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterEMail,
	ISNULL((SELECT TOP 1 User_Telefon FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid), '') AS BeraterTelefon

	, (SELECT TOP 1 User_Picture FROM Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid) AS BeraterPicture

        FROM    KD_Vakanzen V
        WHERE   ( V.WOS_Guid = @WOS_Guid )
                AND ( VakNr = @vakNumber ) 
    END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Load All Assigned Vacancy Data]
    @CustomerID NVARCHAR(50) = '' ,
    @WOS_Guid NVARCHAR(50) = '' ,
	@Beruf nvarchar(255) = '',
	@Ort nvarchar(255) = '',
	@Kanton nvarchar(255) = '', 
	@Region nvarchar(255) = '', 
	@Filiale nvarchar(255) = '',
	@Gruppe nvarchar(255) = '',
	@Anstellung nvarchar(255) = '',
	@Branche nvarchar(255) = '',
	@JobCategorie nvarchar(255) = '',
	@JobDisipline nvarchar(255) = '',
	@JobPosition nvarchar(255) = ''
AS
BEGIN

IF @Beruf 		 Is Null SET @Beruf = '' 	  
IF @Ort 		 Is Null SET @Ort = '' 	  
IF @Kanton 		 Is Null SET @Kanton = '' 	  
IF @Region 		 Is Null SET @Region = '' 	  
IF @Filiale 	 Is Null SET @Filiale = '' 	  
IF @Gruppe 		 Is Null SET @Gruppe = '' 	  
IF @Anstellung 	 Is Null SET @Anstellung = '' 	  
IF @Branche 	 Is Null SET @Branche = '' 	  
IF @JobCategorie Is Null SET @JobCategorie = '' 	  
IF @JobDisipline Is Null SET @JobDisipline = '' 	  
IF @JobPosition  Is Null SET @JobPosition = '' 	  

SET NOCOUNT ON
DECLARE @internCustomerID NVARCHAR(50) = ISNULL( (SELECT TOP (1) Customer_ID FROM dbo.KD_Vakanzen WHERE WOS_Guid = @WOS_Guid ORDER BY VakNr Desc), '')
IF @internCustomerID = ''
BEGIN
	SET @internCustomerID = @WOS_Guid
END
SET @CustomerID = @internCustomerID

SELECT Dbo.KD_Vakanzen.* 
, dbo.[Get JobDisciplines for each VacNumber](kd_vakanzen.vaknr, @CustomerID) AS vje_Job_Disciplines
, dbo.[Get JobDisciplinesMatch for each VacNumber](kd_vakanzen.vaknr, @CustomerID) AS Job_Disciplines_Match
, dbo.[Get JobRegionMatch for each VacNumber](kd_vakanzen.vaknr,KD_Vakanzen.Vak_Region, @CustomerID) AS Vak_Region_Match

FROM Dbo.KD_Vakanzen
WHERE 
  (KD_Vakanzen.WOS_Guid = @WOS_Guid) 
  And (KD_Vakanzen.Bezeichnung Like '%' + @Beruf + '%'      Or @Beruf = '') 
  And (KD_Vakanzen.JobOrt      Like '%' + @Ort + '%'        Or @Ort = '')   
  And (KD_Vakanzen.Vak_Region  Like '%' + @Region + '%'     Or @Region = '') 
  And (KD_Vakanzen.Anstellung  Like '%' + @Anstellung + '%' Or @Anstellung = '')
  And (KD_Vakanzen.Branchen    Like '%' + @Branche + '%'    Or @Branche = '') 
  -- 
  And (KD_Vakanzen.Vak_Kanton  = @Kanton  Or @Kanton = '') 
  And (KD_Vakanzen.Filiale     = @Filiale Or @Filiale = '') 
  And (KD_Vakanzen.Gruppe      = @Gruppe  Or @Gruppe = '')
  And ( 
	(
		    (@JobCategorie = '')
		And (@JobDisipline = '')
		And (@JobPosition  = '')
	) 
	Or (
		KD_Vakanzen.VakNr in (
			SELECT vakExp.VakNr FROM dbo.tblVacancyJobExperience vakExp
			WHERE ( vakExp.Customer_Guid = @CustomerID)
			And (@JobCategorie = '' Or @JobCategorie = vakExp.Berufgruppe)
			And (@JobDisipline = '' Or @JobDisipline = vakExp.BerufErfahrung)
			And (@JobPosition  = '' Or @JobPosition  = vakExp.BerufPosition)
		)
	)
  )
  ORDER BY KD_Vakanzen.Transfered_On DESC
END

GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Assigned Customer Data From WOS By DocArt]
		@Customer_ID NVARCHAR(50),
		@WOSGuid NVARCHAR(50),
		@CustomerNumber INT,
		@modulGuid NVARCHAR(50) ,
		@modulArtName NVARCHAR(50)

AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KDDoc.ID,
           KDDoc.KDNr,
           KDDoc.ZHDNr,
           KDDoc.ESNr,
           KDDoc.ESLohnNr,
           KDDoc.RPNr,
           KDDoc.RENr,
           KDDoc.LogedUser_ID,
           KDDoc.Customer_ID,
           KDDoc.KD_Name,
           KDDoc.ZHD_Vorname,
           KDDoc.ZHD_Nachname,
           KDDoc.KD_Filiale,
           KDDoc.KD_Kanton,
           KDDoc.KD_Ort,
           KDDoc.KD_AGB_WOS,
           KDDoc.ZHDSex,
           KDDoc.ZHD_BriefAnrede,
           KDDoc.KD_eMail,
           KDDoc.ZHD_eMail,
           KDDoc.KD_Guid,
           KDDoc.ZHD_Guid,
           KDDoc.Doc_Guid,
           KDDoc.Doc_Art,
           KDDoc.Doc_Info,
           KDDoc.KD_Berater,
           KDDoc.ZHD_Berater,
           KDDoc.KD_Beruf,
           KDDoc.KD_Branche,
           KDDoc.ZHD_Beruf,
           KDDoc.ZHD_Branche,
           KDDoc.ZHD_AGB_WOS,
           KDDoc.ZHD_GebDat,
           KDDoc.Transfered_User,
           KDDoc.Transfered_On,
           CONVERT(INT, KDDoc.GetResult) GetResult,
           KDDoc.LastNotification,
           KDDoc.ProposeNr,
           KDDoc.FK_StateID,
           KDDoc.WOS_Guid ,
           DocState.Get_On,
           DocState.Viewed_On,
           DocState.ViewedResult,
           DocState.Customer_Feedback,
		   DocState.Customer_Feedback_On,
		   DocState.NotifyAdvisor
		   FROM dbo.Kunden_Doc_Online KDDoc
		   LEFT JOIN dbo.tbl_Customer_WOSDocument_State DocState 
		   ON KDDoc.WOS_Guid = DocState.WOS_ID AND KDDoc.KDNr = DocState.CustomerNr AND KDDoc.FK_StateID = DocState.ID 
		   WHERE (KDDoc.WOS_Guid = @WOSGuid)
		And (ISNULL(@CustomerNumber, 0) = 0 OR KDDoc.KDNr = @CustomerNumber)
		And (ISNULL(@modulGuid, '') = '' OR KDDoc.Doc_Guid = @modulGuid)
		And (ISNULL(@modulArtName, '') = '' OR KDDoc.Doc_Art = @modulArtName)


END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Employee Data For send Mail Notifications]
	@Guid nvarchar(255) = '',
	@USGuid nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON

Select MA.Customer_ID, MA.WOS_Guid, MA.MA_Nachname, MA.MA_Vorname, MA.MA_EMail As Reciever, MA.MA_Language As Language, MA.BriefAnrede MA_BriefAnrede, 
				'' ZHD_Nachname, '' Zhd_BriefAnrede, '' ZHDSex,
				IsNull((US.User_Vorname + ' ' + US.User_Nachname), '') As Berater, 
				IsNull(SetDb.KD_Guid, '') As Customer_ID, 
				IsNull(US.Customer_Name, SetDb.Customer_Name) As Customer_Name, 
				IsNull(SetDb.Customer_Ort, SetDb.Customer_Ort) As Customer_Ort, 
				ISNULL(SetDb.Customer_Strasse, IsNull(SetDb.Customer_Strasse, '')) As Customer_Strasse, 
				(CASE
					WHEN ISNULL(US.User_Telefon, '') = '' THEN SetDb.Customer_Telefon
					ELSE 
                    US.User_Telefon                
				END) User_Telefon,
				(CASE
					WHEN ISNULL(US.USer_Telefax, '') = '' THEN SetDb.Customer_Telefax
					ELSE 
                    US.USer_Telefax                
				END) USer_Telefax,
				(CASE
					WHEN ISNULL(US.User_eMail, '') = '' THEN SetDb.Customer_eMail
					ELSE 
                    US.User_eMail                
				END) User_eMail,
				(CASE
					WHEN ISNULL(US.User_Homepage, '') = '' THEN SetDb.Customer_Homepage
					ELSE 
                    US.User_Homepage                
				END) User_Homepage

				From dbo.Kandidaten MA 
				left Join dbo.MySetting SetDb On MA.WOS_Guid = SetDb.WOS_Guid
				left Join dbo.Customer_Users US On MA.WOS_Guid = US.WOS_Guid
				Where Ma.MA_eMail <> '' And MA.MA_Guid = @Guid  And US.User_ID = @USGuid	
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Customer Data For send Mail Notifications]
	@Guid nvarchar(255) = '',
	@USGuid nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON

Select KD.WOS_Guid, KD.KD_Name, KD.KD_eMail As Reciever, KD.KD_Language As Language, 
				'' ZHD_Nachname, '' Zhd_BriefAnrede, '' ZHDSex,
				'' MA_Nachname, '' MA_BriefAnrede, 
				ISNULL((US.User_Vorname + ' ' + US.User_Nachname), '') As Berater, 
				IsNull(US.Customer_Name, SetDb.Customer_Name) As Customer_Name, 
				IsNull(SetDb.Customer_Ort, SetDb.Customer_Ort) As Customer_Ort, 
				ISNULL(SetDb.Customer_Strasse, IsNull(SetDb.Customer_Strasse, '')) As Customer_Strasse, 
				(CASE
					WHEN ISNULL(US.User_Telefon, '') = '' THEN SetDb.Customer_Telefon
					ELSE 
                    US.User_Telefon                
				END) User_Telefon,
				(CASE
					WHEN ISNULL(US.USer_Telefax, '') = '' THEN SetDb.Customer_Telefax
					ELSE 
                    US.USer_Telefax                
				END) USer_Telefax,
				(CASE
					WHEN ISNULL(US.User_eMail, '') = '' THEN SetDb.Customer_eMail
					ELSE 
                    US.User_eMail                
				END) User_eMail,
				(CASE
					WHEN ISNULL(US.User_Homepage, '') = '' THEN SetDb.Customer_Homepage
					ELSE 
                    US.User_Homepage                
				END) User_Homepage
				From Dbo.kunden KD 
				left Join dbo.MySetting SetDb On KD.WOS_Guid = SetDb.WOS_Guid
				left Join dbo.Customer_Users US On KD.WOS_Guid = US.WOS_Guid
				Where KD.KD_eMail <> '' And KD.KD_Guid = @Guid And US.User_ID = @USGuid
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Customer Responsible Data For send Mail Notifications]
	@Guid nvarchar(255) = '',
	@USGuid nvarchar(255) = ''

AS
BEGIN
	SET NOCOUNT ON

Select KDZ.ZHD_Nachname, KDZ.ZHD_Vorname, KDZ.ZHD_eMail As Reciever, KD.WOS_Guid, KD.KD_Language As Language, 
				kdz.Zhd_BriefAnrede, kdz.ZHDSex,
				'' MA_Nachname, '' MA_BriefAnrede, 
				IsNull((US.User_Vorname + ' ' + US.User_Nachname), '') As Berater, 
				--IsNull(SetDb.KD_Guid, '') As Customer_ID, 
				IsNull(US.Customer_Name, SetDb.Customer_Name) As Customer_Name, 
				IsNull(SetDb.Customer_Ort, SetDb.Customer_Ort) As Customer_Ort, 
				IsNull(SetDb.Customer_Strasse, IsNull(SetDb.Customer_Strasse, '')) As Customer_Strasse, 
				(CASE
					WHEN ISNULL(US.User_Telefon, '') = '' THEN SetDb.Customer_Telefon
					ELSE 
                    US.User_Telefon                
				END) User_Telefon,
				(CASE
					WHEN ISNULL(US.USer_Telefax, '') = '' THEN SetDb.Customer_Telefax
					ELSE 
                    US.USer_Telefax                
				END) USer_Telefax,
				(CASE
					WHEN ISNULL(US.User_eMail, '') = '' THEN SetDb.Customer_eMail
					ELSE 
                    US.User_eMail                
				END) User_eMail,
				(CASE
					WHEN ISNULL(US.User_Homepage, '') = '' THEN SetDb.Customer_Homepage
					ELSE 
                    US.User_Homepage                
				END) User_Homepage
				From Dbo.Kunden_ZHD KDZ 
				Left Join dbo.MySetting SetDb On KDZ.WOS_Guid = SetDb.WOS_Guid 
				left Join dbo.Kunden KD On KDz.WOS_Guid = KD.WOS_Guid And KDz.KD_Guid = KD.KD_Guid 
				left Join dbo.Customer_Users US On KDZ.WOS_Guid = US.WOS_Guid AND US.[User_ID] = @USGuid
				Where KDZ.ZHD_eMail <> '' And KDZ.ZHD_Guid = @Guid


END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load All Vacancies With EndDate]
AS 
    BEGIN
        SET NOCOUNT ON

        SELECT * FROM tblJobCHPlattform
        WHERE   ( EndDate < CONVERT(DATETIME, CONVERT(NVARCHAR(10), GETDATE(), 104), 104) ) 

        SELECT * FROM dbo.tblVacancyJobExperience
        WHERE   Vaknr NOT IN (
                SELECT  Vaknr
                FROM    dbo.KD_Vakanzen
                WHERE   dbo.tblVacancyJobExperience.vaknr = dbo.KD_Vakanzen.Vaknr
                        AND dbo.tblVacancyJobExperience.WOS_Guid = dbo.KD_Vakanzen.WOS_Guid )

        SELECT * FROM tblOstJobCHPlattform
        WHERE   ( EndDate < CONVERT(DATETIME, CONVERT(NVARCHAR(10), GETDATE(), 104), 104) )

    END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Create New Vacancy For Internal Jobplattform]
	@Customer_ID NVARCHAR(50) ,
	@WOSGuid NVARCHAR(50),
	@User_Guid NVARCHAR(255),

	@Customer_Name nvarchar(255),
	@Customer_strasse nvarchar(255),
	@Customer_plz nvarchar(255),
	@Customer_ort nvarchar(255),
	@Customer_land nvarchar(255),
	@Customer_telefon nvarchar(255),
	@Customer_telefax nvarchar(255),
	@Customer_email nvarchar(255),
	@Customer_homepage nvarchar(255),

	@User_Vorname nvarchar(255),
	@User_Nachname nvarchar(255),
	@User_telefon nvarchar(255),
	@User_telefax nvarchar(255),
	@User_email nvarchar(255),
	@User_homepage nvarchar(255),
	@User_Anrede nvarchar(50),
	@User_Initial nvarchar(50),
	@User_Filiale nvarchar(50),

	@Loged_UserName nvarchar(255),
	@Loged_UserGuid nvarchar(255),


	@VakNr INT,
	@KDNr INT,
	@KDZHDNr INT,

	@Berater NVARCHAR(255),
	@Filiale nvarchar(255),
	@vakKontakt  NVARCHAR(4000),
	@UserKontakt  NVARCHAR(255),
	@VakState  NVARCHAR(4000),
	@Bezeichnung  NVARCHAR(4000),

	@ShortDescription NVARCHAR(4000),
	@TitelForSearch NVARCHAR(4000),

	@slogan  NVARCHAR(4000),
	@Gruppe NVARCHAR(4000),

	@Vaklink NVARCHAR(4000),
	@Beginn  NVARCHAR(4000),
	@jobprozent  NVARCHAR(4000),
	@anstellung NVARCHAR(4000),

	@Dauer NVARCHAR(4000),
	@MAAge  NVARCHAR(4000),
	@MASex  NVARCHAR(4000),
	@MAZivil  NVARCHAR(4000),

	@MALohn  NVARCHAR(4000),
	@JobTime  NVARCHAR(4000),
	@JobOrt NVARCHAR(4000),
	@JobPLZ  NVARCHAR(4000),

	@Ausbildung NVARCHAR(4000),
	@_Ausbildung NVARCHAR(4000),
	@Reserve1 NVARCHAR(4000),
	@_Reserve1 NVARCHAR(4000),
	@Reserve2 NVARCHAR(4000),
	@_Reserve2 NVARCHAR(4000),
	@Reserve3 NVARCHAR(4000),
	@_Reserve3 NVARCHAR(4000),
	@Weiterbildung NVARCHAR(4000),
	@_Weiterbildung NVARCHAR(4000),
	@SBeschreibung NVARCHAR(4000),
	@_SBeschreibung NVARCHAR(4000),
	@SKennt NVARCHAR(4000),
	@_SKennt NVARCHAR(4000),
	@EDVKennt NVARCHAR(4000),
	@_EDVKennt NVARCHAR(4000),
	
	@Jobs_Vorspann NVARCHAR(4000),
	@_Jobs_Vorspann NVARCHAR(4000),

	@Jobs_Aufgabe NVARCHAR(4000),
	@_Jobs_Aufgabe NVARCHAR(4000),

	@Jobs_Anforderung NVARCHAR(4000),
	@_Jobs_Anforderung NVARCHAR(4000),

	@Jobs_WirBieten NVARCHAR(4000),
	@_Jobs_WirBieten NVARCHAR(4000),

	@Branchen NVARCHAR(255),
	@MSprache NVARCHAR(255),
	@Region NVARCHAR(255),
	@Vak_Kanton NVARCHAR(2),
	@Qualifikation NVARCHAR(255),

	@User_Picture image,
	@User_Sign image,

	@SetOnline bit,
	@SBNnumber INT = 0 ,
	@vakSubGroup NVARCHAR(255) = '',
	@JobChannelPriority bit

AS

BEGIN
SET NOCOUNT ON
DELETE dbo.KD_Vakanzen WHERE WOS_Guid = @WOSGuid AND VakNr = @VakNr;
Delete dbo.tblVacancyJobExperience Where WOS_Guid = @WOSGuid And VakNr = @VakNr;

IF NOT EXISTS(SELECT TOP (1) ID FROM dbo.Customer_Users WHERE [USER_ID] = @User_Guid AND WOS_Guid = @WOSGuid)
BEGIN

Insert Into dbo.Customer_Users (
		Customer_ID  ,
		WOS_Guid  ,
		Customer_Name  ,
		[User_ID] ,
		User_Sex,
		User_Vorname,
		User_Nachname,
		User_Initial,
		User_Filiale,
		User_Telefon,
		User_Telefax,
		User_eMail,
		User_Homepage,
		User_Picture,
		User_Sign,
		CreatedOn
	)
		Values
	(
		@Customer_ID  ,
		@WOSGuid  ,
		@Customer_Name  ,
		@User_Guid ,
		@User_Anrede,
		@User_Vorname,
		@User_Nachname,
		@User_Initial,
		@User_Filiale,
		@User_Telefon,
		@User_Telefax,
		@User_eMail,
		@User_Homepage,
		@User_Picture,
		@User_Sign,
		getdate()
	)
END	

ELSE

BEGIN
UPDATE dbo.Customer_Users SET 
		User_Sex = @User_Anrede,
		User_Vorname = @User_Vorname,
		User_Nachname = @User_Nachname,
		User_Initial = @User_Initial,
		User_Filiale = @User_Filiale,
		User_Telefon = @User_Telefon,
		User_Telefax = @User_Telefax,
		User_eMail = @User_eMail,
		User_Homepage = @User_Homepage,
		User_Picture = @User_Picture,
		User_Sign = @User_Sign

		WHERE 
		Customer_ID = @Customer_ID
		AND WOS_Guid = @WOSGuid
		AND [User_ID] = @User_Guid
END
		

IF @SetOnline = 1
BEGIN
	INSERT INTO dbo.KD_Vakanzen (
		Customer_ID  ,
		WOS_Guid  ,
		User_Guid ,
		Customer_Name ,
		Customer_Strasse ,
		Customer_Ort ,
		Customer_Telefon ,
		Customer_eMail ,

		VakNr ,
		KDNr ,
		KDZHDNr ,

		Berater ,
		Filiale ,
		vakKontakt  ,
		VakState  ,
		Bezeichnung  ,

		ShortDescription  ,
		TitelForSearch  ,

		slogan  ,
		Gruppe ,

		Vaklink ,
		Beginn  ,
		jobprozent  ,
		anstellung ,

		Dauer ,
		MAAge  ,
		MASex  ,
		MAZivil  ,

		MALohn  ,
		JobTime  ,
		JobOrt ,
		JobPLZ ,

		Ausbildung ,
		_Ausbildung ,
		Reserve1 ,
		_Reserve1 ,
		Reserve2 ,
		_Reserve2 ,
		Reserve3 ,
		_Reserve3 ,
		Weiterbildung ,
		_Weiterbildung ,
		SBeschreibung ,
		_SBeschreibung ,
		SKennt ,
		_SKennt ,
		EDVKennt ,
		_EDVKennt ,

		KDBeschreibung ,
		_KDBeschreibung ,

		Taetigkeit ,
		_Taetigkeit ,

		Anforderung ,
		_Anforderung ,

		KDBietet ,
		_KDBietet ,

		Branchen ,
		Vak_Region ,
		MSprachen ,
		Vak_Kanton ,

		SBNNumber ,
		SubGroup ,

		Qualifikation ,
		Transfered_On ,
		CreatedOn ,
		JobChannelPriority 
		)

		VALUES 
		(
		@Customer_ID  ,
		@WOSGuid  ,
		@User_Guid ,
		@Customer_name ,
		@Customer_strasse ,
		@Customer_plz + ' ' + @Customer_Ort ,
		@Customer_Telefon ,
		@Customer_eMail ,

		@VakNr ,
		@KDNr ,
		@KDZHDNr ,

		@Berater ,
		@Filiale ,
		@UserKontakt  ,
		@VakState  ,
		@Bezeichnung  ,

		@ShortDescription  ,
		@TitelForSearch  ,

		@slogan  ,
		@Gruppe ,

		@Vaklink ,
		@Beginn  ,
		@jobprozent  ,
		@anstellung ,

		@Dauer ,
		@MAAge  ,
		@MASex  ,
		@MAZivil  ,

		@MALohn  ,
		@JobTime  ,
		@JobOrt ,
		@JobPLZ  ,

		@Ausbildung ,
		@_Ausbildung ,
		@Reserve1 ,
		@_Reserve1 ,
		@Reserve2 ,
		@_Reserve2 ,
		@Reserve3 ,
		@_Reserve3 ,
		@Weiterbildung ,
		@_Weiterbildung ,
		@SBeschreibung ,
		@_SBeschreibung ,
		@SKennt ,
		@_SKennt ,
		@EDVKennt ,
		@_EDVKennt ,

		@Jobs_Vorspann ,
		@_Jobs_Vorspann ,

		@Jobs_Aufgabe ,
		@_Jobs_Aufgabe ,

		@Jobs_Anforderung ,
		@_Jobs_Anforderung ,

		@Jobs_WirBieten ,
		@_Jobs_WirBieten ,

		@Branchen ,
		@Region ,
		@MSprache ,
		@Vak_Kanton ,

		@SBNNumber ,
		@vakSubGroup ,

		@Qualifikation ,
		GETDATE() ,
		GETDATE() ,
		@JobChannelPriority 
		)
END

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Vak-Slogan]
	@WOS_Guid nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT KD_Vakanzen.Slogan 
	FROM dbo.KD_Vakanzen
	WHERE (KD_Vakanzen.WOS_Guid = @WOS_Guid)
	And (KD_Vakanzen.Slogan <> '' Or KD_Vakanzen.Slogan Is Not Null)
	Group By KD_Vakanzen.Slogan
	ORDER BY KD_Vakanzen.Slogan

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Create Assigned Available Employee Template For WOS]
	@Customer_ID NVARCHAR(50) ,
	@WOSGuid NVARCHAR(50),
	@EmployeeNumber INT,
	@ScanDoc varbinary(MAX),
	@CreatedFrom NVARCHAR(255),

	@NewId int OUTPUT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN



Insert Into dbo.[tbl_Employee_Online_Template_Document]
(
    [Customer_ID],
    [WOS_Guid],
    [EmployeeNr],
    [ScanDoc],
    [CreatedFrom],
    [CreatedOn],
    [GetResult],
    [Get_On],
    [ViewedResult],
    [Viewed_On]
)
VALUES
(   @Customer_ID,       -- Customer_ID - nvarchar(255)
    @WOSGuid,       -- WOS_ID - nvarchar(255)
    @EmployeeNumber,         -- EmployeeNr - int
    @ScanDoc,         -- EmployeeNr - int
    @CreatedFrom,       -- CreatedFrom - nvarchar(255)
    GETDATE(), -- CreatedOn - datetime
    0,         -- GetResult - int
    GETDATE(), -- Get_On - datetime
    0,         -- ViewedResult - int
    GETDATE()  -- Viewed_On - datetime
    )


		SET @NewId = SCOPE_IDENTITY() 

			
		IF @StartTranCount = 0 COMMIT TRAN
		
	END TRY
	BEGIN CATCH
		IF @StartTranCount = 0 AND @@trancount > 0
		BEGIN
			ROLLBACK TRAN
			DECLARE @message NVARCHAR(MAX)
			DECLARE @state INT
			SELECT @message = ERROR_MESSAGE(), @state = ERROR_STATE()
			RAISERROR (@message, 11, @state)
		END

END CATCH

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Available Employee Document Data]
	@Customer_ID nvarchar(50),
	@WOSGuid nvarchar(50) ,
	@EmployeeNumber INT

AS
BEGIN

SET NOCOUNT ON
	
SELECT MT.ID,
       MT.Customer_ID,
       MT.WOS_Guid,
       MT.EmployeeNr,
       MT.ScanDoc,
       MT.CreatedFrom,
       MT.CreatedOn,
       MT.GetResult,
       MT.Get_On,
       MT.ViewedResult,
       MT.Viewed_On
FROM dbo.[tbl_Employee_Online_Template_Document] MT
WHERE 
	(@Customer_ID = '' OR MT.Customer_ID = @Customer_ID) 
	AND (MT.WOS_Guid = @WOSGuid) 
	AND (MT.EmployeeNr = @EmployeeNumber)
	
  ORDER BY MT.CreatedOn DESC, MT.ID Desc
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Set Propose Notification To Done]
		@Customer_ID NVARCHAR(50),
		@WOSGuid NVARCHAR(50),
		@recID INT

AS
BEGIN
	SET NOCOUNT ON



UPDATE dbo.tbl_Customer_WOSDocument_State SET NotifyAdvisor = NULL 
	WHERE WOS_ID = @WOSGuid AND 
	ID = ISNULL( (SELECT TOP 1 FK_StateID FROM dbo.Kunden_Doc_Online WHERE ID = @recID), 0)

END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Jobplattform Counter Data]
    @CustomerID NVARCHAR(50) ,
    @WOSGuid NVARCHAR(50) ,
		@JobsCHAccountNumber INT ,
		@OstJobAccountNumber NVARCHAR(10)
AS 
    BEGIN

        SET NOCOUNT ON

        DECLARE @OwnCounter INT
        DECLARE @JobsCHCounter INT
        DECLARE @OstJobCounter INT
        DECLARE @JobChannelPriorityCounter INT

-- Ganzen Datensatz zurückgeben
SELECT @OwnCounter = COUNT(*) FROM dbo.KD_Vakanzen WHERE Customer_ID = @CustomerID AND WOS_Guid = @WOSGuid
SELECT @JobsCHCounter = COUNT(*) FROM dbo.tblJobCHPlattform WHERE Customer_Guid = @CustomerID AND (ISNULL(@JobsCHAccountNumber, 0) = 0 OR OrganisationID = @JobsCHAccountNumber)
SELECT @OstJobCounter = COUNT(*) FROM dbo.tblOstJobCHPlattform WHERE Customer_Guid = @CustomerID AND (@OstJobAccountNumber = '' OR Company = @OstJobAccountNumber)
SELECT @JobChannelPriorityCounter = COUNT(*) FROM dbo.KD_Vakanzen WHERE Customer_ID = @CustomerID AND WOS_Guid = @WOSGuid AND JobChannelPriority = 1

SELECT @OwnCounter AS OwnCounter,
       @JobsCHCounter AS JobsCHCounter,
       @OstJobCounter AS OstJobCounter,
       @JobChannelPriorityCounter AS JobChannelPriorityCounter

    END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Load Assigned Vacancy Data By CustomerID]
    @CustomerID NVARCHAR(50),
    @vakNumber INT 
AS 
    BEGIN

        SET NOCOUNT ON

        DECLARE @JobCategorie NVARCHAR(1000)
        DECLARE @JobDiscipline NVARCHAR(1000)
        DECLARE @JobPosition NVARCHAR(1000)

-- Beruf-Gruppe
        SELECT  @JobCategorie = ISNULL(@JobCategorie + '#', '')
                + dbo.tblVacancyJobExperience.Berufgruppe
        FROM    dbo.tblVacancyJobExperience
        WHERE   Customer_Guid = @CustomerID
                AND VakNr = @vakNumber
        SET @JobCategorie = ISNULL(@JobCategorie, '')

-- Beruf-Erfahrung
        SELECT  @JobDiscipline = ISNULL(@JobDiscipline + '#', '')
                + dbo.tblVacancyJobExperience.BerufErfahrung
        FROM    dbo.tblVacancyJobExperience
        WHERE   Customer_Guid = @CustomerID
                AND VakNr = @vakNumber
        SET @JobDiscipline = ISNULL(@JobDiscipline, '')

-- Beruf-Position
        SELECT  @JobPosition = ISNULL(@JobPosition + '#', '')
                + dbo.tblVacancyJobExperience.BerufPosition
        FROM    dbo.tblVacancyJobExperience
        WHERE   Customer_Guid = @CustomerID
                AND VakNr = @vakNumber
        SET @JobPosition = ISNULL(@JobPosition, '')

-- Ganzen Datensatz zurückgeben
        SELECT  
	v.[ID] ,
	v.[VakNr] ,
	v.[KDNr] ,
	v.[KDZHDNr] ,
	v.[Customer_ID] ,
	v.[Customer_Name] ,
	v.[Customer_Strasse] ,
	v.[Customer_Ort] ,
	v.[Customer_Telefon] ,
	v.[Customer_eMail] ,
	v.[Berater] ,
	v.[Filiale] ,
	v.[VakKontakt] ,
	v.[VakState] ,
	v.[Bezeichnung] ,
	v.[Slogan] ,
	v.[Gruppe] ,
	v.[SubGroup] ,
	v.[ExistLink] ,
	v.[JobChannelPriority] ,
	v.[VakLink] ,
	v.[Beginn] ,
	v.[JobProzent] ,
	v.[Anstellung] ,
	v.[Dauer] ,
	v.[MAAge] ,
	v.[MASex] ,
	v.[MAZivil] ,
	v.[MALohn] ,
	v.[Jobtime] ,
	v.[JobOrt] ,
	v.[MAFSchein] ,
	v.[MAAuto] ,
	v.[MANationality] ,
	v.[IEExport] ,
	v.[KDBeschreibung] ,
	v.[KDBietet] ,
	v.[SBeschreibung] ,
	v.[Reserve1] ,
	v.[Taetigkeit] ,
	v.[Anforderung] ,
	v.[Reserve2] ,
	v.[Reserve3] ,
	v.[Ausbildung] ,
	v.[Weiterbildung] ,
	v.[SKennt] ,
	v.[EDVKennt] ,
	v.[Branchen] ,
	v.[CreatedOn], 
	v.[CreatedFrom] ,
	v.[ChangedOn] ,
	v.[ChangedFrom] ,
	v.[Transfered_User] ,
	v.[Transfered_On] ,
	v.[Transfered_Guid] ,
	v.[Result] ,
	v.[Vak_Region] ,
	v.[Vak_Kanton] ,
	v.[MSprachen] ,
	v.[SSprachen] ,
	v.[Qualifikation] ,
	v.[SQualifikation] ,
	v.[User_Guid] ,
	v.[_KDBeschreibung] ,
	v.[_Taetigkeit] ,
	v.[_Anforderung] ,
	v.[_KDBietet] ,
	v.[JobPLZ] ,
	v.[_Reserve1] ,
	v.[_Reserve2] ,
	v.[_Reserve3] ,
	v.[_Weiterbildung] ,
	v.[_SBeschreibung] ,
	v.[_SKennt] ,
	v.[_EDVKennt] ,
	v.[_Ausbildung] ,
	v.[TitelForSearch] ,
	v.[ShortDescription] ,

        @JobCategorie AS Job_Categories ,
        @JobDiscipline AS Job_Disciplines ,
        @JobPosition AS Job_Position, 

	ISNULL((SELECT TOP 1 User_Sex FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid ORDER BY cu.ID), '') AS BeraterSex,
	ISNULL((SELECT TOP 1 User_Vorname FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid ORDER BY cu.ID), '') AS BeraterVorname,
	ISNULL((SELECT TOP 1 User_Nachname FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid ORDER BY cu.ID), '') AS BeraterNachname,
	ISNULL((SELECT TOP 1 User_EMail FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid ORDER BY cu.ID), '') AS BeraterEMail,
	ISNULL((SELECT TOP 1 User_Telefon FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid ORDER BY cu.ID), '') AS BeraterTelefon

	, (SELECT TOP 1 User_Picture FROM dbo.Customer_Users cu WHERE cu.customer_ID = v.Customer_id AND cu.[User_ID] = v.User_Guid ORDER BY cu.ID) AS BeraterPicture

        FROM    dbo.KD_Vakanzen V
        WHERE   ( V.Customer_ID = @CustomerID )
                AND ( VakNr = @vakNumber ) 
    END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Customer WOS Invoice Notification Data]
	@WOSGuid NVARCHAR(255) = '',
	@modulNumber INT,
	@number INT 

AS
BEGIN
	SET NOCOUNT ON

DECLARE @KDGuid NVARCHAR(50) = ''
DECLARE @lastNotification DATETIME
DECLARE @KDEMail NVARCHAR(255) = ''

SELECT TOP (1) @KDGuid = kdDoc.KD_Guid, @lastNotification = kdDoc.LastNotification, @KDEMail = kdDoc.KD_eMail FROM dbo.Kunden_Doc_Online kdDoc 
	WHERE kdDoc.WOS_Guid = @WOSGuid 
	AND kdDoc.RENr = @number
	AND kdDoc.LastNotification IS NOT NULL	
	ORDER BY kdDoc.Transfered_On DESC 
	
SELECT  TOP (1) 
		0 ID ,
        N.Customer_ID ,
        N.MailFrom ,
        @KDEMail MailTo ,
        N.Result ,
        N.[SUBJECT] ,
        N.Body MailBody ,
        N.DocLink ,
        N.Recipient_Guid ,
        @lastNotification CreatedOn
FROM    [SpContract].dbo.MailNotification N
WHERE   N.Customer_ID = @WOSGuid
        AND N.Recipient_Guid = @KDGuid
		ORDER BY N.CreatedOn DESC;

END;
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[List Mail Notification Data]
	@customerID NVARCHAR(50),
	@assignedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON
	
SELECT  N.ID ,
        N.Customer_ID ,
        N.Customer_Name ,
        N.Customer_Ort,
        N.MailFrom ,
        N.MailTo ,
        N.Result ,
        N.[SUBJECT] ,
        N.Body MailBody ,
        N.DocLink ,
        N.Recipient_Guid ,
        N.CreatedOn
FROM    [SpContract].dbo.MailNotification N
				WHERE (@assignedDate IS NULL OR CONVERT(NVARCHAR(10), N.CreatedOn, 104) = CONVERT(NVARCHAR(10), @assignedDate, 104))
				AND (ISNULL(@CustomerID, '') = '' OR N.Customer_ID = @CustomerID)
ORDER BY N.CreatedOn DESC;

END;
GO

/* ------------ end of query -------------------------------- */

