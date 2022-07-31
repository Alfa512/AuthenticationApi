
/*CREATE DATABASE AuthenticationApi
    GO

CREATE TABLE Users (
        Id BigInt IDENTITY(1,1) PRIMARY KEY,
        UserIdentifier uniqueidentifier NOT NULL UNIQUE,
        UserName NVarChar(255) NOT NULL,
        NormalizedUserName NVarChar(255) NOT NULL,
        Email NVarChar(255) NOT NULL,
        NormalizedEmail NVarChar(255) NOT NULL,
        EmailConfirmed bit NOT NULL,
        PasswordHash NVarChar(255) NOT NULL,
        SecurityStamp NVarChar(255) NULL,
        ConcurrencyStamp NVarChar(255) NULL,
        PhoneNumber NVarChar(255) NULL,
        PhoneNumberConfirmed bit NOT NULL,
        TwoFactorEnabled bit not NULL,
        LockoutEnd DateTime NULL,
        LockoutEnabled bit NOT NULL,
        AccessFailedCount NVarChar(255)  NULL,
    [FirstName] NVarChar(255) NOT NULL,
    LastName NVarChar(255) NOT NULL,
    MiddleName NVarChar(255) NULL,
DateOfBirth DateTime NULL,
    Gender bit NOT NULL,
    RegistrationDate DateTime NOT NULL,
    RestorePasswordToken NVarChar(255) NULL,
TokenValidTo DateTime NULL
    );

      ALTER TABLE [dbo].[Users]
  ADD [RegistrationComplete] BIT not null DEFAULT(1)

CREATE TABLE Posts (
    Id BigInt IDENTITY(1,1) PRIMARY KEY,
    PostIdentifier uniqueidentifier NOT NULL UNIQUE,
    CreateBy bigint NOT NULL,
    ChangeBy bigint NOT NULL,
    CreateById uniqueidentifier NOT NULL,
    ChangeById uniqueidentifier NOT NULL,
    CreateDate DateTime NOT NULL,
    ChangeDate DateTime NOT NULL,
    Permalink NVarChar(MAX) NOT NULL,
    Title NVarChar(255) NOT NULL,
    Body NVarChar(MAX) NOT NULL,
    Tag NVarChar(255) NOT NULL,
    PosterImageLink NVarChar(MAX) NOT NULL
);


alter table [dbo].[Posts] 
ADD CONSTRAINT FK_UserPost
FOREIGN KEY (CreatedBy) REFERENCES [Users](Id)

alter table [dbo].[Posts] 
ADD CONSTRAINT FK_UserPostId
FOREIGN KEY (CreatedById) REFERENCES [Users](UserIdentifier)


alter table [dbo].[Posts] 
ADD CONSTRAINT FK_UserPostChangedBy
FOREIGN KEY (ChangedBy) REFERENCES [Users](Id)

alter table [dbo].[Posts] 
ADD CONSTRAINT FK_UserPostChangedById
FOREIGN KEY (ChangedById) REFERENCES [Users](UserIdentifier)


CREATE TABLE PostPreviews(
    Id BigInt IDENTITY(1, 1) PRIMARY KEY,
    PostPreviewIdentifier uniqueidentifier NOT NULL UNIQUE,
    PostIdentifier uniqueidentifier NOT NULL,
    ChangeDate DateTime NOT NULL,
    Title NVarChar(255) NOT NULL,
    Tag NVarChar(255) NOT NULL,
    Author NVarChar(255) NOT NULL,
    Comments Int NOT NULL,
    TextPreview NVarChar(MAX) NOT NULL,
    Permalink NVarChar(MAX) NOT NULL,
    PosterImageLink NVarChar(MAX) NOT NULL,
);

alter table [dbo].[PostPreviews]
ADD CONSTRAINT FK_PostPostPreview
FOREIGN KEY (PostIdentifier) REFERENCES [Posts](PostIdentifier)

alter table [dbo].[PostPreviews] add PostId bigint not null DEFAULT(1)
alter table [dbo].[PostPreviews]
ADD CONSTRAINT FK_PostPostPreview
FOREIGN KEY (PostId) REFERENCES [Posts](Id)

CREATE TABLE Images (
    Id BigInt IDENTITY(1,1) PRIMARY KEY,
    ImageIdentifier uniqueidentifier NOT NULL UNIQUE,
    CreateDate DateTime NOT NULL,
    [Name] NVarChar(255) NOT NULL,
    [Label] NVarChar(255) NOT NULL,
    [Path] NVarChar(MAX) NOT NULL
    );

CREATE TABLE Comments(
	Id BigInt IDENTITY(1,1) PRIMARY KEY,
	CommentIdentifier uniqueidentifier NOT NULL UNIQUE,
	PostIdentifier uniqueidentifier NOT NULL,
	UserIdentifier uniqueidentifier NOT NULL,
	CreateDate DateTime NOT NULL,
	ChangeDate DateTime NOT NULL,
	[Text] NVarChar(MAX) NOT NULL
	
)


alter table [dbo].[Comments] 
ADD CONSTRAINT FK_CommentPost
FOREIGN KEY (PostIdentifier) REFERENCES [Posts](PostIdentifier)


alter table [dbo].[Comments] 
ADD CONSTRAINT FK_CommentUser
FOREIGN KEY (UserIdentifier) REFERENCES [Users](UserIdentifier)


alter table [dbo].[Comments]  add ChangedBy bigint NOT NULL DEFAULT(1)
alter table [dbo].[Comments] 
ADD CONSTRAINT FK_UserCommentChangedBy
FOREIGN KEY (ChangedBy) REFERENCES [Users](Id)

alter table [dbo].[Comments]  add PostId bigint NOT NULL DEFAULT(1)
alter table [dbo].[Comments] 
ADD CONSTRAINT FK_PostCommentPostId
FOREIGN KEY (PostId) REFERENCES [Posts](Id)


  CREATE TABLE [Configuration]
  (
  Id Int IDENTITY(1,1) PRIMARY KEY,
  [Name] NVarChar(255) NOT NULL,
  [Value] NVarChar(255) NOT NULL,
  [IsActive] bit NOT NULL
  )

  
CREATE TABLE UserRoles
(
UserId BigInt NOT NULL,
RoleId BigInt NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE Roles
(
Id BigInt IDENTITY(1,1) PRIMARY KEY,
[Name] NVarChar(255) NOT NULL,
[NormalizedName] NVarChar(255) NOT NULL,
[ConcurrencyStamp] NVarChar(255) NULL,
)

alter table [dbo].[UserRoles] 
ADD CONSTRAINT FK_UserRolesUserId
FOREIGN KEY (UserId) REFERENCES [Users](Id)

alter table [dbo].[UserRoles] 
ADD CONSTRAINT FK_UserRolesRoleId
FOREIGN KEY (RoleId) REFERENCES [Roles](Id)

CREATE TABLE Claims
(
Id Int IDENTITY(1,1) PRIMARY KEY,
[UserId] BigInt NOT NULL,
[ClaimType] NVarChar(255) NULL,
[ClaimValue] NVarChar(255) NULL
)

ALTER TABLE [dbo].[Claims] 
ADD CONSTRAINT FK_UserClaimsUserId
FOREIGN KEY (UserId) REFERENCES [Users](Id)

CREATE TABLE Logins
(
	[Id] BigInt IDENTITY(1,1) PRIMARY KEY,
	[CreateDate] DateTime NOT NULL DEFAULT(GETDATE()),
	[ChangeDate] DateTime NOT NULL DEFAULT(GETDATE()),
	[ChangeBy] NVarChar(100) NOT NULL DEFAULT(SUSER_NAME()),
	[UserId] BigInt NOT NULL,
	[AccessToken] NVarChar(255) NULL,
	[RefreshToken] NVarChar(255) NULL,
	[AccessTokenExpiryTime] DateTime NULL,
	[RefreshTokenExpiryTime] DateTime NULL,
	[LoginProviderId] INT NOT NULL,
	[ProviderCode] NVarChar(255) NOT NULL
	
)

ALTER TABLE [dbo].[Logins] 
ADD CONSTRAINT FK_UserLoginsUserId
FOREIGN KEY (UserId) REFERENCES [Users](Id)

ALTER TABLE [dbo].[Logins] 
ADD CONSTRAINT FK_Logins_LoginProviders
FOREIGN KEY ([LoginProviderId]) REFERENCES [LoginProviders](Id)

CREATE TABLE [LoginProviders] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
	[CreateDate] DateTime NOT NULL DEFAULT(GETDATE()),
	[ChangeDate] DateTime NOT NULL DEFAULT(GETDATE()),
	[ChangeBy] NVarChar(100) NOT NULL DEFAULT(SUSER_NAME()),
    [ProviderCode] NVarChar(255) NOT NULL,
	[ProviderDisplayName] NVarChar(255) NOT NULL,
	[ProviderUrl] NVarChar(255) NOT NULL
);


CREATE TABLE Tokens
(
[UserId] BigInt NOT NULL,
[LoginProvider] NVarChar(255) NOT NULL,
[Name] NVarChar(255) NOT NULL,
[Value] NVarChar(255) NOT NULL
)

ALTER TABLE [dbo].[Tokens] 
ADD CONSTRAINT FK_UserTokensUserId
FOREIGN KEY (UserId) REFERENCES [Users](Id)

ALTER TABLE [dbo].[Logins] 
ADD  CONSTRAINT [PK_dbo.UserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

ALTER TABLE [dbo].[Tokens] 
ADD  CONSTRAINT [PK_dbo.Tokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

    */