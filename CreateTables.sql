create database projectDB;

-- USERS TABLE
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY,
    FullName VARCHAR(100),
    Email VARCHAR(100) UNIQUE,
    PasswordHash VARCHAR(255),
    Role VARCHAR(50), -- 'Student', 'Recruiter', 'TPO', 'Coordinator'
    IsApproved BIT DEFAULT 0
);
select * from StudentBoothCheckIn;
select * from Companies
select * from Booths
-- STUDENTS TABLE
CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    FAST_ID VARCHAR(15) UNIQUE,
    DegreeProgram VARCHAR(10),
    CurrentSemester INT,
    GPA DECIMAL(3,2)
);
SELECT EventID, Title FROM JobFairEvents ORDER BY Date DESC
select * from booths
select * from Companies


-- COMPANIES TABLE
CREATE TABLE Companies (
    CompanyID INT PRIMARY KEY IDENTITY,
    CompanyName VARCHAR(100),
    Sector VARCHAR(100),
    Website VARCHAR(100),
    Address VARCHAR(200)
);

-- RECRUITERS TABLE
CREATE TABLE Recruiters (
    RecruiterID INT PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    CompanyID INT FOREIGN KEY REFERENCES Companies(CompanyID),
    Designation VARCHAR(100),
    ContactNo VARCHAR(20)
);

-- JOB POSTINGS TABLE
CREATE TABLE JobPostings (
    JobID INT PRIMARY KEY IDENTITY,
    CompanyID INT FOREIGN KEY REFERENCES Companies(CompanyID),
    Title VARCHAR(100),
    Description TEXT,
    JobType VARCHAR(20), -- Internship / Full-time
    SalaryRange VARCHAR(50),
    RequiredSkills TEXT,
    Location VARCHAR(100)
);
        
select * from BoothCoordinators
-- JOB FAIR EVENTS TABLE
CREATE TABLE JobFairEvents (
    EventID INT PRIMARY KEY IDENTITY,
    Title VARCHAR(100),
    Description TEXT,
    Date DATE,
    StartTime TIME,
    EndTime TIME,
    Venue VARCHAR(100)
);

-- BOOTHS TABLE
CREATE TABLE Booths (
    BoothID INT PRIMARY KEY IDENTITY,
    EventID INT FOREIGN KEY REFERENCES JobFairEvents(EventID),
    CompanyID INT FOREIGN KEY REFERENCES Companies(CompanyID),
	 CoordinatorID INT FOREIGN KEY (CoordinatorID) REFERENCES Users(UserID),
	VisitorCount INT,
    Location VARCHAR(50)
);

--checkin table
CREATE TABLE StudentBoothCheckIn (
    PRIMARY KEY (StudentID, BoothID),
	CheckInTime DATETIME,
    StudentID INT FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    BoothID INT FOREIGN KEY (BoothID) REFERENCES Booths(BoothID)
);

-- BOOTH COORDINATORS TABLE
CREATE TABLE BoothCoordinators (
    CoordinatorID INT PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    AssignedBoothID INT FOREIGN KEY REFERENCES Booths(BoothID),
    ShiftTime VARCHAR(50)
);

-- APPLICATIONS TABLE
CREATE TABLE Applications (
    ApplicationID INT PRIMARY KEY IDENTITY,
    StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
    JobID INT FOREIGN KEY REFERENCES JobPostings(JobID),
    ApplicationDate DATE
);

-- INTERVIEWS TABLE
CREATE TABLE Interviews (
    InterviewID INT PRIMARY KEY IDENTITY,
    ApplicationID INT FOREIGN KEY REFERENCES Applications(ApplicationID),
    RecruiterID INT FOREIGN KEY REFERENCES Recruiters(RecruiterID),
    InterviewTime DATETIME,
    InterviewLocation VARCHAR(100),
    Result VARCHAR(50) -- Pending, Selected, Rejected
);

-- REVIEWS TABLE
CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY,
    InterviewID INT FOREIGN KEY REFERENCES Interviews(InterviewID),
    StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Feedback TEXT
);


-- SKILLS TABLE
CREATE TABLE Skills (
    SkillID INT PRIMARY KEY IDENTITY,
    SkillName VARCHAR(100) UNIQUE
);

-- CERTIFICATES TABLE
CREATE TABLE Certificates (
    CertificateID INT PRIMARY KEY IDENTITY,
    CertificateName VARCHAR(100) UNIQUE
);


-- REVIEWS for Recruiters TABLE
CREATE TABLE ReviewsRec (
    ReviewID INT PRIMARY KEY IDENTITY,
    StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
	RecruiterID INT FOREIGN KEY REFERENCES Recruiters(RecruiterID),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Feedback TEXT
);


-- STUDENT_SKILLS (Junction Table for Students and Skills)
CREATE TABLE StudentSkills (
    StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
    SkillID INT FOREIGN KEY REFERENCES Skills(SkillID),
    PRIMARY KEY (StudentID, SkillID)
);

-- STUDENT_CERTIFICATES (Junction Table for Students and Certificates)
CREATE TABLE StudentCertificates (
    StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
    CertificateID INT FOREIGN KEY REFERENCES Certificates(CertificateID),
    DateObtained DATE,
    PRIMARY KEY (StudentID, CertificateID)
);

