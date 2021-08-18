
USE [master]
GO

CREATE DATABASE [spPVLPublicData]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'spPVLPublicData', FILENAME = N'<your path>\spPVLPublicData.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'spPVLPublicData_log', FILENAME = N'<your path>\spPVLPublicData_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO



USE [spPVLPublicData]
GO


CREATE TABLE [dbo].[GAV_SputnikWarning](
	[GAV_Number] [int] NULL,
	[Info] [nvarchar](4000) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_CategoryValue] [int] NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[GAV_Versions](
	[GAVNumber] [int] NULL,
	[GAVDate] [datetime] NULL,
	[GAVInfo] [nvarchar](max) NULL,
	[schema_version] [nvarchar](10) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[GAV_Versions_0](
	[ID] [int] NOT NULL,
	[GAVNumber] [int] NULL,
	[GAVDate] [datetime] NULL,
	[GAVInfo] [nvarchar](max) NULL,
	[schema_version] [nvarchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[Meta](
	[gav_number] [int] NULL,
	[version] [nvarchar](10) NULL,
	[ave] [bit] NULL,
	[calculations_changed] [bit] NULL,
	[anhang1] [bit] NULL,
	[publication_date] [date] NULL,
	[validity_start_date] [date] NULL,
	[expiry_date] [date] NULL,
	[state] [int] NULL,
	[unia_validity_start] [date] NULL,
	[unia_validity_end] [date] NULL,
	[ave_validity_start] [date] NULL,
	[ave_validity_end] [date] NULL,
	[name_de] [nvarchar](1000) NULL,
	[name_fr] [nvarchar](1000) NULL,
	[name_it] [nvarchar](1000) NULL,
	[short_name_de] [nvarchar](1000) NULL,
	[short_name_fr] [nvarchar](1000) NULL,
	[short_name_it] [nvarchar](1000) NULL,
	[ID_Meta] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[PVL_DownloadInfo](
	[GAV_Number] [int] NULL,
	[GAV_Name] [nvarchar](255) NULL,
	[IsAnhang1] [bit] NULL,
	[IsAVE] [bit] NULL,
	[AveValidityEnd] [datetime] NULL,
	[AveValidityStart] [datetime] NULL,
	[CalculationsChanged] [bit] NULL,
	[Comment] [nvarchar](1000) NULL,
	[Created] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[PublicationDate] [datetime] NULL,
	[PvlEdition] [nvarchar](255) NULL,
	[SourceEdition] [nvarchar](255) NULL,
	[ValidityStartDate] [datetime] NULL,
	[Version] [nvarchar](255) NULL,
	[XmlSchemaVersion] [nvarchar](255) NULL,
	[ZipFileId] [nvarchar](255) NULL,
	[ZipFileSize] [nvarchar](255) NULL,
	[ZipUrl] [nvarchar](255) NULL,
	[DownloadDate] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Tab_GAVStdParifond](
	[GAV_Number] [int] NULL,
	[Gruppe0] [nvarchar](255) NULL,
	[StdWeek] [money] NULL,
	[StdMonth] [money] NULL,
	[StdYear] [money] NULL,
	[FAG] [money] NOT NULL,
	[Fan] [money] NOT NULL,
	[VAN] [money] NOT NULL,
	[VAG] [money] NOT NULL,
	[WAN] [money] NOT NULL,
	[WAG] [money] NOT NULL,
	[_FAG] [money] NULL,
	[_FAN] [money] NULL,
	[_WAG] [money] NULL,
	[_WAN] [money] NULL,
	[_VAG] [money] NULL,
	[_VAN] [money] NULL,
	[_WAG_S] [money] NULL,
	[_WAN_S] [money] NULL,
	[_WAG_J] [money] NULL,
	[_WAN_J] [money] NULL,
	[_VAG_S] [money] NULL,
	[_VAN_S] [money] NULL,
	[_VAG_J] [money] NULL,
	[_VAN_J] [money] NULL,
	[GAVKanton] [nvarchar](70) NULL,
	[Resor_FAG] [money] NULL,
	[Resor_FAN] [money] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[GavListe FL](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GAVNr] [int] NULL,
	[Deletedrec] [bit] NULL,
	[GavKanton] [nvarchar](255) NULL,
	[Gruppe0] [nvarchar](255) NULL,
	[Gruppe1] [nvarchar](255) NULL,
	[Gruppe2] [nvarchar](255) NULL,
	[Gruppe3] [nvarchar](255) NULL,
	[GavText] [nvarchar](255) NULL,
	[CalcFerien] [int] NULL,
	[Calc13Lohn] [int] NULL,
	[Minlohn] [money] NULL,
	[FeiertagLohn] [money] NULL,
	[Feierbtr] [money] NULL,
	[FerienLohn] [money] NULL,
	[Ferienbtr] [money] NULL,
	[Lohn13] [money] NULL,
	[Lohn13btr] [money] NULL,
	[StdLohn] [money] NULL,
	[Monatslohn] [money] NULL,
	[Mittagszulagen] [money] NULL,
	[FAG] [money] NULL,
	[FAN] [money] NULL,
	[WAG] [money] NULL,
	[WAN] [money] NULL,
	[VAG] [money] NULL,
	[VAN] [money] NULL,
	[FAG_S] [money] NULL,
	[FAN_S] [money] NULL,
	[WAG_S] [money] NULL,
	[WAN_S] [money] NULL,
	[VAG_S] [money] NULL,
	[VAN_S] [money] NULL,
	[FAG_M] [money] NULL,
	[FAN_M] [money] NULL,
	[WAG_M] [money] NULL,
	[WAN_M] [money] NULL,
	[VAG_M] [money] NULL,
	[VAN_M] [money] NULL,
	[FAG_J] [money] NULL,
	[FAN_J] [money] NULL,
	[WAG_J] [money] NULL,
	[WAN_J] [money] NULL,
	[VAG_J] [money] NULL,
	[VAN_J] [money] NULL,
	[GueltigAb] [datetime] NULL,
	[GueltigBis] [datetime] NULL,
	[ZusatzFeier] [nvarchar](255) NULL,
	[Zusatz13Lohn] [nvarchar](255) NULL,
	[FAR-Pflicht] [bit] NULL,
	[Ferientext] [nvarchar](255) NULL,
	[Lohn13text] [nvarchar](255) NULL,
	[SollStd] [nvarchar](255) NULL,
	[StdWeek] [int] NULL,
	[StdMonth] [int] NULL,
	[StdYear] [int] NULL,
	[F_Alter] [nvarchar](255) NULL,
	[L_Alter] [nvarchar](255) NULL,
	[Zusatz LA] [nvarchar](255) NULL,
	[Zusatz1] [nvarchar](255) NULL,
	[Zusatz2] [nvarchar](255) NULL,
	[Zusatz3] [nvarchar](255) NULL,
	[Zusatz4] [nvarchar](255) NULL,
	[Zusatz5] [nvarchar](255) NULL,
	[Zusatz6] [nvarchar](255) NULL,
	[Zusatz7] [nvarchar](255) NULL,
	[Zusatz8] [nvarchar](255) NULL,
	[Zusatz9] [nvarchar](255) NULL,
	[Zusatz10] [nvarchar](255) NULL,
	[Zusatz11] [nvarchar](255) NULL,
	[Zusatz12] [nvarchar](255) NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[PVL Addresses](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RecNr] [int] NULL,
	[GAVNumber] [int] NULL,
	[BerufBez] [nvarchar](100) NULL,
	[GAV_Name] [nvarchar](70) NULL,
	[GAV_ZHD] [nvarchar](70) NULL,
	[GAV_Postfach] [nvarchar](70) NULL,
	[GAV_Strasse] [nvarchar](70) NULL,
	[GAV_PLZ] [nvarchar](10) NULL,
	[GAV_Ort] [nvarchar](70) NULL,
	[GAV_AdressNr] [nvarchar](20) NULL,
	[GAV_Bank] [nvarchar](35) NULL,
	[GAV_BankPLZOrt] [nvarchar](35) NULL,
	[GAV_BankKonto] [nvarchar](35) NULL,
	[GAV_IBAN] [nvarchar](27) NULL,
	[Kanton] [nvarchar](150) NULL,
	[Organ] [nvarchar](70) NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_PVLArchiveDatabases](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DbName] [nvarchar](255) NULL,
	[DbConnstring] [nvarchar](255) NULL,
 CONSTRAINT [PK_tbl_PVLArchiveDatabases] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Tab_GAVStdParifond_2020](
	[GAV_Number] [INT] NULL,
	[Gruppe0] [NVARCHAR](255) NULL,
	[StdWeek] [MONEY] NULL,
	[StdMonth] [MONEY] NULL,
	[StdYear] [MONEY] NULL,
	[FAG] [MONEY] NOT NULL,
	[Fan] [MONEY] NOT NULL,
	[VAN] [MONEY] NOT NULL,
	[VAG] [MONEY] NOT NULL,
	[WAN] [MONEY] NOT NULL,
	[WAG] [MONEY] NOT NULL,
	[_FAG] [MONEY] NULL,
	[_FAN] [MONEY] NULL,
	[_WAG] [MONEY] NULL,
	[_WAN] [MONEY] NULL,
	[_VAG] [MONEY] NULL,
	[_VAN] [MONEY] NULL,
	[_WAG_S] [MONEY] NULL,
	[_WAN_S] [MONEY] NULL,
	[_WAG_J] [MONEY] NULL,
	[_WAN_J] [MONEY] NULL,
	[_VAG_S] [MONEY] NULL,
	[_VAN_S] [MONEY] NULL,
	[_VAG_J] [MONEY] NULL,
	[_VAN_J] [MONEY] NULL,
	[GAVKanton] [NVARCHAR](70) NULL,
	[Resor_FAG] [MONEY] NULL,
	[Resor_FAN] [MONEY] NULL,
	[ID] [INT] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[PVL_Publication_Info](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](50) NULL,
	[User_ID] [NVARCHAR](50) NULL,
	[ContractNumber] [NVARCHAR](20) NOT NULL,
	[VersionNumber] [INT] NOT NULL,
	[PublicationDate] [DATETIME] NULL,
	[Title] [NVARCHAR](MAX) NULL,
	[Content] [NVARCHAR](MAX) NULL,
	[Viewed] [BIT] NULL,
	[CreatedOn] [DATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
 CONSTRAINT [PK_PVL_Publication_Info] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/* ---------------------- end of creating tables -------------------------------- */

USE [spPVLPublicData]
GO


CREATE PROCEDURE [dbo].[Get PVL Address Data] 
	@gavnumber INT,
	@Kanton nvarchar(2),
	@BerufBez nvarchar(255) ,
	@Organ nvarchar(255) 
AS

BEGIN
SET NOCOUNT ON

SELECT TOP (1) ID ,
               RecNr ,
               GAVNumber ,
               BerufBez ,
               GAV_Name ,
               GAV_ZHD ,
               GAV_Postfach ,
               GAV_Strasse ,
               GAV_PLZ ,
               GAV_Ort ,
               GAV_AdressNr ,
               GAV_Bank ,
               GAV_BankPLZOrt ,
               GAV_BankKonto ,
               GAV_IBAN ,
               Kanton ,
               Organ
	   FROM dbo.[PVL Addresses]
	   WHERE (@gavnumber = 0 OR GAVNumber = @gavnumber)
	   AND (@BerufBez = '' OR BerufBez = @BerufBez) 
	   AND (@Kanton = '' OR Kanton = @Kanton)
	   AND (@Organ = '' OR Organ = @Organ)
	   ORDER BY ID DESC	
END

GO


CREATE PROCEDURE [dbo].[Get FL GAV Gruppe0 Data] 

AS
BEGIN
SET NOCOUNT ON

SELECT Gruppe0 
	INTO #gav
	FROM dbo.[GavListe FL]
	WHERE (Deletedrec Is Null Or Deletedrec <> 1) 
	AND (ISNULL(Gruppe0, '') <> '')
	GROUP By Gruppe0 Order By Gruppe0


SELECT g.Gruppe0, (SELECT TOP 1 FL.ID FROM dbo.[GavListe FL] FL WHERE FL.Gruppe0 = G.Gruppe0 ORDER BY ID) GAVNr
	FROM #gav G
	ORDER BY G.Gruppe0

END
GO


CREATE PROCEDURE [dbo].[Get FL GAV Gruppe1 Data] 
	@Gruppe0 nvarchar(255)
AS
BEGIN
SET NOCOUNT ON

Select Gruppe1 From dbo.[GavListe FL]
	WHERE (Deletedrec Is Null Or Deletedrec <> 1) 
	AND ISNULL(Gruppe1, '') <> ''
	AND (@Gruppe0 = '' OR Gruppe0 = @Gruppe0)
	GROUP By Gruppe1 Order By Gruppe1

END
GO


CREATE PROCEDURE [dbo].[Get FL GAV Gruppe2 Data] 
	@Gruppe0 nvarchar(255),
	@Gruppe1 nvarchar(255)
AS
BEGIN
SET NOCOUNT ON

Select Gruppe2 From dbo.[GavListe FL]
	WHERE (Deletedrec Is Null Or Deletedrec <> 1) 
	AND ISNULL(Gruppe2, '') <> ''
	AND (@Gruppe0 = '' OR Gruppe0 = @Gruppe0)
	AND (ISNULL(@Gruppe1, '') = '' OR Gruppe1 = @Gruppe1)
	GROUP By Gruppe2 Order By Gruppe2

END
GO


CREATE PROCEDURE [dbo].[Get FL GAV Gruppe3 Data] 
	@Gruppe0 nvarchar(255),
	@Gruppe1 nvarchar(255),
	@Gruppe2 nvarchar(1000) 
AS
BEGIN
SET NOCOUNT ON

Select Gruppe3 From dbo.[GavListe FL]
	WHERE (Deletedrec Is Null Or Deletedrec <> 1) 
	AND ISNULL(Gruppe3, '') <> ''
	AND (@Gruppe0 = '' OR Gruppe0 = @Gruppe0)
	AND (ISNULL(@Gruppe1, '') = '' OR Gruppe1 = @Gruppe1)
	AND (ISNULL(@Gruppe2, '') = '' OR Gruppe2 = @Gruppe2)
	GROUP By Gruppe3 Order By Gruppe3

END
GO


CREATE PROCEDURE [dbo].[Get FL GAV Text Data] 
	@Gruppe0 nvarchar(255),
	@Gruppe1 nvarchar(1000)='',
	@Gruppe2 nvarchar(1000)='', 
	@Gruppe3 nvarchar(1000)='' 
AS

BEGIN
SET NOCOUNT ON

Select GAVText From dbo.[GavListe FL]
	WHERE (Deletedrec Is Null Or Deletedrec <> 1) 
	AND ISNULL(GAVText, '') <> ''
	AND (@Gruppe0 = '' OR Gruppe0 = @Gruppe0)
	AND (ISNULL(@Gruppe1, '') = '' OR Gruppe1 = @Gruppe1)
	AND (ISNULL(@Gruppe2, '') = '' OR Gruppe2 = @Gruppe2)
	AND (ISNULL(@Gruppe3, '') = '' OR Gruppe3 = @Gruppe3)
	GROUP By GAVText Order By GAVText

END
GO


CREATE PROCEDURE [dbo].[Get FL GAV Salary Data] 
	@Gruppe0 nvarchar(255),
	@Gruppe1 nvarchar(1000)='',
	@Gruppe2 nvarchar(1000)='', 
	@Gruppe3 nvarchar(1000)='',
	@GAVText nvarchar(1000)='' 
AS
BEGIN
SET NOCOUNT ON

Select ID ,
       GAVNr ,
       Deletedrec ,
       GavKanton ,
       Gruppe0 ,
       Gruppe1 ,
       Gruppe2 ,
       Gruppe3 ,
       GavText ,
       CalcFerien ,
       Calc13Lohn ,
       Minlohn ,
       FeiertagLohn ,
       Feierbtr ,
       FerienLohn ,
       Ferienbtr ,
       Lohn13 ,
       Lohn13btr ,
       StdLohn ,
       Monatslohn ,
       Mittagszulagen ,
       FAG ,
       FAN ,
       WAG ,
       WAN ,
       VAG ,
       VAN ,
       FAG_S ,
       FAN_S ,
       WAG_S ,
       WAN_S ,
       VAG_S ,
       VAN_S ,
       FAG_M ,
       FAN_M ,
       WAG_M ,
       WAN_M ,
       VAG_M ,
       VAN_M ,
       FAG_J ,
       FAN_J ,
       WAG_J ,
       WAN_J ,
       VAG_J ,
       VAN_J ,
       GueltigAb ,
       GueltigBis ,
       ZusatzFeier ,
       Zusatz13Lohn ,
       [FAR-Pflicht] ,
       Ferientext ,
       Lohn13text ,
       SollStd ,
       StdWeek ,
       StdMonth ,
       StdYear ,
       F_Alter ,
       L_Alter ,
       [Zusatz LA] ,
       Zusatz1 ,
       Zusatz2 ,
       Zusatz3 ,
       Zusatz4 ,
       Zusatz5 ,
       Zusatz6 ,
       Zusatz7 ,
       Zusatz8 ,
       Zusatz9 ,
       Zusatz10 ,
       Zusatz11 ,
       Zusatz12 From dbo.[GavListe FL] 
	WHERE (Deletedrec Is Null Or Deletedrec <> 1) 
	AND (@Gruppe0 = '' OR Gruppe0 = @Gruppe0) 
	AND (ISNULL(@Gruppe1, '') = '' OR Gruppe1 = @Gruppe1)
	AND (ISNULL(@Gruppe2, '') = '' OR Gruppe2 = @Gruppe2)
	AND (ISNULL(@Gruppe3, '') = '' OR Gruppe3 = @Gruppe3)
	AND (ISNULL(@GAVText, '') = '' OR GAVText = @GAVText)

END
GO


CREATE PROCEDURE [dbo].[List PVL Assigned Advisor Publication Data] 
       @customerID NVARCHAR(50), 
       @userID NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON

Select ID,
       Customer_ID,
       User_ID,
       ContractNumber,
       VersionNumber,
       PublicationDate,
       Title,
       Content,
       Viewed,
       CreatedOn,
       CreatedFrom 
	   FROM dbo.PVL_Publication_Info Where (ISNULL(@customerID, '') = '' OR Customer_ID = @customerID )
       AND (ISNULL(@userID, '') = '' OR User_ID = @userID )
       ORDER BY PublicationDate DESC	
END;
GO


CREATE PROCEDURE [dbo].[Update Assigned Advisor Publication View Data] 
       @customerID NVARCHAR(50), 
       @userID NVARCHAR(50),
       @recID INT,
	   @ContractNumber NVARCHAR(20),
       @VersionNumber INT,
       @PublicationDate DATETIME,
	   @Title NVARCHAR(255),
	   @checked BIT,
       @userData NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON

DELETE dbo.PVL_Publication_Info
WHERE ID = ISNULL(@recID, 0) AND Customer_ID = @customerID AND User_ID = @userID

IF ISNULL(@checked, 0) = 0 
BEGIN
	RETURN
END

INSERT INTO dbo.PVL_Publication_Info
(
    Customer_ID,
    User_ID,
    ContractNumber,
    VersionNumber,
    PublicationDate,
    Title,
    Viewed,
    CreatedOn,
    CreatedFrom
)
VALUES
(   @customerID,       -- Customer_ID - nvarchar(50)
    @userID,       -- User_ID - nvarchar(50)
    @ContractNumber,       -- ContractNumber - nvarchar(20)
    @VersionNumber,         -- VersionNumber - int
    @PublicationDate, -- PublicationDate - datetime
    @Title,       -- Title - nvarchar(max)
    @checked,      -- Viewed - bit
    GETDATE(), -- CreatedOn - datetime
    @userData        -- CreatedFrom - nvarchar(255)
    );

END;
GO

/* ------------------ end of creating sp --------------------------------------- */

/* ------------------ end of query --------------------------------------------- */
