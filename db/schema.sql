
/****** Object:  Table [dbo].[card_deck]    Script Date: 4/9/2018 4:47:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[card_deck](
	[CardID] [int] NOT NULL,
	[DeckID] [int] NOT NULL,
 CONSTRAINT [PK_card_deck] PRIMARY KEY CLUSTERED 
(
	[CardID] ASC,
	[DeckID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[card_tag]    Script Date: 4/9/2018 4:47:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[card_tag](
	[TagID] [int] NOT NULL,
	[CardID] [int] NOT NULL,
 CONSTRAINT [PK_card_tag] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC,
	[CardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[cards]    Script Date: 4/9/2018 4:47:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cards](
	[CardID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Front] [nchar](1000) NOT NULL,
	[Back] [nchar](1000) NOT NULL,
 CONSTRAINT [PK_cards] PRIMARY KEY CLUSTERED 
(
	[CardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[deck_tag]    Script Date: 4/9/2018 4:47:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[deck_tag](
	[TagID] [int] NOT NULL,
	[DeckID] [int] NOT NULL,
 CONSTRAINT [PK_deck_tag] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC,
	[DeckID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[decks]    Script Date: 4/9/2018 4:47:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[decks](
	[DeckID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Name] [nchar](50) NOT NULL,
	[IsPublic] [bit] NOT NULL,
 CONSTRAINT [PK_decks] PRIMARY KEY CLUSTERED 
(
	[DeckID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[tags]    Script Date: 4/9/2018 4:47:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [nchar](50) NOT NULL,
 CONSTRAINT [PK_tags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[users]    Script Date: 4/9/2018 4:47:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nchar](50) NOT NULL,
	[Password] [nchar](50) NOT NULL,
	[IsAdmin] [bit] NOT NULL,
	[DisplayName] [nchar](50) NOT NULL,
 CONSTRAINT [PK_users_UserID] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_users_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[decks] ADD  CONSTRAINT [DF_decks_IsPublic]  DEFAULT ((0)) FOR [IsPublic]
GO
ALTER TABLE [dbo].[card_deck]  WITH CHECK ADD  CONSTRAINT [FK_card_deck_cards_CardID] FOREIGN KEY([CardID])
REFERENCES [dbo].[cards] ([CardID])
GO
ALTER TABLE [dbo].[card_deck] CHECK CONSTRAINT [FK_card_deck_cards_CardID]
GO
ALTER TABLE [dbo].[card_deck]  WITH CHECK ADD  CONSTRAINT [FK_card_deck_decks_DeckID] FOREIGN KEY([DeckID])
REFERENCES [dbo].[decks] ([DeckID])
GO
ALTER TABLE [dbo].[card_deck] CHECK CONSTRAINT [FK_card_deck_decks_DeckID]
GO
ALTER TABLE [dbo].[card_tag]  WITH CHECK ADD  CONSTRAINT [FK_card_tag_cards] FOREIGN KEY([CardID])
REFERENCES [dbo].[cards] ([CardID])
GO
ALTER TABLE [dbo].[card_tag] CHECK CONSTRAINT [FK_card_tag_cards]
GO
ALTER TABLE [dbo].[card_tag]  WITH CHECK ADD  CONSTRAINT [FK_card_tag_tags_TagID] FOREIGN KEY([TagID])
REFERENCES [dbo].[tags] ([TagID])
GO
ALTER TABLE [dbo].[card_tag] CHECK CONSTRAINT [FK_card_tag_tags_TagID]
GO
ALTER TABLE [dbo].[cards]  WITH CHECK ADD  CONSTRAINT [FK_cards_users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[users] ([UserID])
GO
ALTER TABLE [dbo].[cards] CHECK CONSTRAINT [FK_cards_users_UserID]
GO
ALTER TABLE [dbo].[deck_tag]  WITH CHECK ADD  CONSTRAINT [FK_deck_tag_decks_DeckID] FOREIGN KEY([DeckID])
REFERENCES [dbo].[decks] ([DeckID])
GO
ALTER TABLE [dbo].[deck_tag] CHECK CONSTRAINT [FK_deck_tag_decks_DeckID]
GO
ALTER TABLE [dbo].[deck_tag]  WITH CHECK ADD  CONSTRAINT [FK_deck_tag_tags_TagID] FOREIGN KEY([TagID])
REFERENCES [dbo].[tags] ([TagID])
GO
ALTER TABLE [dbo].[deck_tag] CHECK CONSTRAINT [FK_deck_tag_tags_TagID]
GO
ALTER TABLE [dbo].[decks]  WITH CHECK ADD  CONSTRAINT [FK_decks_users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[users] ([UserID])
GO
ALTER TABLE [dbo].[decks] CHECK CONSTRAINT [FK_decks_users_UserID]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email is unique in table.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'users', @level2type=N'CONSTRAINT',@level2name=N'IX_users_Email'
GO
USE [master]
GO
ALTER DATABASE [Flashcards] SET  READ_WRITE 
GO
