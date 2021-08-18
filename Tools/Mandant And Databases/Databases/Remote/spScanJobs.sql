
USE [master]
GO

CREATE DATABASE [spScanJobs]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'spScanJobs', FILENAME = N'<your path>\spScanJobs.mdf' , SIZE = 6863872KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'spScanJobs_log', FILENAME = N'<your path>\spScanJobs_log.ldf' , SIZE = 84416KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO

USE [spScanJobs]
GO


CREATE TABLE [dbo].[tbl_Attachment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](255) NULL,
	[BusinessBranchNumber] [int] NULL,
	[ModulNumber] [int] NULL,
	[DocumentCategoryNumber] [int] NULL,
	[ScanContent] [varbinary](max) NULL,
	[ImportedFileGuid] [nvarchar](50) NULL,
	[CreatedOn] [smalldatetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
	[CheckedOn] [smalldatetime] NULL,
	[CheckedFrom] [nvarchar](255) NULL,
 CONSTRAINT [PK_tbl_Attachment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_ScannedFile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](255) NULL,
	[BusinessBranchNumber] [int] NULL,
	[FoundedCodeValue] [nvarchar](255) NULL,
	[ModulNumber] [int] NULL,
	[RecordNumber] [int] NULL,
	[DocumentCategoryNumber] [int] NULL,
	[IsValid] [tinyint] NULL,
	[ScanContent] [varbinary](max) NULL,
	[ImportedFileGuid] [nvarchar](50) NULL,
	[CreatedOn] [smalldatetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
	[CheckedOn] [smalldatetime] NULL,
	[CheckedFrom] [nvarchar](255) NULL,
 CONSTRAINT [PK_tbl_ScannedFile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_ScannedReport](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](255) NULL,
	[ImportedFileGuid] [nvarchar](50) NULL,
	[FoundedCodeValue] [nvarchar](255) NULL,
	[RecordNumber] [int] NULL,
	[ReportYear] [int] NULL,
	[ReportMonth] [int] NULL,
	[ReportWeek] [int] NULL,
	[ReportFirstDay] [int] NULL,
	[ReportLastDay] [int] NULL,
	[IsValid] [tinyint] NULL,
	[ScanContent] [varbinary](max) NULL,
	[CreatedOn] [smalldatetime] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
	[CheckedOn] [smalldatetime] NULL,
	[CheckedFrom] [nvarchar](255) NULL,
	[ReportLineID] [int] NULL,
 CONSTRAINT [PK_tbl_ScannedReport] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_ScanDropIn](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[BusinessBranch] [NVARCHAR](255) NULL,
	[ModulNumber] [INT] NULL,
	[DocumentCategoryNumber] [INT] NULL,
	[ScanContent] [VARBINARY](MAX) NULL,
	[Extension] [NVARCHAR](10) NULL,
	[CreatedOn] [SMALLDATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
	[CheckedOn] [SMALLDATETIME] NULL,
	[CheckedFrom] [NVARCHAR](255) NULL,
	[ScanFileName] [NVARCHAR](255) NULL,
 CONSTRAINT [PK_tbl_ScanDropIn] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Setting](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](255) NULL,
	[Customer_Name] [NVARCHAR](70) NULL,
	[Recipients] [NVARCHAR](255) NULL,
	[Report_Recipients] [NVARCHAR](255) NULL,
	[bccAddresses] [NVARCHAR](255) NULL,
	[MailSender] [NVARCHAR](255) NULL,
	[MailUserName] [NVARCHAR](255) NULL,
	[MailPassword] [NVARCHAR](255) NULL,
	[SmtpServer] [NVARCHAR](255) NULL,
	[SmtpPort] [INT] NULL,
	[ActivateSSL] [BIT] NULL,
	[TemplateFolder] [NVARCHAR](255) NULL,
 CONSTRAINT [PK_tbl_Setting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/* ---------------------- end of creating tables -------------------------------- */

USE [spScanJobs]
GO

USE [spScanJobs]
GO


Create PROCEDURE [dbo].[Get Notifications]
	@CustomerID NVARCHAR(50),
	@excludeChecked BIT
AS

BEGIN
SET NOCOUNT ON

SELECT ID, Customer_ID, NotifyHeader, NotifyComments, NotifyArt, CreatedOn, CreatedFrom, CheckedOn, CheckedFrom FROM dbo.tbl_Notify
	WHERE Customer_ID = @CustomerID
	AND (@excludeChecked IS NULL OR CheckedOn IS NOT NULL)
	ORDER BY CreatedOn DESC, CheckedOn

END
GO


CREATE PROCEDURE [dbo].[Get Scanjobs Notifications]
	@CustomerID NVARCHAR(50),
	@excludeChecked INT
AS

BEGIN
SET NOCOUNT ON

SELECT ID, Customer_ID, ModulNumber, DocumentCategoryNumber, CONVERT(INT, IsValid) IsValid, 
	FoundedCodeValue, ImportedFileGuid,
	CreatedOn, CheckedOn
	FROM dbo.tbl_ScannedFile
	WHERE Customer_ID = @CustomerID
	--AND ScanContent IS NOT Null
	AND CheckedOn IS NULL
	ORDER BY CreatedOn DESC

END

GO


CREATE PROCEDURE [dbo].[Get Assigned ScanJobs]
	@CustomerID NVARCHAR(50),
	@ScanID NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT Scan.ID, Scan.Customer_ID, Scan.FoundedCodeValue, Scan.ModulNumber, Scan.RecordNumber, Scan.DocumentCategoryNumber, CONVERT(INT, Scan.IsValid) IsValid, 
	Scan.ImportedFileGuid, Scan.CreatedOn, Scan.CreatedFrom, Scan.CheckedOn, Scan.CheckedFrom,
	ISNULL( (SELECT TOP 1 A.ScanContent FROM dbo.tbl_Attachment A WHERE A.Customer_ID = @CustomerID AND A.ImportedFileGuid = @ScanID AND A.CheckedOn IS NULL), NULL) ScanContent, 
	Report.[ReportYear],
	Report.[ReportMonth] ,
	Report.[ReportWeek] ,
	Report.[ReportFirstDay] ,
	Report.[ReportLastDay] ,
	Report.[ReportLineID]

	FROM dbo.tbl_ScannedFile Scan
	Left Join [tbl_ScannedReport] Report On Scan.Customer_ID = Report.Customer_ID And Scan.ImportedFileGuid = Report.ImportedFileGuid 
	WHERE Scan.Customer_ID = @CustomerID
	And Scan.ImportedFileGuid = @ScanID
	AND Scan.CheckedOn IS NULL
	ORDER BY Scan.CreatedOn DESC

END

GO


CREATE PROCEDURE [dbo].[Update Assigned ScanJobs]
	@CustomerID NVARCHAR(50),
	@ScanID NVARCHAR(50),
	@UserData NVARCHAR(255)
AS

BEGIN
SET NOCOUNT ON

DECLARE @RecordInfo NVARCHAR(255) 
SELECT @RecordInfo = S.FoundedCodeValue FROM  dbo.tbl_ScannedFile S
	WHERE S.Customer_ID = @CustomerID
	AND S.ImportedFileGuid = @ScanID

UPDATE dbo.tbl_Attachment SET 
	CheckedOn = GETDATE(),
	CheckedFrom = @UserData
	WHERE Customer_ID = @CustomerID
	AND ImportedFileGuid = @ScanID
	AND CheckedOn IS NULL;

UPDATE dbo.tbl_ScannedFile SET 
	CheckedOn = GETDATE(),
	CheckedFrom = @UserData
	WHERE Customer_ID = @CustomerID
	AND ImportedFileGuid = @ScanID
	AND CheckedOn IS NULL;

UPDATE dbo.tbl_ScannedReport SET 
	CheckedOn = GETDATE(),
	CheckedFrom = @UserData
	WHERE Customer_ID = @CustomerID
	AND ImportedFileGuid = @ScanID
	AND CheckedOn IS NULL

INSERT INTO [spSystemInfo].dbo.tbl_Notify
        ( Customer_ID ,
          NotifyHeader ,
          NotifyComments ,
          NotifyArt ,
          CreatedOn ,
          CreatedFrom ,
          CheckedOn ,
          CheckedFrom
        )
VALUES  ( @CustomerID , -- Customer_ID - nvarchar(50)
          N'Scan-Job' , -- NotifyHeader - nvarchar(255)
          N'Dokument ' + @RecordInfo + ' wurde eingelesen.' , -- NotifyComments - nvarchar(4000)
          5 , -- NotifyArt - int
          GETDATE() , -- CreatedOn - smalldatetime
          @UserData , -- CreatedFrom - nvarchar(255)
          GETDATE() , -- CheckedOn - smalldatetime
          N'System'  -- CheckedFrom - nvarchar(255)
        )
END

GO


CREATE PROCEDURE [dbo].[Create New Report ScanJob DropIn]
	@CustomerID NVARCHAR(50),

	@BusinessBranch NVARCHAR(255) ,
    @ModulNumber INT ,
    @DocumentCategoryNumber INT ,
    @Content VARBINARY(max) ,
    @FileExtension NVARCHAR(10) ,
    @ScanFileName NVARCHAR(255) ,
	@CreatedFrom NVARCHAR(255) NULL

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN


INSERT INTO dbo.tbl_ScanDropIn
        ( Customer_ID ,
          BusinessBranch ,
          ModulNumber ,
          DocumentCategoryNumber ,
          ScanContent ,
          Extension ,
		  ScanFileName ,
          CreatedOn ,
          CreatedFrom ,
          CheckedOn ,
          CheckedFrom
        )
VALUES  ( @CustomerID , -- Customer_ID - nvarchar(255)
          @BusinessBranch , -- BusinessBranchNumber - int
          @ModulNumber , -- ModulNumber - int
          @DocumentCategoryNumber , -- DocumentCategoryNumber - int
          @Content , -- ScanContent - varbinary(max)
          @FileExtension , -- Extension - nvarchar(10)
	      @ScanFileName , -- scanfilename - nvarchar(255)
          GETDATE() , -- CreatedOn - smalldatetime
          @CreatedFrom , -- CreatedFrom - nvarchar(255)
          GETDATE() , -- CheckedOn - smalldatetime
          'System'  -- CheckedFrom - nvarchar(255)
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

END

GO


CREATE PROCEDURE [dbo].[Create New CV ScanJob DropIn]
	@CustomerID NVARCHAR(50),

	@BusinessBranch NVARCHAR(255) ,
    @ModulNumber INT ,
    @DocumentCategoryNumber INT ,
    @Content VARBINARY(max) ,
    @FileExtension NVARCHAR(10) ,
    @ScanFileName NVARCHAR(255) ,
	@CreatedFrom NVARCHAR(255) NULL

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN


INSERT INTO dbo.tbl_ScanDropIn
        ( Customer_ID ,
          BusinessBranch ,
          ModulNumber ,
          DocumentCategoryNumber ,
          ScanContent ,
          Extension ,
		  ScanFileName ,
          CreatedOn ,
          CreatedFrom ,
          CheckedOn ,
          CheckedFrom
        )
VALUES  ( @CustomerID , -- Customer_ID - nvarchar(255)
          @BusinessBranch , -- BusinessBranchNumber - int
          @ModulNumber , -- ModulNumber - int
          @DocumentCategoryNumber , -- DocumentCategoryNumber - int
          @Content , -- ScanContent - varbinary(max)
          @FileExtension , -- Extension - nvarchar(10)
	      @ScanFileName , -- scanfilename - nvarchar(255)
          GETDATE() , -- CreatedOn - smalldatetime
          @CreatedFrom , -- CreatedFrom - nvarchar(255)
          GETDATE() , -- CheckedOn - smalldatetime
          'System'  -- CheckedFrom - nvarchar(255)
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

END

GO


CREATE PROCEDURE [dbo].[Get Notifications EMail Data]
	@CustomerID NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT TOP 1 ID ,
       Customer_ID ,
       Customer_Name ,
       Recipients ,
       Report_Recipients ,
       bccAddresses ,
       MailSender ,
       MailUserName ,
       MailPassword ,
       SmtpServer ,
       SmtpPort ,
       ActivateSSL ,
       TemplateFolder 
	   FROM dbo.tbl_Setting
	   WHERE Customer_ID = @CustomerID

END

GO


CREATE PROCEDURE [dbo].[Create New Applicant Document_NOT USED]
	@Customer_ID NVARCHAR(50),

    @ApplicationNumber INT ,
    @ApplicantNumber INT ,
    @Type INT ,
    @Flag INT ,
    @Title NVARCHAR(255) ,
    @Content VARBINARY(max) ,
    @TrXMLResult VARBINARY(max) ,
    @TrXMLID INT ,
    @TrXMLCreatedOn Datetime ,

	@NewId int OUTPUT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		
INSERT INTO dbo.tbl_application_Documnt
        ( Customer_ID ,
          ApplicationNumber ,
          ApplicantNumber ,
          [Type] ,
          Flag ,
          Title ,
          Content ,
          TrXMLResult ,
          TrXMLID ,
          TrXMLCreatedOn ,
          CreatedOn ,
          CreatedFrom
        )
VALUES  ( @Customer_ID , -- Customer_ID - nvarchar(50)
          @ApplicationNumber , -- ApplicationNumber - int
          @ApplicantNumber , -- ApplicantNumber - int
          @Type , -- Type - int
          @Flag , -- Flag - int
          @Title , -- Title - nvarchar(50)
          @Content , -- Content - varbinary(max)
          @TrXMLResult , -- TrXMLResult - varbinary(max)
          @TrXMLID , -- TrXMLID - int
          @TrXMLCreatedOn , -- TrXMLCreatedOn - datetime
          GETDATE() , -- CreatedOn - datetime
          N'System'  -- CreatedFrom - nvarchar(255)
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


CREATE PROCEDURE [dbo].[Get Customer Data For ScanJobs]

AS

BEGIN
SET NOCOUNT ON

SELECT Customer_ID FROM [spScanJobs].dbo.tbl_ScannedFile
	WHERE (ISNULL(Customer_ID, '') <> '')
	AND (CheckedOn IS NULL)
	AND IsValid = 1
	GROUP BY Customer_ID
	ORDER BY Customer_ID DESC

END
GO


CREATE PROCEDURE [dbo].[Load DropIn Data For Creating Application]
	@CustomerID NVARCHAR(50),

	@dropInFilename NVARCHAR(255)

AS

BEGIN
SET NOCOUNT ON


SELECT TOP 1 Customer_ID ,
          BusinessBranch ,
          ModulNumber ,
          DocumentCategoryNumber ,
          ScanContent ,
          Extension ,
		  ScanFileName ,
          CreatedOn ,
          CreatedFrom ,
          CheckedOn ,
          CheckedFrom
       FROM dbo.tbl_ScanDropIn
	   WHERE Customer_ID = @CustomerID
	   AND ScanFileName = @dropInFilename
						
END
GO


CREATE PROCEDURE [dbo].[Load ALL Scanjob Data]
	@CustomerID NVARCHAR(50) ,
	@ScanID INT ,
	@assignedDate DATETIME
AS

BEGIN
    SET NOCOUNT ON;

    SELECT  S.ID,
            S.Customer_ID,
						ISNULL((SELECT TOP (1) Customer_Name FROM spScanJobs.dbo.tbl_Setting setting WHERE setting.Customer_ID = S.Customer_ID ORDER BY setting.Customer_ID), '') CustomerName,
            S.BusinessBranchNumber,
            S.ModulNumber,
            S.DocumentCategoryNumber,
						(CASE 
WHEN ISNULL(@ScanID, 0) = 0 THEN NULL
ELSE	S.ScanContent
END)
ScanContent ,
            S.ImportedFileGuid,
            S.CreatedOn,
            S.CreatedFrom,
            S.CheckedOn,
            S.CheckedFrom 
			FROM spScanJobs.dbo.tbl_Attachment S
    
				WHERE (@assignedDate IS NULL OR CONVERT(NVARCHAR(10), S.CreatedOn, 104) = CONVERT(NVARCHAR(10), @assignedDate, 104))
				AND (ISNULL(@CustomerID, '') = '' OR S.Customer_ID = @CustomerID)
			ORDER BY S.CreatedOn DESC;

END;
GO


CREATE PROCEDURE [dbo].[Load Assigned Scanjob Data]
	@CustomerID NVARCHAR(50) ,
	@ScanID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  S.ID,
            S.Customer_ID,
						ISNULL((SELECT TOP (1) Customer_Name FROM spScanJobs.dbo.tbl_Setting setting WHERE setting.Customer_ID = S.Customer_ID ORDER BY setting.Customer_ID), '') CustomerName,
            S.BusinessBranchNumber,
            S.ModulNumber,
            S.DocumentCategoryNumber,
						S.ScanContent,
            S.ImportedFileGuid,
            S.CreatedOn,
            S.CreatedFrom,
            S.CheckedOn,
            S.CheckedFrom 
			FROM spScanJobs.dbo.tbl_Attachment S
    
				WHERE (ISNULL(@CustomerID, '') = '' OR S.Customer_ID = @CustomerID)
				AND S.ID = @ScanID
			ORDER BY S.CreatedOn DESC;

END;
GO

/* ------------------ end of creating sp --------------------------------------- */

/* ------------------ end of query --------------------------------------------- */

