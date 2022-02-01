SET IDENTITY_INSERT [dbo].[AircraftCategories] ON 
GO
INSERT [dbo].[AircraftCategories] (Id, [Name]) VALUES (1,N'Airplane')
GO
INSERT [dbo].[AircraftCategories] (Id, [Name]) VALUES (2,N'Helicopter/Rotorcraft')
GO
INSERT [dbo].[AircraftCategories] (Id, [Name]) VALUES (3,N'Glider')
GO
INSERT [dbo].[AircraftCategories] (Id, [Name]) VALUES (4,N'Flight Simulator')
GO
INSERT [dbo].[AircraftCategories] (Id, [Name]) VALUES (5, N'Unmanned Aerial Vehicle (UAV)')
GO
SET IDENTITY_INSERT [dbo].[AircraftCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[AircraftClasses] ON 
GO
INSERT [dbo].[AircraftClasses] ([Id], [Name]) VALUES (1, N'Single Engine Land (ASEL)')
GO
INSERT [dbo].[AircraftClasses] ([Id], [Name]) VALUES (2, N'Multi Engine Land (AMEL)')
GO
INSERT [dbo].[AircraftClasses] ([Id], [Name]) VALUES (3, N'Single Engine Sea (ASES)')
GO
INSERT [dbo].[AircraftClasses] ([Id], [Name]) VALUES (4, N'Multi Engine Sea (AMES)')
GO
SET IDENTITY_INSERT [dbo].[AircraftClasses] OFF
GO
SET IDENTITY_INSERT [dbo].[AircraftMakes] ON 
GO
INSERT [dbo].[AircraftMakes] ([Id], [Name]) VALUES (1, N'Beechcraft')
GO
INSERT [dbo].[AircraftMakes] ([Id], [Name]) VALUES (2, N'Boeing')
GO

GO
SET IDENTITY_INSERT [dbo].[AircraftMakes] OFF
GO
SET IDENTITY_INSERT [dbo].[AircraftModels] ON 
GO
INSERT [dbo].[AircraftModels] ([Id], [Name]) VALUES (1, N'182T')
GO
INSERT [dbo].[AircraftModels] ([Id], [Name]) VALUES (2, N'172R')
GO
GO
SET IDENTITY_INSERT [dbo].[AircraftModels] OFF
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (1, N'Aghanistan', N'AF', N'AFG')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (2, N'Aland Islands', N'AX', N'ALA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (3, N'Albania', N'AL', N'ALB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (4, N'Algeria', N'DZ', N'DZA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (5, N'American Samoa', N'AS', N'ASM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (6, N'Andorra', N'AD', N'AND')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (7, N'Angola', N'AO', N'AGO')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (8, N'Anguilla', N'AI', N'AIA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (9, N'Antarctica', N'AQ', N'ATA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (10, N'Antigua and Barbuda', N'AG', N'ATG')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (11, N'Argentina', N'AR', N'ARG')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (12, N'Armenia', N'AM', N'ARM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (13, N'Aruba', N'AW', N'ABW')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (14, N'Australia', N'AU', N'AUS')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (15, N'Austria', N'AT', N'AUT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (16, N'Azerbaijan', N'AZ', N'AZE')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (17, N'Bahamas', N'BS', N'BHS')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (18, N'Bahrain', N'BH', N'BHR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (19, N'Bangladesh', N'BD', N'BGD')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (20, N'Barbados', N'BB', N'BRB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (21, N'Belarus', N'BY', N'BLR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (22, N'Belgium', N'BE', N'BEL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (23, N'Belize', N'BZ', N'BLZ')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (24, N'Benin', N'BJ', N'BEN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (25, N'Bermuda', N'BM', N'BMU')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (26, N'Bhutan', N'BT', N'BTN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (27, N'Bolivia', N'BO', N'BOL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (28, N'Bonaire, Sint Eustatius and Saba', N'BQ', N'BES')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (29, N'Bosnia and Herzegovina', N'BA', N'BIH')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (30, N'Botswana', N'BW', N'BWA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (31, N'Bouvet Island', N'BV', N'BVT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (32, N'Brazil', N'BR', N'BRA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (33, N'British Indian Ocean Territory', N'IO', N'IOT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (34, N'Brunei', N'BN', N'BRN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (35, N'Bulgaria', N'BG', N'BGR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (36, N'Burkina Faso', N'BF', N'BFA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (37, N'Burundi', N'BI', N'BDI')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (38, N'Cambodia', N'KH', N'KHM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (39, N'Cameroon', N'CM', N'CMR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (40, N'Canada', N'CA', N'CAN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (41, N'Cape Verde', N'CV', N'CPV')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (42, N'Cayman Islands', N'KY', N'CYM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (43, N'Central African Republic', N'CF', N'CAF')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (44, N'Chad', N'TD', N'TCD')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (45, N'Chile', N'CL', N'CHL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (46, N'China', N'CN', N'CHN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (47, N'Christmas Island', N'CX', N'CXR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (48, N'Cocos (Keeling) Islands', N'CC', N'CCK')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (49, N'Colombia', N'CO', N'COL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (50, N'Comoros', N'KM', N'COM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (51, N'Congo', N'CG', N'COG')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (52, N'Cook Islands', N'CK', N'COK')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (53, N'Costa Rica', N'CR', N'CRI')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (54, N'Ivory Coast', N'CI', N'CIV')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (55, N'Croatia', N'HR', N'HRV')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (56, N'Cuba', N'CU', N'CUB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (57, N'Curacao', N'CW', N'CUW')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (58, N'Cyprus', N'CY', N'CYP')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (59, N'Czech Republic', N'CZ', N'CZE')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (60, N'Democratic Republic of the Congo', N'CD', N'COD')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (61, N'Denmark', N'DK', N'DNK')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (62, N'Djibouti', N'DJ', N'DJI')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (63, N'Dominica', N'DM', N'DMA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (64, N'Dominican Republic', N'DO', N'DOM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (65, N'Ecuador', N'EC', N'ECU')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (66, N'Egypt', N'EG', N'EGY')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (67, N'El Salvador', N'SV', N'SLV')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (68, N'Equatorial Guinea', N'GQ', N'GNQ')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (69, N'Eritrea', N'ER', N'ERI')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (70, N'Estonia', N'EE', N'EST')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (71, N'Ethiopia', N'ET', N'ETH')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (72, N'Falkland Islands (Malvinas)', N'FK', N'FLK')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (73, N'Faroe Islands', N'FO', N'FRO')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (74, N'Fiji', N'FJ', N'FJI')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (75, N'Finland', N'FI', N'FIN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (76, N'France', N'FR', N'FRA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (77, N'French Guiana', N'GF', N'GUF')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (78, N'French Polynesia', N'PF', N'PYF')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (79, N'French Southern Territories', N'TF', N'ATF')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (80, N'Gabon', N'GA', N'GAB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (81, N'Gambia', N'GM', N'GMB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (82, N'Georgia', N'GE', N'GEO')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (83, N'Germany', N'DE', N'DEU')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (84, N'Ghana', N'GH', N'GHA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (85, N'Gibraltar', N'GI', N'GIB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (86, N'Greece', N'GR', N'GRC')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (87, N'Greenland', N'GL', N'GRL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (88, N'Grenada', N'GD', N'GRD')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (89, N'Guadaloupe', N'GP', N'GLP')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (90, N'Guam', N'GU', N'GUM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (91, N'Guatemala', N'GT', N'GTM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (92, N'Guernsey', N'GG', N'GGY')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (93, N'Guinea', N'GN', N'GIN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (94, N'Guinea-Bissau', N'GW', N'GNB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (95, N'Guyana', N'GY', N'GUY')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (96, N'Haiti', N'HT', N'HTI')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (97, N'Heard Island and McDonald Islands', N'HM', N'HMD')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (98, N'Honduras', N'HN', N'HND')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (99, N'Hong Kong', N'HK', N'HKG')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (100, N'Hungary', N'HU', N'HUN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (101, N'Iceland', N'IS', N'ISL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (102, N'India', N'IN', N'IND')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (103, N'Indonesia', N'ID', N'IDN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (104, N'Iran', N'IR', N'IRN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (105, N'Iraq', N'IQ', N'IRQ')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (106, N'Ireland', N'IE', N'IRL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (107, N'Isle of Man', N'IM', N'IMN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (108, N'Israel', N'IL', N'ISR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (109, N'Italy', N'IT', N'ITA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (110, N'Jamaica', N'JM', N'JAM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (111, N'Japan', N'JP', N'JPN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (112, N'Jersey', N'JE', N'JEY')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (113, N'Jordan', N'JO', N'JOR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (114, N'Kazakhstan', N'KZ', N'KAZ')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (115, N'Kenya', N'KE', N'KEN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (116, N'Kiribati', N'KI', N'KIR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (117, N'Kosovo', N'XK', N'---')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (118, N'Kuwait', N'KW', N'KWT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (119, N'Kyrgyzstan', N'KG', N'KGZ')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (120, N'Laos', N'LA', N'LAO')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (121, N'Latvia', N'LV', N'LVA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (122, N'Lebanon', N'LB', N'LBN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (123, N'Lesotho', N'LS', N'LSO')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (124, N'Liberia', N'LR', N'LBR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (125, N'Libya', N'LY', N'LBY')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (126, N'Liechtenstein', N'LI', N'LIE')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (127, N'Lithuania', N'LT', N'LTU')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (128, N'Luxembourg', N'LU', N'LUX')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (129, N'Macao', N'MO', N'MAC')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (130, N'Macedonia', N'MK', N'MKD')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (131, N'Madagascar', N'MG', N'MDG')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (132, N'Malawi', N'MW', N'MWI')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (133, N'Malaysia', N'MY', N'MYS')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (134, N'Maldives', N'MV', N'MDV')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (135, N'Mali', N'ML', N'MLI')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (136, N'Malta', N'MT', N'MLT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (137, N'Marshall Islands', N'MH', N'MHL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (138, N'Martinique', N'MQ', N'MTQ')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (139, N'Mauritania', N'MR', N'MRT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (140, N'Mauritius', N'MU', N'MUS')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (141, N'Mayotte', N'YT', N'MYT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (142, N'Mexico', N'MX', N'MEX')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (143, N'Micronesia', N'FM', N'FSM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (144, N'Moldava', N'MD', N'MDA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (145, N'Monaco', N'MC', N'MCO')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (146, N'Mongolia', N'MN', N'MNG')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (147, N'Montenegro', N'ME', N'MNE')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (148, N'Montserrat', N'MS', N'MSR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (149, N'Morocco', N'MA', N'MAR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (150, N'Mozambique', N'MZ', N'MOZ')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (151, N'Myanmar (Burma)', N'MM', N'MMR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (152, N'Namibia', N'NA', N'NAM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (153, N'Nauru', N'NR', N'NRU')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (154, N'Nepal', N'NP', N'NPL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (155, N'Netherlands', N'NL', N'NLD')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (156, N'New Caledonia', N'NC', N'NCL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (157, N'New Zealand', N'NZ', N'NZL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (158, N'Nicaragua', N'NI', N'NIC')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (159, N'Niger', N'NE', N'NER')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (160, N'Nigeria', N'NG', N'NGA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (161, N'Niue', N'NU', N'NIU')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (162, N'Norfolk Island', N'NF', N'NFK')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (163, N'North Korea', N'KP', N'PRK')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (164, N'Northern Mariana Islands', N'MP', N'MNP')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (165, N'Norway', N'NO', N'NOR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (166, N'Oman', N'OM', N'OMN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (167, N'Pakistan', N'PK', N'PAK')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (168, N'Palau', N'PW', N'PLW')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (169, N'Palestine', N'PS', N'PSE')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (170, N'Panama', N'PA', N'PAN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (171, N'Papua New Guinea', N'PG', N'PNG')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (172, N'Paraguay', N'PY', N'PRY')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (173, N'Peru', N'PE', N'PER')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (174, N'Phillipines', N'PH', N'PHL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (175, N'Pitcairn', N'PN', N'PCN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (176, N'Poland', N'PL', N'POL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (177, N'Portugal', N'PT', N'PRT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (178, N'Puerto Rico', N'PR', N'PRI')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (179, N'Qatar', N'QA', N'QAT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (180, N'Reunion', N'RE', N'REU')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (181, N'Romania', N'RO', N'ROU')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (182, N'Russia', N'RU', N'RUS')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (183, N'Rwanda', N'RW', N'RWA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (184, N'Saint Barthelemy', N'BL', N'BLM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (185, N'Saint Helena', N'SH', N'SHN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (186, N'Saint Kitts and Nevis', N'KN', N'KNA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (187, N'Saint Lucia', N'LC', N'LCA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (188, N'Saint Martin', N'MF', N'MAF')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (189, N'Saint Pierre and Miquelon', N'PM', N'SPM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (190, N'Saint Vincent and the Grenadines', N'VC', N'VCT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (191, N'Samoa', N'WS', N'WSM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (192, N'San Marino', N'SM', N'SMR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (193, N'Sao Tome and Principe', N'ST', N'STP')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (194, N'Saudi Arabia', N'SA', N'SAU')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (195, N'Senegal', N'SN', N'SEN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (196, N'Serbia', N'RS', N'SRB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (197, N'Seychelles', N'SC', N'SYC')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (198, N'Sierra Leone', N'SL', N'SLE')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (199, N'Singapore', N'SG', N'SGP')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (200, N'Sint Maarten', N'SX', N'SXM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (201, N'Slovakia', N'SK', N'SVK')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (202, N'Slovenia', N'SI', N'SVN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (203, N'Solomon Islands', N'SB', N'SLB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (204, N'Somalia', N'SO', N'SOM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (205, N'South Africa', N'ZA', N'ZAF')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (206, N'South Georgia and the South Sandwich Islands', N'GS', N'SGS')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (207, N'South Korea', N'KR', N'KOR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (208, N'South Sudan', N'SS', N'SSD')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (209, N'Spain', N'ES', N'ESP')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (210, N'Sri Lanka', N'LK', N'LKA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (211, N'Sudan', N'SD', N'SDN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (212, N'Suriname', N'SR', N'SUR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (213, N'Svalbard and Jan Mayen', N'SJ', N'SJM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (214, N'Swaziland', N'SZ', N'SWZ')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (215, N'Sweden', N'SE', N'SWE')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (216, N'Switzerland', N'CH', N'CHE')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (217, N'Syria', N'SY', N'SYR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (218, N'Taiwan', N'TW', N'TWN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (219, N'Tajikistan', N'TJ', N'TJK')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (220, N'Tanzania', N'TZ', N'TZA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (221, N'Thailand', N'TH', N'THA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (222, N'Timor-Leste (East Timor)', N'TL', N'TLS')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (223, N'Togo', N'TG', N'TGO')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (224, N'Tokelau', N'TK', N'TKL')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (225, N'Tonga', N'TO', N'TON')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (226, N'Trinidad and Tobago', N'TT', N'TTO')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (227, N'Tunisia', N'TN', N'TUN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (228, N'Turkey', N'TR', N'TUR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (229, N'Turkmenistan', N'TM', N'TKM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (230, N'Turks and Caicos Islands', N'TC', N'TCA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (231, N'Tuvalu', N'TV', N'TUV')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (232, N'Uganda', N'UG', N'UGA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (233, N'Ukraine', N'UA', N'UKR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (234, N'United Arab Emirates', N'AE', N'ARE')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (235, N'United Kingdom', N'GB', N'GBR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (236, N'United States', N'US', N'USA')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (237, N'United States Minor Outlying Islands', N'UM', N'UMI')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (238, N'Uruguay', N'UY', N'URY')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (239, N'Uzbekistan', N'UZ', N'UZB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (240, N'Vanuatu', N'VU', N'VUT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (241, N'Vatican City', N'VA', N'VAT')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (242, N'Venezuela', N'VE', N'VEN')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (243, N'Vietnam', N'VN', N'VNM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (244, N'Virgin Islands, British', N'VG', N'VGB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (245, N'Virgin Islands, US', N'VI', N'VIR')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (246, N'Wallis and Futuna', N'WF', N'WLF')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (247, N'Western Sahara', N'EH', N'ESH')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (248, N'Yemen', N'YE', N'YEM')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (249, N'Zambia', N'ZM', N'ZMB')
GO
INSERT [dbo].[Countries] ([Id], [Name], [TwoCharCountryCode], [ThreeCharCountryCode]) VALUES (250, N'Zimbabwe', N'ZW', N'ZWE')
GO
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[EquipmentClassifications] ON 
GO
INSERT [dbo].[EquipmentClassifications] ([Id], [Name], [IsActive]) VALUES (1, N'Aircraft', 1)
GO
INSERT [dbo].[EquipmentClassifications] ([Id], [Name], [IsActive]) VALUES (2, N'Engine', 1)
GO
INSERT [dbo].[EquipmentClassifications] ([Id], [Name], [IsActive]) VALUES (3, N'Propeller', 1)
GO
INSERT [dbo].[EquipmentClassifications] ([Id], [Name], [IsActive]) VALUES (4, N'Appliance', 1)
GO
INSERT [dbo].[EquipmentClassifications] ([Id], [Name], [IsActive]) VALUES (5, N'Avionics', 1)
GO
SET IDENTITY_INSERT [dbo].[EquipmentClassifications] OFF
GO
SET IDENTITY_INSERT [dbo].[EquipmentStatuses] ON 
GO
INSERT [dbo].[EquipmentStatuses] ([Id], [Name], [IsActive]) VALUES (1, N'Installed', 1)
GO
INSERT [dbo].[EquipmentStatuses] ([Id], [Name], [IsActive]) VALUES (2, N'Repaired', 1)
GO
INSERT [dbo].[EquipmentStatuses] ([Id], [Name], [IsActive]) VALUES (3, N'Removed', 1)
GO
SET IDENTITY_INSERT [dbo].[EquipmentStatuses] OFF
GO
SET IDENTITY_INSERT [dbo].[FlightSimulatorClasses] ON 
GO
INSERT [dbo].[FlightSimulatorClasses] ([Id], [Name]) VALUES (1, N'Aviation Training Device (ATD)')
GO
INSERT [dbo].[FlightSimulatorClasses] ([Id], [Name]) VALUES (2, N'Full Flight Simulator (FFS)')
GO
INSERT [dbo].[FlightSimulatorClasses] ([Id], [Name]) VALUES (3, N'Flight Training Device (FTD)')
GO
SET IDENTITY_INSERT [dbo].[FlightSimulatorClasses] OFF
GO
SET IDENTITY_INSERT [dbo].[InstructorTypes] ON 
GO
INSERT [dbo].[InstructorTypes] ([Id], [Name]) VALUES (1, N'None')
GO
INSERT [dbo].[InstructorTypes] ([Id], [Name]) VALUES (2, N'Chief Flight Instructor')
GO
INSERT [dbo].[InstructorTypes] ([Id], [Name]) VALUES (3, N'Asst. Chief Instructor')
GO
INSERT [dbo].[InstructorTypes] ([Id], [Name]) VALUES (4, N'Check Instructor')
GO
SET IDENTITY_INSERT [dbo].[InstructorTypes] OFF
GO

SET IDENTITY_INSERT [dbo].[Permissions] ON 
GO
INSERT [dbo].[Permissions] ([Id], [PermissionType]) VALUES (1, N'Create')
GO
INSERT [dbo].[Permissions] ([Id], [PermissionType]) VALUES (2, N'View')
GO
INSERT [dbo].[Permissions] ([Id], [PermissionType]) VALUES (3, N'Edit')
GO
INSERT [dbo].[Permissions] ([Id], [PermissionType]) VALUES (4, N'Delete')
GO
SET IDENTITY_INSERT [dbo].[Permissions] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 
GO
INSERT [dbo].[UserRoles] (Id, [Name], [Priority]) VALUES (1,N'Super Admin',1)
GO
INSERT [dbo].[UserRoles] (id, [Name], [Priority]) VALUES (2, N'Admin',2)
GO
INSERT [dbo].[UserRoles] (id, [Name], [Priority]) VALUES (3, N'Office Staff',999)
GO
INSERT [dbo].[UserRoles] (id, [Name], [Priority]) VALUES (4, N'Instructor',999)
GO
INSERT [dbo].[UserRoles] (id,[Name], [Priority]) VALUES (5, N'Mechanic',999)
GO
INSERT [dbo].[UserRoles] (id,[Name], [Priority]) VALUES (6, N'Pilot (Renter)',999)
GO
INSERT [dbo].[UserRoles] (id,[Name], [Priority]) VALUES (7, N'Owner',999)
GO
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO

SET IDENTITY_INSERT [dbo].ModuleDetails ON
GO
INSERT INTO ModuleDetails(Id,Name,ControllerName,ActionName,DisplayName,Icon,OrderNo,IsActive) VALUES (1,'Company','Company','Index','Company','account_balance',1,1)
INSERT INTO ModuleDetails(Id,Name,ControllerName,ActionName,DisplayName,Icon,OrderNo,IsActive) VALUES (2,'User','User','Index','User','accessibility',2,1)
INSERT INTO ModuleDetails(Id,Name,ControllerName,ActionName,DisplayName,Icon,OrderNo,IsActive) VALUES (3,'InstructorType','InstructorType','Index','Instructor Type','assignment_ind',3,1)
INSERT INTO ModuleDetails(Id,Name,ControllerName,ActionName,DisplayName,Icon,OrderNo,IsActive) VALUES (4,'Aircraft','Aircraft','Index','Aircraft','flight_takeoff',4,1)
INSERT INTO ModuleDetails(Id,Name,ControllerName,ActionName,DisplayName,Icon,OrderNo,IsActive) VALUES (5,'UserRolePermission','UserRolePermission','Index','User Role Permission','lock_open',5,1)
INSERT INTO ModuleDetails(Id,Name,ControllerName,ActionName,DisplayName,Icon,OrderNo,IsActive) VALUES (6,'Scheduler','Scheduler','Index','Schedule','event_note',6,1)
Go
SET IDENTITY_INSERT [dbo].ModuleDetails OFF
GO

--INSERT INTO UserRolePermissions VALUES(1,1,1,null,1)
--INSERT INTO UserRolePermissions VALUES(1,2,1,null,1)
--INSERT INTO UserRolePermissions VALUES(1,3,1,null,1)
--INSERT INTO UserRolePermissions VALUES(1,4,1,null,1)
--INSERT INTO UserRolePermissions VALUES(1,1,2,null,1)
--INSERT INTO UserRolePermissions VALUES(1,2,2,null,1)
--INSERT INTO UserRolePermissions VALUES(1,3,2,null,1)
--INSERT INTO UserRolePermissions VALUES(1,4,2,null,1)
--INSERT INTO UserRolePermissions VALUES(1,1,3,null,1)
--INSERT INTO UserRolePermissions VALUES(1,2,3,null,1)
--INSERT INTO UserRolePermissions VALUES(1,3,3,null,1)
--INSERT INTO UserRolePermissions VALUES(1,4,3,null,1)
--INSERT INTO UserRolePermissions VALUES(1,1,4,null,1)
--INSERT INTO UserRolePermissions VALUES(1,2,4,null,1)
--INSERT INTO UserRolePermissions VALUES(1,3,4,null,1)
--INSERT INTO UserRolePermissions VALUES(1,4,4,null,1)
--INSERT INTO UserRolePermissions VALUES(1,1,5,null,1)
--INSERT INTO UserRolePermissions VALUES(1,2,5,null,1)
--INSERT INTO UserRolePermissions VALUES(1,3,5,null,1)
--INSERT INTO UserRolePermissions VALUES(1,4,5,null,1)
--Go

INSERT INTO Users Values('Vishal', 'Modi', 'vmodi@cendien.com', 1,'8980228627',1,0,null,null,null,'1994-10-21','Male',
						102,'VGVzdEAxMjQ=',1,1,0,SYSDATETIME(),null,null,null,null,null)
GO

INSERT INTO Users Values('Israel', 'Denis', 'idenis@cendien.com', 1,'(214) 202-5896',1,0,null,null,null,'1994-10-21','Male',
						236,'VGVzdEAxMjQ=',1,1,0,SYSDATETIME(),null,null,null,null,null)
GO

SET IDENTITY_INSERT [dbo].ScheduleActivityTypes ON
GO
INSERT INTO ScheduleActivityTypes(Id,Name,IsActive) VALUES (1,'Charter Flight',1)
INSERT INTO ScheduleActivityTypes(Id,Name,IsActive) VALUES (2,'Discovery Flight',1)
INSERT INTO ScheduleActivityTypes(Id,Name,IsActive) VALUES (3,'Dual Flight Training',1)
INSERT INTO ScheduleActivityTypes(Id,Name,IsActive) VALUES (4,'Ground Training',1)
INSERT INTO ScheduleActivityTypes(Id,Name,IsActive) VALUES (5,'Maintenance',1)
INSERT INTO ScheduleActivityTypes(Id,Name,IsActive) VALUES (6,'Renter Flight',1)
INSERT INTO ScheduleActivityTypes(Id,Name,IsActive) VALUES (7,'Student Solo',1)
INSERT INTO ScheduleActivityTypes(Id,Name,IsActive) VALUES (8,'Tour Flight',1)
Go
SET IDENTITY_INSERT [dbo].ScheduleActivityTypes OFF
GO

SET IDENTITY_INSERT [dbo].UserRoleVsScheduleActivityType ON
GO

Insert into [dbo].[UserRoleVsScheduleActivityType] (Id, UserRoleId, ActivityTypeIds) values (1, 2, '1,2,3,4,5,6,7,8')
Insert into [dbo].[UserRoleVsScheduleActivityType] (Id, UserRoleId, ActivityTypeIds) values (2, 3, '1,2,3,4,5,6,7,8')
Insert into [dbo].[UserRoleVsScheduleActivityType] (Id, UserRoleId, ActivityTypeIds) values (3, 4, '2,3,4,7')
Insert into [dbo].[UserRoleVsScheduleActivityType] (Id, UserRoleId, ActivityTypeIds) values (4, 6, '3,6')

GO
SET IDENTITY_INSERT [dbo].UserRoleVsScheduleActivityType OFF
GO