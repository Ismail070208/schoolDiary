CREATE TABLE Classes (
    Id INT IDENTITY PRIMARY KEY,
    ClassName NVARCHAR(20)
);

CREATE TABLE Students (
    Id INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    ClassId INT FOREIGN KEY REFERENCES Classes(Id)
);

INSERT INTO Classes (ClassName)
VALUES ('5'), ('6'), ('7'), ('8'), ('9'), ('10'), ('11'), ('12');

DELETE FROM Classes;

ALTER TABLE Classes ADD Grade INT;
ALTER TABLE Classes ADD Section NVARCHAR(5);

SELECT * FROM Classes;

UPDATE dbo.Classes
SET Grade = CAST(LEFT(ClassName, LEN(ClassName) - 1) AS INT),
    Section = RIGHT(ClassName, 1);

