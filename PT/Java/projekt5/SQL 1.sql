CREATE TABLE Department
(
	DepartmentID int NOT NULL
            GENERATED ALWAYS AS IDENTITY
            (START WITH 1, INCREMENT BY 1),  
	Shortcut varchar(255) NOT NULL,
	FullName varchar(255) NOT NULL,
	PRIMARY KEY (DepartmentID)
);

CREATE TABLE Student
(
	StudentID int NOT NULL
            GENERATED ALWAYS AS IDENTITY
            (START WITH 1, INCREMENT BY 1),  
	FirstName varchar(255) NOT NULL,
	LastName varchar(255) NOT NULL,
        DepartmentID int NOT NULL,
	PRIMARY KEY (StudentID),
        FOREIGN KEY (DepartmentID) 
                        REFERENCES Department(DepartmentID)
);