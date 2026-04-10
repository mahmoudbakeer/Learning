create database DVLD_Mahmoud

CREATE TABLE Peoples (
    PersonID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    SecondName NVARCHAR(100) NOT NULL,
    ThirdName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(50) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    DateOfBirth DATE NOT NULL,
    NationalityCountryID INT NOT NULL,
    NationalNo NVARCHAR(50) NOT NULL
);

CREATE TABLE Countries (
    CountryID INT IDENTITY(1,1) PRIMARY KEY,
    CountryName NVARCHAR(100) NOT NULL
);

CREATE TABLE LicenseClasses (
    LicenseClassID INT IDENTITY(1,1) PRIMARY KEY,
    ClassName NVARCHAR(100) NOT NULL,
    ClassDescription NVARCHAR(255) NOT NULL,
    MinimumAllowedAge INT NOT NULL,
    DefaultValiditylength INT NOT NULL,
    ClassFees DECIMAL(10,2) NOT NULL
);

CREATE TABLE ApplicationTypes (
    ApplicationTypeID INT IDENTITY(1,1) PRIMARY KEY,
    ApplicationTypeTitle NVARCHAR(100) NOT NULL,
    ApplicationTypeFees DECIMAL(10,2) NOT NULL
);

CREATE TABLE TestTypes (
    TestTypeID INT IDENTITY(1,1) PRIMARY KEY,
    TestTypeTitle NVARCHAR(100) NOT NULL,
    TestTypeFees DECIMAL(10,2) NOT NULL,
    TestTypeDescription NVARCHAR(255) NOT NULL
);

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    PersonID INT NOT NULL,
    IsActive BIT NOT NULL
);

CREATE TABLE Applications (
    ApplicationID INT IDENTITY(1,1) PRIMARY KEY,
    ApplicationDate DATE NOT NULL,
    ApplicationStatus NVARCHAR(255) NOT NULL,
    ApplicantPersonID INT NOT NULL,
    CreatedByUserID INT NOT NULL,
    ApplicationTypeID INT NOT NULL,
    PaidFees DECIMAL(10,2) NOT NULL,
    LastStatusDate DATE NOT NULL
);

CREATE TABLE LocalDrivingLicenseApplications (
    LocalDrivingLicenseApplicationID INT IDENTITY(1,1) PRIMARY KEY,
    ApplicationID INT NOT NULL,
    LicenseClassID INT NOT NULL
);

CREATE TABLE TestAppointments (
    TestAppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    CreatedByUserID INT NOT NULL,
    LocalDrivingLicenseApplicationID INT NOT NULL,
    TestClassID INT NOT NULL,
    AppointmentDate DATE NOT NULL,
    PaidFees DECIMAL(10,2) NOT NULL,
    IsLocked BIT NOT NULL
);

CREATE TABLE Tests (
    TestID INT IDENTITY(1,1) PRIMARY KEY,
    TestResult INT NOT NULL,
    Notes NVARCHAR(255) NOT NULL,
    CreatedByUserID INT NOT NULL,
    TestAppointmentID INT NOT NULL
);

CREATE TABLE Drivers (
    DriverID INT IDENTITY(1,1) PRIMARY KEY,
    PersonID INT NOT NULL,
    CreatedByUserID INT NOT NULL,
    CreatedDate DATE NOT NULL
);

CREATE TABLE Licenses (
    LicenseID INT IDENTITY(1,1) PRIMARY KEY,
    LicenseClassID INT NOT NULL,
    IssueDate DATE NOT NULL,
    ExpirationDate DATE NOT NULL,
    CreatedByUserID INT NOT NULL,
    DriverID INT NOT NULL,
    ApplicationID INT NOT NULL,
    PaidFees DECIMAL(10,2) NOT NULL,
    IsActive BIT NOT NULL,
    Notes NVARCHAR(255) NOT NULL,
    IssueReason NVARCHAR(255) NOT NULL
);

CREATE TABLE InternationalLicenses (
    InternationalLicenseID INT IDENTITY(1,1) PRIMARY KEY,
    CreatedByUserID INT NOT NULL,
    IssueDate DATE NOT NULL,
    ExpirationDate DATE NOT NULL,
    DriverID INT NOT NULL,
    LicenseID INT NOT NULL,
    ApplicationID INT NOT NULL,
    IsActive BIT NOT NULL
);

CREATE TABLE DetainedLicenses (
    DetainedID INT IDENTITY(1,1) PRIMARY KEY,
    ReleaseApplicationID INT NOT NULL,
    DetainDate DATE NOT NULL,
    ReleaseDate DATE NOT NULL,
    IsReleased BIT NOT NULL,
    FineFees DECIMAL(10,2) NOT NULL,
    ReleasedByUserID INT NOT NULL,
    LicenseID INT NOT NULL,
    CreatedByUserID INT NOT NULL
);


ALTER TABLE Peoples
ADD FOREIGN KEY (NationalityCountryID) REFERENCES Countries(CountryID);

ALTER TABLE Users
ADD FOREIGN KEY (PersonID) REFERENCES Peoples(PersonID);

ALTER TABLE Applications
ADD FOREIGN KEY (ApplicantPersonID) REFERENCES Peoples(PersonID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    FOREIGN KEY (ApplicationTypeID) REFERENCES ApplicationTypes(ApplicationTypeID);

ALTER TABLE LocalDrivingLicenseApplications
ADD FOREIGN KEY (ApplicationID) REFERENCES Applications(ApplicationID),
    FOREIGN KEY (LicenseClassID) REFERENCES LicenseClasses(LicenseClassID);

ALTER TABLE TestAppointments
ADD FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    FOREIGN KEY (LocalDrivingLicenseApplicationID) REFERENCES LocalDrivingLicenseApplications(LocalDrivingLicenseApplicationID),
    FOREIGN KEY (TestClassID) REFERENCES TestTypes(TestTypeID);

ALTER TABLE Tests
ADD FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    FOREIGN KEY (TestAppointmentID) REFERENCES TestAppointments(TestAppointmentID);

ALTER TABLE Drivers
ADD FOREIGN KEY (PersonID) REFERENCES Peoples(PersonID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID);

ALTER TABLE Licenses
ADD FOREIGN KEY (LicenseClassID) REFERENCES LicenseClasses(LicenseClassID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    FOREIGN KEY (DriverID) REFERENCES Drivers(DriverID),
    FOREIGN KEY (ApplicationID) REFERENCES Applications(ApplicationID);

ALTER TABLE InternationalLicenses
ADD FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID),
    FOREIGN KEY (DriverID) REFERENCES Drivers(DriverID),
    FOREIGN KEY (LicenseID) REFERENCES Licenses(LicenseID),
    FOREIGN KEY (ApplicationID) REFERENCES Applications(ApplicationID);

ALTER TABLE DetainedLicenses
ADD FOREIGN KEY (ReleaseApplicationID) REFERENCES Applications(ApplicationID),
    FOREIGN KEY (ReleasedByUserID) REFERENCES Users(UserID),
    FOREIGN KEY (LicenseID) REFERENCES Licenses(LicenseID),
    FOREIGN KEY (CreatedByUserID) REFERENCES Users(UserID);
