USE SchoolDiaryDB;
GO

CREATE TABLE Students (
    Id INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    ClassName NVARCHAR(20)
);

CREATE TABLE Teachers (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100),
    Subject NVARCHAR(50)
);

CREATE TABLE Attendance (
    Id INT IDENTITY PRIMARY KEY,
    StudentId INT FOREIGN KEY REFERENCES Students(Id),
    Date DATE,
    IsPresent BIT,
    Note NVARCHAR(200)
);

CREATE TABLE Grades (
    Id INT IDENTITY PRIMARY KEY,
    StudentId INT FOREIGN KEY REFERENCES Students(Id),
    Subject NVARCHAR(50),
    Grade FLOAT,
    Teacher NVARCHAR(100),
    Date DATE
);

CREATE TABLE Attendance (
    Id INT IDENTITY PRIMARY KEY,
    StudentId INT NOT NULL,
    TeacherId INT NOT NULL,
    Date DATE NOT NULL,
    IsPresent BIT NOT NULL,
    Note NVARCHAR(200),

    CONSTRAINT FK_Attendance_Student FOREIGN KEY (StudentId) REFERENCES Students(Id),
    CONSTRAINT FK_Attendance_Teacher FOREIGN KEY (TeacherId) REFERENCES Teachers(Id)
);