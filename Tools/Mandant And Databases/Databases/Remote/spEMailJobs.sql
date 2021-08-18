
USE [master]
GO

CREATE DATABASE [spEMailJobs]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'spEMailJobs', FILENAME = N'<your path>\spEMailJobs.mdf' , SIZE = 32635264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'spEMailJobs_log', FILENAME = N'<your path>\spEMailJobs_log.ldf' , SIZE = 84416KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO


USE [spEMailJobs]
GO


CREATE TABLE [dbo].[tbl_EMail_Attachment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_REID] [int] NULL,
	[DocumentCategoryNumber] [int] NULL,
	[AttachmentFileName] [nvarchar](255) NULL,
	[ScanContent] [varbinary](max) NULL,
	[CreatedOn] [smalldatetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
 CONSTRAINT [PK_tbl_EMail_Attachment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_EMail_BodyParsing_Pattern](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](50) NULL,
	[CustomerID] [nvarchar](255) NULL,
	[Lastname] [nvarchar](255) NULL,
	[Firstname] [nvarchar](255) NULL,
	[Gender] [nvarchar](255) NULL,
	[Street] [nvarchar](255) NULL,
	[PostOfficeBox] [nvarchar](255) NULL,
	[Postcode] [nvarchar](255) NULL,
	[Location] [nvarchar](255) NULL,
	[Country] [nvarchar](255) NULL,
	[Nationality] [nvarchar](255) NULL,
	[EMail] [nvarchar](255) NULL,
	[Telephone] [nvarchar](255) NULL,
	[MobilePhone] [nvarchar](255) NULL,
	[Birthdate] [nvarchar](255) NULL,
	[Permission] [nvarchar](255) NULL,
	[Profession] [nvarchar](255) NULL,
	[OtherProfession] [nvarchar](255) NULL,
	[Auto] [nvarchar](255) NULL,
	[Motorcycle] [nvarchar](255) NULL,
	[Bicycle] [nvarchar](255) NULL,
	[DrivingLicence1] [nvarchar](255) NULL,
	[DrivingLicence2] [nvarchar](255) NULL,
	[DrivingLicence3] [nvarchar](255) NULL,
	[CivilState] [nvarchar](255) NULL,
	[Language] [nvarchar](255) NULL,
	[LanguageLevel] [nvarchar](255) NULL,
	[VacancyNumber] [nvarchar](255) NULL,
	[ApplicationLabel] [nvarchar](255) NULL,
	[Advisor] [nvarchar](255) NULL,
	[BusinessBranch] [nvarchar](255) NULL,
	[Dismissalperiod] [nvarchar](255) NULL,
	[Availability] [nvarchar](255) NULL,
	[Comment] [nvarchar](255) NULL,
	[Attachment_CV] [nvarchar](255) NULL,
	[Attachment_1] [nvarchar](255) NULL,
	[Attachment_2] [nvarchar](255) NULL,
	[Attachment_3] [nvarchar](255) NULL,
	[Attachment_4] [nvarchar](255) NULL,
	[Attachment_5] [nvarchar](255) NULL,
	[CreatedOn] [smalldatetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
	[VacancyCustomerID] [nvarchar](50) NULL,
 CONSTRAINT [PK_tbl_EMail_BodyParsing_Pattern] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_EMail_Setting](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[Report_Senders] [NVARCHAR](255) NULL,
	[CV_Senders] [NVARCHAR](255) NULL,
	[Report_Recipients] [NVARCHAR](255) NULL,
	[CV_Recipients] [NVARCHAR](255) NULL,
	[Report_FTPUser] [NVARCHAR](255) NULL,
	[CV_FTPUser] [NVARCHAR](255) NULL,
	[Report_FTPPW] [NVARCHAR](255) NULL,
	[CV_FTPPW] [NVARCHAR](255) NULL,
	[Report_FTPRD] [NVARCHAR](255) NULL,
	[CV_FTPRD] [NVARCHAR](255) NULL,
	[MailUserName] [NVARCHAR](255) NULL,
	[MailPassword] [NVARCHAR](255) NULL,
	[SmtpServer] [NVARCHAR](255) NULL,
	[SmtpPort] [INT] NULL,
	[ActivateSSL] [BIT] NULL,
	[TemplateFolder] [NVARCHAR](255) NULL,
	[PriorityModul] [INT] NULL,
 CONSTRAINT [PK_EMail_Setting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Received_EMail](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](50) NULL,
	[EMailSubject] [NVARCHAR](255) NULL,
	[EMailUidl] [INT] NULL,
	[EMailFrom] [NVARCHAR](255) NULL,
	[EMailTo] [NVARCHAR](255) NULL,
	[HasHtmlBody] [BIT] NULL,
	[EMailPlainTextBody] [NVARCHAR](MAX) NULL,
	[EMailBody] [NVARCHAR](MAX) NULL,
	[CreatedOn] [SMALLDATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
	[EMailMime] [NVARCHAR](MAX) NULL,
	[ApplicationID] [INT] NULL,
	[Content] [VARBINARY](MAX) NULL,
	[EMailDate] [DATETIME] NULL,
	[EMLFileName] [NVARCHAR](255) NULL,
 CONSTRAINT [PK_tbl_Received_EMail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/* --------------- end of creating tables ----------------------------------- */

USE [spEMailJobs]
GO

CREATE FUNCTION [dbo].[RemoveAllSpaces]
(
    @InputStr NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
declare @S NVARCHAR(MAX) = @InputStr
declare @ResultStr varchar(MAX)

set @ResultStr = @InputStr

while charindex(CHAR(160), @ResultStr) > 0
    set @ResultStr = replace(@InputStr, CHAR(160), '')

set @ResultStr = replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(
    replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(
    replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(
    replace(replace(@ResultStr ,
        char(1), ''),char(2), ''),char(3), ''),char(4), ''),char(5), ''),char(6), ''),char(7), ''),char(8), ''),char(9), ''),char(10), ''),
        char(11), ''),char(12), ''),char(13), ''),char(14), ''),char(15), ''),char(16), ''),char(17), ''),char(18), ''),char(19), ''),char(20), ''),
        char(21), ''),char(22), ''),char(23), ''),char(24), ''),char(25), ''),char(26), ''),char(27), ''),char(28), ''),char(29), ''),char(30), ''),
        char(31), ''), char(0) COLLATE Latin1_General_100_BIN2, '')

set @ResultStr = replace(@ResultStr, ' ', '')
    ;with  cte1(N) As (Select 1 From (Values(1),(1),(1),(1),(1),(1),(1),(1),(1),(1)) N(N)),
           cte2(C) As (Select Top (32) Char(Row_Number() over (Order By (Select NULL))-1) From cte1 a,cte1 b)
    Select @S = Replace(@ResultStr, C, ' ')
     From  cte2

    Return ltrim(rtrim(replace(replace(replace(@S,' ','†‡'),'‡†',''),'†‡',' ')))

END
GO

/* ------------ create sp ---------------------------------- */

USE [spEMailJobs]
GO


CREATE PROCEDURE [dbo].[Create New EMailAttachment]
	@AttachmentName NVARCHAR(255),
    @ScanContent VARBINARY(max) ,
    @DocumentCategoryNumber INT ,
    @EMailID INT ,
    @CreatedFrom NVARCHAR(255) ,

	@NewId int OUTPUT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN


INSERT INTO dbo.tbl_EMail_Attachment
        ( FK_REID ,
          DocumentCategoryNumber ,
          AttachmentFileName ,
          ScanContent ,
          CreatedOn ,
          CreatedFrom
        )
VALUES  ( @EMailID , -- FK_REID - int
          @DocumentCategoryNumber , -- DocumentCategoryNumber - int
          @AttachmentName , -- AttachmentFileName - nvarchar(255)
          @ScanContent , -- ScanContent - varbinary(max)
          GETDATE() , -- CreatedOn - smalldatetime
          @CreatedFrom  -- CreatedFrom - nvarchar(255)
        )
			
		SET @NewId = @@Identity
						
			
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


CREATE PROCEDURE [dbo].[Load EMail Pattern For BodyParsing Data]
	@CustomerID NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT TOP 1
        ID ,
        Customer_ID ,
        CustomerID ,
        Lastname ,
        Firstname ,
        Gender ,
        Street ,
        PostOfficeBox ,
        Postcode ,
        Location ,
        Country ,
        Nationality ,
        EMail ,
        Telephone ,
        MobilePhone ,
        Birthdate ,
        Permission ,
        Profession ,
        OtherProfession ,
        Auto ,
        Motorcycle ,
        Bicycle ,
        DrivingLicence1 ,
        DrivingLicence2 ,
        DrivingLicence3 ,
        CivilState ,
        Language ,
        LanguageLevel ,
        VacancyCustomerID ,
        VacancyNumber ,
        ApplicationLabel ,
        Advisor ,
        BusinessBranch ,
        Dismissalperiod ,
        Availability ,
        Comment ,
        Attachment_CV ,
        Attachment_1 ,
        Attachment_2 ,
        Attachment_3 ,
        Attachment_4 ,
        Attachment_5 ,
        CreatedOn ,
        CreatedFrom
FROM    dbo.tbl_EMail_BodyParsing_Pattern
WHERE   ( Customer_ID = @CustomerID );

END

GO


CREATE PROCEDURE [dbo].[Load EMail Setting Data For CVParsing]
	@CustomerID NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT TOP (1) ID ,
             Customer_ID ,
             Report_Senders ,
             CV_Senders ,
             Report_Recipients ,
             CV_Recipients ,
             Report_FTPUser ,
             CV_FTPUser ,
             Report_FTPPW ,
             CV_FTPPW ,
             Report_FTPRD ,
             CV_FTPRD ,
             MailUserName ,
             MailPassword ,
             SmtpServer ,
             SmtpPort ,
             ActivateSSL ,
             TemplateFolder ,
             PriorityModul
	   FROM dbo.tbl_EMail_Setting
	   WHERE
	   Customer_ID = @CustomerID 
	   AND 
	   ISNULL(CV_Senders, '') <> ''
ORDER BY ID;
END

GO


Create PROCEDURE [dbo].[Load EMail Setting Data]
	@EMailFrom NVARCHAR(50),
	@EMailTo NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT TOP 1 ID ,
             Customer_ID ,
             Report_Senders ,
             CV_Senders ,
             Report_Recipients ,
             CV_Recipients ,
             Report_FTPUser ,
             CV_FTPUser ,
             Report_FTPPW ,
             CV_FTPPW ,
             Report_FTPRD ,
             CV_FTPRD ,
             MailUserName ,
             MailPassword ,
             SmtpServer ,
             SmtpPort ,
             ActivateSSL ,
             TemplateFolder
	   FROM dbo.tbl_EMail_Setting
	   WHERE  
	   (@EMailFrom LIKE '%' + Report_Senders + '%' OR @EMailFrom LIKE '%' + CV_Senders + '%')
	   AND 
	   (@EMailTo LIKE '%' + Report_Recipients + '%' OR @EMailTo LIKE '%' + CV_Recipients + '%')

END
GO


CREATE PROCEDURE [dbo].[Load User Data For Received EMail]
	@EMailFrom NVARCHAR(50),
	@EMailTo NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT TOP 1 ID ,
             Customer_ID ,
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
             CreatedOn ,
             CreatedFrom ,
             ChangedOn ,
             ChangedFrom
	   FROM [spSystemInfo].dbo.tbl_Customer_Advisors
	   WHERE ISNULL(Deactivated, 0) <> 1
	   AND ISNULL(EMail, '') <> ''
	   AND (@EMailFrom LIKE '%' + EMail + '%')

END
GO


CREATE PROCEDURE [dbo].[Load EMail Setting Data For ReportParsing]
	@CustomerID NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT TOP 1 ID ,
             Customer_ID ,
             Report_Senders ,
             CV_Senders ,
             Report_Recipients ,
             CV_Recipients ,
             Report_FTPUser ,
             CV_FTPUser ,
             Report_FTPPW ,
             CV_FTPPW ,
             Report_FTPRD ,
             CV_FTPRD ,
             MailUserName ,
             MailPassword ,
             SmtpServer ,
             SmtpPort ,
             ActivateSSL ,
             TemplateFolder
	   FROM dbo.tbl_EMail_Setting
	   WHERE 
	   Customer_ID = @CustomerID 
	   AND 
	   ISNULL(Report_Senders, '') <> ''

END
GO


CREATE PROCEDURE [dbo].[Create New EMailJob]
	@CustomerID NVARCHAR(50),
	@ApplicationID INT ,
	@EMailFrom NVARCHAR(255),
	@EMailTo NVARCHAR(255),
	@EMailSubject NVARCHAR(255),
	@EMailBody NVARCHAR(max) ,
	@EMailPlainTextBody NVARCHAR(max),
    @EMailUidl INT ,
    @ModulNumber INT ,
    @ExistsAttachment bit ,
    @HasHtmlBody bit ,
    @EMailDate datetime ,
	@CreatedFrom NVARCHAR(255) ,
	@eMailMime NVARCHAR(max) ,
	@Content VARBINARY(max) ,
	@EMLFilename NVARCHAR(255) ,

	@NewId int OUTPUT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN


INSERT INTO dbo.tbl_Received_EMail
        ( Customer_ID ,
          ApplicationID ,
          EMailSubject ,
          EMailUidl ,
          EMailFrom ,
          EMailTo ,
          HasHtmlBody ,
          EMailPlainTextBody ,
          EMailBody ,
          EMailMime ,
          Content ,
          EMailDate ,
          CreatedOn ,
          CreatedFrom ,
           EMLFileName
       )
VALUES  ( @CustomerID , -- Customer_ID - nvarchar(255)
          @ApplicationID ,
          @EMailSubject , -- EMailSubject - nvarchar(255)
          @EMailUidl , -- EMailUidl - int
          @EMailFrom , -- EMailFrom - nvarchar(255)
          @EMailTo , -- EMailTo - nvarchar(255)
          @HasHtmlBody , -- HasHtmlBody - bit
          @EMailPlainTextBody , -- EMailPlainTextBody - nvarchar(4000)
          @EMailBody , -- EMailBody - nvarchar(4000)
          @EMailMime ,
          @Content ,
          @EMailDate ,
          GETDATE() , -- CreatedOn - smalldatetime
          @CreatedFrom ,  -- CreatedFrom - nvarchar(255)
		  @EMLFileName
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


CREATE PROCEDURE [dbo].[Load EMail Data For Assigned Application]
	@CustomerID NVARCHAR(50),
	@applicationID INT

AS

BEGIN
SET NOCOUNT ON

SELECT TOP 1 ID ,
             Customer_ID ,
             EMailSubject ,
             EMailUidl ,
             EMailFrom ,
             EMailTo ,
             HasHtmlBody ,
             EMailPlainTextBody ,
             EMailBody ,
             CreatedOn ,
             CreatedFrom ,
             EMailMime ,
             ApplicationID ,
             Content

FROM  dbo.tbl_Received_EMail
WHERE (ISNULL(@CustomerID, '') = '' OR Customer_ID = @CustomerID)
AND ApplicationID = @applicationID

END;
GO


CREATE PROCEDURE [dbo].[Load EMail Attatchment Data For Assigned EMail]
	@eMailID INT

AS

BEGIN
SET NOCOUNT ON

SELECT ID ,
       FK_REID ,
       DocumentCategoryNumber ,
       AttachmentFileName ,
       ScanContent ,
       CreatedOn ,
       CreatedFrom

FROM  dbo.tbl_EMail_Attachment
WHERE FK_REID = @eMailID

END;
GO


CREATE PROCEDURE [dbo].[Load Assigned EMail Data For Duplicate Check]
	@CustomerID NVARCHAR(50),
	@EMailSubject NVARCHAR(255),
	@EMailBody NVARCHAR(MAX) ,
	@EMailPlainTextBody NVARCHAR(MAX)
	
AS

BEGIN
SET NOCOUNT ON

SET @EMailSubject = dbo.RemoveAllSpaces(@EMailSubject)
SET @EMailBody = dbo.RemoveAllSpaces(@EMailBody)
SET @EMailPlainTextBody = dbo.RemoveAllSpaces(@EMailPlainTextBody)

SELECT TOP (1)   
          ID ,
					Customer_ID ,
          EMailSubject ,
          EMailUidl ,
          ApplicationID ,
			ISNULL( (SELECT TOP (1) EmployeeID FROM [applicant].dbo.tbl_application WHERE id = ISNULL(ApplicationID, 0) ORDER BY ID), 0) EmployeeID ,
          EMailFrom ,
          EMailDate ,
          EMLFileName ,
          CreatedOn
FROM  dbo.tbl_Received_EMail
WHERE Customer_ID = @CustomerID
AND (ISNULL(@EMailSubject, '') = '' OR dbo.RemoveAllSpaces(EMailSubject) = @EMailSubject)
AND 
(
(ISNULL(@EMailBody, '') = '' OR dbo.RemoveAllSpaces(EMailBody) = @EMailBody)
OR (ISNULL(@EMailPlainTextBody, '') = '' OR dbo.RemoveAllSpaces(EMailPlainTextBody) = @EMailPlainTextBody)
)
ORDER BY CreatedOn DESC
END;
GO



CREATE PROCEDURE [dbo].[Load Assigned Application EMail Data]
	@CustomerID NVARCHAR(50),
	@ApplicationID INT,
	@eMailDate DATETIME
	
AS

BEGIN
SET NOCOUNT ON

SELECT TOP (1)   
          ID ,
					Customer_ID ,
          EMailSubject ,
          EMailUidl ,
          ApplicationID ,
		ISNULL( (SELECT TOP (1) EmployeeID FROM [applicant].dbo.tbl_application WHERE id = ISNULL(ApplicationID, 0) ORDER BY ID), 0) EmployeeID ,
          EMailFrom ,
          EMailDate ,
          EMLFileName ,
          CreatedOn
FROM  dbo.tbl_Received_EMail
WHERE Customer_ID = @CustomerID
AND (@eMailDate IS NULL OR CONVERT(NVARCHAR(10), EMailDate, 104) = CONVERT(NVARCHAR(10), @eMailDate, 104))
AND (ISNULL(@ApplicationID, 0) = 0 OR ApplicationID = @ApplicationID)
ORDER BY EMailDate DESC
END;
GO


/* ------------------- end of query ---------------------------------- */
