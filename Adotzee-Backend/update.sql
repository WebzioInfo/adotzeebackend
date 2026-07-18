BEGIN TRANSACTION;
ALTER TABLE [AddonColleges] DROP CONSTRAINT [FK_AddonColleges_AddonCourses_AddonCourseId];

ALTER TABLE [AddonColleges] DROP CONSTRAINT [FK_AddonColleges_Colleges_CollegeId];

ALTER TABLE [AddonCourses] DROP CONSTRAINT [FK_AddonCourses_Courses_CourseId];

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Email');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Users] ALTER COLUMN [Email] nvarchar(450) NOT NULL;

ALTER TABLE [Users] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Users] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Users] ADD [UpdatedAt] datetime2 NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Leads]') AND [c].[name] = N'Email');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Leads] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Leads] ALTER COLUMN [Email] nvarchar(450) NULL;

ALTER TABLE [Courses] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Courses] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Courses] ADD [UpdatedAt] datetime2 NULL;

ALTER TABLE [Colleges] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Colleges] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Colleges] ADD [UpdatedAt] datetime2 NULL;

ALTER TABLE [AddonCourses] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [AddonCourses] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [AddonCourses] ADD [UpdatedAt] datetime2 NULL;

ALTER TABLE [AddonColleges] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [AddonColleges] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [AddonColleges] ADD [UpdatedAt] datetime2 NULL;

CREATE TABLE [Reviews] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(255) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [MobileNumber] nvarchar(20) NULL,
    [City] nvarchar(100) NULL,
    [State] nvarchar(100) NULL,
    [Course] nvarchar(255) NOT NULL,
    [CollegeName] nvarchar(255) NULL,
    [Rating] int NOT NULL,
    [ReviewTitle] nvarchar(500) NOT NULL,
    [ReviewMessage] nvarchar(max) NOT NULL,
    [StudentPhoto] nvarchar(max) NULL,
    [VerificationType] nvarchar(max) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Featured] bit NOT NULL,
    [DisplayName] nvarchar(255) NULL,
    [DisplayInitials] nvarchar(10) NULL,
    [IsAnonymous] bit NOT NULL,
    [ApprovedAt] datetime2 NULL,
    [ApprovedBy] nvarchar(255) NULL,
    [IpAddress] nvarchar(50) NULL,
    [UserAgent] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Reviews] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

CREATE INDEX [IX_Leads_Email] ON [Leads] ([Email]);

CREATE INDEX [IX_Leads_Status] ON [Leads] ([Status]);

ALTER TABLE [AddonColleges] ADD CONSTRAINT [FK_AddonColleges_AddonCourses_AddonCourseId] FOREIGN KEY ([AddonCourseId]) REFERENCES [AddonCourses] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [AddonColleges] ADD CONSTRAINT [FK_AddonColleges_Colleges_CollegeId] FOREIGN KEY ([CollegeId]) REFERENCES [Colleges] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [AddonCourses] ADD CONSTRAINT [FK_AddonCourses_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260718151138_AddReviewModule', N'9.0.7');

COMMIT;
GO

