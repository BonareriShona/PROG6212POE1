
-- Drop the database completely
DROP DATABASE IF EXISTS CMCSWebDB;
-- 1. Create the database
CREATE DATABASE CMCSWebDB;

-- 2. Switch to the database
USE CMCSWebDB;

-- 3. Create Lecturers table
CREATE TABLE Lecturers (
    LecturerId INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    HourlyRate DECIMAL(10,2) NOT NULL
);

-- 4. Create Claims table
CREATE TABLE Claims (
    ClaimId INT AUTO_INCREMENT PRIMARY KEY,
    LecturerId INT NOT NULL,
    HoursWorked DECIMAL(5,2) NOT NULL,
    HourlyRate DECIMAL(10,2) NOT NULL,
    Notes VARCHAR(500),
    Status VARCHAR(50) NOT NULL,
    DocumentPath VARCHAR(255),
    SubmittedAt DATETIME NOT NULL,
    FOREIGN KEY (LecturerId) REFERENCES Lecturers(LecturerId)
);
