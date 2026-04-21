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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250724072616_initial'
)
BEGIN
    CREATE TABLE [Colleges] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Location] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Colleges] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250724072616_initial'
)
BEGIN
    CREATE TABLE [Courses] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [Stream] nvarchar(max) NOT NULL,
        [Duration] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Courses] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250724072616_initial'
)
BEGIN
    CREATE TABLE [AddonCourses] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [CourseId] int NOT NULL,
        CONSTRAINT [PK_AddonCourses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AddonCourses_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250724072616_initial'
)
BEGIN
    CREATE TABLE [AddonColleges] (
        [Id] int NOT NULL IDENTITY,
        [AddonCourseId] int NOT NULL,
        [CollegeId] int NOT NULL,
        CONSTRAINT [PK_AddonColleges] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AddonColleges_AddonCourses_AddonCourseId] FOREIGN KEY ([AddonCourseId]) REFERENCES [AddonCourses] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AddonColleges_Colleges_CollegeId] FOREIGN KEY ([CollegeId]) REFERENCES [Colleges] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250724072616_initial'
)
BEGIN
    CREATE INDEX [IX_AddonColleges_AddonCourseId] ON [AddonColleges] ([AddonCourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250724072616_initial'
)
BEGIN
    CREATE INDEX [IX_AddonColleges_CollegeId] ON [AddonColleges] ([CollegeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250724072616_initial'
)
BEGIN
    CREATE INDEX [IX_AddonCourses_CourseId] ON [AddonCourses] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250724072616_initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250724072616_initial', N'9.0.7');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112135841_MigrationName'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Colleges]') AND [c].[name] = N'Location');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Colleges] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [Colleges] DROP COLUMN [Location];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112135841_MigrationName'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Colleges]') AND [c].[name] = N'Name');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Colleges] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Colleges] ALTER COLUMN [Name] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112135841_MigrationName'
)
BEGIN
    ALTER TABLE [Colleges] ADD [Address] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112135841_MigrationName'
)
BEGIN
    ALTER TABLE [Colleges] ADD [GoogleMapsUrl] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112135841_MigrationName'
)
BEGIN
    ALTER TABLE [Colleges] ADD [IsRecommended] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112135841_MigrationName'
)
BEGIN
    ALTER TABLE [Colleges] ADD [Latitude] float NOT NULL DEFAULT 0.0E0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112135841_MigrationName'
)
BEGIN
    ALTER TABLE [Colleges] ADD [Longitude] float NOT NULL DEFAULT 0.0E0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112135841_MigrationName'
)
BEGIN
    ALTER TABLE [Colleges] ADD [PlaceId] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260112135841_MigrationName'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260112135841_MigrationName', N'9.0.7');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205090024_MakeLatLongNullable'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Colleges]') AND [c].[name] = N'Longitude');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Colleges] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Colleges] ALTER COLUMN [Longitude] float NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205090024_MakeLatLongNullable'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Colleges]') AND [c].[name] = N'Latitude');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Colleges] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Colleges] ALTER COLUMN [Latitude] float NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205090024_MakeLatLongNullable'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Colleges]') AND [c].[name] = N'IsRecommended');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Colleges] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Colleges] ALTER COLUMN [IsRecommended] bit NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260205090024_MakeLatLongNullable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260205090024_MakeLatLongNullable', N'9.0.7');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303105111_AddLeadsTable'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [IsBlocked] bit NOT NULL,
        [Role] nvarchar(max) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303105111_AddLeadsTable'
)
BEGIN
    CREATE TABLE [Leads] (
        [Id] int NOT NULL IDENTITY,
        [FullName] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NULL,
        [CourseInterested] nvarchar(max) NULL,
        [CollegeInterested] nvarchar(max) NULL,
        [Source] int NOT NULL,
        [Status] int NOT NULL,
        [Priority] int NOT NULL,
        [Notes] nvarchar(max) NULL,
        [FollowUpDate] datetime2 NULL,
        [AssignedToUserId] int NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Leads] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Leads_Users_AssignedToUserId] FOREIGN KEY ([AssignedToUserId]) REFERENCES [Users] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303105111_AddLeadsTable'
)
BEGIN
    CREATE INDEX [IX_Leads_AssignedToUserId] ON [Leads] ([AssignedToUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260303105111_AddLeadsTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260303105111_AddLeadsTable', N'9.0.7');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260305054936_lead added'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260305054936_lead added', N'9.0.7');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Colleges]') AND [c].[name] = N'Location');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Colleges] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Colleges] DROP COLUMN [Location];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    ALTER TABLE [Courses] ADD [DisplayOrder] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Colleges]') AND [c].[name] = N'Name');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Colleges] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [Colleges] ALTER COLUMN [Name] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    ALTER TABLE [Colleges] ADD [Address] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    ALTER TABLE [Colleges] ADD [DisplayOrder] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    ALTER TABLE [Colleges] ADD [GoogleMapsUrl] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    ALTER TABLE [Colleges] ADD [IsRecommended] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    ALTER TABLE [Colleges] ADD [Latitude] float NOT NULL DEFAULT 0.0E0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    ALTER TABLE [Colleges] ADD [Longitude] float NOT NULL DEFAULT 0.0E0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    ALTER TABLE [Colleges] ADD [PlaceId] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    ALTER TABLE [AddonCourses] ADD [DisplayOrder] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420064032_AddDisplayOrder'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260420064032_AddDisplayOrder', N'9.0.7');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420101900_UpdateSchema'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Role');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var7 + '];');
    EXEC(N'UPDATE [Users] SET [Role] = N'''' WHERE [Role] IS NULL');
    ALTER TABLE [Users] ALTER COLUMN [Role] nvarchar(max) NOT NULL;
    ALTER TABLE [Users] ADD DEFAULT N'' FOR [Role];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420101900_UpdateSchema'
)
BEGIN
    ALTER TABLE [Courses] ADD [DisplayOrder] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420101900_UpdateSchema'
)
BEGIN
    ALTER TABLE [Colleges] ADD [DisplayOrder] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420101900_UpdateSchema'
)
BEGIN
    ALTER TABLE [AddonCourses] ADD [DisplayOrder] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420101900_UpdateSchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260420101900_UpdateSchema', N'9.0.7');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420104849_ModernizeAdotzeeSchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260420104849_ModernizeAdotzeeSchema', N'9.0.7');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260420110336_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260420110336_InitialCreate', N'9.0.7');
END;

COMMIT;
GO

