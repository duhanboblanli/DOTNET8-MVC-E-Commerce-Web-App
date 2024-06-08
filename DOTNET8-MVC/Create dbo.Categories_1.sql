USE [E-Commerce]
GO

/****** Object: Table [dbo].[Categories] Script Date: 15.05.2024 6:53:17 ÖS ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Categories] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (30) NOT NULL,
    [DisplayOrder] INT           NOT NULL,
	[AD]         NVARCHAR (30) NOT NULL
);


