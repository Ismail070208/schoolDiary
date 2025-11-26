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