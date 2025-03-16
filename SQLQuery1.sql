CREATE TABLE [dbo].[Users] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]   NVARCHAR (100) NOT NULL,
    [LastName]    NVARCHAR (100) NOT NULL,
    [Gender]      NVARCHAR (10)  NOT NULL,
    [Age]         INT            NOT NULL,
    [Height]      DECIMAL (5, 2) NULL,
    [Weight]      DECIMAL (5, 2) NULL,
    [Email]       NVARCHAR (255) NOT NULL,
    [Password]    NVARCHAR (255) NOT NULL,
    [UserRole]    INT            DEFAULT ((2)) NOT NULL,
    [CreatedBy]   INT            NULL,
    [UpdatedBy]   INT            NULL,
    [DeletedBy]   INT            NULL,
    [CreatedDate] DATETIME       DEFAULT (getdate()) NULL,
    [UpdatedDate] DATETIME       NULL,
    [DeletedDate] DATETIME       NULL,
    [IsDeleted]   BIT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[RefreshTokens] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Token]          NVARCHAR (255) NOT NULL,
    [UserId]         INT            NOT NULL,
    [ExpirationDate] DATETIME2 (7)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[ProgramDetails] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [ProgramId]      INT            NULL,
    [ExerciseName]   NVARCHAR (100) NOT NULL,
    [Sets]           INT            NOT NULL,
    [Reps]           INT            NOT NULL,
    [Duration]       INT            NULL,
    [CaloriesBurned] DECIMAL (5, 2) NULL,
    [VideoUrl]       NVARCHAR (255) NULL,
    [CreatedBy]      INT            NULL,
    [UpdatedBy]      INT            NULL,
    [DeletedBy]      INT            NULL,
    [CreatedDate]    DATETIME       DEFAULT (getdate()) NULL,
    [UpdatedDate]    DATETIME       NULL,
    [DeletedDate]    DATETIME       NULL,
    [IsDeleted]      BIT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[FitnessPrograms] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Orders] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [UserId]      INT           NULL,
    [ProgramId]   INT           NULL,
    [OrderDate]   DATETIME      DEFAULT (getdate()) NULL,
    [Status]      NVARCHAR (50) DEFAULT ('Pending') NULL,
    [CreatedBy]   INT           NULL,
    [UpdatedBy]   INT           NULL,
    [DeletedBy]   INT           NULL,
    [CreatedDate] DATETIME      DEFAULT (getdate()) NULL,
    [UpdatedDate] DATETIME      NULL,
    [DeletedDate] DATETIME      NULL,
    [IsDeleted]   BIT           DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ProgramId]) REFERENCES [dbo].[FitnessPrograms] ([Id])
);

CREATE TABLE [dbo].[FitnessPrograms] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [ProgramName]        NVARCHAR (255)  NOT NULL,
    [ProgramDescription] NVARCHAR (MAX)  NOT NULL,
    [Category]           NVARCHAR (50)   NOT NULL,
    [Price]              DECIMAL (10, 2) NOT NULL,
    [Duration]           INT             NULL,
    [ImageUrl]           NVARCHAR (255)  NULL,
    [CreatedBy]          INT             NULL,
    [UpdatedBy]          INT             NULL,
    [DeletedBy]          INT             NULL,
    [CreatedDate]        DATETIME        DEFAULT (getdate()) NULL,
    [UpdatedDate]        DATETIME        NULL,
    [DeletedDate]        DATETIME        NULL,
    [IsDeleted]          BIT             DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
