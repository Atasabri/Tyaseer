
create proc [dbo].[EditProviderFCM]
@provider_id int ,
@fcm nvarchar(max)
as
begin
update Providers set FCM=@fcm WHERE ID=@provider_id
end
GO
/****** Object:  StoredProcedure [dbo].[EditUserFCM]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[EditUserFCM]
@user_id int ,
@fcm nvarchar(max)
as
begin
update Users set FCM=@fcm WHERE ID=@user_id
end
GO
/****** Object:  Table [dbo].[Admins]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admins](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[IsManager] [bit] NOT NULL,
	[Access_Users] [bit] NOT NULL,
	[Access_Products] [bit] NOT NULL,
	[Access_Categories] [bit] NOT NULL,
	[Access_Contacts] [bit] NOT NULL,
	[Access_Orders] [bit] NOT NULL,
	[Access_Providers] [bit] NOT NULL,
	[Access_Codes] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[OffersOnly] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Cities]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CityName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Codes]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Codes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Discount] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmailVerify]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailVerify](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order_Details]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_Details](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Price] [float] NOT NULL,
	[Order_ID] [int] NOT NULL,
	[Product_ID] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[Accepted] [bit] NOT NULL,
	[DateNeeded] [date] NOT NULL,
	[Discount] [float] NULL,
	[FinalPrice] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Orders]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [date] NOT NULL,
	[TotalPrice] [float] NOT NULL,
	[FinalPrice] [float] NOT NULL,
	[User_ID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Phone] [nvarchar](22) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Discount] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product_Data]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_Data](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Item] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Product_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product_NotAvaiableDates]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_NotAvaiableDates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [date] NOT NULL,
	[Product_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product_Photos]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_Photos](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Product_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product_Rates]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_Rates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Rate] [float] NULL,
	[Comment] [nvarchar](max) NULL,
	[Product_ID] [int] NOT NULL,
	[User_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product_Types]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_Types](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Product_ID] [int] NOT NULL,
	[Type_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Price] [float] NOT NULL,
	[Lat] [float] NOT NULL,
	[Log] [float] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsOffer] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[Cat_ID] [int] NOT NULL,
	[Provider_ID] [int] NOT NULL,
	[City_ID] [int] NOT NULL,
	[NumberOfUsers] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Provider_Rates]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provider_Rates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Rate] [float] NULL,
	[Comment] [nvarchar](max) NULL,
	[Provider_ID] [int] NOT NULL,
	[User_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Providers]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Providers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](300) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[TradeName] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](22) NOT NULL,
	[Expire_Date] [date] NULL,
	[FCM] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Subscribers]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscribers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Types]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Types](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/19/2019 10:30:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](300) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NULL,
	[Phone] [nvarchar](22) NOT NULL,
	[Facebook_ID] [nvarchar](max) NULL,
	[Twitter_ID] [nvarchar](max) NULL,
	[Google_ID] [nvarchar](max) NULL,
	[FCM] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

GO
INSERT [dbo].[Categories] ([ID], [Name], [OffersOnly]) VALUES (2, N'مكان', 0)
GO
INSERT [dbo].[Categories] ([ID], [Name], [OffersOnly]) VALUES (3, N'تنسيق', 0)
GO
INSERT [dbo].[Categories] ([ID], [Name], [OffersOnly]) VALUES (4, N'طعام', 0)
GO
INSERT [dbo].[Categories] ([ID], [Name], [OffersOnly]) VALUES (5, N'عروض مميزة', 1)
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Cities] ON 

GO
INSERT [dbo].[Cities] ([ID], [CityName]) VALUES (1, N'Cairo')
GO
INSERT [dbo].[Cities] ([ID], [CityName]) VALUES (2, N'6 October')
GO
INSERT [dbo].[Cities] ([ID], [CityName]) VALUES (3, N'Giza')
GO
SET IDENTITY_INSERT [dbo].[Cities] OFF
GO
SET IDENTITY_INSERT [dbo].[Order_Details] ON 

GO
INSERT [dbo].[Order_Details] ([ID], [Price], [Order_ID], [Product_ID], [Count], [Accepted], [DateNeeded], [Discount], [FinalPrice]) VALUES (1, 465.98, 1, 6, 2, 1, CAST(0xE33E0B00 AS Date), NULL, 465.98)
GO
INSERT [dbo].[Order_Details] ([ID], [Price], [Order_ID], [Product_ID], [Count], [Accepted], [DateNeeded], [Discount], [FinalPrice]) VALUES (2, 232.99, 1, 7, 1, 0, CAST(0xE43E0B00 AS Date), NULL, 232.99)
GO
INSERT [dbo].[Order_Details] ([ID], [Price], [Order_ID], [Product_ID], [Count], [Accepted], [DateNeeded], [Discount], [FinalPrice]) VALUES (3, 465.98, 2, 6, 2, 0, CAST(0xE33E0B00 AS Date), 50, 232.99)
GO
INSERT [dbo].[Order_Details] ([ID], [Price], [Order_ID], [Product_ID], [Count], [Accepted], [DateNeeded], [Discount], [FinalPrice]) VALUES (4, 465.98, 3, 6, 2, 0, CAST(0xE33E0B00 AS Date), NULL, 465.98)
GO
INSERT [dbo].[Order_Details] ([ID], [Price], [Order_ID], [Product_ID], [Count], [Accepted], [DateNeeded], [Discount], [FinalPrice]) VALUES (5, 465.98, 4, 6, 2, 0, CAST(0xE23E0B00 AS Date), NULL, 465.98)
GO
SET IDENTITY_INSERT [dbo].[Order_Details] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

GO
INSERT [dbo].[Orders] ([ID], [Date], [TotalPrice], [FinalPrice], [User_ID], [Name], [Address], [Phone], [Email], [Discount]) VALUES (1, CAST(0xE23E0B00 AS Date), 698.97, 698.97, 1, N'Ata Sabri Ata Ahmed', N'taha subra - Cairo', N'01142229025', N'ataeldon@gmail.com', NULL)
GO
INSERT [dbo].[Orders] ([ID], [Date], [TotalPrice], [FinalPrice], [User_ID], [Name], [Address], [Phone], [Email], [Discount]) VALUES (2, CAST(0xE23E0B00 AS Date), 465.98, 232.99, 1, N'Ata Sabri Ata Ahmed', N'taha subra - Cairo', N'01142229025', N'ataeldon@gmail.com', 50)
GO
INSERT [dbo].[Orders] ([ID], [Date], [TotalPrice], [FinalPrice], [User_ID], [Name], [Address], [Phone], [Email], [Discount]) VALUES (3, CAST(0xE23E0B00 AS Date), 465.98, 465.98, 1, N'Ata Sabri Ata Ahmed', N'taha subra - Cairo', N'01142229025', N'ataeldon@gmail.com', NULL)
GO
INSERT [dbo].[Orders] ([ID], [Date], [TotalPrice], [FinalPrice], [User_ID], [Name], [Address], [Phone], [Email], [Discount]) VALUES (4, CAST(0xE23E0B00 AS Date), 465.98, 465.98, 1, N'Ata Sabri Ata Ahmed', N'taha subra - Cairo', N'01142229025', N'ataeldon@gmail.com', NULL)
GO
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Product_Data] ON 

GO
INSERT [dbo].[Product_Data] ([ID], [Item], [Value], [Product_ID]) VALUES (1, N'المساحة ', N'545 م', 7)
GO
INSERT [dbo].[Product_Data] ([ID], [Item], [Value], [Product_ID]) VALUES (2, N'عدد الكراسي', N'200 كرسي', 7)
GO
SET IDENTITY_INSERT [dbo].[Product_Data] OFF
GO
SET IDENTITY_INSERT [dbo].[Product_NotAvaiableDates] ON 

GO
INSERT [dbo].[Product_NotAvaiableDates] ([ID], [Date], [Product_ID]) VALUES (1, CAST(0xDC3E0B00 AS Date), 7)
GO
INSERT [dbo].[Product_NotAvaiableDates] ([ID], [Date], [Product_ID]) VALUES (2, CAST(0xDD3E0B00 AS Date), 7)
GO
INSERT [dbo].[Product_NotAvaiableDates] ([ID], [Date], [Product_ID]) VALUES (4, CAST(0xDF3E0B00 AS Date), 7)
GO
INSERT [dbo].[Product_NotAvaiableDates] ([ID], [Date], [Product_ID]) VALUES (5, CAST(0xE23E0B00 AS Date), 7)
GO
SET IDENTITY_INSERT [dbo].[Product_NotAvaiableDates] OFF
GO
SET IDENTITY_INSERT [dbo].[Product_Photos] ON 

GO
INSERT [dbo].[Product_Photos] ([ID], [Product_ID]) VALUES (7, 7)
GO
INSERT [dbo].[Product_Photos] ([ID], [Product_ID]) VALUES (8, 7)
GO
SET IDENTITY_INSERT [dbo].[Product_Photos] OFF
GO
SET IDENTITY_INSERT [dbo].[Product_Rates] ON 

GO
INSERT [dbo].[Product_Rates] ([ID], [Rate], [Comment], [Product_ID], [User_ID]) VALUES (1, 3.9, N'This Is Good', 7, 1)
GO
SET IDENTITY_INSERT [dbo].[Product_Rates] OFF
GO
SET IDENTITY_INSERT [dbo].[Product_Types] ON 

GO
INSERT [dbo].[Product_Types] ([ID], [Product_ID], [Type_ID]) VALUES (1, 7, 1)
GO
INSERT [dbo].[Product_Types] ([ID], [Product_ID], [Type_ID]) VALUES (2, 7, 2)
GO
SET IDENTITY_INSERT [dbo].[Product_Types] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

GO
INSERT [dbo].[Products] ([ID], [Name], [Price], [Lat], [Log], [Description], [IsOffer], [Active], [Cat_ID], [Provider_ID], [City_ID], [NumberOfUsers]) VALUES (6, N'Product 2', 232.99, 20.19289182, 21.989898, N'This Prodcut For Test', 0, 1, 2, 6, 1, 12)
GO
INSERT [dbo].[Products] ([ID], [Name], [Price], [Lat], [Log], [Description], [IsOffer], [Active], [Cat_ID], [Provider_ID], [City_ID], [NumberOfUsers]) VALUES (7, N'Product 2', 232.99, 20.19289182, 21.989898, N'This Prodcut For Test', 0, 1, 2, 6, 1, 12)
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Provider_Rates] ON 

GO
INSERT [dbo].[Provider_Rates] ([ID], [Rate], [Comment], [Provider_ID], [User_ID]) VALUES (1, 4, N'This Is Coooool', 6, 1)
GO
INSERT [dbo].[Provider_Rates] ([ID], [Rate], [Comment], [Provider_ID], [User_ID]) VALUES (2, NULL, N'This Is Coooool Provider', 3, 1)
GO
SET IDENTITY_INSERT [dbo].[Provider_Rates] OFF
GO
SET IDENTITY_INSERT [dbo].[Providers] ON 

GO
INSERT [dbo].[Providers] ([ID], [Token], [Name], [Email], [Password], [TradeName], [Phone], [Expire_Date], [FCM]) VALUES (3, N'WC8vw2CQkGrLUip1e/cUO+HjmFkdFMJL+rMmt/oNB37kGKLW75QQUYBooUVCegkP', N'Ata Sabri', N'ataelon@gmail.com', N'123456', N'Ata_Sabri', N'01142229025', NULL, N'12334456778')
GO
INSERT [dbo].[Providers] ([ID], [Token], [Name], [Email], [Password], [TradeName], [Phone], [Expire_Date], [FCM]) VALUES (6, N't7fCbntMWD/40QY7jsXmT4a3jYMNJ7Fmazx5divD14xJZS6iZZk8sst+A+I7kOsi', N'Ata Sabri', N'ataeldon@gmail.com', N'123', N'Ata_Sabri', N'01142229025', NULL, N'12334456778')
GO
SET IDENTITY_INSERT [dbo].[Providers] OFF
GO
SET IDENTITY_INSERT [dbo].[Subscribers] ON 

GO
INSERT [dbo].[Subscribers] ([ID], [Email]) VALUES (1, N'ataeldon@gmail.com')
GO
SET IDENTITY_INSERT [dbo].[Subscribers] OFF
GO
SET IDENTITY_INSERT [dbo].[Types] ON 

GO
INSERT [dbo].[Types] ([ID], [TypeName]) VALUES (1, N'فرح')
GO
INSERT [dbo].[Types] ([ID], [TypeName]) VALUES (2, N'طهور')
GO
INSERT [dbo].[Types] ([ID], [TypeName]) VALUES (3, N'سبوع')
GO
INSERT [dbo].[Types] ([ID], [TypeName]) VALUES (4, N'حنة')
GO
SET IDENTITY_INSERT [dbo].[Types] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

GO
INSERT [dbo].[Users] ([ID], [Token], [Name], [Email], [Password], [Phone], [Facebook_ID], [Twitter_ID], [Google_ID], [FCM]) VALUES (1, N't7fCbntMWD/40QY7jsXmT4a3jYMNJ7Fmazx5divD14xJZS6iZZk8sst+A+I7kOsi', N'Ata Sabri', N'ataeldon@gmail.com', N'01142229025', N'01142229025', NULL, NULL, NULL, N'32323223232')
GO
INSERT [dbo].[Users] ([ID], [Token], [Name], [Email], [Password], [Phone], [Facebook_ID], [Twitter_ID], [Google_ID], [FCM]) VALUES (2, N'hAyjrtLGXZTBlBrJ8qq3qgI/4qdiln7rPC7t+hyQz22ehVQGnUz1HMsb+lbTOjCy', N'Ata Ahmed', N'ataeldn@gmail.com', NULL, N'01142229025', N'0123456677', NULL, NULL, N'32323223232')
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__EmailVer__A9D10534119156A1]    Script Date: 11/19/2019 10:30:15 AM ******/
ALTER TABLE [dbo].[EmailVerify] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Provider__1EB4F8176548C1EF]    Script Date: 11/19/2019 10:30:15 AM ******/
ALTER TABLE [dbo].[Providers] ADD UNIQUE NONCLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Provider__A9D105347921EE20]    Script Date: 11/19/2019 10:30:15 AM ******/
ALTER TABLE [dbo].[Providers] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Users__1EB4F81777EC355E]    Script Date: 11/19/2019 10:30:15 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Users__A9D10534AAE2BCFA]    Script Date: 11/19/2019 10:30:15 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Order_Details]  WITH CHECK ADD  CONSTRAINT [FK__Order_Det__Order__31EC6D26] FOREIGN KEY([Order_ID])
REFERENCES [dbo].[Orders] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order_Details] CHECK CONSTRAINT [FK__Order_Det__Order__31EC6D26]
GO
ALTER TABLE [dbo].[Order_Details]  WITH CHECK ADD  CONSTRAINT [FK__Order_Det__Produ__32E0915F] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order_Details] CHECK CONSTRAINT [FK__Order_Det__Produ__32E0915F]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK__Orders__User_ID__2F10007B] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK__Orders__User_ID__2F10007B]
GO
ALTER TABLE [dbo].[Product_Data]  WITH CHECK ADD  CONSTRAINT [FK__Product_D__Produ__29572725] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Data] CHECK CONSTRAINT [FK__Product_D__Produ__29572725]
GO
ALTER TABLE [dbo].[Product_NotAvaiableDates]  WITH CHECK ADD  CONSTRAINT [FK__Product_N__Produ__2C3393D0] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_NotAvaiableDates] CHECK CONSTRAINT [FK__Product_N__Produ__2C3393D0]
GO
ALTER TABLE [dbo].[Product_Photos]  WITH CHECK ADD  CONSTRAINT [FK__Product_P__Produ__22AA2996] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Photos] CHECK CONSTRAINT [FK__Product_P__Produ__22AA2996]
GO
ALTER TABLE [dbo].[Product_Rates]  WITH CHECK ADD  CONSTRAINT [FK__Product_R__Produ__25869641] FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Rates] CHECK CONSTRAINT [FK__Product_R__Produ__25869641]
GO
ALTER TABLE [dbo].[Product_Rates]  WITH CHECK ADD  CONSTRAINT [FK__Product_R__User___267ABA7A] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Rates] CHECK CONSTRAINT [FK__Product_R__User___267ABA7A]
GO
ALTER TABLE [dbo].[Product_Types]  WITH CHECK ADD FOREIGN KEY([Product_ID])
REFERENCES [dbo].[Products] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product_Types]  WITH CHECK ADD FOREIGN KEY([Type_ID])
REFERENCES [dbo].[Types] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK__Cities__City_ID__1FCDBCEB] FOREIGN KEY([City_ID])
REFERENCES [dbo].[Cities] ([ID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK__Cities__City_ID__1FCDBCEB]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK__Products__Cat_ID__1FCDBCEB] FOREIGN KEY([Cat_ID])
REFERENCES [dbo].[Categories] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK__Products__Cat_ID__1FCDBCEB]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK__Providers__Provider_ID__1FCDBCEB] FOREIGN KEY([Provider_ID])
REFERENCES [dbo].[Providers] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK__Providers__Provider_ID__1FCDBCEB]
GO
ALTER TABLE [dbo].[Provider_Rates]  WITH CHECK ADD  CONSTRAINT [FK__Provider___Provi__1BFD2C07] FOREIGN KEY([Provider_ID])
REFERENCES [dbo].[Providers] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Provider_Rates] CHECK CONSTRAINT [FK__Provider___Provi__1BFD2C07]
GO
ALTER TABLE [dbo].[Provider_Rates]  WITH CHECK ADD  CONSTRAINT [FK__Provider___User___1CF15040] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Provider_Rates] CHECK CONSTRAINT [FK__Provider___User___1CF15040]
GO
USE [master]
GO
ALTER DATABASE [Tyaseer] SET  READ_WRITE 
GO
