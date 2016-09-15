USE [ccms]
GO
/****** Object:  Table [dbo].[viewtree]    Script Date: 05/17/2014 12:47:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[viewtree](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[page_id] [int] NOT NULL,
	[parent_id] [int] NOT NULL,
	[layout_id] [int] NULL,
	[ordering] [int] NOT NULL,
 CONSTRAINT [PK_viewtree] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


USE [ccms]
GO
/****** Object:  Table [dbo].[users]    Script Date: 05/17/2014 12:47:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[login] [varchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[password] [varchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[fullname] [varchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[email] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[active] [bit] NOT NULL,
	[permissions] [int] NOT NULL,
	[groups] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF


USE [ccms]
GO
/****** Object:  Table [dbo].[template]    Script Date: 05/17/2014 12:47:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[template](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](max) COLLATE Latin1_General_CI_AS NOT NULL,
	[data] [varchar](max) COLLATE Latin1_General_CI_AS NOT NULL,
	[auth_group] [int] NOT NULL CONSTRAINT [DF_template_auth_group]  DEFAULT ((0)),
	[active] [bit] NOT NULL CONSTRAINT [DF_template_active]  DEFAULT ((1)),
 CONSTRAINT [PK_template] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF


USE [ccms]
GO
/****** Object:  Table [dbo].[state]    Script Date: 05/17/2014 12:47:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[state](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](15) COLLATE Latin1_General_CI_AS NOT NULL,
	[description] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_state] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF


USE [ccms]
GO
/****** Object:  Table [dbo].[session_data]    Script Date: 05/17/2014 12:47:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[session_data](
	[session_id] [varchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[user_id] [int] NOT NULL,
	[user_data] [varchar](max) COLLATE Latin1_General_CI_AS NOT NULL,
	[active] [bit] NOT NULL CONSTRAINT [DF_session_data_active]  DEFAULT ((0)),
	[session_opened] [datetime] NOT NULL,
	[session_closed] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

USE [ccms]
GO
/****** Object:  Table [dbo].[page_content_ref]    Script Date: 05/17/2014 12:47:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[page_content_ref](
	[content_id] [int] NOT NULL,
	[page_id] [int] NOT NULL,
	[slot_num] [int] NULL
) ON [PRIMARY]



USE [ccms]
GO
/****** Object:  Table [dbo].[page]    Script Date: 05/17/2014 12:47:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[page](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[state] [int] NOT NULL,
	[name] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[linktext] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[title] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[description] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[keywords] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[created_date] [datetime] NULL,
	[created_user] [int] NULL,
	[updated_date] [datetime] NULL,
	[updated_user] [int] NULL,
	[layout_id] [int] NULL,
	[auth_group] [int] NULL,
 CONSTRAINT [PK_page] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF


USE [ccms]
GO
/****** Object:  Table [dbo].[layout]    Script Date: 05/17/2014 12:47:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[layout](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[layout_url] [varchar](255) COLLATE Latin1_General_CI_AS NULL,
	[active] [bit] NOT NULL CONSTRAINT [DF_layout_active]  DEFAULT ((1)),
	[auth_group] [int] NULL,
 CONSTRAINT [PK_layout] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

USE [ccms]
GO
/****** Object:  Table [dbo].[groups]    Script Date: 05/17/2014 12:47:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[groups](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[description] [varchar](255) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF




USE [ccms]
GO
/****** Object:  Table [dbo].[content_version]    Script Date: 05/17/2014 12:47:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[content_version](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[content_id] [int] NOT NULL,
	[version_id] [int] NOT NULL,
	[state_id] [int] NOT NULL,
	[data] [text] COLLATE Latin1_General_CI_AS NULL,
	[edit_user] [int] NOT NULL,
	[updated_date] [datetime] NULL,
 CONSTRAINT [PK_content_version] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]




