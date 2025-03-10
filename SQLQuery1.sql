CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    Age INT NOT NULL,
    Height DECIMAL(5,2),
    Weight DECIMAL(5,2),
    Email NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    CreatedBy INT,
    UpdatedBy INT,
    DeletedBy INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    DeletedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE FitnessPrograms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProgramName NVARCHAR(255) NOT NULL,
    ProgramDescription NVARCHAR(MAX) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Duration INT,
    ImageUrl NVARCHAR(255),
    CreatedBy INT,
    UpdatedBy INT,
    DeletedBy INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    DeletedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE ProgramDetails (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProgramId INT,
    ExerciseName NVARCHAR(100) NOT NULL,
    Sets INT NOT NULL,
    Reps INT NOT NULL,
    Duration INT,
    CaloriesBurned DECIMAL(5,2),
    VideoUrl NVARCHAR(255),
    CreatedBy INT,
    UpdatedBy INT,
    DeletedBy INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    DeletedDate DATETIME,
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (ProgramId) REFERENCES FitnessPrograms(Id) ON DELETE CASCADE
);

CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    ProgramId INT,
    OrderDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Pending',
    CreatedBy INT,
    UpdatedBy INT,
    DeletedBy INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    DeletedDate DATETIME,
    IsDeleted BIT DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProgramId) REFERENCES FitnessPrograms(Id)
);
