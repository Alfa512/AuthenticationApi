/*
USE [AuthenticationApi]
GO

INSERT INTO [dbo].[Users]
           ([UserIdentifier]
           ,[UserName]
           ,[NormalizedUserName]
           ,[Email]
           ,[NormalizedEmail]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[ConcurrencyStamp]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEnd]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
           ,[FirstName]
           ,[LastName]
           ,[MiddleName]
           ,[DateOfBirth]
           ,[Gender]
           ,[RegistrationDate]
           ,[RestorePasswordToken]
           ,[TokenValidTo])
     VALUES
           ('00000001-0001-0007-0001-000000000000'
           ,'Alfa'
           ,'alfa'
           ,'alfa512ks@mail.ru'
           ,'alfa512ks@mail.ru'
           ,0
           ,'NOPASS007'
           ,'NISSMO34Alfa'
           ,'cstamp'
           ,null
           ,0
           ,0
           ,null
           ,0
           ,0
           ,'Ruslan'
           ,'Muslimov'
           ,'S'
           ,GetDate()
           ,1
           ,GetDate()
           ,null
           ,null)
GO

--alter table  [dbo].[Users] alter column [AccessFailedCount] int not null

  INSERT INTO [AuthenticationApi].[dbo].[Roles]
  ([Name],[NormalizedName])
  VALUES ('User', 'user')

  
  INSERT INTO [AuthenticationApi].[dbo].[Roles]
  ([Name],[NormalizedName])
  VALUES ('Administrator', 'administrator')

*/