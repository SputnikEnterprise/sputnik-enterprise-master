
USE [master]
GO

CREATE DATABASE [applicant]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'applicant', FILENAME = N'<your path>\applicant.mdf' , SIZE = 32192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'applicant_log', FILENAME = N'<your path>\applicant_log.ldf' , SIZE = 504KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO

USE [applicant]
GO


CREATE TABLE [dbo].[tbl_applicant](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](50) NULL,
	[CVLProfileID] [INT] NULL,
	[EmployeeID] [INT] NULL,
	[Lastname] [NVARCHAR](255) NULL,
	[Firstname] [NVARCHAR](255) NULL,
	[Gender] [NVARCHAR](1) NULL,
	[Street] [NVARCHAR](255) NULL,
	[PostOfficeBox] [NVARCHAR](255) NULL,
	[Postcode] [NVARCHAR](255) NULL,
	[Location] [NVARCHAR](255) NULL,
	[Country] [NVARCHAR](255) NULL,
	[Nationality] [NVARCHAR](255) NULL,
	[EMail] [NVARCHAR](255) NULL,
	[Telephone] [NVARCHAR](255) NULL,
	[MobilePhone] [NVARCHAR](255) NULL,
	[Birthdate] [SMALLDATETIME] NULL,
	[Permission] [NVARCHAR](255) NULL,
	[Profession] [NVARCHAR](255) NULL,
	[Auto] [BIT] NULL,
	[Motorcycle] [BIT] NULL,
	[Bicycle] [BIT] NULL,
	[DrivingLicence1] [NVARCHAR](255) NULL,
	[DrivingLicence2] [NVARCHAR](255) NULL,
	[DrivingLicence3] [NVARCHAR](255) NULL,
	[CivilState] [INT] NULL,
	[Language] [NVARCHAR](255) NULL,
	[LanguageLevel] [INT] NULL,
	[CreatedOn] [DATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
	[ApplicantLifecycle] [INT] NULL,
	[CheckedOn] [SMALLDATETIME] NULL,
	[CheckedFrom] [NVARCHAR](255) NULL,
	[Latitude] [FLOAT] NULL,
	[Longitude] [FLOAT] NULL,
 CONSTRAINT [PK_tbl_applicant_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_applicant_Document](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[FK_ApplicantID] [INT] NULL,
	[Type] [INT] NULL,
	[Flag] [INT] NULL,
	[Title] [NVARCHAR](255) NULL,
	[FileExtension] [NVARCHAR](10) NULL,
	[Content] [VARBINARY](MAX) NULL,
	[Hashvalue] [NVARCHAR](255) NULL,
	[CreatedOn] [DATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
	[CheckedOn] [SMALLDATETIME] NULL,
	[CheckedFrom] [NVARCHAR](255) NULL,
 CONSTRAINT [PK_tbl_applicant_Document_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_applicant_Document]  WITH CHECK ADD  CONSTRAINT [tbl_applicant_Document_FK_ApplicantID] FOREIGN KEY([FK_ApplicantID])
REFERENCES [dbo].[tbl_applicant] ([ID])
GO

ALTER TABLE [dbo].[tbl_applicant_Document] CHECK CONSTRAINT [tbl_applicant_Document_FK_ApplicantID]
GO


CREATE TABLE [dbo].[tbl_application](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](50) NULL,
	[ApplicationLabel] [NVARCHAR](255) NULL,
	[FK_ApplicantID] [INT] NULL,
	[EmployeeID] [INT] NULL,
	[VacancyNumber] [INT] NULL,
	[Advisor] [NVARCHAR](255) NULL,
	[BusinessBranch] [NVARCHAR](255) NULL,
	[Dismissalperiod] [NVARCHAR](255) NULL,
	[Availability] [NVARCHAR](255) NULL,
	[Comment] [NVARCHAR](4000) NULL,
	[CreatedOn] [DATETIME] NULL,
	[CreatedFrom] [NVARCHAR](255) NULL,
	[CheckedOn] [SMALLDATETIME] NULL,
	[CheckedFrom] [NVARCHAR](255) NULL,
	[ApplicationLifecycle] [INT] NULL,
 CONSTRAINT [PK_tbl_application_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_application]  WITH CHECK ADD  CONSTRAINT [tbl_application_FK_ApplicantID] FOREIGN KEY([FK_ApplicantID])
REFERENCES [dbo].[tbl_applicant] ([ID])
GO

ALTER TABLE [dbo].[tbl_application] CHECK CONSTRAINT [tbl_application_FK_ApplicantID]
GO


CREATE TABLE [dbo].[tbl_FirstnamesWithGender](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](50) NOT NULL,
	[Firstname] [NVARCHAR](255) NULL,
	[Gender] [NVARCHAR](1) NULL,
	[CreatedOn] [DATETIME] NOT NULL,
	[CreatedFrom] [NVARCHAR](255) NOT NULL,
 CONSTRAINT [tbl_FirstnamesWithGender_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tbl_Lastnames](
	[ID] [INT] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [NVARCHAR](50) NOT NULL,
	[Lastname] [NVARCHAR](255) NULL,
	[CreatedOn] [DATETIME] NOT NULL,
	[CreatedFrom] [NVARCHAR](255) NOT NULL,
 CONSTRAINT [tbl_Lastnames_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



/* ------------ create sp ---------------------------*/

USE [applicant]
GO


CREATE PROCEDURE [dbo].[DeleteCvProfile]
	@ID [int] = NULL
	,@TrxmlID [int] = NULL
	,@DeleteRelated [bit] = 0
AS

BEGIN
	SET NOCOUNT ON
	DECLARE
		@Success bit = 0
		,@StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		IF (@ID IS NULL AND @TrxmlID IS NOT NULL) BEGIN
			SELECT @ID = ID FROM tbl_CvProfile WHERE TrxmlID = @TrxmlID
		END

		IF (@ID IS NOT NULL) BEGIN
			DECLARE
				@ID_CvPersonal int
				,@ID_CvDocumentText int
				,@ID_CvDocumentHtml int
			SELECT
				@ID_CvPersonal = FK_CvPersonal
				,@ID_CvDocumentText = FK_CvDocumentText
				,@ID_CvDocumentHtml = FK_CvDocumentHtml
			FROM
				tbl_CvProfile
			WHERE
				ID = @ID
			IF (@DeleteRelated = 1) BEGIN
				DECLARE @ID_CvSkill int
				SELECT @ID_CvSkill = ID FROM tbl_CvSkill WHERE FK_CvProfile = @ID
				IF (@ID_CvSkill IS NOT NULL) BEGIN
					DELETE FROM tbl_CvComputerSkill WHERE FK_CvSkill = @ID_CvSkill
					DELETE FROM tbl_CvLanguageSkill WHERE FK_CvSkill = @ID_CvSkill
					DELETE FROM tbl_CvSoftSkill WHERE FK_CvSkill = @ID_CvSkill
					DELETE FROM tbl_CvSkill WHERE ID = @ID_CvSkill
				END
				DELETE FROM tbl_CvEducationHistory WHERE FK_CvProfile = @ID
				DELETE FROM tbl_CvEmploymentHistory WHERE FK_CvProfile = @ID
				DECLARE @ID_CvOther int
				SELECT @ID_CvOther = ID FROM tbl_CvOther WHERE FK_CvProfile = @ID
				IF (@ID_CvOther IS NOT NULL) BEGIN
					DELETE FROM tbl_CvHobby WHERE FK_CvOther = @ID_CvOther
					DELETE FROM tbl_CvReference WHERE FK_CvOther = @ID_CvOther
					DELETE FROM tbl_CvOther WHERE ID = @ID_CvOther
				END
				DECLARE @ID_CvCustomArea int
				DECLARE @ID_CvPicture int
				SELECT @ID_CvCustomArea = ID, @ID_CvPicture = FK_CvProfilePicture FROM tbl_CvCustomArea WHERE FK_CvProfile = @ID
				IF (@ID_CvCustomArea IS NOT NULL) BEGIN
					DELETE FROM tbl_CvExtraInfo WHERE FK_CvCustomArea = @ID_CvCustomArea
					DELETE FROM tbl_CvTransportation WHERE FK_CvCustomArea = @ID_CvCustomArea
					DELETE FROM tbl_CvCustomArea WHERE ID = @ID_CvCustomArea
					IF (@ID_CvPicture IS NOT NULL) BEGIN
						DELETE FROM tbl_CvPicture WHERE ID = @ID_CvPicture
					END
				END
			END
			DELETE FROM tbl_CvProfile WHERE ID = @ID
			IF (@DeleteRelated = 1) BEGIN
				IF (@ID_CvDocumentText IS NOT NULL) BEGIN
					DELETE FROM tbl_CvDocumentText WHERE ID = @ID_CvDocumentText
				END 
				IF (@ID_CvDocumentHtml IS NOT NULL) BEGIN
					DELETE FROM tbl_CvDocumentHtml WHERE ID = @ID_CvDocumentHtml
				END 
				IF (@ID_CvPersonal IS NOT NULL) BEGIN 
					DECLARE @ID_CvAddress int
					SELECT @ID_CvAddress = FK_CvAddress FROM tbl_CvPersonal WHERE ID = @ID_CvPersonal
					DELETE FROM tbl_CvPhoneNumber WHERE FK_CvPersonal = @ID_CvPersonal
					DELETE FROM tbl_CvEmail WHERE FK_CvPersonal = @ID_CvPersonal
					DELETE FROM tbl_CvSocialMedia WHERE FK_CvPersonal = @ID_CvPersonal
					DELETE FROM tbl_CvPersonal WHERE ID = @ID_CvPersonal
					IF (@ID_CvAddress IS NOT NULL) BEGIN
						DELETE FROM tbl_CvAddress WHERE ID = @ID_CvAddress
					END
				END
			END
			SET @Success = 1
		END
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
	RETURN @Success
END
GO


CREATE PROCEDURE [dbo].[Update Applicant With CVLData]
	@Customer_ID NVARCHAR(50),
    @ApplicantID INT,
    @ApplicationID INT,
    @ProfileID INT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

	DECLARE @AppLastName NVARCHAR(255)
	DECLARE @AppFirstName NVARCHAR(255)

	DECLARE @CVLPersID INT 
	DECLARE @LastName NVARCHAR(255)
	DECLARE @FirstName NVARCHAR(255)
	DECLARE @Gender NVARCHAR(1)
	DECLARE @EMail NVARCHAR(255)
	DECLARE @BirthDate DATETIME

	DECLARE @mobileNumber NVARCHAR(50) = ''
	DECLARE @phoneNumber NVARCHAR(50) = ''
	DECLARE @formatedNumber NVARCHAR(50) = ''

SELECT  @AppLastName = ISNULL(app.Lastname, '') ,
        @AppFirstName = ISNULL(app.Firstname, '') ,
		@EMail = ISNULL(app.EMail, '') ,
		@BirthDate = app.Birthdate,
		@phoneNumber = app.Telephone, 
		@mobileNumber = app.MobilePhone
FROM    dbo.tbl_applicant app
WHERE   app.Customer_ID = @Customer_ID
        AND app.ID = @ApplicantID;


SELECT  @CVLPersID = cvlPers.ID ,
        @LastName = cvlPers.LastName ,
        @FirstName = cvlPers.FirstName ,
        @Gender = ISNULL(cvlPers.FK_GenderCode, 'm')
FROM    [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation cvlPers
        LEFT JOIN [spCVLizerBaseInfo].dbo.tbl_CVLProfile cvlProfile ON cvlProfile.ID = cvlPers.FK_CVLID
WHERE   cvlProfile.Customer_ID = @Customer_ID
        AND cvlProfile.ID = @ProfileID; 

DECLARE @cvlStreet NVARCHAR(70)
DECLARE @cvlLocation NVARCHAR(70)
DECLARE @cvlPostcode NVARCHAR(70)
DECLARE @cvlCountryCode NVARCHAR(70)

SELECT TOP 1 @cvlStreet = ISNULL(cvlPers.Street, ''), @cvlLocation = ISNULL(cvlPers.City, ''), @cvlPostcode = ISNULL(cvlPers.PostCode, ''), @cvlCountryCode = cvlPers.FK_CountryCode
		FROM     [spCVLizerBaseInfo].dbo.tbl_CVLAddress cvlPers
                          WHERE     cvlPers.FK_PersonalID = @CVLPersID


IF @AppFirstName <> @FirstName
    AND @AppLastName <> @LastName
    BEGIN
        UPDATE  dbo.tbl_applicant
        SET     Lastname = @LastName ,
                Firstname = @FirstName ,
                Gender = @Gender 
				,
                Birthdate = ( SELECT TOP 1
                                        cvlPers.DateOfBirth
                              FROM      [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation cvlPers
                              WHERE     cvlPers.ID = @CVLPersID
                            ) ,
                EMail = ( SELECT TOP 1
                                    cvlEMail.EMailAddress
                          FROM      [spCVLizerBaseInfo].dbo.tbl_CVLPersonalEMails cvlEMail
                          WHERE     cvlEMail.FK_PersonalID = @CVLPersID
                        ) ,
                Street = @cvlStreet,
                Location = @cvlLocation,
                Postcode = @cvlPostcode,
                Country = @cvlCountryCode,
                Nationality = ( SELECT TOP 1
                                        tb.FK_NationalityCode
                                FROM    [spCVLizerBaseInfo].dbo.tbl_CVLPersonalNationality tb
                                WHERE   tb.FK_PersonalID = @CVLPersID
                              )
        WHERE   dbo.tbl_applicant.ID = @ApplicantID
                AND dbo.tbl_applicant.Customer_ID = @Customer_ID;

    END;


DECLARE @appStreet NVARCHAR(70)
DECLARE @appLocation NVARCHAR(70)
DECLARE @appPostcode NVARCHAR(70)
DECLARE @appCountryCode NVARCHAR(70)

SELECT TOP 1 @appStreet = ISNULL(app.Street, ''), @appLocation = ISNULL(app.Location, ''), @appPostcode = ISNULL(app.Postcode, ''), @appCountryCode = ISNULL(app.Country, '')
	FROM     [applicant].dbo.tbl_applicant app
                    WHERE    app.Customer_ID = @Customer_ID AND app.ID = @ApplicantID 

IF @appStreet = ''
    BEGIN
		UPDATE  applicant.dbo.tbl_applicant
			SET     Street = @cvlStreet
			WHERE   dbo.tbl_applicant.ID = @ApplicantID
					AND dbo.tbl_applicant.Customer_ID = @Customer_ID;
    END;

IF @appLocation = ''
    BEGIN
		UPDATE  applicant.dbo.tbl_applicant
			SET     Location = @cvlLocation
			WHERE   dbo.tbl_applicant.ID = @ApplicantID
					AND dbo.tbl_applicant.Customer_ID = @Customer_ID;
    END;

IF @appPostcode = ''
    BEGIN
		UPDATE  applicant.dbo.tbl_applicant
			SET     Postcode = @cvlPostcode
			WHERE   dbo.tbl_applicant.ID = @ApplicantID
					AND dbo.tbl_applicant.Customer_ID = @Customer_ID;
    END;

IF @appCountryCode = ''
    BEGIN
		UPDATE  applicant.dbo.tbl_applicant
			SET     Country = @cvlCountryCode
			WHERE   dbo.tbl_applicant.ID = @ApplicantID
					AND dbo.tbl_applicant.Customer_ID = @Customer_ID;
    END;


UPDATE  applicant.dbo.tbl_applicant
SET     CVLProfileID = @ProfileID
WHERE   dbo.tbl_applicant.ID = @ApplicantID
        AND dbo.tbl_applicant.Customer_ID = @Customer_ID;

IF @Gender = ''
    BEGIN
        SELECT TOP 1 @Gender = cvlPers.FK_GenderCode
		FROM     [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation cvlPers
                          WHERE     cvlPers.FirstName LIKE @FirstName

	UPDATE  applicant.dbo.tbl_applicant
        SET     Gender = @Gender
        WHERE   dbo.tbl_applicant.ID = @ApplicantID
                AND dbo.tbl_applicant.Customer_ID = @Customer_ID;
    END;


IF @EMail = ''
    BEGIN
        UPDATE  applicant.dbo.tbl_applicant
        SET     EMail = ( SELECT TOP 1
                                    cvlEMail.EMailAddress
                          FROM      [spCVLizerBaseInfo].dbo.tbl_CVLPersonalEMails cvlEMail
                          WHERE     cvlEMail.FK_PersonalID = @CVLPersID
                        )
        WHERE   dbo.tbl_applicant.ID = @ApplicantID
                AND dbo.tbl_applicant.Customer_ID = @Customer_ID;
    END;

IF @phoneNumber = ''
BEGIN
	SELECT TOP 1 @phoneNumber = REPLACE(cvlTelefon.PhoneNumber, ' ', ''),
								@formatedNumber = cvlTelefon.PhoneNumber
							  FROM      [spCVLizerBaseInfo].dbo.tbl_CVLPersonalPhoneNumbers cvlTelefon
							  WHERE     cvlTelefon.FK_PersonalID = @CVLPersID
	IF CHARINDEX('079', @phoneNumber, 0) > 0 OR CHARINDEX('078', @phoneNumber, 0) > 0 OR CHARINDEX('076', @phoneNumber, 0) > 0 OR CHARINDEX('077', @phoneNumber, 0) > 0
		OR CHARINDEX('4179', @phoneNumber, 0) > 0 OR CHARINDEX('4178', @phoneNumber, 0) > 0 OR CHARINDEX('4176', @phoneNumber, 0) > 0 OR CHARINDEX('4177', @phoneNumber, 0) > 0
		OR CHARINDEX('41(0)79', @phoneNumber, 0) > 0 OR CHARINDEX('41(0)78', @phoneNumber, 0) > 0 OR CHARINDEX('41(0)76', @phoneNumber, 0) > 0 OR CHARINDEX('41(0)77', @phoneNumber, 0) > 0
	BEGIN
		SET @mobileNumber = @formatedNumber
		SET @phoneNumber = ''
		SET @formatedNumber = ''
	END
	IF @phoneNumber <> '' 
	BEGIN
		SET @phoneNumber = @formatedNumber
	END 

    UPDATE  applicant.dbo.tbl_applicant SET 
                Telephone = @phoneNumber ,
                MobilePhone = @mobileNumber
        WHERE   dbo.tbl_applicant.ID = @ApplicantID
                AND dbo.tbl_applicant.Customer_ID = @Customer_ID;

END

IF @BirthDate IS NULL
    BEGIN
        UPDATE  applicant.dbo.tbl_applicant
        SET     Birthdate = ( SELECT TOP 1
                                        cvlPers.DateOfBirth
                              FROM      [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation cvlPers
                              WHERE     cvlPers.ID = @CVLPersID
                            )
        WHERE   dbo.tbl_applicant.ID = @ApplicantID
                AND dbo.tbl_applicant.Customer_ID = @Customer_ID;
    END;

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


CREATE PROCEDURE [dbo].[Create New Application With Applicant]
	@Customer_ID NVARCHAR(50),
    @VacancyNumber INT,
	@ApplicationLabel NVARCHAR(255),
	@BusinessBranch NVARCHAR(255),
	@Advisor NVARCHAR(255),
    @Dismissalperiod NVARCHAR(255) ,
    @Availability NVARCHAR(255) ,
    @Comment NVARCHAR(4000) ,

    @Lastname NVARCHAR(255) ,
    @Firstname NVARCHAR(255) ,
    @Gender NVARCHAR(1) ,
    @Street NVARCHAR(255) ,
    @PostOfficeBox NVARCHAR(255) ,
    @Postcode NVARCHAR(255) ,
    @Location NVARCHAR(255) ,
    @Country NVARCHAR(255) ,
    @Nationality NVARCHAR(255) ,
    @EMail NVARCHAR(255) ,
    @Telephone NVARCHAR(255) ,
    @MobilePhone NVARCHAR(255) ,
    @Birthdate Date ,
    @Permission NVARCHAR(255) ,
    @Profession NVARCHAR(255) ,
    @Auto NVARCHAR(255) ,
    @Motorcycle NVARCHAR(255) ,
    @Bicycle NVARCHAR(255) ,
    @DrivingLicence1 NVARCHAR(255) ,
    @DrivingLicence2 NVARCHAR(255) ,
    @DrivingLicence3 NVARCHAR(255) ,
    @CivilState INT ,
    @Language NVARCHAR(255) ,
    @LanguageLevel INT ,

	@NewApplicationId int OUTPUT,
	@NewApplicantId int OUTPUT

AS

BEGIN
    SET NOCOUNT ON;

    DECLARE @StartTranCount INT;

    IF ISNULL(@Country, '') = '' OR (LEN(ISNULL(@Country, '')) <= 3 AND LEN(ISNULL(@Country, '')) > 0)
		BEGIN
			SET @Country = ISNULL(@Country, '') 
		END;
    IF LEN(ISNULL(@Country, '')) > 3 
		BEGIN
			SET @Country = ISNULL((SELECT TOP (1) Code FROM spCVLizerBaseInfo.dbo.tbl_Base_ISOCountry WHERE Bez_DE = ISNULL(@Country, '') ORDER BY Code), '')
		END;

    BEGIN TRY
        SET @StartTranCount = @@TRANCOUNT;
        IF @StartTranCount = 0
            BEGIN TRAN;

        INSERT INTO [applicant].dbo.tbl_applicant
        (
            Customer_ID,
            Lastname,
            Firstname,
            Gender,
            Street,
            PostOfficeBox,
            Postcode,
            Location,
            Country,
            Nationality,
            EMail,
            Telephone,
            MobilePhone,
            Birthdate,
            Permission,
            Profession,
            [Auto],
            Motorcycle,
            Bicycle,
            DrivingLicence1,
            DrivingLicence2,
            DrivingLicence3,
            CivilState,
            [Language],
            LanguageLevel,
            CreatedOn,
            CreatedFrom
        )
        VALUES
        (   @Customer_ID,     -- Customer_ID - nvarchar(50)
            @Lastname,        -- Lastname - nvarchar(255)
            @Firstname,       -- Firstname - nvarchar(255)
            @Gender,          -- Gender - NVARCHAR(1)
            @Street,          -- Street - nvarchar(255)
            @PostOfficeBox,   -- PostOfficeBox - nvarchar(255)
            @Postcode,        -- Postcode - nvarchar(255)
            @Location,        -- Location - nvarchar(255)
            @Country,         -- Country - nvarchar(255)
            @Nationality,     -- Nationality - nvarchar(255)
            @EMail,           -- EMail - nvarchar(255)
            @Telephone,       -- Telephone - nvarchar(255)
            @MobilePhone,     -- MobilePhone - nvarchar(255)
            @Birthdate,       -- Birthdate - smalldatetime
            @Permission,      -- Permission - nvarchar(1)
            @Profession,      -- Profession - nvarchar(255)
            @Auto,            -- Auto - bit
            @Motorcycle,      -- Motorcycle - bit
            @Bicycle,         -- Bicycle - bit
            @DrivingLicence1, -- DrivingLicence1 - nvarchar(5)
            @DrivingLicence2, -- DrivingLicence2 - nvarchar(5)
            @DrivingLicence3, -- DrivingLicence3 - nvarchar(5)
            @CivilState,      -- CivilState - int
            @Language,        -- Language - nvarchar(50)
            @LanguageLevel,   -- LanguageLevel - int
            GETDATE(),        -- CreatedOn - datetime
            N'System'         -- CreatedFrom - nvarchar(255)
            );

        SET @NewApplicantId = SCOPE_IDENTITY();

        INSERT INTO [applicant].dbo.tbl_application
        (
            Customer_ID,
            FK_ApplicantID,
            VacancyNumber,
            ApplicationLabel,
            BusinessBranch,
            Advisor,
            Dismissalperiod,
            Availability,
            Comment,
            ApplicationLifecycle,
            CreatedOn,
            CreatedFrom
        )
        VALUES
        (   @Customer_ID,               -- Customer_ID - nvarchar(50)
            @NewApplicantId,            -- FK_ApplicantID - int
            @VacancyNumber,             -- VacancyNumber - int
            @ApplicationLabel,          -- ApplicationLabel - nvarchar(255)
            @BusinessBranch,            -- BusinessBranch - nvarchar(50)
            @Advisor, @Dismissalperiod, -- Dismissalperiod - nvarchar(255)
            @Availability,              -- Availability - nvarchar(255)
            @Comment,                   -- Comment - nvarchar(4000)
            0,                          -- ApplicationLifecycle int
            GETDATE(),                  -- CreatedOn - datetime
            N'System'                   -- CreatedFrom - nvarchar(255)
            );

        SET @NewApplicationId = SCOPE_IDENTITY();


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


CREATE PROCEDURE [dbo].[Create New Application]
	@Customer_ID NVARCHAR(50),
	@FK_ApplicantID INT,
	@EmployeeID INT,
    @VacancyNumber INT,
    @ApplicationLabel NVARCHAR(255) ,
	@BusinessBranch NVARCHAR(255),
	@Advisor NVARCHAR(255),
    @Dismissalperiod NVARCHAR(255) ,
    @Availability NVARCHAR(255) ,
    @Comment NVARCHAR(4000) ,

	@NewId int OUTPUT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_application
        ( Customer_ID ,
          FK_ApplicantID ,
		  EmployeeID ,
          VacancyNumber ,
          ApplicationLabel ,
		  BusinessBranch , 
          Advisor ,
          Dismissalperiod ,
          Availability ,
          Comment ,
		  ApplicationLifecycle ,
          CreatedOn ,
          CreatedFrom
        )
VALUES  ( @Customer_ID , -- Customer_ID - nvarchar(50)
          @FK_ApplicantID , -- FK_ApplicantID - int
		  @EmployeeID ,
          @VacancyNumber , -- VacancyNumber - int
          @ApplicationLabel , -- BusinessBranch - nvarchar(50)
          @BusinessBranch , -- BusinessBranch - nvarchar(50)
          @Advisor ,
          @Dismissalperiod , -- Dismissalperiod - nvarchar(255)
          @Availability , -- Availability - nvarchar(255)
          @Comment , -- Comment - nvarchar(4000)
		  0 , -- ApplicationLifecycle int
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


CREATE PROCEDURE [dbo].[Create New Applicant Document]
    @FK_ApplicantID INT ,
    @Type INT ,
    @Flag INT ,
    @Title NVARCHAR(255) ,
    @FileExtension NVARCHAR(10) ,
    @Content VARBINARY(max) ,
    @HashValue NVARCHAR(255) ,

	@NewId int OUTPUT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN


INSERT INTO dbo.tbl_applicant_Document
        ( FK_ApplicantID ,
          [Type] ,
          Flag ,
          Title ,
          FileExtension ,
          Content ,
		  hashvalue ,
          CreatedOn ,
          CreatedFrom
        )
VALUES  ( @FK_ApplicantID , -- FK_ApplicantID - int
          @Type , -- Type - int
          @Flag , -- Flag - int
          @Title , -- Title - nvarchar(50)
          @FileExtension ,
          @Content , -- Content - varbinary(max)
          @hashvalue ,
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


CREATE PROCEDURE [dbo].[List New Applicant With Applications For Notifications]
	@CustomerID NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT A.ID ,
       A.Customer_ID ,
       A.EmployeeID ,
       A.Lastname ,
       A.Firstname ,
       A.Gender ,
       A.Street ,
       A.PostOfficeBox ,
       A.Postcode ,
       A.Location ,
       A.Country ,
       A.Nationality ,
       A.EMail ,
       A.Telephone ,
       A.MobilePhone ,
       A.Birthdate ,
       A.Permission ,
       A.Profession ,
       A.Auto ,
       A.Motorcycle ,
       A.Bicycle ,
       A.DrivingLicence1 ,
       A.DrivingLicence2 ,
       A.DrivingLicence3 ,
       A.CivilState ,
       A.Language ,
       A.LanguageLevel ,
       A.ApplicantLifecycle ,
       A.CreatedOn ,
       A.CreatedFrom ,
       A.CheckedOn ,
       A.CheckedFrom ,
	   AP.ID APID ,
	   AP.ApplicationLabel ,
	   AP.VacancyNumber ,
	   AP.Advisor ,
	   AP.BusinessBranch ,
	   AP.Dismissalperiod ,
	   AP.Availability ,
	   AP.Comment ,
       AP.CreatedOn APCreatedOn,
	   AP.ApplicationLifecycle 
	FROM dbo.tbl_applicant A
	LEFT JOIN dbo.tbl_application AP ON AP.Customer_ID = A.Customer_ID AND AP.FK_ApplicantID = A.ID 
	WHERE A.Customer_ID = @CustomerID
	AND A.CheckedOn IS NULL
	ORDER BY A.CreatedOn DESC

END
GO


CREATE PROCEDURE [dbo].[List New Applicant For Notifications]
	@CustomerID NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT A.ID ,
       A.Customer_ID ,
       A.EmployeeID ,
       A.Lastname ,
       A.Firstname ,
       A.Gender ,
       A.Street ,
       A.PostOfficeBox ,
       A.Postcode ,
       A.Latitude ,
       A.Longitude ,
       A.Location ,
			(CASE 
				WHEN  LEN(ISNULL(A.Country, '') ) > 3 THEN ISNULL((SELECT TOP (1) C.Code FROM spCVLizerBaseInfo.dbo.tbl_Base_ISOCountry C WHERE Bez_DE = ISNULL(A.Country, '') ORDER BY C.Code), '')
				WHEN  LEN(ISNULL(A.Country, '') ) <= 3 AND LEN(ISNULL(A.Country, '') ) > 0 THEN A.Country
				ELSE	''
			END) Country ,
			(CASE 
				WHEN  ISNULL(A.Nationality, '') = '' THEN ISNULL((SELECT TOP (1) pn.FK_NationalityCode FROM spCVLizerBaseInfo.dbo.tbl_CVLPersonalNationality pn 
						WHERE pn.FK_PersonalID = dbo.uf_GetPersonalIDWithCVLProfileID(A.CVLProfileID) ORDER BY pn.FK_PersonalID), '')
				
				ELSE	ISNULL(A.Nationality, '')
			END) Nationality ,
       A.EMail ,
       A.Telephone ,
       A.MobilePhone ,
       CONVERT(DATE, A.Birthdate) Birthdate ,
       A.Permission ,
       A.Profession ,
       A.Auto ,
       A.Motorcycle ,
       A.Bicycle ,
       A.DrivingLicence1 ,
       A.DrivingLicence2 ,
       A.DrivingLicence3 ,
       A.CivilState ,
       A.Language ,
       A.LanguageLevel ,
       A.ApplicantLifecycle ,
       A.CreatedOn ,
       A.CreatedFrom ,
       A.CheckedOn ,
       A.CheckedFrom ,
       A.CVLProfileID
	FROM dbo.tbl_applicant A
	WHERE A.Customer_ID = @CustomerID
	AND ISNULL(A.CVLProfileID, 0) > 0
	AND IsNull(A.Firstname, '') <> '' 
	AND IsNull(A.Lastname, '') <> '' 
	AND A.CheckedOn IS NULL
	ORDER BY A.CreatedOn DESC

END;
GO


CREATE PROCEDURE [dbo].[List New Applicant Document For Notifications]
	@fk_ApplicantID NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT ID ,
       FK_ApplicantID ,
       Type ,
       Flag ,
       Title ,
       FileExtension ,
       Content ,
       CreatedOn ,
       CreatedFrom ,
       CheckedOn ,
       CheckedFrom
	FROM dbo.tbl_applicant_Document
	WHERE FK_ApplicantID = @fk_ApplicantID
	AND CheckedOn IS NULL
	ORDER BY CreatedOn DESC

END
GO


CREATE PROCEDURE [dbo].[List New Application For Notifications]
	@CustomerID NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT ID ,
       Customer_ID ,
       FK_ApplicantID ,
       EmployeeID ,
       VacancyNumber ,
       ApplicationLabel ,
       Advisor ,
       BusinessBranch ,
       Dismissalperiod ,
       Availability ,
       Comment ,
       CreatedOn ,
       CreatedFrom ,
       CheckedOn ,
       CheckedFrom ,
       ApplicationLifecycle 
	FROM dbo.tbl_application
	WHERE Customer_ID = @CustomerID
	AND CheckedOn IS NULL
	ORDER BY CreatedOn DESC

END
GO


CREATE PROCEDURE [dbo].[Update Applicant For Notifications As Checked]
	@CustomerID NVARCHAR(50),
	@recordID INT ,
	@destApplicantNumber INT ,
	@userID NVARCHAR(50) ,
	@Checked BIT ,
	@userData NVARCHAR(255) 
AS

BEGIN
SET NOCOUNT ON

DECLARE @profileID INT = ISNULL((SELECT TOP (1) CVLProfileID FROM applicant.dbo.tbl_applicant WHERE ID = @recordID ORDER BY ID DESC), 0)

UPDATE  dbo.tbl_applicant
SET     CheckedOn = GETDATE() ,
EmployeeID = @destApplicantNumber
WHERE   ID = @recordID; 

UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLProfile
SET     CheckedOn = GETDATE() 
WHERE   Customer_ID = @CustomerID 
AND ID = @profileID; 

END;
GO


CREATE PROCEDURE [dbo].[Update Document For Notifications As Checked]
	@CustomerID NVARCHAR(50),
	@recordID INT ,
	@Checked BIT 
AS

BEGIN
SET NOCOUNT ON

UPDATE  dbo.tbl_applicant_Document
SET     CheckedOn = GETDATE() 
WHERE   ID = @recordID; 

END
GO


CREATE PROCEDURE [dbo].[Update All Applicant Data As Checked]
	@CustomerID NVARCHAR(50),
	@ApplicantID INT ,
	@Checked BIT 
AS

BEGIN
SET NOCOUNT ON


UPDATE  dbo.tbl_application
SET     CheckedOn = GETDATE() 
WHERE   FK_ApplicantID = @ApplicantID;

UPDATE  dbo.tbl_applicant
SET     CheckedOn = GETDATE() 
WHERE   ID = @ApplicantID;

UPDATE  dbo.tbl_applicant_Document
SET     CheckedOn = GETDATE() 
WHERE   FK_ApplicantID = @ApplicantID; 

END
GO


CREATE PROCEDURE [dbo].[List All Applications For Assigned Applicant]
	@CustomerID NVARCHAR(50),
	@ApplicantID INT 
AS

BEGIN
SET NOCOUNT ON


SELECT ID ,
       Customer_ID ,
       FK_ApplicantID ,
       VacancyNumber ,
       Advisor ,
       BusinessBranch ,
       Dismissalperiod ,
       Availability ,
       Comment ,
       CreatedOn ,
       CreatedFrom ,
       CheckedOn ,
       CheckedFrom ,
       ApplicationLabel ,
       ApplicationLifecycle
	FROM dbo.tbl_application
	WHERE Customer_ID = @CustomerID
	AND FK_ApplicantID = @ApplicantID
	AND CheckedOn IS NULL
	ORDER BY CreatedOn DESC

END
GO


CREATE PROCEDURE [dbo].[List All Documents For Assigned Applicant]
	@CustomerID NVARCHAR(50),
	@ApplicantID INT 
AS

BEGIN
SET NOCOUNT ON


SELECT ID ,
       FK_ApplicantID ,
       Type ,
       Flag ,
       Title ,
       FileExtension ,
       Content ,
       Hashvalue ,
       CreatedOn ,
       CreatedFrom ,
       CheckedOn ,
       CheckedFrom
	FROM dbo.tbl_applicant_Document
	WHERE 
	FK_ApplicantID = @ApplicantID
	AND CheckedOn IS NULL
	ORDER BY CreatedOn DESC

END
GO


CREATE PROCEDURE [dbo].[Update Application For Notifications As Checked]
	@CustomerID NVARCHAR(50),
	@recordID INT ,
	@destApplicationNumber INT ,
	@destApplicantNumber INT ,
	@userID NVARCHAR(50) ,
	@Checked BIT ,
	@userData NVARCHAR(255) 
AS

BEGIN
SET NOCOUNT ON

UPDATE  dbo.tbl_application
SET     CheckedOn = GETDATE() ,
EmployeeID = @destApplicantNumber
WHERE   ID = @recordID; 

END
GO


CREATE PROCEDURE [dbo].[Update Application With Scan DropIn Data]
	@Customer_ID NVARCHAR(50),
    @ApplicationID INT,
	@Advisor NVARCHAR(70),
	@BusinessBranch NVARCHAR(255)

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN


UPDATE [applicant].dbo.tbl_application
SET    Advisor = @Advisor ,
       BusinessBranch = @BusinessBranch
WHERE  Customer_ID = @Customer_ID
       AND ID = @ApplicationID;


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

CREATE PROCEDURE [dbo].[Update Applicant With Existing Employee Data]
	@CustomerID NVARCHAR(50),
	@EmployeeID INT ,
	@newExistingEmployeeNumber INT
AS

BEGIN
SET NOCOUNT ON

UPDATE  dbo.tbl_applicant
SET     CheckedOn = GETDATE() ,
EmployeeID = @newExistingEmployeeNumber
WHERE   Customer_ID = @CustomerID
AND EmployeeID = @EmployeeID; 

END
GO


CREATE PROCEDURE [dbo].[Update Applicant And CVL PersonalInformation With Existing Employee Data]
	@CustomerID NVARCHAR(50),
	@CVLProfileID INT ,
	@EmployeeID INT ,

	@Lastname nvarchar(255),
	@Firstname nvarchar(255),
	@Street nvarchar(70),
	@Countrycode nvarchar(3),
	@Postcode nvarchar(10),
	@Location nvarchar(70),
	@Gender nvarchar(1),
	@Nationality nvarchar(3),
	@CivilState INT,
	@CivilStateLabel NVARCHAR(1),
	@Birthdate datetime,
	@EMail NVARCHAR(20) ,
	@Telephone NVARCHAR(20) ,
	@Canton NVARCHAR(10) ,
	@Latitude FLOAT ,
	@Longitude FLOAT

AS

BEGIN
SET NOCOUNT ON

IF YEAR(@Birthdate) <= 1900 
BEGIN
	SET @Birthdate = NULL 
END

UPDATE [applicant].dbo.tbl_applicant
SET Lastname = @Lastname,
    Firstname = @Firstname,
    Street = @Street,
    Country = @Countrycode,
    Postcode = @Postcode,
    Location = @Location,
    Gender = @Gender,
    Nationality = @Nationality,
    CivilState = @CivilState,
    Birthdate = @Birthdate,
    EMail = @EMail,
    Telephone = @Telephone,
    Latitude = @Latitude,
    Longitude = @Longitude
WHERE CVLProfileID = @CVLProfileID
      AND EmployeeID = @EmployeeID;

DECLARE @personalID INT
SET @personalID = ISNULL((SELECT TOP (1) ID FROM [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation WHERE FK_CVLID = @CVLProfileID ORDER BY ID), 0)

UPDATE [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation
SET Lastname = @Lastname,
    Firstname = @Firstname,
    DateOfBirth = @Birthdate,
	FK_GenderCode = @gender --ISNULL((SELECT TOP 1 g.code FROM [spCVLizerBaseInfo].dbo.[tbl_Base_Gender] g WHERE g.code = @gender), '') 
WHERE FK_CVLID = @CVLProfileID
	AND ID = @personalID;

UPDATE [spCVLizerBaseInfo].dbo.tbl_CVLPersonalNationality
SET FK_NationalityCode = @Nationality
WHERE FK_PersonalID = @personalID;
	

	DELETE [spCVLizerBaseInfo].dbo.tbl_CVLPersonalEMails WHERE FK_PersonalID = @personalID;
	IF ISNULL(@EMail, '') <> ''
	BEGIN
		INSERT INTO [spCVLizerBaseInfo].dbo.tbl_CVLPersonalEMails
			(FK_PersonalID, EMailAddress)
			VALUES (@personalID, @EMail);
	END

UPDATE [spCVLizerBaseInfo].dbo.tbl_CVLAddress
SET street = @Street ,
PostCode = @Postcode ,
City = @Location ,
Latitude = @Latitude ,
Longitude = @Longitude ,
FK_CountryCode = @Countrycode ,
state = @Canton
WHERE FK_PersonalID = @personalID;

IF ISNULL(@CivilStateLabel, '') = '' 
BEGIN
	DELETE FROM [spCVLizerBaseInfo].dbo.tbl_CVLPersonalCivilstate WHERE FK_PersonalID = @personalID AND FK_CivilStateCode <> 'c';
END
ELSE
BEGIN
	IF EXISTS(SELECT TOP (1) ID FROM [spCVLizerBaseInfo].dbo.tbl_CVLPersonalCivilstate WHERE FK_PersonalID = @personalID AND FK_CivilStateCode <> 'c')
	BEGIN	
		UPDATE [spCVLizerBaseInfo].dbo.tbl_CVLPersonalCivilstate
		SET FK_CivilStateCode = @CivilStateLabel
		WHERE FK_PersonalID = @personalID;
	END
	ELSE
	BEGIN	
		INSERT INTO [spCVLizerBaseInfo].dbo.tbl_CVLPersonalCivilstate
		(FK_PersonalID, FK_CivilStateCode) VALUES
		(@personalid, @CivilStateLabel);
	END

END


END;
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

DECLARE @tblProfile TABLE (PersonID INT);
DECLARE @tblPostCode TABLE (PersonID INT);

INSERT INTO @tblProfile (PersonID) SELECT ID PersonalID FROM [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation 
	WHERE FK_CVLID IN (SELECT P.ID FROM [spCVLizerBaseInfo].dbo.tbl_CVLProfile P WHERE P.Customer_ID = @CustomerID)
	AND (ISNULL(FirstName, '') <> '' AND ISNULL(LastName, '') <> '')

IF @postcode <> ''
BEGIN
	INSERT INTO @tblPostCode (PersonID) SELECT FK_PersonalID PersonalID FROM [spCVLizerBaseInfo].dbo.tbl_CVLAddress 
		WHERE FK_PersonalID IN (SELECT ID FROM @tblProfile) AND PostCode = @postcode;
	
	DELETE FROM @tblProfile WHERE PersonID NOT IN (SELECT PC.PersonID FROM @tblPostCode PC);
END


SELECT P.ID PersonID, P.FK_CVLID, P.FirstName, P.LastName, 
	Profile.CreatedOn, Profile.Customer_ID, 
	(SELECT TOP 1 AP.EmployeeID FROM [applicant].dbo.tbl_applicant AP WHERE AP.Customer_ID = @CustomerID AND AP.CVLProfileID = Profile.ID) EmployeeID,
	CA.Street, CA.PostCode, CA.City, CA.State, CA.FK_CountryCode
FROM [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation P 
LEFT JOIN [spCVLizerBaseInfo].dbo.tbl_CVLAddress CA ON CA.FK_PersonalID = P.ID	
LEFT JOIN [spCVLizerBaseInfo].dbo.tbl_CVLProfile Profile ON Profile.ID = P.FK_CVLID AND Profile.Customer_ID = @CustomerID
WHERE P.ID IN (SELECT tblP.PersonID FROM @tblProfile tblP) 
AND (ISNULL(P.FirstName, '') <> '' AND ISNULL(P.LastName, '') <> '')
ORDER BY P.LastName, P.FirstName

END;
GO


CREATE PROCEDURE [dbo].[Update Assigned Application Data]
	@CustomerID NVARCHAR(50),
	@ApplicationID INT ,
	@Advisor NVARCHAR(50) ,
	@ApplicationLabel NVARCHAR(255) ,
	@ApplicationLifecycle INT ,
	@BusinessBranch NVARCHAR(255) ,
	@Comments NVARCHAR(4000) 
AS

BEGIN
SET NOCOUNT ON

UPDATE  dbo.tbl_application
SET     Customer_ID = @CustomerID ,
Advisor = @Advisor,
ApplicationLabel = @ApplicationLabel,
ApplicationLifecycle = @ApplicationLifecycle,
BusinessBranch = @BusinessBranch
WHERE   ID = @ApplicationID; 

END
GO


CREATE PROCEDURE [dbo].[Create New CVLProfile For Not Validated CVL Data]
    @applicantID INT ,
    @applicationID INT ,

    @NewCVLId INT OUTPUT 
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN


DECLARE @customerid NVARCHAR(50) = ''
SELECT TOP (1) @customerid = customer_id FROM [applicant].dbo.tbl_applicant WHERE ID = @applicantID ORDER BY id DESC

-- insert profile
		INSERT INTO [spCVLizerBaseInfo].dbo.tbl_CVLProfile
		        ( Customer_ID ,
		          CreatedOn ,
		          CreatedFrom
		        )
		VALUES  ( @customerid , -- Customer_ID - nvarchar(50)
		          GETDATE() , -- CreatedOn - datetime
		          N'System'  -- CreatedFrom - nvarchar(255)
		        ) 
				
		SET @NewCVLId = SCOPE_IDENTITY() 

		UPDATE[applicant].dbo.tbl_applicant SET CVLProfileID = @NewCVLId WHERE ID = @applicantID

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


CREATE PROCEDURE [dbo].[Update Applicant Data With CVLData Priority]
	@Customer_ID NVARCHAR(50),
  @ApplicantID INT,
  @ApplicationID INT,
  @ProfileID INT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

	DECLARE @appLastName NVARCHAR(255)
	DECLARE @appFirstName NVARCHAR(255)
	DECLARE @appGender NVARCHAR(1)
	DECLARE @appEMail NVARCHAR(255)
	DECLARE @appBirthDate DATETIME
	DECLARE @appmobileNumber NVARCHAR(50) = ''
	DECLARE @appPhoneNumber NVARCHAR(50) = ''
	DECLARE @appStreet NVARCHAR(70)
	DECLARE @appLocation NVARCHAR(70)
	DECLARE @appPostcode NVARCHAR(70)
	DECLARE @appCountryCode NVARCHAR(70)

	DECLARE @CVLPersID INT 
	DECLARE @cvLastName NVARCHAR(255)
	DECLARE @cvFirstName NVARCHAR(255)
	DECLARE @cvGender NVARCHAR(1)
	DECLARE @cvlBirthDate DATETIME
	DECLARE @cvlStreet NVARCHAR(70)
	DECLARE @cvlLocation NVARCHAR(70)
	DECLARE @cvlPostcode NVARCHAR(70)
	DECLARE @cvlCountryCode NVARCHAR(70)
	DECLARE @formatedNumber NVARCHAR(50) = ''

SELECT TOP (1) @appLastName = ISNULL(app.Lastname, '') ,
        @appFirstName = ISNULL(app.Firstname, '') ,
        @appGender = ISNULL(app.Gender, '') ,
				@appEMail = ISNULL(app.EMail, '') ,
				@appBirthDate = app.Birthdate,
				@appPhoneNumber = ISNULL(app.Telephone, ''), 
				@appmobileNumber = ISNULL(app.MobilePhone, ''),
				@appStreet = ISNULL(app.Street, ''), 
				@appLocation = ISNULL(app.Location, ''), 
				@appPostcode = ISNULL(app.Postcode, ''), 
				@appCountryCode = ISNULL(app.Country, '') 
FROM    [applicant].dbo.tbl_applicant app
WHERE   app.Customer_ID = @Customer_ID
        AND app.ID = @ApplicantID
				ORDER BY app.ID;

IF LEN(ISNULL(@appCountryCode, '') ) > 3
BEGIN
	SET @appCountryCode = ISNULL((SELECT TOP (1) Code FROM spCVLizerBaseInfo.dbo.tbl_Base_ISOCountry WHERE Bez_DE = ISNULL(@appCountryCode, '') ORDER BY Code), '')
END

IF CAST(@appBirthDate AS DATE) IS NULL
BEGIN
	SET @appBirthDate = NULL -- cast('01.01.1900' AS DATE)
END;


-- cvl data
SELECT TOP (1) @CVLPersID = cvlPers.ID ,
        @cvLastName = cvlPers.LastName ,
        @cvFirstName = cvlPers.FirstName ,
        @cvGender = ISNULL(cvlPers.FK_GenderCode, '') ,
        @cvlBirthDate = cvlPers.DateOfBirth 
FROM    [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation cvlPers
        LEFT JOIN [spCVLizerBaseInfo].dbo.tbl_CVLProfile cvlProfile ON cvlProfile.ID = cvlPers.FK_CVLID
WHERE   cvlProfile.Customer_ID = @Customer_ID
        AND cvlProfile.ID = @ProfileID
				ORDER BY cvlProfile.ID; 
IF ISNULL(@cvGender, '') = ''
    BEGIN
    SELECT TOP (1) @cvGender = ISNULL(cvlPers.FK_GenderCode, '')
		FROM     [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation cvlPers
                          WHERE     cvlPers.FirstName LIKE @cvFirstName AND ISNULL(cvlPers.FK_GenderCode, '') <> '' 
		ORDER BY cvlPers.FirstName
		END;
IF CAST(@cvlBirthDate AS DATE) IS NULL
BEGIN
	SET @cvlBirthDate = NULL -- cast('01.01.1900' AS DATE)
END;

SELECT TOP (1) @cvlStreet = ISNULL(cvlAdd.Street, ''),
				@cvlLocation = ISNULL(cvlAdd.City, ''), 
				@cvlPostcode = ISNULL(cvlAdd.PostCode, ''), 
				@cvlCountryCode = ISNULL(cvlAdd.FK_CountryCode, '')
FROM    [spCVLizerBaseInfo].dbo.tbl_CVLAddress cvlAdd
WHERE   cvlAdd.FK_PersonalID = @CVLPersID 
				ORDER BY cvlAdd.FK_PersonalID;

UPDATE  applicant.dbo.tbl_applicant
SET     CVLProfileID = @ProfileID
WHERE   tbl_applicant.ID = @ApplicantID
        AND tbl_applicant.Customer_ID = @Customer_ID;

-- update priority tables
IF ISNULL(@appFirstName, '') <> ISNULL(@cvFirstName, '') AND ISNULL(@cvFirstName, '') <> ''
    BEGIN
        UPDATE  dbo.tbl_applicant
        SET     Firstname = @cvFirstName 
        WHERE   tbl_applicant.ID = @ApplicantID
                AND tbl_applicant.Customer_ID = @Customer_ID;
    END;
IF ISNULL(@appLastName, '') <> ISNULL(@cvLastName, '') AND ISNULL(@cvLastName, '') <> ''
    BEGIN
        UPDATE  dbo.tbl_applicant
        SET     Lastname = @cvLastName                 
        WHERE   tbl_applicant.ID = @ApplicantID
                AND tbl_applicant.Customer_ID = @Customer_ID;
    END;

IF ISNULL(@appStreet, '') <> ISNULL(@cvlStreet, '') AND ISNULL(@cvlStreet, '') <> ''
    BEGIN
		UPDATE  applicant.dbo.tbl_applicant
			SET     Street = @cvlStreet
			WHERE   tbl_applicant.ID = @ApplicantID
					AND tbl_applicant.Customer_ID = @Customer_ID;
    END;

IF ISNULL(@appLocation, '') <> ISNULL(@cvlLocation, '') AND ISNULL(@cvlLocation, '') <> ''
    BEGIN
		UPDATE  applicant.dbo.tbl_applicant
			SET     [Location] = @cvlLocation
			WHERE   tbl_applicant.ID = @ApplicantID
					AND tbl_applicant.Customer_ID = @Customer_ID;
    END;

IF ISNULL(@appPostcode, '') <> ISNULL(@cvlPostcode, '') AND ISNULL(@cvlPostcode, '') <> ''
    BEGIN
		UPDATE  applicant.dbo.tbl_applicant
			SET     Postcode = @cvlPostcode
			WHERE   tbl_applicant.ID = @ApplicantID
					AND tbl_applicant.Customer_ID = @Customer_ID;
    END;

IF ISNULL(@appCountryCode, '') <> ISNULL(@cvlCountryCode, '') AND ISNULL(@cvlCountryCode, '') <> ''
    BEGIN
		UPDATE  applicant.dbo.tbl_applicant
			SET     Country = @cvlCountryCode
			WHERE   tbl_applicant.ID = @ApplicantID
					AND tbl_applicant.Customer_ID = @Customer_ID;
    END;

IF ISNULL(@appGender, '') <> ISNULL(@cvGender, '') AND ISNULL(@cvGender, '') <> ''
    BEGIN
		UPDATE  applicant.dbo.tbl_applicant
        SET     Gender = @cvGender
        WHERE   tbl_applicant.ID = @ApplicantID
                AND tbl_applicant.Customer_ID = @Customer_ID;
		SET @appGender = @cvGender
    END;
IF ISNULL(@appGender, '') <> ISNULL(@cvGender, '') AND ISNULL(@appGender, '') <> '' AND ISNULL(@cvGender, '') = ''
    BEGIN
		UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation 
				SET FK_GenderCode = @appGender
				WHERE   ID = @CVLPersID
        AND FK_CVLID = @ProfileID
    END;

IF ISNULL(@appEMail, '') = ''
    BEGIN
    UPDATE  applicant.dbo.tbl_applicant
        SET     EMail = ( SELECT TOP (1)
                                    cvlEMail.EMailAddress
                          FROM      [spCVLizerBaseInfo].dbo.tbl_CVLPersonalEMails cvlEMail
                          WHERE     cvlEMail.FK_PersonalID = @CVLPersID ORDER BY cvlEMail.FK_PersonalID
                        )
        WHERE   tbl_applicant.ID = @ApplicantID
                AND tbl_applicant.Customer_ID = @Customer_ID;
    END;

IF ISNULL(@appPhoneNumber, '') = ''
		BEGIN
		SELECT TOP (1) @appPhoneNumber = REPLACE(cvlTelefon.PhoneNumber, ' ', ''),
									@formatedNumber = cvlTelefon.PhoneNumber
									FROM      [spCVLizerBaseInfo].dbo.tbl_CVLPersonalPhoneNumbers cvlTelefon
									WHERE     cvlTelefon.FK_PersonalID = @CVLPersID ORDER BY cvlTelefon.FK_PersonalID

		IF CHARINDEX('079', @appPhoneNumber, 0) > 0 OR CHARINDEX('078', @appPhoneNumber, 0) > 0 OR CHARINDEX('076', @appPhoneNumber, 0) > 0 OR CHARINDEX('077', @appPhoneNumber, 0) > 0
			OR CHARINDEX('4179', @appPhoneNumber, 0) > 0 OR CHARINDEX('4178', @appPhoneNumber, 0) > 0 OR CHARINDEX('4176', @appPhoneNumber, 0) > 0 OR CHARINDEX('4177', @appPhoneNumber, 0) > 0
			OR CHARINDEX('41(0)79', @appPhoneNumber, 0) > 0 OR CHARINDEX('41(0)78', @appPhoneNumber, 0) > 0 OR CHARINDEX('41(0)76', @appPhoneNumber, 0) > 0 OR CHARINDEX('41(0)77', @appPhoneNumber, 0) > 0
		BEGIN
			SET @appmobileNumber = @formatedNumber
			SET @appPhoneNumber = ''
			SET @formatedNumber = ''
		END
		IF @appPhoneNumber <> '' 
		BEGIN
			SET @appPhoneNumber = @formatedNumber
		END 

    UPDATE  applicant.dbo.tbl_applicant SET 
                Telephone = @appPhoneNumber ,
                MobilePhone = @appmobileNumber
        WHERE   tbl_applicant.ID = @ApplicantID
                AND tbl_applicant.Customer_ID = @Customer_ID;

		END;

IF (CAST(@appBirthDate AS DATE) <> CAST(@cvlBirthDate AS DATE) OR @appBirthDate IS NULL ) AND @cvlBirthDate IS NOT NULL	
    BEGIN
    UPDATE  applicant.dbo.tbl_applicant
	    SET     Birthdate = @cvlBirthDate
        WHERE   tbl_applicant.ID = @ApplicantID
                AND tbl_applicant.Customer_ID = @Customer_ID;
    END;
IF @cvlBirthDate IS NULL AND @appBirthDate IS NOT NULL 
    BEGIN
    UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation
	    SET     DateOfBirth = @appBirthDate
        WHERE   tbl_CVLPersonalInformation.FK_CVLID = @ProfileID
                AND tbl_CVLPersonalInformation.ID = @CVLPersID;
    END;




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


CREATE PROCEDURE [dbo].[Update CVL Data With ApplicantData Priority]
	@Customer_ID NVARCHAR(50),
  @ApplicantID INT,
  @ApplicationID INT,
  @ProfileID INT

AS

BEGIN
SET NOCOUNT ON

	DECLARE @StartTranCount int


	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

	DECLARE @appLastName NVARCHAR(255)
	DECLARE @appFirstName NVARCHAR(255)
	DECLARE @appGender NVARCHAR(1)
	DECLARE @appEMail NVARCHAR(255)
	DECLARE @appBirthDate DATETIME
	DECLARE @appmobileNumber NVARCHAR(50) = ''
	DECLARE @appPhoneNumber NVARCHAR(50) = ''
	DECLARE @appStreet NVARCHAR(70)
	DECLARE @appLocation NVARCHAR(70)
	DECLARE @appPostcode NVARCHAR(70)
	DECLARE @appCountryCode NVARCHAR(70)

	DECLARE @CVLPersID INT 
	DECLARE @cvLastName NVARCHAR(255)
	DECLARE @cvFirstName NVARCHAR(255)
	DECLARE @cvGender NVARCHAR(1)
	DECLARE @cvlBirthDate DATETIME
	DECLARE @cvlStreet NVARCHAR(70)
	DECLARE @cvlLocation NVARCHAR(70)
	DECLARE @cvlPostcode NVARCHAR(70)
	DECLARE @cvlCountryCode NVARCHAR(70)
	DECLARE @formatedNumber NVARCHAR(50) = ''

SELECT TOP (1) @appLastName = ISNULL(app.Lastname, '') ,
        @appFirstName = ISNULL(app.Firstname, '') ,
        @appGender = ISNULL(app.Gender, '') ,
				@appEMail = ISNULL(app.EMail, '') ,
				@appBirthDate = app.Birthdate,
				@appPhoneNumber = ISNULL(app.Telephone, ''), 
				@appmobileNumber = ISNULL(app.MobilePhone, ''),
				@appStreet = ISNULL(app.Street, ''), 
				@appLocation = ISNULL(app.Location, ''), 
				@appPostcode = ISNULL(app.Postcode, ''), 
				@appCountryCode = ISNULL(app.Country, '')
FROM    [applicant].dbo.tbl_applicant app
WHERE   app.Customer_ID = @Customer_ID
        AND app.ID = @ApplicantID
				ORDER BY app.ID;

IF LEN(ISNULL(@appCountryCode, '') ) > 3
BEGIN
	SET @appCountryCode = ISNULL((SELECT TOP (1) Code FROM spCVLizerBaseInfo.dbo.tbl_Base_ISOCountry WHERE Bez_DE = ISNULL(@appCountryCode, '') ORDER BY Code), '')
END

IF CAST(@appBirthDate AS DATE) IS NULL
BEGIN
	SET @appBirthDate = NULL 
END;


-- cvl data
SELECT TOP (1) @CVLPersID = cvlPers.ID ,
        @cvLastName = cvlPers.LastName ,
        @cvFirstName = cvlPers.FirstName ,
        @cvGender = ISNULL(cvlPers.FK_GenderCode, '') ,
        @cvlbirthdate = cvlPers.DateOfBirth
FROM    [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation cvlPers
        LEFT JOIN [spCVLizerBaseInfo].dbo.tbl_CVLProfile cvlProfile ON cvlProfile.ID = cvlPers.FK_CVLID
WHERE   cvlProfile.Customer_ID = @Customer_ID
        AND cvlProfile.ID = @ProfileID
				ORDER BY cvlProfile.ID; 
IF ISNULL(@cvGender, '') = ''
    BEGIN
    SELECT TOP (1) @cvGender = ISNULL(cvlPers.FK_GenderCode, '')
		FROM     [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation cvlPers
                          WHERE     cvlPers.FirstName LIKE @cvFirstName AND ISNULL(cvlPers.FK_GenderCode, '') <> '' 
		ORDER BY cvlPers.FirstName
		END;
IF CAST(@cvlbirthdate AS DATE) IS NULL
BEGIN
	SET @cvlbirthdate = NULL 
END;

SELECT TOP (1) @cvlStreet = ISNULL(cvlAdd.Street, ''), @cvlLocation = ISNULL(cvlAdd.City, ''), 
				@cvlPostcode = ISNULL(cvlAdd.PostCode, ''), 
				@cvlCountryCode = ISNULL(cvlAdd.FK_CountryCode, '')
FROM    [spCVLizerBaseInfo].dbo.tbl_CVLAddress cvlAdd
WHERE   cvlAdd.FK_PersonalID = @CVLPersID 
				ORDER BY cvlAdd.FK_PersonalID;

UPDATE  [applicant].dbo.tbl_applicant
SET     CVLProfileID = @ProfileID
WHERE   tbl_applicant.ID = @ApplicantID
        AND tbl_applicant.Customer_ID = @Customer_ID;

-- update priority tables
IF ISNULL(@appFirstName, '') <> ISNULL(@cvFirstName, '') AND ISNULL(@appFirstName, '') <> ''
    BEGIN
        UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation
        SET     Firstname = @appFirstName 
        WHERE   tbl_CVLPersonalInformation.FK_CVLID = @ProfileID
                AND tbl_CVLPersonalInformation.ID = @CVLPersID;
    END;
IF ISNULL(@appLastName, '') <> ISNULL(@cvLastName, '') AND ISNULL(@appLastName, '') <> ''
    BEGIN
        UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation
        SET     Lastname = @appLastName 
        WHERE   tbl_CVLPersonalInformation.FK_CVLID = @ProfileID
                AND tbl_CVLPersonalInformation.ID = @CVLPersID;
    END;

IF ISNULL(@appStreet, '') <> ISNULL(@cvlStreet, '') AND ISNULL(@appStreet, '') <> ''
    BEGIN
		UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLAddress
			SET     Street = @appStreet
			WHERE   tbl_CVLAddress.FK_PersonalID = @CVLPersID;
    END;

IF ISNULL(@appLocation, '') <> ISNULL(@cvlLocation, '') AND ISNULL(@appLocation, '') <> ''
    BEGIN
		UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLAddress
			SET     City = @appLocation
			WHERE   tbl_CVLAddress.FK_PersonalID = @CVLPersID;
    END;

IF ISNULL(@appPostcode, '') <> ISNULL(@cvlPostcode, '') AND ISNULL(@appPostcode, '') <> ''
    BEGIN
		UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLAddress
			SET     PostCode = @appPostcode
			WHERE   tbl_CVLAddress.FK_PersonalID = @CVLPersID;
    END;

IF ISNULL(@appCountryCode, '') <> ISNULL(@cvlCountryCode, '') AND ISNULL(@appCountryCode, '') <> ''
    BEGIN
		UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLAddress
			SET     FK_CountryCode = @appCountryCode
			WHERE   tbl_CVLAddress.FK_PersonalID = @CVLPersID;
    END;

IF ISNULL(@appGender, '') <> ISNULL(@cvGender, '') AND ISNULL(@appGender, '') <> ''
    BEGIN
		UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation 
        SET     FK_GenderCode = @appGender
        WHERE   tbl_CVLPersonalInformation.FK_CVLID = @ProfileID
                AND tbl_CVLPersonalInformation.ID = @CVLPersID;
		SET @cvGender = @appGender
    END;
IF ISNULL(@appGender, '') <> ISNULL(@cvGender, '') AND ISNULL(@cvGender, '') <> '' AND ISNULL(@appGender, '') = ''
    BEGIN
		UPDATE  [applicant].dbo.tbl_applicant
        SET     Gender = @cvGender
        WHERE   Customer_ID = @Customer_ID AND ID = @ApplicantID;
    END;

IF ISNULL(@appEMail, '') = ''
    BEGIN
		IF NOT EXISTS(SELECT TOP (1)
																			EMailAddress
														FROM      [spCVLizerBaseInfo].dbo.tbl_CVLPersonalEMails 
														WHERE     FK_PersonalID = @CVLPersID AND EMailAddress NOT LIKE @appEMail
													)
			BEGIN
				INSERT INTO [spCVLizerBaseInfo].dbo.tbl_CVLPersonalEMails
				(
						FK_PersonalID,
						EMailAddress
				)
				VALUES
				(   @CVLPersID,  -- FK_PersonalID - int
						@appEMail -- EMailAddress - nvarchar(255)
						);
			END;

    END;

IF ISNULL(@appPhoneNumber, '') = ''
		BEGIN

    SELECT TOP (1) @appPhoneNumber = REPLACE(Telephone, ' ', ''),
				@appmobileNumber = REPLACE(MobilePhone, ' ', '')    
				FROM applicant.dbo.tbl_applicant        
				WHERE   tbl_applicant.ID = @ApplicantID
                AND tbl_applicant.Customer_ID = @Customer_ID ORDER BY tbl_applicant.ID;

		IF CHARINDEX('079', @appPhoneNumber, 0) > 0 OR CHARINDEX('078', @appPhoneNumber, 0) > 0 OR CHARINDEX('076', @appPhoneNumber, 0) > 0 OR CHARINDEX('077', @appPhoneNumber, 0) > 0
			OR CHARINDEX('4179', @appPhoneNumber, 0) > 0 OR CHARINDEX('4178', @appPhoneNumber, 0) > 0 OR CHARINDEX('4176', @appPhoneNumber, 0) > 0 OR CHARINDEX('4177', @appPhoneNumber, 0) > 0
			OR CHARINDEX('41(0)79', @appPhoneNumber, 0) > 0 OR CHARINDEX('41(0)78', @appPhoneNumber, 0) > 0 OR CHARINDEX('41(0)76', @appPhoneNumber, 0) > 0 OR CHARINDEX('41(0)77', @appPhoneNumber, 0) > 0
		BEGIN
			SET @appmobileNumber = @formatedNumber
			SET @appPhoneNumber = ''
			SET @formatedNumber = ''
		END
		IF @appPhoneNumber <> '' 
		BEGIN
			SET @appPhoneNumber = @formatedNumber
		END 

		IF NOT EXISTS(SELECT TOP (1)
																			PhoneNumber
														FROM      [spCVLizerBaseInfo].dbo.tbl_CVLPersonalPhoneNumbers 
														WHERE     FK_PersonalID = @CVLPersID AND PhoneNumber NOT LIKE @appPhoneNumber
													)
			BEGIN
				INSERT INTO [spCVLizerBaseInfo].dbo.tbl_CVLPersonalPhoneNumbers 
				(
						FK_PersonalID,
						PhoneNumber
				)
				VALUES
				(   @CVLPersID,  -- FK_PersonalID - int
						@appPhoneNumber -- PhoneNumber - nvarchar(255)
						);
			END;


		END;

IF (CAST(@appBirthDate AS DATE) <> CAST(@cvlBirthDate AS DATE) OR @cvlBirthDate IS NULL) AND @appBirthDate IS NOT NULL	
    BEGIN
    UPDATE  [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation
	    SET     DateOfBirth = @appBirthDate
        WHERE   tbl_CVLPersonalInformation.FK_CVLID = @ProfileID
                AND tbl_CVLPersonalInformation.ID = @CVLPersID;
    END;

IF @appBirthDate IS NULL AND @cvlBirthDate IS NOT NULL 
    BEGIN
    UPDATE  [applicant].dbo.tbl_applicant
	    SET     Birthdate = @cvlBirthDate
				WHERE   Customer_ID = @Customer_ID
								AND ID = @ApplicantID;
    END;


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


CREATE PROCEDURE [dbo].[List New Applicant For Notifications For ALL Customers]
	@CustomerID NVARCHAR(50)
AS

BEGIN
SET NOCOUNT ON

SELECT A.ID ,
       A.Customer_ID ,
       A.EmployeeID ,
       A.Lastname ,
       A.Firstname ,
       A.Gender ,
       A.Street ,
       A.PostOfficeBox ,
       A.Postcode ,
       A.Latitude ,
       A.Longitude ,
       A.Location ,
			(CASE 
				WHEN  LEN(ISNULL(A.Country, '') ) > 3 THEN ISNULL((SELECT TOP (1) Code FROM spCVLizerBaseInfo.dbo.tbl_Base_ISOCountry WHERE Bez_DE = ISNULL(A.Country, '') ORDER BY Code), '')
				WHEN  LEN(ISNULL(A.Country, '') ) <= 3 AND LEN(ISNULL(A.Country, '') ) > 0 THEN Country
				ELSE	''
			END) Country ,
			(CASE 
				WHEN  ISNULL(A.Nationality, '') = '' THEN ISNULL((SELECT TOP (1) pn.FK_NationalityCode FROM spCVLizerBaseInfo.dbo.tbl_CVLPersonalNationality pn 
						WHERE pn.FK_PersonalID = dbo.uf_GetPersonalIDWithCVLProfileID(A.CVLProfileID) ORDER BY pn.FK_PersonalID), '')
				
				ELSE	ISNULL(A.Nationality, '')
			END) Nationality ,
       A.EMail ,
       A.Telephone ,
       A.MobilePhone ,
       CONVERT(DATE, A.Birthdate) Birthdate ,
       A.Permission ,
       A.Profession ,
       A.Auto ,
       A.Motorcycle ,
       A.Bicycle ,
       A.DrivingLicence1 ,
       A.DrivingLicence2 ,
       A.DrivingLicence3 ,
       A.CivilState ,
       A.Language ,
       A.LanguageLevel ,
       A.ApplicantLifecycle ,
       A.CreatedOn ,
       A.CreatedFrom ,
       A.CheckedOn ,
       A.CheckedFrom ,
       A.CVLProfileID
	FROM dbo.tbl_applicant A
	WHERE (ISNULL(@CustomerID, '') = '' OR A.Customer_ID = @CustomerID)
	AND ISNULL(A.CVLProfileID, 0) > 0
	AND ISNULL(A.Firstname, '') <> '' 
	AND ISNULL(A.Lastname, '') <> '' 
	AND A.CheckedOn IS NULL
	ORDER BY A.CreatedOn DESC

END;
GO

/* end of creating sp ------------------------*/



CREATE FUNCTION [dbo].[uf_GetPersonalIDWithCVLProfileID]
(
    @CVLProfileID INT
)

RETURNS INT
AS

BEGIN
DECLARE @PersonalID INT = 0
SELECT TOP (1) @PersonalID = P.ID FROM spCVLizerBaseInfo.dbo.tbl_CVLPersonalInformation P WHERE P.FK_CVLID = @CVLProfileID ORDER BY P.id DESC

    RETURN @PersonalID;

END;


/* ----------------- end of query -------------------------------*/

