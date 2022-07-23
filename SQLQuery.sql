CREATE DATABASE ATIVIDADE;
GO

USE ATIVIDADE;
GO

CREATE TABLE Users(
	IdUser				DECIMAL NOT NULL UNIQUE,
	[Name]			    VARCHAR(255) NOT NULL,
	[Email]				VARCHAR(255) NOT NULL,
	[Password]			VARCHAR(255) NOT NULL,
);
GO

INSERT INTO Users(IdUser, [Name], [Email], [Password])
VALUES				 (1, 'admin','admin@email.com', 'admin123');
GO


SELECT * FROM Users;
GO