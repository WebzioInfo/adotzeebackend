IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Colleges] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Colleges] PRIMARY KEY ([Id])
);

CREATE TABLE [Courses] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [Stream] nvarchar(max) NOT NULL,
    [Duration] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id])
);

CREATE TABLE [AddonCourses] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [CourseId] int NOT NULL,
    CONSTRAINT [PK_AddonCourses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AddonCourses_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AddonColleges] (
    [Id] int NOT NULL IDENTITY,
    [AddonCourseId] int NOT NULL,
    [CollegeId] int NOT NULL,
    CONSTRAINT [PK_AddonColleges] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AddonColleges_AddonCourses_AddonCourseId] FOREIGN KEY ([AddonCourseId]) REFERENCES [AddonCourses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AddonColleges_Colleges_CollegeId] FOREIGN KEY ([CollegeId]) REFERENCES [Colleges] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AddonColleges_AddonCourseId] ON [AddonColleges] ([AddonCourseId]);

CREATE INDEX [IX_AddonColleges_CollegeId] ON [AddonColleges] ([CollegeId]);

CREATE INDEX [IX_AddonCourses_CourseId] ON [AddonCourses] ([CourseId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250724072616_initial', N'9.0.7');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Colleges]') AND [c].[name] = N'Location');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Colleges] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Colleges] DROP COLUMN [Location];

ALTER TABLE [Courses] ADD [DisplayOrder] int NOT NULL DEFAULT 0;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Colleges]') AND [c].[name] = N'Name');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Colleges] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Colleges] ALTER COLUMN [Name] nvarchar(max) NULL;

ALTER TABLE [Colleges] ADD [Address] nvarchar(max) NULL;

ALTER TABLE [Colleges] ADD [DisplayOrder] int NOT NULL DEFAULT 0;

ALTER TABLE [Colleges] ADD [GoogleMapsUrl] nvarchar(max) NULL;

ALTER TABLE [Colleges] ADD [IsRecommended] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Colleges] ADD [Latitude] float NOT NULL DEFAULT 0.0E0;

ALTER TABLE [Colleges] ADD [Longitude] float NOT NULL DEFAULT 0.0E0;

ALTER TABLE [Colleges] ADD [PlaceId] nvarchar(max) NULL;

ALTER TABLE [AddonCourses] ADD [DisplayOrder] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260420064032_AddDisplayOrder', N'9.0.7');

COMMIT;
GO

