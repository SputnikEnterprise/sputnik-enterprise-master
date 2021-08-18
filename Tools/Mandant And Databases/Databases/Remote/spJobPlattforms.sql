
USE [master]
GO

CREATE DATABASE [spJobPlattforms]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'spJobPlattforms', FILENAME = N'<your path>\spJobPlattforms.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'spJobPlattforms_log', FILENAME = N'<your path>\spJobPlattforms_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO


USE [spJobPlattforms]
GO

CREATE TABLE [dbo].[tbl_AVAMAdvertisement](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](50) NULL,
	[User_ID] [NVARCHAR](50) NULL,
	[JobroomID] [NVARCHAR](50) NULL,
	[VacancyNumber] [INT] NULL,
	[QueryContent] [NVARCHAR](MAX) NULL,
	[Notify] [BIT] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
	[CreatedOn] [DATETIME] NULL,
 CONSTRAINT [tbl_AVAMAdvertisement_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_AVAMQueryResult](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](50) NULL,
	[User_ID] [NVARCHAR](50) NULL,
	[Advertisement_ID] [INT] NOT NULL,
	[JobroomID] [NVARCHAR](50) NULL,
	[ResultContent] [NVARCHAR](MAX) NULL,
	[ReportingObligation] [BIT] NULL,
	[ReportingObligationEndDate] [DATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
	[CreatedOn] [DATETIME] NULL,
	[QueryContent] [NVARCHAR](MAX) NULL,
 CONSTRAINT [tbl_AVAMQueryResult_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/* ------------------- end of creating tables ----------------------------------- */


USE [spJobPlattforms]
GO

CREATE PROCEDURE [dbo].[Load Assigned AVAM Query Result]
	@Customer_ID NVARCHAR(50),
	@JobroomID NVARCHAR(50)

AS

BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

		SELECT ID,
               Customer_ID,
               User_ID,
               Advertisement_ID,
               JobroomID,
               ResultContent,
               CreatedFrom,
               CreatedOn
			   FROM dbo.[tbl_AVAMQueryResult]
			WHERE Customer_ID = @Customer_ID AND JobroomID = @JobroomID 


    END TRY
    BEGIN CATCH
            ROLLBACK TRAN;
            DECLARE @message NVARCHAR(MAX);
            DECLARE @state INT;
            SELECT @message = ERROR_MESSAGE(),
                   @state = ERROR_STATE();
            RAISERROR(@message, 11, @state);

    END CATCH;

END;

GO

CREATE PROCEDURE [dbo].[Add AVAM Advertisment Query Data]
	@Customer_ID NVARCHAR(50),
	@User_ID NVARCHAR(50),
	@JobroomID NVARCHAR(50),
    @VacancyNumber INT,
    @QueryContent NVARCHAR(max) ,
    @ResultContent NVARCHAR(max) ,
    @ReportingObligation BIT ,
    @ReportingObligationEndDate DATETIME ,
    @Notify BIT ,
	@CreatedFrom NVARCHAR(255),

	@NewId int OUTPUT

AS

BEGIN
SET NOCOUNT ON

DECLARE @StartTranCount INT;

BEGIN TRY
    SET @StartTranCount = @@TRANCOUNT;
    IF @StartTranCount = 0
        BEGIN TRAN;

    INSERT INTO dbo.tbl_AVAMAdvertisement
    (
        Customer_ID,
        User_ID,
        JobroomID,
        VacancyNumber,
        QueryContent,
        Notify,
        CreatedFrom,
        CreatedOn
    )
    VALUES
    (   @Customer_ID,   -- Customer_ID - nvarchar(50)
        @User_ID,       -- User_ID - nvarchar(50)
        @JobroomID,       -- JobroomID - nvarchar(50)
        @VacancyNumber, -- VacancyNumber - int
        @QueryContent,  -- QueryContent - nvarchar(max)
        @Notify,        -- Notify - bit
        @CreatedFrom,   -- CreatedFrom - nvarchar(255)
        GETDATE()       -- CreatedOn - datetime
        );

    SET @NewId = SCOPE_IDENTITY();

	INSERT INTO dbo.tbl_AVAMQueryResult
	(
        Customer_ID,
        User_ID,
        JobroomID,
	    Advertisement_ID,
	    ResultContent,
	    ReportingObligation,
	    ReportingObligationEndDate,
	    CreatedFrom,
	    CreatedOn
	)
	VALUES
    (   @Customer_ID,   -- Customer_ID - nvarchar(50)
        @User_ID,       -- User_ID - nvarchar(50)
        @JobroomID,       -- JobroomID - nvarchar(50)
		@NewId,        -- Advertisement_ID - int
	    @ResultContent,      -- ResultContent - nvarchar(max)
	    @ReportingObligation,
	    @ReportingObligationEndDate,
	    @CreatedFrom,      -- CreatedFrom - nvarchar(255)
	    GETDATE() -- CreatedOn - datetime
	    )

    SET @NewId = SCOPE_IDENTITY();


    IF @StartTranCount = 0
        COMMIT TRAN;

END TRY
BEGIN CATCH
    IF @StartTranCount = 0
       AND @@trancount > 0
    BEGIN
        ROLLBACK TRAN;
        DECLARE @message NVARCHAR(MAX);
        DECLARE @state INT;
        SELECT @message = ERROR_MESSAGE(),
               @state = ERROR_STATE();
        RAISERROR(@message, 11, @state);
    END;

END CATCH;

END
GO


CREATE PROCEDURE [dbo].[Add AVAM Query Result]
	@Customer_ID NVARCHAR(50),
	@User_ID NVARCHAR(50),
	@JobroomID NVARCHAR(50),
    @ResultContent NVARCHAR(max) ,
    @ReportingObligation BIT ,
    @ReportingObligationEndDate DATETIME ,
	@CreatedFrom NVARCHAR(255),

	@NewId INT  OUTPUT

AS

BEGIN
    SET NOCOUNT ON;

    DECLARE @StartTranCount INT;


    BEGIN TRY
        SET @StartTranCount = @@TRANCOUNT;
        IF @StartTranCount = 0
            BEGIN TRAN;
    DECLARE @AdvertismentID INT = ISNULL(( SELECT TOP (1) ID FROM dbo.tbl_AVAMAdvertisement WHERE JobroomID = @JobroomID), 0)
	DELETE dbo.[tbl_AVAMQueryResult] WHERE JobroomID = @JobroomID;

		INSERT INTO dbo.[tbl_AVAMQueryResult]
		(
			Customer_ID,
			User_ID,
			JobroomID,
			Advertisement_ID,
			ResultContent,
			ReportingObligation,
			ReportingObligationEndDate,
			CreatedFrom,
			CreatedOn
		)
		VALUES
		(@Customer_ID,
		 @User_ID,
		 @JobroomID,
		 @AdvertismentID,
		 @ResultContent,
		 @ReportingObligation,
		 @ReportingObligationEndDate,
		 @CreatedFrom,
		 GETDATE()
		);

		SET @NewId = SCOPE_IDENTITY();

        IF @StartTranCount = 0
            COMMIT TRAN;

    END TRY
    BEGIN CATCH
        IF @StartTranCount = 0
           AND @@trancount > 0
        BEGIN
            ROLLBACK TRAN;
            DECLARE @message NVARCHAR(MAX);
            DECLARE @state INT;
            SELECT @message = ERROR_MESSAGE(),
                   @state = ERROR_STATE();
            RAISERROR(@message, 11, @state);
        END;

    END CATCH;

END;

GO


CREATE PROCEDURE [dbo].[Update AVAM Query Result]
	@Customer_ID NVARCHAR(50),
	@User_ID NVARCHAR(50),
	@JobroomID NVARCHAR(50),
    @ResultContent NVARCHAR(max) ,
    @ReportingObligation BIT ,
    @ReportingObligationEndDate DATETIME ,
	@CreatedFrom NVARCHAR(255)

AS

BEGIN
    SET NOCOUNT ON;

    DECLARE @StartTranCount INT;


    BEGIN TRY
        SET @StartTranCount = @@TRANCOUNT;
        IF @StartTranCount = 0
            BEGIN TRAN;
    DECLARE @AdvertismentID INT = ISNULL(( SELECT TOP (1) ID FROM dbo.tbl_AVAMAdvertisement WHERE JobroomID = @JobroomID), 0)

		UPDATE dbo.[tbl_AVAMQueryResult]
		SET 
			User_ID = @User_ID,
			Advertisement_ID = @AdvertismentID,
			ResultContent = @ResultContent,
			ReportingObligation = @ReportingObligation,
			ReportingObligationEndDate = @ReportingObligationEndDate,
			CreatedFrom = @CreatedFrom,
			CreatedOn = GETDATE()
			WHERE Customer_ID = @Customer_ID AND JobroomID = @JobroomID 



        IF @StartTranCount = 0
            COMMIT TRAN;

    END TRY
    BEGIN CATCH
        IF @StartTranCount = 0
           AND @@trancount > 0
        BEGIN
            ROLLBACK TRAN;
            DECLARE @message NVARCHAR(MAX);
            DECLARE @state INT;
            SELECT @message = ERROR_MESSAGE(),
                   @state = ERROR_STATE();
            RAISERROR(@message, 11, @state);
        END;

    END CATCH;

END;

GO

CREATE PROCEDURE [dbo].[Add AVAM Query Result Data]
	@Customer_ID NVARCHAR(50),
	@User_ID NVARCHAR(50),
	@JobroomID NVARCHAR(50),
    @QueryContent NVARCHAR(MAX) ,
    @ResultContent NVARCHAR(MAX) ,
    @ReportingObligation BIT ,
    @ReportingObligationEndDate DATETIME ,
	@CreatedFrom NVARCHAR(255),

	@NewId INT  OUTPUT

AS

BEGIN
    SET NOCOUNT ON;

    DECLARE @StartTranCount INT;


    BEGIN TRY
        SET @StartTranCount = @@TRANCOUNT;
        IF @StartTranCount = 0
            BEGIN TRAN;
    DECLARE @AdvertismentID INT = ISNULL(( SELECT TOP (1) ID FROM dbo.tbl_AVAMAdvertisement WHERE JobroomID = @JobroomID), 0)

		INSERT INTO dbo.[tbl_AVAMQueryResult]
		(
			Customer_ID,
			User_ID,
			JobroomID,
			Advertisement_ID,
			QueryContent,
			ResultContent,
			ReportingObligation,
			ReportingObligationEndDate,
			CreatedFrom,
			CreatedOn
		)
		VALUES
		(@Customer_ID,
		 @User_ID,
		 @JobroomID,
		 @AdvertismentID,
		 @QueryContent,
		 @ResultContent,
		 @ReportingObligation,
		 @ReportingObligationEndDate,
		 @CreatedFrom,
		 GETDATE()
		);

		SET @NewId = SCOPE_IDENTITY();

        IF @StartTranCount = 0
            COMMIT TRAN;

    END TRY
    BEGIN CATCH
        IF @StartTranCount = 0
           AND @@trancount > 0
        BEGIN
            ROLLBACK TRAN;
            DECLARE @message NVARCHAR(MAX);
            DECLARE @state INT;
            SELECT @message = ERROR_MESSAGE(),
                   @state = ERROR_STATE();
            RAISERROR(@message, 11, @state);
        END;

    END CATCH;

END;

GO

/* -------------------- end of creating sp ----------------------------- */

/* -------------------- end of query --------------------------------*/

