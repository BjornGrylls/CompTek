DROP DATABASE IF EXISTS CompTekBase;
CREATE DATABASE CompTekBase;
USE CompTekBase;


CREATE TABLE Users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name TEXT NOT NULL,
    created DATETIME DEFAULT CURRENT_TIMESTAMP,
    last_modified DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

CREATE TABLE Motor (
    id INT PRIMARY KEY AUTO_INCREMENT,
    HK INT NOT NULL,
    bil INT,
    created DATETIME DEFAULT CURRENT_TIMESTAMP,
    last_modified DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (Bil) REFERENCES Bil(id)
    ON UPDATE CASCADE
    ON DELETE SET NULL
);

CREATE TABLE Dæk (
    id INT PRIMARY KEY AUTO_INCREMENT,
    size INT NOT NULL,
    Bil INT,
    created DATETIME DEFAULT CURRENT_TIMESTAMP,
    last_modified DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (Bil) REFERENCES Bil(id)
    ON UPDATE CASCADE
    ON DELETE SET NULL
);

INSERT INTO Bil (type)
VALUES ("Toyota"), ("Mazda"), ("Ford");

INSERT INTO Motor (HK, bil)
VALUES (5000, 2), (3000, 1);

INSERT INTO Dæk (size, bil)
VALUES (20, 3), (11, 2);


CREATE VIEW bilMedMotor AS
SELECT Bil.type AS Type,
Motor.HK,
Motor.bil AS BilID
FROM Bil
INNER JOIN Motor ON Motor.bil = Bil.id
ORDER BY Motor.HK;

CREATE VIEW bilMedDæk AS
SELECT Bil.type AS Type,
Dæk.size AS Size,
Dæk.bil AS BilID
FROM Bil
INNER Join Dæk ON Dæk.bil = Bil.id
ORDER BY Dæk.size;

SELECT bilMedMotor.type AS Type,
bilMedMotor.HK,
bilMedDæk.size AS Size
FROM bilMedMotor
INNER JOIN bilMedDæk ON bilMedDæk.type = bilMedMotor.type;
