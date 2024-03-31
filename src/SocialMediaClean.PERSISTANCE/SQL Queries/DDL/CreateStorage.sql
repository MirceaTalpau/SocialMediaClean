USE [SocialMediaClean]

CREATE TABLE Users(
ID INT IDENTITY PRIMARY KEY,
Email VARCHAR(255) NOT NULL UNIQUE,
Password VARCHAR(MAX),
PasswordSalt VARCHAR(MAX),
PhoneNumber VARCHAR(15),
JoinedAt DATETIME DEFAULT GETDATE(),
BirthDay DATETIME,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
Gender VARCHAR(2),
Country VARCHAR(50),
City VARCHAR(50),
Address VARCHAR(75),
ProfilePictureURL VARCHAR(MAX),
CoverPictureURL VARCHAR(MAX),
PasswordForgotToken VARCHAR(MAX),
PasswordChangeToken VARCHAR(MAX),
EmailVerified BIT NOT NULL DEFAULT 0,
EmailVerifyToken VARCHAR(MAX)
);

CREATE UNIQUE NONCLUSTERED INDEX id1_phoneNumber_notnull
ON Users(PhoneNumber)
WHERE PhoneNumber IS NOT NULL;


CREATE TABLE LogEntries (
    Id INT PRIMARY KEY IDENTITY,
    Message NVARCHAR(MAX),
    LogLevel NVARCHAR(50),
    Timestamp DATETIME DEFAULT GETDATE()
);

CREATE TABLE Groups(
ID INT IDENTITY PRIMARY KEY,
AuthorID INT FOREIGN KEY REFERENCES Users(ID) NOT NULL,
Name NVARCHAR(50) NOT NULL,
Description NVARCHAR(MAX),
CreatedAt DATETIME DEFAULT GETDATE(),
);

CREATE TABLE Statuses(
ID INT IDENTITY PRIMARY KEY,
Status NVARCHAR(MAX) NOT NULL,
)

CREATE TABLE Posts(
ID INT IDENTITY PRIMARY KEY,
AuthorID INT FOREIGN KEY REFERENCES Users(ID) NOT NULL,
SharedByID INT FOREIGN KEY REFERENCES Users(ID) DEFAULT NULL,
StatusID INT FOREIGN KEY REFERENCES Statuses(ID) DEFAULT 1,
GroupID INT FOREIGN KEY REFERENCES Groups(ID) DEFAULT NULL,
Body NVARCHAR(MAX) NOT NULL,
CreatedAt DATETIME DEFAULT GETDATE(),
);

CREATE TABLE Pictures(
ID INT IDENTITY PRIMARY KEY,
PostID INT FOREIGN KEY REFERENCES Posts(ID),
PictureURI NVARCHAR(MAX) NOT NULL,
CreatedAt DATETIME DEFAULT GETDATE(),
);

CREATE TABLE Videos(
ID INT IDENTITY PRIMARY KEY,
PostID INT FOREIGN KEY REFERENCES Posts(ID),
VideoURI NVARCHAR(MAX) NOT NULL,
CreatedAt DATETIME DEFAULT GETDATE(),
);

INSERT INTO Statuses(Status) VALUES('Public');
INSERT INTO Statuses(Status) VALUES('Friends');
INSERT INTO Statuses(Status) VALUES('OnlyMe');


