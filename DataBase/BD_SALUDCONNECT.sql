USE master;
GO

CREATE DATABASE BD_SALUDCONNECT;
GO

USE BD_SALUDCONNECT;
GO


CREATE TABLE TB_RELATIONSHIP(
	ID_RELATIONSHIP INT PRIMARY KEY IDENTITY(1,1),
	DESCRIPTION_RELATIONSHIP VARCHAR(20)
)

INSERT INTO TB_RELATIONSHIP (DESCRIPTION_RELATIONSHIP) VALUES
('Esposo(a)'),
('Conviviente'),
('Hermano(a)'),
('Tio(a)'),
('Familiar');
GO



CREATE TABLE TB_EMERGENCY_CONTACT(
	ID_E_CONTACT INT PRIMARY KEY IDENTITY (1,1),
	NAMES_CONTACT VARCHAR(50)	NULL,
	LAST_NAME_PAT VARCHAR(50)  NULL,
    LAST_NAME_MAT VARCHAR(50) NULL,
	ID_RELATIONSHIP INT,
	PHONE_EMERGENCY VARCHAR(9) NULL

	FOREIGN KEY (ID_RELATIONSHIP) REFERENCES TB_RELATIONSHIP(ID_RELATIONSHIP)

)
GO

INSERT INTO TB_EMERGENCY_CONTACT (NAMES_CONTACT,LAST_NAME_PAT,LAST_NAME_MAT,ID_RELATIONSHIP,PHONE_EMERGENCY) VALUES
('Juan Miguel','Casas','Villanueva',1,'928878787'),
('','','',5,'');
GO


-- Tabla Roles
CREATE TABLE TB_ROLES (
    ID_ROLE INT PRIMARY KEY IDENTITY(1,1),
    NAME_ROLE VARCHAR(20) NOT NULL,
    FLG_DELETE BIT DEFAULT 0
);
GO

INSERT INTO TB_ROLES (NAME_ROLE) VALUES
('Paciente'),
('Administrador'),
('Doctor');
GO

-- Tabla Especialidades
CREATE TABLE TB_SPECIALTIES (
    ID_SPECIALTY INT PRIMARY KEY IDENTITY(1,1),
    NAME_SPECIALTY VARCHAR(50) NOT NULL,
    FLG_DELETE BIT DEFAULT 0
);
GO

INSERT INTO TB_SPECIALTIES (NAME_SPECIALTY) VALUES
('Cardiología'),
('Dermatología'),
('Neurología'),
('Pediatría'),
('Ortopedía'),
('Oftalmología'),
('Psiquiatría'),
('Medicina Interna');
GO

-- Tabla Usuarios (pacientes y doctores)
CREATE TABLE TB_USERS (
    ID_USER INT PRIMARY KEY IDENTITY(1,1),
    FIRST_NAME VARCHAR(50) NOT NULL,
    LAST_NAME_PAT VARCHAR(50) NOT NULL,
    LAST_NAME_MAT VARCHAR(50) NOT NULL,
    DOCUMENT VARCHAR(9) NOT NULL UNIQUE,
	BIRTHDATE DATE NOT NULL,
	PHONE VARCHAR(13) NOT NULL,
	GENDER CHAR(1) NOT NULL,
	CONSTRAINT CK_USER_GENDER CHECK (GENDER IN ('M','F')),
    EMAIL VARCHAR(100) NOT NULL UNIQUE,
    PASSWORD_HASH VARCHAR(255) NOT NULL,
    ID_ROLE INT NOT NULL REFERENCES TB_ROLES(ID_ROLE),
	ID_E_CONTACT INT,
    FLG_DELETE BIT DEFAULT 0,

	FOREIGN KEY (ID_E_CONTACT) REFERENCES TB_EMERGENCY_CONTACT(ID_E_CONTACT)
);
GO

INSERT INTO TB_USERS (FIRST_NAME, LAST_NAME_PAT, LAST_NAME_MAT,DOCUMENT, BIRTHDATE, PHONE, GENDER,EMAIL, PASSWORD_HASH, ID_ROLE, ID_E_CONTACT)
VALUES
('Admin', 'Principal', '1', '123456723', '1990-05-10', '912345890', 'M', 'admin@example.com', 'clave123', 2, 2),
('Juan', 'Pérez', 'González', '123456789', '1990-05-10', '923457890', 'M', 'juan.perez@example.com', 'clave123', 1, 1),
('María', 'López', 'Hernández', '987654321', '1985-08-25', '987654321', 'F', 'maria.lopez@example.com', 'clave123', 3 , 2),
('Carlos', 'Ramírez', 'Sánchez', '456789123', '1992-12-15', '987654321', 'M', 'carlos.ramirez@example.com', 'clave123', 3, 2);
GO

SELECT * FROM TB_USERS

-- Tabla Servicios vinculados a una especialidad
CREATE TABLE TB_SERVICES (
    ID_SERVICE INT PRIMARY KEY IDENTITY(1,1),
    NAME_SERVICE VARCHAR(100) NOT NULL,
	DESCRIPTION VARCHAR(500) NULL,
    PRICE DECIMAL(10,2) NOT NULL,
    DURATION_MINUTES INT NOT NULL,
    ID_SPECIALTY INT NOT NULL REFERENCES TB_SPECIALTIES(ID_SPECIALTY),
    FLG_DELETE BIT DEFAULT 0
);
GO

INSERT INTO TB_SERVICES (NAME_SERVICE, DESCRIPTION, PRICE, DURATION_MINUTES, ID_SPECIALTY) VALUES
-- Cardiología
('Consulta Cardiología', 'Evaluación integral del sistema cardiovascular, diagnóstico y manejo de enfermedades cardíacas.', 150.00, 30, 1),
('Electrocardiograma', 'Prueba diagnóstica para evaluar la actividad eléctrica del corazón.', 80.00, 15, 1),

-- Dermatología
('Consulta Dermatología', 'Diagnóstico y tratamiento de enfermedades de la piel, cabello y uñas.', 120.00, 25, 2),
('Tratamiento Láser Dermatológico', 'Procedimiento para tratar lesiones cutáneas mediante tecnología láser.', 250.00, 45, 2),

-- Neurología
('Consulta Neurología', 'Evaluación y tratamiento de trastornos del sistema nervioso central y periférico.', 200.00, 40, 3),
('Electroencefalograma', 'Registro de la actividad eléctrica cerebral para diagnóstico neurológico.', 120.00, 35, 3),

-- Pediatría
('Consulta Pediatría', 'Atención médica integral para bebés, niños y adolescentes.', 100.00, 20, 4),
('Vacunación Pediátrica', 'Administración de vacunas para prevención de enfermedades en niños.', 50.00, 15, 4),

-- Ortopedía
('Consulta Ortopédica', 'Diagnóstico y tratamiento de lesiones y enfermedades del sistema musculoesquelético.', 130.00, 30, 5),
('Terapia Física Ortopédica', 'Rehabilitación para lesiones musculares y óseas.', 90.00, 40, 5),

-- Oftalmología
('Consulta Oftalmológica', 'Evaluación de la salud ocular y corrección de problemas visuales.', 110.00, 25, 6),
('Prueba de Campo Visual', 'Evaluación de la visión periférica para detectar daños oculares.', 70.00, 30, 6),

-- Psiquiatría
('Consulta Psiquiátrica', 'Evaluación y tratamiento de trastornos mentales y emocionales.', 180.00, 40, 7),
('Terapia Cognitivo-Conductual', 'Tratamiento psicoterapéutico para trastornos emocionales y de conducta.', 150.00, 50, 7),

-- Medicina Interna
('Consulta Medicina Interna', 'Atención integral para adultos con enfoque en diagnóstico y tratamiento de enfermedades internas.', 140.00, 30, 8),
('Pruebas de Laboratorio', 'Análisis clínicos para diagnóstico y seguimiento de enfermedades.', 60.00, 20, 8);
GO


-- Tabla para relacionar doctores con sus especialidades y experiencia
CREATE TABLE TB_DOCTOR_SPECIALTIES (
    ID_DOCTOR INT NOT NULL REFERENCES TB_USERS(ID_USER),
    ID_SPECIALTY INT NOT NULL REFERENCES TB_SPECIALTIES(ID_SPECIALTY),
    YEARS_EXPERIENCE INT DEFAULT 0,
    PRIMARY KEY (ID_DOCTOR, ID_SPECIALTY)
);
GO

INSERT INTO TB_DOCTOR_SPECIALTIES (ID_DOCTOR, ID_SPECIALTY, YEARS_EXPERIENCE) VALUES
(3, 1, 10),  -- Carlos Ramírez: Cardiología, 10 años experiencia
(4, 2, 5);
GO

-- Tabla Citas (con servicio y doctores/pacientes)
CREATE TABLE TB_APPOINTMENTS (
    ID_APPOINTMENT INT PRIMARY KEY IDENTITY(1,1),
    ID_PATIENT INT NOT NULL REFERENCES TB_USERS(ID_USER),
    ID_DOCTOR INT NOT NULL REFERENCES TB_USERS(ID_USER),
    ID_SERVICE INT NOT NULL REFERENCES TB_SERVICES(ID_SERVICE),
    DATE_APPOINTMENT DATETIME NOT NULL,  
    STATE CHAR(1) NOT NULL,
    CONSTRAINT CHK_STATE CHECK (STATE IN ('A','P','X','N')) -- A = Completas , P = Pendiente, X = Cancelado , N = Inasistencia


);
GO




INSERT INTO TB_APPOINTMENTS (ID_PATIENT, ID_DOCTOR, ID_SERVICE, DATE_APPOINTMENT, STATE) VALUES
(2, 3, 1, '2025-10-01 09:00:00', 'A'),
(2, 4, 2, '2025-10-10 14:30:00', 'X');
GO
