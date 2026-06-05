USE StudentDB
CREATE TABLE Students(
    StudentID int PRIMARY KEY IDENTITY(1,1),
    StudentName VARCHAR(100) NOT NULL
);

CREATE TABLE Attendance(
    AttendanceID int PRIMARY KEY IDENTITY(1,1),
    StudentID INT NOT NULL,
    AttendanceDate DATE NOT NULL,
    IsPresent BIT NOT NULL,

    FOREIGN KEY (StudentID) REFERENCES Students(StudentID)
)