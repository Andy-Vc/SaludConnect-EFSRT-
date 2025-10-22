-- ============================================
-- SCRIPT DE INSERTS DE DATOS
-- BD_SALUDCONNECT
-- ============================================

USE BD_SALUDCONNECT;
GO

SET DATEFORMAT DMY;
GO

-- ============================================
-- INSERTS: TB_RELATIONSHIP
-- ============================================
INSERT INTO TB_RELATIONSHIP (DESCRIPTION_RELATIONSHIP) VALUES
('Esposo(a)'),
('Conviviente'),
('Hermano(a)'),
('Tio(a)'),
('Familiar');
GO

-- ============================================
-- INSERTS: TB_EMERGENCY_CONTACT
-- ============================================
INSERT INTO TB_EMERGENCY_CONTACT (NAMES_CONTACT, LAST_NAME_PAT, LAST_NAME_MAT, ID_RELATIONSHIP, PHONE_EMERGENCY) VALUES
('Juan Miguel', 'Casas', 'Villanueva', 1, '928878787'),
('Sin Contacto', '', '', 5, ''),
('María Elena', 'Torres', 'Vega', 2, '945123456'),
('Pedro Luis', 'Gómez', 'Castro', 3, '987654321'),
('Rosa María', 'Fernández', 'Díaz', 1, '912345678'),
('Carlos Alberto', 'Mendoza', 'Ruiz', 4, '998877665');
GO

-- ============================================
-- INSERTS: TB_ROLES
-- ============================================
INSERT INTO TB_ROLES (NAME_ROLE) VALUES
('Paciente'),
('Administrador'),
('Doctor');
GO

-- ============================================
-- INSERTS: TB_SPECIALTIES
-- ============================================
INSERT INTO TB_SPECIALTIES (NAME_SPECIALTY, DESCRIPTION_SPECIALITY) VALUES
('Cardiología', 'Atiende enfermedades del corazón y sistema circulatorio.'),
('Dermatología', 'Tratamiento de enfermedades y cuidado de la piel.'),
('Neurología', 'Diagnóstico y tratamiento de trastornos del sistema nervioso.'),
('Pediatría', 'Atención médica integral para niños y adolescentes.'),
('Ortopedía', 'Trata lesiones y deformidades del sistema músculo-esquelético.'),
('Oftalmología', 'Previene y trata enfermedades de los ojos y la visión.'),
('Psiquiatría', 'Atiende trastornos mentales y del comportamiento.'),
('Medicina Interna', 'Diagnóstico y manejo de enfermedades en adultos.');
GO

-- ============================================
-- INSERTS: TB_CONSULTORIES
-- ============================================
INSERT INTO TB_CONSULTORIES (ID_SPECIALTY, NUMBER_CONSULTORIES, FLOOR_NUMBER) VALUES
-- Cardiología (ID: 1)
(1, 'C-301', 3),
(1, 'C-302', 3),
(1, 'C-303', 3),
-- Dermatología (ID: 2)
(2, 'D-101', 1),
(2, 'D-102', 1),
(2, 'D-103', 1),
-- Neurología (ID: 3)
(3, 'N-201', 2),
(3, 'N-202', 2),
-- Pediatría (ID: 4)
(4, 'P-104', 1),
(4, 'P-105', 1),
-- Ortopedía (ID: 5)
(5, 'O-203', 2),
(5, 'O-204', 2),
-- Oftalmología (ID: 6)
(6, 'OF-106', 1),
(6, 'OF-107', 1),
-- Psiquiatría (ID: 7)
(7, 'PS-304', 3),
(7, 'PS-305', 3),
-- Medicina Interna (ID: 8)
(8, 'M-305', 3),
(8, 'M-306', 3);
GO

-- ============================================
-- INSERTS: TB_USERS (Admin y Pacientes)
-- ============================================
INSERT INTO TB_USERS (FIRST_NAME, LAST_NAME_PAT, LAST_NAME_MAT, DOCUMENT, BIRTHDATE, PHONE, GENDER, EMAIL, PASSWORD_HASH, ID_ROLE, ID_E_CONTACT, DATE_REGISTER, PROFILE_PICTURE)
VALUES
-- Admin
('Admin', 'Principal', 'Uno', '123456723', '10/05/1990', '912345890', 'M', 'admin@example.com', 'clave123', 2, 2, '10/10/2024 10:00:00', NULL),
-- Pacientes
('Juan', 'Pérez', 'González', '123456789', '10/05/1990', '923457890', 'M', 'juan.perez@example.com', 'clave123', 1, 1, '15/01/2025 14:30:00', NULL),
('Ana', 'García', 'Díaz', '111222333', '01/01/2000', '900111222', 'F', 'ana.garcia@example.com', 'clave123', 1, 2, '20/02/2025 17:00:00', NULL),
('Luis', 'Martínez', 'Flores', '98898989', '10/05/1995', '912345290', 'M', 'luis.martinez@example.com', 'clave123', 1, 3, '10/10/2024 10:00:00', NULL),
('Carmen', 'Soto', 'Ramos', '555666777', '15/07/1988', '965432178', 'F', 'carmen.soto@example.com', 'clave123', 1, 4, '05/03/2025 11:20:00', NULL),
('Roberto', 'Navarro', 'Cruz', '888999000', '22/11/1992', '977889900', 'M', 'roberto.navarro@example.com', 'clave123', 1, 5, '12/02/2025 09:45:00', NULL);
GO

-- ============================================
-- INSERTS: TB_USERS (Doctores - 2 por especialidad)
-- ============================================
INSERT INTO TB_USERS (FIRST_NAME, LAST_NAME_PAT, LAST_NAME_MAT, DOCUMENT, BIRTHDATE, PHONE, GENDER, EMAIL, PASSWORD_HASH, ID_ROLE, ID_E_CONTACT, DATE_REGISTER, PROFILE_PICTURE)
VALUES
-- Cardiología (2 doctores)
('María', 'López', 'Hernández', '987654321', '25/08/1985', '987654322', 'F', 'maria.lopez@example.com', 'clave123', 3, 2, '01/12/2024 08:00:00', NULL),
('Jorge', 'Vega', 'Morales', '741852963', '12/03/1983', '945678123', 'M', 'jorge.vega@example.com', 'clave123', 3, 6, '15/11/2024 09:30:00', NULL),

-- Dermatología (2 doctores)
('Carlos', 'Ramírez', 'Sánchez', '456789123', '15/12/1992', '987654323', 'M', 'carlos.ramirez@example.com', 'clave123', 3, 2, '05/12/2024 09:15:00', NULL),
('Patricia', 'Torres', 'Campos', '369258147', '20/06/1990', '912567834', 'F', 'patricia.torres@example.com', 'clave123', 3, 3, '10/01/2025 10:00:00', NULL),

-- Neurología (2 doctores)
('Ricardo', 'Mendoza', 'Luna', '159357486', '08/09/1987', '998765432', 'M', 'ricardo.mendoza@example.com', 'clave123', 3, 4, '20/12/2024 11:45:00', NULL),
('Sofía', 'Paredes', 'Ríos', '753951456', '14/04/1989', '956781234', 'F', 'sofia.paredes@example.com', 'clave123', 3, 5, '18/01/2025 08:30:00', NULL),

-- Pediatría (2 doctores)
('Elena', 'Castro', 'Gutiérrez', '852963741', '30/01/1991', '923456789', 'F', 'elena.castro@example.com', 'clave123', 3, 6, '22/11/2024 14:00:00', NULL),
('Diego', 'Rojas', 'Salazar', '321654987', '17/07/1986', '934567890', 'M', 'diego.rojas@example.com', 'clave123', 3, 2, '28/12/2024 15:20:00', NULL),

-- Ortopedía (2 doctores)
('Fernando', 'Díaz', 'Rojas', '654321987', '03/03/1980', '933444555', 'M', 'fernando.diaz@example.com', 'clave123', 3, 3, '15/12/2024 10:00:00', NULL),
('Valentina', 'Herrera', 'Ponce', '147258369', '25/10/1988', '967890123', 'F', 'valentina.herrera@example.com', 'clave123', 3, 4, '05/01/2025 09:00:00', NULL),

-- Oftalmología (2 doctores)
('Andrés', 'Silva', 'Vargas', '963852741', '19/05/1984', '978901234', 'M', 'andres.silva@example.com', 'clave123', 3, 5, '08/12/2024 13:00:00', NULL),
('Lucía', 'Flores', 'Núñez', '258369147', '11/12/1993', '989012345', 'F', 'lucia.flores@example.com', 'clave123', 3, 6, '16/01/2025 10:30:00', NULL),

-- Psiquiatría (2 doctores)
('Gabriel', 'Morales', 'Chávez', '789456123', '07/02/1982', '956123789', 'M', 'gabriel.morales@example.com', 'clave123', 3, 2, '12/12/2024 11:00:00', NULL),
('Daniela', 'Reyes', 'Carrillo', '654789321', '28/08/1990', '967234890', 'F', 'daniela.reyes@example.com', 'clave123', 3, 3, '19/01/2025 14:45:00', NULL),

-- Medicina Interna (2 doctores)
('Alberto', 'Ortiz', 'Guzmán', '456123789', '16/11/1985', '945678901', 'M', 'alberto.ortiz@example.com', 'clave123', 3, 4, '02/12/2024 08:15:00', NULL),
('Isabel', 'Campos', 'Montoya', '321987654', '23/04/1991', '978345612', 'F', 'isabel.campos@example.com', 'clave123', 3, 5, '14/01/2025 09:30:00', NULL);
GO

-- ============================================
-- INSERTS: TB_DOCTOR_SPECIALTIES
-- Cada doctor tiene UNA sola especialidad
-- ============================================
INSERT INTO TB_DOCTOR_SPECIALTIES (ID_DOCTOR, ID_SPECIALTY, YEARS_EXPERIENCE, EXPERIENCE, DOC_LANGUAGES) VALUES
-- Cardiología (ID: 1)
(7, 1, 12, 'Especialista en enfermedades cardiovasculares con amplia experiencia en diagnóstico avanzado.', 'Español, Inglés'),
(8, 1, 8, 'Cardiólogo con enfoque en arritmias y electrofisiología cardíaca.', 'Español, Portugués'),

-- Dermatología (ID: 2)
(9, 2, 7, 'Dermatólogo especializado en tratamientos estéticos y dermatología clínica.', 'Español, Inglés, Francés'),
(10, 2, 5, 'Experta en enfermedades de la piel y procedimientos láser.', 'Español, Inglés'),

-- Neurología (ID: 3)
(11, 3, 10, 'Neurólogo con especialización en trastornos del movimiento y epilepsia.', 'Español, Inglés, Alemán'),
(12, 3, 6, 'Especialista en cefaleas, migrañas y neurología pediátrica.', 'Español, Inglés'),

-- Pediatría (ID: 4)
(13, 4, 9, 'Pediatra con experiencia en atención integral del niño y adolescente.', 'Español, Inglés'),
(14, 4, 11, 'Especialista en neonatología y cuidados intensivos pediátricos.', 'Español, Inglés, Italiano'),

-- Ortopedía (ID: 5)
(15, 5, 15, 'Traumatólogo especializado en cirugía de columna y reemplazos articulares.', 'Español, Inglés'),
(16, 5, 8, 'Ortopedista con enfoque en medicina deportiva y lesiones traumáticas.', 'Español, Inglés, Portugués'),

-- Oftalmología (ID: 6)
(17, 6, 13, 'Oftalmólogo especializado en cirugía refractiva y cataratas.', 'Español, Inglés'),
(18, 6, 7, 'Experta en enfermedades de la retina y glaucoma.', 'Español, Francés'),

-- Psiquiatría (ID: 7)
(19, 7, 14, 'Psiquiatra con especialización en trastornos del ánimo y ansiedad.', 'Español, Inglés'),
(20, 7, 9, 'Especialista en psiquiatría infantil y del adolescente.', 'Español, Inglés, Portugués'),

-- Medicina Interna (ID: 8)
(21, 8, 11, 'Internista con amplia experiencia en enfermedades crónicas y diabetes.', 'Español, Inglés'),
(22, 8, 8, 'Especialista en medicina interna con enfoque en enfermedades infecciosas.', 'Español, Inglés, Italiano');
GO

-- ============================================
-- INSERTS: TB_DOCTOR_SCHEDULES
-- Horarios diarios para los próximos 30 días
-- ============================================

-- CARDIOLOGÍA - Dra. María López (ID: 7)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(7, 1, '21/10/2025 08:00:00', '21/10/2025 12:00:00', 20),
(7, 1, '22/10/2025 08:00:00', '22/10/2025 12:00:00', 20),
(7, 1, '23/10/2025 14:00:00', '23/10/2025 18:00:00', 20),
(7, 1, '24/10/2025 08:00:00', '24/10/2025 12:00:00', 20),
(7, 1, '27/10/2025 08:00:00', '27/10/2025 12:00:00', 20),
(7, 1, '28/10/2025 14:00:00', '28/10/2025 18:00:00', 20),
(7, 1, '29/10/2025 08:00:00', '29/10/2025 12:00:00', 20),
(7, 1, '30/10/2025 08:00:00', '30/10/2025 12:00:00', 20),
(7, 1, '31/10/2025 14:00:00', '31/10/2025 18:00:00', 20);

-- CARDIOLOGÍA - Dr. Jorge Vega (ID: 8)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(8, 2, '21/10/2025 14:00:00', '21/10/2025 18:00:00', 20),
(8, 2, '22/10/2025 14:00:00', '22/10/2025 18:00:00', 20),
(8, 2, '23/10/2025 08:00:00', '23/10/2025 12:00:00', 20),
(8, 2, '24/10/2025 14:00:00', '24/10/2025 18:00:00', 20),
(8, 2, '25/10/2025 08:00:00', '25/10/2025 12:00:00', 20),
(8, 2, '28/10/2025 08:00:00', '28/10/2025 12:00:00', 20),
(8, 2, '29/10/2025 14:00:00', '29/10/2025 18:00:00', 20),
(8, 2, '30/10/2025 14:00:00', '30/10/2025 18:00:00', 20);

-- DERMATOLOGÍA - Dr. Carlos Ramírez (ID: 9)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(9, 4, '21/10/2025 09:00:00', '21/10/2025 13:00:00', 20),
(9, 4, '22/10/2025 09:00:00', '22/10/2025 13:00:00', 20),
(9, 4, '23/10/2025 15:00:00', '23/10/2025 19:00:00', 20),
(9, 4, '24/10/2025 09:00:00', '24/10/2025 13:00:00', 20),
(9, 4, '25/10/2025 09:00:00', '25/10/2025 13:00:00', 20),
(9, 4, '28/10/2025 09:00:00', '28/10/2025 13:00:00', 20),
(9, 4, '29/10/2025 15:00:00', '29/10/2025 19:00:00', 20),
(9, 4, '30/10/2025 09:00:00', '30/10/2025 13:00:00', 20);

-- DERMATOLOGÍA - Dra. Patricia Torres (ID: 10)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(10, 5, '21/10/2025 10:00:00', '21/10/2025 14:00:00', 20),
(10, 5, '22/10/2025 15:00:00', '22/10/2025 19:00:00', 20),
(10, 5, '23/10/2025 10:00:00', '23/10/2025 14:00:00', 20),
(10, 5, '24/10/2025 15:00:00', '24/10/2025 19:00:00', 20),
(10, 5, '25/10/2025 10:00:00', '25/10/2025 14:00:00', 20),
(10, 5, '28/10/2025 15:00:00', '28/10/2025 19:00:00', 20),
(10, 5, '29/10/2025 10:00:00', '29/10/2025 14:00:00', 20),
(10, 5, '30/10/2025 15:00:00', '30/10/2025 19:00:00', 20);

-- NEUROLOGÍA - Dr. Ricardo Mendoza (ID: 11)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(11, 7, '21/10/2025 08:00:00', '21/10/2025 12:00:00', 20),
(11, 7, '22/10/2025 08:00:00', '22/10/2025 12:00:00', 20),
(11, 7, '23/10/2025 13:00:00', '23/10/2025 17:00:00', 20),
(11, 7, '24/10/2025 08:00:00', '24/10/2025 12:00:00', 20),
(11, 7, '25/10/2025 08:00:00', '25/10/2025 12:00:00', 20),
(11, 7, '28/10/2025 13:00:00', '28/10/2025 17:00:00', 20),
(11, 7, '29/10/2025 08:00:00', '29/10/2025 12:00:00', 20);

-- NEUROLOGÍA - Dra. Sofía Paredes (ID: 12)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(12, 8, '21/10/2025 14:00:00', '21/10/2025 18:00:00', 20),
(12, 8, '22/10/2025 14:00:00', '22/10/2025 18:00:00', 20),
(12, 8, '23/10/2025 09:00:00', '23/10/2025 13:00:00', 20),
(12, 8, '24/10/2025 14:00:00', '24/10/2025 18:00:00', 20),
(12, 8, '25/10/2025 14:00:00', '25/10/2025 18:00:00', 20),
(12, 8, '28/10/2025 09:00:00', '28/10/2025 13:00:00', 20),
(12, 8, '29/10/2025 14:00:00', '29/10/2025 18:00:00', 20);

-- PEDIATRÍA - Dra. Elena Castro (ID: 13)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(13, 9, '21/10/2025 08:00:00', '21/10/2025 12:00:00', 20),
(13, 9, '22/10/2025 08:00:00', '22/10/2025 12:00:00', 20),
(13, 9, '23/10/2025 14:00:00', '23/10/2025 18:00:00', 20),
(13, 9, '24/10/2025 08:00:00', '24/10/2025 12:00:00', 20),
(13, 9, '25/10/2025 08:00:00', '25/10/2025 12:00:00', 20),
(13, 9, '28/10/2025 14:00:00', '28/10/2025 18:00:00', 20),
(13, 9, '29/10/2025 08:00:00', '29/10/2025 12:00:00', 20),
(13, 9, '30/10/2025 08:00:00', '30/10/2025 12:00:00', 20);

-- PEDIATRÍA - Dr. Diego Rojas (ID: 14)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(14, 10, '21/10/2025 13:00:00', '21/10/2025 17:00:00', 20),
(14, 10, '22/10/2025 13:00:00', '22/10/2025 17:00:00', 20),
(14, 10, '23/10/2025 09:00:00', '23/10/2025 13:00:00', 20),
(14, 10, '24/10/2025 13:00:00', '24/10/2025 17:00:00', 20),
(14, 10, '25/10/2025 13:00:00', '25/10/2025 17:00:00', 20),
(14, 10, '28/10/2025 09:00:00', '28/10/2025 13:00:00', 20),
(14, 10, '29/10/2025 13:00:00', '29/10/2025 17:00:00', 20),
(14, 10, '30/10/2025 13:00:00', '30/10/2025 17:00:00', 20);

-- ORTOPEDÍA - Dr. Fernando Díaz (ID: 15)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(15, 11, '21/10/2025 08:00:00', '21/10/2025 12:00:00', 20),
(15, 11, '22/10/2025 14:00:00', '22/10/2025 18:00:00', 20),
(15, 11, '23/10/2025 08:00:00', '23/10/2025 12:00:00', 20),
(15, 11, '24/10/2025 14:00:00', '24/10/2025 18:00:00', 20),
(15, 11, '25/10/2025 08:00:00', '25/10/2025 12:00:00', 20),
(15, 11, '28/10/2025 14:00:00', '28/10/2025 18:00:00', 20),
(15, 11, '29/10/2025 08:00:00', '29/10/2025 12:00:00', 20),
(15, 11, '30/10/2025 14:00:00', '30/10/2025 18:00:00', 20);

-- ORTOPEDÍA - Dra. Valentina Herrera (ID: 16)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(16, 12, '21/10/2025 09:00:00', '21/10/2025 13:00:00', 20),
(16, 12, '22/10/2025 09:00:00', '22/10/2025 13:00:00', 20),
(16, 12, '23/10/2025 15:00:00', '23/10/2025 19:00:00', 20),
(16, 12, '24/10/2025 09:00:00', '24/10/2025 13:00:00', 20),
(16, 12, '25/10/2025 09:00:00', '25/10/2025 13:00:00', 20),
(16, 12, '28/10/2025 15:00:00', '28/10/2025 19:00:00', 20),
(16, 12, '29/10/2025 09:00:00', '29/10/2025 13:00:00', 20),
(16, 12, '30/10/2025 09:00:00', '30/10/2025 13:00:00', 20);

-- OFTALMOLOGÍA - Dr. Andrés Silva (ID: 17)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(17, 13, '21/10/2025 08:00:00', '21/10/2025 12:00:00', 20),
(17, 13, '22/10/2025 08:00:00', '22/10/2025 12:00:00', 20),
(17, 13, '23/10/2025 14:00:00', '23/10/2025 18:00:00', 20),
(17, 13, '24/10/2025 08:00:00', '24/10/2025 12:00:00', 20),
(17, 13, '25/10/2025 08:00:00', '25/10/2025 12:00:00', 20),
(17, 13, '28/10/2025 14:00:00', '28/10/2025 18:00:00', 20),
(17, 13, '29/10/2025 08:00:00', '29/10/2025 12:00:00', 20),
(17, 13, '30/10/2025 08:00:00', '30/10/2025 12:00:00', 20);

-- OFTALMOLOGÍA - Dra. Lucía Flores (ID: 18)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(18, 14, '21/10/2025 14:00:00', '21/10/2025 18:00:00', 20),
(18, 14, '22/10/2025 14:00:00', '22/10/2025 18:00:00', 20),
(18, 14, '23/10/2025 09:00:00', '23/10/2025 13:00:00', 20),
(18, 14, '24/10/2025 14:00:00', '24/10/2025 18:00:00', 20),
(18, 14, '25/10/2025 14:00:00', '25/10/2025 18:00:00', 20),
(18, 14, '28/10/2025 09:00:00', '28/10/2025 13:00:00', 20),
(18, 14, '29/10/2025 14:00:00', '29/10/2025 18:00:00', 20),
(18, 14, '30/10/2025 14:00:00', '30/10/2025 18:00:00', 20);

-- PSIQUIATRÍA - Dr. Gabriel Morales (ID: 19)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(19, 15, '21/10/2025 10:00:00', '21/10/2025 14:00:00', 20),
(19, 15, '22/10/2025 10:00:00', '22/10/2025 14:00:00', 20),
(19, 15, '23/10/2025 16:00:00', '23/10/2025 20:00:00', 20),
(19, 15, '24/10/2025 10:00:00', '24/10/2025 14:00:00', 20),
(19, 15, '25/10/2025 10:00:00', '25/10/2025 14:00:00', 20),
(19, 15, '28/10/2025 16:00:00', '28/10/2025 20:00:00', 20),
(19, 15, '29/10/2025 10:00:00', '29/10/2025 14:00:00', 20),
(19, 15, '30/10/2025 10:00:00', '30/10/2025 14:00:00', 20);

-- PSIQUIATRÍA - Dra. Daniela Reyes (ID: 20)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(20, 16, '21/10/2025 15:00:00', '21/10/2025 19:00:00', 20),
(20, 16, '22/10/2025 15:00:00', '22/10/2025 19:00:00', 20),
(20, 16, '23/10/2025 10:00:00', '23/10/2025 14:00:00', 20),
(20, 16, '24/10/2025 15:00:00', '24/10/2025 19:00:00', 20),
(20, 16, '25/10/2025 15:00:00', '25/10/2025 19:00:00', 20),
(20, 16, '28/10/2025 10:00:00', '28/10/2025 14:00:00', 20),
(20, 16, '29/10/2025 15:00:00', '29/10/2025 19:00:00', 20),
(20, 16, '30/10/2025 15:00:00', '30/10/2025 19:00:00', 20);

-- MEDICINA INTERNA - Dr. Alberto Ortiz (ID: 21)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(21, 17, '21/10/2025 08:00:00', '21/10/2025 12:00:00', 20),
(21, 17, '22/10/2025 08:00:00', '22/10/2025 12:00:00', 20),
(21, 17, '23/10/2025 14:00:00', '23/10/2025 18:00:00', 20),
(21, 17, '24/10/2025 08:00:00', '24/10/2025 12:00:00', 20),
(21, 17, '25/10/2025 08:00:00', '25/10/2025 12:00:00', 20),
(21, 17, '28/10/2025 14:00:00', '28/10/2025 18:00:00', 20),
(21, 17, '29/10/2025 08:00:00', '29/10/2025 12:00:00', 20),
(21, 17, '30/10/2025 08:00:00', '30/10/2025 12:00:00', 20);

-- MEDICINA INTERNA - Dra. Isabel Campos (ID: 22)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(22, 18, '21/10/2025 13:00:00', '21/10/2025 17:00:00', 20),
(22, 18, '22/10/2025 13:00:00', '22/10/2025 17:00:00', 20),
(22, 18, '23/10/2025 09:00:00', '23/10/2025 13:00:00', 20),
(22, 18, '24/10/2025 13:00:00', '24/10/2025 17:00:00', 20),
(22, 18, '25/10/2025 13:00:00', '25/10/2025 17:00:00', 20),
(22, 18, '28/10/2025 09:00:00', '28/10/2025 13:00:00', 20),
(22, 18, '29/10/2025 13:00:00', '29/10/2025 17:00:00', 20),
(22, 18, '30/10/2025 13:00:00', '30/10/2025 17:00:00', 20);

-- Horarios adicionales (próximos días hasta 30 días)
-- CARDIOLOGÍA - Dra. María López (ID: 7)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(7, 1, '03/11/2025 08:00:00', '03/11/2025 12:00:00', 20),
(7, 1, '04/11/2025 14:00:00', '04/11/2025 18:00:00', 20),
(7, 1, '05/11/2025 08:00:00', '05/11/2025 12:00:00', 20),
(7, 1, '06/11/2025 08:00:00', '06/11/2025 12:00:00', 20),
(7, 1, '07/11/2025 14:00:00', '07/11/2025 18:00:00', 20),
(7, 1, '10/11/2025 08:00:00', '10/11/2025 12:00:00', 20),
(7, 1, '11/11/2025 08:00:00', '11/11/2025 12:00:00', 20),
(7, 1, '12/11/2025 14:00:00', '12/11/2025 18:00:00', 20),
(7, 1, '13/11/2025 08:00:00', '13/11/2025 12:00:00', 20),
(7, 1, '14/11/2025 08:00:00', '14/11/2025 12:00:00', 20);

-- DERMATOLOGÍA - Dr. Carlos Ramírez (ID: 9)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(9, 4, '31/10/2025 09:00:00', '31/10/2025 13:00:00', 20),
(9, 4, '01/11/2025 09:00:00', '01/11/2025 13:00:00', 20),
(9, 4, '04/11/2025 15:00:00', '04/11/2025 19:00:00', 20),
(9, 4, '05/11/2025 09:00:00', '05/11/2025 13:00:00', 20),
(9, 4, '06/11/2025 09:00:00', '06/11/2025 13:00:00', 20),
(9, 4, '07/11/2025 15:00:00', '07/11/2025 19:00:00', 20),
(9, 4, '08/11/2025 09:00:00', '08/11/2025 13:00:00', 20),
(9, 4, '11/11/2025 09:00:00', '11/11/2025 13:00:00', 20),
(9, 4, '12/11/2025 15:00:00', '12/11/2025 19:00:00', 20);

-- PEDIATRÍA - Dra. Elena Castro (ID: 13)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(13, 9, '31/10/2025 08:00:00', '31/10/2025 12:00:00', 20),
(13, 9, '01/11/2025 08:00:00', '01/11/2025 12:00:00', 20),
(13, 9, '04/11/2025 14:00:00', '04/11/2025 18:00:00', 20),
(13, 9, '05/11/2025 08:00:00', '05/11/2025 12:00:00', 20),
(13, 9, '06/11/2025 08:00:00', '06/11/2025 12:00:00', 20),
(13, 9, '07/11/2025 14:00:00', '07/11/2025 18:00:00', 20),
(13, 9, '10/11/2025 08:00:00', '10/11/2025 12:00:00', 20),
(13, 9, '11/11/2025 08:00:00', '11/11/2025 12:00:00', 20);

-- ORTOPEDÍA - Dr. Fernando Díaz (ID: 15)
INSERT INTO TB_DOCTOR_SCHEDULES (ID_DOCTOR, ID_CONSULTORIES, FECHA_INICIO, FECHA_FIN, DURACION_CITA) VALUES
(15, 11, '31/10/2025 08:00:00', '31/10/2025 12:00:00', 20),
(15, 11, '01/11/2025 14:00:00', '01/11/2025 18:00:00', 20),
(15, 11, '04/11/2025 08:00:00', '04/11/2025 12:00:00', 20),
(15, 11, '05/11/2025 14:00:00', '05/11/2025 18:00:00', 20),
(15, 11, '06/11/2025 08:00:00', '06/11/2025 12:00:00', 20),
(15, 11, '07/11/2025 14:00:00', '07/11/2025 18:00:00', 20),
(15, 11, '08/11/2025 08:00:00', '08/11/2025 12:00:00', 20);
GO

-- ============================================
-- INSERTS: TB_APPOINTMENTS
-- Al menos 1 cita por doctor
-- ============================================
INSERT INTO TB_APPOINTMENTS (ID_PATIENT, ID_DOCTOR, ID_SPECIALTY, DATE_APPOINTMENT, STATE, APPOINTMENT_PRICE, ID_CONSULTORIES) VALUES
-- Cardiología
(2, 7, 1, '21/10/2025 08:00:00', 'P', 150.00, 1),
(3, 7, 1, '21/10/2025 08:20:00', 'P', 150.00, 1),
(4, 8, 1, '21/10/2025 14:00:00', 'P', 150.00, 2),
(5, 8, 1, '21/10/2025 14:20:00', 'P', 150.00, 2),

-- Dermatología
(2, 9, 2, '21/10/2025 09:00:00', 'P', 120.00, 4),
(3, 9, 2, '21/10/2025 09:20:00', 'P', 120.00, 4),
(4, 10, 2, '21/10/2025 10:00:00', 'P', 120.00, 5),
(5, 10, 2, '21/10/2025 10:20:00', 'P', 120.00, 5),

-- Neurología
(2, 11, 3, '21/10/2025 08:00:00', 'P', 130.00, 7),
(3, 11, 3, '21/10/2025 08:20:00', 'P', 130.00, 7),
(4, 12, 3, '21/10/2025 14:00:00', 'P', 130.00, 8),
(5, 12, 3, '21/10/2025 14:20:00', 'P', 130.00, 8),

-- Pediatría
(2, 13, 4, '21/10/2025 08:00:00', 'P', 110.00, 9),
(3, 13, 4, '21/10/2025 08:20:00', 'P', 110.00, 9),
(4, 14, 4, '21/10/2025 13:00:00', 'P', 110.00, 10),
(5, 14, 4, '21/10/2025 13:20:00', 'P', 110.00, 10),

-- Ortopedía
(2, 15, 5, '21/10/2025 08:00:00', 'P', 140.00, 11),
(3, 15, 5, '21/10/2025 08:20:00', 'P', 140.00, 11),
(4, 16, 5, '21/10/2025 09:00:00', 'P', 140.00, 12),
(5, 16, 5, '21/10/2025 09:20:00', 'P', 140.00, 12),

-- Oftalmología
(2, 17, 6, '21/10/2025 08:00:00', 'P', 125.00, 13),
(3, 17, 6, '21/10/2025 08:20:00', 'P', 125.00, 13),
(4, 18, 6, '21/10/2025 14:00:00', 'P', 125.00, 14),
(5, 18, 6, '21/10/2025 14:20:00', 'P', 125.00, 14),

-- Psiquiatría
(2, 19, 7, '21/10/2025 10:00:00', 'P', 160.00, 15),
(3, 19, 7, '21/10/2025 10:20:00', 'P', 160.00, 15),
(4, 20, 7, '21/10/2025 15:00:00', 'P', 160.00, 16),
(5, 20, 7, '21/10/2025 15:20:00', 'P', 160.00, 16),

-- Medicina Interna
(2, 21, 8, '21/10/2025 08:00:00', 'P', 140.00, 17),
(3, 21, 8, '21/10/2025 08:20:00', 'P', 140.00, 17),
(4, 22, 8, '21/10/2025 13:00:00', 'P', 140.00, 18),
(5, 22, 8, '21/10/2025 13:20:00', 'A', 140.00, 18);
GO

-- ============================================
-- INSERTS: TB_SERVICES
-- ============================================
INSERT INTO TB_SERVICES (NAME_SERVICE, DESCRIPTION, PRICE, DURATION_MINUTES, ID_SPECIALTY) VALUES
-- Cardiología
('Consulta Cardiología', 'Evaluación integral del sistema cardiovascular.', 150.00, 30, 1), 
('Electrocardiograma', 'Prueba diagnóstica de actividad eléctrica del corazón.', 80.00, 15, 1),
('Ecocardiograma', 'Ultrasonido del corazón para evaluar estructura y función.', 200.00, 45, 1),
('Holter 24 horas', 'Monitoreo continuo del ritmo cardíaco.', 250.00, 30, 1),

-- Dermatología
('Consulta Dermatología', 'Diagnóstico y tratamiento de enfermedades de la piel.', 120.00, 25, 2), 
('Biopsia de Piel', 'Toma de muestra de tejido cutáneo.', 250.00, 45, 2),
('Crioterapia', 'Tratamiento con frío para lesiones cutáneas.', 180.00, 30, 2),
('Dermatoscopia', 'Examen detallado de lesiones de piel.', 100.00, 20, 2),

-- Neurología
('Consulta Neurología', 'Evaluación de trastornos del sistema nervioso.', 130.00, 30, 3),
('Electroencefalograma', 'Registro de actividad eléctrica cerebral.', 220.00, 60, 3),
('Electromiografía', 'Evaluación de músculos y nervios periféricos.', 280.00, 45, 3),

-- Pediatría
('Consulta Pediatría', 'Atención médica integral para niños.', 110.00, 25, 4),
('Control de Niño Sano', 'Evaluación del crecimiento y desarrollo.', 90.00, 30, 4),
('Vacunación', 'Aplicación de vacunas según calendario.', 60.00, 15, 4),

-- Ortopedía
('Consulta Ortopedia', 'Evaluación de lesiones músculo-esqueléticas.', 140.00, 30, 5),
('Infiltración', 'Aplicación de medicamento en articulación.', 200.00, 20, 5),
('Radiografía', 'Imagen diagnóstica de huesos y articulaciones.', 120.00, 15, 5),

-- Oftalmología
('Consulta Oftalmología', 'Examen completo de la visión.', 125.00, 30, 6),
('Tonometría', 'Medición de presión intraocular.', 80.00, 15, 6),
('Campimetría', 'Evaluación del campo visual.', 150.00, 30, 6),

-- Psiquiatría
('Consulta Psiquiatría', 'Evaluación y tratamiento de salud mental.', 160.00, 45, 7),
('Psicoterapia Individual', 'Sesión terapéutica personalizada.', 180.00, 60, 7),
('Evaluación Psiquiátrica Integral', 'Valoración completa del estado mental.', 220.00, 90, 7),

-- Medicina Interna
('Consulta Medicina Interna', 'Atención integral de enfermedades en adultos.', 140.00, 30, 8), 
('Prueba de Glucosa en Ayunas', 'Análisis de niveles de azúcar en sangre.', 60.00, 20, 8),
('Perfil Lipídico', 'Análisis de colesterol y triglicéridos.', 80.00, 20, 8),
('Hemograma Completo', 'Análisis detallado de células sanguíneas.', 70.00, 20, 8);
GO

-- ============================================
-- INSERTS: TB_MEDICAL_RECORDS
-- ============================================
INSERT INTO TB_MEDICAL_RECORDS (ID_APPOINTMENT, OBSERVATIONS, DIAGNOSIS, TREATMENT, FOLLOW_UP_DATE) VALUES
(32, 
 'Paciente refiere fatiga crónica. Examen físico sin hallazgos relevantes. Presión arterial normal.', 
 'Síndrome de Fatiga Crónica', 
 'Indicar reposo adecuado, mejorar hábitos de sueño. Vitaminas del complejo B. Control en 2 semanas.', 
 '05/11/2025'
);
GO

-- ============================================
-- INSERTS: TB_ADDITIONAL_SERVICES
-- ============================================
INSERT INTO TB_ADDITIONAL_SERVICES (ID_RECORD, ID_SERVICE, PRICE_AT_TIME, STATE) VALUES
(1, 18, 70.00, 'P'),
(1, 19, 80.00, 'P');
GO


SET DATEFORMAT YMD;

PRINT 'Datos insertados exitosamente en BD_SALUDCONNECT.';
PRINT 'Total de doctores: 16 (2 por cada una de las 8 especialidades)';
PRINT 'Total de pacientes: 5';
PRINT 'Horarios creados para los próximos 30 días';
PRINT 'Citas de ejemplo creadas para cada doctor';
GO

SELECT * FROM TB_APPOINTMENTS ORDER BY 1 DESC
SELECT SCOPE_IDENTITY() AS IdAppointment;
