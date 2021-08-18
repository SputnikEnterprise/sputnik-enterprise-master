
USE [master]
GO

CREATE DATABASE [spSystemInfo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'spSystemInfo', FILENAME = N'<your path>\spSystemInfo.mdf' , SIZE = 7819264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'spSystemInfo_log', FILENAME = N'<your path>\spSystemInfo_log.ldf' , SIZE = 6782720KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO

USE [spSystemInfo]
GO


CREATE TABLE [dbo].[tbl_CustomerSetting](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](50) NULL,
	[Customer_Name] [nvarchar](70) NULL,
	[KD_ZHD] [nvarchar](70) NULL,
	[ModulName] [nvarchar](50) NULL,
	[Vacancy_ID] [nvarchar](50) NULL,
	[Employee_ID] [nvarchar](50) NULL,
	[Employment_ID] [nvarchar](50) NULL,
	[Customer_Strasse] [nvarchar](255) NULL,
	[Customer_Ort] [nvarchar](255) NULL,
	[Customer_Telefon] [nvarchar](70) NULL,
	[Customer_Telefax] [nvarchar](70) NULL,
	[Customer_eMail] [nvarchar](255) NULL,
	[Customer_Homepage] [nvarchar](255) NULL,
	[Customer_Logo] [varbinary](max) NULL,
	[Customer_AGB] [varbinary](max) NULL,
	[Customer_cssFile] [nvarchar](255) NULL,
	[PrintVerleih] [int] NULL,
	[Customer_AGBFest] [varbinary](max) NULL,
	[Customer_AGBSonst] [varbinary](max) NULL,
	[Customer_AGBFest_I] [varbinary](max) NULL,
	[Customer_AGBSonst_I] [varbinary](max) NULL,
	[Customer_AGBFest_F] [varbinary](max) NULL,
	[Customer_AGBSonst_F] [varbinary](max) NULL,
	[Customer_AGBFest_E] [varbinary](max) NULL,
	[Customer_AGBSonst_E] [varbinary](max) NULL,
	[Customer_AGB_I] [varbinary](max) NULL,
	[Customer_AGB_F] [varbinary](max) NULL,
	[Customer_AGB_E] [varbinary](max) NULL,
	[Rahmenvertrag] [varbinary](max) NULL,
	[Rahmenvertrag_I] [varbinary](max) NULL,
	[Rahmenvertrag_F] [varbinary](max) NULL,
	[Rahmenvertrag_E] [varbinary](max) NULL,
	[AutoNotification] [int] NULL,
	[Visible_Candidate_Fields] [nvarchar](255) NULL,
	[Visible_Vacancy_Fields] [nvarchar](255) NULL,
	[Autonotification_MA] [bit] NULL,
	[Autonotification_KD] [bit] NULL,
	[GAVUnia] [nvarchar](255) NULL,
	[TplFilename] [nvarchar](255) NULL,
 CONSTRAINT [PK_tbl_CustomerSetting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Notify](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](50) NULL,
	[NotifyHeader] [nvarchar](255) NULL,
	[NotifyComments] [nvarchar](max) NULL,
	[NotifyArt] [int] NULL,
	[CreatedOn] [smalldatetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
	[CheckedOn] [smalldatetime] NULL,
	[CheckedFrom] [nvarchar](255) NULL,
 CONSTRAINT [PK_tbl_Notify] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_ErrorMessage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](50) NULL,
	[SourceModul] [nvarchar](255) NULL,
	[MessageHeader] [nvarchar](4000) NULL,
	[MessageContent] [nvarchar](4000) NULL,
	[CreatedOn] [smalldatetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
 CONSTRAINT [PK_tbl_ErrorMessage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Notify_Viewed](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](50) NULL,
	[User_ID] [nvarchar](50) NULL,
	[NotifyID] [int] NULL,
	[CheckedOn] [smalldatetime] NULL,
	[CheckedFrom] [nvarchar](255) NULL,
 CONSTRAINT [PK_tbl_Notify_Viewed] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_ServiceProviderData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProviderName] [nvarchar](255) NULL,
	[AccountName] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[Password] [nvarchar](4000) NULL,
 CONSTRAINT [PK_tbl_ServiceProviderData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_eCallLOGs](
	[RecID] [int] NULL,
	[Datum] [smalldatetime] NULL,
	[Content] [nvarchar](4000) NULL,
	[Status] [int] NULL,
	[sender] [nvarchar](255) NULL,
	[Recipient] [nvarchar](255) NULL,
	[JobGuid] [nvarchar](255) NULL,
	[JobPoint] [money] NULL,
	[Fakturiert] [bit] NULL,
	[Customer_Name] [nvarchar](255) NULL,
	[Customer_ID] [nvarchar](255) NULL,
	[Fak_Date] [datetime] NULL,
	[ResultCode] [int] NULL,
	[ResultMessage] [nvarchar](255) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_eCallLOGs_New](
	[RecID] [int] NULL,
	[Datum] [smalldatetime] NULL,
	[FinishDate] [datetime] NULL,
	[Content] [nvarchar](4000) NULL,
	[Status] [int] NULL,
	[Receiver] [nvarchar](255) NULL,
	[Originator] [nvarchar](255) NULL,
	[sender] [nvarchar](255) NULL,
	[NotificationDate] [datetime] NULL,
	[JobGuid] [nvarchar](255) NULL,
	[JobPoint] [money] NULL,
	[TypeOfMessage] [int] NULL,
	[ResultMessage] [nvarchar](4000) NULL,
	[ResultCode] [int] NULL,
	[Customer_ID] [nvarchar](255) NULL,
	[User_ID] [nvarchar](255) NULL,
	[Fakturiert] [bit] NULL,
	[Fak_Date] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tblCustomerPayableServices](
	[Customer_Guid] [nvarchar](255) NULL,
	[User_Guid] [nvarchar](255) NULL,
	[ServiceName] [nvarchar](255) NULL,
	[Servicedate] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
	[UsedPoints] [nvarchar](50) NULL,
	[JobID] [nvarchar](255) NULL,
	[AuthorizedCredit] [bit] NULL,
	[AuthorizedPoints] [money] NULL,
	[Fakturiert] [bit] NULL,
	[T2] [money] NULL,
	[Validated] [bit] NULL,
	[ValidatedOn] [datetime] NULL,
	[Fak_Date] [datetime] NULL,
	[JobID_2] [nvarchar](255) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[_MySetting](
	[MDGuid] [nvarchar](100) NULL,
	[MDName] [nvarchar](100) NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[SP_KDListe](
	[MDName] [nvarchar](255) NULL,
	[WV-User] [int] NULL,
	[User gekauft] [int] NULL,
	[GAV] [int] NULL,
	[DS] [int] NULL,
	[Email] [int] NULL,
	[Vakanz] [int] NULL,
	[WOS] [int] NULL,
	[QMS] [int] NULL,
	[ID] [int] NOT NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[SP_UserInfo](
	[ModulName] [nvarchar](50) NULL,
	[MDGuid] [nvarchar](100) NULL,
	[MDName] [nvarchar](100) NULL,
	[MachineName] [nvarchar](100) NULL,
	[UserData] [nvarchar](255) NULL,
	[Param] [nvarchar](4000) NULL,
	[CreatedOn] [datetime] NULL,
	[BiosData] [nvarchar](255) NULL,
	[WindowsKey] [nvarchar](255) NULL,
	[MachineDomain] [nvarchar](255) NULL,
	[NetworkID] [nvarchar](255) NULL,
	[HDDID] [nvarchar](255) NULL,
	[ProcessorID] [nvarchar](255) NULL,
	[UserGuid] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[CustomerLocation] [nvarchar](255) NULL,
	[CustomerTelefon] [nvarchar](255) NULL,
	[CustomerEMail] [nvarchar](255) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Customer_Advisors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](50) NULL,
	[User_ID] [nvarchar](50) NULL,
	[UserNr] [int] NULL,
	[Loginname] [nvarchar](200) NULL,
	[Logindata] [nvarchar](200) NULL,
	[Lastname] [nvarchar](255) NULL,
	[Firstname] [nvarchar](255) NULL,
	[Salutation] [nvarchar](50) NULL,
	[Postoffice] [nvarchar](70) NULL,
	[Street] [nvarchar](255) NULL,
	[Postcode] [nvarchar](70) NULL,
	[Location] [nvarchar](255) NULL,
	[Country] [nvarchar](70) NULL,
	[Telephone] [nvarchar](255) NULL,
	[Mobile] [nvarchar](255) NULL,
	[EMail] [nvarchar](255) NULL,
	[Birthdate] [datetime] NULL,
	[Language] [nvarchar](70) NULL,
	[Shortcut] [nvarchar](10) NULL,
	[Branchoffice] [nvarchar](200) NULL,
	[Firsttitle] [nvarchar](100) NULL,
	[Secondtitle] [nvarchar](100) NULL,
	[EMail_UserName] [nvarchar](255) NULL,
	[EMail_UserPW] [nvarchar](255) NULL,
	[EMail_SMTP] [nvarchar](255) NULL,
	[jch_layoutID] [int] NULL,
	[jch_logoID] [int] NULL,
	[OstJob_ID] [nvarchar](10) NULL,
	[ostjob_Kontingent] [int] NULL,
	[JCH_SubID] [int] NULL,
	[Deactivated] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
	[ChangedOn] [datetime] NULL,
	[ChangedFrom] [nvarchar](255) NULL,
	[AsCostCenter] [bit] NULL,
	[LogonMorePlaces] [bit] NULL,
 CONSTRAINT [PK_tbl_Customer_Advisors_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Customer_Notifications](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NotifyArt] [int] NULL,
	[User_ID] [nvarchar](50) NULL,
	[NotifyGroup] [nvarchar](255) NULL,
	[NotifyHeader] [nvarchar](max) NULL,
	[NotifyComments] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
	[CheckedOn] [datetime] NULL,
	[CheckedFrom] [nvarchar](255) NULL,
 CONSTRAINT [PK_tbl_Customer_Notifications] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Blacklist_Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [nvarchar](255) NULL,
	[InternalIPAddress] [nvarchar](255) NULL,
	[HostName] [nvarchar](255) NULL,
	[ExternalIPAddress] [nvarchar](255) NULL,
	[DomainName] [nvarchar](255) NULL,
	[Allowed] [bit] NULL,
	[Comments] [nvarchar](255) NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_tbl_Blacklist_Customer_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Blacklist_Station](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InternalIPAddress] [nvarchar](255) NULL,
	[HostName] [nvarchar](255) NULL,
	[ExternalIPAddress] [nvarchar](255) NULL,
	[UserName] [nvarchar](255) NULL,
	[DomainName] [nvarchar](255) NULL,
	[Allowed] [bit] NULL,
	[Comments] [nvarchar](255) NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_tbl_Blacklist_Station_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Logs](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[Level] [varchar](max) NOT NULL,
	[CallSite] [varchar](max) NOT NULL,
	[Type] [varchar](max) NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[StackTrace] [varchar](max) NOT NULL,
	[InnerException] [varchar](max) NOT NULL,
	[AdditionalInfo] [varchar](max) NOT NULL,
	[LoggedOnDate] [datetime] NOT NULL,
 CONSTRAINT [pk_logs] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_CustomerInfoForServices](
	[Customer_ID] [nvarchar](100) NULL,
	[Customer_Name] [nvarchar](100) NOT NULL,
	[DeniedServiceName] [nvarchar](50) NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[UserDStatistik](
	[USERData] [nvarchar](255) NULL,
	[MDName] [nvarchar](100) NULL,
	[FirstTimeThisMonth] [datetime] NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_JobplattformCustomerData](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](50) NULL,
	[CustomerNumber] [INT] NULL,
	[JobplattformLabel] [NVARCHAR](255) NULL,
	[CreatedOn] [SMALLDATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
 CONSTRAINT [PK_tbl_JobplattformCustomerData_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_CustomerData](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[CustomerNumber] [INT] NULL,
	[Customer_ID] [NVARCHAR](50) NULL,
	[CustomerName] [NVARCHAR](255) NULL,
	[Location] [NVARCHAR](255) NULL,
	[CustomerGroupNumber] [INT] NULL,
 CONSTRAINT [PK_tbl_CustomerData_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Customer](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[CustomerGroupNumber] [INT] NULL,
	[CustomerName] [NVARCHAR](255) NULL,
 CONSTRAINT [PK_tbl_Customer_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Logs] ADD  CONSTRAINT [df_logs_loggedondate]  DEFAULT (GETUTCDATE()) FOR [LoggedOnDate]
GO



/* ---------------------- end of creating tables -------------------------------- */




CREATE FUNCTION [dbo].[Uf_Load Radius For Given Postfach]
(
	@countryCode NVARCHAR(3) = 'CH' ,
	@Postcode NVARCHAR(10) ,
	@maxRadius INT
)
RETURNS 
@ParsedList table
(
	ID INT , CountryCode NVARCHAR(3), Postcode NVARCHAR(10), PlaceName NVARCHAR(255), AdminCode1 NVARCHAR(255), Latitude FLOAT, Longitude FLOAT, Distance DECIMAL(10, 5)
)
AS
BEGIN

DECLARE @clientLat  Float 
DECLARE @clientLong Float 

SELECT TOP 1 @clientLat = Latitude, @clientLong = Longitude FROM [spPublicData].dbo.tbl_PostcodeCoordinations
	WHERE CountryCode = @countryCode And Postcode = @postcode 

DECLARE @tbl_Listing TABLE (ID INT, Distance DECIMAL(18, 12));

INSERT INTO @tbl_Listing
(
    ID
	, Distance
)
    SELECT ID, [spPublicData].dbo.CalcDistanceBetweenLocations(@clientLat, @clientLong, [spPublicData].dbo.[tbl_PostcodeCoordinations].Latitude, [spPublicData].dbo.[tbl_PostcodeCoordinations].Longitude, 1)
	FROM [spPublicData].dbo.tbl_PostcodeCoordinations
    WHERE dbo.CalcDistanceBetweenLocations(@clientLat, @clientLong, [spPublicData].dbo.[tbl_PostcodeCoordinations].Latitude, [spPublicData].dbo.[tbl_PostcodeCoordinations].Longitude, 1) <= @maxRadius

INSERT INTO @ParsedList (ID, CountryCode , Postcode , PlaceName , AdminCode1 , Latitude , Longitude , Distance)
SELECT L.ID, PC.CountryCode, PC.Postcode, PC.PlaceName, PC.AdminCode1, PC.Latitude, PC.Longitude, L.Distance
	FROM @tbl_Listing L LEFT JOIN
	[spPublicData].dbo.tbl_PostcodeCoordinations PC ON PC.ID = L.ID 
	ORDER BY PC.CountryCode, L.Distance
RETURN
END
GO


CREATE FUNCTION [dbo].[uf_SplitIntegerKeys]
(
	@KeyList VARCHAR(4000),
    @separator CHAR(1) = ','
)
RETURNS 
@ParsedList TABLE
(
	KeyValue INT
)
AS
BEGIN
	DECLARE @keyID varchar(10), @Pos int

	SET @keyList = LTRIM(RTRIM(@keyList))+ @separator
	SET @Pos = CHARINDEX(@separator, @keyList, 1)

	IF REPLACE(@keyList, @separator, '') <> ''
	BEGIN
		WHILE @Pos > 0
		BEGIN
			SET @keyID = LTRIM(RTRIM(LEFT(@keyList, @Pos - 1)))
			IF @keyID <> '' AND TRY_CAST(@keyID AS int) IS NOT NULL 
			BEGIN
				INSERT INTO @ParsedList (KeyValue) 
				VALUES (CAST(@keyID AS int)) --Use Appropriate conversion
			END
			SET @keyList = RIGHT(@keyList, LEN(@keyList) - @Pos)
			SET @Pos = CHARINDEX(@separator, @keyList, 1)

		END
	END	
	RETURN
END

GO


CREATE FUNCTION [dbo].[Uf_SplitMyString]
(
    @StringToSplit VARCHAR(4000),
    @Separator VARCHAR(128)
)
RETURNS 
@ParsedList TABLE
(
	KeyValue NVARCHAR(255)
)
AS
BEGIN
	DECLARE @keyID NVARCHAR(255), @Pos INT

	SET @StringToSplit = LTRIM(RTRIM(@StringToSplit))+ @separator
	SET @Pos = CHARINDEX(@separator, @StringToSplit, 1)

	IF REPLACE(@StringToSplit, @separator, '') <> ''
	BEGIN
		WHILE @Pos > 0
		BEGIN
			SET @keyID = LTRIM(RTRIM(LEFT(@StringToSplit, @Pos - 1)))
			IF @keyID <> ''
			BEGIN
				INSERT INTO @ParsedList (KeyValue) 
				VALUES (CAST(@keyID AS NVARCHAR(255))) --Use Appropriate conversion
			END
			SET @StringToSplit = RIGHT(@StringToSplit, LEN(@StringToSplit) - @Pos)
			SET @Pos = CHARINDEX(@separator, @StringToSplit, 1)

		END
	END	
	RETURN
END

GO


CREATE FUNCTION [dbo].[CalcDistanceBetweenLocations]
      (@LatitudeA       FLOAT = NULL,
       @LongitudeA       FLOAT = NULL,
       @LatitudeB       FLOAT = NULL,
       @LongitudeB       FLOAT = NULL,
       @InKilometers      BIT = 0
       )
RETURNS FLOAT
AS
BEGIN
      -- just set @InKilometers to 0 for miles or 1 for km
      -- ex:  SELECT dbo.CalcDistanceBetweenLocations (30.123,27.1,28.14,32.23, 0)

      -- select field1, field2, dbo.CalcDistanceBetweenLocations(lat1, long1, lat2,
      -- long2, 0) as distance from yourtable
      -- where dbo.CalcDistanceBetweenLocations(lat1, long1, lat2,
      -- long2, 0) <= 10 --within the ten miles range
      DECLARE @Distance FLOAT

      SET @Distance = (SIN(RADIANS(@LatitudeA)) *
              SIN(RADIANS(@LatitudeB)) +
              COS(RADIANS(@LatitudeA)) *
              COS(RADIANS(@LatitudeB)) *
              COS(RADIANS(@LongitudeA - @LongitudeB)))

      --Get distance in miles
        SET @Distance = (DEGREES(ACOS(@Distance))) * 69.09

      --If specified, convert to kilometers
      IF @InKilometers = 1
            SET @Distance = @Distance * 1.609344

      RETURN @Distance

END
GO


CREATE FUNCTION [dbo].[uf_CalculateAgeFromToday]
(
	@DateOfBirth DATETIME ,
	@ToDate DATETIME
)
RETURNS INT 

AS
BEGIN
DECLARE @AgeinYear INT
IF @DateOfBirth IS NULL 
BEGIN 
RETURN 0
END 
IF @ToDate IS NULL 
BEGIN
SET @ToDate = GETDATE()
END

SELECT @AgeinYear = FLOOR((CAST (@ToDate AS INTEGER) - CAST(@DateOfBirth AS INTEGER)) / 365.25)

	RETURN ISNULL(@AgeinYear, 0)
END

GO

/* -------------------------- end of creating functions --------------------------------------- */


USE [spSystemInfo]
GO


CREATE PROCEDURE [dbo].[Update Assigned Notification]
	@CustomerID NVARCHAR(50),
	@RecordID INT,
	@UserID NVARCHAR(50),
	@Checked INT,
	@UserData NVARCHAR(255)
AS

BEGIN
SET NOCOUNT ON

if @Checked = 1 
begin
UPDATE dbo.tbl_Notify SET 
	CheckedOn = GETDATE(),
	CheckedFrom = @UserData
	WHERE Customer_ID = @CustomerID
	AND ID = @RecordID;
end
else
begin
UPDATE dbo.tbl_Notify SET 
	CheckedOn = Null,
	CheckedFrom = Null
	WHERE Customer_ID = @CustomerID
	AND ID = @RecordID;
end

if @Checked = 1
begin
INSERT INTO dbo.tbl_Notify_Viewed
        ( Customer_ID ,
          User_ID ,
          NotifyID ,
          CheckedOn ,
          CheckedFrom
        )
VALUES  ( @CustomerID , -- Customer_ID - nvarchar(50)
          @UserID , -- User_ID - nvarchar(50)
          @RecordID , -- NotifyID - int
          GETDATE() , -- CheckedOn - smalldatetime
          @UserData  -- CheckedFrom - nvarchar(255)
        )
end
else
begin
Delete tbl_Notify_Viewed Where Customer_ID = @CustomerID And NotifyID = @RecordID
end

END

GO


CREATE PROCEDURE [dbo].[Get Provider Login Data]
	@CustomerID NVARCHAR(50),
	@ProviderName NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON


IF NOT EXISTS (SELECT TOP (1) DeniedServiceName FROM dbo.tbl_CustomerInfoForServices WHERE Customer_ID = @CustomerID ) 
BEGIN
	IF NOT EXISTS (SELECT TOP (1) ID FROM spSystemInfo.dbo.tbl_Notify WHERE NotifyArt = 15 AND NotifyHeader = @ProviderName AND Customer_ID = @CustomerID )
	BEGIN
	INSERT INTO spSystemInfo.dbo.tbl_Notify
	(
			Customer_ID,
			NotifyHeader,
			NotifyComments,
			NotifyArt,
			CreatedOn,
			CreatedFrom
	)
	VALUES
	(   @CustomerID,                   -- Customer_ID - nvarchar(50)
			@ProviderName,                   -- NotifyHeader - nvarchar(255)
			N'customer_id not founded!',                   -- NotifyComments - nvarchar(max)
			15,                     -- NotifyArt - int
			GETDATE(), -- CreatedOn - smalldatetime
			N'system'                   -- CreatedFrom - nvarchar(255)
			)
	END
	ELSE
	BEGIN
		UPDATE spSystemInfo.dbo.tbl_Notify SET CreatedOn = GETDATE() WHERE NotifyArt = 15 AND NotifyHeader = @ProviderName AND Customer_ID = @CustomerID
	END
END

DECLARE @allowedCustomer BIT = 1
IF EXISTS (SELECT TOP (1) DeniedServiceName FROM dbo.tbl_CustomerInfoForServices WHERE Customer_ID = @CustomerID AND DeniedServiceName = @ProviderName)
	BEGIN
	SET @allowedCustomer = 0
	END

IF @allowedCustomer = 0 
	BEGIN
	SELECT ID ,
				 ProviderName ,
				 AccountName ,
				 UserName ,
				 [Password] UserPassword
		FROM dbo.tbl_ServiceProviderData
		WHERE ID = 0
	END
ELSE
	BEGIN
	SELECT ID ,
				 ProviderName ,
				 AccountName ,
				 UserName ,
				 [Password] UserPassword
		FROM dbo.tbl_ServiceProviderData
		WHERE (ProviderName = @ProviderName)

	END
END
GO


CREATE Procedure [dbo].[Get Customer for Payment Data]

As
Begin
SET NOCOUNT ON

SELECT * FROM tbl_CustomerInfoForServices 
	ORDER BY Customer_Name

End

GO



Create Procedure [dbo].[Get Customer UserName for Payment Data]
	@customerGuid nvarchar(50)

As
Begin
SET NOCOUNT ON

Select Distinct CreatedFrom As UserName From tblCustomerPayableServices 
	Where Customer_Guid = @customerGuid
	And CreatedOn Is Not Null 
	Order by CreatedFrom 
end

GO


CREATE Procedure [dbo].[Get Paid DeltaVista Service Data]
	@customerGuid nvarchar(50),
	@userName nvarchar(70) = '',
	@serviceName nvarchar(70) = '',
	@serviceDate nvarchar(10) = '',
	@jahr int = 2015,
	@monat int = 1

As
SET NOCOUNT ON

IF @serviceName = 'SOLVENCY_QUICK_CHECK'
BEGIN
	SET @serviceName = 'QUICK_CHECK_BUSINESS'
END
ELSE
BEGIN
	SET @serviceName = 'CREDIT_CHECK_BUSINESS'
END

Set @userName = @userName + '%'
Set @serviceName = @serviceName + '%'

SELECT t.ID,
	IsNull(t.Customer_Guid, '') As Customer_Guid, 
	IsNull(t.User_Guid , '') As User_Guid, 
	IsNull(t.ServiceName, '') As ServiceName, 
	IsNull(t.ServiceDate, '') As ServiceDate, 
	IsNull(t.CreatedOn, '') As CreatedOn,
	IsNull(t.CreatedFrom, '') As CreatedFrom,
	'' AS Content, '' AS Recipient, '' AS sender, 0 AS [Status],
	'' AS UserData,

	CONVERT(MONEY, 1) AS AuthorizedItems,
	CONVERT(MONEY, 1) AS AuthorizedCredit,

	IsNull(t.JobID, '') as JobID,
	t.Fakturiert, t.ValidatedOn Fak_Date,
	t.Validated
	, 0 ResultCode, '' ResultMessage
	
	FROM tblCustomerPayableServices t 

	WHERE 
	t.Customer_Guid = @customerGuid AND
	(@userName = '' Or t.CreatedFrom Like @userName) AND
	(@serviceDate = '' OR convert(nvarchar(10), t.Servicedate, 104) = @serviceDate) 
	And (@jahr = 0 OR Year(t.ServiceDate) = @jahr)
	And (@monat = 0 OR Month(t.ServiceDate) = @monat)
	And (@serviceName = '' Or t.ServiceName Like @serviceName)

	Order By t.ServiceDate Desc
GO




CREATE Procedure [dbo].[Get Paid Service Data]
	@customerGuid nvarchar(50),
	@userName nvarchar(70) = '',
	@serviceName nvarchar(70) = '',
	@serviceDate nvarchar(10) = '',
	@jahr int = 2015,
	@monat int = 1

As
SET NOCOUNT ON

SET @serviceName = @serviceName + '%'
IF @jahr = 0
BEGIN
	SET @jahr = YEAR(GETDATE())
END

SELECT e.ID, e.Datum ServiceDate, e.Content, e.[Status], e.Sender, e.Recipient,e.JobGuid, e.Fakturiert, e.Fak_Date, e.Customer_ID Customer_Guid
	, (
	CASE 
	WHEN e.sender <> '' THEN 'ECALL_FAXCREDIT' 
	WHEN e.sender = '' THEN 'ECALL_SMSCREDIT'
    ELSE 'Not Defined!'
	END ) AS ServiceName 
	, (
	CASE 
	WHEN e.sender <> '' THEN 
		( CASE WHEN e.Jobpoint > 1.5 THEN e.Jobpoint / 1.5
        ELSE 1
		END ) 

	WHEN e.sender = '' THEN 
		( CASE WHEN e.Jobpoint > 1.1 THEN e.Jobpoint / 1.1
        ELSE 1
		END ) 

	ELSE 1
	END ) AS AuthorizedItems
	, ISNULL( (SELECT TOP 1 CreatedFrom FROM dbo.tblCustomerPayableServices WHERE JobID = e.JobGuid), '') UserData
	, e.ResultCode, e.ResultMessage

	INTO #eCallLog

	FROM dbo.tbl_eCallLOGs e 
	WHERE 
	e.Customer_ID = @customerGuid AND
	(@serviceDate = '' OR convert(nvarchar(10), e.datum, 104) = @serviceDate) 
	And (@jahr = 0 OR Year(e.Datum) = @jahr)
	And (@monat = 0 OR Month(e.Datum) = @monat)

SELECT * FROM 
	#eCallLog 
	WHERE 
	(@userName = '' Or UserData Like @userName) AND
	(@serviceName = '' Or ServiceName Like @serviceName)
	AND ServiceDate >= '21.03.2015'
	Order By ServiceDate Desc

GO



CREATE PROCEDURE [dbo].[Get Paid Service Data For Selected JobGuid]
	@customerGuid nvarchar(50),
	@JobGuid nvarchar(70) = ''

AS
SET NOCOUNT ON

SELECT e.ID, e.Datum ServiceDate, e.Content, e.[Status], e.Sender, e.Recipient,e.JobGuid, e.Fakturiert, e.Customer_ID Customer_Guid
	, (
	CASE 
	WHEN e.sender <> '' THEN 'ECALL_FAXCREDIT' 
	WHEN e.sender = '' THEN 'ECALL_SMSCREDIT'
    ELSE 'Not Defined!'
	END ) AS ServiceName 
	, (
	CASE 
	WHEN e.sender <> '' THEN 
		( CASE WHEN e.Jobpoint > 1.5 THEN e.Jobpoint / 1.5
        ELSE 1
		END ) 

	WHEN e.sender = '' THEN 
		( CASE WHEN e.Jobpoint > 1.1 THEN e.Jobpoint / 1.1
        ELSE 1
		END ) 

	ELSE 1
	END ) AS AuthorizedItems
	, ISNULL( (SELECT TOP 1 CreatedFrom FROM dbo.tblCustomerPayableServices WHERE JobID = @JobGuid), '') UserData
	
	FROM dbo.tbl_eCallLOGs e 
	WHERE 
	e.Customer_ID = @customerGuid 
	AND e.JobGuid = @JobGuid



GO



CREATE Procedure [dbo].[Get Search DeltaVista Payment Data]
	@customerGuid nvarchar(50),
	@userName nvarchar(70) = '',
	@serviceName nvarchar(70) = '',
	@serviceDate nvarchar(10) = '',
	@jahr int = 2015,
	@monat int = 1

AS
SET NOCOUNT ON

IF @serviceName = 'SOLVENCY_QUICK_CHECK'
BEGIN
	SET @serviceName = 'QUICK_CHECK_BUSINESS'
END
ELSE
BEGIN
	SET @serviceName = 'CREDIT_CHECK_BUSINESS'
END

Set @userName = @userName + '%'
Set @serviceName = @serviceName + '%'

SELECT t.ID,
	IsNull(t.Customer_Guid, '') As Customer_Guid, 
	IsNull(t.User_Guid , '') As User_Guid, 
	IsNull(t.ServiceName, '') As ServiceName, 
	IsNull(t.ServiceDate, '') As ServiceDate, 
	IsNull(t.CreatedOn, '') As CreatedOn,
	IsNull(t.CreatedFrom, '') As CreatedFrom,

	CONVERT(MONEY, 1) AS AuthorizedItems,
	CONVERT(MONEY, 1) AS AuthorizedCredit,

	IsNull(t.JobID, '') as JobID,
	t.Fakturiert, 
	t.Fak_Date, 
	t.Validated
	
	FROM tblCustomerPayableServices t 

	WHERE 
	t.Customer_Guid = @customerGuid AND
	(@userName = '' Or t.CreatedFrom Like @userName) AND
	(@serviceDate = '' OR convert(nvarchar(10), t.Servicedate, 104) = @serviceDate) 
	And (@jahr = 0 OR Year(t.ServiceDate) = @jahr)
	And (@monat = 0 OR Month(t.ServiceDate) = @monat)
	And (@serviceName = '' Or t.ServiceName Like @serviceName)
	
	Order By t.ServiceDate Desc


GO


CREATE Procedure [dbo].[Get Search Payment Data]
	@customerGuid nvarchar(50),
	@userName nvarchar(70) = '',
	@serviceName nvarchar(70) = '',
	@serviceDate nvarchar(10) = '',
	@jahr int = 2015,
	@monat int = 1

As
SET NOCOUNT ON

SET @userName = @userName + '%'
Set @serviceName = @serviceName + '%'

BEGIN TRY DROP TABLE #t END TRY BEGIN CATCH END CATCH;

SELECT t.ID,
	IsNull(t.Customer_Guid, '') As Customer_Guid, 
	IsNull(t.User_Guid , '') As User_Guid, 
	IsNull(t.ServiceName, '') As ServiceName, 
	IsNull(t.ServiceDate, '') As ServiceDate, 
	IsNull(t.CreatedOn, '') As CreatedOn,
	IsNull(t.CreatedFrom, '') As CreatedFrom,
	CONVERT(MONEY, (
	CASE 
	WHEN t.ServiceName = 'ECALL_FAXCREDIT' THEN IsNull(t.t2, 0) 
	WHEN t.ServiceName = 'ECALL_SMSCREDIT' THEN IsNull(t.t2, 0) 
	WHEN t.ServiceName = 'QUICK_CHECK_BUSINESS' THEN 1
	WHEN t.ServiceName = 'CREDIT_CHECK_BUSINESS' THEN 1
	ELSE 0
	END 
	)) AS AuthorizedItems,

	CONVERT(MONEY, ISNULL(t.UsedPoints, 0)) AS AuthorizedCredit,

	IsNull(t.JobID, '') as JobID,
	t.AuthorizedCredit,
	t.validated,
	t.Fakturiert,
	t.Fak_Date

	FROM tblCustomerPayableServices t 
	WHERE 
	t.Customer_Guid = @customerGuid 
	AND (@userName = '' Or t.CreatedFrom Like @userName) 
	AND (@serviceDate = '' OR convert(nvarchar(10), t.Servicedate, 104) = @serviceDate) 
	And (@jahr = 0 OR Year(t.ServiceDate) = @jahr)
	And (@monat = 0 OR Month(t.ServiceDate) = @monat)
	And (@serviceName = '' Or t.ServiceName Like @serviceName)
	and t.Servicedate > (SELECT TOP 1 MAX(e.Datum) FROM dbo.tbl_eCallLOGs e)
	Order By t.ServiceDate Desc

GO


CREATE PROCEDURE [dbo].[Insert Sputnik Login Data] 
	@Modulname nvarchar(255) = '', 
	@MDGuid nvarchar(255) = '', 
	@MDname nvarchar(255) = '', 
	@UserGuid nvarchar(255) = '', 
	@Username nvarchar(255) = '', 
	@Machinename nvarchar(255) = '', 
	@DomainUsername nvarchar(255) = '', 
	@Domainname nvarchar(255) = ''

AS
BEGIN
SET NOCOUNT ON

INSERT  INTO [SP_UserInfo]
        ( ModulName ,
          MDGuid ,
          MDName ,
		  UserGuid,
		  UserName, 
          MachineName ,
          MachineDomain ,
          UserData ,
          CreatedOn
        )
VALUES  ( @Modulname ,
          LTRIM(@MDGuid) ,
          LTRIM(@MDname) ,
          LTRIM(@UserGuid) ,
          LTRIM(@UserName) ,
          LTRIM(@Machinename) ,
          LTRIM(@Domainname) ,
          LTRIM(@DomainUsername) ,
          GETDATE()
        );

END

GO


CREATE PROCEDURE [dbo].[Get Customer Data For ApplicationJobs]

AS

BEGIN
SET NOCOUNT ON

SELECT Customer_ID FROM [applicant].dbo.tbl_applicant
	WHERE (ISNULL(Customer_ID, '') <> '')
	AND IsNull(Firstname, '') <> '' 
	AND IsNull(Lastname, '') <> '' 
	AND CheckedOn IS NULL
	GROUP BY Customer_ID
	ORDER BY Customer_ID DESC

END

GO


CREATE PROCEDURE [dbo].[Insert Sputnik User Data]
    @CustomerID NVARCHAR(50) ,
    @UserID NVARCHAR(50) ,
    @UserNr INT ,
    @Loginname NVARCHAR(255) ,
    @Logindata NVARCHAR(255) ,
    @Lastname NVARCHAR(255) ,
    @Firstname NVARCHAR(255) ,
    @Salutation NVARCHAR(50) ,
    @Postoffice NVARCHAR(255) ,
    @Street NVARCHAR(255) ,
    @Postcode NVARCHAR(255) ,
    @Location NVARCHAR(255) ,
    @Country NVARCHAR(255) ,
    @Telephone NVARCHAR(50) ,
    @Mobile NVARCHAR(50) ,
    @EMail NVARCHAR(255) ,
    @Birthdate DATETIME ,
    @Language NVARCHAR(50) ,
    @Shortcut NVARCHAR(50) ,
    @Branchoffice NVARCHAR(255) ,
    @Firsttitle NVARCHAR(255) ,
    @Secondtitle NVARCHAR(255) ,
    @EMail_UserName NVARCHAR(255) ,
    @EMail_UserPW NVARCHAR(255) ,
    @EMail_SMTP NVARCHAR(255) ,
    @jch_layoutID INT ,
    @jch_logoID INT ,
    @OstJob_ID NVARCHAR(50) ,
    @ostjob_Kontingent INT ,
    @JCH_SubID INT ,
    @Deactivated BIT ,
    @AsCostCenter BIT ,
    @LogonMorePlaces BIT ,
    @createdFrom NVARCHAR(255)
AS
    BEGIN
        SET NOCOUNT ON;

IF EXISTS(SELECT TOP 1 @UserNr FROM dbo.tbl_Customer_Advisors WHERE     
Customer_ID = @CustomerID AND User_ID = @UserID AND UserNr = @UserNr)

BEGIN 

UPDATE [tbl_Customer_Advisors] 
SET Customer_ID = @CustomerID ,
User_ID = @UserID ,
UserNr = @UserNr ,
Loginname = @Loginname ,
Logindata = @Logindata ,
Lastname = @Lastname ,
Firstname = @Firstname ,
Salutation = @Salutation ,
Postoffice = @Postoffice ,
Street = @Street ,
Postcode = @Postcode ,
Location = @Location ,
Country = @Country ,
Telephone = @Telephone ,
Mobile = @Mobile ,
EMail = @EMail ,
Birthdate = @Birthdate ,
Language = @Language ,
Shortcut = @Shortcut ,
Branchoffice = @Branchoffice ,
Firsttitle = @Firsttitle ,
Secondtitle = @Secondtitle ,
EMail_UserName = @EMail_UserName ,
EMail_UserPW = @EMail_UserPW ,
EMail_SMTP = @EMail_SMTP ,
jch_layoutID = @jch_layoutID ,
jch_logoID = @jch_logoID ,
OstJob_ID = @OstJob_ID ,
ostjob_Kontingent = @ostjob_Kontingent ,
JCH_SubID = @JCH_SubID ,
Deactivated = @Deactivated 
,[AsCostCenter] = @AsCostCenter
,[LogonMorePlaces] = @LogonMorePlaces

,CreatedOn = GETDATE() 
,CreatedFrom = @CreatedFrom
WHERE 
Customer_ID = @CustomerID AND User_ID = @UserID AND UserNr = @UserNr

END

ELSE

BEGIN

        INSERT INTO [tbl_Customer_Advisors] (   Customer_ID ,
                                                User_ID ,
                                                UserNr ,
                                                Loginname ,
                                                Logindata ,
                                                Lastname ,
                                                Firstname ,
                                                Salutation ,
                                                Postoffice ,
                                                Street ,
                                                Postcode ,
                                                Location ,
                                                Country ,
                                                Telephone ,
                                                Mobile ,
                                                EMail ,
                                                Birthdate ,
                                                Language ,
                                                Shortcut ,
                                                Branchoffice ,
                                                Firsttitle ,
                                                Secondtitle ,
                                                EMail_UserName ,
                                                EMail_UserPW ,
                                                EMail_SMTP ,
                                                jch_layoutID ,
                                                jch_logoID ,
                                                OstJob_ID ,
                                                ostjob_Kontingent ,
                                                JCH_SubID ,
                                                Deactivated ,
                                                AsCostCenter ,
                                                LogonMorePlaces ,
                                                CreatedOn ,
                                                CreatedFrom
                                            )
        VALUES ( @CustomerID ,
                 @UserID ,
                 @UserNr ,
                 @Loginname ,
                 @Logindata ,
                 @Lastname ,
                 @Firstname ,
                 @Salutation ,
                 @Postoffice ,
                 @Street ,
                 @Postcode ,
                 @Location ,
                 @Country ,
                 @Telephone ,
                 @Mobile ,
                 @EMail ,
                 @Birthdate ,
                 @Language ,
                 @Shortcut ,
                 @Branchoffice ,
                 @Firsttitle ,
                 @Secondtitle ,
                 @EMail_UserName ,
                 @EMail_UserPW ,
                 @EMail_SMTP ,
                 @jch_layoutID ,
                 @jch_logoID ,
                 @OstJob_ID ,
                 @ostjob_Kontingent ,
                 @JCH_SubID ,
                 @Deactivated ,
				 @AsCostCenter ,
				 @LogonMorePlaces ,
                 GETDATE(),
                 @createdFrom
               );

    END;
END

GO


CREATE PROCEDURE [dbo].[List Customer Notifications]
	@UserID NVARCHAR(50),
	@excludeChecked BIT
AS

BEGIN
    SET NOCOUNT ON;

    IF @excludeChecked = 1
        BEGIN
            SET @UserID = '';
        END;

    SELECT   ID ,
             NotifyArt ,
             User_ID ,
             NotifyGroup ,
             NotifyHeader ,
             NotifyComments ,
             CreatedOn ,
             CreatedFrom ,
             CheckedOn ,
             CheckedFrom
    FROM     dbo.tbl_Customer_Notifications
    WHERE    @UserID = CASE WHEN @excludeChecked = 0 THEN @UserID
                            ELSE User_ID
                       END
    ORDER BY CreatedOn DESC ,
             CheckedOn;

END;
GO


CREATE PROCEDURE [dbo].[Add Customer Notification Content]
	@CustomerID NVARCHAR(50),
	@notifyHeader NVARCHAR(MAX),
	@notifyComment NVARCHAR(MAX),
	@UserData NVARCHAR(255)
AS

BEGIN
    SET NOCOUNT ON;
	INSERT INTO dbo.tbl_Customer_Notifications (   NotifyArt ,
	                                               User_ID ,
	                                               NotifyGroup ,
	                                               NotifyHeader ,
	                                               NotifyComments ,
	                                               CreatedOn ,
	                                               CreatedFrom 
	                                           )
	VALUES (   0 ,         -- NotifyArt - int
	           N'' ,       -- User_ID - nvarchar(50)
	           N'' ,       -- NotifyGroup - nvarchar(255)
	           @notifyHeader ,       -- NotifyHeader - nvarchar(255)
	           @notifyComment ,       -- NotifyComments - nvarchar(max)
	           GETDATE() , -- CreatedOn - datetime
	           @UserData        -- CreatedFrom - nvarchar(255)
	       )

END;
GO


CREATE PROCEDURE [dbo].[Update Assigned Customer Notification]
	@CustomerID NVARCHAR(50),
	@RecordID INT,
	@UserID NVARCHAR(50),
	@Checked INT,
	@UserData NVARCHAR(255)
AS

BEGIN
    SET NOCOUNT ON;

    IF @Checked = 1
        BEGIN
            UPDATE dbo.tbl_Customer_Notifications
            SET    User_ID = @UserID ,
                   CheckedOn = GETDATE() ,
                   CheckedFrom = @UserData
            WHERE  ID = @RecordID;
        END;
    ELSE
        BEGIN
            UPDATE dbo.tbl_Customer_Notifications
            SET    User_ID = '' ,
                   CheckedOn = NULL ,
                   CheckedFrom = ''
            WHERE  ID = @RecordID;
        END;

    INSERT INTO dbo.tbl_Notify_Viewed (   Customer_ID ,
                                          User_ID ,
                                          NotifyID ,
                                          CheckedOn ,
                                          CheckedFrom
                                      )
    VALUES (   @CustomerID , -- Customer_ID - nvarchar(50)
               @UserID ,     -- User_ID - nvarchar(50)
               @RecordID ,   -- NotifyID - int
               GETDATE() ,   -- CheckedOn - smalldatetime
               @UserData     -- CheckedFrom - nvarchar(255)
           );

END;
GO


CREATE PROCEDURE [dbo].[Update Assigned Customer Notification Content]
	@CustomerID NVARCHAR(50),
	@RecordID INT,
	@notifyHeader NVARCHAR(MAX),
	@notifyComment NVARCHAR(MAX),
	@UserData NVARCHAR(255)
AS

BEGIN
    SET NOCOUNT ON;

            UPDATE dbo.tbl_Customer_Notifications
            SET    User_ID = '' ,
                   CheckedOn = Null ,
                   NotifyHeader = @notifyHeader ,
                   NotifyComments = @notifyComment ,
				   CreatedOn = GETDATE() ,
				   CreatedFrom = @UserData ,
                   CheckedFrom = ''
            WHERE  ID = @RecordID;

END;
GO


CREATE PROCEDURE [dbo].[Add Notify Data]
	@CustomerID NVARCHAR(50),
          @NotifyHeader NVARCHAR(255) ,
          @NotifyComments NVARCHAR(MAX) ,
          @NotifyArt INT ,
          @CreatedFrom NVARCHAR(255) 
AS

BEGIN
SET NOCOUNT ON

INSERT INTO [spSystemInfo].dbo.tbl_Notify
        ( Customer_ID ,
          NotifyHeader ,
          NotifyComments ,
          NotifyArt ,
          CreatedOn ,
          CreatedFrom 
        )
VALUES  ( @CustomerID , -- Customer_ID - nvarchar(50)
          @NotifyHeader , -- NotifyHeader - nvarchar(255)
          @NotifyComments , -- NotifyComments - nvarchar(4000)
          @NotifyArt , -- NotifyArt - int
          GETDATE() , -- CreatedOn - smalldatetime
          @CreatedFrom  -- CreatedFrom - nvarchar(255)
        )
END

GO


CREATE PROCEDURE [dbo].[Station IS Allowed For Update]
	@LocalIPAddress NVARCHAR(50),
	@LocalHostName NVARCHAR(255),
	@LocalUserName NVARCHAR(255),
	@LocalDomainName NVARCHAR(255),
	@ExternalIPAddress NVARCHAR(255)
AS

BEGIN
SET NOCOUNT ON

DECLARE @existsData INT = 2

IF Not EXISTS(SELECT TOP (1) Allowed FROM [spSystemInfo].[dbo].[tbl_Blacklist_Station] bl
	WHERE bl.HostName = @LocalHostName 
	AND bl.DomainName = @LocalDomainName
	AND (ISNULL(@ExternalIPAddress, '') = '' OR bl.ExternalIPAddress = @ExternalIPAddress) 
	ORDER BY bl.Allowed DESC	
	)

BEGIN 
SET @existsData = 2
END 
ELSE
BEGIN 
SET @existsData = 1
END

IF @existsData = 2
BEGIN
INSERT INTO [spSystemInfo].[dbo].[tbl_Blacklist_Station] (   InternalIPAddress ,
                                      HostName ,
                                      ExternalIPAddress ,
                                      UserName ,
                                      DomainName ,
                                      Allowed ,
                                      Comments ,
									  CreatedOn
                                  )
VALUES (   @LocalIPAddress ,  -- InternalIPAddress - nvarchar(255)
           @LocalHostName ,  -- HostName - nvarchar(255)
           @ExternalIPAddress ,  -- ExternalIPAddress - nvarchar(255)
           @LocalUserName ,  -- UserName - nvarchar(255)
           @LocalDomainName ,  -- DomainName - nvarchar(255)
           0 , -- Allowed - bit
           N'' ,   -- Comments - nvarchar(255)
		   GETDATE()
       )
END

SELECT TOP 1 ISNULL(Allowed, 0) Allowed
	FROM [spSystemInfo].[dbo].[tbl_Blacklist_Station] bl
	WHERE bl.HostName = @LocalHostName 
	AND bl.DomainName = @LocalDomainName
	AND (ISNULL(@ExternalIPAddress, '') = '' OR bl.ExternalIPAddress = @ExternalIPAddress) 
	ORDER BY bl.Allowed DESC

END

GO


CREATE PROCEDURE [dbo].[Customer IS Allowed For Update]
	@CustomerID NVARCHAR(50),
	@LocalIPAddress NVARCHAR(50),
	@LocalHostName NVARCHAR(255),
	@LocalUserName NVARCHAR(255),
	@LocalDomainName NVARCHAR(255),
	@ExternalIPAddress NVARCHAR(255)
AS

BEGIN
SET NOCOUNT ON

DECLARE @existsData INT = 2
IF Not EXISTS(SELECT 1 FROM [spSystemInfo].[dbo].[tbl_Blacklist_Customer] WHERE CustomerID = @CustomerID) 
BEGIN 
SET @existsData = 2
END 
ELSE
BEGIN 
SET @existsData = 1
END

IF @existsData = 2
BEGIN
INSERT INTO [spSystemInfo].[dbo].[tbl_Blacklist_Customer] (  CustomerID, InternalIPAddress ,
                                      HostName ,
                                      ExternalIPAddress ,
                                      DomainName ,
                                      Allowed ,
                                      Comments ,
									  CreatedOn
                                  )
VALUES (   @CustomerID,
@LocalIPAddress ,  -- InternalIPAddress - nvarchar(255)
           @LocalHostName ,  -- HostName - nvarchar(255)
           @ExternalIPAddress ,  -- ExternalIPAddress - nvarchar(255)
           @LocalDomainName ,  -- DomainName - nvarchar(255)
           1 , -- Allowed - bit
           N'' ,   -- Comments - nvarchar(255)
		   GETDATE()
       )

DECLARE @subject NVARCHAR(255) = 'customerID is inserted as allowed for FTP update!'
DECLARE @body NVARCHAR(max) = 'customerID is inserted as allowed for FTP update!<br><br>'
SET @body = @body + '<table><tr><strong>customerID:</strong> ' + ISNULL(@CustomerID, '') + '<br></tr>'
SET @body = @body + '<tr><strong>ip-address:</strong> ' + ISNULL(@LocalIPAddress, '') + '<br></tr>'
SET @body = @body + '<tr><strong>LocalHostName:</strong> ' + ISNULL(@LocalHostName, '') + '<br></tr>'
SET @body = @body + '<tr><strong>LocalDomainName:</strong> ' + ISNULL(@LocalDomainName, '') + '<br></tr>'
SET @body = @body + '<tr><strong>external-address:</strong> ' + ISNULL(@ExternalIPAddress, '') + '<br></tr></table>'

EXEC msdb.dbo.sp_send_dbmail 
    @profile_name = 'Blacklist_Data',
	@recipients='info@domain.com',
    @subject = @subject,
    @body = @body,
    @body_format = 'HTML' ;

END

SELECT ISNULL(Allowed, 0) Allowed
	FROM [spSystemInfo].[dbo].[tbl_Blacklist_Customer]
	WHERE (CustomerID = @CustomerID)

END

GO


create procedure [dbo].[InsertLog] 
(
	@level varchar(max),
	@callSite varchar(max),
	@type varchar(max),
	@message varchar(max),
	@stackTrace varchar(max),
	@innerException varchar(max),
	@additionalInfo varchar(max)
)
as

insert into dbo.Logs
(
	[Level],
	CallSite,
	[Type],
	[Message],
	StackTrace,
	InnerException,
	AdditionalInfo
)
values
(
	@level,
	@callSite,
	@type,
	@message,
	@stackTrace,
	@innerException,
	@additionalInfo
)


GO


CREATE PROCEDURE [dbo].[Send Notification For New Files To Sputnik]
	@CustomerID NVARCHAR(50),
	@LocalIPAddress NVARCHAR(50),
	@LocalHostName NVARCHAR(255),
	@LocalUserName NVARCHAR(255),
	@LocalDomainName NVARCHAR(255),
	@ExternalIPAddress NVARCHAR(255)
AS

BEGIN
SET NOCOUNT ON


DECLARE @subject NVARCHAR(255) = 'program must be updated!'
DECLARE @body NVARCHAR(max) = 'program must be updated!<br><br>'
SET @body = @body + '<table><tr><strong>customerID:</strong> ' + ISNULL(@CustomerID, '') + '<br></tr>'
SET @body = @body + '<tr><strong>ip-address:</strong> ' + ISNULL(@LocalIPAddress, '') + '<br></tr>'
SET @body = @body + '<tr><strong>LocalHostName:</strong> ' + ISNULL(@LocalHostName, '') + '<br></tr>'
SET @body = @body + '<tr><strong>LocalDomainName:</strong> ' + ISNULL(@LocalDomainName, '') + '<br></tr>'
SET @body = @body + '<tr><strong>external-address:</strong> ' + ISNULL(@ExternalIPAddress, '') + '<br></tr></table>'

EXEC msdb.dbo.sp_send_dbmail 
    @profile_name = 'UpdateNotification',
	@recipients='info@domain.com',
    @subject = @subject,
    @body = @body,
    @body_format = 'HTML' ;


END

GO


CREATE PROCEDURE [dbo].[Get Search CVLizer Data]
	@customerGuid nvarchar(50),
	@userName nvarchar(70) = '',
	@serviceName nvarchar(70) = '',
	@serviceDate nvarchar(10) = '',
	@jahr int = 2015,
	@monat int = 1

AS
SET NOCOUNT ON

Set @userName = @userName + '%'
Set @serviceName = @serviceName + '%'

SELECT t.ID,
	IsNull(t.Customer_Guid, '') As Customer_Guid, 
	IsNull(t.User_Guid , '') As User_Guid, 
	IsNull(t.ServiceName, '') As ServiceName, 
	IsNull(t.ServiceDate, '') As ServiceDate, 
	IsNull(t.CreatedOn, '') As CreatedOn,
	IsNull(t.CreatedFrom, '') As CreatedFrom,

	CONVERT(MONEY, 1) AS AuthorizedItems,
	CONVERT(MONEY, 1) AS AuthorizedCredit,

	IsNull(t.JobID, '') as JobID,
	t.Fakturiert, 
	t.Fak_Date, 
	t.Validated
	
	FROM tblCustomerPayableServices t 

	WHERE 
	t.Customer_Guid = @customerGuid AND
	(@userName = '' Or t.CreatedFrom Like @userName) AND
	(@serviceDate = '' OR convert(nvarchar(10), t.Servicedate, 104) = @serviceDate) 
	And (@jahr = 0 OR Year(t.ServiceDate) = @jahr)
	And (@monat = 0 OR Month(t.ServiceDate) = @monat)
	And (@serviceName = '' Or t.ServiceName Like @serviceName)
	AND ServiceDate >= '21.03.2015'
	
	Order By t.ServiceDate Desc

GO


CREATE PROCEDURE [dbo].[Load CVL Search Result Data]
	@CustomerID NVARCHAR(50),
	@postcode nvarchar(20),
	@radius INT,
	@functionTitels nvarchar(4000),
	@competences nvarchar(4000),
	@languages nvarchar(4000)

AS

BEGIN
SET NOCOUNT ON

DECLARE @tblProfile TABLE (CVLID INT, PersonID INT);
DECLARE @tblPostCode TABLE (PersonID INT);

INSERT INTO @tblProfile (PersonID, CVLID) SELECT ID, FK_CVLID FROM [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation 
	WHERE FK_CVLID IN (SELECT P.ID FROM [spCVLizerBaseInfo].dbo.tbl_CVLProfile P WHERE P.Customer_ID = @CustomerID)
	AND (ISNULL(FirstName, '') <> '' AND ISNULL(LastName, '') <> '')

IF ISNULL(@postcode, '') <> ''
BEGIN
	INSERT INTO @tblPostCode (PersonID) SELECT FK_PersonalID PersonalID FROM [spCVLizerBaseInfo].dbo.tbl_CVLAddress 
		WHERE FK_PersonalID IN (SELECT ID FROM @tblProfile) AND PostCode = @postcode;
	
	DELETE FROM @tblProfile WHERE PersonID NOT IN (SELECT PC.PersonID FROM @tblPostCode PC);
END


IF ISNULL(@functionTitels, '') <> ''
BEGIN
DECLARE @tblFunctionList TABLE (JobTitel NVARCHAR(255));
INSERT INTO @tblFunctionList SELECT KeyValue FROM [spSystemInfo].[dbo].[Uf_SplitMyString](@functionTitels,',')

DECLARE @tblWork TABLE (CVLID INT, WorkID INT);
INSERT INTO @tblWork
(
    CVLID, WorkID
)
SELECT FK_CVLID, ID FROM dbo.tbl_CVLWork WHERE FK_CVLID IN (SELECT CVLID FROM @tblProfile);

DECLARE @tblFunctions TABLE (WorkID INT);
INSERT INTO @tblFunctions (WorkID) SELECT FK_WorkID FROM dbo.tbl_CVLWorkPhases 
	WHERE FK_PhasesID IN (SELECT FK_WorkPhaseID FROM dbo.tbl_CVLWorkPhaseFunctions WHERE [Function] IN (SELECT FL.JobTitel FROM @tblFunctionList FL));

	DELETE FROM @tblWork WHERE WorkID NOT IN (SELECT F.WorkID FROM @tblFunctions F);

	DELETE FROM @tblProfile WHERE CVLID NOT IN (SELECT W.CVLID FROM @tblWork W);

END


SELECT P.ID PersonalID, P.FK_CVLID CVLProfileID, P.FirstName, P.LastName, P.DateOfBirth ,
	Profile.CreatedOn, Profile.Customer_ID, 
	(SELECT TOP 1 AP.EmployeeID FROM [applicant].dbo.tbl_applicant AP WHERE AP.Customer_ID = @CustomerID AND AP.CVLProfileID = Profile.ID) EmployeeID,
	CA.Street, CA.PostCode, CA.City Location, CA.State, CA.FK_CountryCode CountryCode
FROM [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation P 
LEFT JOIN [spCVLizerBaseInfo].dbo.tbl_CVLAddress CA ON CA.FK_PersonalID = P.ID	
LEFT JOIN [spCVLizerBaseInfo].dbo.tbl_CVLProfile Profile ON Profile.ID = P.FK_CVLID AND Profile.Customer_ID = @CustomerID
WHERE P.ID IN (SELECT tblP.PersonID FROM @tblProfile tblP) 
AND (ISNULL(P.FirstName, '') <> '' AND ISNULL(P.LastName, '') <> '')
ORDER BY P.LastName, P.FirstName

END;
GO


CREATE PROCEDURE [dbo].[Load Geo Coordination Data]
	@CustomerID NVARCHAR(50) ,
	@countryCode NVARCHAR(2) ,
	@firstPostcode NVARCHAR(20) ,
	@secondPostcode NVARCHAR(20)

AS

BEGIN
    SET NOCOUNT ON;

	IF ISNULL(@firstPostcode, '') = '' AND ISNULL(@secondPostcode, '') = ''
	BEGIN	
SELECT P.* FROM [spPublicData].dbo.[tbl_PostcodeCoordinations] p
	WHERE P.CountryCode = @countryCode
	ORDER BY p.PlaceName
END 
	ELSE
BEGIN	
SELECT P.* FROM [spPublicData].dbo.[tbl_PostcodeCoordinations] p
	WHERE P.CountryCode = @countryCode
	AND	P.postcode IN (@firstPostcode, @secondPostcode)
 END
 
END;
GO


CREATE PROCEDURE [dbo].[Load Geo Data For Given Postcode Radius]
	@countryCode NVARCHAR(3) = 'CH' ,
    @Postcode NVARCHAR(10) ,
    @maxRadius INT 
AS
BEGIN

DECLARE @clientLat FLOAT
DECLARE @clientLong FLOAT

SELECT TOP 1 @clientLat = Latitude, @clientLong = Longitude FROM [spPublicData].dbo.tbl_PostcodeCoordinations
	WHERE CountryCode = @countryCode And Postcode = @postcode

DECLARE @tbl_Listing TABLE (ID INT, Distance DECIMAL(18, 12));

INSERT INTO @tbl_Listing
(
    ID,
	Distance
)
    SELECT ID, [spPublicData].dbo.CalcDistanceBetweenLocations(@clientLat, @clientLong, [spPublicData].dbo.[tbl_PostcodeCoordinations].Latitude, [spPublicData].dbo.[tbl_PostcodeCoordinations].Longitude, 1)
	FROM [spPublicData].dbo.tbl_PostcodeCoordinations
    WHERE dbo.CalcDistanceBetweenLocations(@clientLat, @clientLong, [spPublicData].dbo.[tbl_PostcodeCoordinations].Latitude, [spPublicData].dbo.[tbl_PostcodeCoordinations].Longitude, 1) <= @maxRadius

SELECT L.ID, PC.CountryCode, PC.Postcode, PC.PlaceName, PC.AdminCode1, PC.Latitude, PC.Longitude, L.Distance
	FROM @tbl_Listing L LEFT JOIN
	[spPublicData].dbo.tbl_PostcodeCoordinations PC ON PC.ID = L.ID
	ORDER BY PC.CountryCode, L.Distance

END
GO


CREATE PROCEDURE [dbo].[Load Assigned Notifications]
	@CustomerID NVARCHAR(50),
	@notifyArt INT,
	@excludeChecked int
AS

BEGIN
SET NOCOUNT ON

SELECT ID, Customer_ID, NotifyHeader, NotifyComments, NotifyArt, CreatedOn, CreatedFrom, CheckedOn, CheckedFrom FROM dbo.tbl_Notify
	WHERE (ISNULL(Customer_ID, '') = '' OR Customer_ID = @CustomerID)
	AND (@notifyArt IS NULL OR NotifyArt = @notifyArt)
	AND (@excludeChecked = 0 OR CheckedOn IS NULL)
	ORDER BY CreatedOn DESC, CheckedOn

END
GO


CREATE PROCEDURE [dbo].[Load Assigned Notifications For TODO]
	@CustomerID NVARCHAR(50)

AS

BEGIN
SET NOCOUNT ON

SELECT N.ID, N.Customer_ID, N.NotifyHeader, N.NotifyComments, N.NotifyArt, N.CreatedOn, N.CreatedFrom, N.CheckedOn, N.CheckedFrom FROM dbo.tbl_Notify N
	WHERE 
	(N.NotifyArt IN (12, 13, 14))
AND N.ID NOT IN (SELECT TOP (1) NW.NotifyID FROM dbo.tbl_Notify_Viewed NW WHERE NW.Customer_ID = @CustomerID AND NW.NotifyID = N.ID ORDER BY NW.ID)
	ORDER BY CreatedOn DESC

END
GO


CREATE PROCEDURE [dbo].[Load Jobplattform Customer Data]
	@CustomerID NVARCHAR(50),
	@userID NVARCHAR(255),
	@CustomerNumber INT
AS

BEGIN
SET NOCOUNT ON

DECLARE @customerName NVARCHAR(255) = ISNULL((SELECT TOP 1 MDName FROM dbo.SP_UserInfo WHERE MDGuid = @CustomerID), '')
DECLARE @userFullName NVARCHAR(255) = ISNULL((SELECT TOP 1 (CA.Lastname + ', ' + CA.Firstname) FROM dbo.tbl_Customer_Advisors CA WHERE CA.Customer_ID = @CustomerID AND CA.[USER_ID] = @userID), '')
DECLARE @recID INT = 0

SELECT @recID = ID 
	FROM dbo.[tbl_JobplattformCustomerData]
	WHERE (Customer_ID = @CustomerID)
AND 
([CustomerNumber] = @CustomerNumber)

IF ISNULL(@recID, 0) = 0
BEGIN
SELECT 0 ID ,
       @CustomerID Customer_ID ,
       @CustomerNumber CustomerNumber ,
       '' JobplattformLabel ,
       GETDATE() [CreatedOn] ,
       '?' [CreatedFrom] ,
			 @customerName CustomerName ,
			 @userFullName AdvisorFullName

END
ELSE
BEGIN
SELECT ID ,
       Customer_ID ,
       CustomerNumber ,
       JobplattformLabel ,
       [CreatedOn] ,
       [CreatedFrom] ,
			 @customerName CustomerName ,
			 @userFullName AdvisorFullName
	FROM dbo.[tbl_JobplattformCustomerData]
	WHERE (Customer_ID = @CustomerID)
AND 
([CustomerNumber] = @CustomerNumber)
END

END
GO



CREATE Procedure [dbo].[Load Customer Denied Services Data]
	@customerID NVARCHAR(50)
As
Begin
SET NOCOUNT ON

SELECT * FROM tbl_CustomerInfoForServices 
	WHERE Customer_ID = @customerID
	ORDER BY Customer_Name

End
GO


CREATE PROCEDURE [dbo].[List Advisor Login By Assigned Date Data]
	@customerID nvarchar(50),
	@assignedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON
	
BEGIN TRY DROP TABLE #tmpUser END TRY BEGIN CATCH END CATCH;
BEGIN TRY DROP TABLE #tmpPLUser END TRY BEGIN CATCH END CATCH;
BEGIN TRY DROP TABLE #tmpMonthlyUser END TRY BEGIN CATCH END CATCH;
BEGIN TRY DROP TABLE #tmpPLMonthlyUser END TRY BEGIN CATCH END CATCH;
BEGIN TRY DROP TABLE #tmpUserFinal END TRY BEGIN CATCH END CATCH;

DECLARE @Month INT = MONTH(@assignedDate)
DECLARE @Year INT = YEAR(@assignedDate)

DECLARE @userData TABLE(
    UserCount INT NULL,
    UserMonthCount INT NULL,
	Customer_ID NVARCHAR(50) NULL,
	Customername NVARCHAR(255) NULL,
	AdvisorID NVARCHAR(50) NULL,
	Advisor NVARCHAR(255) NULL,
	LogYear INT NULL,
	LogMonth INT NULL,
	CreatedOn DATETIME NULL    
);

SELECT A.MDName CustomerName, A.UserName
INTO #tmpUser
FROM [spSystemInfo].dbo.SP_UserInfo A
WHERE 
A.UserName NOT IN (
  'username'
) 
AND (@assignedDate IS NULL OR CONVERT(NVARCHAR(10), A.CreatedOn, 104) = CONVERT(NVARCHAR(10), @assignedDate, 104))
AND (ISNULL(@CustomerID, '') = '' OR A.MDGuid = @CustomerID)
GROUP BY MDName, UserName, CONVERT(NVARCHAR(10), A.CreatedOn, 104)
ORDER BY MDName, UserName

SELECT 'CustomerGroup' CustomerName, A.UserName 
INTO #tmpPLUser
FROM [spSystemInfo].dbo.SP_UserInfo A
WHERE 
A.UserName IN (
'username'
) 
AND (@assignedDate IS NULL OR CONVERT(NVARCHAR(10), A.CreatedOn, 104) = CONVERT(NVARCHAR(10), @assignedDate, 104))
AND (ISNULL(@CustomerID, '') = '' OR A.MDGuid = @CustomerID)
GROUP BY A.UserName, CONVERT(NVARCHAR(10), A.CreatedOn, 104)
ORDER BY UserName


-- Monthly data
SELECT A.MDName CustomerName, A.UserName
INTO #tmpMonthlyUser
FROM [spSystemInfo].dbo.SP_UserInfo A
WHERE 
A.UserName NOT IN (
  'username'
) 
AND ( @assignedDate IS NULL OR (MONTH(A.CreatedOn) = @Month AND YEAR(A.CreatedOn) = @Year) )
AND (ISNULL(@CustomerID, '') = '' OR A.MDGuid = @CustomerID)
GROUP BY MDName, UserName
ORDER BY MDName, UserName

SELECT 'CustomerGroup' CustomerName, A.UserName 
INTO #tmpPLMonthlyUser
FROM [spSystemInfo].dbo.SP_UserInfo A
WHERE 
A.UserName IN (
  'username'
) 
AND 
(@assignedDate IS NULL OR (MONTH(A.CreatedOn) = @Month AND YEAR(A.CreatedOn) = @Year))
GROUP BY UserName
ORDER BY UserName




INSERT INTO @userData 
(
    UserCount,
	UserMonthCount,
    Customername,
    Advisor,
    LogYear,
    LogMonth
)
SELECT (SELECT COUNT(*) FROM #tmpUser T WHERE T.CustomerName = tu.CustomerName GROUP BY T.CustomerName) UserCount,
	   (SELECT COUNT(*) FROM #tmpMonthlyUser T WHERE T.CustomerName = tu.CustomerName GROUP BY T.CustomerName) UserMonthCount, 
	   tu.CustomerName ,
	   tu.UserName AdvisorName ,
	   YEAR(@assignedDate) ,
	   MONTH(@assignedDate) 
	   FROM #tmpUser tu;

INSERT INTO @userData 
(
    UserCount,
	UserMonthCount,
    Customername,
    Advisor,
    LogYear,
    LogMonth
)
SELECT (SELECT COUNT(*) FROM #tmpUser T WHERE T.CustomerName = tu.CustomerName GROUP BY T.CustomerName) UserCount,
	   (SELECT COUNT(*) FROM #tmpPLMonthlyUser T WHERE T.CustomerName = tu.CustomerName GROUP BY T.CustomerName) UserMonthCount, 
       tu.CustomerName,
	   tu.UserName AdvisorName ,
	   YEAR(@assignedDate) ,
	   MONTH(@assignedDate) 
	   FROM #tmpPLUser tu;

SELECT UserCount,
UserMonthCount,
       '' Customer_ID,
       Customername,
       '' AdvisorID,
       Advisor,
       LogYear,
       LogMonth,
       CONVERT(DATE, @assignedDate) CreatedOn 
	   FROM @userData
	   ORDER BY Customername, Advisor

END;
GO


CREATE PROCEDURE [dbo].[List Advisor Login Data By Assigned CustomerID And UserID For Month And Year]
	@customerID NVARCHAR(50),
	@assignedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON
	
BEGIN TRY DROP TABLE #tmpMonthlyUser END TRY BEGIN CATCH END CATCH;
BEGIN TRY DROP TABLE #tmpPLMonthlyUser END TRY BEGIN CATCH END CATCH;
BEGIN TRY DROP TABLE #tmpUserFinal END TRY BEGIN CATCH END CATCH;

DECLARE @Month INT = MONTH(@assignedDate)
DECLARE @Year INT = YEAR(@assignedDate)

DECLARE @userData TABLE(
    UserCount INT NULL,
    UserMonthCount INT NULL,
	Customer_ID NVARCHAR(50) NULL,
	Customername NVARCHAR(255) NULL,
	AdvisorID NVARCHAR(50) NULL,
	Advisor NVARCHAR(255) NULL,
	LogYear INT NULL,
	LogMonth INT NULL,
	CreatedOn DATETIME NULL    
);

SELECT A.MDName CustomerName, A.UserName
INTO #tmpMonthlyUser
FROM [spSystemInfo].dbo.SP_UserInfo A
WHERE 
A.UserName NOT IN (
  'username'
) 
AND ( @assignedDate IS NULL OR (MONTH(A.CreatedOn) = @Month AND YEAR(A.CreatedOn) = @Year) )
AND (ISNULL(@CustomerID, '') = '' OR A.MDGuid = @CustomerID)
GROUP BY MDName, UserName
ORDER BY MDName, UserName

SELECT 'CustomerGroup' CustomerName, A.UserName 
INTO #tmpPLMonthlyUser
FROM [spSystemInfo].dbo.SP_UserInfo A
WHERE 
A.UserName IN (
  'username'
) 
AND 
(@assignedDate IS NULL OR (MONTH(A.CreatedOn) = @Month AND YEAR(A.CreatedOn) = @Year))
GROUP BY UserName
ORDER BY UserName


INSERT INTO @userData 
(
    UserCount,
    Customername,
    Advisor,
    LogYear,
    LogMonth
)
SELECT (SELECT COUNT(*) FROM #tmpMonthlyUser T WHERE T.CustomerName = tu.CustomerName GROUP BY T.CustomerName) UserCount,
	   tu.CustomerName ,
	   tu.UserName AdvisorName ,
	   YEAR(@assignedDate) ,
	   MONTH(@assignedDate) 
	   FROM #tmpMonthlyUser tu;

INSERT INTO @userData 
(
    UserCount,
    Customername,
    Advisor,
    LogYear,
    LogMonth
)
SELECT (SELECT COUNT(*) FROM #tmpMonthlyUser T WHERE T.CustomerName = tu.CustomerName GROUP BY T.CustomerName) UserCount,
       tu.CustomerName,
	   tu.UserName AdvisorName ,
	   YEAR(@assignedDate) ,
	   MONTH(@assignedDate) 
	   FROM #tmpPLMonthlyUser tu;


SELECT UserCount, Customername, Advisor FROM @userData 
	   ORDER BY Customername, Advisor

END;
GO


/* ------------------ end of creating sp --------------------------------------- */

/* ------------------ end of query --------------------------------------------- */

