USE [applicant]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvTransportation]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvTransportation]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvExtraInfo]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvExtraInfo]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvCustomArea]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvCustomArea]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvHobby]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvHobby]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvReference]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvReference]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvOther]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvOther]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvComputerSkill]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvComputerSkill]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvLanguageSkill]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvLanguageSkill]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvSoftSkill]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvSoftSkill]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvSkill]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvSkill]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvEducationHistory]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvEducationHistory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvEmploymentHistory]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvEmploymentHistory]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvProfile]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvProfile]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvPhoneNumber]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvPhoneNumber]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvEmail]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvEmail]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvSocialMedia]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvSocialMedia]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvPersonal]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvPersonal]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvAddress]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvAddress]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvApproval]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvApproval]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvAvailability]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvAvailability]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvComputerSkillType]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvComputerSkillType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvCountry]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvCountry]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvDegreeDirection]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvDegreeDirection]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvDiploma]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvDiploma]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvDocumentHtml]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvDocumentHtml]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvDocumentText]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvDocumentText]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvDriversLicence]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvDriversLicence]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvEducation]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvEducation]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvEducationDetail]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvEducationDetail]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvEducationLevel]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvEducationLevel]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvEmailType]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvEmailType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvGender]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvGender]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvHighestEducationLevel]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvHighestEducationLevel]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvInstituteType]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvInstituteType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvJobTitle]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvJobTitle]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvLanguageSkillType]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvLanguageSkillType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvLanguageProficiency]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvLanguageProficiency]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvMaritalStatus]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvMaritalStatus]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvNationality]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvNationality]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvPicture]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvPicture]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvPhoneNumberType]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvPhoneNumberType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvProfileStatus]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvProfileStatus]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvRegion]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvRegion]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvSalary]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvSalary]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvSocialMediaType]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvSocialMediaType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_CvSoftSkillType]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_CvSoftSkillType]
GO



CREATE TABLE [dbo].[tbl_CvApproval](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvApproval_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvApproval_U1] ON [dbo].[tbl_CvApproval]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvApproval_U2] ON [dbo].[tbl_CvApproval]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(1, N'SeasonalWorker', N'A', N'Saisonnier')
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(2, N'YearsOfResidence', N'B', N'Jahresaufenthalt')
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(3, N'Branch', N'C', N'Niederlassung')
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(4, N'AsylumSeeker', N'N', N'Asylant')
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(5, N'Refugee', N'F', N'Flüchtling')
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(6, N'Commuter', N'G', N'Grenzgänger')
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(7, N'CommuterWithholdingTax', N'Q', N'Grengänger mit Quellensteuer')
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(8, N'ShortTermResident', N'L', N'Kurzaufenthalter')
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(9, N'Rest', N'K', N'Uebrige')
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(10, N'Swiss', N'S', N'Schweizer')
INSERT INTO [dbo].[tbl_CvApproval]([ID],[Name],[Code],[Description]) VALUES(11, N'ReportingProcedures', N'M', N'Meldeverfahren')



CREATE TABLE [dbo].[tbl_CvAvailability](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvAvailability_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvAvailability_U1] ON [dbo].[tbl_CvAvailability]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvAvailability_U2] ON [dbo].[tbl_CvAvailability]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvAvailability]([ID],[Name],[Code],[Description]) VALUES(1, N'DirectlyAvailable', N'1', N'Direkt zur Verfügung')
INSERT INTO [dbo].[tbl_CvAvailability]([ID],[Name],[Code],[Description]) VALUES(2, N'MonthsLt1', N'2', N'0-1 Monate')
INSERT INTO [dbo].[tbl_CvAvailability]([ID],[Name],[Code],[Description]) VALUES(3, N'Months1', N'3', N'1 Monat')
INSERT INTO [dbo].[tbl_CvAvailability]([ID],[Name],[Code],[Description]) VALUES(4, N'Months2', N'4', N'2 Monate')
INSERT INTO [dbo].[tbl_CvAvailability]([ID],[Name],[Code],[Description]) VALUES(5, N'Months3', N'5', N'3 Monate')
INSERT INTO [dbo].[tbl_CvAvailability]([ID],[Name],[Code],[Description]) VALUES(6, N'Months3To6', N'6', N'3-6 Monate')
INSERT INTO [dbo].[tbl_CvAvailability]([ID],[Name],[Code],[Description]) VALUES(7, N'MonthsGt6', N'7', N'Länger als 6 Monate')
INSERT INTO [dbo].[tbl_CvAvailability]([ID],[Name],[Code],[Description]) VALUES(8, N'Negotiable', N'8', N'Verhandlungsbasis')



CREATE TABLE [dbo].[tbl_CvComputerSkillType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvComputerSkillType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvComputerSkillType_U1] ON [dbo].[tbl_CvComputerSkillType]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvComputerSkillType_U2] ON [dbo].[tbl_CvComputerSkillType]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
-- Daten werden dynamisch erzeugt




CREATE TABLE [dbo].[tbl_CvCountry](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvCountry_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvCountry_U1] ON [dbo].[tbl_CvCountry]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvCountry_U2] ON [dbo].[tbl_CvCountry]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(4, N'AFG', N'AF', N'Afghanistan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(8, N'ALB', N'AL', N'Albania')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(10, N'ATA', N'AQ', N'Antarctica')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(12, N'DZA', N'DZ', N'Algeria')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(16, N'ASM', N'AS', N'American Samoa')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(20, N'AND', N'AD', N'Andorra')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(24, N'AGO', N'AO', N'Angola')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(28, N'ATG', N'AG', N'Antigua and Barbuda')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(31, N'AZE', N'AZ', N'Azerbaijan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(32, N'ARG', N'AR', N'Argentina')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(36, N'AUS', N'AU', N'Australia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(40, N'AUT', N'AT', N'Austria')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(44, N'BHS', N'BS', N'Bahamas')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(48, N'BHR', N'BH', N'Bahrain')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(50, N'BGD', N'BD', N'Bangladesh')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(51, N'ARM', N'AM', N'Armenia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(52, N'BRB', N'BB', N'Barbados')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(56, N'BEL', N'BE', N'Belgium')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(60, N'BMU', N'BM', N'Bermuda')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(64, N'BTN', N'BT', N'Bhutan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(68, N'BOL', N'BO', N'Bolivia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(70, N'BIH', N'BA', N'Bosnien-Herzegowina')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(72, N'BWA', N'BW', N'Botswana')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(74, N'BVT', N'BV', N'Bouvet Island')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(76, N'BRA', N'BR', N'Brazil')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(84, N'BLZ', N'BZ', N'Belize')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(86, N'IOT', N'IO', N'British Indian Ocean Territory')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(90, N'SLB', N'SB', N'Solomon Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(92, N'VGB', N'VG', N'British Virgin Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(96, N'BRN', N'BN', N'Brunei')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(100, N'BGR', N'BG', N'Bulgaria')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(104, N'MMR', N'MM', N'Myanmar (Burma)')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(108, N'BDI', N'BI', N'Burundi')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(112, N'BLR', N'BY', N'Belarus')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(116, N'KHM', N'KH', N'Cambodia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(120, N'CMR', N'CM', N'Cameroon')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(124, N'CAN', N'CA', N'Canada')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(132, N'CPV', N'CV', N'Cape Verde')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(136, N'CYM', N'KY', N'Cayman Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(140, N'CAF', N'CF', N'Central African Rep.')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(144, N'LKA', N'LK', N'Sri Lanka')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(148, N'TCD', N'TD', N'Chad')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(152, N'CHL', N'CL', N'Chile')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(156, N'CHN', N'CN', N'China')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(158, N'TWN', N'TW', N'Taiwan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(162, N'CXR', N'CX', N'Christmas Island')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(166, N'CCK', N'CC', N'Cocos Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(170, N'COL', N'CO', N'Colombia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(174, N'COM', N'KM', N'Comoros')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(175, N'MYT', N'YT', N'Mayotte')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(178, N'COG', N'CG', N'Congo Brazzaville')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(180, N'COD', N'CD', N'Congo Kinshasa')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(184, N'COK', N'CK', N'Cook Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(188, N'CRI', N'CR', N'Costa Rica')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(191, N'HRV', N'HR', N'Croatia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(192, N'CUB', N'CU', N'Cuba')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(196, N'CYP', N'CY', N'Cyprus')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(203, N'CZE', N'CZ', N'Czech Republic')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(204, N'BEN', N'BJ', N'Benin')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(208, N'DNK', N'DK', N'Denmark')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(212, N'DMA', N'DM', N'Dominica')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(214, N'DOM', N'DO', N'Dominican Republic')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(218, N'ECU', N'EC', N'Ecuador')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(222, N'SLV', N'SV', N'El Salvador')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(226, N'GNQ', N'GQ', N'Equatorial Guinea')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(231, N'ETH', N'ET', N'Ethiopia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(232, N'ERI', N'ER', N'Eritrea')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(233, N'EST', N'EE', N'Estonia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(234, N'FRO', N'FO', N'Faroe Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(238, N'FLK', N'FK', N'Falkland Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(239, N'SGS', N'GS', N'South Georgia and the South Sandwich Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(242, N'FJI', N'FJ', N'Fiji')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(246, N'FIN', N'FI', N'Finland')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(248, N'ALA', N'AX', N'Åland')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(250, N'FRA', N'FR', N'France')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(254, N'GUF', N'GF', N'French Guiana')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(258, N'PYF', N'PF', N'French Polynesia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(260, N'ATF', N'TF', N'French Southern Territories')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(262, N'DJI', N'DJ', N'Djibouti')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(266, N'GAB', N'GA', N'Gabon')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(268, N'GEO', N'GE', N'Georgia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(270, N'GMB', N'GM', N'Gambia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(275, N'PSE', N'PS', N'Palestina')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(276, N'DEU', N'DE', N'Germany')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(288, N'GHA', N'GH', N'Ghana')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(292, N'GIB', N'GI', N'Gibraltar')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(296, N'KIR', N'KI', N'Kiribati')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(300, N'GRC', N'GR', N'Greece')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(304, N'GRL', N'GL', N'Greenland')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(308, N'GRD', N'GD', N'Grenada')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(312, N'GLP', N'GP', N'Guadeloupe')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(316, N'GUM', N'GU', N'Guam')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(320, N'GTM', N'GT', N'Guatemala')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(324, N'GIN', N'GN', N'Guinea')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(328, N'GUY', N'GY', N'Guyana')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(332, N'HTI', N'HT', N'Haiti')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(334, N'HMD', N'HM', N'Heard Island and McDonald Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(336, N'VAT', N'VA', N'Vatican City State')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(340, N'HND', N'HN', N'Honduras')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(344, N'HKG', N'HK', N'Hong Kong')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(348, N'HUN', N'HU', N'Hungary')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(352, N'ISL', N'IS', N'Iceland')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(356, N'IND', N'IN', N'India')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(360, N'IDN', N'ID', N'Indonesia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(364, N'IRN', N'IR', N'Iran')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(368, N'IRQ', N'IQ', N'Iraq')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(372, N'IRL', N'IE', N'Ireland')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(376, N'ISR', N'IL', N'Israel')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(380, N'ITA', N'IT', N'Italy')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(384, N'CIV', N'CI', N'Côte d´Ivoire')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(388, N'JAM', N'JM', N'Jamaika')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(392, N'JPN', N'JP', N'Japan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(398, N'KAZ', N'KZ', N'Kazakhstan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(400, N'JOR', N'JO', N'Jordan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(404, N'KEN', N'KE', N'Kenya')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(408, N'PRK', N'KP', N'North Korea')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(410, N'KOR', N'KR', N'South Korea')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(414, N'KWT', N'KW', N'Kuwait')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(417, N'KGZ', N'KG', N'Kyrgyzstan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(418, N'LAO', N'LA', N'Laos')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(422, N'LBN', N'LB', N'Lebanon')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(426, N'LSO', N'LS', N'Lesotho')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(428, N'LVA', N'LV', N'Latvia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(430, N'LBR', N'LR', N'Liberia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(434, N'LBY', N'LY', N'Libya')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(438, N'LIE', N'LI', N'Liechtenstein')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(440, N'LTU', N'LT', N'Lithuania')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(442, N'LUX', N'LU', N'Luxembourg')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(446, N'MAC', N'MO', N'Macao')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(450, N'MDG', N'MG', N'Madagascar')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(454, N'MWI', N'MW', N'Malawi')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(458, N'MYS', N'MY', N'Malaysia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(462, N'MDV', N'MV', N'Maldives')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(466, N'MLI', N'ML', N'Mali')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(470, N'MLT', N'MT', N'Malta')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(474, N'MTQ', N'MQ', N'Martinique')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(478, N'MRT', N'MR', N'Mauritania')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(480, N'MUS', N'MU', N'Mauritius')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(484, N'MEX', N'MX', N'Mexico')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(492, N'MCO', N'MC', N'Monaco')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(496, N'MNG', N'MN', N'Mongolia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(498, N'MDA', N'MD', N'Moldova')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(499, N'MNE', N'ME', N'Montenegro')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(500, N'MSR', N'MS', N'Montserrat')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(504, N'MAR', N'MA', N'Morocco')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(508, N'MOZ', N'MZ', N'Mozambique')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(512, N'OMN', N'OM', N'Oman')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(516, N'NAM', N'NA', N'Namibia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(520, N'NRU', N'NR', N'Nauru')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(524, N'NPL', N'NP', N'Nepal')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(528, N'NLD', N'NL', N'Netherlands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(530, N'ANT', N'AN', N'Netherlands Antilles')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(533, N'ABW', N'AW', N'Aruba')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(540, N'NCL', N'NC', N'New Caledonia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(548, N'VUT', N'VU', N'Vanuatu')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(554, N'NZL', N'NZ', N'New Zealand')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(558, N'NIC', N'NI', N'Nicaragua')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(562, N'NER', N'NE', N'Niger')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(566, N'NGA', N'NG', N'Nigeria')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(570, N'NIU', N'NU', N'Niue')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(574, N'NFK', N'NF', N'Norfolk Island')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(578, N'NOR', N'NO', N'Norway')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(580, N'MNP', N'MP', N'Northern Mariana Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(581, N'UMI', N'UM', N'United States Minor Outlying Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(583, N'FSM', N'FM', N'Micronesia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(584, N'MHL', N'MH', N'Marshall Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(585, N'PLW', N'PW', N'Palau')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(586, N'PAK', N'PK', N'Pakistan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(591, N'PAN', N'PA', N'Panama')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(598, N'PNG', N'PG', N'Papua New Guinea')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(600, N'PRY', N'PY', N'Paraguay')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(604, N'PER', N'PE', N'Peru')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(608, N'PHL', N'PH', N'Philippines')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(612, N'PCN', N'PN', N'Pitcairn')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(616, N'POL', N'PL', N'Poland')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(620, N'PRT', N'PT', N'Portugal')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(624, N'GNB', N'GW', N'Guinea Bissau')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(626, N'TLS', N'TL', N'East Timor')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(630, N'PRI', N'PR', N'Puerto Rico')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(634, N'QAT', N'QA', N'Qatar')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(638, N'REU', N'RE', N'Reunion')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(642, N'ROU', N'RO', N'Romania')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(643, N'RUS', N'RU', N'Russia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(646, N'RWA', N'RW', N'Rwanda')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(652, N'BLM', N'BL', N'Saint Barthélemy')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(654, N'SHN', N'SH', N'Saint Helena')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(659, N'KNA', N'KN', N'Saint Kitts and Nevis')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(660, N'AIA', N'AI', N'Anguilla')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(662, N'LCA', N'LC', N'Saint Lucia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(663, N'MAF', N'MF', N'Sint Maarten')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(666, N'SPM', N'PM', N'Saint Pierre and Miquelon')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(670, N'VCT', N'VC', N'Saint Vincent Grenadines')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(674, N'SMR', N'SM', N'San Marino')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(678, N'STP', N'ST', N'Sao Tome and Principe')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(682, N'SAU', N'SA', N'Saudi Arabia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(686, N'SEN', N'SN', N'Senegal')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(688, N'SRB', N'RS', N'Serbia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(690, N'SYC', N'SC', N'Seychelles')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(694, N'SLE', N'SL', N'Sierra Leone')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(702, N'SGP', N'SG', N'Singapore')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(703, N'SVK', N'SK', N'Slovakia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(704, N'VNM', N'VN', N'Vietnam')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(705, N'SVN', N'SI', N'Slovenia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(706, N'SOM', N'SO', N'Somalia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(710, N'ZAF', N'ZA', N'South Africa')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(716, N'ZWE', N'ZW', N'Zimbabwe')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(724, N'ESP', N'ES', N'Spain')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(732, N'ESH', N'EH', N'Spanish Western Sahara')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(736, N'SDN', N'SD', N'Sudan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(740, N'SUR', N'SR', N'Suriname')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(744, N'SJM', N'SJ', N'Svalbard and Jan Mayen')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(748, N'SWZ', N'SZ', N'Swaziland')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(752, N'SWE', N'SE', N'Sweden')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(756, N'CHE', N'CH', N'Switzerland')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(760, N'SYR', N'SY', N'Syria')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(762, N'TJK', N'TJ', N'Tajikistan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(764, N'THA', N'TH', N'Thailand')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(768, N'TGO', N'TG', N'Togo')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(772, N'TKL', N'TK', N'Tokelau')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(776, N'TON', N'TO', N'Tonga')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(780, N'TTO', N'TT', N'Trinidad and Tobago')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(784, N'ARE', N'AE', N'United Arab Emirates')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(788, N'TUN', N'TN', N'Tunisia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(792, N'TUR', N'TR', N'Turkey')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(795, N'TKM', N'TM', N'Turkmenistan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(796, N'TCA', N'TC', N'Turks and Caicos Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(798, N'TUV', N'TV', N'Tuvalu')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(800, N'UGA', N'UG', N'Uganda')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(804, N'UKR', N'UA', N'Ukraine')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(807, N'MKD', N'MK', N'Macedonia')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(818, N'EGY', N'EG', N'Egypt')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(826, N'GBR', N'GB', N'United Kingdom')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(831, N'GGY', N'GG', N'Guernsey')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(832, N'JEY', N'JE', N'Jersey')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(833, N'IMN', N'IM', N'Isle of Man')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(834, N'TZA', N'TZ', N'Tanzania')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(840, N'USA', N'US', N'United States')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(850, N'VIR', N'VI', N'U.S. Virgin Islands')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(854, N'BFA', N'BF', N'Burkina Faso')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(858, N'URY', N'UY', N'Uruguay')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(860, N'UZB', N'UZ', N'Uzbekistan')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(862, N'VEN', N'VE', N'Venezuela')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(876, N'WLF', N'WF', N'Wallis and Futuna')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(882, N'WSM', N'WS', N'Western Samoa')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(887, N'YEM', N'YE', N'Yemen')
INSERT INTO [dbo].[tbl_CvCountry]([ID],[Name],[Code],[Description]) VALUES(894, N'ZMB', N'ZM', N'Zambia')



CREATE TABLE [dbo].[tbl_CvDegreeDirection](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvDegreeDirection_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvDegreeDirection_U1] ON [dbo].[tbl_CvDegreeDirection]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvDegreeDirection_U2] ON [dbo].[tbl_CvDegreeDirection]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
-- Daten werden dynamisch erzeugt



CREATE TABLE [dbo].[tbl_CvDiploma](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvDiploma_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvDiploma_U1] ON [dbo].[tbl_CvDiploma]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvDiploma_U2] ON [dbo].[tbl_CvDiploma]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvDiploma]([ID],[Name],[Code],[Description]) VALUES(1, N'Yes', N'1', N'Ja')
INSERT INTO [dbo].[tbl_CvDiploma]([ID],[Name],[Code],[Description]) VALUES(2, N'No', N'2', N'Nein')
INSERT INTO [dbo].[tbl_CvDiploma]([ID],[Name],[Code],[Description]) VALUES(4, N'Unknown', N'4', N'Unbekannt')




CREATE TABLE [dbo].[tbl_CvDocumentHtml](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](max) NULL
 CONSTRAINT [tbl_CvDocumentHtml_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[tbl_CvDocumentText](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](max) NULL
 CONSTRAINT [tbl_CvDocumentText_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[tbl_CvDriversLicence](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvDriversLicence_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvDriversLicence_U1] ON [dbo].[tbl_CvDriversLicence]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvDriversLicence_U2] ON [dbo].[tbl_CvDriversLicence]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
-- Daten werden dynamisch erzeugt



CREATE TABLE [dbo].[tbl_CvEducation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvEducation_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvEducation_U1] ON [dbo].[tbl_CvEducation]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvEducation_U2] ON [dbo].[tbl_CvEducation]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
-- Daten werden dynamisch erzeugt



CREATE TABLE [dbo].[tbl_CvEducationDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvEducationDetail_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvEducationDetail_U1] ON [dbo].[tbl_CvEducationDetail]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvEducationDetail_U2] ON [dbo].[tbl_CvEducationDetail]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
-- Daten werden dynamisch erzeugt



CREATE TABLE [dbo].[tbl_CvEducationLevel](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvEducationLevel_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvEducationLevel_U1] ON [dbo].[tbl_CvEducationLevel]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvEducationLevel_U2] ON [dbo].[tbl_CvEducationLevel]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvEducationLevel]([ID],[Name],[Code],[Description]) VALUES(1, N'SecondaryEducation', N'1', N'Schulabschluss')
INSERT INTO [dbo].[tbl_CvEducationLevel]([ID],[Name],[Code],[Description]) VALUES(2, N'VocationalEducation', N'2', N'Berufsausbildung')
INSERT INTO [dbo].[tbl_CvEducationLevel]([ID],[Name],[Code],[Description]) VALUES(3, N'University', N'3', N'Universität')
INSERT INTO [dbo].[tbl_CvEducationLevel]([ID],[Name],[Code],[Description]) VALUES(4, N'Bachelor', N'4', N'Bachelor/Fachhochschule')
INSERT INTO [dbo].[tbl_CvEducationLevel]([ID],[Name],[Code],[Description]) VALUES(5, N'Master', N'5', N'Master')
INSERT INTO [dbo].[tbl_CvEducationLevel]([ID],[Name],[Code],[Description]) VALUES(6, N'PostMaster', N'6', N'Post-Master')
INSERT INTO [dbo].[tbl_CvEducationLevel]([ID],[Name],[Code],[Description]) VALUES(7, N'Course', N'7', N'Kurs')



CREATE TABLE [dbo].[tbl_CvEmailType](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvEmailType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvEmailType_U1] ON [dbo].[tbl_CvEmailType]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvEmailType_U2] ON [dbo].[tbl_CvEmailType]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvEmailType]([ID],[Name],[Code],[Description]) VALUES(1, N'Unspecified', N'1', N'Unspecified')
INSERT INTO [dbo].[tbl_CvEmailType]([ID],[Name],[Code],[Description]) VALUES(2, N'Corporate', N'2', N'Corporate')
INSERT INTO [dbo].[tbl_CvEmailType]([ID],[Name],[Code],[Description]) VALUES(3, N'NonCorporate', N'3', N'NonCorporate')



CREATE TABLE [dbo].[tbl_CvGender](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvGender_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvGender_U1] ON [dbo].[tbl_CvGender]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvGender_U2] ON [dbo].[tbl_CvGender]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvGender]([ID],[Name],[Code],[Description]) VALUES(0, N'NotKnown', N'0', N'Nicht bekannt')
INSERT INTO [dbo].[tbl_CvGender]([ID],[Name],[Code],[Description]) VALUES(1, N'Male', N'1', N'Mann')
INSERT INTO [dbo].[tbl_CvGender]([ID],[Name],[Code],[Description]) VALUES(2, N'Female', N'2', N'Frau')



CREATE TABLE [dbo].[tbl_CvHighestEducationLevel](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvHighestEducationLevel_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvHighestEducationLevel_U1] ON [dbo].[tbl_CvHighestEducationLevel]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvHighestEducationLevel_U2] ON [dbo].[tbl_CvHighestEducationLevel]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvHighestEducationLevel]([ID],[Name],[Code],[Description]) VALUES(1, N'SecondaryEducation', N'1', N'Schulabschluss')
INSERT INTO [dbo].[tbl_CvHighestEducationLevel]([ID],[Name],[Code],[Description]) VALUES(2, N'VocationalEducation', N'2', N'Berufsausbildung')
INSERT INTO [dbo].[tbl_CvHighestEducationLevel]([ID],[Name],[Code],[Description]) VALUES(3, N'University', N'3', N'Universität')
INSERT INTO [dbo].[tbl_CvHighestEducationLevel]([ID],[Name],[Code],[Description]) VALUES(4, N'Bachelor', N'4', N'Bachelor/Fachhochschule')
INSERT INTO [dbo].[tbl_CvHighestEducationLevel]([ID],[Name],[Code],[Description]) VALUES(5, N'Master', N'5', N'Master')
INSERT INTO [dbo].[tbl_CvHighestEducationLevel]([ID],[Name],[Code],[Description]) VALUES(6, N'PostMaster', N'6', N'Post-Master')



CREATE TABLE [dbo].[tbl_CvInstituteType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvInstituteType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvInstituteType_U1] ON [dbo].[tbl_CvInstituteType]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvInstituteType_U2] ON [dbo].[tbl_CvInstituteType]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
-- Daten werden dynamisch erzeugt



CREATE TABLE [dbo].[tbl_CvJobTitle](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvJobTitle_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvJobTitle_U1] ON [dbo].[tbl_CvJobTitle]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvJobTitle_U2] ON [dbo].[tbl_CvJobTitle]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
-- Daten werden dynamisch erzeugt



CREATE TABLE [dbo].[tbl_CvLanguageProficiency](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvLanguageProficiency_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvLanguageProficiency_U1] ON [dbo].[tbl_CvLanguageProficiency]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvLanguageProficiency_U2] ON [dbo].[tbl_CvLanguageProficiency]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvLanguageProficiency]([ID],[Name],[Code],[Description]) VALUES(1, N'No', N'1', N'Nein')
INSERT INTO [dbo].[tbl_CvLanguageProficiency]([ID],[Name],[Code],[Description]) VALUES(2, N'Elementary', N'2', N'Grundkenntnisse')
INSERT INTO [dbo].[tbl_CvLanguageProficiency]([ID],[Name],[Code],[Description]) VALUES(3, N'Good', N'3', N'Gute Kenntnisse')
INSERT INTO [dbo].[tbl_CvLanguageProficiency]([ID],[Name],[Code],[Description]) VALUES(4, N'Advanced', N'4', N'Fliessend')
INSERT INTO [dbo].[tbl_CvLanguageProficiency]([ID],[Name],[Code],[Description]) VALUES(5, N'BusinessFluent', N'5', N'Verhandlungssicher')
INSERT INTO [dbo].[tbl_CvLanguageProficiency]([ID],[Name],[Code],[Description]) VALUES(6, N'Native', N'6', N'Muttersprache')


CREATE TABLE [dbo].[tbl_CvLanguageSkillType](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvLanguageSkillType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvLanguageSkillType_U1] ON [dbo].[tbl_CvLanguageSkillType]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvLanguageSkillType_U2] ON [dbo].[tbl_CvLanguageSkillType]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(1, N'aar', N'AA', N'Afar')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(2, N'abk', N'AB', N'Abkhazian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(3, N'ave', N'AE', N'Avestan')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(4, N'afr', N'AF', N'Afrikaans')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(5, N'aka', N'AK', N'Akan')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(6, N'amh', N'AM', N'Amharic')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(7, N'arg', N'AN', N'Aragonese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(8, N'ara', N'AR', N'Arabic')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(9, N'asm', N'AS', N'Assamese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(10, N'ava', N'AV', N'Avaric')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(11, N'aym', N'AY', N'Aymara')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(12, N'aze', N'AZ', N'Azerbaijani')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(13, N'bak', N'BA', N'Bashkir')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(14, N'bel', N'BE', N'Belarusian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(15, N'bul', N'BG', N'Bulgarian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(16, N'bih', N'BH', N'Bihari')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(17, N'bis', N'BI', N'Bislama')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(18, N'bam', N'BM', N'Bambara')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(19, N'ben', N'BN', N'Bengali')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(20, N'tib/bod', N'BO', N'Tibetan')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(21, N'bre', N'BR', N'Breton')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(22, N'bos', N'BS', N'Bosnian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(23, N'cat', N'CA', N'Catalan')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(24, N'che', N'CE', N'Chechen')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(25, N'cha', N'CH', N'Chamorro')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(26, N'cos', N'CO', N'Corsican')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(27, N'cre', N'CR', N'Cree')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(28, N'cze/ces', N'CS', N'Czech')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(29, N'chv', N'CV', N'Chuvash')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(30, N'wel/cym', N'CY', N'Welsh')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(31, N'dan', N'DA', N'Danish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(32, N'ger/deu', N'DE', N'German')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(33, N'div', N'DV', N'Divehi')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(34, N'dzo', N'DZ', N'Dzongkha')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(35, N'ewe', N'EE', N'Ewe')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(36, N'ell', N'EL', N'Greek')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(37, N'eng', N'EN', N'English')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(38, N'epo', N'EO', N'Esperanto')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(39, N'spa', N'ES', N'Spanish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(40, N'est', N'ET', N'Estonian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(41, N'baq/eus', N'EU', N'Basque')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(42, N'per/fas', N'FA', N'Persian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(43, N'ful', N'FF', N'Fulah')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(44, N'fin', N'FI', N'Finnish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(45, N'fij', N'FJ', N'Fijian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(46, N'fao', N'FO', N'Faroese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(47, N'fre/fra', N'FR', N'French')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(48, N'fry', N'FY', N'West Frisian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(49, N'gle', N'GA', N'Irish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(50, N'gla', N'GD', N'Scottish Gaelic')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(51, N'grn', N'GN', N'Guarani')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(52, N'guj', N'GU', N'Gujarati')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(53, N'glv', N'GV', N'Manx')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(54, N'hau', N'HA', N'Hausa')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(55, N'heb', N'HE', N'Hebrew')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(56, N'hin', N'HI', N'Hindi')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(57, N'hrv', N'HR', N'Croatian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(58, N'hat', N'HT', N'Haitian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(59, N'hun', N'HU', N'Hungarian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(60, N'arm/hye', N'HY', N'Armenian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(61, N'her', N'HZ', N'Herero')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(62, N'ina', N'IA', N'Interlingua')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(63, N'ind', N'ID', N'Indonesian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(64, N'ibo', N'IG', N'Igbo')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(65, N'iii', N'II', N'Sichuan Yi')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(66, N'ipk', N'IK', N'Inupiaq')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(67, N'ice/isl', N'IS', N'Icelandic')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(68, N'ita', N'IT', N'Italian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(69, N'iku', N'IU', N'Inuktitut')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(70, N'jpn', N'JA', N'japanese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(71, N'jav', N'JV', N'Javanese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(72, N'geo/kat', N'KA', N'Georgian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(73, N'kon', N'KG', N'Kongo')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(74, N'kik', N'KI', N'Kikuyu')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(75, N'kua', N'KJ', N'Kwanyama')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(76, N'kaz', N'KK', N'Kazakh')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(77, N'kal', N'KL', N'Kalaallisut')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(78, N'khm', N'KM', N'Khmer')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(79, N'kan', N'KN', N'Kannada')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(80, N'kor', N'KO', N'Korean')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(81, N'kau', N'KR', N'Kanuri')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(82, N'kas', N'KS', N'Kashmiri')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(83, N'kur', N'KU', N'Kurdish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(84, N'kom', N'KV', N'Komi')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(85, N'cor', N'KW', N'Cornish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(86, N'kir', N'KY', N'Kirghiz')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(87, N'lat', N'LA', N'Latin')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(88, N'ltz', N'LB', N'Luxembourgish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(89, N'lug', N'LG', N'Ganda')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(90, N'lim', N'LI', N'Limburgish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(91, N'lin', N'LN', N'Lingala')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(92, N'lao', N'LO', N'Lao')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(93, N'lit', N'LT', N'Lithuanian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(94, N'lub', N'LU', N'Luba-Katanga')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(95, N'lav', N'LV', N'Latvian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(96, N'mne', N'ME', N'Montenegrin')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(97, N'mlg', N'MG', N'Malagasy')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(98, N'mah', N'MH', N'Marshallese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(99, N'mao/mri', N'MI', N'Maori')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(100, N'mac/mkd', N'MK', N'Macedonian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(101, N'mal', N'ML', N'Malayalam')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(102, N'mon', N'MN', N'Mongolian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(103, N'mar', N'MR', N'Marathi')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(104, N'may/msa', N'MS', N'Malay')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(105, N'mlt', N'MT', N'Maltese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(106, N'bur/mya', N'MY', N'Burmese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(107, N'nde', N'ND', N'North Ndebele')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(108, N'nep', N'NE', N'Nepali')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(109, N'ndo', N'NG', N'Ndonga')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(110, N'dut/nld', N'NL', N'Dutch')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(111, N'nor', N'NO', N'Norwegian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(112, N'nbl', N'NR', N'South Ndebele')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(113, N'nav', N'NV', N'Navajo')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(114, N'nya', N'NY', N'Chichewa')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(115, N'oci', N'OC', N'Occitan')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(116, N'oji', N'OJ', N'Ojibwa')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(117, N'orm', N'OM', N'Oromo')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(118, N'ori', N'OR', N'Oriya')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(119, N'oss', N'OS', N'Ossetian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(120, N'pan', N'PA', N'Panjabi')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(121, N'pol', N'PL', N'Polish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(122, N'pus', N'PS', N'Pashto')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(123, N'por', N'PT', N'Portuguese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(124, N'que', N'QU', N'Quechua')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(125, N'roh', N'RM', N'Rhaeto-Romance')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(126, N'run', N'RN', N'Rundi')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(127, N'rum/ron', N'RO', N'Romanian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(128, N'rus', N'RU', N'Russian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(129, N'kin', N'RW', N'Rwanda')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(130, N'san', N'SA', N'Sanskrit')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(131, N'srd', N'SC', N'Sardinian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(132, N'snd', N'SD', N'Sindhi')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(133, N'sme', N'SE', N'Northern Sami')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(134, N'sag', N'SG', N'Sango')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(135, N'sin', N'SI', N'Sinhala')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(136, N'slk/slo', N'SK', N'Slovak')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(137, N'slv', N'SL', N'Slovenian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(138, N'smo', N'SM', N'Samoan')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(139, N'sna', N'SN', N'Shona')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(140, N'som', N'SO', N'Somali')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(141, N'alb/sqi', N'SQ', N'Albanian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(142, N'srp', N'SR', N'Serbian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(143, N'ssw', N'SS', N'Swati')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(144, N'sot', N'ST', N'Southern Sotho')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(145, N'swe', N'SV', N'Swedish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(146, N'swa', N'SW', N'Swahili')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(147, N'tam', N'TA', N'Tamil')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(148, N'tel', N'TE', N'Telugu')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(149, N'tgk', N'TG', N'Tajik')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(150, N'tha', N'TH', N'Thai')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(151, N'tir', N'TI', N'Tigrinya')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(152, N'tuk', N'TK', N'Turkmen')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(153, N'tgl', N'TL', N'Tagalog')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(154, N'tsn', N'TN', N'Tswana')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(155, N'ton', N'TO', N'Tonga')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(156, N'tur', N'TR', N'Turkish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(157, N'tso', N'TS', N'Tsonga')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(158, N'twi', N'TW', N'Twi')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(159, N'tah', N'TY', N'Tahitian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(160, N'uig', N'UG', N'Uighur')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(161, N'ukr', N'UK', N'Ucrainian')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(162, N'urd', N'UR', N'Urdu')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(163, N'uzb', N'UZ', N'Uzbek')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(164, N'ven', N'VE', N'Venda')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(165, N'vie', N'VI', N'Vietnamese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(166, N'wol', N'WO', N'Wolof')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(167, N'xho', N'XH', N'Xhosa')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(168, N'yid', N'YI', N'Yiddish')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(169, N'yor', N'YO', N'Yoruba')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(170, N'zha', N'ZA', N'Zhuang')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(171, N'chi/zho', N'ZH', N'Chinese')
INSERT INTO [dbo].[tbl_CvLanguageSkillType]([ID],[Name],[Code],[Description]) VALUES(172, N'zul', N'ZU', N'Zulu')




CREATE TABLE [dbo].[tbl_CvMaritalStatus](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvMaritalStatus_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvMaritalStatus_U1] ON [dbo].[tbl_CvMaritalStatus]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvMaritalStatus_U2] ON [dbo].[tbl_CvMaritalStatus]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvMaritalStatus]([ID],[Name],[Code],[Description]) VALUES(1, N'Married', N'1', N'Verheiratet')
INSERT INTO [dbo].[tbl_CvMaritalStatus]([ID],[Name],[Code],[Description]) VALUES(2, N'Unmarried', N'2', N'Unverheiratet')
INSERT INTO [dbo].[tbl_CvMaritalStatus]([ID],[Name],[Code],[Description]) VALUES(3, N'LivingTogether', N'3', N'Zusammenlebende')
INSERT INTO [dbo].[tbl_CvMaritalStatus]([ID],[Name],[Code],[Description]) VALUES(4, N'Widowed', N'4', N'Verwitwet')
INSERT INTO [dbo].[tbl_CvMaritalStatus]([ID],[Name],[Code],[Description]) VALUES(5, N'Divorced', N'5', N'Geschieden')
INSERT INTO [dbo].[tbl_CvMaritalStatus]([ID],[Name],[Code],[Description]) VALUES(6, N'RegisteredPartnership', N'6', N'Eingetragenen Partnerschaft')



CREATE TABLE [dbo].[tbl_CvNationality](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvNationality_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvNationality_U1] ON [dbo].[tbl_CvNationality]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvNationality_U2] ON [dbo].[tbl_CvNationality]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(1, N'Andorran', N'AD', N'Andorran')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(2, N'Emirati', N'AE', N'Emirati')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(3, N'Afghan', N'AF', N'Afghan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(4, N'Antigua', N'AG', N'Antigua')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(5, N'Albanian', N'AL', N'Albanian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(6, N'Armenian', N'AM', N'Armenian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(7, N'Angolian', N'AO', N'Angolian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(8, N'Argentinian', N'AR', N'Argentinian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(9, N'Austrian', N'AT', N'Austrian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(10, N'Australian', N'AU', N'Australian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(11, N'Azerbaijan', N'AZ', N'Azerbaijani')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(12, N'Bosnian', N'BA', N'Bosnian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(13, N'Barbadian', N'BB', N'Barbadian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(14, N'Bangladeshi', N'BD', N'Bangladeshi')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(15, N'Belgian', N'BE', N'Belgian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(16, N'Burkina Fasose', N'BF', N'Burkinabe')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(17, N'Bulgarian', N'BG', N'Bulgarian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(18, N'Bahraini', N'BH', N'Bahraini')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(19, N'Burundian', N'BI', N'Burundian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(20, N'Benieise', N'BJ', N'Beninese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(21, N'Bruneian', N'BN', N'Bruneian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(22, N'Bolivian', N'BO', N'Bolivian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(23, N'Brazilian', N'BR', N'Brazilian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(24, N'Bahamian', N'BS', N'Bahamian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(25, N'Bhutanese', N'BT', N'Bhutanese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(26, N'Motswana', N'BW', N'Motswana')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(27, N'Belarusian', N'BY', N'Belarusian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(28, N'Belizean', N'BZ', N'Belizean')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(29, N'Canadian', N'CA', N'Canadian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(30, N'Congolese', N'CD', N'Congolese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(31, N'Central African', N'CF', N'Central African')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(32, N'Congolese-Brazzavillian', N'CG', N'Congolese-Brazzavillian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(33, N'Swiss', N'CH', N'Swiss')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(34, N'Ivorian', N'CI', N'Ivorian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(35, N'Chilean', N'CL', N'Chilean')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(36, N'Comoronian', N'CM', N'Cameroonian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(37, N'Chinese', N'CN', N'Chinese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(38, N'Colombian', N'CO', N'Colombian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(39, N'Costa Rican', N'CR', N'Costa Rican')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(40, N'Cuban', N'CU', N'Cuban')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(41, N'Cape Verdean', N'CV', N'Cape Verdean')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(42, N'Cypriot', N'CY', N'Cypriot')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(43, N'Czech', N'CZ', N'Czech')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(44, N'German', N'DE', N'German')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(45, N'Djiboutian', N'DJ', N'Djiboutian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(46, N'Danish', N'DK', N'Danish')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(47, N'Dominican (Dominica)', N'DM', N'Dominican (Dominica)')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(48, N'Dominican', N'DO', N'Dominican (Dominican Republic)')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(49, N'Algerian', N'DZ', N'Algerian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(50, N'Ecadorian', N'EC', N'Ecuadorian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(51, N'Estonian', N'EE', N'Estonian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(52, N'Egyptian', N'EG', N'Egyptian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(53, N'Eritean', N'ER', N'Eritrean')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(54, N'Spanish', N'ES', N'Spanish')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(55, N'Ethiopan', N'ET', N'Ethiopian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(56, N'Finnish', N'FI', N'Finnish')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(57, N'Fijian', N'FJ', N'Fijian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(58, N'Micronesian', N'FM', N'Micronesian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(59, N'French', N'FR', N'French')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(60, N'Gabonese', N'GA', N'Gabonese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(61, N'British', N'GB', N'British')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(62, N'Grendian', N'GD', N'Grendian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(63, N'Georgian', N'GE', N'Georgian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(64, N'Ghanaian', N'GH', N'Ghanaian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(65, N'Gambian', N'GM', N'Gambian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(66, N'Guinean', N'GN', N'Guinean')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(67, N'Equatorial Guinean', N'GQ', N'Equatorial Guinean')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(68, N'Greek', N'GR', N'Greek')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(69, N'Guatemalan', N'GT', N'Guatemalan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(70, N'Guinean (Bissau)', N'GW', N'Guinean (Bissau)')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(71, N'Guyanese', N'GY', N'Guyanese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(72, N'Honduran', N'HN', N'Honduran')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(73, N'Croatian', N'HR', N'Croatian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(74, N'Haitian', N'HT', N'Haitian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(75, N'Hungarian', N'HU', N'Hungarian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(76, N'Indonesian', N'ID', N'Indonesian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(77, N'Irish', N'IE', N'Irish')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(78, N'Israelian', N'IL', N'Israeli')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(79, N'Indian', N'IN', N'Indian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(80, N'Iraqi', N'IQ', N'Iraqi')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(81, N'Iranian', N'IR', N'Iranian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(82, N'Icelandic', N'IS', N'Icelandic')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(83, N'Italian', N'IT', N'Italian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(84, N'Jamaican', N'JM', N'Jamaican')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(85, N'Jordanian', N'JO', N'Jordanian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(86, N'Japanese', N'JP', N'Japanese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(87, N'Kenyan', N'KE', N'Kenyan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(88, N'Kyrgyz', N'KG', N'Kyrgyz')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(89, N'Cambodian', N'KH', N'Cambodian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(90, N'Kiribatian', N'KI', N'I-Kiribati')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(91, N'Comorian', N'KM', N'Comorian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(92, N'Kittitian', N'KN', N'Kittitian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(93, N'North Korean', N'KP', N'North Korean')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(94, N'South Korean', N'KR', N'South Korean')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(95, N'Kuwati', N'KW', N'Kuwaiti')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(96, N'Kazakh', N'KZ', N'Kazakh')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(97, N'Lao', N'LA', N'Lao')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(98, N'Lebanese', N'LB', N'Lebanese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(99, N'St. Lucian', N'LC', N'St. Lucian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(100, N'Liechtenstein', N'LI', N'Liechtenstein')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(101, N'Sri Lankan', N'LK', N'Sri Lankan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(102, N'Liberian', N'LR', N'Liberian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(103, N'Basotho', N'LS', N'Basotho')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(104, N'Lithanian', N'LT', N'Lithuanian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(105, N'Luxembourgish', N'LU', N'Luxembourgish')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(106, N'Latvian', N'LV', N'Latvian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(107, N'Lybian', N'LY', N'Libyan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(108, N'Moroccan', N'MA', N'Moroccan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(109, N'Monegasque', N'MC', N'Monegasque')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(110, N'Moldavian', N'MD', N'Moldavian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(111, N'Malagasy', N'MG', N'Malagasy')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(112, N'Marshallese', N'MH', N'Marshallese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(113, N'Macedonian', N'MK', N'Macedonian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(114, N'Malayan', N'ML', N'Malian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(115, N'Myanmar', N'MM', N'Burmese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(116, N'Mongolian', N'MN', N'Mongolian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(117, N'Mauritanian', N'MR', N'Mauritanian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(118, N'Maltese', N'MT', N'Maltese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(119, N'Mauritian', N'MU', N'Mauritian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(120, N'Maldivian', N'MV', N'Maldivian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(121, N'Malawian', N'MW', N'Malawian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(122, N'Mexican', N'MX', N'Mexican')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(123, N'Malaysian', N'MY', N'Malaysian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(124, N'Mozambican', N'MZ', N'Mozambican')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(125, N'Namibian', N'NA', N'Namibian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(126, N'Nigerien', N'NE', N'Nigerien')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(127, N'Nigerian', N'NG', N'Nigerian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(128, N'Nicaraguan', N'NI', N'Nicaraguan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(129, N'Dutch', N'NL', N'Dutch')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(130, N'Norwegian', N'NO', N'Norwegian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(131, N'Nepalese', N'NP', N'Nepalese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(132, N'Nauruan', N'NR', N'Nauruan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(133, N'New Zealander', N'NZ', N'New Zealander')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(134, N'Omani', N'OM', N'Omani')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(135, N'Panamanian', N'PA', N'Panamanian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(136, N'Peruvian', N'PE', N'Peruvian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(137, N'Papuan', N'PG', N'Papuan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(138, N'Filipino', N'PH', N'Filipino')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(139, N'Pakistanian', N'PK', N'Pakistani')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(140, N'Polish', N'PL', N'Polish')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(141, N'Portuguese', N'PT', N'Portuguese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(142, N'Palauan', N'PW', N'Palauan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(143, N'Paraguayan', N'PY', N'Paraguayan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(144, N'Qatarian', N'QA', N'Qatari')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(145, N'Romanian', N'RO', N'Romanian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(146, N'Russian', N'RU', N'Russian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(147, N'Rwandan', N'RW', N'Rwandan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(148, N'Saudi-Arabian', N'SA', N'Saudi-Arabian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(149, N'Solomon Islander', N'SB', N'Solomon Islander')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(150, N'Seychellois', N'SC', N'Seychellois')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(151, N'Sudanese', N'SD', N'Sudanese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(152, N'Swedish', N'SE', N'Swedish')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(153, N'Singaporian', N'SG', N'Singaporean')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(154, N'Slovanian', N'SI', N'Slovenian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(155, N'Slovakian', N'SK', N'Slovak')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(156, N'Sierra Leonean', N'SL', N'Sierra Leonean')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(157, N'Sammarinese', N'SM', N'Sammarinese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(158, N'Senegalese', N'SN', N'Senegalese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(159, N'Somali', N'SO', N'Somali')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(160, N'Surinamese', N'SR', N'Surinamese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(161, N'São Toméan', N'ST', N'São Toméan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(162, N'Salvadoran', N'SV', N'Salvadoran')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(163, N'Syrian', N'SY', N'Syrian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(164, N'Swazi', N'SZ', N'Swazi')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(165, N'Chadian', N'TD', N'Chadian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(166, N'Togolese', N'TG', N'Togolese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(167, N'Thai', N'TH', N'Thai')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(168, N'Tajik', N'TJ', N'Tajikistani')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(169, N'Turkmen', N'TM', N'Turkmen')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(170, N'Tunisian', N'TN', N'Tunisian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(171, N'Tongan', N'TO', N'Tongan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(172, N'Turkish', N'TR', N'Turkish')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(173, N'Trinidadian', N'TT', N'Trinidadian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(174, N'Tuvaluan', N'TV', N'Tuvaluan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(175, N'Taiwanese', N'TW', N'Taiwanese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(176, N'Tanzanian', N'TZ', N'Tanzanian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(177, N'Ukrainian', N'UA', N'Ukrainian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(178, N'Ugandan', N'UG', N'Ugandan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(179, N'American', N'US', N'American')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(180, N'Uruguayan', N'UY', N'Uruguayan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(181, N'Uzbeckistani', N'UZ', N'Uzbekistani')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(182, N'Vatican', N'VA', N'Vatican')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(183, N'St. Vincentian', N'VC', N'St. Vincentian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(184, N'Venezuelan', N'VE', N'Venezuelan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(185, N'Vietnamese', N'VN', N'Vietnamese')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(186, N'Vanuatuan', N'VU', N'Vanuatuan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(187, N'Samoan', N'WS', N'Samoan')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(188, N'Yemeni', N'YE', N'Yemeni')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(189, N'Serbian', N'YU', N'Serbian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(190, N'South African', N'ZA', N'South African')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(191, N'Zambian', N'ZM', N'Zambian')
INSERT INTO [dbo].[tbl_CvNationality]([ID],[Name],[Code],[Description]) VALUES(192, N'Zimbabwean', N'ZW', N'Zimbabwean')



CREATE TABLE [dbo].[tbl_CvPicture](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Content] [varbinary](max) NULL,
	[Filename] [nvarchar](255) NULL,
	[ContentType] [nvarchar](255) NULL
 CONSTRAINT [tbl_CvPicture_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[tbl_CvPhoneNumberType](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvPhoneNumberType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvPhoneNumberType_U1] ON [dbo].[tbl_CvPhoneNumberType]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvPhoneNumberType_U2] ON [dbo].[tbl_CvPhoneNumberType]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvPhoneNumberType]([ID],[Name],[Code],[Description]) VALUES(1, N'Mobile', N'1', N'Mobile')
INSERT INTO [dbo].[tbl_CvPhoneNumberType]([ID],[Name],[Code],[Description]) VALUES(2, N'Home', N'2', N'Home')
INSERT INTO [dbo].[tbl_CvPhoneNumberType]([ID],[Name],[Code],[Description]) VALUES(3, N'Fax', N'3', N'Fax')



CREATE TABLE [dbo].[tbl_CvProfileStatus](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvProfileStatus_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvProfileStatus_U1] ON [dbo].[tbl_CvProfileStatus]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvProfileStatus_U2] ON [dbo].[tbl_CvProfileStatus]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvProfileStatus]([ID],[Name],[Code],[Description]) VALUES(1, N'New', N'1', N'Neu')
INSERT INTO [dbo].[tbl_CvProfileStatus]([ID],[Name],[Code],[Description]) VALUES(2, N'Active', N'2', N'Aktiv')
INSERT INTO [dbo].[tbl_CvProfileStatus]([ID],[Name],[Code],[Description]) VALUES(3, N'Inactive', N'3', N'Inaktiv')
INSERT INTO [dbo].[tbl_CvProfileStatus]([ID],[Name],[Code],[Description]) VALUES(4, N'Canceled', N'4', N'Abgebrochen')
INSERT INTO [dbo].[tbl_CvProfileStatus]([ID],[Name],[Code],[Description]) VALUES(5, N'Anonymous', N'5', N'Anonym')



CREATE TABLE [dbo].[tbl_CvRegion](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvRegion_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvRegion_U1] ON [dbo].[tbl_CvRegion]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvRegion_U2] ON [dbo].[tbl_CvRegion]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(1, N'NL-NH', N'1', N'NL Noord-Holland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(2, N'NL-UT', N'2', N'NL Utrecht')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(3, N'NL-ZH', N'3', N'NL Zuid-Holland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(4, N'NL-GE', N'4', N'NL Gelderland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(5, N'NL-OV', N'5', N'NL Overijssel')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(6, N'NL-NB', N'6', N'NL Noord-Brabant')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(7, N'NL-ZE', N'7', N'NL Zeeland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(8, N'NL-LI', N'8', N'NL Limburg')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(9, N'NL-DR', N'9', N'NL Drenthe')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(10, N'NL-FL', N'10', N'NL Flevoland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(11, N'NL-GR', N'11', N'NL Groningen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(12, N'NL-FR', N'12', N'NL Friesland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(14, N'GB-ABD', N'14', N'UK Aberdeenshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(15, N'GB-ABE', N'15', N'UK Aberdeen City')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(16, N'GB-AGB', N'16', N'UK Argyll and Bute')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(17, N'GB-AGY', N'17', N'UK Isle of Anglesey')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(18, N'GB-ANS', N'18', N'UK Angus')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(19, N'GB-ANT', N'19', N'UK Antrim')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(20, N'GB-ARD', N'20', N'UK Ards')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(21, N'GB-ARM', N'21', N'UK Armagh')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(22, N'GB-BAS', N'22', N'UK Bath and North East Somerset')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(23, N'GB-BBD', N'23', N'UK Blackburn with Darwen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(24, N'GB-BDF', N'24', N'UK Bedford')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(25, N'GB-BDG', N'25', N'UK Barking and Dagenham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(26, N'GB-BEN', N'26', N'UK Brent')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(27, N'GB-BEX', N'27', N'UK Bexley')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(28, N'GB-BFS', N'28', N'UK Belfast')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(29, N'GB-BGE', N'29', N'UK Bridgend')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(30, N'GB-BGW', N'30', N'UK Blaenau Gwent')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(31, N'GB-BIR', N'31', N'UK Birmingham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(32, N'GB-BKM', N'32', N'UK Buckinghamshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(33, N'GB-BLA', N'33', N'UK Ballymena')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(34, N'GB-BLY', N'34', N'UK Ballymoney')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(35, N'GB-BMH', N'35', N'UK Bournemouth')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(36, N'GB-BNB', N'36', N'UK Banbridge')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(37, N'GB-BNE', N'37', N'UK Barnet')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(38, N'GB-BNH', N'38', N'UK Brighton and Hove')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(39, N'GB-BNS', N'39', N'UK Barnsley')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(40, N'GB-BOL', N'40', N'UK Bolton')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(41, N'GB-BPL', N'41', N'UK Blackpool')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(42, N'GB-BRC', N'42', N'UK Bracknell Forest')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(43, N'GB-BRD', N'43', N'UK Bradford')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(44, N'GB-BRY', N'44', N'UK Bromley')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(45, N'GB-BST', N'45', N'UK City of Bristol')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(46, N'GB-BUR', N'46', N'UK Bury')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(47, N'GB-CAM', N'47', N'UK Cambridgeshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(48, N'GB-CAY', N'48', N'UK Caerphilly')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(49, N'GB-CBF', N'49', N'UK Central Bedfordshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(50, N'GB-CGN', N'50', N'UK Ceredigion')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(51, N'GB-CGV', N'51', N'UK Craigavon')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(52, N'GB-CHE', N'52', N'UK Cheshire East')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(53, N'GB-CHW', N'53', N'UK Cheshire West and Chester')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(54, N'GB-CKF', N'54', N'UK Carrickfergus')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(55, N'GB-CKT', N'55', N'UK Cookstown')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(56, N'GB-CLD', N'56', N'UK Calderdale')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(57, N'GB-CLK', N'57', N'UK Clackmannanshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(58, N'GB-CLR', N'58', N'UK Coleraine')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(59, N'GB-CMA', N'59', N'UK Cumbria')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(60, N'GB-CMD', N'60', N'UK Camden')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(61, N'GB-CMN', N'61', N'UK Carmarthenshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(62, N'GB-CON', N'62', N'UK Cornwall')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(63, N'GB-COV', N'63', N'UK Coventry')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(64, N'GB-CRF', N'64', N'UK Cardiff')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(65, N'GB-CRY', N'65', N'UK Croydon')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(66, N'GB-CSR', N'66', N'UK Castlereagh')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(67, N'GB-CWY', N'67', N'UK Conwy')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(68, N'GB-DAL', N'68', N'UK Darlington')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(69, N'GB-DBY', N'69', N'UK Derbyshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(70, N'GB-DEN', N'70', N'UK Denbighshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(71, N'GB-DER', N'71', N'UK Derby')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(72, N'GB-DEV', N'72', N'UK Devon')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(73, N'GB-DGN', N'73', N'UK Dungannon and South Tyrone')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(74, N'GB-DGY', N'74', N'UK Dumfries and Galloway')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(75, N'GB-DNC', N'75', N'UK Doncaster')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(76, N'GB-DND', N'76', N'UK Dundee City')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(77, N'GB-DOR', N'77', N'UK Dorset')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(78, N'GB-DOW', N'78', N'UK Down')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(79, N'GB-DRY', N'79', N'UK Derry')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(80, N'GB-DUD', N'80', N'UK Dudley')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(81, N'GB-DUR', N'81', N'UK County Durham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(82, N'GB-EAL', N'82', N'UK Ealing')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(83, N'GB-EAY', N'83', N'UK East Ayrshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(84, N'GB-EDH', N'84', N'UK City of Edinburgh')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(85, N'GB-EDU', N'85', N'UK East Dunbartonshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(86, N'GB-ELN', N'86', N'UK East Lothian')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(87, N'GB-ELS', N'87', N'UK Eilean Siar')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(88, N'GB-ENF', N'88', N'UK Enfield')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(89, N'GB-ERW', N'89', N'UK East Renfrewshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(90, N'GB-ERY', N'90', N'UK East Riding of Yorkshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(91, N'GB-ESS', N'91', N'UK Essex')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(92, N'GB-ESX', N'92', N'UK East Sussex')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(93, N'GB-FAL', N'93', N'UK Falkirk')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(94, N'GB-FER', N'94', N'UK Fermanagh')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(95, N'GB-FIF', N'95', N'UK Fife')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(96, N'GB-FLN', N'96', N'UK Flintshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(97, N'GB-GAT', N'97', N'UK Gateshead')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(98, N'GB-GLG', N'98', N'UK Glasgow City')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(99, N'GB-GLS', N'99', N'UK Gloucestershire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(100, N'GB-GRE', N'100', N'UK Greenwich')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(101, N'GB-GWN', N'101', N'UK Gwynedd')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(102, N'GB-HAL', N'102', N'UK Halton')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(103, N'GB-HAM', N'103', N'UK Hampshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(104, N'GB-HAV', N'104', N'UK Havering')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(105, N'GB-HCK', N'105', N'UK Hackney')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(106, N'GB-HEF', N'106', N'UK Herefordshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(107, N'GB-HIL', N'107', N'UK Hillingdon')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(108, N'GB-HLD', N'108', N'UK Highland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(109, N'GB-HMF', N'109', N'UK Hammersmith and Fulham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(110, N'GB-HNS', N'110', N'UK Hounslow')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(111, N'GB-HPL', N'111', N'UK Hartlepool')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(112, N'GB-HRT', N'112', N'UK Hertfordshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(113, N'GB-HRW', N'113', N'UK Harrow')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(114, N'GB-HRY', N'114', N'UK Haringey')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(115, N'GB-IOW', N'115', N'UK Isle of Wight')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(116, N'GB-ISL', N'116', N'UK Islington')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(117, N'GB-IVC', N'117', N'UK Inverclyde')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(118, N'GB-KEC', N'118', N'UK Kensington and Chelsea')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(119, N'GB-KEN', N'119', N'UK Kent')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(120, N'GB-KHL', N'120', N'UK Kingston upon Hull')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(121, N'GB-KIR', N'121', N'UK Kirklees')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(122, N'GB-KTT', N'122', N'UK Kingston upon Thames')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(123, N'GB-KWL', N'123', N'UK Knowsley')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(124, N'GB-LAN', N'124', N'UK Lancashire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(125, N'GB-LBH', N'125', N'UK Lambeth')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(126, N'GB-LCE', N'126', N'UK Leicester')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(127, N'GB-LDS', N'127', N'UK Leeds')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(128, N'GB-LEC', N'128', N'UK Leicestershire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(129, N'GB-LEW', N'129', N'UK Lewisham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(130, N'GB-LIN', N'130', N'UK Lincolnshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(131, N'GB-LIV', N'131', N'UK Liverpool')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(132, N'GB-LMV', N'132', N'UK Limavady')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(133, N'GB-LND', N'133', N'UK City of London')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(134, N'GB-LRN', N'134', N'UK Larne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(135, N'GB-LSB', N'135', N'UK Lisburn')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(136, N'GB-LUT', N'136', N'UK Luton')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(137, N'GB-MAN', N'137', N'UK Manchester')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(138, N'GB-MDB', N'138', N'UK Middlesbrough')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(139, N'GB-MDW', N'139', N'UK Medway')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(140, N'GB-MFT', N'140', N'UK Magherafelt')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(141, N'GB-MIK', N'141', N'UK Milton Keynes')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(142, N'GB-MLN', N'142', N'UK Midlothian')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(143, N'GB-MON', N'143', N'UK Monmouthshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(144, N'GB-MRT', N'144', N'UK Merton')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(145, N'GB-MRY', N'145', N'UK Moray')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(146, N'GB-MTY', N'146', N'UK Merthyr Tydfil')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(147, N'GB-MYL', N'147', N'UK Moyle')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(148, N'GB-NAY', N'148', N'UK North Ayrshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(149, N'GB-NBL', N'149', N'UK Northumberland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(150, N'GB-NDN', N'150', N'UK North Down')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(151, N'GB-NEL', N'151', N'UK North East Lincolnshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(152, N'GB-NET', N'152', N'UK Newcastle upon Tyne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(153, N'GB-NFK', N'153', N'UK Norfolk')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(154, N'GB-NGM', N'154', N'UK Nottingham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(155, N'GB-NLK', N'155', N'UK North Lanarkshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(156, N'GB-NLN', N'156', N'UK North Lincolnshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(157, N'GB-NSM', N'157', N'UK North Somerset')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(158, N'GB-NTA', N'158', N'UK Newtownabbey')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(159, N'GB-NTH', N'159', N'UK Northamptonshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(160, N'GB-NTL', N'160', N'UK Neath Port Talbot')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(161, N'GB-NTT', N'161', N'UK Nottinghamshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(162, N'GB-NTY', N'162', N'UK North Tyneside')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(163, N'GB-NWM', N'163', N'UK Newham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(164, N'GB-NWP', N'164', N'UK Newport')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(165, N'GB-NYK', N'165', N'UK North Yorkshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(166, N'GB-NYM', N'166', N'UK Newry and Mourne District')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(167, N'GB-OLD', N'167', N'UK Oldham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(168, N'GB-OMH', N'168', N'UK Omagh')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(169, N'GB-ORK', N'169', N'UK Orkney Islands')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(170, N'GB-OXF', N'170', N'UK Oxfordshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(171, N'GB-PEM', N'171', N'UK Pembrokeshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(172, N'GB-PKN', N'172', N'UK Perth and Kinross')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(173, N'GB-PLY', N'173', N'UK Plymouth')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(174, N'GB-POL', N'174', N'UK Poole')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(175, N'GB-POR', N'175', N'UK Portsmouth')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(176, N'GB-POW', N'176', N'UK Powys')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(177, N'GB-PTE', N'177', N'UK Peterborough')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(178, N'GB-RCC', N'178', N'UK Redcar and Cleveland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(179, N'GB-RCH', N'179', N'UK Rochdale')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(180, N'GB-RCT', N'180', N'UK Rhondda, Cynon, Taff')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(181, N'GB-RDB', N'181', N'UK Redbridge')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(182, N'GB-RDG', N'182', N'UK Reading')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(183, N'GB-RFW', N'183', N'UK Renfrewshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(184, N'GB-RIC', N'184', N'UK Richmond upon Thames')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(185, N'GB-ROT', N'185', N'UK Rotherham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(186, N'GB-RUT', N'186', N'UK Rutland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(187, N'GB-SAW', N'187', N'UK Sandwell')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(188, N'GB-SAY', N'188', N'UK South Ayrshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(189, N'GB-SCB', N'189', N'UK The Scottish Borders')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(190, N'GB-SFK', N'190', N'UK Suffolk')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(191, N'GB-SFT', N'191', N'UK Sefton')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(192, N'GB-SGC', N'192', N'UK South Gloucestershire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(193, N'GB-SHF', N'193', N'UK Sheffield')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(194, N'GB-SHN', N'194', N'UK St. Helens')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(195, N'GB-SHR', N'195', N'UK Shropshire Council')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(196, N'GB-SKP', N'196', N'UK Stockport')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(197, N'GB-SLF', N'197', N'UK Salford')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(198, N'GB-SLG', N'198', N'UK Slough')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(199, N'GB-SLK', N'199', N'UK South Lanarkshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(200, N'GB-SND', N'200', N'UK Sunderland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(201, N'GB-SOL', N'201', N'UK Solihull')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(202, N'GB-SOM', N'202', N'UK Somerset')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(203, N'GB-SOS', N'203', N'UK Southend-on-Sea')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(204, N'GB-SRY', N'204', N'UK Surrey')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(205, N'GB-STB', N'205', N'UK Strabane')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(206, N'GB-STE', N'206', N'UK Stoke-on-Trent')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(207, N'GB-STG', N'207', N'UK Stirling')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(208, N'GB-STH', N'208', N'UK Southampton')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(209, N'GB-STN', N'209', N'UK Sutton')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(210, N'GB-STS', N'210', N'UK Staffordshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(211, N'GB-STT', N'211', N'UK Stockton-on-Tees')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(212, N'GB-STY', N'212', N'UK South Tyneside')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(213, N'GB-SWA', N'213', N'UK Swansea')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(214, N'GB-SWD', N'214', N'UK Swindon')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(215, N'GB-SWK', N'215', N'UK Southwark')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(216, N'GB-TAM', N'216', N'UK Tameside')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(217, N'GB-TFW', N'217', N'UK Telford and Wrekin')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(218, N'GB-THR', N'218', N'UK Thurrock')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(219, N'GB-TOB', N'219', N'UK Torbay')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(220, N'GB-TOF', N'220', N'UK Torfaen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(221, N'GB-TRF', N'221', N'UK Trafford')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(222, N'GB-TWH', N'222', N'UK Tower Hamlets')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(223, N'GB-VGL', N'223', N'UK The Vale of Glamorgan')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(224, N'GB-WAR', N'224', N'UK Warwickshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(225, N'GB-WBK', N'225', N'UK West Berkshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(226, N'GB-WDU', N'226', N'UK West Dunbartonshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(227, N'GB-WFT', N'227', N'UK Waltham Forest')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(228, N'GB-WGN', N'228', N'UK Wigan')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(229, N'GB-WIL', N'229', N'UK Wiltshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(230, N'GB-WKF', N'230', N'UK Wakefield')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(231, N'GB-WLL', N'231', N'UK Walsall')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(232, N'GB-WLN', N'232', N'UK West Lothian')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(233, N'GB-WLV', N'233', N'UK Wolverhampton')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(234, N'GB-WND', N'234', N'UK Wandsworth')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(235, N'GB-WNM', N'235', N'UK Windsor and Maidenhead')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(236, N'GB-WOK', N'236', N'UK Wokingham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(237, N'GB-WOR', N'237', N'UK Worcestershire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(238, N'GB-WRL', N'238', N'UK Wirral')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(239, N'GB-WRT', N'239', N'UK Warrington')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(240, N'GB-WRX', N'240', N'UK Wrexham')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(241, N'GB-WSM', N'241', N'UK Westminster')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(242, N'GB-WSX', N'242', N'UK West Sussex')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(243, N'GB-YOR', N'243', N'UK York')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(244, N'GB-ZET', N'244', N'UK Shetland Islands')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(245, N'GG', N'245', N'UK Guernsey')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(246, N'IM', N'246', N'UK Isle of Man')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(247, N'JE', N'247', N'UK Jersey')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(249, N'US-AK', N'249', N'US Alaska')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(250, N'US-AL', N'250', N'US Alabama')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(251, N'US-AR', N'251', N'US Arkansas')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(252, N'US-AS', N'252', N'US American Samoa')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(253, N'US-AZ', N'253', N'US Arizona')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(254, N'US-CA', N'254', N'US California')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(255, N'US-CO', N'255', N'US Colorado')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(256, N'US-CT', N'256', N'US Connecticut')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(257, N'US-DC', N'257', N'US District of Columbia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(258, N'US-DE', N'258', N'US Delaware')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(259, N'US-FL', N'259', N'US Florida')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(260, N'FM', N'260', N'US Federated States of Micronesia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(261, N'US-GA', N'261', N'US Georgia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(262, N'US-GU', N'262', N'US Guam')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(263, N'US-HI', N'263', N'US Hawaii')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(264, N'US-IA', N'264', N'US Iowa')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(265, N'US-ID', N'265', N'US Idaho')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(266, N'US-IL', N'266', N'US Illinois')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(267, N'US-IN', N'267', N'US Indiana')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(268, N'US-KS', N'268', N'US Kansas')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(269, N'US-KY', N'269', N'US Kentucky')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(270, N'US-LA', N'270', N'US Louisiana')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(271, N'US-MA', N'271', N'US Massachusetts')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(272, N'US-MD', N'272', N'US Maryland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(273, N'US-ME', N'273', N'US Maine')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(274, N'MH', N'274', N'US Marshall Islands')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(275, N'US-MI', N'275', N'US Michigan')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(276, N'US-MN', N'276', N'US Minnesota')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(277, N'US-MO', N'277', N'US Missouri')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(278, N'US-MP', N'278', N'US Northern Mariana Islands')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(279, N'US-MS', N'279', N'US Mississippi')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(280, N'US-MT', N'280', N'US Montana')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(281, N'US-NC', N'281', N'US North Carolina')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(282, N'US-ND', N'282', N'US North Dakota')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(283, N'US-NE', N'283', N'US Nebraska')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(284, N'US-NH', N'284', N'US New Hampshire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(285, N'US-NJ', N'285', N'US New Jersey')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(286, N'US-NM', N'286', N'US New Mexico')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(287, N'US-NV', N'287', N'US Nevada')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(288, N'US-NY', N'288', N'US New York')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(289, N'US-OH', N'289', N'US Ohio')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(290, N'US-OK', N'290', N'US Oklahoma')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(291, N'US-OR', N'291', N'US Oregon')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(292, N'US-PA', N'292', N'US Pennsylvania')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(293, N'US-PR', N'293', N'US Puerto Rico')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(294, N'PW', N'294', N'US Palau')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(295, N'US-RI', N'295', N'US Rhode Island')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(296, N'US-SC', N'296', N'US South Carolina')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(297, N'US-SD', N'297', N'US South Dakota')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(298, N'US-TN', N'298', N'US Tennessee')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(299, N'US-TX', N'299', N'US Texas')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(300, N'US-UM', N'300', N'US Minor Outlying Islands')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(301, N'US-UT', N'301', N'US Utah')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(302, N'US-VA', N'302', N'US Virginia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(303, N'US-VI', N'303', N'US Virgin Islands')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(304, N'US-VT', N'304', N'US Vermont')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(305, N'US-WA', N'305', N'US Washington')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(306, N'US-WI', N'306', N'US Wisconsin')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(307, N'US-WV', N'307', N'US West Virginia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(308, N'US-WY', N'308', N'US Wyoming')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(310, N'CA-AB', N'310', N'CA Alberta')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(311, N'CA-BC', N'311', N'CA British Columbia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(312, N'CA-MB', N'312', N'CA Manitoba')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(313, N'CA-NB', N'313', N'CA New Brunswick')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(314, N'CA-NL', N'314', N'CA Newfoundland and Labrador')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(315, N'CA-NS', N'315', N'CA Nova Scotia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(316, N'CA-NT', N'316', N'CA Northwest Territories')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(317, N'CA-NU', N'317', N'CA Nunavut')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(318, N'CA-ON', N'318', N'CA Ontario')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(319, N'CA-PE', N'319', N'CA Prince Edward Island')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(320, N'CA-QC', N'320', N'CA Quebec')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(321, N'CA-SK', N'321', N'CA Saskatchewan')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(322, N'CA-YT', N'322', N'CA Yukon Territory')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(324, N'AU-NT', N'324', N'AU Northern Territory')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(325, N'AU-WA', N'325', N'AU Western Australia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(326, N'AU-ACT', N'326', N'AU Australian Capital Territory')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(327, N'AU-VIC', N'327', N'AU Victoria')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(328, N'AU-SA', N'328', N'AU South Australia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(329, N'AU-QLD', N'329', N'AU Queensland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(330, N'AU-TAS', N'330', N'AU Tasmania')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(331, N'AU-NSW', N'331', N'AU New South Wales')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(333, N'FR-91', N'333', N'FR Essonne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(334, N'FR-92', N'334', N'FR Hauts-de-Seine')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(335, N'FR-75', N'335', N'FR Paris')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(336, N'FR-77', N'336', N'FR Seine-et-Marne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(337, N'FR-93', N'337', N'FR Seine-Saint-Denis')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(338, N'FR-95', N'338', N'FR Val-d''Oise')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(339, N'FR-94', N'339', N'FR Val-de-Marne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(340, N'FR-78', N'340', N'FR Yvelines')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(341, N'FR-08', N'341', N'FR Ardennes')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(342, N'FR-10', N'342', N'FR Aube')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(343, N'FR-52', N'343', N'FR Haute-Marne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(344, N'FR-51', N'344', N'FR Marne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(345, N'FR-02', N'345', N'FR Aisne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(346, N'FR-60', N'346', N'FR Oise')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(347, N'FR-80', N'347', N'FR Somme')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(348, N'FR-27', N'348', N'FR Eure')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(349, N'FR-76', N'349', N'FR Seine-Maritime')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(350, N'FR-67', N'350', N'FR Bas-Rhin')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(351, N'FR-68', N'351', N'FR Haut-Rhin')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(352, N'FR-25', N'352', N'FR Doubs')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(353, N'FR-24', N'353', N'FR Dordogne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(354, N'FR-70', N'354', N'FR Haute-Saône')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(355, N'FR-33', N'355', N'FR Gironde')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(356, N'FR-39', N'356', N'FR Jura')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(357, N'FR-40', N'357', N'FR Landes')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(358, N'FR-90', N'358', N'FR Territoire de Belfort')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(359, N'FR-47', N'359', N'FR Lot-et-Garonne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(360, N'FR-44', N'360', N'FR Loire-Atlantique')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(361, N'FR-64', N'361', N'FR Pyrénées-Atlantiques')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(362, N'FR-49', N'362', N'FR Maine-et-Loire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(363, N'FR-09', N'363', N'FR Ariège')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(364, N'FR-03', N'364', N'FR Allier')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(365, N'FR-53', N'365', N'FR Mayenne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(366, N'FR-12', N'366', N'FR Aveyron')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(367, N'FR-15', N'367', N'FR Cantal')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(368, N'FR-72', N'368', N'FR Sarthe')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(369, N'FR-32', N'369', N'FR Gers')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(370, N'FR-43', N'370', N'FR Haute-Loire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(371, N'FR-85', N'371', N'FR Vendée')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(372, N'FR-31', N'372', N'FR Haute-Garonne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(373, N'FR-63', N'373', N'FR Puy-de-Dôme')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(374, N'FR-14', N'374', N'FR Calvados')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(375, N'FR-65', N'375', N'FR Hautes-Pyrénées')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(376, N'FR-11', N'376', N'FR Aude')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(377, N'FR-50', N'377', N'FR Manche')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(378, N'FR-46', N'378', N'FR Lot')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(379, N'FR-30', N'379', N'FR Gard')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(380, N'FR-61', N'380', N'FR Orne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(381, N'FR-81', N'381', N'FR Tarn')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(382, N'FR-34', N'382', N'FR Hérault')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(383, N'FR-21', N'383', N'FR Côte-d''Or')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(384, N'FR-82', N'384', N'FR Tarn-et-Garonne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(385, N'FR-48', N'385', N'FR Lozère')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(386, N'FR-58', N'386', N'FR Nièvre')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(387, N'FR-19', N'387', N'FR Corrèze')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(388, N'FR-66', N'388', N'FR Pyrénées-Orientales')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(389, N'FR-71', N'389', N'FR Saône-et-Loire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(390, N'FR-23', N'390', N'FR Creuse')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(391, N'FR-04', N'391', N'FR Alpes-de-Haute-Provence')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(392, N'FR-89', N'392', N'FR Yonne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(393, N'FR-87', N'393', N'FR Haute-Vienne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(394, N'FR-06', N'394', N'FR Alpes-Maritimes')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(395, N'FR-59', N'395', N'FR Nord')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(396, N'FR-22', N'396', N'FR Côtes-d''Armor')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(397, N'FR-01', N'397', N'FR Ain')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(398, N'FR-13', N'398', N'FR Bouches-du-Rhône')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(399, N'FR-62', N'399', N'FR Pas-de-Calais')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(400, N'FR-29', N'400', N'FR Finistère')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(401, N'FR-07', N'401', N'FR Ardèche')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(402, N'FR-05', N'402', N'FR Hautes-Alpes')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(403, N'FR-54', N'403', N'FR Meurthe-et-Moselle')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(404, N'FR-35', N'404', N'FR Ille-et-Vilaine')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(405, N'FR-26', N'405', N'FR Drôme')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(406, N'FR-83', N'406', N'FR Var')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(407, N'FR-55', N'407', N'FR Meuse')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(408, N'FR-56', N'408', N'FR Morbihan')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(409, N'FR-74', N'409', N'FR Haute-Savoie')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(410, N'FR-84', N'410', N'FR Vaucluse')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(411, N'FR-18', N'411', N'FR Cher')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(412, N'FR-57', N'412', N'FR Moselle')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(413, N'FR-16', N'413', N'FR Charente')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(414, N'FR-38', N'414', N'FR Isère')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(415, N'FR-2A', N'415', N'FR Corse-du-Sud')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(416, N'FR-28', N'416', N'FR Eure-et-Loir')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(417, N'FR-88', N'417', N'FR Vosges')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(418, N'FR-17', N'418', N'FR Charente-Maritime')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(419, N'FR-42', N'419', N'FR Loire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(420, N'FR-2B', N'420', N'FR Haute-Corse')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(421, N'FR-36', N'421', N'FR Indre')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(422, N'FR-79', N'422', N'FR Deux-Sèvres')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(423, N'FR-69', N'423', N'FR Rhône')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(424, N'FR-GP', N'424', N'FR Guadeloupe')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(425, N'FR-37', N'425', N'FR Indre-et-Loire')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(426, N'FR-86', N'426', N'FR Vienne')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(427, N'FR-73', N'427', N'FR Savoie')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(428, N'FR-MQ', N'428', N'FR Martinique')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(429, N'FR-41', N'429', N'FR Loir-et-Cher')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(430, N'FR-GF', N'430', N'FR Guyane')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(431, N'FR-45', N'431', N'FR Loiret')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(432, N'FR-RE', N'432', N'FR La Réunion')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(433, N'FR-YT', N'433', N'FR Mayotte')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(435, N'DE-MV', N'435', N'DE Mecklenburg-Vorpommern')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(436, N'DE-NI', N'436', N'DE Niedersachsen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(437, N'DE-HE', N'437', N'DE Hessen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(438, N'DE-TH', N'438', N'DE Thüringen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(439, N'DE-NW', N'439', N'DE Nordrhein-Westfalen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(440, N'DE-BW', N'440', N'DE Baden-Württemberg')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(441, N'DE-BE', N'441', N'DE Berlin')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(442, N'DE-SH', N'442', N'DE Schleswig-Holstein')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(443, N'DE-SN', N'443', N'DE Sachsen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(444, N'DE-HH', N'444', N'DE Hamburg')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(445, N'DE-ST', N'445', N'DE Sachsen-Anhalt')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(446, N'DE-HB', N'446', N'DE Bremen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(447, N'DE-SL', N'447', N'DE Saarland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(448, N'DE-BY', N'448', N'DE Bayern')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(449, N'DE-RP', N'449', N'DE Rheinland-Pfalz')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(450, N'DE-BB', N'450', N'DE Brandenburg')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(452, N'AT-9', N'452', N'AT Wien')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(453, N'AT-1', N'453', N'AT Burgenland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(454, N'AT-2', N'454', N'AT Kärnten')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(455, N'AT-5', N'455', N'AT Salzburg')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(456, N'AT-3', N'456', N'AT Niederösterreich')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(457, N'AT-6', N'457', N'AT Steiermark')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(458, N'AT-7', N'458', N'AT Tirol')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(459, N'AT-8', N'459', N'AT Vorarlberg')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(460, N'AT-4', N'460', N'AT Oberösterreich')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(462, N'DK-84', N'462', N'DK Region Hovedstaden')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(463, N'DK-82', N'463', N'DK Region Midtjylland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(464, N'DK-81', N'464', N'DK Region Nordjylland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(465, N'DK-85', N'465', N'DK Region Sjælland')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(466, N'DK-83', N'466', N'DK Region Syddanmark')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(468, N'SE-K', N'468', N'SE Blekinge län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(469, N'SE-W', N'469', N'SE Dalarnas län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(470, N'SE-X', N'470', N'SE Gävleborgs län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(471, N'SE-I', N'471', N'SE Gotlands län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(472, N'SE-N', N'472', N'SE Hallands län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(473, N'SE-Z', N'473', N'SE Jämtlands län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(474, N'SE-F', N'474', N'SE Jönköpings län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(475, N'SE-H', N'475', N'SE Kalmar län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(476, N'SE-G', N'476', N'SE Kronobergs län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(477, N'SE-BD', N'477', N'SE Norrbottens län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(478, N'SE-T', N'478', N'SE Örebro län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(479, N'SE-E', N'479', N'SE Östergötlands län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(480, N'SE-M', N'480', N'SE Skåne län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(481, N'SE-D', N'481', N'SE Södermanlands län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(482, N'SE-AB', N'482', N'SE Stockholms län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(483, N'SE-C', N'483', N'SE Uppsala län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(484, N'SE-S', N'484', N'SE Värmlands län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(485, N'SE-AC', N'485', N'SE Västerbottens län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(486, N'SE-Y', N'486', N'SE Västernorrlands län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(487, N'SE-U', N'487', N'SE Västmanlands län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(488, N'SE-O', N'488', N'SE Västra Götalands län')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(490, N'BE-VAN', N'490', N'BE Antwerpen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(491, N'BE-BRU', N'491', N'BE Brussel')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(492, N'BE-WHT', N'492', N'BE Henegouwen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(493, N'BE-VLI', N'493', N'BE Limburg')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(494, N'BE-WLG', N'494', N'BE Luik')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(495, N'BE-WLX', N'495', N'BE Luxemburg')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(496, N'BE-WNA', N'496', N'BE Namen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(497, N'BE-VOV', N'497', N'BE Oost-Vlaanderen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(498, N'BE-VBR', N'498', N'BE Vlaams-Brabant')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(499, N'BE-WBR', N'499', N'BE Waals-Brabant')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(500, N'BE-VWV', N'500', N'BE West-Vlaanderen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(502, N'ES-VI', N'502', N'ES Álava')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(503, N'ES-AB', N'503', N'ES Albacete')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(504, N'ES-A', N'504', N'ES Alicante')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(505, N'ES-AL', N'505', N'ES Almería')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(506, N'ES-AV', N'506', N'ES Ávila')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(507, N'ES-BA', N'507', N'ES Badajoz')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(508, N'ES-PM', N'508', N'ES Baleares')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(509, N'ES-B', N'509', N'ES Barcelona')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(510, N'ES-BU', N'510', N'ES Burgos')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(511, N'ES-CC', N'511', N'ES Cáceres')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(512, N'ES-CA', N'512', N'ES Cádiz')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(513, N'ES-CS', N'513', N'ES Castellón')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(514, N'ES-CR', N'514', N'ES Ciudad Real')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(515, N'ES-CO', N'515', N'ES Córdoba')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(516, N'ES-C', N'516', N'ES La Coruña')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(517, N'ES-CU', N'517', N'ES Cuenca')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(518, N'ES-GI', N'518', N'ES Gerona')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(519, N'ES-GR', N'519', N'ES Granada')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(520, N'ES-GU', N'520', N'ES Guadalajara')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(521, N'ES-SS', N'521', N'ES Guipúzcoa')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(522, N'ES-H', N'522', N'ES Huelva')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(523, N'ES-HU', N'523', N'ES Huesca')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(524, N'ES-J', N'524', N'ES Jaén')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(525, N'ES-LE', N'525', N'ES León')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(526, N'ES-L', N'526', N'ES Lérida')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(527, N'ES-LO', N'527', N'ES La Rioja')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(528, N'ES-LU', N'528', N'ES Lugo')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(529, N'ES-M', N'529', N'ES Madrid')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(530, N'ES-MA', N'530', N'ES Málaga')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(531, N'ES-MU', N'531', N'ES Murcia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(532, N'ES-NA', N'532', N'ES Navarra')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(533, N'ES-OR', N'533', N'ES Orense')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(534, N'ES-O', N'534', N'ES Asturias')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(535, N'ES-P', N'535', N'ES Palencia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(536, N'ES-GC', N'536', N'ES Las Palmas')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(537, N'ES-PO', N'537', N'ES Pontevedra')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(538, N'ES-SA', N'538', N'ES Salamanca')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(539, N'ES-TF', N'539', N'ES Santa Cruz de Tenerife')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(540, N'ES-S', N'540', N'ES Cantabria')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(541, N'ES-SG', N'541', N'ES Segovia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(542, N'ES-SE', N'542', N'ES Sevilla')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(543, N'ES-SO', N'543', N'ES Soria')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(544, N'ES-T', N'544', N'ES Tarragona')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(545, N'ES-TE', N'545', N'ES Teruel')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(546, N'ES-TO', N'546', N'ES Toledo')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(547, N'ES-V', N'547', N'ES Valencia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(548, N'ES-VA', N'548', N'ES Valladolid')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(549, N'ES-BI', N'549', N'ES Vizcaya')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(550, N'ES-ZA', N'550', N'ES Zamora')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(551, N'ES-Z', N'551', N'ES Zaragoza')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(552, N'ES-CE', N'552', N'ES Ceuta')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(553, N'ES-ML', N'553', N'ES Melilla')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(554, N'554', N'554', N'BR Rio Grande do Norte')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(555, N'555', N'555', N'BR Tocantins')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(556, N'556', N'556', N'BR São Paulo')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(557, N'557', N'557', N'BR Amapá')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(558, N'558', N'558', N'BR Piauí')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(559, N'559', N'559', N'BR Mato Grosso do Sul')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(560, N'560', N'560', N'BR Acre')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(561, N'561', N'561', N'BR Pernambuco')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(562, N'562', N'562', N'BR Ceará')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(563, N'563', N'563', N'BR Goiás')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(564, N'564', N'564', N'BR Mato Grosso')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(565, N'565', N'565', N'BR Pará')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(566, N'566', N'566', N'BR Alagoas')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(567, N'567', N'567', N'BR Sergipe')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(568, N'568', N'568', N'BR Maranhão')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(569, N'569', N'569', N'BR Paraíba')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(570, N'570', N'570', N'BR Rondônia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(571, N'571', N'571', N'BR Amazonas')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(572, N'572', N'572', N'BR Rio de Janeiro')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(573, N'573', N'573', N'BR Santa Catarina')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(574, N'574', N'574', N'BR Distrito Federal')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(575, N'575', N'575', N'BR Bahia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(576, N'576', N'576', N'BR Espírito Santo')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(577, N'577', N'577', N'BR Rio Grande do Sul')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(578, N'578', N'578', N'BR Minas Gerais')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(579, N'579', N'579', N'BR Paraná')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(580, N'580', N'580', N'BR Roraima')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(581, N'581', N'581', N'IT Toscana')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(582, N'582', N'582', N'IT Ligurien')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(583, N'583', N'583', N'IT Marken')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(584, N'584', N'584', N'IT Calabria')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(585, N'585', N'585', N'IT Veneto')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(586, N'586', N'586', N'IT Lazio')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(587, N'587', N'587', N'IT Apulien')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(589, N'589', N'589', N'IT Campania')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(590, N'590', N'590', N'IT Umbria')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(591, N'591', N'591', N'IT Trentino-Alto Adige')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(592, N'592', N'592', N'IT Sardinien')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(593, N'593', N'593', N'IT Piemonte')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(594, N'594', N'594', N'IT Emilia-Romagna')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(595, N'595', N'595', N'IT Basilicata')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(596, N'596', N'596', N'IT Molise')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(597, N'597', N'597', N'IT Friuli-Venezia Giulia')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(598, N'598', N'598', N'IT Valle d''Aosta')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(599, N'599', N'599', N'IT Abruzzen')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(600, N'600', N'600', N'IT Lombardei')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(601, N'601', N'601', N'IT Sizilien')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(602, N'602', N'602', N'PT Guarda')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(603, N'603', N'603', N'PT Região Autónoma da Madeira')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(604, N'604', N'604', N'PT Leiria')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(605, N'605', N'605', N'PT Castelo Branco')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(606, N'606', N'606', N'PT Lisboa')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(607, N'607', N'607', N'PT Viseu')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(608, N'608', N'608', N'PT Portalegre')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(609, N'609', N'609', N'PT Setúbal')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(610, N'610', N'610', N'PT Évora')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(611, N'611', N'611', N'PT Faro')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(612, N'612', N'612', N'PT Vila Real')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(613, N'613', N'613', N'PT Braga')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(614, N'614', N'614', N'PT Aveiro')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(615, N'615', N'615', N'PT Porto')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(616, N'616', N'616', N'PT Coimbra')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(617, N'617', N'617', N'PT Viana do Castelo')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(618, N'618', N'618', N'PT Região Autónoma dos Açores')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(619, N'619', N'619', N'PT Bragança')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(620, N'620', N'620', N'PT Beja')
INSERT INTO [dbo].[tbl_CvRegion]([ID],[Name],[Code],[Description]) VALUES(621, N'621', N'621', N'PT Santarém')



CREATE TABLE [dbo].[tbl_CvSalary](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvSalary_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvSalary_U1] ON [dbo].[tbl_CvSalary]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvSalary_U2] ON [dbo].[tbl_CvSalary]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
INSERT INTO [dbo].[tbl_CvSalary]([ID],[Name],[Code],[Description]) VALUES(1, N'Lte20k', N'1', N'< 20.000')
INSERT INTO [dbo].[tbl_CvSalary]([ID],[Name],[Code],[Description]) VALUES(2, N'Lte40k', N'2', N'20.000 - 40.000')
INSERT INTO [dbo].[tbl_CvSalary]([ID],[Name],[Code],[Description]) VALUES(3, N'Lte60k', N'3', N'40.000 - 60.000')
INSERT INTO [dbo].[tbl_CvSalary]([ID],[Name],[Code],[Description]) VALUES(4, N'Lte80k', N'4', N'60.000 - 80.000')
INSERT INTO [dbo].[tbl_CvSalary]([ID],[Name],[Code],[Description]) VALUES(5, N'Lte100k', N'5', N'80.000 - 100.000')
INSERT INTO [dbo].[tbl_CvSalary]([ID],[Name],[Code],[Description]) VALUES(6, N'Lte150k', N'6', N'100.000 - 150.000')
INSERT INTO [dbo].[tbl_CvSalary]([ID],[Name],[Code],[Description]) VALUES(7, N'Gt150k', N'7', N'> 150.000')



CREATE TABLE [dbo].[tbl_CvSocialMediaType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvSocialMediaType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvSocialMediaType_U1] ON [dbo].[tbl_CvSocialMediaType]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvSocialMediaType_U2] ON [dbo].[tbl_CvSocialMediaType]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
-- Daten werden dynamisch erzeugt




CREATE TABLE [dbo].[tbl_CvSoftSkillType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvSoftSkillType_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvSoftSkillType_U1] ON [dbo].[tbl_CvSoftSkillType]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvSoftSkillType_U2] ON [dbo].[tbl_CvSoftSkillType]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
-- Daten werden dynamisch erzeugt



CREATE TABLE [dbo].[tbl_CvAddress](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AddressLine] [nvarchar](255) NULL,
	[StreetName] [nvarchar](255) NULL,
	[StreetNumberBase] [nvarchar](255) NULL,
	[StreetNumberExtension] [nvarchar](255) NULL,
	[PostalCode] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[FK_CvRegion] [int] NULL,
	[FK_CvCountry] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedFrom] [nvarchar](255) NOT NULL,
	[ChangedOn] [datetime] NULL,
	[ChangedFrom] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvAddress_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvAddress]  WITH CHECK ADD CONSTRAINT [tbl_CvAddress_CvRegion_FK1] FOREIGN KEY([FK_CvRegion])
REFERENCES [dbo].[tbl_CvRegion] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvAddress] CHECK CONSTRAINT [tbl_CvAddress_CvRegion_FK1]
GO
ALTER TABLE [dbo].[tbl_CvAddress]  WITH CHECK ADD CONSTRAINT [tbl_CvAddress_CvCountry_FK1] FOREIGN KEY([FK_CvCountry])
REFERENCES [dbo].[tbl_CvCountry] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvAddress] CHECK CONSTRAINT [tbl_CvAddress_CvCountry_FK1]
GO



CREATE TABLE [dbo].[tbl_CvPersonal](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Initials] [nvarchar](255) NULL,
	[Title] [nvarchar](255) NULL,
	[FirstName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[LastNamePrefix] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[FullName] [nvarchar](255) NULL,
	[DateOfBirth] [datetime] NULL,
	[PlaceOfBirth] [nvarchar](255) NULL,
	[FK_CvNationality] [int] NULL,
	[FK_CvGender] [int] NULL,
	[FK_CvDriversLicence] [int] NULL,
	[FK_CvMaritalStatus] [int] NULL,
	[Availability] [nvarchar](255) NULL,
	[MilitaryService] [nvarchar](255) NULL,
	[FK_CvAddress] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedFrom] [nvarchar](255) NOT NULL,
	[ChangedOn] [datetime] NULL,
	[ChangedFrom] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvPersonal_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvPersonal]  WITH CHECK ADD CONSTRAINT [tbl_CvPersonal_CvNationality_FK1] FOREIGN KEY([FK_CvNationality])
REFERENCES [dbo].[tbl_CvNationality] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvPersonal] CHECK CONSTRAINT [tbl_CvPersonal_CvNationality_FK1]
GO
ALTER TABLE [dbo].[tbl_CvPersonal]  WITH CHECK ADD CONSTRAINT [tbl_CvPersonal_CvGender_FK1] FOREIGN KEY([FK_CvGender])
REFERENCES [dbo].[tbl_CvGender] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvPersonal] CHECK CONSTRAINT [tbl_CvPersonal_CvGender_FK1]
GO
ALTER TABLE [dbo].[tbl_CvPersonal]  WITH CHECK ADD CONSTRAINT [tbl_CvPersonal_CvDriversLicence_FK1] FOREIGN KEY([FK_CvDriversLicence])
REFERENCES [dbo].[tbl_CvDriversLicence] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvPersonal] CHECK CONSTRAINT [tbl_CvPersonal_CvDriversLicence_FK1]
GO
ALTER TABLE [dbo].[tbl_CvPersonal]  WITH CHECK ADD CONSTRAINT [tbl_CvPersonal_CvMaritalStatus_FK1] FOREIGN KEY([FK_CvMaritalStatus])
REFERENCES [dbo].[tbl_CvMaritalStatus] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvPersonal] CHECK CONSTRAINT [tbl_CvPersonal_CvMaritalStatus_FK1]
GO
ALTER TABLE [dbo].[tbl_CvPersonal]  WITH CHECK ADD CONSTRAINT [tbl_CvPersonal_CvAddress_FK1] FOREIGN KEY([FK_CvAddress])
REFERENCES [dbo].[tbl_CvAddress] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvPersonal] CHECK CONSTRAINT [tbl_CvPersonal_CvAddress_FK1]
GO



CREATE TABLE [dbo].[tbl_CvPhoneNumber](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvPersonal] [int] NOT NULL,
	[FK_CvPhoneNumberType] [int] NOT NULL,
	[PhoneNumber] [nvarchar](255) NOT NULL
 CONSTRAINT [tbl_CvPhoneNumber_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvPhoneNumber]  WITH CHECK ADD CONSTRAINT [tbl_CvPhoneNumber_CvPersonal_FK1] FOREIGN KEY([FK_CvPersonal])
REFERENCES [dbo].[tbl_CvPersonal] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvPhoneNumber] CHECK CONSTRAINT [tbl_CvPhoneNumber_CvPersonal_FK1]
GO
ALTER TABLE [dbo].[tbl_CvPhoneNumber]  WITH CHECK ADD CONSTRAINT [tbl_CvPhoneNumber_CvPhoneNumberType_FK1] FOREIGN KEY([FK_CvPhoneNumberType])
REFERENCES [dbo].[tbl_CvPhoneNumberType] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvPhoneNumber] CHECK CONSTRAINT [tbl_CvPhoneNumber_CvPhoneNumberType_FK1]
GO



CREATE TABLE [dbo].[tbl_CvEmail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvPersonal] [int] NOT NULL,
	[FK_CvEmailType] [int] NOT NULL,
	[Email] [nvarchar](255) NOT NULL
 CONSTRAINT [tbl_CvEmail_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvEmail]  WITH CHECK ADD CONSTRAINT [tbl_CvEmail_CvPersonal_FK1] FOREIGN KEY([FK_CvPersonal])
REFERENCES [dbo].[tbl_CvPersonal] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEmail] CHECK CONSTRAINT [tbl_CvEmail_CvPersonal_FK1]
GO
ALTER TABLE [dbo].[tbl_CvEmail]  WITH CHECK ADD CONSTRAINT [tbl_CvEmail_CvEmailType_FK1] FOREIGN KEY([FK_CvEmailType])
REFERENCES [dbo].[tbl_CvEmailType] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEmail] CHECK CONSTRAINT [tbl_CvEmail_CvEmailType_FK1]
GO



CREATE TABLE [dbo].[tbl_CvSocialMedia](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvPersonal] [int] NOT NULL,
	[FK_CvSocialMediaType] [int] NULL,
	[Url] [nvarchar](255) NOT NULL
 CONSTRAINT [tbl_CvSocialMedia_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvSocialMedia]  WITH CHECK ADD CONSTRAINT [tbl_CvSocialMedia_CvPersonal_FK1] FOREIGN KEY([FK_CvPersonal])
REFERENCES [dbo].[tbl_CvPersonal] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvSocialMedia] CHECK CONSTRAINT [tbl_CvSocialMedia_CvPersonal_FK1]
GO
ALTER TABLE [dbo].[tbl_CvSocialMedia]  WITH CHECK ADD CONSTRAINT [tbl_CvSocialMedia_CvSocialMediaType_FK1] FOREIGN KEY([FK_CvSocialMediaType])
REFERENCES [dbo].[tbl_CvSocialMediaType] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvSocialMedia] CHECK CONSTRAINT [tbl_CvSocialMedia_CvSocialMediaType_FK1]
GO



CREATE TABLE [dbo].[tbl_CvProfile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Customer_ID] [nvarchar](50) NULL,
	[BusinessBranch] [nvarchar](50) NULL,
	[TrxmlID] [int] NULL,
	[FK_CvPersonal] [int] NOT NULL,
	[FK_CvDocumentText] [int] NULL,
	[FK_CvDocumentHtml] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedFrom] [nvarchar](255) NOT NULL,
	[ChangedOn] [datetime] NULL,
	[ChangedFrom] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvProfile_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [tbl_CvProfile_U1] ON [dbo].[tbl_CvProfile]
(
	[TrxmlID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvProfile]  WITH CHECK ADD CONSTRAINT [tbl_CvProfile_CvPersonal_FK1] FOREIGN KEY([FK_CvPersonal])
REFERENCES [dbo].[tbl_CvPersonal] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvProfile] CHECK CONSTRAINT [tbl_CvProfile_CvPersonal_FK1]
GO
ALTER TABLE [dbo].[tbl_CvProfile]  WITH CHECK ADD CONSTRAINT [tbl_CvProfile_CvDocumentText_FK1] FOREIGN KEY([FK_CvDocumentText])
REFERENCES [dbo].[tbl_CvDocumentText] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvProfile] CHECK CONSTRAINT [tbl_CvProfile_CvDocumentText_FK1]
GO
ALTER TABLE [dbo].[tbl_CvProfile]  WITH CHECK ADD CONSTRAINT [tbl_CvProfile_CvDocumentHtml_FK1] FOREIGN KEY([FK_CvDocumentHtml])
REFERENCES [dbo].[tbl_CvDocumentHtml] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvProfile] CHECK CONSTRAINT [tbl_CvProfile_CvDocumentHtml_FK1]
GO



CREATE TABLE [dbo].[tbl_CvEducationHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvProfile] [int] NOT NULL,
	[FK_CvEducation] [int] NULL,
	[FK_CvEducationLevel] [int] NULL,
	[FK_CvEducationDetail] [int] NULL,
	[DegreeDirection] [nvarchar](255) NULL,
	[FK_CvDegreeDirection] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[InstituteNameAndPlace] [nvarchar](255) NULL,
	[InstituteName] [nvarchar](255) NULL,
	[InstitutePlace] [nvarchar](255) NULL,
	[FK_CvInstituteType] [int] NULL,
	[FK_CvDiploma] [int] NULL,
	[DiplomaDate] [datetime] NULL,
	[Subjects] [nvarchar](255) NULL,
	[IsHighestItem] [bit] NOT NULL
 CONSTRAINT [tbl_CvEducationHistory_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory]  WITH CHECK ADD CONSTRAINT [tbl_CvEducationHistory_CvProfile_FK1] FOREIGN KEY([FK_CvProfile])
REFERENCES [dbo].[tbl_CvProfile] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory] CHECK CONSTRAINT [tbl_CvEducationHistory_CvProfile_FK1]
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory]  WITH CHECK ADD CONSTRAINT [tbl_CvEducationHistory_CvEducation_FK1] FOREIGN KEY([FK_CvEducation])
REFERENCES [dbo].[tbl_CvEducation] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory] CHECK CONSTRAINT [tbl_CvEducationHistory_CvEducation_FK1]
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory]  WITH CHECK ADD CONSTRAINT [tbl_CvEducationHistory_CvEducationLevel_FK1] FOREIGN KEY([FK_CvEducationLevel])
REFERENCES [dbo].[tbl_CvEducationLevel] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory] CHECK CONSTRAINT [tbl_CvEducationHistory_CvEducationLevel_FK1]
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory]  WITH CHECK ADD CONSTRAINT [tbl_CvEducationHistory_CvEducationDetail_FK1] FOREIGN KEY([FK_CvEducationDetail])
REFERENCES [dbo].[tbl_CvEducationDetail] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory] CHECK CONSTRAINT [tbl_CvEducationHistory_CvEducationDetail_FK1]
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory]  WITH CHECK ADD CONSTRAINT [tbl_CvEducationHistory_CvDegreeDirection_FK1] FOREIGN KEY([FK_CvDegreeDirection])
REFERENCES [dbo].[tbl_CvDegreeDirection] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory] CHECK CONSTRAINT [tbl_CvEducationHistory_CvDegreeDirection_FK1]
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory]  WITH CHECK ADD CONSTRAINT [tbl_CvEducationHistory_CvInstituteType_FK1] FOREIGN KEY([FK_CvInstituteType])
REFERENCES [dbo].[tbl_CvInstituteType] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory] CHECK CONSTRAINT [tbl_CvEducationHistory_CvInstituteType_FK1]
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory]  WITH CHECK ADD CONSTRAINT [tbl_CvEducationHistory_CvDiploma_FK1] FOREIGN KEY([FK_CvDiploma])
REFERENCES [dbo].[tbl_CvDiploma] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEducationHistory] CHECK CONSTRAINT [tbl_CvEducationHistory_CvDiploma_FK1]
GO




CREATE TABLE [dbo].[tbl_CvEmploymentHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvProfile] [int] NOT NULL,
	[JobTitle] [nvarchar](255) NULL,
	[FK_CvJobTitle] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ExperienceYears] [int] NULL,
	[EmployerNameAndPlace] [nvarchar](255) NULL,
	[EmployerName] [nvarchar](255) NULL,
	[EmployerPlace] [nvarchar](255) NULL,
	[Description] [nvarchar](4000) NULL,
	[QuitReason] [nvarchar](255) NULL,
	[IsLastItem] [bit] NOT NULL,
	[IsLastItemWithJobTitle] [bit] NOT NULL,
	[IsCurrentEmployer] [bit] NOT NULL,
	[Remarks] [nvarchar](255) NULL,
 CONSTRAINT [tbl_CvEmploymentHistory_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvEmploymentHistory]  WITH CHECK ADD CONSTRAINT [tbl_CvEmploymentHistory_CvProfile_FK1] FOREIGN KEY([FK_CvProfile])
REFERENCES [dbo].[tbl_CvProfile] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEmploymentHistory] CHECK CONSTRAINT [tbl_CvEmploymentHistory_CvProfile_FK1]
GO
ALTER TABLE [dbo].[tbl_CvEmploymentHistory]  WITH CHECK ADD CONSTRAINT [tbl_CvEmploymentHistory_CvJobTitle_FK1] FOREIGN KEY([FK_CvJobTitle])
REFERENCES [dbo].[tbl_CvJobTitle] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvEmploymentHistory] CHECK CONSTRAINT [tbl_CvEmploymentHistory_CvJobTitle_FK1]
GO




CREATE TABLE [dbo].[tbl_CvSkill](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvProfile] [int] NOT NULL
 CONSTRAINT [tbl_CvSkill_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvSkill]  WITH CHECK ADD CONSTRAINT [tbl_CvSkill_CvProfile_FK1] FOREIGN KEY([FK_CvProfile])
REFERENCES [dbo].[tbl_CvProfile] ([ID])
GO



CREATE TABLE [dbo].[tbl_CvComputerSkill](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvSkill] [int] NOT NULL,
	[Text] [nvarchar](255) NULL,
	[FK_CvComputerSkillType] [int] NULL,
	[Duration] [nvarchar](255) NULL
 CONSTRAINT [tbl_CvComputerSkill_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvComputerSkill]  WITH CHECK ADD CONSTRAINT [tbl_CvComputerSkill_CvSkill_FK1] FOREIGN KEY([FK_CvSkill])
REFERENCES [dbo].[tbl_CvSkill] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvComputerSkill] CHECK CONSTRAINT [tbl_CvComputerSkill_CvSkill_FK1]
GO
ALTER TABLE [dbo].[tbl_CvComputerSkill]  WITH CHECK ADD CONSTRAINT [tbl_CvComputerSkill_CvComputerSkillType_FK1] FOREIGN KEY([FK_CvComputerSkillType])
REFERENCES [dbo].[tbl_CvComputerSkillType] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvComputerSkill] CHECK CONSTRAINT [tbl_CvComputerSkill_CvComputerSkillType_FK1]
GO



CREATE TABLE [dbo].[tbl_CvLanguageSkill](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvSkill] [int] NOT NULL,
	[Text] [nvarchar](255) NULL,
	[FK_CvLanguageSkillType] [int] NULL,
	[FK_CvLanguageProficiency] [int] NULL,
	[IsNativeLanguage] [bit] NOT NULL
 CONSTRAINT [tbl_CvLanguageSkill_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvLanguageSkill]  WITH CHECK ADD CONSTRAINT [tbl_CvLanguageSkill_CvSkill_FK1] FOREIGN KEY([FK_CvSkill])
REFERENCES [dbo].[tbl_CvSkill] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvLanguageSkill] CHECK CONSTRAINT [tbl_CvLanguageSkill_CvSkill_FK1]
GO
ALTER TABLE [dbo].[tbl_CvLanguageSkill]  WITH CHECK ADD CONSTRAINT [tbl_CvLanguageSkill_CvLanguageSkillType_FK1] FOREIGN KEY([FK_CvLanguageSkillType])
REFERENCES [dbo].[tbl_CvLanguageSkillType] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvLanguageSkill] CHECK CONSTRAINT [tbl_CvLanguageSkill_CvLanguageSkillType_FK1]
GO
ALTER TABLE [dbo].[tbl_CvLanguageSkill]  WITH CHECK ADD CONSTRAINT [tbl_CvLanguageSkill_CvLanguageProficiency_FK1] FOREIGN KEY([FK_CvLanguageProficiency])
REFERENCES [dbo].[tbl_CvLanguageProficiency] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvLanguageSkill] CHECK CONSTRAINT [tbl_CvLanguageSkill_CvLanguageProficiency_FK1]
GO



CREATE TABLE [dbo].[tbl_CvSoftSkill](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvSkill] [int] NOT NULL,
	[Text] [nvarchar](255) NULL,
	[FK_CvSoftSkillType] [int] NULL
 CONSTRAINT [tbl_CvSoftSkill_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvSoftSkill]  WITH CHECK ADD CONSTRAINT [tbl_CvSoftSkill_CvSkill_FK1] FOREIGN KEY([FK_CvSkill])
REFERENCES [dbo].[tbl_CvSkill] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvSoftSkill] CHECK CONSTRAINT [tbl_CvSoftSkill_CvSkill_FK1]
GO
ALTER TABLE [dbo].[tbl_CvSoftSkill]  WITH CHECK ADD CONSTRAINT [tbl_CvSoftSkill_CvSoftSkillType_FK1] FOREIGN KEY([FK_CvSoftSkillType])
REFERENCES [dbo].[tbl_CvSoftSkillType] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvSoftSkill] CHECK CONSTRAINT [tbl_CvSoftSkill_CvSoftSkillType_FK1]
GO



CREATE TABLE [dbo].[tbl_CvOther](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvProfile] [int] NOT NULL,
	[TotalExperience] [nvarchar](255) NULL,
	[Salary] [nvarchar](255) NULL,
	[Benefits] [nvarchar](255) NULL
 CONSTRAINT [tbl_CvOther_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvOther]  WITH CHECK ADD CONSTRAINT [tbl_CvOther_CvProfile_FK1] FOREIGN KEY([FK_CvProfile])
REFERENCES [dbo].[tbl_CvProfile] ([ID])
GO



CREATE TABLE [dbo].[tbl_CvHobby](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvOther] [int] NOT NULL,
	[Text] [nvarchar](255) NULL
 CONSTRAINT [tbl_CvHobby_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvHobby]  WITH CHECK ADD CONSTRAINT [tbl_CvHobby_CvOther_FK1] FOREIGN KEY([FK_CvOther])
REFERENCES [dbo].[tbl_CvOther] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvHobby] CHECK CONSTRAINT [tbl_CvHobby_CvOther_FK1]
GO



CREATE TABLE [dbo].[tbl_CvReference](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvOther] [int] NOT NULL,
	[Text] [nvarchar](255) NULL
 CONSTRAINT [tbl_CvReference_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvReference]  WITH CHECK ADD CONSTRAINT [tbl_CvReference_CvOther_FK1] FOREIGN KEY([FK_CvOther])
REFERENCES [dbo].[tbl_CvOther] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvReference] CHECK CONSTRAINT [tbl_CvReference_CvOther_FK1]
GO



CREATE TABLE [dbo].[tbl_CvCustomArea](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvProfile] [int] NOT NULL,
	[CvTitle] [nvarchar](255) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[EditedOn] [datetime] NULL,
	[EditedBy] [nvarchar](255) NULL,
	[TotalExperienceYears] [int] NULL,
	[CurrentJob] [nvarchar](255) NULL,
	[CurrentEmployer] [nvarchar](255) NULL,
	[Last3Experiences] [nvarchar](2047) NULL,
	[FK_CvHighestEducationLevel] [int] NULL,
	[FK_CvSalary] [int] NULL,
	[FK_CvProfileStatus] [int] NULL,
	[FK_CvAvailability] [int] NULL,
	[CvComment] [nvarchar](2047) NULL,
	[LearnedOccupation] [nvarchar](255) NULL,
	[FK_CvApproval] [int] NULL,
	[POBox] [nvarchar](255) NULL,
	[ExternalID] [nvarchar](255) NULL,
	[FK_CvProfilePicture] [int] NULL
 CONSTRAINT [tbl_CvCustomArea_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvCustomArea]  WITH CHECK ADD CONSTRAINT [tbl_CvCustomArea_CvProfile_FK1] FOREIGN KEY([FK_CvProfile])
REFERENCES [dbo].[tbl_CvProfile] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvCustomArea] CHECK CONSTRAINT [tbl_CvCustomArea_CvProfile_FK1]
GO
ALTER TABLE [dbo].[tbl_CvCustomArea]  WITH CHECK ADD CONSTRAINT [tbl_CvCustomArea_CvHighestEducationLevel_FK1] FOREIGN KEY([FK_CvHighestEducationLevel])
REFERENCES [dbo].[tbl_CvHighestEducationLevel] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvCustomArea] CHECK CONSTRAINT [tbl_CvCustomArea_CvHighestEducationLevel_FK1]
GO
ALTER TABLE [dbo].[tbl_CvCustomArea]  WITH CHECK ADD CONSTRAINT [tbl_CvCustomArea_CvSalary_FK1] FOREIGN KEY([FK_CvSalary])
REFERENCES [dbo].[tbl_CvSalary] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvCustomArea] CHECK CONSTRAINT [tbl_CvCustomArea_CvSalary_FK1]
GO
ALTER TABLE [dbo].[tbl_CvCustomArea]  WITH CHECK ADD CONSTRAINT [tbl_CvCustomArea_CvProfileStatus_FK1] FOREIGN KEY([FK_CvProfileStatus])
REFERENCES [dbo].[tbl_CvProfileStatus] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvCustomArea] CHECK CONSTRAINT [tbl_CvCustomArea_CvProfileStatus_FK1]
GO
ALTER TABLE [dbo].[tbl_CvCustomArea]  WITH CHECK ADD CONSTRAINT [tbl_CvCustomArea_CvAvailability_FK1] FOREIGN KEY([FK_CvAvailability])
REFERENCES [dbo].[tbl_CvAvailability] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvCustomArea] CHECK CONSTRAINT [tbl_CvCustomArea_CvAvailability_FK1]
GO
ALTER TABLE [dbo].[tbl_CvCustomArea]  WITH CHECK ADD CONSTRAINT [tbl_CvCustomArea_CvApproval_FK1] FOREIGN KEY([FK_CvApproval])
REFERENCES [dbo].[tbl_CvApproval] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvCustomArea] CHECK CONSTRAINT [tbl_CvCustomArea_CvApproval_FK1]
GO
ALTER TABLE [dbo].[tbl_CvCustomArea]  WITH CHECK ADD CONSTRAINT [tbl_CvCustomArea_CvPicture_FK1] FOREIGN KEY([FK_CvProfilePicture])
REFERENCES [dbo].[tbl_CvPicture] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvCustomArea] CHECK CONSTRAINT [tbl_CvCustomArea_CvPicture_FK1]
GO



CREATE TABLE [dbo].[tbl_CvExtraInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvCustomArea] [int] NOT NULL,
	[Key] [nvarchar](255) NOT NULL,
	[Value] [nvarchar](255) NULL
 CONSTRAINT [tbl_CvExtraInfo_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvExtraInfo]  WITH CHECK ADD CONSTRAINT [tbl_CvExtraInfo_CvCustomArea_FK1] FOREIGN KEY([FK_CvCustomArea])
REFERENCES [dbo].[tbl_CvCustomArea] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvExtraInfo] CHECK CONSTRAINT [tbl_CvExtraInfo_CvCustomArea_FK1]
GO




CREATE TABLE [dbo].[tbl_CvTransportation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FK_CvCustomArea] [int] NOT NULL,
	[DriversLicence] [nvarchar](255) NULL,
	[Car] [bit] NULL,
	[Motorcycle] [bit] NULL,
	[Bicycle] [bit] NULL
 CONSTRAINT [tbl_CvTransportation_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_CvTransportation]  WITH CHECK ADD CONSTRAINT [tbl_CvTransportation_CvCustomArea_FK1] FOREIGN KEY([FK_CvCustomArea])
REFERENCES [dbo].[tbl_CvCustomArea] ([ID])
GO
ALTER TABLE [dbo].[tbl_CvTransportation] CHECK CONSTRAINT [tbl_CvTransportation_CvCustomArea_FK1]
GO

