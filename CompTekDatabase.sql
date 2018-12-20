DROP DATABASE IF EXISTS CompTekBase;
CREATE DATABASE CompTekBase;
USE CompTekBase;


CREATE TABLE Users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    username TEXT NOT NULL,
    password TEXT NOT NULL,
    created DATETIME DEFAULT CURRENT_TIMESTAMP,
    last_modified DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

INSERT INTO Users (username, password) VALUES ('admin', 'admin');


SELECT username FROM Users WHERE username = admin;