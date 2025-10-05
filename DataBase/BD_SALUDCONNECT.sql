USE master;
GO

CREATE DATABASE BD_SALUDCONNECT;
GO

USE BD_SALUDCONNECT;
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
<<<<<<< HEAD
('CardiologÌa'),
('DermatologÌa'),
('NeurologÌa'),
('PediatrÌa'),
('OrtopedÌa'),
('OftalmologÌa'),
('PsiquiatrÌa'),
('Medicina Interna');
=======
('Cardiolog√≠a'),
('Dermatolog√≠a'),
('Neurolog√≠a'),
('Pediatr√≠a');
>>>>>>> 567bab0db3d9854871c34331c9d5001c32546de7
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
    FLG_DELETE BIT DEFAULT 0
);
GO

<<<<<<< HEAD
INSERT INTO TB_USERS (FIRST_NAME, LAST_NAME_PAT, LAST_NAME_MAT,DOCUMENT, BIRTHDATE, PHONE, GENDER,EMAIL, PASSWORD_HASH, ID_ROLE)
VALUES
('Admin', 'Principal', '1', '123456723', '1990-05-10', '912345890', 'M', 'admin@example.com', 'clave123', 2),
('Juan', 'PÈrez', 'Gonz·lez', '123456789', '1990-05-10', '923457890', 'M', 'juan.perez@example.com', 'clave123', 1),
('MarÌa', 'LÛpez', 'Hern·ndez', '987654321', '1985-08-25', '987654321', 'F', 'maria.lopez@example.com', 'clave123', 3),
('Carlos', 'RamÌrez', 'S·nchez', '456789123', '1992-12-15', '987654321', 'M', 'carlos.ramirez@example.com', 'clave123', 3);
=======
INSERT INTO TB_USERS (FIRST_NAME, LAST_NAME_MAT, LAST_NAME_PAT, DOCUMENT, EMAIL, PASSWORD_HASH, ID_ROLE) VALUES
('Juan', 'Gonz√°lez', 'P√©rez', '123456789', 'juan.perez@example.com', 'clave123', 1),  -- Paciente
('Ana', 'Mart√≠nez', 'L√≥pez', '987654321', 'ana.lopez@example.com', 'clave123', 2),   -- Administrador
('Carlos', 'S√°nchez', 'Ram√≠rez', '456789123', 'carlos.ramirez@example.com', 'clave123', 3),  -- Doctor
('Laura', 'G√≥mez', 'Fern√°ndez', '789123456', 'laura.fernandez@example.com', 'clave123', 3); -- Doctor
>>>>>>> 567bab0db3d9854871c34331c9d5001c32546de7
GO


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
<<<<<<< HEAD
-- CardiologÌa
('Consulta CardiologÌa', 'EvaluaciÛn integral del sistema cardiovascular, diagnÛstico y manejo de enfermedades cardÌacas.', 150.00, 30, 1),
('Electrocardiograma', 'Prueba diagnÛstica para evaluar la actividad elÈctrica del corazÛn.', 80.00, 15, 1),

-- DermatologÌa
('Consulta DermatologÌa', 'DiagnÛstico y tratamiento de enfermedades de la piel, cabello y uÒas.', 120.00, 25, 2),
('Tratamiento L·ser DermatolÛgico', 'Procedimiento para tratar lesiones cut·neas mediante tecnologÌa l·ser.', 250.00, 45, 2),

-- NeurologÌa
('Consulta NeurologÌa', 'EvaluaciÛn y tratamiento de trastornos del sistema nervioso central y perifÈrico.', 200.00, 40, 3),
('Electroencefalograma', 'Registro de la actividad elÈctrica cerebral para diagnÛstico neurolÛgico.', 120.00, 35, 3),

-- PediatrÌa
('Consulta PediatrÌa', 'AtenciÛn mÈdica integral para bebÈs, niÒos y adolescentes.', 100.00, 20, 4),
('VacunaciÛn Pedi·trica', 'AdministraciÛn de vacunas para prevenciÛn de enfermedades en niÒos.', 50.00, 15, 4),

-- OrtopedÌa
('Consulta OrtopÈdica', 'DiagnÛstico y tratamiento de lesiones y enfermedades del sistema musculoesquelÈtico.', 130.00, 30, 5),
('Terapia FÌsica OrtopÈdica', 'RehabilitaciÛn para lesiones musculares y Ûseas.', 90.00, 40, 5),

-- OftalmologÌa
('Consulta OftalmolÛgica', 'EvaluaciÛn de la salud ocular y correcciÛn de problemas visuales.', 110.00, 25, 6),
('Prueba de Campo Visual', 'EvaluaciÛn de la visiÛn perifÈrica para detectar daÒos oculares.', 70.00, 30, 6),

-- PsiquiatrÌa
('Consulta Psiqui·trica', 'EvaluaciÛn y tratamiento de trastornos mentales y emocionales.', 180.00, 40, 7),
('Terapia Cognitivo-Conductual', 'Tratamiento psicoterapÈutico para trastornos emocionales y de conducta.', 150.00, 50, 7),

-- Medicina Interna
('Consulta Medicina Interna', 'AtenciÛn integral para adultos con enfoque en diagnÛstico y tratamiento de enfermedades internas.', 140.00, 30, 8),
('Pruebas de Laboratorio', 'An·lisis clÌnicos para diagnÛstico y seguimiento de enfermedades.', 60.00, 20, 8);
=======
('Consulta Cardiolog√≠a', 'Evaluaci√≥n integral del sistema cardiovascular, diagn√≥stico y manejo de enfermedades card√≠acas.', 150.00, 30, 1),
('Consulta Dermatolog√≠a', 'Diagn√≥stico y tratamiento de enfermedades de la piel, cabello y u√±as.', 120.00, 25, 2),
('Consulta Neurolog√≠a', 'Evaluaci√≥n y tratamiento de trastornos del sistema nervioso central y perif√©rico.', 200.00, 40, 3),
('Consulta Pediatr√≠a', 'Atenci√≥n m√©dica integral para beb√©s, ni√±os y adolescentes.', 100.00, 20, 4);
>>>>>>> 567bab0db3d9854871c34331c9d5001c32546de7
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
(3, 1, 10),  -- Carlos Ram√≠rez: Cardiolog√≠a, 10 a√±os experiencia
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
    CONSTRAINT CHK_STATE CHECK (STATE IN ('A','P','X','N'))
);
GO

INSERT INTO TB_APPOINTMENTS (ID_PATIENT, ID_DOCTOR, ID_SERVICE, DATE_APPOINTMENT, STATE) VALUES
(1, 3, 1, '2025-10-01 09:00:00', 'A'),
(1, 4, 2, '2025-10-10 14:30:00', 'X');
<<<<<<< HEAD
GO
=======
GO
>>>>>>> 567bab0db3d9854871c34331c9d5001c32546de7
