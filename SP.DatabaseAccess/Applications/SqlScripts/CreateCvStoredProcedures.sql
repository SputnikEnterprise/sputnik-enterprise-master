USE [applicant]
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvAddress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvAddress]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
25.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvAddress

Bemerkung:
-

Eingabeparameter:
@Addressline
@StreetName
@StreetNumberBase
@StreetNumberExtension
@PostalCode
@City
@FK_CvRegion                Fremdschlüssel tab_CvRegion.ID
@FK_CvCountry               Fremdschlüssel tab_CvCountry.ID

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvAddress]
	@AddressLine [nvarchar](255) = NULL
	,@StreetName [nvarchar](255) = NULL
	,@StreetNumberBase [nvarchar](255) = NULL
	,@StreetNumberExtension [nvarchar](255) = NULL
	,@PostalCode [nvarchar](255) = NULL
	,@City [nvarchar](255) = NULL
	,@FK_CvRegion [int] = NULL
	,@FK_CvCountry [int] = NULL
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvAddress (
			[AddressLine]
			,[StreetName]
			,[StreetNumberBase]
			,[StreetNumberExtension]
			,[PostalCode]
			,[City]
			,[FK_CvRegion]
			,[FK_CvCountry]
			,[CreatedOn]
			,[CreatedFrom]
		)
		VALUES (
			@AddressLine
			,@StreetName
			,@StreetNumberBase
			,@StreetNumberExtension
			,@PostalCode
			,@City
			,@FK_CvRegion
			,@FK_CvCountry
			,GETDATE() -- CreatedOn
			,N'System' -- CreatedFrom
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvCustomArea]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvCustomArea]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
10.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvCustomArea

Bemerkung:
-

Eingabeparameter:
@FK_CvProfile               Fremdschlüssel tab_CvProfile.ID
@CvTitle
@CreatedOn
@CreatedBy
@EditedOn
@EditedBy
@TotalExperienceYears
@CurrentJob
@CurrentEmployer
@Last3Experiences
@FK_CvHighestEducationLevel Fremdschlüssel tab_CvHighestEducationLevel.ID
@FK_CvSalary                Fremdschlüssel tab_CvSalary.ID
@FK_CvProfileStatus         Fremdschlüssel tab_CvProfileStatus.ID
@FK_CvAvailability          Fremdschlüssel tab_CvAvailability.ID
@CvComment
@LearnedOccupation
@FK_CvApproval              Fremdschlüssel tab_CvApproval.ID
@POBox
@ExternalID
@FK_CvProfilePicture        Fremdschlüssel tab_CvPicture.ID

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvCustomArea]
	@FK_CvProfile [int]
	,@CvTitle [nvarchar](255) = NULL
	,@CreatedOn [datetime] = NULL
	,@CreatedBy [nvarchar](255) = NULL
	,@EditedOn [datetime] = NULL
	,@EditedBy [nvarchar](255) = NULL
	,@TotalExperienceYears [int] = NULL
	,@CurrentJob [nvarchar](255) = NULL
	,@CurrentEmployer [nvarchar](255) = NULL
	,@Last3Experiences [nvarchar](2047) = NULL
	,@FK_CvHighestEducationLevel [int] = NULL
	,@FK_CvSalary [int] = NULL
	,@FK_CvProfileStatus [int] = NULL
	,@FK_CvAvailability [int] = NULL
	,@CvComment [nvarchar](2047) = NULL
	,@LearnedOccupation [nvarchar](255) = NULL
	,@FK_CvApproval [int] = NULL
	,@POBox [nvarchar](255) = NULL
	,@ExternalID [nvarchar](255) = NULL
	,@FK_CvProfilePicture [int] = NULL
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvCustomArea (
			[FK_CvProfile]
			,[CvTitle]
			,[CreatedOn]
			,[CreatedBy]
			,[EditedOn]
			,[EditedBy]
			,[TotalExperienceYears]
			,[CurrentJob]
			,[CurrentEmployer]
			,[Last3Experiences]
			,[FK_CvHighestEducationLevel]
			,[FK_CvSalary]
			,[FK_CvProfileStatus]
			,[FK_CvAvailability]
			,[CvComment]
			,[LearnedOccupation]
			,[FK_CvApproval]
			,[POBox]
			,[ExternalID]
			,[FK_CvProfilePicture]
		)
		VALUES (
			@FK_CvProfile
			,@CvTitle
			,@CreatedOn
			,@CreatedBy
			,@EditedOn
			,@EditedBy
			,@TotalExperienceYears
			,@CurrentJob
			,@CurrentEmployer
			,@Last3Experiences
			,@FK_CvHighestEducationLevel
			,@FK_CvSalary
			,@FK_CvProfileStatus
			,@FK_CvAvailability
			,@CvComment
			,@LearnedOccupation
			,@FK_CvApproval
			,@POBox
			,@ExternalID
			,@FK_CvProfilePicture
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvDocumentHtml]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvDocumentHtml]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
28.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvDocumentHtml

Bemerkung:
-

Eingabeparameter:
@Content

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvDocumentHtml]
	@Content [nvarchar](max)
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvDocumentHtml (
			[Content]
		)
		VALUES (
			@Content
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvDocumentText]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvDocumentText]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
28.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvDocumentText

Bemerkung:
-

Eingabeparameter:
@Content

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvDocumentText]
	@Content [nvarchar](max)
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvDocumentText (
			[Content]
		)
		VALUES (
			@Content
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvEducationHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvEducationHistory]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
28.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvEducationHistory

Bemerkung:
-

Eingabeparameter:
@FK_CvProfile               Fremdschlüssel tab_CvProfile.ID
@FK_CvEducation             Fremdschlüssel tab_CvEducation.ID
@FK_CvEducationLevel        Fremdschlüssel tab_CvEducationLevel.ID
@FK_CvEducationDetail       Fremdschlüssel tab_CvEducationDetail.ID
@DegreeDirection
@FK_CvDegreeDirection       Fremdschlüssel tab_CvDegreeDirection.ID
@StartDate
@EndDate
@InstituteNameAndPlace
@InstituteName
@InstitutePlace
@FK_CvInstituteType         Fremdschlüssel tab_CvInstituteType.ID
@FK_CvDiploma               Fremdschlüssel tab_CvDiploma.ID
@DiplomaDate
@Subjects
@IsHighestItem

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvEducationHistory]
	@FK_CvProfile [int]
	,@FK_CvEducation [int] = NULL
	,@FK_CvEducationLevel [int] = NULL
	,@FK_CvEducationDetail [int] = NULL
	,@DegreeDirection [nvarchar](255) = NULL
	,@FK_CvDegreeDirection [int] = NULL
	,@StartDate [datetime] = NULL
	,@EndDate [datetime] = NULL
	,@InstituteNameAndPlace [nvarchar](255) = NULL
	,@InstituteName [nvarchar](255) = NULL
	,@InstitutePlace [nvarchar](255) = NULL
	,@FK_CvInstituteType [int] = NULL
	,@FK_CvDiploma [int] = NULL
	,@DiplomaDate [datetime] = NULL
	,@Subjects [nvarchar](255) = NULL
	,@IsHighestItem [bit] = NULL
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvEducationHistory (
			[FK_CvProfile]
			,[FK_CvEducation]
			,[FK_CvEducationLevel]
			,[FK_CvEducationDetail]
			,[DegreeDirection]
			,[FK_CvDegreeDirection]
			,[StartDate]
			,[EndDate]
			,[InstituteNameAndPlace]
			,[InstituteName]
			,[InstitutePlace]
			,[FK_CvInstituteType]
			,[FK_CvDiploma]
			,[DiplomaDate]
			,[Subjects]
			,[IsHighestItem]
		)
		VALUES (
			@FK_CvProfile
			,@FK_CvEducation
			,@FK_CvEducationLevel
			,@FK_CvEducationDetail
			,@DegreeDirection
			,@FK_CvDegreeDirection
			,@StartDate
			,@EndDate
			,@InstituteNameAndPlace
			,@InstituteName
			,@InstitutePlace
			,@FK_CvInstituteType
			,@FK_CvDiploma
			,@DiplomaDate
			,@Subjects
			,@IsHighestItem
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvEmploymentHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvEmploymentHistory]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
28.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvEmploymentHistory

Bemerkung:
-

Eingabeparameter:
@FK_CvProfile               Fremdschlüssel tab_CvProfile.ID
@JobTitle
@FK_CvJobTitle              Fremdschlüssel tab_CvJobTitle.ID
@StartDate
@EndDate
@ExperienceYears
@EmployerNameAndPlace
@EmployerName
@EmployerPlace
@Description
@QuitReason
@IsLastItem
@IsLastItemWithJobTitle
@IsCurrentEmployer
@Remarks

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvEmploymentHistory]
	@FK_CvProfile [int]
	,@JobTitle [nvarchar](255) = NULL
	,@FK_CvJobTitle [int] = NULL
	,@StartDate [datetime] = NULL
	,@EndDate [datetime] = NULL
	,@ExperienceYears [int] = NULL
	,@EmployerNameAndPlace [nvarchar](255) = NULL
	,@EmployerName [nvarchar](255) = NULL
	,@EmployerPlace [nvarchar](255) = NULL
	,@Description [nvarchar](255) = NULL
	,@QuitReason [nvarchar](255) = NULL
	,@IsLastItem [bit] = NULL
	,@IsLastItemWithJobTitle [bit] = NULL
	,@IsCurrentEmployer [bit] = NULL
	,@Remarks [nvarchar](255) = NULL
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvEmploymentHistory (
			[FK_CvProfile]
			,[JobTitle]
			,[FK_CvJobTitle]
			,[StartDate]
			,[EndDate]
			,[ExperienceYears]
			,[EmployerNameAndPlace]
			,[EmployerName]
			,[EmployerPlace]
			,[Description]
			,[QuitReason]
			,[IsLastItem]
			,[IsLastItemWithJobTitle]
			,[IsCurrentEmployer]
			,[Remarks]
		)
		VALUES (
			@FK_CvProfile
			,@JobTitle
			,@FK_CvJobTitle
			,@StartDate
			,@EndDate
			,@ExperienceYears
			,@EmployerNameAndPlace
			,@EmployerName
			,@EmployerPlace
			,@Description
			,@QuitReason
			,@IsLastItem
			,@IsLastItemWithJobTitle
			,@IsCurrentEmployer
			,@Remarks
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvEmail]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
27.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvEmail

Bemerkung:
-

Eingabeparameter:
@FK_CvPersonal              Fremdschlüssel tab_CvPersonal.ID
@FK_CvEmailType             Fremdschlüssel tab_CvEmailType.ID
@Email

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvEmail]
	@FK_CvPersonal [int]
	,@FK_CvEmailType [int]
	,@Email [nvarchar](255)
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvEmail (
			[FK_CvPersonal]
			,[FK_CvEmailType]
			,[Email]
		)
		VALUES (
			@FK_CvPersonal
			,@FK_CvEmailType
			,@Email
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvExtraInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvExtraInfo]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
10.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvExtraInfo

Bemerkung:
-

Eingabeparameter:
@FK_CvCustomArea            Fremdschlüssel tab_CvCustomArea.ID
@Key
@Value

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvExtraInfo]
	@FK_CvCustomArea [int]
	,@Key [nvarchar](255)
	,@Value [nvarchar](255) = NULL
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvExtraInfo (
			[FK_CvCustomArea]
			,[Key]
			,[Value]
		)
		VALUES (
			@FK_CvCustomArea
			,@Key
			,@Value
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvHobby]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvHobby]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
10.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvHobby

Bemerkung:
-

Eingabeparameter:
@FK_CvOther                 Fremdschlüssel tab_CvOther.ID
@Text

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvHobby]
	@FK_CvOther [int]
	,@Text [nvarchar](255)
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvHobby (
			[FK_CvOther]
			,[Text]
		)
		VALUES (
			@FK_CvOther
			,@Text
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvOther]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvOther]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
10.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvOther

Bemerkung:
-

Eingabeparameter:
@FK_CvProfile               Fremdschlüssel tab_CvProfile.ID
@TotalExperience
@Salary
@Benefits

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvOther]
	@FK_CvProfile [int]
	,@TotalExperience [nvarchar](255) = NULL
	,@Salary [nvarchar](255) = NULL
	,@Benefits [nvarchar](255) = NULL
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvOther (
			[FK_CvProfile]
			,[TotalExperience]
			,[Salary]
			,[Benefits]
		)
		VALUES (
			@FK_CvProfile
			,@TotalExperience
			,@Salary
			,@Benefits
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




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvPersonal]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvPersonal]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
25.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvPersonal

Bemerkung:
-

Eingabeparameter:
@Initials
@Title
@FirstName
@MiddleName
@LastNamePrefix
@LastName
@FullName
@DateOfBirth
@PlaceOfBirth
@FK_CvNationality           Fremdschlüssel tab_CvNationality.ID
@FK_CvGender                Fremdschlüssel tab_CvGender.ID
@FK_CvDriversLicence        Fremdschlüssel tab_CvDriversLicence.ID
@FK_CvMaritalStatus         Fremdschlüssel tab_CvMaritalStatus.ID
@Availability
@MilitaryService
@FK_CvAddress               Fremdschlüssel tab_CvAddress.ID

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvPersonal]
	@Initials [nvarchar](255) = NULL
	,@Title [nvarchar](255) = NULL
	,@FirstName [nvarchar](255) = NULL
	,@MiddleName [nvarchar](255) = NULL
	,@LastNamePrefix [nvarchar](255) = NULL
	,@LastName [nvarchar](255) = NULL
	,@FullName [nvarchar](255) = NULL
	,@DateOfBirth [datetime] = NULL
	,@PlaceOfBirth [nvarchar](255) = NULL
	,@FK_CvNationality [int] = NULL
	,@FK_CvGender [int] = NULL
	,@FK_CvDriversLicence [int] = NULL
	,@FK_CvMaritalStatus [int] = NULL
	,@Availability [nvarchar](255) = NULL
	,@MilitaryService [nvarchar](255) = NULL
	,@FK_CvAddress [int]
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvPersonal (
			[Initials]
			,[Title]
			,[FirstName]
			,[MiddleName]
			,[LastNamePrefix]
			,[LastName]
			,[FullName]
			,[DateOfBirth]
			,[PlaceOfBirth]
			,[FK_CvNationality]
			,[FK_CvGender]
			,[FK_CvDriversLicence]
			,[FK_CvMaritalStatus]
			,[Availability]
			,[MilitaryService]
			,[FK_CvAddress]
			,[CreatedOn]
			,[CreatedFrom]
		)
		VALUES (
			@Initials
			,@Title
			,@FirstName
			,@MiddleName
			,@LastNamePrefix
			,@LastName
			,@FullName
			,@DateOfBirth
			,@PlaceOfBirth
			,@FK_CvNationality
			,@FK_CvGender
			,@FK_CvDriversLicence
			,@FK_CvMaritalStatus
			,@Availability
			,@MilitaryService
			,@FK_CvAddress
			,GETDATE() -- CreatedOn
			,N'System' -- CreatedFrom
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvPhoneNumber]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvPhoneNumber]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
27.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvPhoneNumber

Bemerkung:
-

Eingabeparameter:
@FK_CvPersonal              Fremdschlüssel tab_CvPersonal.ID
@FK_CvPhoneNumberType       Fremdschlüssel tab_CvPhoneNumberType.ID
@PhoneNumber

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvPhoneNumber]
	@FK_CvPersonal [int]
	,@FK_CvPhoneNumberType [int]
	,@PhoneNumber [nvarchar](255)
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvPhoneNumber (
			[FK_CvPersonal]
			,[FK_CvPhoneNumberType]
			,[PhoneNumber]
		)
		VALUES (
			@FK_CvPersonal
			,@FK_CvPhoneNumberType
			,@PhoneNumber
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvPicture]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvPicture]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
28.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvPicture

Bemerkung:
-

Eingabeparameter:
@Content
@Filename
@ContentType

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvPicture]
	@Content [varbinary](max)
	,@Filename [nvarchar](255)
	,@ContentType [nvarchar](255)
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvPicture (
			[Content]
			,[Filename]
			,[ContentType]
		)
		VALUES (
			@Content
			,@Filename
			,@ContentType
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvProfile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvProfile]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
21.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvProfile

Bemerkung:
-

Eingabeparameter:
@TrxmlID                    TRXML ID
@FK_CvPersonal              Fremdschlüssel tab_CvPersonal.ID
@FK_CvDocumentText          Fremdschlüssel tab_CvDocumentText.ID
@FK_CvDocumentHtml          Fremdschlüssel tab_CvDocumentHtml.ID

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvProfile]
	@TrxmlID [int] = NULL
	,@FK_CvPersonal [int]
	,@FK_CvDocumentText [int] = NULL
	,@FK_CvDocumentHtml [int] = NULL
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvProfile (
			[TrxmlID]
			,[FK_CvPersonal]
			,[FK_CvDocumentText]
			,[FK_CvDocumentHtml]
			,[CreatedOn]
			,[CreatedFrom]
    )
		VALUES (
			@TrxmlID
			,@FK_CvPersonal
			,@FK_CvDocumentText
			,@FK_CvDocumentHtml
			,GETDATE() -- CreatedOn
			,N'System' -- CreatedFrom
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvReference]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvReference]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
10.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvReference

Bemerkung:
-

Eingabeparameter:
@FK_CvOther                 Fremdschlüssel tab_CvOther.ID
@Text

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvReference]
	@FK_CvOther [int]
	,@Text [nvarchar](255)
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvReference (
			[FK_CvOther]
			,[Text]
		)
		VALUES (
			@FK_CvOther
			,@Text
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvSkill]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvSkill]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
29.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvSkill

Bemerkung:
-

Eingabeparameter:
@FK_CvProfile               Fremdschlüssel tab_CvSkill.ID

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvSkill]
	@FK_CvProfile [int]
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvSkill (
			[FK_CvProfile]
    )
		VALUES (
			@FK_CvProfile
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvComputerSkill]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvComputerSkill]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
29.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvComputerSkill

Bemerkung:
-

Eingabeparameter:
@FK_CvSkill                 Fremdschlüssel tab_CvSkill.ID
@Text
@FK_CvComputerSkillType     Fremdschlüssel tab_CvComputerSkillType.ID
@Duration

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvComputerSkill]
	@FK_CvSkill [int]
	,@Text [nvarchar](255) = NULL
	,@FK_CvComputerSkillType [int] = NULL
	,@Duration [nvarchar](255) = NULL
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvComputerSkill (
			[FK_CvSkill]
			,[Text]
			,[FK_CvComputerSkillType]
			,[Duration]
    )
		VALUES (
			@FK_CvSkill
			,@Text
			,@FK_CvComputerSkillType
			,@Duration
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvLanguageSkill]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvLanguageSkill]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
29.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvLanguageSkill

Bemerkung:
-

Eingabeparameter:
@FK_CvSkill                 Fremdschlüssel tab_CvSkill.ID
@Text
@FK_CvLanguageSkillType     Fremdschlüssel tab_CvLanguageSkillType.ID
@FK_CvLanguageProficiency    Fremdschlüssel tab_CvLanguageProficiency.ID
@IsNativeLanguage

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvLanguageSkill]
	@FK_CvSkill [int]
	,@Text [nvarchar](255) = NULL
	,@FK_CvLanguageSkillType [int] = NULL
	,@FK_CvLanguageProficiency [int] = NULL
	,@IsNativeLanguage [bit]
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvLanguageSkill (
			[FK_CvSkill]
			,[Text]
			,[FK_CvLanguageSkillType]
			,[FK_CvLanguageProficiency]
			,[IsNativeLanguage]
    )
		VALUES (
			@FK_CvSkill
			,@Text
			,@FK_CvLanguageSkillType
			,@FK_CvLanguageProficiency
			,@IsNativeLanguage
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




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvSoftSkill]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvSoftSkill]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
29.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvSoftSkill

Bemerkung:
-

Eingabeparameter:
@FK_CvSkill                 Fremdschlüssel tab_CvSkill.ID
@Text
@FK_CvSoftSkillType         Fremdschlüssel tab_CvSoftSkillType.ID

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvSoftSkill]
	@FK_CvSkill [int]
	,@Text [nvarchar](255) = NULL
	,@FK_CvSoftSkillType [int] = NULL
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvSoftSkill (
			[FK_CvSkill]
			,[Text]
			,[FK_CvSoftSkillType]
    )
		VALUES (
			@FK_CvSkill
			,@Text
			,@FK_CvSoftSkillType
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




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvSocialMedia]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvSocialMedia]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
27.01.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvSocialMedia

Bemerkung:
-

Eingabeparameter:
@FK_CvPersonal              Fremdschlüssel tab_CvPersonal.ID
@FK_CvSocialMediaType       Fremdschlüssel tab_CvSocialMediaType.ID
@Url

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvSocialMedia]
	@FK_CvPersonal [int]
	,@FK_CvSocialMediaType [int] = null
	,@Url [nvarchar](255)
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvSocialMedia (
			[FK_CvPersonal]
			,[FK_CvSocialMediaType]
			,[Url]
		)
		VALUES (
			@FK_CvPersonal
			,@FK_CvSocialMediaType
			,@Url
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvTransportation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvTransportation]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
12.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvTransportation

Bemerkung:
-

Eingabeparameter:
@FK_CvCustomArea            Fremdschlüssel tab_CvCustomArea.ID
@DriversLicence
@Car
@Motorcycle
@Bicycle

Ausgabeparameter:
@NewId                      Neue erstellte ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvTransportation]
	@FK_CvCustomArea [int]
	,@DriversLicence [nvarchar](255) = NULL
	,@Car [bit]
	,@Motorcycle [bit]
	,@Bicycle [bit]
	,@NewId int OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

		INSERT INTO dbo.tbl_CvTransportation (
			[FK_CvCustomArea]
			,[DriversLicence]
			,[Car]
			,[Motorcycle]
			,[Bicycle]
		)
		VALUES (
			@FK_CvCustomArea
			,@DriversLicence
			,@Car
			,@Motorcycle
			,@Bicycle
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



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvComputerSkillType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvComputerSkillType]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
19.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvComputerSkillType falls @Code nicht existiert.

Bemerkung:
-

Eingabeparameter:
@Code
@Name
@Description

Ausgabeparameter:
@Id                         Neue erstellte oder vorhandene ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvComputerSkillType]
	@Code [nvarchar](255)
	,@Name [nvarchar](255) = null
	,@Description [nvarchar](255) = null
	,@Id int OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @Id = Id FROM dbo.tbl_CvComputerSkillType WHERE [Code] = @Code
	IF @ID IS NULL BEGIN
		DECLARE @StartTranCount int
		BEGIN TRY
			SET @StartTranCount = @@TRANCOUNT
			IF @StartTranCount = 0 BEGIN TRAN
			INSERT INTO dbo.tbl_CvComputerSkillType (
				[Name]
				,[Code]
				,[Description]
			)
			VALUES (
				ISNULL(@Name, @Code)
				,@Code
				,ISNULL(@Description, ISNULL(@Name, @Code))
			)
			SET @Id = @@Identity
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
END
GO




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvDegreeDirection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvDegreeDirection]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
19.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvDegreeDirection falls @Code nicht existiert.

Bemerkung:
-

Eingabeparameter:
@Code
@Name
@Description

Ausgabeparameter:
@Id                         Neue erstellte oder vorhandene ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvDegreeDirection]
	@Code [nvarchar](255)
	,@Name [nvarchar](255) = null
	,@Description [nvarchar](255) = null
	,@Id int OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @Id = Id FROM dbo.tbl_CvDegreeDirection WHERE [Code] = @Code
	IF @ID IS NULL BEGIN
		DECLARE @StartTranCount int
		BEGIN TRY
			SET @StartTranCount = @@TRANCOUNT
			IF @StartTranCount = 0 BEGIN TRAN
			INSERT INTO dbo.tbl_CvDegreeDirection (
				[Name]
				,[Code]
				,[Description]
			)
			VALUES (
				ISNULL(@Name, @Code)
				,@Code
				,ISNULL(@Description, ISNULL(@Name, @Code))
			)
			SET @Id = @@Identity
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
END
GO




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvDriversLicence]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvDriversLicence]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
19.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvDriversLicence falls @Code nicht existiert.

Bemerkung:
-

Eingabeparameter:
@Code
@Name
@Description

Ausgabeparameter:
@Id                         Neue erstellte oder vorhandene ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvDriversLicence]
	@Code [nvarchar](255)
	,@Name [nvarchar](255) = null
	,@Description [nvarchar](255) = null
	,@Id int OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @Id = Id FROM dbo.tbl_CvDriversLicence WHERE [Code] = @Code
	IF @ID IS NULL BEGIN
		DECLARE @StartTranCount int
		BEGIN TRY
			SET @StartTranCount = @@TRANCOUNT
			IF @StartTranCount = 0 BEGIN TRAN
			INSERT INTO dbo.tbl_CvDriversLicence (
				[Name]
				,[Code]
				,[Description]
			)
			VALUES (
				ISNULL(@Name, @Code)
				,@Code
				,ISNULL(@Description, ISNULL(@Name, @Code))
			)
			SET @Id = @@Identity
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
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvEducation]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvEducation]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
19.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvEducation falls @Code nicht existiert.

Bemerkung:
-

Eingabeparameter:
@Code
@Name
@Description

Ausgabeparameter:
@Id                         Neue erstellte oder vorhandene ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvEducation]
	@Code [nvarchar](255)
	,@Name [nvarchar](255) = null
	,@Description [nvarchar](255) = null
	,@Id int OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @Id = Id FROM dbo.tbl_CvEducation WHERE [Code] = @Code
	IF @ID IS NULL BEGIN
		DECLARE @StartTranCount int
		BEGIN TRY
			SET @StartTranCount = @@TRANCOUNT
			IF @StartTranCount = 0 BEGIN TRAN
			INSERT INTO dbo.tbl_CvEducation (
				[Name]
				,[Code]
				,[Description]
			)
			VALUES (
				ISNULL(@Name, @Code)
				,@Code
				,ISNULL(@Description, ISNULL(@Name, @Code))
			)
			SET @Id = @@Identity
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
END
GO




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvEducationDetail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvEducationDetail]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
19.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvEducationDetail falls @Code nicht existiert.

Bemerkung:
-

Eingabeparameter:
@Code
@Name
@Description

Ausgabeparameter:
@Id                         Neue erstellte oder vorhandene ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvEducationDetail]
	@Code [nvarchar](255)
	,@Name [nvarchar](255) = null
	,@Description [nvarchar](255) = null
	,@Id int OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @Id = Id FROM dbo.tbl_CvEducationDetail WHERE [Code] = @Code
	IF @ID IS NULL BEGIN
		DECLARE @StartTranCount int
		BEGIN TRY
			SET @StartTranCount = @@TRANCOUNT
			IF @StartTranCount = 0 BEGIN TRAN
			INSERT INTO dbo.tbl_CvEducationDetail (
				[Name]
				,[Code]
				,[Description]
			)
			VALUES (
				ISNULL(@Name, @Code)
				,@Code
				,ISNULL(@Description, ISNULL(@Name, @Code))
			)
			SET @Id = @@Identity
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
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvInstituteType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvInstituteType]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
19.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvInstituteType falls @Code nicht existiert.

Bemerkung:
-

Eingabeparameter:
@Code
@Name
@Description

Ausgabeparameter:
@Id                         Neue erstellte oder vorhandene ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvInstituteType]
	@Code [nvarchar](255)
	,@Name [nvarchar](255) = null
	,@Description [nvarchar](255) = null
	,@Id int OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @Id = Id FROM dbo.tbl_CvInstituteType WHERE [Code] = @Code
	IF @ID IS NULL BEGIN
		DECLARE @StartTranCount int
		BEGIN TRY
			SET @StartTranCount = @@TRANCOUNT
			IF @StartTranCount = 0 BEGIN TRAN
			INSERT INTO dbo.tbl_CvInstituteType (
				[Name]
				,[Code]
				,[Description]
			)
			VALUES (
				ISNULL(@Name, @Code)
				,@Code
				,ISNULL(@Description, ISNULL(@Name, @Code))
			)
			SET @Id = @@Identity
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
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvJobTitle]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvJobTitle]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
19.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvJobTitle falls @Code nicht existiert.

Bemerkung:
-

Eingabeparameter:
@Code
@Name
@Description

Ausgabeparameter:
@Id                         Neue erstellte oder vorhandene ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvJobTitle]
	@Code [nvarchar](255)
	,@Name [nvarchar](255) = null
	,@Description [nvarchar](255) = null
	,@Id int OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @Id = Id FROM dbo.tbl_CvJobTitle WHERE [Code] = @Code
	IF @ID IS NULL BEGIN
		DECLARE @StartTranCount int
		BEGIN TRY
			SET @StartTranCount = @@TRANCOUNT
			IF @StartTranCount = 0 BEGIN TRAN
			INSERT INTO dbo.tbl_CvJobTitle (
				[Name]
				,[Code]
				,[Description]
			)
			VALUES (
				ISNULL(@Name, @Code)
				,@Code
				,ISNULL(@Description, ISNULL(@Name, @Code))
			)
			SET @Id = @@Identity
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
END
GO




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvSocialMediaType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvSocialMediaType]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
19.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvSocialMediaType falls @Code nicht existiert.

Bemerkung:
-

Eingabeparameter:
@Code
@Name
@Description

Ausgabeparameter:
@Id                         Neue erstellte oder vorhandene ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvSocialMediaType]
	@Code [nvarchar](255)
	,@Name [nvarchar](255) = null
	,@Description [nvarchar](255) = null
	,@Id int OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @Id = Id FROM dbo.tbl_CvSocialMediaType WHERE [Code] = @Code
	IF @ID IS NULL BEGIN
		DECLARE @StartTranCount int
		BEGIN TRY
			SET @StartTranCount = @@TRANCOUNT
			IF @StartTranCount = 0 BEGIN TRAN
			INSERT INTO dbo.tbl_CvSocialMediaType (
				[Name]
				,[Code]
				,[Description]
			)
			VALUES (
				ISNULL(@Name, @Code)
				,@Code
				,ISNULL(@Description, ISNULL(@Name, @Code))
			)
			SET @Id = @@Identity
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
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateCvSoftSkillType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateCvSoftSkillType]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
19.02.2016  Franz Egle      Erste Version

Beschreibung:
Erstellt einen Eintrag in der Tabelle tbl_CvSoftSkillType falls @Code nicht existiert.

Bemerkung:
-

Eingabeparameter:
@Code
@Name
@Description

Ausgabeparameter:
@Id                         Neue erstellte oder vorhandene ID

Rückgabewert:
-  
*/
CREATE PROCEDURE [dbo].[CreateCvSoftSkillType]
	@Code [nvarchar](255)
	,@Name [nvarchar](255) = null
	,@Description [nvarchar](255) = null
	,@Id int OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @Id = Id FROM dbo.tbl_CvSoftSkillType WHERE [Code] = @Code
	IF @ID IS NULL BEGIN
		DECLARE @StartTranCount int
		BEGIN TRY
			SET @StartTranCount = @@TRANCOUNT
			IF @StartTranCount = 0 BEGIN TRAN
			INSERT INTO dbo.tbl_CvSoftSkillType (
				[Name]
				,[Code]
				,[Description]
			)
			VALUES (
				ISNULL(@Name, @Code)
				,@Code
				,ISNULL(@Description, ISNULL(@Name, @Code))
			)
			SET @Id = @@Identity
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
END
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteCvProfile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteCvProfile]
GO
/*
================================================================================
(c) Copyright © 2004-2016 
    Datenbank Version: Microsoft SQL Server 2014
================================================================================

Änderungsliste:
Datum       Autor           Änderung
22.01.2016  Franz Egle      Erste Version

Beschreibung:
Löscht einen Eintrag in der Tabelle tbl_CvProfile

Bemerkung:
-

Eingabeparameter:
@ID                         Löscht den Datensatz mit der ID wenn angegeben
@TrxmlID                    Löscht den Datensatz mit der TrxmlID wenn angegeben
@DeleteRelated              Löscht Abhängige Entitäten wenn 1

Ausgabeparameter:
-

Rückgabewert:
@Success                    1 wenn erfolgreich  
*/
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
