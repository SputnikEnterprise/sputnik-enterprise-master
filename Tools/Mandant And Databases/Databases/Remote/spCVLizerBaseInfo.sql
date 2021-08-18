USE [master]
GO

CREATE DATABASE [spCVLizerBaseInfo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'spCVLizerBaseInfo', FILENAME = N'<your path>\spCVLizerBaseInfo.mdf' , SIZE = 70650880KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'spCVLizerBaseInfo_log', FILENAME = N'<your path>\spCVLizerBaseInfo_log.ldf' , SIZE = 2876352KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO


USE [spCVLizerBaseInfo]
GO

CREATE TABLE [dbo].[tbl_Base_CERF](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_CERF_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_CivilState](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_CivilState_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_Contract](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_Contract_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_EducationType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_EducationType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_Employment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_Employment_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_Experience](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_Experience_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_Gender](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_Gender_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_ISCED](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_ISCED_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_ISOCountry](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_ISOCountry_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_ISOLanguage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_ISOLanguage_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_NACE](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_NACE_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_OperationArea](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_OperationArea_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_Position](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_Position_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_Base_Skill](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Bez_DE] [nvarchar](255) NOT NULL,
	[Bez_EN] [nvarchar](255) NOT NULL,
 CONSTRAINT [tbl_Base_Skill_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLAddDrivingLicences](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_AddID] [int] NULL,
	[DrivingLicence] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLAddDrivingLicences_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLAddInternetresources](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_AddID] [int] NULL,
	[URL] [nvarchar](255) NULL,
	[Title] [nvarchar](255) NULL,
	[Source] [nvarchar](255) NULL,
	[Snippet] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLAddInternetresources_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLAdditionalInformations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CVLID] [int] NULL,
	[MilitaryService] [bit] NULL,
	[Competences] [nvarchar](max) NULL,
	[Additionals] [nvarchar](max) NULL,
	[Interests] [nvarchar](max) NULL,
 CONSTRAINT [tbl_CVLAdditionalInformations_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLAddLanguages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_AddID] [int] NULL,
	[FK_LanguageCode] [nvarchar](50) NULL,
	[FK_LanguageLevelCode] [nvarchar](50) NULL,
 CONSTRAINT [tbl_CVLAddLanguages_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLAddress](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PersonalID] [int] NULL,
	[Street] [nvarchar](255) NULL,
	[PostCode] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[FK_CountryCode] [nvarchar](50) NULL,
	[State] [nvarchar](255) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
 CONSTRAINT [tbl_CVLAddress_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLAddUndatedIndustries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_AddID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Weight] [money] NULL,
 CONSTRAINT [tbl_CVLAddUndatedIndustries_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLAddUndatedOperationAreas](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_AddID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Weight] [money] NULL,
 CONSTRAINT [tbl_CVLAddUndatedOperationAreas_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLAddUndatedSkills](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_AddID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Weight] [money] NULL,
 CONSTRAINT [tbl_CVLAddUndatedSkills_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLData_Experiences](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](50) NULL,
	[ProfileID] [int] NULL,
	[Experience_Code] [nvarchar](255) NULL,
	[Experience_in_Month] [int] NULL,
	[Last_experience] [datetime] NULL,
	[Skill] [bit] NULL,
	[OperationArea] [bit] NULL,
	[JobTitel] [bit] NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [tbl_CVLData_Experiences_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLDocuments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CVLID] [int] NULL,
	[DocClass] [nvarchar](50) NULL,
	[Pages] [int] NULL,
	[Plaintext] [nvarchar](max) NULL,
	[FileType] [nvarchar](50) NULL,
	[DocBinary] [varbinary](max) NULL,
	[DocID] [int] NULL,
	[DocSize] [int] NULL,
	[DocLanguage] [nvarchar](50) NULL,
	[FileHashvalue] [nvarchar](max) NULL,
	[DocXML] [nvarchar](max) NULL,
 CONSTRAINT [tbl_CVLDocuments_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLEducation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CVLID] [int] NULL,
	[AdditionalText] [nvarchar](1000) NULL,
 CONSTRAINT [tbl_CVLEducation_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLEducationPhases](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_EducationID] [int] NULL,
	[FK_PhasesID] [int] NULL,
	[FK_IsCedCode] [nvarchar](50) NULL,
	[Completed] [bit] NULL,
	[Score] [int] NULL,
 CONSTRAINT [tbl_CVLEducationPhases_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLEducationType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_EducPhasesID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Weight] [money] NULL,
 CONSTRAINT [tbl_CVLEducationType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLEducPhaseGraduations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_EducPhasesID] [int] NULL,
	[Graduations] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLEducPhaseGraduations_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLEducPhaseSchoolnames](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_EducPhasesID] [int] NULL,
	[Schoolname] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLEducPhaseSchoolnames_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLObjective](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CVLID] [int] NULL,
	[AvailabilityDate] [datetime] NULL,
 CONSTRAINT [tbl_CVLObjective_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLObjPhaseCompanies](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ObjPhaseID] [int] NULL,
	[Company] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLObjPhaseCompanies_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLObjPhaseEmployments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ObjPhaseID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLObjPhaseEmployments_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLObjPhaseFunctions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ObjPhaseID] [int] NULL,
	[Function] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLObjPhaseFunctions_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLObjPhasePositions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ObjPhaseID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLObjPhasePositions_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLObjPhases](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ObjID] [int] NULL,
	[FK_PhasesID] [int] NULL,
	[Project] [bit] NULL,
 CONSTRAINT [tbl_CVLObjPhases_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLObjPhaseWorktimes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ObjPhaseID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLObjPhaseWorktimes_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLObjSalary](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ObjID] [int] NULL,
	[Salary] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLObjSalary_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPersonalCivilstate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PersonalID] [int] NULL,
	[FK_CivilStateCode] [nvarchar](50) NULL,
 CONSTRAINT [tbl_CVLPersonalCivilstate_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPersonalEMails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PersonalID] [int] NULL,
	[EMailAddress] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLPersonalEMails_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPersonalHomepages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PersonalID] [int] NULL,
	[Homepage] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLPersonalHomepages_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPersonalInformation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CVLID] [int] NULL,
	[FirstName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[FK_GenderCode] [nvarchar](50) NULL,
	[FK_IsCedCode] [nvarchar](50) NULL,
	[DateOfBirth] [datetime] NULL,
	[PlaceOfBirth] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLPersonalInformation_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPersonalNationality](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PersonalID] [int] NULL,
	[FK_NationalityCode] [nvarchar](50) NULL,
 CONSTRAINT [tbl_CVLPersonalNationality_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPersonalPhoneNumbers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PersonalID] [int] NULL,
	[PhoneNumber] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLPersonalPhoneNumbers_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPersonalTelefaxNumbers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PersonalID] [int] NULL,
	[TelefaxNumber] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLPersonalTelefaxNumbers_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPersonalTitle](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PersonalID] [int] NULL,
	[Title] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLPersonalTitle_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPhaseCustomCodes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PhasesID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Weight] [money] NULL,
 CONSTRAINT [tbl_CVLPhaseCustomCodes_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPhaseDocumentIDs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PhasesID] [int] NULL,
	[Code] [int] NULL,
 CONSTRAINT [tbl_CVLPhaseDocumentIDs_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPhaseIndustries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PhasesID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Weight] [money] NULL,
 CONSTRAINT [tbl_CVLPhaseIndustries_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPhaseInternetRessources](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PhasesID] [int] NULL,
	[URL] [nvarchar](255) NULL,
	[Title] [nvarchar](255) NULL,
	[Source] [nvarchar](255) NULL,
	[Snippet] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLPhaseInternetRessources_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPhaseLocations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PhasesID] [int] NULL,
	[Street] [nvarchar](255) NULL,
	[PostCode] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[FK_CountryCode] [nvarchar](50) NULL,
	[State] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLPhaseLocations_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPhaseOperationAreas](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PhasesID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Weight] [money] NULL,
 CONSTRAINT [tbl_CVLPhaseOperationAreas_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPhases](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateFrom] [datetime] NULL,
	[DateTo] [datetime] NULL,
	[DateFromFuzzy] [nvarchar](255) NULL,
	[DateToFuzzy] [nvarchar](255) NULL,
	[Duration] [int] NULL,
	[Current] [bit] NULL,
	[SubPhase] [bit] NULL,
	[Comments] [nvarchar](max) NULL,
	[PlainText] [nvarchar](max) NULL,
 CONSTRAINT [tbl_CVLPhases_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPhaseSkills](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PhasesID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Weight] [money] NULL,
 CONSTRAINT [tbl_CVLPhaseSkills_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPhaseSoftSkills](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PhasesID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Weight] [money] NULL,
 CONSTRAINT [tbl_CVLPhaseSoftSkills_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPhaseTopics](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PhasesID] [int] NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLPhaseTopics_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLProfile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedFrom] [nvarchar](255) NOT NULL,
	[CheckedOn] [smalldatetime] NULL,
 CONSTRAINT [tbl_CVLProfile_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPublications](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CVLID] [int] NULL,
	[FK_PhasesID] [int] NULL,
	[Proceedings] [nvarchar](max) NULL,
	[Institute] [nvarchar](max) NULL,
 CONSTRAINT [tbl_CVLPublications_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLPubPhaseAuthors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_PubPhaseID] [int] NULL,
	[Authors] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLPubPhaseAuthors_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLStatistics](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CVLID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
	[Weight] [money] NULL,
	[Duration] [int] NULL,
	[Domain] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLStatistics_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLWork](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CVLID] [int] NULL,
	[AdditionalText] [nvarchar](max) NULL,
 CONSTRAINT [tbl_CVLWork_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLWorkPhaseCompanies](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_WorkPhaseID] [int] NULL,
	[Company] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLWorkPhaseCompanies_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLWorkPhaseEmployments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_WorkPhaseID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLWorkPhaseEmployments_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLWorkPhaseFunctions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_WorkPhaseID] [int] NULL,
	[Function] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLWorkPhaseFunctions_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLWorkPhaseFunctions_Test](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_WorkPhaseID] [int] NULL,
	[Function] [nvarchar](255) NULL
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLWorkPhasePositions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_WorkPhaseID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLWorkPhasePositions_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLWorkPhases](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_WorkID] [int] NULL,
	[FK_PhasesID] [int] NULL,
	[Project] [bit] NULL,
 CONSTRAINT [tbl_CVLWorkPhases_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_CVLWorkPhaseWorktimes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_WorkPhaseID] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CVLWorkPhaseWorktimes_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_ParsedCVLFiles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [nvarchar](50) NULL,
	[FileName] [nvarchar](255) NULL,
	[FileHashvalue] [nvarchar](max) NULL,
	[FileDescription] [nvarchar](255) NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_tbl_ParsedCVLFiles_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_SearchCVLQueryNotifications](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](50) NULL,
	[User_ID] [nvarchar](50) NULL,
	[QueryName] [nvarchar](255) NULL,
	[QueryContent] [nvarchar](max) NULL,
	[QueryResultContent] [nvarchar](max) NULL,
	[Notify] [bit] NULL,
	[CreatedFrom] [nvarchar](255) NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [tbl_SearchCVLQueryNotifications_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




CREATE TABLE [dbo].[tbl_SearchCVLQueryResult](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_SearchQueryID] [int] NOT NULL,
	[CVLProfileID] [int] NULL,
	[PersonalID] [int] NULL,
	[EmployeeID] [int] NULL,
	[Firstname] [nvarchar](255) NULL,
	[Lastname] [nvarchar](255) NULL,
	[Postcode] [nvarchar](20) NULL,
	[Street] [nvarchar](255) NULL,
	[Location] [nvarchar](255) NULL,
	[CountryCode] [nvarchar](2) NULL,
	[DateOfBirth] [datetime] NULL,
	[EmployeeAge] [int] NULL,
	[JobTitel] [nvarchar](255) NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [tbl_SearchCVLQueryResult_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_CVLAddDrivingLicences]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAddDrivingLicences_AddID_FK1] FOREIGN KEY([FK_AddID])
REFERENCES [dbo].[tbl_CVLAdditionalInformations] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLAddDrivingLicences] CHECK CONSTRAINT [tbl_CVLAddDrivingLicences_AddID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLAddInternetresources]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAddInternetresources_AddID_FK1] FOREIGN KEY([FK_AddID])
REFERENCES [dbo].[tbl_CVLAdditionalInformations] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLAddInternetresources] CHECK CONSTRAINT [tbl_CVLAddInternetresources_AddID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLAdditionalInformations]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAdditionalInformations_CVLID_FK1] FOREIGN KEY([FK_CVLID])
REFERENCES [dbo].[tbl_CVLProfile] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLAdditionalInformations] CHECK CONSTRAINT [tbl_CVLAdditionalInformations_CVLID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLAddLanguages]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAddLanguages_AddID_FK1] FOREIGN KEY([FK_AddID])
REFERENCES [dbo].[tbl_CVLAdditionalInformations] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLAddLanguages] CHECK CONSTRAINT [tbl_CVLAddLanguages_AddID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLAddLanguages]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAddLanguages_LanguageCode_FK1] FOREIGN KEY([FK_LanguageCode])
REFERENCES [dbo].[tbl_Base_ISOLanguage] ([Code])
GO

ALTER TABLE [dbo].[tbl_CVLAddLanguages] CHECK CONSTRAINT [tbl_CVLAddLanguages_LanguageCode_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLAddLanguages]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAddLanguages_LanguageLevelCode_FK1] FOREIGN KEY([FK_LanguageLevelCode])
REFERENCES [dbo].[tbl_Base_CERF] ([Code])
GO

ALTER TABLE [dbo].[tbl_CVLAddLanguages] CHECK CONSTRAINT [tbl_CVLAddLanguages_LanguageLevelCode_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLAddress]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAddress_CountryCode_FK1] FOREIGN KEY([FK_CountryCode])
REFERENCES [dbo].[tbl_Base_ISOCountry] ([Code])
GO

ALTER TABLE [dbo].[tbl_CVLAddress] CHECK CONSTRAINT [tbl_CVLAddress_CountryCode_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLAddress]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAddress_PersonalID_FK1] FOREIGN KEY([FK_PersonalID])
REFERENCES [dbo].[tbl_CVLPersonalInformation] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLAddress] CHECK CONSTRAINT [tbl_CVLAddress_PersonalID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLAddUndatedIndustries]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAddUndatedIndustries_AddID_FK1] FOREIGN KEY([FK_AddID])
REFERENCES [dbo].[tbl_CVLAdditionalInformations] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLAddUndatedIndustries] CHECK CONSTRAINT [tbl_CVLAddUndatedIndustries_AddID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLAddUndatedOperationAreas]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAddUndatedOperationAreas_AddID_FK1] FOREIGN KEY([FK_AddID])
REFERENCES [dbo].[tbl_CVLAdditionalInformations] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLAddUndatedOperationAreas] CHECK CONSTRAINT [tbl_CVLAddUndatedOperationAreas_AddID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLAddUndatedSkills]  WITH CHECK ADD  CONSTRAINT [tbl_CVLAddUndatedSkills_AddID_FK1] FOREIGN KEY([FK_AddID])
REFERENCES [dbo].[tbl_CVLAdditionalInformations] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLAddUndatedSkills] CHECK CONSTRAINT [tbl_CVLAddUndatedSkills_AddID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLDocuments]  WITH CHECK ADD  CONSTRAINT [tbl_CVLDocuments_CVLID_FK1] FOREIGN KEY([FK_CVLID])
REFERENCES [dbo].[tbl_CVLProfile] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLDocuments] CHECK CONSTRAINT [tbl_CVLDocuments_CVLID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLEducation]  WITH CHECK ADD  CONSTRAINT [tbl_CVLEducation_CVLID_FK1] FOREIGN KEY([FK_CVLID])
REFERENCES [dbo].[tbl_CVLProfile] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLEducation] CHECK CONSTRAINT [tbl_CVLEducation_CVLID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLEducationPhases]  WITH CHECK ADD  CONSTRAINT [tbl_CVLEducationPhases_EducationID_FK1] FOREIGN KEY([FK_EducationID])
REFERENCES [dbo].[tbl_CVLEducation] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLEducationPhases] CHECK CONSTRAINT [tbl_CVLEducationPhases_EducationID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLEducationPhases]  WITH CHECK ADD  CONSTRAINT [tbl_CVLEducationPhases_IsCedCode_FK1] FOREIGN KEY([FK_IsCedCode])
REFERENCES [dbo].[tbl_Base_ISCED] ([Code])
GO

ALTER TABLE [dbo].[tbl_CVLEducationPhases] CHECK CONSTRAINT [tbl_CVLEducationPhases_IsCedCode_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLEducationPhases]  WITH CHECK ADD  CONSTRAINT [tbl_CVLEducationPhases_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLEducationPhases] CHECK CONSTRAINT [tbl_CVLEducationPhases_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLEducationType]  WITH CHECK ADD  CONSTRAINT [tbl_CVLEducationType_EducPhasesID_FK1] FOREIGN KEY([FK_EducPhasesID])
REFERENCES [dbo].[tbl_CVLEducationPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLEducationType] CHECK CONSTRAINT [tbl_CVLEducationType_EducPhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLEducPhaseGraduations]  WITH CHECK ADD  CONSTRAINT [tbl_CVLEducPhaseGraduations_EducPhasesID_FK1] FOREIGN KEY([FK_EducPhasesID])
REFERENCES [dbo].[tbl_CVLEducationPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLEducPhaseGraduations] CHECK CONSTRAINT [tbl_CVLEducPhaseGraduations_EducPhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLEducPhaseSchoolnames]  WITH CHECK ADD  CONSTRAINT [tbl_CVLEducPhaseSchoolnames_EducPhasesID_FK1] FOREIGN KEY([FK_EducPhasesID])
REFERENCES [dbo].[tbl_CVLEducationPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLEducPhaseSchoolnames] CHECK CONSTRAINT [tbl_CVLEducPhaseSchoolnames_EducPhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLObjective]  WITH CHECK ADD  CONSTRAINT [tbl_CVLObjective_CVLID_FK1] FOREIGN KEY([FK_CVLID])
REFERENCES [dbo].[tbl_CVLProfile] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLObjective] CHECK CONSTRAINT [tbl_CVLObjective_CVLID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLObjPhaseCompanies]  WITH CHECK ADD  CONSTRAINT [tbl_CVLObjPhaseCompanies_ObjPhaseID_FK1] FOREIGN KEY([FK_ObjPhaseID])
REFERENCES [dbo].[tbl_CVLObjPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLObjPhaseCompanies] CHECK CONSTRAINT [tbl_CVLObjPhaseCompanies_ObjPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLObjPhaseEmployments]  WITH CHECK ADD  CONSTRAINT [tbl_CVLObjPhaseEmployments_ObjPhaseID_FK1] FOREIGN KEY([FK_ObjPhaseID])
REFERENCES [dbo].[tbl_CVLObjPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLObjPhaseEmployments] CHECK CONSTRAINT [tbl_CVLObjPhaseEmployments_ObjPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLObjPhaseFunctions]  WITH CHECK ADD  CONSTRAINT [tbl_CVLObjPhaseFunctions_ObjPhaseID_FK1] FOREIGN KEY([FK_ObjPhaseID])
REFERENCES [dbo].[tbl_CVLObjPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLObjPhaseFunctions] CHECK CONSTRAINT [tbl_CVLObjPhaseFunctions_ObjPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLObjPhasePositions]  WITH CHECK ADD  CONSTRAINT [tbl_CVLObjPhasePositions_ObjPhaseID_FK1] FOREIGN KEY([FK_ObjPhaseID])
REFERENCES [dbo].[tbl_CVLObjPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLObjPhasePositions] CHECK CONSTRAINT [tbl_CVLObjPhasePositions_ObjPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLObjPhases]  WITH CHECK ADD  CONSTRAINT [tbl_CVLObjPhases_ObjID_FK1] FOREIGN KEY([FK_ObjID])
REFERENCES [dbo].[tbl_CVLObjective] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLObjPhases] CHECK CONSTRAINT [tbl_CVLObjPhases_ObjID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLObjPhases]  WITH CHECK ADD  CONSTRAINT [tbl_CVLObjPhases_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLObjPhases] CHECK CONSTRAINT [tbl_CVLObjPhases_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLObjPhaseWorktimes]  WITH CHECK ADD  CONSTRAINT [tbl_CVLObjPhaseWorktimes_ObjPhaseID_FK1] FOREIGN KEY([FK_ObjPhaseID])
REFERENCES [dbo].[tbl_CVLObjPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLObjPhaseWorktimes] CHECK CONSTRAINT [tbl_CVLObjPhaseWorktimes_ObjPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLObjSalary]  WITH CHECK ADD  CONSTRAINT [tbl_CVLObjSalary_ObjID_FK1] FOREIGN KEY([FK_ObjID])
REFERENCES [dbo].[tbl_CVLObjective] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLObjSalary] CHECK CONSTRAINT [tbl_CVLObjSalary_ObjID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalCivilstate]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalCivilstate_FK_CivilStateCode_FK1] FOREIGN KEY([FK_CivilStateCode])
REFERENCES [dbo].[tbl_Base_CivilState] ([Code])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalCivilstate] CHECK CONSTRAINT [tbl_CVLPersonalCivilstate_FK_CivilStateCode_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalCivilstate]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalCivilstate_FK_PersonalID_FK1] FOREIGN KEY([FK_PersonalID])
REFERENCES [dbo].[tbl_CVLPersonalInformation] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalCivilstate] CHECK CONSTRAINT [tbl_CVLPersonalCivilstate_FK_PersonalID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalEMails]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalEMails_FK_PersonalID_FK1] FOREIGN KEY([FK_PersonalID])
REFERENCES [dbo].[tbl_CVLPersonalInformation] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalEMails] CHECK CONSTRAINT [tbl_CVLPersonalEMails_FK_PersonalID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalHomepages]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalHomepages_FK_PersonalID_FK1] FOREIGN KEY([FK_PersonalID])
REFERENCES [dbo].[tbl_CVLPersonalInformation] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalHomepages] CHECK CONSTRAINT [tbl_CVLPersonalHomepages_FK_PersonalID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalInformation]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalInformation_CVLID_FK1] FOREIGN KEY([FK_CVLID])
REFERENCES [dbo].[tbl_CVLProfile] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalInformation] CHECK CONSTRAINT [tbl_CVLPersonalInformation_CVLID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalInformation]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalInformation_GenderCode_FK1] FOREIGN KEY([FK_GenderCode])
REFERENCES [dbo].[tbl_Base_Gender] ([Code])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalInformation] CHECK CONSTRAINT [tbl_CVLPersonalInformation_GenderCode_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalInformation]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalInformation_IsCedCode_FK1] FOREIGN KEY([FK_IsCedCode])
REFERENCES [dbo].[tbl_Base_ISCED] ([Code])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalInformation] CHECK CONSTRAINT [tbl_CVLPersonalInformation_IsCedCode_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalNationality]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalNationality_FK_PersonalID_FK1] FOREIGN KEY([FK_PersonalID])
REFERENCES [dbo].[tbl_CVLPersonalInformation] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalNationality] CHECK CONSTRAINT [tbl_CVLPersonalNationality_FK_PersonalID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalNationality]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalNationality_NationalityCode_FK1] FOREIGN KEY([FK_NationalityCode])
REFERENCES [dbo].[tbl_Base_ISOCountry] ([Code])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalNationality] CHECK CONSTRAINT [tbl_CVLPersonalNationality_NationalityCode_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalPhoneNumbers]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalPhoneNumbers_FK_PersonalID_FK1] FOREIGN KEY([FK_PersonalID])
REFERENCES [dbo].[tbl_CVLPersonalInformation] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalPhoneNumbers] CHECK CONSTRAINT [tbl_CVLPersonalPhoneNumbers_FK_PersonalID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalTelefaxNumbers]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalTelefaxNumbers_FK_PersonalID_FK1] FOREIGN KEY([FK_PersonalID])
REFERENCES [dbo].[tbl_CVLPersonalInformation] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalTelefaxNumbers] CHECK CONSTRAINT [tbl_CVLPersonalTelefaxNumbers_FK_PersonalID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPersonalTitle]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPersonalTitle_FK_PersonalID_FK1] FOREIGN KEY([FK_PersonalID])
REFERENCES [dbo].[tbl_CVLPersonalInformation] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPersonalTitle] CHECK CONSTRAINT [tbl_CVLPersonalTitle_FK_PersonalID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPhaseCustomCodes]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPhaseCustomCodes_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPhaseCustomCodes] CHECK CONSTRAINT [tbl_CVLPhaseCustomCodes_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPhaseDocumentIDs]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPhaseDocumentIDs_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPhaseDocumentIDs] CHECK CONSTRAINT [tbl_CVLPhaseDocumentIDs_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPhaseIndustries]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPhaseIndustries_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPhaseIndustries] CHECK CONSTRAINT [tbl_CVLPhaseIndustries_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPhaseInternetRessources]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPhaseInternetRessources_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPhaseInternetRessources] CHECK CONSTRAINT [tbl_CVLPhaseInternetRessources_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPhaseLocations]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPhaseLocations_CountryCode_FK1] FOREIGN KEY([FK_CountryCode])
REFERENCES [dbo].[tbl_Base_ISOCountry] ([Code])
GO

ALTER TABLE [dbo].[tbl_CVLPhaseLocations] CHECK CONSTRAINT [tbl_CVLPhaseLocations_CountryCode_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPhaseLocations]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPhaseLocations_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPhaseLocations] CHECK CONSTRAINT [tbl_CVLPhaseLocations_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPhaseOperationAreas]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPhaseOperationAreas_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPhaseOperationAreas] CHECK CONSTRAINT [tbl_CVLPhaseOperationAreas_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPhaseSkills]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPhaseSkills_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPhaseSkills] CHECK CONSTRAINT [tbl_CVLPhaseSkills_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPhaseSoftSkills]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPhaseSoftSkills_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPhaseSoftSkills] CHECK CONSTRAINT [tbl_CVLPhaseSoftSkills_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPhaseTopics]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPhaseTopics_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPhaseTopics] CHECK CONSTRAINT [tbl_CVLPhaseTopics_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPublications]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPublications_CVLID_FK1] FOREIGN KEY([FK_CVLID])
REFERENCES [dbo].[tbl_CVLProfile] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPublications] CHECK CONSTRAINT [tbl_CVLPublications_CVLID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPublications]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPublications_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPublications] CHECK CONSTRAINT [tbl_CVLPublications_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLPubPhaseAuthors]  WITH CHECK ADD  CONSTRAINT [tbl_CVLPubPhaseAuthors_FK_PubPhaseID_FK1] FOREIGN KEY([FK_PubPhaseID])
REFERENCES [dbo].[tbl_CVLPublications] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLPubPhaseAuthors] CHECK CONSTRAINT [tbl_CVLPubPhaseAuthors_FK_PubPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLStatistics]  WITH CHECK ADD  CONSTRAINT [tbl_CVLStatistics_CVLID_FK1] FOREIGN KEY([FK_CVLID])
REFERENCES [dbo].[tbl_CVLProfile] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLStatistics] CHECK CONSTRAINT [tbl_CVLStatistics_CVLID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLWork]  WITH CHECK ADD  CONSTRAINT [tbl_CVLWork_CVLID_FK1] FOREIGN KEY([FK_CVLID])
REFERENCES [dbo].[tbl_CVLProfile] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLWork] CHECK CONSTRAINT [tbl_CVLWork_CVLID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhaseCompanies]  WITH CHECK ADD  CONSTRAINT [tbl_CVLWorkPhaseCompanies_WorkPhaseID_FK1] FOREIGN KEY([FK_WorkPhaseID])
REFERENCES [dbo].[tbl_CVLWorkPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhaseCompanies] CHECK CONSTRAINT [tbl_CVLWorkPhaseCompanies_WorkPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhaseEmployments]  WITH CHECK ADD  CONSTRAINT [tbl_CVLWorkPhaseEmployments_WorkPhaseID_FK1] FOREIGN KEY([FK_WorkPhaseID])
REFERENCES [dbo].[tbl_CVLWorkPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhaseEmployments] CHECK CONSTRAINT [tbl_CVLWorkPhaseEmployments_WorkPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhaseFunctions]  WITH CHECK ADD  CONSTRAINT [tbl_CVLWorkPhaseFunctions_WorkPhaseID_FK1] FOREIGN KEY([FK_WorkPhaseID])
REFERENCES [dbo].[tbl_CVLWorkPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhaseFunctions] CHECK CONSTRAINT [tbl_CVLWorkPhaseFunctions_WorkPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhasePositions]  WITH CHECK ADD  CONSTRAINT [tbl_CVLWorkPhasePositions_WorkPhaseID_FK1] FOREIGN KEY([FK_WorkPhaseID])
REFERENCES [dbo].[tbl_CVLWorkPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhasePositions] CHECK CONSTRAINT [tbl_CVLWorkPhasePositions_WorkPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhases]  WITH CHECK ADD  CONSTRAINT [tbl_CVLWorkPhases_PhasesID_FK1] FOREIGN KEY([FK_PhasesID])
REFERENCES [dbo].[tbl_CVLPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhases] CHECK CONSTRAINT [tbl_CVLWorkPhases_PhasesID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhases]  WITH CHECK ADD  CONSTRAINT [tbl_CVLWorkPhases_WorkID_FK1] FOREIGN KEY([FK_WorkID])
REFERENCES [dbo].[tbl_CVLWork] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhases] CHECK CONSTRAINT [tbl_CVLWorkPhases_WorkID_FK1]
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhaseWorktimes]  WITH CHECK ADD  CONSTRAINT [tbl_CVLWorkPhaseWorktimes_WorkPhaseID_FK1] FOREIGN KEY([FK_WorkPhaseID])
REFERENCES [dbo].[tbl_CVLWorkPhases] ([ID])
GO

ALTER TABLE [dbo].[tbl_CVLWorkPhaseWorktimes] CHECK CONSTRAINT [tbl_CVLWorkPhaseWorktimes_WorkPhaseID_FK1]
GO

ALTER TABLE [dbo].[tbl_SearchCVLQueryResult]  WITH CHECK ADD  CONSTRAINT [tbl_SearchCVLQueryResult_FK_SearchQueryID_FK1] FOREIGN KEY([FK_SearchQueryID])
REFERENCES [dbo].[tbl_SearchCVLQueryNotifications] ([ID])
GO

ALTER TABLE [dbo].[tbl_SearchCVLQueryResult] CHECK CONSTRAINT [tbl_SearchCVLQueryResult_FK_SearchQueryID_FK1]
GO



/* ----------------- end of creating table -------------------------------- */


/* ----------------- create functions ------------------------------------- */

USE [spCVLizerBaseInfo]
GO


CREATE FUNCTION [dbo].[uf_LoadApplicationDataWithProfileID] 
	(
	@ProfileID INT
	)  

RETURNS @ApplicationData  table
(
    ApplicationID   INT ,
    ApplicantID   INT,
    CVLProfilID     int,
    CVLPersonalID   INT ,
    ApplicationLabel   nvarchar(255) ,
    Customer_ID   nvarchar(50) ,
    CreatedOn   datetime 
   )
AS

BEGIN

DECLARE @applicantID int
DECLARE @applicationLabel NVARCHAR(255)
DECLARE @CustomerID NVARCHAR(50)
DECLARE @Applicationid INT
DECLARE @CVLPersonalID INT
DECLARE @CreatedOn DATETIME
 
SELECT @applicantID = ID  FROM [applicant].dbo.tbl_applicant WHERE CVLProfileID = @ProfileID
SELECT @Applicationid = ID, @CustomerID = Customer_ID,  @applicationLabel = ApplicationLabel, @CreatedOn = CreatedOn FROM [applicant].dbo.tbl_application WHERE FK_ApplicantID = @applicantID
SELECT @CVLPersonalID = ID FROM dbo.tbl_CVLPersonalInformation WHERE FK_CVLID = @ProfileID

		INSERT @ApplicationData
			SELECT @Applicationid , 
			@applicantID ,
			@ProfileID ,
			@CVLPersonalID ,
			@applicationLabel ,
			@CustomerID ,
			@CreatedOn

		RETURN
END;
GO


CREATE FUNCTION [dbo].[uf_LoadCVLProfileDataWithApplicationID] 
	(
	@Applicationid INT
	)  

RETURNS @CLVProfilData  TABLE
(
    ApplicationID     INT,
    ApplicantID   INT,
    CVLProfilID   INT ,
    CVLPersonalID   INT ,
    ApplicationLabel   NVARCHAR(255) ,
    Customer_ID   NVARCHAR(50) ,
    CreatedOn   DATETIME 
   )
AS

BEGIN

DECLARE @applicantID int
DECLARE @applicationLabel NVARCHAR(255)
DECLARE @CustomerID NVARCHAR(50)
DECLARE @CVLProfileID INT
DECLARE @CVLPersonalID INT
DECLARE @CreatedOn DATETIME
 
SELECT @applicantID = fk_ApplicantID, @CustomerID = Customer_ID,  @applicationLabel = ApplicationLabel, @CreatedOn = CreatedOn FROM [applicant].dbo.tbl_application WHERE ID = @Applicationid
SELECT @CVLProfileID = CVLProfileID FROM [applicant].dbo.tbl_applicant WHERE ID = @applicantID
SELECT @CVLPersonalID = ID FROM dbo.tbl_CVLPersonalInformation WHERE FK_CVLID = @CVLProfileID

		INSERT @CLVProfilData
			SELECT @Applicationid, 
			@applicantID , 
			@CVLProfileID ,
			@CVLPersonalID ,
			@applicationLabel ,
			@CustomerID ,
			@CreatedOn

		RETURN
END;
GO



CREATE FUNCTION [dbo].[DelimitedSplit8K]
--===== Define I/O parameters
        (@pString VARCHAR(8000), @pDelimiter CHAR(1))
--WARNING!!! DO NOT USE MAX DATA-TYPES HERE!  IT WILL KILL PERFORMANCE!
RETURNS TABLE WITH SCHEMABINDING AS
 RETURN
--===== "Inline" CTE Driven "Tally Table" produces values from 1 up to 10,000...
     -- enough to cover VARCHAR(8000)
  WITH E1(N) AS (
                 SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL
                 SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL
                 SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1 UNION ALL SELECT 1
                ),                          --10E+1 or 10 rows
       E2(N) AS (SELECT 1 FROM E1 a, E1 b), --10E+2 or 100 rows
       E4(N) AS (SELECT 1 FROM E2 a, E2 b), --10E+4 or 10,000 rows max
 cteTally(N) AS (--==== This provides the "base" CTE and limits the number of rows right up front
                     -- for both a performance gain and prevention of accidental "overruns"
                 SELECT TOP (ISNULL(DATALENGTH(@pString),0)) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) FROM E4
                ),
cteStart(N1) AS (--==== This returns N+1 (starting position of each "element" just once for each delimiter)
                 SELECT 1 UNION ALL
                 SELECT t.N+1 FROM cteTally t WHERE SUBSTRING(@pString,t.N,1) = @pDelimiter
                ),
cteLen(N1,L1) AS(--==== Return start and length (for use in substring)
                 SELECT s.N1,
                        ISNULL(NULLIF(CHARINDEX(@pDelimiter,@pString,s.N1),0)-s.N1,8000)
                   FROM cteStart s
                )
--===== Do the actual split. The ISNULL/NULLIF combo handles the length for the final element when no delimiter is found.
 SELECT ItemNumber = ROW_NUMBER() OVER(ORDER BY l.N1),
        Item       = SUBSTRING(@pString, l.N1, l.L1)
   FROM cteLen l
;
GO




CREATE FUNCTION [dbo].[uf_App_CivilstateCode] ( @Code INT )
RETURNS NVARCHAR(max)
AS
    BEGIN
        DECLARE @Return NVARCHAR(255) = ''
				DECLARE @value NVARCHAR(50) = ''

        IF ISNULL(@Code, 0) = 0
					BEGIN
						SET @value = ''
					END
				IF @code = 1
					BEGIN
						SET @value = 'm'
					END
				IF @code = 2
					BEGIN
						SET @value = 's'
					END
				IF @code = 4
					BEGIN
						SET @value = 'w'
					END
				IF @code = 5
					BEGIN
						SET @value = 'd'
					END
				IF @code = 6
					BEGIN
						SET @value = 'p'
					END
        
				SET @Return = @value
        
        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_App_CivilstateLabel] ( @Code INT )
RETURNS NVARCHAR(max)
AS
    BEGIN
        DECLARE @Return NVARCHAR(255) = ''
				DECLARE @value NVARCHAR(50) = ''

        IF ISNULL(@Code, 0) = 0
					BEGIN
						SET @value = ''
					END
				IF @code = 1
					BEGIN
						SET @value = 'm'
					END
				IF @code = 2
					BEGIN
						SET @value = 's'
					END
				IF @code = 4
					BEGIN
						SET @value = 'w'
					END
				IF @code = 5
					BEGIN
						SET @value = 'd'
					END
				IF @code = 6
					BEGIN
						SET @value = 'p'
					END
        
				SET @Return = ISNULL(( SELECT TOP (1)
                                    Bez_DE
                             FROM   dbo.[tbl_Base_CivilState]
                             WHERE  ( Code = @value)
                           ORDER BY Code), '')
        
        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_CedLable] ( @Code NVARCHAR(50) )
RETURNS NVARCHAR(max)
AS
    BEGIN
        DECLARE @Return NVARCHAR(max) = ''
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN ''
            END
        SET @Return = ISNULL(( SELECT TOP 1
                                    Bez_DE
                             FROM   dbo.[tbl_Base_ISCED]
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), '')
        
        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_CivilstateLable] ( @Code NVARCHAR(50) )
RETURNS NVARCHAR(max)
AS
    BEGIN
        DECLARE @Return NVARCHAR(max) = ''
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN ''
            END
        SET @Return = ISNULL(( SELECT TOP 1
                                    Bez_DE
                             FROM   dbo.[tbl_Base_CivilState]
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), '')
        
        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_CountryCode] ( @label NVARCHAR(255) )
RETURNS NVARCHAR(3)
AS
    BEGIN
        DECLARE @Return NVARCHAR(3) = @label
        IF ISNULL(@label, '') = ''
            BEGIN 
                RETURN ''
            END
        SET @Return = ISNULL(( SELECT TOP (1)
                                    Code
                             FROM   [spCVLizerBaseInfo].dbo.[tbl_Base_ISOCountry]
                             WHERE  ( @label = ''
                                      OR [Bez_DE] = @label OR [Bez_EN] = @label
                                    ) ORDER BY Code
                           ), '')
        
        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_CountryLable] ( @Code NVARCHAR(50) )
RETURNS NVARCHAR(max)
AS
    BEGIN
        DECLARE @Return NVARCHAR(max) = ''
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN ''
            END
        SET @Return = ISNULL(( SELECT TOP 1
                                    Bez_DE
                             FROM   dbo.[tbl_Base_ISOCountry]
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), '')
        
        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_GenderLable] ( @Code NVARCHAR(50) )
RETURNS NVARCHAR(max)
AS
    BEGIN
        DECLARE @Return NVARCHAR(max) = ''
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN ''
            END
        SET @Return = ISNULL(( SELECT TOP 1
                                    Bez_DE
                             FROM   dbo.tbl_Base_Gender
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), '')
        
        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_LanguageLable] ( @Code NVARCHAR(50) )
RETURNS NVARCHAR(max)
AS
    BEGIN
        DECLARE @Return NVARCHAR(max) = ''
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN ''
            END
        SET @Return = ISNULL(( SELECT TOP 1
                                    Bez_DE
                             FROM   dbo.[tbl_Base_ISOLanguage]
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), '')
        
        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_LanguageLevelLable] ( @Code NVARCHAR(50) )
RETURNS NVARCHAR(max)
AS
    BEGIN
        DECLARE @Return NVARCHAR(max) = ''
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN ''
            END
        SET @Return = ISNULL(( SELECT TOP 1
                                    Bez_DE
                             FROM   dbo.[tbl_Base_CERF]
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), '')
        
        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_ValidateCedCode] ( @Code NVARCHAR(50) )
RETURNS INT
AS
    BEGIN
        DECLARE @Return INT = 1
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN 1
            END
        SET @Code = ISNULL(( SELECT TOP 1
                                    Code
                             FROM   dbo.[tbl_Base_ISCED]
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), 'not founded')
        IF @Code = 'not founded'
            BEGIN
                RETURN 0
            END 

        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_ValidateCivilstateCode] ( @Code NVARCHAR(50) )
RETURNS INT
AS
    BEGIN
        DECLARE @Return INT = 1
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN 1
            END
        SET @Code = ISNULL(( SELECT TOP 1
                                    Code
                             FROM   dbo.[tbl_Base_CivilState]
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), 'not founded')
        IF @Code = 'not founded'
            BEGIN
                RETURN 0
            END 

        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_ValidateCountryCode] ( @Code NVARCHAR(50) )
RETURNS INT
AS
    BEGIN
        DECLARE @Return INT = 1
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN 1
            END
        SET @Code = ISNULL(( SELECT TOP 1
                                    Code
                             FROM   dbo.[tbl_Base_ISOCountry]
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), 'not founded')
        IF @Code = 'not founded'
            BEGIN
                RETURN 0
            END 

        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_ValidateGenderCode] ( @Code NVARCHAR(50) )
RETURNS INT
AS
    BEGIN
        DECLARE @Return INT = 1
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN 1
            END
        SET @Code = ISNULL(( SELECT TOP 1
                                    Code
                             FROM   dbo.tbl_Base_Gender
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), 'not founded')
        IF @Code = 'not founded'
            BEGIN
                RETURN 0
            END 

        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_ValidateLanguageCode] ( @Code NVARCHAR(50) )
RETURNS INT
AS
    BEGIN
        DECLARE @Return INT = 1
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN 1
            END
        SET @Code = ISNULL(( SELECT TOP 1
                                    Code
                             FROM   dbo.[tbl_Base_ISOLanguage]
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), 'not founded')
        IF @Code = 'not founded'
            BEGIN
                RETURN 0
            END 

        RETURN @Return
    END
GO


CREATE FUNCTION [dbo].[uf_CLV_ValidateLanguageLevelCode] ( @Code NVARCHAR(50) )
RETURNS INT
AS
    BEGIN
        DECLARE @Return INT = 1
        IF ISNULL(@Code, '') = ''
            BEGIN 
                RETURN 1
            END
        SET @Code = ISNULL(( SELECT TOP 1
                                    Code
                             FROM   dbo.[tbl_Base_CERF]
                             WHERE  ( @Code = ''
                                      OR Code = @Code
                                    )
                           ), 'not founded')
        IF @Code = 'not founded'
            BEGIN
                RETURN 0
            END 

        RETURN @Return
    END
GO


/* ------------------- end of function -------------------------------*/



USE [spCVLizerBaseInfo]
GO

CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Data For Notification]
    @CVLProfileID INT ,
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  Personal.ID ,
            Personal.FK_CVLID ,
            Personal.FirstName ,
            Personal.LastName ,
            Personal.FK_GenderCode , dbo.uf_CLV_GenderLable(FK_GenderCode) GenderLabel, 
            Personal.FK_IsCedCode , dbo.uf_CLV_CedLable(FK_IsCedCode) IsCedLable, 
            Personal.DateOfBirth ,
            Personal.PlaceOfBirth 
    FROM    dbo.tbl_CVLPersonalInformation Personal
    WHERE   Personal.FK_CVLID = @CVLProfileID AND Personal.ID = @PersonalID;
	
END;
GO

Create PROCEDURE [dbo].[Load Assigned CVL Personal Address Data For Notification]
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID, 
          FK_PersonalID ,
          Street ,
          PostCode ,
          City ,
          FK_CountryCode ,
          State
		From dbo.tbl_CVLAddress
    WHERE   FK_PersonalID = @PersonalID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Title Data For Notification] 
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PersonalID ,
            Title
    FROM    dbo.tbl_CVLPersonalTitle
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Nationality Data For Notification] 
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PersonalID ,
            FK_NationalityCode, dbo.uf_CLV_CountryLable(FK_NationalityCode) NationalityCodeLable
    FROM    dbo.tbl_CVLPersonalNationality
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal CivilState Data For Notification] 
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PersonalID ,
            [FK_CivilStateCode], dbo.uf_CLV_CivilstateLable([FK_CivilStateCode]) CivilStateCodeLable
    FROM    dbo.tbl_CVLPersonalCivilstate
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal EMail Data For Notification] 
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PersonalID ,
            EMailAddress
    FROM    dbo.tbl_CVLPersonalEMails
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Homepage Data For Notification] 
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PersonalID ,
            Homepage
    FROM    dbo.tbl_CVLPersonalHomepages
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;

END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Telefonnumber Data For Notification] 
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PersonalID ,
            PhoneNumber
    FROM    dbo.tbl_CVLPersonalPhoneNumbers
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Telefaxnumber Data For Notification] 
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PersonalID ,
            TelefaxNumber
    FROM    dbo.tbl_CVLPersonalTelefaxNumbers
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Data For Notification]
    @CVLProfileID INT ,
	@WorkID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID, FK_CVLID, AdditionalText
    FROM    dbo.tbl_CVLWork
    WHERE   ID = @WorkID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL WorkPhase Data For Notification]
	@WorkID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  WPhase.ID ,
            WPhase.FK_WorkID ,
            WPhase.FK_PhasesID ,
			WPhase.Project 
    FROM    dbo.tbl_CVLWorkPhases WPhase
    WHERE   WPhase.FK_WorkID = @WorkID
	ORDER BY WPhase.ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Company Data For Notification]
	@WorkPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_WorkPhaseID ,
            Company
    FROM    dbo.tbl_CVLWorkPhaseCompanies
    WHERE   FK_WorkPhaseID = @WorkPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Function Data For Notification]
	@WorkPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_WorkPhaseID ,
            [Function]
    FROM    dbo.tbl_CVLWorkPhaseFunctions
    WHERE   FK_WorkPhaseID = @WorkPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Position Data For Notification]
	@WorkPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_WorkPhaseID ,
            Code, Name 
    FROM    dbo.tbl_CVLWorkPhasePositions
    WHERE   FK_WorkPhaseID = @WorkPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Employment Data For Notification]
	@WorkPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_WorkPhaseID ,
            Code, Name 
    FROM    dbo.tbl_CVLWorkPhaseEmployments
    WHERE   FK_WorkPhaseID = @WorkPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Worktime Data For Notification]
	@WorkPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_WorkPhaseID ,
            Code, Name 
    FROM    dbo.tbl_CVLWorkPhaseWorktimes
    WHERE   FK_WorkPhaseID = @WorkPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL EducationType Data For Notification]
	@educationID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_EducPhasesID ,
            Code ,
            Name ,
            Weight 
    FROM    dbo.tbl_CVLEducationType 
    WHERE   FK_EducPhasesID = @educationID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Education Schoolname Data For Notification]
	@educationID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_EducPhasesID ,
            Schoolname 
    FROM    dbo.tbl_CVLEducPhaseSchoolnames 
    WHERE   FK_EducPhasesID = @educationID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Education Graduation Data For Notification]
	@educationID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_EducPhasesID ,
            Graduations 
    FROM    dbo.tbl_CVLEducPhaseGraduations 
    WHERE   FK_EducPhasesID = @educationID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Publication Data For Notification]
    @CVLProfileID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            FK_PhasesID ,
            Proceedings , 
			Institute
    FROM    dbo.tbl_CVLPublications
    WHERE   FK_CVLID = @CVLProfileID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Publication Authors Data For Notification]
    @publicationID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PubPhaseID ,
            Authors 
    FROM    dbo.tbl_CVLPubPhaseAuthors 
    WHERE   FK_PubPhaseID = @publicationID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional Information Data For Notification]
    @CVLProfileID INT ,
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            MilitaryService ,
            Competences ,
            Additionals ,
            Interests 
    FROM    dbo.tbl_CVLAdditionalInformations
    WHERE   FK_CVLID = @CVLProfileID
	AND ID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional DrivingLicence Data For Notification]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            DrivingLicence
    FROM    dbo.tbl_CVLAddDrivingLicences
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional Undated Skill Data For Notification]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLAddUndatedSkills
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional Undated OperationArea Data For Notification]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLAddUndatedOperationAreas
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional Undated Industry Data For Notification]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLAddUndatedIndustries
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional InternetResource Data For Notification]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            URL ,
            Title ,
            Source ,
            Snippet
    FROM    dbo.tbl_CVLAddInternetresources
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional Language Data For Notification]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            FK_LanguageCode ,
            FK_LanguageLevelCode 
    FROM    dbo.tbl_CVLAddLanguages
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Objective Data For Notification]
    @CVLProfileID INT ,
	@ObjID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            AvailabilityDate 
    FROM    dbo.tbl_CVLObjective
    WHERE   FK_CVLID = @CVLProfileID
	And ID = @ObjID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Objective Salary Data For Notification]
	@objID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_ObjID ,
            Salary
    FROM    dbo.tbl_CVLObjSalary
    WHERE   FK_ObjID = @objID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Objective Phase Data For Notification]
	@objID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_ObjID ,
            FK_PhasesID ,
			Project 
    FROM    dbo.tbl_CVLObjPhases
    WHERE   FK_ObjID = @objID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Objective Company Data For Notification]
	@ObjPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_ObjPhaseID ,
            Company
    FROM    dbo.tbl_CVLObjPhaseCompanies
    WHERE   FK_ObjPhaseID = @ObjPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Objective Function Data For Notification]
	@ObjPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_ObjPhaseID ,
            [Function]
    FROM    dbo.tbl_CVLObjPhaseFunctions
    WHERE   FK_ObjPhaseID = @ObjPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Objective Position Data For Notification]
	@ObjPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_ObjPhaseID ,
            Code, Name 
    FROM    dbo.tbl_CVLObjPhasePositions
    WHERE   FK_ObjPhaseID = @ObjPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Objective Employment Data For Notification]
	@ObjPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_ObjPhaseID ,
            Code, Name 
    FROM    dbo.tbl_CVLObjPhaseEmployments
    WHERE   FK_ObjPhaseID = @ObjPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Objective Worktime Data For Notification]
	@ObjPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_ObjPhaseID ,
            Code, Name 
    FROM    dbo.tbl_CVLObjPhaseWorktimes
    WHERE   FK_ObjPhaseID = @ObjPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Statistic Data For Notification]
    @CVLProfileID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            Code ,
            Name , 
			Weight , 
			Duration ,
			Domain
    FROM    dbo.tbl_CVLStatistics
    WHERE   FK_CVLID = @CVLProfileID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Data For Notification]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID , 
	DateFrom, 
	DateTo, 
	DateFromFuzzy, 
	DateToFuzzy, 
	Duration, 
	[Current], 
	SubPhase, 
	Comments, 
	PlainText
    FROM    dbo.tbl_CVLPhases 
    WHERE   ID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Location Data For Notification]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Street ,
            PostCode ,
            City ,
            FK_CountryCode , 
            State
    FROM    dbo.tbl_CVLPhaseLocations
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Skill Data For Notification] 
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLPhaseSkills
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase SoftSkill Data For Notification] 
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLPhaseSoftSkills
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase OperationArea Data For Notification]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLPhaseOperationAreas
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Industry Data For Notification]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLPhaseIndustries
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase CustomCode Data For Notification]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLPhaseCustomCodes
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Topic Data For Notification]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Name
    FROM    dbo.tbl_CVLPhaseTopics
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase InternetResource Data For Notification]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            URL ,
            Title ,
            Source ,
            Snippet
    FROM    dbo.tbl_CVLPhaseInternetRessources
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase DocumentID Data For Notification]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code
    FROM    dbo.tbl_CVLPhaseDocumentIDs
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[CreateCVLProfile]
    @CustomerID [NVARCHAR](50) ,
    @Firstname [NVARCHAR](255) ,
    @Lastname [NVARCHAR](255) ,
    @GenderCode [NVARCHAR](50) ,
    @IsCedCode [NVARCHAR](50) ,
    @DateOfBirth Datetime ,
    @PlaceOfBirth [NVARCHAR](255) ,
    @NationalityCode [NVARCHAR](max) ,
    @CivilStatusCode [NVARCHAR](max) ,

    @Titles [NVARCHAR](max) ,
    @PhoneNumbers [NVARCHAR](max) ,
    @TelefaxNumbers [NVARCHAR](max) ,
    @EMails [NVARCHAR](max) ,
    @Homepages [NVARCHAR](max) ,

	@Street [nvarchar](255) NULL,
	@PostCode [nvarchar](255) NULL,
	@City [nvarchar](255) NULL,
	@CountryCode [nvarchar](50) NULL,
	@AddressState [nvarchar](255) NULL,
	@WorkAdditionalText [nvarchar](max) NULL,
	@EducationAdditionalText [nvarchar](max) NULL,

    @NewCVLId INT OUTPUT ,
    @NewPersonalId INT OUTPUT,
    @NewWorkId INT OUTPUT,
    @NewEducationId INT OUTPUT

AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

-- insert profile
		INSERT INTO dbo.tbl_CVLProfile
		        ( Customer_ID ,
		          CreatedOn ,
		          CreatedFrom
		        )
		VALUES  ( @customerid , -- Customer_ID - nvarchar(50)
		          GETDATE() , -- CreatedOn - datetime
		          N'System'  -- CreatedFrom - nvarchar(255)
		        ) 
				
		SET @NewCVLId = @@Identity

IF dbo.uf_CLV_ValidateGenderCode(@GenderCode) = 0 
BEGIN
	SET @GenderCode = NULL 
END
IF dbo.uf_CLV_ValidateCedCode(@IsCedCode) = 0 
BEGIN
	SET @IsCedCode = NULL
END
IF dbo.uf_CLV_ValidateCountryCode(@CountryCode) = 0 
BEGIN
	SET @CountryCode = NULL
END

-- insert personalinformation
		INSERT INTO dbo.tbl_CVLPersonalInformation
		        ( FK_CVLID ,
		          FirstName ,
		          LastName ,
		          FK_GenderCode ,
		          FK_IsCedCode ,
		          DateOfBirth ,
		          PlaceOfBirth 
		        )
		VALUES  ( @NewCVLId , -- FK_CVLID - int
		          @FirstName , -- FirstName - nvarchar(255)
		          @LastName , -- LastName - nvarchar(255)
		          @GenderCode , -- FK_GenderCode - nvarchar(50)
		          @IsCedCode , -- FK_IsCedCode - nvarchar(50)
		          @DateOfBirth , -- DateOfBirth - datetime
		          @PlaceOfBirth  -- PlaceOfBirth - nvarchar(255)
		        )

		SET @NewPersonalId = @@Identity


-- insert Work
INSERT INTO dbo.tbl_CVLWork
        ( FK_CVLID, AdditionalText )
VALUES  ( @NewCVLId, -- FK_CVLID - int
          @WorkAdditionalText  -- AdditionalText - nvarchar(1000)
          )
						
		SET @NewWorkId = @@Identity

-- insert education
INSERT INTO dbo.tbl_CVLEducation
        ( FK_CVLID, AdditionalText )
VALUES  ( @NewCVLId, -- FK_CVLID - int
          @EducationAdditionalText  -- AdditionalText - nvarchar(1000)
          )
						
		SET @NewEducationId = @@Identity



DECLARE @nr varchar(max), @Pos int

-- insert @NationalityCode
SET @NationalityCode = LTRIM(RTRIM(@NationalityCode))+ ','
SET @Pos = CHARINDEX(',', @NationalityCode, 1)

IF REPLACE(@NationalityCode, ',', '') <> ''
    BEGIN
        WHILE @Pos > 0
            BEGIN
                SET @nr = LTRIM(RTRIM(LEFT(@NationalityCode, @Pos - 1)));
                IF @nr <> ''
                    BEGIN
                        IF dbo.uf_CLV_ValidateCountryCode(@nr) = 0
                            BEGIN
                                SET @nr = '';
                            END;

                        IF @nr <> ''
                            BEGIN
                                INSERT  INTO [dbo].[tbl_CVLPersonalNationality]
                                        ( [FK_PersonalID] ,
                                          [FK_NationalityCode]
			                            )
                                VALUES  ( @NewPersonalId , -- FK_PersonalID - int
                                          @nr  -- FK_NationalityCode - nvarchar(50)
			                            );
                            END;
                    END;
                SET @NationalityCode = RIGHT(@NationalityCode,
                                             LEN(@NationalityCode) - @Pos);
                SET @Pos = CHARINDEX(',', @NationalityCode, 1);

            END;
    END;	

-- insert @NationalityCode
SET @CivilStatusCode = LTRIM(RTRIM(@CivilStatusCode))+ ','
SET @Pos = CHARINDEX(',', @CivilStatusCode, 1)

IF REPLACE(@CivilStatusCode, ',', '') <> ''
    BEGIN
        WHILE @Pos > 0
            BEGIN
                SET @nr = LTRIM(RTRIM(LEFT(@CivilStatusCode, @Pos - 1)));
                IF @nr <> ''
                    BEGIN
                        IF dbo.uf_CLV_ValidateCivilstateCode(@nr) = 0
                            BEGIN
                                SET @nr = '';
                            END;
                        IF @nr <> ''
                            BEGIN

                                INSERT  INTO [dbo].[tbl_CVLPersonalCivilstate]
                                        ( [FK_PersonalID] ,
                                          [FK_CivilStateCode]
			                            )
                                VALUES  ( @NewPersonalId , -- FK_PersonalID - int
                                          @nr  -- FK_CivilStateCode - nvarchar(50)
			                            );
                            END;
                    END;
                SET @CivilStatusCode = RIGHT(@CivilStatusCode,
                                             LEN(@CivilStatusCode) - @Pos);
                SET @Pos = CHARINDEX(',', @CivilStatusCode, 1);

            END;
    END;	

-- insert Titles
SET @Titles = LTRIM(RTRIM(@Titles))+ ','
SET @Pos = CHARINDEX(',', @Titles, 1)

IF REPLACE(@Titles, ',', '') <> ''
    BEGIN
        WHILE @Pos > 0
            BEGIN
                SET @nr = LTRIM(RTRIM(LEFT(@Titles, @Pos - 1)));
                IF @nr <> ''
                    BEGIN
                        INSERT  INTO [dbo].[tbl_CVLPersonalTitle]
                                ( [FK_PersonalID], [Title] )
                        VALUES  ( @NewPersonalId, -- FK_PersonalID - int
                                  @nr  -- Title - nvarchar(255)
                                  ); 
                    END;
                SET @Titles = RIGHT(@Titles, LEN(@Titles) - @Pos);
                SET @Pos = CHARINDEX(',', @Titles, 1);

            END;
    END;	

-- insert phone numbers
SET @PhoneNumbers = LTRIM(RTRIM(@PhoneNumbers))+ ','
SET @Pos = CHARINDEX(',', @PhoneNumbers, 1)

IF REPLACE(@PhoneNumbers, ',', '') <> ''
    BEGIN
        WHILE @Pos > 0
            BEGIN
                SET @nr = LTRIM(RTRIM(LEFT(@PhoneNumbers, @Pos - 1)));
                IF @nr <> ''
                    BEGIN
                        INSERT  INTO [dbo].[tbl_CVLPersonalPhoneNumbers]
                                ( [FK_PersonalID], [PhoneNumber] )
                        VALUES  ( @NewPersonalId, -- FK_PersonalID - int
                                  @nr );  -- PhoneNumber - nvarchar(255)
                    END;
                SET @PhoneNumbers = RIGHT(@PhoneNumbers,
                                          LEN(@PhoneNumbers) - @Pos);
                SET @Pos = CHARINDEX(',', @PhoneNumbers, 1);

            END;
    END;	

-- insert telefax numbers
SET @TelefaxNumbers = LTRIM(RTRIM(@TelefaxNumbers))+ ','
SET @Pos = CHARINDEX(',', @TelefaxNumbers, 1)

IF REPLACE(@TelefaxNumbers, ',', '') <> ''
    BEGIN
        WHILE @Pos > 0
            BEGIN
                SET @nr = LTRIM(RTRIM(LEFT(@TelefaxNumbers, @Pos - 1)));
                IF @nr <> ''
                    BEGIN
                        INSERT  INTO [dbo].[tbl_CVLPersonalTelefaxNumbers]
                                ( [FK_PersonalID] ,
                                  [TelefaxNumber]
			                    )
                        VALUES  ( @NewPersonalId , -- FK_PersonalID - int
                                  @nr  -- TelefaxNumber - nvarchar(255)
			                    );
                    END;
                SET @TelefaxNumbers = RIGHT(@TelefaxNumbers,
                                            LEN(@TelefaxNumbers) - @Pos);
                SET @Pos = CHARINDEX(',', @TelefaxNumbers, 1);

            END;
    END;	


-- insert emails 
SET @EMails = LTRIM(RTRIM(@EMails))+ ','
SET @Pos = CHARINDEX(',', @EMails, 1)

IF REPLACE(@EMails, ',', '') <> ''
    BEGIN
        WHILE @Pos > 0
            BEGIN
                SET @nr = LTRIM(RTRIM(LEFT(@EMails, @Pos - 1)));
                IF @nr <> ''
                    BEGIN
                        INSERT  INTO [dbo].[tbl_CVLPersonalEMails]
                                ( [FK_PersonalID], [EMailAddress] )
                        VALUES  ( @NewPersonalId, -- FK_PersonalID - int
                                  @nr  -- EMailAddress - nvarchar(255)
                                  );			        
                    END;
                SET @EMails = RIGHT(@EMails, LEN(@EMails) - @Pos);
                SET @Pos = CHARINDEX(',', @EMails, 1);

            END;
    END;	


-- insert homepage
SET @Homepages = LTRIM(RTRIM(@Homepages))+ ','
SET @Pos = CHARINDEX(',', @Homepages, 1)

IF REPLACE(@Homepages, ',', '') <> ''
    BEGIN
        WHILE @Pos > 0
            BEGIN
                SET @nr = LTRIM(RTRIM(LEFT(@Homepages, @Pos - 1)));
                IF @nr <> ''
                    BEGIN
                        INSERT  INTO [dbo].[tbl_CVLPersonalHomepages]
                                ( [FK_PersonalID], [Homepage] )
                        VALUES  ( @NewPersonalId, -- FK_PersonalID - int
                                  @nr  -- Homepage - nvarchar(255)
                                  );
                    END;
                SET @Homepages = RIGHT(@Homepages, LEN(@Homepages) - @Pos);
                SET @Pos = CHARINDEX(',', @Homepages, 1);

            END;
    END;	


INSERT  INTO dbo.tbl_CVLAddress
        ( FK_PersonalID ,
          Street ,
          PostCode ,
          City ,
          FK_CountryCode ,
          State
        )
VALUES  ( @NewPersonalId , -- FK_PersonalID - int
          @Street , -- Street - nvarchar(255)
          @PostCode , -- PostCode - nvarchar(255)
          @City , -- City - nvarchar(255)
          @CountryCode , -- FK_CountryCode - nvarchar(50)
          @AddressState  -- State - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLPhase]
    @DateFrom DATETIME ,
    @DateTo DATETIME ,
    @DateFromFuzzy [NVARCHAR](255) ,
    @DateToFuzzy [NVARCHAR](255) ,
    @Duration INT ,
    @IsCurrent BIT ,
    @SubPhase BIT ,
    @Comments [NVARCHAR](MAX) ,
    @PlainText [NVARCHAR](MAX) ,
    @Topics [NVARCHAR](MAX) ,
    @DocumentID [NVARCHAR](MAX) ,
    @NewPhaseId INT OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLPhases
        ( DateFrom ,
          DateTo ,
          DateFromFuzzy ,
          DateToFuzzy ,
          Duration ,
          [Current] ,
          SubPhase ,
          Comments ,
          PlainText
        )
VALUES  ( @DateFrom , -- DateFrom - datetime
          @DateTo , -- DateTo - datetime
          @DateFromFuzzy , -- DateFromFuzzy - nvarchar(255)
          @DateToFuzzy , -- DateToFuzzy - nvarchar(255)
          @Duration , -- Duration - int
          @IsCurrent , -- Current - bit
          @SubPhase , -- SubPhase - bit
          @Comments , -- Comments - nvarchar(max)
          @PlainText  -- PlainText - nvarchar(max)
        )
						
		SET @NewPhaseId = @@Identity

DECLARE @nr varchar(max), @Pos int

-- insert topics 
SET @Topics = LTRIM(RTRIM(@Topics))+ '#'
SET @Pos = CHARINDEX('#', @Topics, 1)

IF REPLACE(@Topics, '#', '') <> ''
BEGIN
	WHILE @Pos > 0
	BEGIN
		SET @nr = LTRIM(RTRIM(LEFT(@Topics, @Pos - 1)))
		IF @nr <> ''
		BEGIN
			INSERT INTO [dbo].[tbl_CVLPhaseTopics]
			        ( [FK_PhasesID], [Name] )
			VALUES  ( @NewPhaseId, -- FK_PhasesID - int
			          @nr  -- Name - nvarchar(255)
			          )
		END
		SET @Topics = RIGHT(@Topics, LEN(@Topics) - @Pos)
		SET @Pos = CHARINDEX('#', @Topics, 1)

	END
END	

-- insert document id
SET @DocumentID = LTRIM(RTRIM(@DocumentID))+ '#'
SET @Pos = CHARINDEX('#', @DocumentID, 1)

IF REPLACE(@DocumentID, '#', '') <> ''
BEGIN
	WHILE @Pos > 0
	BEGIN
		SET @nr = LTRIM(RTRIM(LEFT(@DocumentID, @Pos - 1)))
		IF @nr <> ''
		BEGIN
			INSERT INTO [dbo].tbl_CVLPhaseDocumentIDs
			        ( [FK_PhasesID], [Code] )
			VALUES  ( @NewPhaseId, -- FK_PhasesID - int
			          CONVERT(INT, @nr)  -- Name - nvarchar(255)
			          )
		END
		SET @DocumentID = RIGHT(@DocumentID, LEN(@DocumentID) - @Pos)
		SET @Pos = CHARINDEX('#', @DocumentID, 1)

	END
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
END
GO




CREATE PROCEDURE [dbo].[CreateCVLPhaseLocation]
    @PhasesID INT ,
	@Street [nvarchar](255) NULL,
	@PostCode [nvarchar](255) NULL,
	@City [nvarchar](255) NULL,
	@CountryCode [nvarchar](50) NULL,
	@AddressState [nvarchar](255) NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

IF dbo.uf_CLV_ValidateCountryCode(@CountryCode) = 0 
BEGIN
	SET @CountryCode = NULL
END

INSERT INTO dbo.tbl_CVLPhaseLocations
        ( FK_PhasesID ,
          Street ,
          PostCode ,
          City ,
          FK_CountryCode ,
          State
        )
VALUES  ( @PhasesID , -- FK_PhasesID - int
          @Street , -- Street - nvarchar(255)
          @PostCode , -- PostCode - nvarchar(255)
          @City , -- City - nvarchar(255)
          @CountryCode , -- FK_CountryCode - nvarchar(50)
          @AddressState  -- State - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLPhaseSkill]
    @PhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL,
	@Weight money NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLPhaseSkills
        ( FK_PhasesID, Code, Name, Weight )
VALUES  ( @PhasesID, -- FK_PhasesID - int
          @Code, -- Code - nvarchar(50)
          @CodeName, -- Name - nvarchar(255)
          @Weight  -- Weight - money
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




CREATE PROCEDURE [dbo].[CreateCVLPhaseSoftSkill]
    @PhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL,
	@Weight money NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLPhaseSoftSkills
        ( FK_PhasesID, Code, Name, Weight )
VALUES  ( @PhasesID, -- FK_PhasesID - int
          @Code, -- Code - nvarchar(50)
          @CodeName, -- Name - nvarchar(255)
          @Weight  -- Weight - money
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




CREATE PROCEDURE [dbo].[CreateCVLPhaseOperationArea]
    @PhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL,
	@Weight money NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLPhaseOperationAreas
        ( FK_PhasesID, Code, Name, Weight )
VALUES  ( @PhasesID, -- FK_PhasesID - int
          @Code, -- Code - nvarchar(50)
          @CodeName, -- Name - nvarchar(255)
          @Weight  -- Weight - money
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




CREATE PROCEDURE [dbo].[CreateCVLPhaseIndustry]
    @PhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL,
	@Weight money NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLPhaseIndustries
        ( FK_PhasesID, Code, Name, Weight )
VALUES  ( @PhasesID, -- FK_PhasesID - int
          @Code, -- Code - nvarchar(50)
          @CodeName, -- Name - nvarchar(255)
          @Weight  -- Weight - money
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




CREATE PROCEDURE [dbo].[CreateCVLPhaseCustomCode]
    @PhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL,
	@Weight money NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLPhaseCustomCodes
        ( FK_PhasesID, Code, Name, Weight )
VALUES  ( @PhasesID, -- FK_PhasesID - int
          @Code, -- Code - nvarchar(50)
          @CodeName, -- Name - nvarchar(255)
          @Weight  -- Weight - money
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




CREATE PROCEDURE [dbo].[CreateCVLPhaseInternetResource]
    @PhasesID INT ,
	@Url [nvarchar](255) NULL,
	@Title [nvarchar](255) NULL,
	@Source [nvarchar](255) NULL,
	@Snippet [nvarchar](255) NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLPhaseInternetRessources
        ( FK_PhasesID ,
          URL ,
          Title ,
          Source ,
          Snippet
        )
VALUES  ( @PhasesID , -- FK_PhasesID - int
          @Url , -- URL - nvarchar(255)
          @Title , -- Title - nvarchar(255)
          @Source , -- Source - nvarchar(255)
          @Snippet  -- Snippet - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLWorkPhase]
    @CVLWorkID INT ,
    @PhaseID INT ,
	@Project BIT NULL,
    @Companies [NVARCHAR](MAX) ,
    @Functions [NVARCHAR](MAX) ,
    @NewWorkPhaseId INT OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLWorkPhases
        ( FK_WorkID, FK_PhasesID, Project )
VALUES  ( @CVLWorkID, -- FK_WorkID - int
          @PhaseID, -- FK_PhasesID - int
          @Project  -- Project - bit
          )

		SET @NewWorkPhaseId = @@Identity

DECLARE @nr varchar(max), @Pos int

-- insert companies 
SET @Companies = LTRIM(RTRIM(@Companies))+ '#'
SET @Pos = CHARINDEX('#', @Companies, 1)

IF REPLACE(@Companies, '#', '') <> ''
BEGIN
	WHILE @Pos > 0
	BEGIN
		SET @nr = LTRIM(RTRIM(LEFT(@Companies, @Pos - 1)))
		IF @nr <> ''
		BEGIN
			INSERT INTO [dbo].[tbl_CVLWorkPhaseCompanies]
			        ( [FK_WorkPhaseID], [Company] )
			VALUES  ( @NewWorkPhaseId, -- FK_PhasesID - int
			          @nr  -- Company - nvarchar(255)
			          )
		END
		SET @Companies = RIGHT(@Companies, LEN(@Companies) - @Pos)
		SET @Pos = CHARINDEX('#', @Companies, 1)

	END
END	

-- insert functions 
SET @Functions = LTRIM(RTRIM(@Functions))+ '#'
SET @Pos = CHARINDEX('#', @Functions, 1)

IF REPLACE(@Functions, '#', '') <> ''
BEGIN
	WHILE @Pos > 0
	BEGIN
		SET @nr = LTRIM(RTRIM(LEFT(@Functions, @Pos - 1)))
		IF @nr <> ''
		BEGIN
			INSERT INTO [dbo].[tbl_CVLWorkPhaseFunctions]
			        ( [FK_WorkPhaseID], [Function] )
			VALUES  ( @NewWorkPhaseId, -- FK_PhasesID - int
			          @nr  -- Function - nvarchar(255)
			          )
		END
		SET @Functions = RIGHT(@Functions, LEN(@Functions) - @Pos)
		SET @Pos = CHARINDEX('#', @Functions, 1)

	END
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
END
GO




CREATE PROCEDURE [dbo].[CreateCVLWorkPhasePosition]
    @WorkPhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLWorkPhasePositions
        ( FK_WorkPhaseID, Code, Name )
VALUES  ( @WorkPhasesID, -- FK_WorkPhaseID - int
          @Code, -- Code - nvarchar(50)
          @CodeName  -- Name - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLWorkPhaseEmployment]
    @WorkPhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLWorkPhaseEmployments
        ( FK_WorkPhaseID, Code, Name )
VALUES  ( @WorkPhasesID, -- FK_WorkPhaseID - int
          @Code, -- Code - nvarchar(50)
          @CodeName  -- Name - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLWorkPhaseWorktime]
    @WorkPhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLWorkPhaseWorktimes
        ( FK_WorkPhaseID, Code, Name )
VALUES  ( @WorkPhasesID, -- FK_WorkPhaseID - int
          @Code, -- Code - nvarchar(50)
          @CodeName  -- Name - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLEducationPhase]
    @CVLEducationID INT ,
    @PhaseID INT ,
    @IsCedCode [NVARCHAR](50) ,
	@Completed BIT NULL,
    @Score INT ,
    @SchoolName [NVARCHAR](MAX) ,
    @Graduation [NVARCHAR](MAX) ,
    @NewEducationPhaseId INT OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

IF dbo.uf_CLV_ValidateCedCode(@IsCedCode) = 0 
BEGIN
	SET @IsCedCode = NULL
END

INSERT INTO dbo.tbl_CVLEducationPhases
        ( FK_EducationID ,
          FK_PhasesID ,
          FK_IsCedCode ,
          Completed ,
          Score
        )
VALUES  ( @CVLEducationID , -- FK_EducationID - int
          @PhaseID , -- FK_PhasesID - int
          @IsCedCode , -- FK_IsCedCode - nvarchar(50)
          @Completed , -- Completed - bit
          @Score  -- Score - int
        )

		SET @NewEducationPhaseId = @@Identity

DECLARE @nr varchar(max), @Pos int

-- insert schoolname 
SET @SchoolName = LTRIM(RTRIM(@SchoolName))+ '#'
SET @Pos = CHARINDEX('#', @SchoolName, 1)

IF REPLACE(@SchoolName, '#', '') <> ''
BEGIN
	WHILE @Pos > 0
	BEGIN
		SET @nr = LTRIM(RTRIM(LEFT(@SchoolName, @Pos - 1)))
		IF @nr <> ''
		BEGIN
			INSERT INTO [dbo].[tbl_CVLEducPhaseSchoolnames]
			        ( [FK_EducPhasesID], [Schoolname] )
			VALUES  ( @NewEducationPhaseId, -- FK_EducPhasesID - int
			          @nr  -- Schoolname - nvarchar(255)
			          )
		END
		SET @SchoolName = RIGHT(@SchoolName, LEN(@SchoolName) - @Pos)
		SET @Pos = CHARINDEX('#', @SchoolName, 1)

	END
END	

-- insert functions 
SET @Graduation = LTRIM(RTRIM(@Graduation))+ '#'
SET @Pos = CHARINDEX('#', @Graduation, 1)

IF REPLACE(@Graduation, '#', '') <> ''
BEGIN
	WHILE @Pos > 0
	BEGIN
		SET @nr = LTRIM(RTRIM(LEFT(@Graduation, @Pos - 1)))
		IF @nr <> ''
		BEGIN
			INSERT INTO [dbo].[tbl_CVLEducPhaseGraduations]
			        ( [FK_EducPhasesID] ,
			          [Graduations]
			        )
			VALUES  ( @NewEducationPhaseId , -- FK_EducPhasesID - int
			          @nr  -- Graduations - nvarchar(255)
			        )
		END
		SET @Graduation = RIGHT(@Graduation, LEN(@Graduation) - @Pos)
		SET @Pos = CHARINDEX('#', @Graduation, 1)

	END
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
END
GO




CREATE PROCEDURE [dbo].[CreateCVLEducationPhaseEducationType]
    @EducPhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL,
	@Weight money NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLEducationType
        ( FK_EducPhasesID, Code, Name, Weight )
VALUES  ( @EducPhasesID, -- FK_EducPhasesID - int
          @Code, -- Code - nvarchar(50)
          @CodeName, -- Name - nvarchar(255)
          @Weight  -- Weight - money
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




CREATE PROCEDURE [dbo].[CreateCVLPublicationPhase]
    @CVLProfileID INT ,
    @PhaseID INT ,
    @Proceedings [NVARCHAR](MAX) ,
    @Institute [NVARCHAR](MAX) ,
    @NewPublicationPhaseId INT OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLPublications
        ( FK_CVLID ,
          FK_PhasesID ,
          Proceedings ,
          Institute
        )
VALUES  ( @CVLProfileID , -- FK_CVLID - int
          @PhaseID , -- FK_PhasesID - int
          @Proceedings , -- Proceedings - nvarchar(1000)
          @Institute  -- Institute - nvarchar(1000)
		  )

		SET @NewPublicationPhaseId = @@Identity

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




CREATE PROCEDURE [dbo].[CreateCVLPublicationPhaseAutor]
    @CVLPubPhaseID INT ,
    @Autor [NVARCHAR](MAX) 
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLPubPhaseAuthors
        ( FK_PubPhaseID, Authors )
VALUES  ( @CVLPubPhaseID, -- FK_PubID - int
          @Autor  -- Authors - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLAdditionalInformation]
    @CVLProfileID INT ,
    @MilitaryService BIT ,
    @Competences [NVARCHAR](MAX) ,
    @Additionals [NVARCHAR](MAX) ,
    @Interests [NVARCHAR](MAX) ,
    @NewAdditioalInfoId INT OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLAdditionalInformations
        ( FK_CVLID ,
          MilitaryService ,
          Competences ,
          Additionals ,
          Interests
        )
VALUES  ( @CVLProfileID , -- FK_CVLID - int
          @MilitaryService , -- MilitaryService - bit
          @Competences , -- Competences - nvarchar(1000)
          @Additionals , -- Additionals - nvarchar(1000)
          @Interests  -- Interests - nvarchar(1000)
        )

		SET @NewAdditioalInfoId = @@Identity

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




CREATE PROCEDURE [dbo].[CreateCVLAdditionalLanguage]
    @CVLAdditionalID INT ,
    @LanguageCode [NVARCHAR](50) ,
    @LanguageLevelCode [NVARCHAR](50) 
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

IF dbo.uf_CLV_ValidateLanguageCode(@LanguageCode) = 0 
BEGIN
	SET @LanguageCode = NULL
END
IF dbo.uf_CLV_ValidateLanguageLevelCode(@LanguageLevelCode) = 0 
BEGIN
	SET @LanguageLevelCode = NULL
END

INSERT INTO dbo.tbl_CVLAddLanguages
        ( FK_AddID ,
          FK_LanguageCode ,
          FK_LanguageLevelCode
        )
VALUES  ( @CVLAdditionalID , -- FK_AddID - int
          @LanguageCode , -- FK_LanguageCode - nvarchar(50)
          @LanguageLevelCode  -- FK_LanguageLevelCode - nvarchar(50)
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




CREATE PROCEDURE [dbo].[CreateCVLAdditionalDriverLicence]
    @CVLAdditionalID INT ,
    @DrivingLicence [NVARCHAR](255)
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLAddDrivingLicences
        ( FK_AddID, DrivingLicence )
VALUES  ( @CVLAdditionalID, -- FK_AddID - int
          @DrivingLicence  -- DrivingLicence - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLAdditionalUSkill]
    @CVLAdditionalID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL,
	@Weight money NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLAddUndatedSkills
        ( FK_AddID, Code, Name, Weight )
VALUES  ( @CVLAdditionalID, -- FK_AddID - int
          @Code, -- Code - nvarchar(50)
          @CodeName, -- Name - nvarchar(255)
          @Weight  -- Weight - money
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




CREATE PROCEDURE [dbo].[CreateCVLAdditionalUOperationArea]
    @CVLAdditionalID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL,
	@Weight money NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLAddUndatedOperationAreas
        ( FK_AddID, Code, Name, Weight )
VALUES  ( @CVLAdditionalID, -- FK_AddID - int
          @Code, -- Code - nvarchar(50)
          @CodeName, -- Name - nvarchar(255)
          @Weight  -- Weight - money
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




CREATE PROCEDURE [dbo].[CreateCVLAdditionalUIndustry]
    @CVLAdditionalID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL,
	@Weight money NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLAddUndatedIndustries
        ( FK_AddID, Code, Name, Weight )
VALUES  ( @CVLAdditionalID, -- FK_AddID - int
          @Code, -- Code - nvarchar(50)
          @CodeName, -- Name - nvarchar(255)
          @Weight  -- Weight - money
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




CREATE PROCEDURE [dbo].[CreateCVLAdditionalInternetResource]
    @CVLAdditionalID INT ,
	@Url [nvarchar](255) NULL,
	@Title [nvarchar](255) NULL,
	@Source [nvarchar](255) NULL,
	@Snippet [nvarchar](255) NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLAddInternetresources
        ( FK_AddID ,
          URL ,
          Title ,
          Source ,
          Snippet
        )
VALUES  ( @CVLAdditionalID , -- FK_PhasesID - int
          @Url , -- URL - nvarchar(255)
          @Title , -- Title - nvarchar(255)
          @Source , -- Source - nvarchar(255)
          @Snippet  -- Snippet - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLObjective]
    @CVLProfileID INT ,
    @AvailabilityDate DATE ,
    @NewObjectiveId INT OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLObjective
        ( FK_CVLID, AvailabilityDate )
VALUES  ( @CVLProfileID, -- FK_CVLID - int
          @AvailabilityDate  -- AvailabilityDate - datetime
          )

		SET @NewObjectiveId = @@Identity
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




CREATE PROCEDURE [dbo].[CreateCVLObjectivePhase]
    @CVLObjID INT ,
    @PhaseID INT ,
	@Project BIT NULL,
    @Companies [NVARCHAR](MAX) ,
    @Functions [NVARCHAR](MAX) ,
    @NewObjPhaseId INT OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLObjPhases
        ( FK_ObjID, FK_PhasesID, Project )
VALUES  ( @CVLObjID, -- FK_ObjID - int
          @PhaseID, -- FK_PhasesID - int
          @Project  -- Project - bit
          )

		SET @NewObjPhaseId = @@Identity

DECLARE @nr varchar(max), @Pos int

-- insert companies 
SET @Companies = LTRIM(RTRIM(@Companies))+ '#'
SET @Pos = CHARINDEX('#', @Companies, 1)

IF REPLACE(@Companies, '#', '') <> ''
BEGIN
	WHILE @Pos > 0
	BEGIN
		SET @nr = LTRIM(RTRIM(LEFT(@Companies, @Pos - 1)))
		IF @nr <> ''
		BEGIN
			INSERT INTO [dbo].[tbl_CVLobjPhaseCompanies]
			        ( [FK_ObjPhaseID], [Company] )
			VALUES  ( @NewObjPhaseId, -- FK_PhasesID - int
			          @nr  -- Company - nvarchar(255)
			          )
		END
		SET @Companies = RIGHT(@Companies, LEN(@Companies) - @Pos)
		SET @Pos = CHARINDEX('#', @Companies, 1)

	END
END	

-- insert functions 
SET @Functions = LTRIM(RTRIM(@Functions))+ '#'
SET @Pos = CHARINDEX('#', @Functions, 1)

IF REPLACE(@Functions, '#', '') <> ''
BEGIN
	WHILE @Pos > 0
	BEGIN
		SET @nr = LTRIM(RTRIM(LEFT(@Functions, @Pos - 1)))
		IF @nr <> ''
		BEGIN
			INSERT INTO [dbo].[tbl_CVLObjPhaseFunctions]
			        ( [FK_ObjPhaseID], [Function] )
			VALUES  ( @NewObjPhaseId, -- FK_PhasesID - int
			          @nr  -- Function - nvarchar(255)
			          )
		END
		SET @Functions = RIGHT(@Functions, LEN(@Functions) - @Pos)
		SET @Pos = CHARINDEX('#', @Functions, 1)

	END
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
END
GO




CREATE PROCEDURE [dbo].[CreateCVLObjPhasePosition]
    @ObjPhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLobjPhasePositions
        ( FK_ObjPhaseID, Code, Name )
VALUES  ( @ObjPhasesID, -- FK_WorkPhaseID - int
          @Code, -- Code - nvarchar(50)
          @CodeName  -- Name - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLObjPhaseEmployment]
    @ObjPhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLObjPhaseEmployments
        ( FK_ObjPhaseID, Code, Name )
VALUES  ( @ObjPhasesID, -- FK_WorkPhaseID - int
          @Code, -- Code - nvarchar(50)
          @CodeName  -- Name - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLObjPhaseWorktime]
    @ObjPhasesID INT ,
	@Code [nvarchar](50) NULL,
	@CodeName [nvarchar](255) NULL
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLObjPhaseWorktimes
        ( FK_ObjPhaseID, Code, Name )
VALUES  ( @ObjPhasesID, -- FK_WorkPhaseID - int
          @Code, -- Code - nvarchar(50)
          @CodeName  -- Name - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLObjectiveSalary]
    @CVLObjID INT ,
    @salary [NVARCHAR](255)
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLObjSalary
        ( FK_ObjID, Salary )
VALUES  ( @CVLObjID, -- FK_ObjID - int
          @salary  -- Salary - nvarchar(255)
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




CREATE PROCEDURE [dbo].[CreateCVLStatistic]
    @CVLProfileID INT ,
    @Code [NVARCHAR] (50) ,
    @Name [NVARCHAR] (255) ,
    @Weight MONEY  ,
    @Duration INT ,
    @Domain [NVARCHAR](255) ,
    @NewStatisticId INT OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLStatistics
        ( FK_CVLID ,
          Code ,
          Name ,
          Weight ,
          Duration ,
          Domain
        )
VALUES  ( @CVLProfileID , -- FK_CVLID - int
          @Code , -- Code - nvarchar(50)
          @Name ,
          @Weight , -- Weight - money
          @Duration , -- Duration - int
          @Domain  -- Domain - nvarchar(50)
        )

		SET @NewStatisticId = @@Identity

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




CREATE PROCEDURE [dbo].[CreateCVLDocument]
    @CVLProfileID INT ,
    @DocClass [NVARCHAR] (50) ,
    @Pages INT ,
    @Plaintext [NVARCHAR] (max) ,
    @FileType [NVARCHAR] (50) ,
    @DocBinary VARBINARY (max) ,
    @DocID INT ,
    @DocSize INT ,
    @DocLanguage [NVARCHAR] (50) ,
    @FileHashvalue [NVARCHAR] (MAX) ,
    @DocXML [NVARCHAR] (max) ,
    @NewDocumentId INT OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLDocuments
        ( FK_CVLID ,
          DocClass ,
          Pages ,
          Plaintext ,
          FileType ,
          DocBinary ,
          DocID ,
          DocSize ,
          DocLanguage,
		  FileHashvalue ,
          DocXML
        )
VALUES  ( @CVLProfileID , -- FK_CVLID - int
          @DocClass , -- DocClass - nvarchar(50)
          @Pages , -- Pages - int
          @Plaintext , -- Plaintext - nvarchar(max)
          @FileType , -- FileType - nvarchar(50)
          @DocBinary , -- DocBinary - varbinary(1)
          @DocID , -- DocID - int
          @DocSize , -- DocSize - int
          @DocLanguage , -- DocLanguage - nvarchar(50)
          @FileHashvalue , -- FileHashvalue - nvarchar(max)
          @DocXML  -- DocXML - nvarchar(max)
        )

		SET @NewDocumentId = @@Identity

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





CREATE PROCEDURE [dbo].[Load CVL Profile Data]
    @CustomerID [NVARCHAR] (50) ,
    @CVLProfileID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  P.ID ,
            P.Customer_ID ,
            Personal.ID PersonalID ,
(
CASE            
WHEN Personal.ID IS NULL THEN (SELECT TOP (1) a.firstname FROM [applicant].dbo.tbl_applicant a WHERE a.ID = uAP.ApplicantID ORDER BY ID)
ELSE
Personal.FirstName
END
) FirstName
 ,
(
CASE            
WHEN Personal.ID IS NULL THEN (SELECT TOP (1) a.LastName FROM [applicant].dbo.tbl_applicant a WHERE a.ID = uAP.ApplicantID ORDER BY ID)
ELSE
Personal.LastName
END
) LastName
,
(
CASE            
WHEN Personal.ID IS NULL THEN (SELECT TOP (1) a.Birthdate FROM [applicant].dbo.tbl_applicant a WHERE a.ID = uAP.ApplicantID ORDER BY ID)
ELSE
Personal.DateOfBirth
END
) DateOfBirth
,
(
CASE            
WHEN Personal.ID IS NULL THEN (SELECT TOP (1) dbo.uf_CLV_GenderLable(a.Gender) FROM [applicant].dbo.tbl_applicant a WHERE a.ID = uAP.ApplicantID ORDER BY ID)
ELSE
dbo.uf_CLV_GenderLable(Personal.FK_GenderCode)
END
) GenderLabel
,

            W.ID WorkID ,
            E.ID EducationID ,
            A.ID AdditionalID ,
            O.ID ObjectiveID ,
            P.CreatedOn ,
            P.CreatedFrom
    FROM    dbo.tbl_CVLProfile P
            LEFT JOIN dbo.tbl_CVLPersonalInformation Personal ON Personal.FK_CVLID = P.ID
            LEFT JOIN dbo.tbl_CVLWork W ON W.FK_CVLID = P.ID
            LEFT JOIN dbo.tbl_CVLEducation E ON E.FK_CVLID = P.ID
            LEFT JOIN dbo.tbl_CVLAdditionalInformations A ON A.FK_CVLID = P.ID
            LEFT JOIN dbo.tbl_CVLObjective O ON O.FK_CVLID = P.ID

				CROSS APPLY dbo.uf_LoadApplicationDataWithProfileID(p.ID) AS uAP

    WHERE   (IsNull(@CustomerID, '') = '' OR P.Customer_ID = @CustomerID)
AND P.ID = @CVLProfileID

            --AND ( ISNULL(@CVLProfileID, 0) = 0
            --      OR P.ID = @CVLProfileID
            --    )
				ORDER BY P.ID DESC;

END;
GO





CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Data]
    @CVLProfileID INT ,
	@PersonalID INT = 0 

AS

BEGIN
    SET NOCOUNT ON;

IF ISNULL(@PersonalID, 0) = 0
BEGIN
	SET @PersonalID = ISNULL((SELECT TOP (1) ID FROM spCVLizerBaseInfo.dbo.tbl_CVLPersonalInformation WHERE FK_CVLID = @CVLProfileID ORDER BY FK_CVLID), 0)
END

IF ISNULL(@PersonalID, 0) > 0
BEGIN
    SELECT  Personal.ID ,
            Personal.FK_CVLID ,
            Personal.FirstName ,
            Personal.LastName ,
            Personal.FK_GenderCode , dbo.uf_CLV_GenderLable(FK_GenderCode) GenderLabel, 
            Personal.DateOfBirth ,
            Personal.FK_IsCedCode , dbo.uf_CLV_CedLable(Personal.FK_IsCedCode) IsCedLable, 
            Personal.PlaceOfBirth 
    FROM    dbo.tbl_CVLPersonalInformation Personal
    WHERE   Personal.FK_CVLID = @CVLProfileID 
	AND (IsNull(@PersonalID, 0) = 0 OR Personal.ID = @PersonalID);

END
ELSE
BEGIN
    SELECT  0 ID ,
            Personal.CVLProfileID FK_CVLID ,
						Personal.FirstName FirstName,
						Personal.LastName LastName,
						Personal.Birthdate DateOfBirth, 
						Personal.Gender FK_GenderCode , dbo.uf_CLV_GenderLable(Personal.Gender) GenderLabel,
            '' FK_IsCedCode , '' IsCedLable, 
            '' PlaceOfBirth 
    FROM    [applicant].dbo.tbl_applicant Personal

				CROSS APPLY dbo.uf_LoadApplicationDataWithProfileID(@CVLProfileID) AS uAP

    WHERE   Personal.CVLProfileID = @CVLProfileID;
END
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Address Data]
    @CVLProfileID INT ,
		@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

IF ISNULL(@PersonalID, 0) = 0
BEGIN
	SET @PersonalID = ISNULL((SELECT TOP (1) ID FROM spCVLizerBaseInfo.dbo.tbl_CVLPersonalInformation WHERE FK_CVLID = @CVLProfileID ORDER BY FK_CVLID), 0)
END

IF ISNULL(@PersonalID, 0) > 0
BEGIN
    SELECT  ID ,
            FK_PersonalID ,
            Street ,
            PostCode ,
            City ,
            FK_CountryCode , dbo.uf_CLV_CountryLable(FK_CountryCode) CountryLable, 
            State
    FROM    dbo.tbl_CVLAddress
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
END
ELSE
BEGIN
    SELECT  A.ID ,
            0 FK_PersonalID ,
            A.Street ,
            A.PostCode ,
            A.Location City ,
            dbo.uf_CLV_CountryCode(A.Country) FK_CountryCode , 
						(CASE 
WHEN LEN(ISNULL(A.Country, '')) = 0 THEN ''
WHEN LEN(ISNULL(A.Country, '')) > 3 THEN dbo.uf_CLV_CountryCode(A.Country)
ELSE A.Country
END
						)
						FK_CountryCode ,
						(CASE 
WHEN LEN(ISNULL(A.Country, '')) = 0 THEN ''
WHEN LEN(ISNULL(A.Country, '')) > 3 THEN A.Country
ELSE dbo.uf_CLV_CountryLable(A.Country)
END
						)
						CountryLable, 
            '' State
    FROM    [applicant].dbo.tbl_applicant A
    WHERE		A.CVLProfileID = @CVLProfileID
	ORDER BY ID;
END
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Photo]
	@CVLProfileID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            DocClass ,
            Pages ,
            Plaintext ,
            FileType ,
            DocBinary ,
            DocID ,
            DocSize ,
            DocLanguage ,
			FileHashvalue ,
            DocXML 
    FROM    dbo.tbl_CVLDocuments
    WHERE   FK_CVLID = @CVLProfileID
	AND DocClass = 'Photo'
	AND DocBinary IS NOT NULL ;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Title Data]
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PersonalID ,
            Title
    FROM    dbo.tbl_CVLPersonalTitle
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Nationality Data]
    @CVLProfileID INT ,
		@PersonalID INT = 0 

AS

BEGIN
    SET NOCOUNT ON;

IF ISNULL(@PersonalID, 0) = 0
BEGIN
	SET @PersonalID = ISNULL((SELECT TOP (1) ID FROM spCVLizerBaseInfo.dbo.tbl_CVLPersonalInformation WHERE FK_CVLID = @CVLProfileID ORDER BY FK_CVLID), 0)
END

IF ISNULL(@PersonalID, 0) > 0
BEGIN
    SELECT  ID ,
            FK_PersonalID ,
            FK_NationalityCode, dbo.uf_CLV_CountryLable(FK_NationalityCode) NationalityCodeLable
    FROM    dbo.tbl_CVLPersonalNationality
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
END
ELSE
BEGIN
    SELECT  A.ID ,
            0 FK_PersonalID ,
            A.Nationality FK_NationalityCode, dbo.uf_CLV_CountryLable(A.Nationality) NationalityCodeLable
    FROM    [applicant].dbo.tbl_applicant A
    WHERE		A.CVLProfileID = @CVLProfileID
	ORDER BY ID;
END

END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal CivilState Data]
    @CVLProfileID INT ,
		@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

IF ISNULL(@PersonalID, 0) = 0
BEGIN
	SET @PersonalID = ISNULL((SELECT TOP (1) ID FROM spCVLizerBaseInfo.dbo.tbl_CVLPersonalInformation WHERE FK_CVLID = @CVLProfileID ORDER BY FK_CVLID), 0)
END

IF ISNULL(@PersonalID, 0) > 0
BEGIN
    SELECT  ID ,
            FK_PersonalID ,
            [FK_CivilStateCode], dbo.uf_CLV_CivilstateLable([FK_CivilStateCode]) CivilStateCodeLable
    FROM    dbo.tbl_CVLPersonalCivilstate
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
END
ELSE
BEGIN
    SELECT  A.ID ,
            0 FK_PersonalID ,
            dbo.uf_App_CivilstateCode(A.CivilState) FK_CivilStateCode, dbo.uf_App_CivilstateLabel(A.CivilState) CivilStateCodeLable
    FROM    [applicant].dbo.tbl_applicant A
    WHERE		A.CVLProfileID = @CVLProfileID
	ORDER BY ID;
END
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal EMail Data]
    @CVLProfileID INT ,
		@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

IF ISNULL(@PersonalID, 0) = 0
BEGIN
	SET @PersonalID = ISNULL((SELECT TOP (1) ID FROM spCVLizerBaseInfo.dbo.tbl_CVLPersonalInformation WHERE FK_CVLID = @CVLProfileID ORDER BY FK_CVLID), 0)
END

IF ISNULL(@PersonalID, 0) > 0
BEGIN
    SELECT  ID ,
            FK_PersonalID ,
            EMailAddress
    FROM    dbo.tbl_CVLPersonalEMails
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
END
ELSE
BEGIN
    SELECT  A.ID ,
            0 FK_PersonalID ,
            A.EMail EMailAddress
    FROM    [applicant].dbo.tbl_applicant A
    WHERE		A.CVLProfileID = @CVLProfileID
	ORDER BY ID;
END
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Homepage Data]
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PersonalID ,
            Homepage
    FROM    dbo.tbl_CVLPersonalHomepages
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;

END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Telefonnumber Data]
    @CVLProfileID INT ,
		@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

IF ISNULL(@PersonalID, 0) = 0
BEGIN
	SET @PersonalID = ISNULL((SELECT TOP (1) ID FROM spCVLizerBaseInfo.dbo.tbl_CVLPersonalInformation WHERE FK_CVLID = @CVLProfileID ORDER BY FK_CVLID), 0)
END

IF ISNULL(@PersonalID, 0) > 0
BEGIN
    SELECT  ID ,
            FK_PersonalID ,
            PhoneNumber
    FROM    dbo.tbl_CVLPersonalPhoneNumbers
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
END
ELSE
BEGIN
		DECLARE @table TABLE (ID INT NOT NULL, FK_PersonalID INT NOT NULL, PhoneNumber NVARCHAR(255) NULL)

INSERT INTO @table (ID, FK_PersonalID, PhoneNumber)
    SELECT  A.ID ,
            0 ,
            ISNULL(A.Telephone, '')
    FROM    [applicant].dbo.tbl_applicant A
    WHERE		A.CVLProfileID = @CVLProfileID
						AND ISNULL(A.Telephone, '') <> ''

INSERT INTO @table (ID, FK_PersonalID, PhoneNumber)
    SELECT  0 ,
            0 ,
            ISNULL(A.MobilePhone, '')
    FROM    [applicant].dbo.tbl_applicant A
    WHERE		A.CVLProfileID = @CVLProfileID
						AND ISNULL(A.MobilePhone, '') <> ''
		
		SELECT ID,
               FK_PersonalID,
               PhoneNumber FROM @table

END
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Personal Telefaxnumber Data]
	@PersonalID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PersonalID ,
            TelefaxNumber
    FROM    dbo.tbl_CVLPersonalTelefaxNumbers
    WHERE   FK_PersonalID = @PersonalID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Location Data]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Street ,
            PostCode ,
            City ,
            FK_CountryCode , dbo.uf_CLV_CountryLable(FK_CountryCode) CountryLable ,
            State
    FROM    dbo.tbl_CVLPhaseLocations
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Skill Data]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLPhaseSkills
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase SoftSkill Data]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLPhaseSoftSkills
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase OperationArea Data]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLPhaseOperationAreas
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Industry Data]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLPhaseIndustries
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase CustomCode Data]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLPhaseCustomCodes
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Topic Data]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Name
    FROM    dbo.tbl_CVLPhaseTopics
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase InternetResource Data]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            URL ,
            Title ,
            Source ,
            Snippet
    FROM    dbo.tbl_CVLPhaseInternetRessources
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase DocumentID Data]
	@PhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PhasesID ,
            Code
    FROM    dbo.tbl_CVLPhaseDocumentIDs
    WHERE   FK_PhasesID = @PhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Data]
    @CVLProfileID INT ,
	@WorkID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  WPhase.ID ,
            WPhase.FK_WorkID ,
            WPhase.FK_PhasesID ,
			WPhase.Project , 
            Phase.DateFrom , Phase.DateTo, 
			Phase.DateFromFuzzy, Phase.DateToFuzzy , Phase.Duration , Phase.[Current], Phase.SubPhase, Phase.Comments, Phase.PlainText
    FROM    dbo.tbl_CVLWorkPhases WPhase
	LEFT JOIN dbo.tbl_CVLPhases Phase ON Phase.ID = WPhase.FK_PhasesID
    WHERE   WPhase.FK_WorkID = @WorkID
	ORDER BY WPhase.ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Company Data]
	@WorkPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_WorkPhaseID ,
            Company
    FROM    dbo.tbl_CVLWorkPhaseCompanies
    WHERE   FK_WorkPhaseID = @WorkPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Function Data]
	@WorkPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_WorkPhaseID ,
            [Function]
    FROM    dbo.tbl_CVLWorkPhaseFunctions
    WHERE   FK_WorkPhaseID = @WorkPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Position Data]
	@WorkPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_WorkPhaseID ,
            Code, Name 
    FROM    dbo.tbl_CVLWorkPhasePositions
    WHERE   FK_WorkPhaseID = @WorkPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Employment Data]
	@WorkPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_WorkPhaseID ,
            Code, Name 
    FROM    dbo.tbl_CVLWorkPhaseEmployments
    WHERE   FK_WorkPhaseID = @WorkPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Work Worktime Data]
	@WorkPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_WorkPhaseID ,
            Code, Name 
    FROM    dbo.tbl_CVLWorkPhaseWorktimes
    WHERE   FK_WorkPhaseID = @WorkPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Education Data]
    @CVLProfileID INT ,
	@EducationID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  EPhase.ID ,
            EPhase.FK_EducationID ,
            EPhase.FK_PhasesID ,
            EPhase.FK_IsCedCode , dbo.[uf_CLV_CedLable](EPhase.FK_IsCedCode) IsCedCodeLable,
            EPhase.Completed ,
            EPhase.Score , 
            Phase.DateFrom , Phase.DateTo, 
			Phase.DateFromFuzzy, Phase.DateToFuzzy , Phase.Duration , Phase.[Current], Phase.SubPhase, Phase.Comments, Phase.PlainText
    FROM    dbo.tbl_CVLEducationPhases EPhase
	LEFT JOIN dbo.tbl_CVLPhases Phase ON Phase.ID = EPhase.FK_PhasesID
    WHERE   EPhase.FK_EducationID = @EducationID
	ORDER BY EPhase.ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Schoolname Data]
	@EducationPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_EducPhasesID ,
            Schoolname
    FROM    dbo.tbl_CVLEducPhaseSchoolnames
    WHERE   FK_EducPhasesID = @EducationPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase Graduation Data]
	@EducationPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_EducPhasesID ,
            Graduations
    FROM    dbo.tbl_CVLEducPhaseGraduations
    WHERE   FK_EducPhasesID = @EducationPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Phase EducationType Data]
	@EducationPhaseID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_EducPhasesID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLEducationType
    WHERE   FK_EducPhasesID = @EducationPhaseID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Publication Data]
    @CVLProfileID INT  

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  Pub.ID ,
            Pub.FK_PhasesID ,
            Pub.Proceedings ,
            Pub.Institute , 
            Phase.DateFrom , Phase.DateTo, 
			Phase.DateFromFuzzy, Phase.DateToFuzzy , Phase.Duration , Phase.[Current], Phase.SubPhase, Phase.Comments, Phase.PlainText
    FROM    dbo.tbl_CVLPublications Pub
	LEFT JOIN dbo.tbl_CVLPhases Phase ON Phase.ID = Pub.FK_PhasesID
    WHERE   Pub.FK_CVLID = @CVLProfileID
	ORDER BY Pub.ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Publication Authors Data]
	@publicationID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_PubPhaseID ,
            Authors
    FROM    dbo.tbl_CVLPubPhaseAuthors
    WHERE   FK_PubPhaseID = @publicationID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional Information Data]
    @CVLProfileID INT ,
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            MilitaryService ,
            Competences ,
            Additionals ,
            Interests 
    FROM    dbo.tbl_CVLAdditionalInformations
    WHERE   FK_CVLID = @CVLProfileID
	AND ID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional DrivingLicence Data]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            DrivingLicence
    FROM    dbo.tbl_CVLAddDrivingLicences
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional Undated Skill Data]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLAddUndatedSkills
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional Undated OperationArea Data]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLAddUndatedOperationAreas
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional Undated Industry Data]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            Code ,
            Name ,
            Weight
    FROM    dbo.tbl_CVLAddUndatedIndustries
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional InternetResource Data]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            URL ,
            Title ,
            Source ,
            Snippet
    FROM    dbo.tbl_CVLAddInternetresources
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Additional Language Data]
	@AddID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_AddID ,
            FK_LanguageCode , dbo.uf_CLV_LanguageLable(FK_LanguageCode) LanguageLable ,
            FK_LanguageLevelCode , dbo.uf_CLV_LanguageLevelLable(FK_LanguageLevelCode) LanguageLevelLable
    FROM    dbo.tbl_CVLAddLanguages
    WHERE   FK_AddID = @AddID
	ORDER BY ID;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned Document Data]
	@ID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            DocClass ,
            Pages ,
            Plaintext ,
            FileType ,
            DocBinary ,
            DocID ,
            DocSize ,
            DocLanguage ,
			FileHashvalue ,
            DocXML 
    FROM    dbo.tbl_CVLDocuments
    WHERE   ID = @ID
	AND DocBinary IS NOT NULL
	ORDER BY ID DESC;
	
END;
GO




CREATE PROCEDURE [dbo].[Load CVL File Hashvalues]
	@customerID NVARCHAR(50),
	@cvHashvalue NVARCHAR(max)

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            CustomerID ,
			FileHashvalue 
    FROM    dbo.tbl_ParsedCVLFiles
    WHERE   CustomerID = @customerID 
	AND FileHashValue = @cvHashvalue
	ORDER BY ID DESC;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned Applicant Documents From CVL Data]
	@CVLProfileID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            DocClass ,
            Pages ,
            Plaintext ,
            FileType ,
            DocID ,
            DocSize ,
            DocLanguage ,
			FileHashvalue ,
			DocBinary ,
            DocXML 
    FROM    dbo.tbl_CVLDocuments
    WHERE   FK_CVLID = @CVLProfileID
	AND DocClass NOT Like 'Plot%'
	AND DocClass NOT IN ('Photo')
	AND DocBinary IS NOT NULL 
	ORDER BY ID DESC;
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned Applicant Picture From CVL Data]
	@CVLProfileID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            DocClass ,
            Pages ,
            Plaintext ,
            FileType ,
            DocID ,
            DocSize ,
            DocLanguage ,
			FileHashvalue ,
			DocBinary ,
            DocXML 
    FROM    dbo.tbl_CVLDocuments
    WHERE   FK_CVLID = @CVLProfileID
	AND DocClass IN ('Photo')
	AND DocBinary IS NOT NULL 
	ORDER BY ID DESC;
	
END;
GO




CREATE PROCEDURE [dbo].[Load ALL CVL Profile Data]
	@assignedDate DATETIME
AS

BEGIN
    SET NOCOUNT ON;

    SELECT  P.ID ,
            P.Customer_ID ,
			ISNULL((SELECT TOP (1) CD.CustomerName FROM spSystemInfo.dbo.tbl_CustomerData CD WHERE CD.Customer_ID = P.Customer_ID ORDER BY CD.ID), 'not defined!') CustomerName,
Personal.ID PersonalID, 
(
CASE            
WHEN Personal.ID IS NULL THEN (SELECT TOP (1) a.firstname FROM [applicant].dbo.tbl_applicant a WHERE a.ID = uAP.ApplicantID ORDER BY ID)
ELSE
Personal.FirstName
END
) FirstName
 ,
(
CASE            
WHEN Personal.ID IS NULL THEN (SELECT TOP (1) a.LastName FROM [applicant].dbo.tbl_applicant a WHERE a.ID = uAP.ApplicantID ORDER BY ID)
ELSE
Personal.LastName
END
) LastName
,
(
CASE            
WHEN Personal.ID IS NULL THEN (SELECT TOP (1) a.Birthdate FROM [applicant].dbo.tbl_applicant a WHERE a.ID = uAP.ApplicantID ORDER BY ID)
ELSE
Personal.DateOfBirth
END
) DateOfBirth
,
(
CASE            
WHEN Personal.ID IS NULL THEN (SELECT TOP (1) dbo.uf_CLV_GenderLable(a.Gender) FROM [applicant].dbo.tbl_applicant a WHERE a.ID = uAP.ApplicantID ORDER BY ID)
ELSE
dbo.uf_CLV_GenderLable(Personal.FK_GenderCode)
END
) GenderLabel
,

            W.ID WorkID ,
            E.ID EducationID ,
            A.ID AdditionalID ,
            O.ID ObjectiveID ,
            P.CreatedOn ,
            P.CreatedFrom ,
			--uap.CVLProfilID CVLProfilID ,
			--uAP.CVLPersonalID CVLPersonalID ,
			uap.ApplicationID ApplicationID ,
			uAP.ApplicantID ApplicantID ,
			uAP.ApplicationLabel ApplicationLabel 

    FROM    dbo.tbl_CVLProfile P
            LEFT JOIN dbo.tbl_CVLPersonalInformation Personal ON Personal.FK_CVLID = P.ID
            LEFT JOIN dbo.tbl_CVLWork W ON W.FK_CVLID = P.ID
            LEFT JOIN dbo.tbl_CVLEducation E ON E.FK_CVLID = P.ID
            LEFT JOIN dbo.tbl_CVLAdditionalInformations A ON A.FK_CVLID = P.ID
            LEFT JOIN dbo.tbl_CVLObjective O ON O.FK_CVLID = P.ID
			CROSS APPLY dbo.uf_LoadApplicationDataWithProfileID(p.ID) AS uAP 
			--ON uap.CVLProfilID = p.ID
			WHERE (@assignedDate IS NULL OR CONVERT(NVARCHAR(10), P.CreatedOn, 104) = CONVERT(NVARCHAR(10), @assignedDate, 104))
			ORDER BY P.ID DESC;

END;
GO




CREATE PROCEDURE [dbo].[Load Assigned CVL Document Data For Notification]
    @CVLProfileID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            DocClass ,
            Isnull (Pages, 0 ) Pages, 
			PlainText , 
			FileType ,
			DocBinary, 
			DocID , 
			DocSize , 
			DocLanguage,
			FileHashvalue, 
			DocXML
    FROM    dbo.tbl_CVLDocuments
    WHERE   FK_CVLID = @CVLProfileID
	ORDER BY ID;
	
END
GO




CREATE PROCEDURE [dbo].[Load ALL Assigned CVL Documents]
	@CVLProfileID INT 

AS

BEGIN
    SET NOCOUNT ON;

    SELECT  ID ,
            FK_CVLID ,
            DocClass ,
            Pages ,
            Plaintext ,
            FileType ,
            DocID ,
            DocSize ,
            DocLanguage ,
			FileHashvalue ,
			DocBinary ,
            DocXML 
    FROM    dbo.tbl_CVLDocuments
    WHERE   FK_CVLID = @CVLProfileID
	AND DocBinary IS NOT NULL 
	ORDER BY ID DESC;
	
END;
GO




CREATE PROCEDURE [dbo].[Add File Hash Info]
	@customerID NVARCHAR(50),
	@myFilename NVARCHAR(255),
	@FileHashvalue NVARCHAR(max)

AS

--SELECT * FROM [tbl_ParsedCVLFiles]
BEGIN
    SET NOCOUNT ON;



	INSERT INTO dbo.[tbl_ParsedCVLFiles] (   [CustomerID] ,
	                                     [FileName] ,
	                                     [FileHashvalue] ,
	                                     [FileDescription] ,
	                                     [CreatedOn]
	                                 )
	VALUES (   @customerID ,     -- CustomerID - nvarchar(50)
	           @myFilename ,     -- FileName - nvarchar(255)
	           @FileHashvalue ,     -- FileHashvalue - nvarchar(max)
	           N'CVL-File' ,     -- FileDescription - nvarchar(255)
	           GETDATE() -- CreatedOn - datetime
	       )
	
END;
GO




CREATE PROCEDURE [dbo].[CreateCVLExperiences]
    @CustomerID NVARCHAR(50) ,
	@ProfileID INT NULL,
	@DateFrom DATETIME NULL,
	@DateTo DATETIME NULL,
	@Duration INT NULL,
	@Code [nvarchar](50) NULL,
	@Skills BIT	NULL,
	@OpAreas BIT NULL,
	@JobTitel BIT NULL,

    @NewID INT OUTPUT
AS

BEGIN
	SET NOCOUNT ON
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

INSERT INTO dbo.tbl_CVLData_Experiences (   Customer_ID ,
                                        ProfileID ,
                                        Experience_Code ,
                                        Experience_in_Month ,
                                        Last_experience ,
                                        Skill ,
                                        OperationArea ,
                                        JobTitel ,
CreatedOn
                                    )
VALUES (   @CustomerID ,       -- Customer_ID - nvarchar(50)
           @ProfileID ,         -- ProfileID - int
           @Code ,       -- Experience_Code - nvarchar(50)
           @Duration ,         -- Experience_in_Month - int
           @DateTo , -- Last_experience - datetime
           @Skills ,      -- Sills - bit
           @OpAreas ,       -- OperationArea - bit
           @JobTitel ,       -- JobTitel - bit
GETDATE()
       )


		SET @NewID = @@Identity

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




CREATE PROCEDURE [dbo].[Load Assigned CVLPersonalInformation With Document Filehash Values]
	@customerID NVARCHAR(50),
	@FileHashvalue NVARCHAR(max)

AS

BEGIN
    SET NOCOUNT ON;

SELECT cp.ID ,
       cp.FK_CVLID ,
       cp.FirstName ,
       cp.LastName ,
       cp.FK_GenderCode ,
       cp.FK_IsCedCode ,
       cp.DateOfBirth ,
       cp.PlaceOfBirth ,
	   pf.Customer_ID
FROM   dbo.tbl_CVLPersonalInformation cp
left Join tbl_CVLProfile pf On pf.ID = cp.FK_CVLID and pf.Customer_ID = @customerID
WHERE  cp.FK_CVLID IN (   SELECT cd.FK_CVLID
                          FROM   dbo.tbl_CVLDocuments cd
                          WHERE  cd.FileHashvalue IN (   SELECT pf.FileHashvalue
                                                         FROM   dbo.tbl_ParsedCVLFiles pf
                                                         WHERE  pf.FileHashvalue IN (
    SELECT item FROM dbo.[DelimitedSplit8K](@FileHashvalue, ',')                                         )));
	
END;
GO




CREATE PROCEDURE [dbo].[Load EMail Data For Assigned Application]
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

FROM  [spEMailJobs].dbo.tbl_Received_EMail
WHERE --(ISNULL(@CustomerID, '') = '' OR Customer_ID = @CustomerID)
ApplicationID = @applicationID

END;
GO




Create PROCEDURE [dbo].[Load EMail Attatchment Data For Assigned EMail]
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

FROM  [spEMailJobs].dbo.tbl_EMail_Attachment
WHERE FK_REID = @eMailID

END;
GO




CREATE PROCEDURE [dbo].[Add Profilmatcher Query Notification]
	@customerID NVARCHAR(50),
	@UserID NVARCHAR(50),
	@EmployeeNumber INT,
	@CustomerNumber INT,
	@QueryContent NVARCHAR(max),
	@QueryResultContent NVARCHAR(max),
	@QueryName NVARCHAR(max),
	@Size INT,
	@Notify BIT,
	@CreatedFrom NVARCHAR(255)
	  
AS

BEGIN
    SET NOCOUNT ON;
		
	INSERT INTO dbo.[tbl_X28PMQueryNotifications]
	(
	    [Customer_ID],
	    [User_ID],
	    [EmployeeNumber],
	    [CustomerNumber],
	    [QueryName],
	    [QueryContent],
	    [QueryResultContent],
	    [Size],
	    [Notify],
	    [CreatedFrom],
	    [CreatedOn]
	)
	VALUES
	(   @customerID,      -- Customer_ID - nvarchar(50)
	    @UserID,      -- User_ID - nvarchar(50)
	    @EmployeeNumber,        -- EmployeeNumber - int
	    @CustomerNumber,        -- CustomerNumber - int
	    @QueryName,      -- QueryName - nvarchar(255)
	    @QueryContent,      -- QueryContent - nvarchar(max)
	    @QueryResultContent,      -- QueryResultContent - nvarchar(max)
	    @Size,        -- Size - int
	    @Notify,     -- Notify - bit
	    @CreatedFrom,      -- CreatedFrom - nvarchar(255)
	    GETDATE() -- CreatedOn - datetime
	)

END;
GO




CREATE PROCEDURE [dbo].[Load Profilmatcher Assigned Query Notifications]
	@customerID NVARCHAR(50),
	@ID INT
	  
AS

BEGIN
    SET NOCOUNT ON;
		
	SELECT ID,
           Customer_ID,
           User_ID,
           EmployeeNumber,
           CustomerNumber,
           QueryName,
           QueryContent,
           QueryResultContent,
           Size,
           Notify,
           CreatedFrom,
           CreatedOn 
					 FROM dbo.[tbl_X28PMQueryNotifications]
	WHERE Customer_ID = @customerID
	AND (ID = @ID)

END;
GO




CREATE PROCEDURE [dbo].[Delete Profilmatcher Assigned Query Notifications]
	@customerID NVARCHAR(50),
	@ID INT
	  
AS

BEGIN
    SET NOCOUNT ON;
		
	DELETE dbo.[tbl_X28PMQueryNotifications]
	WHERE Customer_ID = @customerID
	AND (ID = @ID)

END;
GO




CREATE PROCEDURE [dbo].[Load Profilmatcher Query Notifications]
	@customerID NVARCHAR(50),
	@UserID NVARCHAR(50),
	@EmployeeNumber INT,
	@CustomerNumber INT
	  
AS

BEGIN
    SET NOCOUNT ON;
		
	SELECT ID,
           Customer_ID,
           [User_ID],
           EmployeeNumber,
           CustomerNumber,
           QueryName,
           QueryContent,
           QueryResultContent,
           Size,
           Notify,
           CreatedFrom,
           CreatedOn
					 FROM dbo.[tbl_X28PMQueryNotifications]
	WHERE Customer_ID = @customerID
	AND (@userID = '' OR [User_ID] = @userID)
	AND (@EmployeeNumber = 0 OR EmployeeNumber = @EmployeeNumber)
	AND (@CustomerNumber = 0 OR CustomerNumber = @CustomerNumber)
	ORDER BY CreatedOn DESC	

END;
GO




CREATE PROCEDURE [dbo].[Load Assigned Customer Experiences Postcode And Cities]
	@CustomerID NVARCHAR(50) 

AS

BEGIN
    SET NOCOUNT ON;

SELECT Ad.Postcode, Ad.City FROM dbo.tbl_CVLAddress Ad
	WHERE Ad.FK_PersonalID IN (SELECT Pin.ID FROM dbo.tbl_CVLPersonalInformation Pin WHERE Pin.ID IN (SELECT P.ID FROM dbo.tbl_CVLProfile P WHERE  P.Customer_ID = @CustomerID))
	AND FK_CountryCode = 'CH'
	AND (ISNULL(Ad.PostCode, '') <> '' AND ISNULL(Ad.City, '') <> '')
	GROUP BY Ad.Postcode, Ad.City  ORDER BY Ad.City
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned Customer Experiences Duration]
	@CustomerID NVARCHAR(50) ,
	@Code NVARCHAR(255)

AS

BEGIN
    SET NOCOUNT ON;

DECLARE @tbl TABLE(
        Experience_Code NVARCHAR(255),
Duration INT
)


INSERT INTO @tbl
(
    Experience_Code,
	Duration
)
SELECT Experience_Code, Experience_in_Month FROM dbo.tbl_CVLData_Experiences 
	WHERE Customer_ID = @CustomerID
	AND Experience_Code = @Code
	GROUP BY Experience_Code, Experience_in_Month  ORDER BY Experience_Code 


SELECT tb.Experience_Code,
	tb.Duration,
(
CASE 

WHEN ISNULL((SELECT TOP 1 Bez_DE FROM dbo.tbl_Base_Skill WHERE Code = tb.Experience_Code), '') = '' THEN
ISNULL((SELECT TOP 1 Bez_DE FROM dbo.tbl_Base_OperationArea WHERE Code = tb.Experience_Code), '')
 ELSE 
ISNULL((SELECT TOP 1 Bez_DE FROM dbo.tbl_Base_Skill WHERE Code = tb.Experience_Code), '')

END
) AS ExperienceLabel

 FROM @tbl	tb
ORDER BY ExperienceLabel
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned Customer Experiences Language]
	@CustomerID NVARCHAR(50) 

AS

BEGIN
    SET NOCOUNT ON;

DECLARE @tbl TABLE(
        LanguageCode NVARCHAR(10)
)

INSERT INTO @tbl
(
    LanguageCode
)
SELECT FK_LanguageCode	FROM dbo.tbl_CVLAddLanguages 
	WHERE FK_AddID IN 
	(SELECT Ad.ID  FROM dbo.tbl_CVLAddress Ad WHERE Ad.FK_PersonalID  IN (SELECT Pin.ID FROM dbo.tbl_CVLPersonalInformation Pin WHERE Pin.ID IN (SELECT P.ID FROM dbo.tbl_CVLProfile P WHERE  P.Customer_ID = @CustomerID)))
	AND ISNULL(FK_LanguageCode, '') <> '' 
	GROUP BY FK_LanguageCode 
	ORDER BY FK_LanguageCode


SELECT tb.LanguageCode,
ISNULL((SELECT TOP 1 Bez_DE FROM dbo.tbl_Base_ISOLanguage WHERE Code = tb.LanguageCode), '') 
 AS LanguageLabel

 FROM @tbl	tb
ORDER BY LanguageLabel
	
END;
GO




CREATE PROCEDURE [dbo].[TableTruncate] 
	@TableName NVARCHAR(128) 
AS 


 -- SET NOCOUNT ON added to prevent extra result sets from 
 -- interfering with SELECT statements. 
 SET NOCOUNT ON; 
 

 BEGIN TRAN 
 

 DECLARE @NextId NUMERIC = CASE WHEN (IDENT_CURRENT(@TableName) = 1) THEN 1 ELSE 0 END 
 DECLARE @Sql NVARCHAR(MAX) = 'DELETE FROM [' + @TableName + ']' 
 EXECUTE sp_executesql @Sql 
 

 IF (@@ERROR = 0) BEGIN 
 	DBCC CHECKIDENT (@TableName, RESEED, @NextId) 
 	 
 	COMMIT TRAN 
 END ELSE BEGIN 
 	-- Error 
 	ROLLBACK 
 END 
GO




CREATE PROCEDURE [dbo].[Load Assigned Customer Experiences Job Groups]
	@CustomerID NVARCHAR(50) 

AS
BEGIN
    SET NOCOUNT ON;

DECLARE @tbl TABLE(
        Code NVARCHAR(255)
)

INSERT INTO @tbl
(
    Code
)
SELECT Experience_Code FROM dbo.tbl_CVLData_Experiences WHERE Customer_ID = @CustomerID AND OperationArea = 1
	GROUP BY Experience_Code  ORDER BY Experience_Code 


SELECT tb.Code,
ISNULL((SELECT TOP 1 Bez_DE FROM dbo.tbl_Base_OperationArea WHERE Code = tb.Code), '') ExperienceLabel
 FROM @tbl	tb
ORDER BY ExperienceLabel
	
END;
GO




CREATE PROCEDURE [dbo].[Load Assigned Customer Experiences]
	@CustomerID NVARCHAR(50)

AS

BEGIN
    SET NOCOUNT ON;

DECLARE @tbl TABLE(
        Code NVARCHAR(255)
)

INSERT INTO @tbl
(
    Code
)
SELECT Experience_Code FROM dbo.tbl_CVLData_Experiences WHERE Customer_ID = @CustomerID AND Skill = 1
	GROUP BY Experience_Code  ORDER BY Experience_Code 


SELECT tb.Code,
ISNULL((SELECT TOP 1 Bez_DE FROM dbo.tbl_Base_Skill WHERE Code = tb.Code), '') ExperienceLabel
 FROM @tbl	tb
ORDER BY ExperienceLabel
	
END;
GO




CREATE PROCEDURE [dbo].[Load CVL Search Result Data]
	@CustomerID NVARCHAR(50),
	@postcode nvarchar(20),
	@radius INT,
	@jobTitels nvarchar(4000),
	@opAreaTitels nvarchar(4000),
	@competences nvarchar(4000),
	@languages nvarchar(4000)

AS

BEGIN
SET NOCOUNT ON

DECLARE @tblProfile TABLE (CVLID INT, PersonID INT);
DECLARE @tblPostCode TABLE (PersonID INT);

INSERT INTO @tblProfile (PersonID, CVLID) SELECT Pers.ID, Pers.FK_CVLID FROM [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation Pers
	WHERE Pers.FK_CVLID IN (SELECT P.ID FROM [spCVLizerBaseInfo].dbo.tbl_CVLProfile P WHERE P.Customer_ID = @CustomerID)
	AND (ISNULL(Pers.FirstName, '') <> '' AND ISNULL(Pers.LastName, '') <> '') 
	--AND Pers.DateOfBirth IS NOT NULL 

IF ISNULL(@postcode, '') <> ''
BEGIN

	IF ISNULL(@radius, 0) > 0
	BEGIN
		DECLARE @tblDistaces TABLE (PostCode NVARCHAR(10), Distance DECIMAL(12, 5))
		INSERT INTO @tblDistaces (PostCode, Distance) SELECT Postcode, Distance FROM [spPublicData].dbo.[Uf_Load Radius For Given Postfach]('CH', @postcode, @radius)

		INSERT INTO @tblPostCode (PersonID) SELECT A.FK_PersonalID PersonalID FROM [spCVLizerBaseInfo].dbo.tbl_CVLAddress A
			WHERE A.FK_PersonalID IN (SELECT P.PersonID FROM @tblProfile P) AND A.PostCode IN (SELECT Di.PostCode FROM @tblDistaces Di)
	END
	ELSE
	BEGIN
		INSERT INTO @tblPostCode (PersonID) SELECT A.FK_PersonalID PersonalID FROM [spCVLizerBaseInfo].dbo.tbl_CVLAddress A
			WHERE A.FK_PersonalID IN (SELECT P.PersonID FROM @tblProfile P)  AND A.PostCode = @postcode
	END
	
	DELETE FROM @tblProfile WHERE PersonID NOT IN (SELECT PC.PersonID FROM @tblPostCode PC);

END
--SELECT * FROM @tblProfile

IF ISNULL(@jobTitels, '') <> ''
BEGIN
DECLARE @tblJobTitleList TABLE (jobTitelLabel NVARCHAR(255));
INSERT INTO @tblJobTitleList SELECT KeyValue FROM [spPublicData].[dbo].[Uf_SplitMyString](@jobTitels,',')
--SELECT * FROM @tblJobTitleList

DECLARE @tblJobTitel TABLE (ProfileID INT);
INSERT INTO @tblJobTitel
(
    ProfileID
)
SELECT E.ProfileID FROM dbo.tbl_CVLData_Experiences E WHERE E.ProfileID IN (SELECT P.CVLID FROM @tblProfile P) 
		AND E.Experience_Code IN (SELECT CL.jobTitelLabel FROM @tblJobTitleList CL)
		AND E.JobTitel = 1;

	DELETE FROM @tblProfile WHERE CVLID NOT IN (SELECT C.ProfileID FROM @tblJobTitel C);

END

IF ISNULL(@opAreaTitels, '') <> ''
BEGIN
DECLARE @tblOPAreaList TABLE (opAreaLabel NVARCHAR(255));
INSERT INTO @tblOPAreaList SELECT KeyValue FROM [spPublicData].[dbo].[Uf_SplitMyString](@opAreaTitels,',')
--SELECT * FROM @tblOPAreaList

DECLARE @tblOPArea TABLE (ProfileID INT);
INSERT INTO @tblOPArea
(
    ProfileID
)
SELECT E.ProfileID FROM dbo.tbl_CVLData_Experiences E WHERE E.ProfileID IN (SELECT P.CVLID FROM @tblProfile P) 
		AND E.Experience_Code IN (SELECT CL.opAreaLabel FROM @tblOPAreaList CL)
		AND E.OperationArea = 1;

	DELETE FROM @tblProfile WHERE CVLID NOT IN (SELECT C.ProfileID FROM @tblOPArea C);

END


IF ISNULL(@competences, '') <> ''
BEGIN
DECLARE @tblCoompentencList TABLE (CompentencLabel NVARCHAR(255));
INSERT INTO @tblCoompentencList SELECT KeyValue FROM [spPublicData].[dbo].[Uf_SplitMyString](@competences,',')
--SELECT * FROM @tblCoompentencList

DECLARE @tblCompetence TABLE (ProfileID INT);
INSERT INTO @tblCompetence
(
    ProfileID
)
SELECT E.ProfileID FROM dbo.tbl_CVLData_Experiences E WHERE E.ProfileID IN (SELECT P.CVLID FROM @tblProfile P) 
		AND E.Experience_Code IN (SELECT CL.CompentencLabel FROM @tblCoompentencList CL)
		AND E.Skill = 1;

	DELETE FROM @tblProfile WHERE CVLID NOT IN (SELECT C.ProfileID FROM @tblCompetence C);

END


IF ISNULL(@languages, '') <> ''
BEGIN
DECLARE @tblLanguageList TABLE (languageLabel NVARCHAR(255));
INSERT INTO @tblLanguageList SELECT KeyValue FROM [spPublicData].[dbo].[Uf_SplitMyString](@languages,',')

DECLARE @tblAI TABLE(ProfileID INT, AdID INT)
INSERT INTO @tblAI
(
    ProfileID,
	AdID
)
SELECT AI.FK_CVLID, AI.ID FROM dbo.tbl_CVLAdditionalInformations AI WHERE AI.FK_CVLID IN (SELECT P.CVLID FROM @tblProfile P) 
	AND AI.ID IN (SELECT L.FK_AddID FROM dbo.tbl_CVLAddLanguages L WHERE L.FK_LanguageCode IN (SELECT LL.languageLabel From @tblLanguageList LL))

	DELETE FROM @tblProfile WHERE CVLID NOT IN (SELECT AI.ProfileID FROM @tblAI AI);

END

SELECT P.ID PersonalID, P.FK_CVLID CVLProfileID, P.FirstName, P.LastName, 
(	
CASE WHEN TRY_CAST(P.DateOfBirth AS DATETIME ) IS NOT NULL THEN  P.DateOfBirth 
ELSE NULL  
END
)
DateOfBirth

	, [spPublicData].dbo.[uf_CalculateAgeFromToday](P.DateOfBirth, NULL ) EmployeeAge

	, Profile.CreatedOn, Profile.Customer_ID, 
	(SELECT TOP 1 AP.EmployeeID FROM [applicant].dbo.tbl_applicant AP WHERE AP.Customer_ID = @CustomerID AND AP.CVLProfileID = Profile.ID) EmployeeID,
	(SELECT TOP 1 EX.Experience_Code FROM [spCVLizerBaseInfo].dbo.tbl_CVLData_Experiences EX WHERE EX.ProfileID = Profile.ID AND EX.JobTitel = 1 ORDER BY EX.Last_experience DESC) JobTitel,
	CA.Street, CA.PostCode, CA.City Location, CA.State, CA.FK_CountryCode CountryCode
FROM [spCVLizerBaseInfo].dbo.tbl_CVLPersonalInformation P 
LEFT JOIN [spCVLizerBaseInfo].dbo.tbl_CVLAddress CA ON CA.FK_PersonalID = P.ID	
LEFT JOIN [spCVLizerBaseInfo].dbo.tbl_CVLProfile Profile ON Profile.ID = P.FK_CVLID AND Profile.Customer_ID = @CustomerID
WHERE P.ID IN (SELECT tblP.PersonID FROM @tblProfile tblP) 
AND (ISNULL(P.FirstName, '') <> '' AND ISNULL(P.LastName, '') <> '')
ORDER BY P.LastName, P.FirstName

END;
GO




CREATE PROCEDURE [dbo].[Add CVLSearch Query Notification]
	@customerID NVARCHAR(50),
	@UserID NVARCHAR(50),
	@QueryContent NVARCHAR(max),
	@QueryResultContent NVARCHAR(max),
	@QueryName NVARCHAR(max),
	@Notify BIT ,
	
	@newID INT OUTPUT
	  
AS

BEGIN
    SET NOCOUNT ON;
	DECLARE @StartTranCount int
	DECLARE @OldID int

	DECLARE @userName NVARCHAR(255) 
	SET @userName = ISNULL((SELECT TOP 1 Lastname + ', ' + Firstname FROM [spSystemInfo].dbo.tbl_Customer_Advisors WHERE Customer_ID = @customerID AND User_ID = @UserID), '')

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

	SET @OldID = ISNULL((SELECT TOP 1 ID FROM [tbl_SearchCVLQueryNotifications] WHERE Customer_ID = @customerID AND User_ID = @UserID AND QueryName = @QueryName), 0)
	DELETE FROM [tbl_SearchCVLQueryResult] WHERE FK_SearchQueryID = @OldID
	DELETE FROM [tbl_SearchCVLQueryNotifications] WHERE ID = @OldID

	INSERT INTO dbo.[tbl_SearchCVLQueryNotifications]
	(
	    [Customer_ID],
	    [User_ID],
	    [QueryName],
	    [QueryContent],
	    [QueryResultContent],
	    [Notify],
	    [CreatedFrom],
	    [CreatedOn]
	)
	VALUES
	(   @customerID,      -- Customer_ID - nvarchar(50)
	    @UserID,      -- User_ID - nvarchar(50)
	    @QueryName,      -- QueryName - nvarchar(255)
	    @QueryContent,      -- QueryContent - nvarchar(max)
		@QueryResultContent,      -- QueryResultContent - nvarchar(max)
	    @Notify,     -- Notify - bit
	    @userName,      -- CreatedFrom - nvarchar(255)
	    GETDATE() -- CreatedOn - datetime
	)

		SET @newID = @@Identity

		IF @StartTranCount = 0 COMMIT TRAN
		
	END TRY
	BEGIN CATCH
		IF @StartTranCount = 0 AND @@trancount > 0
		BEGIN
			ROLLBACK TRAN
		END 

		DECLARE @message NVARCHAR(MAX)
		DECLARE @state INT
		SELECT @message = ERROR_MESSAGE(), @state = ERROR_STATE()
		RAISERROR (@message, 11, @state)

END CATCH

END;
GO




CREATE PROCEDURE [dbo].[Add CVLSearch Query Result]
	@customerID NVARCHAR(50),
	@UserID NVARCHAR(50),
	@FK_SearchQueryID INT,
	@CVLProfileID INT,
	@PersonalID INT,
	@EmployeeID INT,
	@Firstname NVARCHAR(255),
	@Lastname NVARCHAR(255),
	@Postcode NVARCHAR(20),
	@Street NVARCHAR(255),
	@Location NVARCHAR(255),
	@CountryCode NVARCHAR(2),
	@DateOfBirth DATETIME,
	@EmployeeAge INT,
	@JobTitel NVARCHAR(255),
	@CreatedOn DATETIME

AS

BEGIN
    SET NOCOUNT ON;
	DECLARE @StartTranCount int

	BEGIN TRY
		SET @StartTranCount = @@TRANCOUNT
		IF @StartTranCount = 0 BEGIN TRAN

	INSERT INTO dbo.[tbl_SearchCVLQueryResult]
	(
	    [FK_SearchQueryID],
	    [CVLProfileID],
	    [PersonalID],
	    [EmployeeID],
	    [Firstname],
	    [Lastname],
	    [Postcode],
	    [Street],
	    [Location],
	    [CountryCode],
	    [DateOfBirth],
	    [EmployeeAge],
	    [JobTitel],
	    [CreatedOn]
	)
	VALUES
	(   @FK_SearchQueryID,         -- FK_SearchQueryID - int
	    @CVLProfileID,         -- CVLProfileID - int
	    @PersonalID,         -- PersonalID - int
	    @EmployeeID,         -- EmployeeID - int
	    @Firstname,       -- Firstname - nvarchar(255)
	    @Lastname,       -- Lastname - nvarchar(255)
	    @Postcode,       -- Postcode - nvarchar(20)
	    @Street,       -- Street - nvarchar(255)
	    @Location,       -- Location - nvarchar(255)
	    @CountryCode,       -- CountryCode - nvarchar(2)
	    @DateOfBirth, -- DateOfBirtch - datetime
	    @EmployeeAge,         -- EmployeeAge - int
	    @JobTitel,       -- JobTitel - nvarchar(255)
	    @CreatedOn  -- CreatedOn - datetime
	)

		IF @StartTranCount = 0 COMMIT TRAN
		
	END TRY
	BEGIN CATCH
		IF @StartTranCount = 0 AND @@trancount > 0
		BEGIN
			ROLLBACK TRAN
		END 

		DECLARE @message NVARCHAR(MAX)
		DECLARE @state INT
		SELECT @message = ERROR_MESSAGE(), @state = ERROR_STATE()
		RAISERROR (@message, 11, @state)

END CATCH

END;
GO




CREATE PROCEDURE [dbo].[Load CVLSearch Query Notifications]
	@customerID NVARCHAR(50),
	@UserID NVARCHAR(50)
	  
AS

BEGIN
    SET NOCOUNT ON;
		
	SELECT ID,
           Customer_ID,
           User_ID,
           QueryName,
           QueryContent,
           QueryResultContent,
           Notify,
           CreatedFrom,
           CreatedOn,
(SELECT COUNT(*) FROM dbo.[tbl_SearchCVLQueryResult] WHERE FK_SearchQueryID = dbo.[tbl_SearchCVLQueryNotifications].ID) ResultCount
					 FROM dbo.[tbl_SearchCVLQueryNotifications]
	WHERE Customer_ID = @customerID
	AND (@userID = '' OR [User_ID] = @userID)
	ORDER BY CreatedOn DESC	

END;
GO




CREATE PROCEDURE [dbo].[Load CVLSearch Assigned Query Result Notifications]
	@customerID NVARCHAR(50),
	@searchID INT
	  
AS

BEGIN
    SET NOCOUNT ON;
		
	SELECT ID,
           FK_SearchQueryID,
           CVLProfileID,
           PersonalID,
           EmployeeID,
           Firstname,
           Lastname,
           Postcode,
           Street,
           Location,
           CountryCode,
           DateOfBirth,
           EmployeeAge,
           JobTitel,
           CreatedOn
					 FROM dbo.[tbl_SearchCVLQueryResult]
	WHERE (FK_SearchQueryID = @searchID)

END;
GO




CREATE PROCEDURE [dbo].[Update CVLSearch Assigned Query Notifier State]
	@customerID NVARCHAR(50),
	@searchID INT
	  
AS

BEGIN
    SET NOCOUNT ON;
		
UPDATE dbo.[tbl_SearchCVLQueryNotifications]
SET Notify = (CASE
                  WHEN Notify = 0 THEN
                      1
                  ELSE
                      0
              END
             )
WHERE Customer_ID = @customerID
      AND (ID = @searchID);

END;
GO




CREATE PROCEDURE [dbo].[Delete CVLSearch Assigned Query Notifications]
	@customerID NVARCHAR(50),
	@searchID INT
	  
AS

BEGIN
    SET NOCOUNT ON;
		
	DELETE FROM dbo.[tbl_SearchCVLQueryResult]
	WHERE (FK_SearchQueryID = @searchID)

	DELETE FROM dbo.[tbl_SearchCVLQueryNotifications]
	WHERE Customer_ID = @customerID
	AND (ID = @searchID)

END;
GO





CREATE PROCEDURE [dbo].[Load CVL Customer Data]
    @Customer_ID NVARCHAR(50)

AS

BEGIN
    SET NOCOUNT ON;

    SELECT P.Customer_ID,
           C.CustomerName,
           C.Location,
           C.CustomerNumber,
           C.CustomerGroupNumber
    FROM spCVLizerBaseInfo.dbo.tbl_CVLProfile P
        LEFT JOIN spSystemInfo.dbo.tbl_CustomerData C
            ON C.Customer_ID = P.Customer_ID
    WHERE (
              ISNULL(@Customer_ID, '') = ''
              OR P.Customer_ID = @Customer_ID
          )
    GROUP BY P.Customer_ID,
             C.CustomerName,
             C.Location,
             C.CustomerNumber,
             C.CustomerGroupNumber
    ORDER BY C.CustomerName,
             C.Location,
             C.CustomerNumber,
             C.CustomerGroupNumber;

END;
GO


/* ----------------------------- end of creating sp -------------------------------*/

/* ----------------------------- end of query -------------------------------*/

