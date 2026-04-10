INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationTypeFees)
VALUES 
('New Local Driving License Service', 15.00),
('Renew Driving License Service', 5.00),
('Replacement for a Lost Driving License', 10.00),
('Replacement for a Damaged Driving License', 5.00),
('Release Detained Driving License', 15.00),
('New International License', 50.00);


INSERT INTO LicenseClasses 
(ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees)
VALUES
('Class 1 - Small Motorcycle',
 'It allows the driver to drive small motorcycles.',
 18, 5, 15.00),

('Class 2 - Heavy Motorcycle License',
 'Heavy Motorcycle License (Large Motorcycle).',
 21, 5, 30.00),

('Class 3 - Ordinary driving license',
 'Ordinary driving license (car license).',
 18, 10, 20.00),

('Class 4 - Commercial',
 'Commercial driving license (taxi/limousine).',
 21, 10, 200.00),

('Class 5 - Agricultural',
 'Agricultural and work vehicles used in farming.',
 21, 10, 50.00),

('Class 6 - Small and medium bus',
 'Small and medium bus license.',
 21, 10, 250.00),

('Class 7 - Truck and heavy vehicle',
 'Truck and heavy vehicle license.',
 21, 10, 300.00);


 INSERT INTO TestTypes 
(TestTypeTitle, TestTypeDescription, TestTypeFees)
VALUES
('Vision Test',
 'This assesses the applicant''s visual acuity to ensure they have sufficient vision to drive safely.',
 10.00),

('Written (Theory) Test',
 'This test assesses the applicant''s knowledge of traffic rules, road signs, and driving regulations. It typically consists of multiple-choice questions.',
 20.00),

('Practical (Street) Test',
 'This test evaluates the applicant''s driving skills and ability to operate a motor vehicle safely on public roads. A licensed examiner supervises the test.',
 30.00);


 ALTER TABLE Peoples
ALTER COLUMN DateOfBirth DATETIME NOT NULL;

ALTER TABLE Applications
ALTER COLUMN ApplicationDate DATETIME NOT NULL;

ALTER TABLE Applications
ALTER COLUMN LastStatusDate DATETIME NOT NULL;

ALTER TABLE TestAppointments
ALTER COLUMN AppointmentDate DATETIME NOT NULL;

ALTER TABLE Drivers
ALTER COLUMN CreatedDate DATETIME NOT NULL;

ALTER TABLE Licenses
ALTER COLUMN IssueDate DATETIME NOT NULL;

ALTER TABLE Licenses
ALTER COLUMN ExpirationDate DATETIME NOT NULL;

ALTER TABLE InternationalLicenses
ALTER COLUMN IssueDate DATETIME2 NOT NULL;

ALTER TABLE InternationalLicenses
ALTER COLUMN ExpirationDate DATETIME NOT NULL;

ALTER TABLE DetainedLicenses
ALTER COLUMN DetainDate DATETIME NOT NULL;

ALTER TABLE DetainedLicenses
ALTER COLUMN ReleaseDate DATETIME NOT NULL;


