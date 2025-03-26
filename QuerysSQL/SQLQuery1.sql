CREATE DATABASE DB_ControldeActivos

GO

USE DB_ControldeActivos

GO

CREATE TABLE Activos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    FechaAdquisicion DATE NOT NULL,
    Estado NVARCHAR(50) NOT NULL
);


SELECT * FROM Activos


INSERT INTO Activos (Nombre, Descripcion, FechaAdquisicion, Estado)
VALUES
    ('Laptop Dell XPS 15', 'Laptop de alto rendimiento para tareas de diseño', '2023-05-10', 'Activo'),
    ('Impresora HP LaserJet Pro', 'Impresora láser para oficina', '2022-08-15', 'Inactivo'),
    ('Monitor Samsung 24"', 'Pantalla de 24 pulgadas para estaciones de trabajo', '2023-03-01', 'Activo'),
    ('Proyector Epson XGA', 'Proyector de 2000 lúmenes para presentaciones', '2021-11-25', 'En Mantenimiento'),
    ('Mouse Logitech MX Master', 'Mouse ergonómico inalámbrico', '2023-02-10', 'Activo'),
    ('Teclado Mecánico Corsair', 'Teclado mecánico con retroiluminación RGB', '2022-12-05', 'Activo'),
    ('Servidor Dell PowerEdge', 'Servidor para almacenamiento y respaldo de datos', '2021-07-20', 'Activo'),
    ('Cámara Web Logitech C920', 'Cámara de alta definición para videoconferencias', '2023-01-18', 'Activo'),
    ('Router Cisco RV260W', 'Router empresarial con VPN', '2022-05-22', 'Activo'),
    ('Estabilizador de Voltaje APC', 'Estabilizador para proteger equipos eléctricos', '2020-10-30', 'Inactivo'),
    ('Disco Duro Externo Seagate 2TB', 'Disco duro externo para almacenamiento adicional', '2023-03-15', 'Activo'),
    ('Laptop HP Pavilion', 'Laptop de uso general para tareas diarias', '2023-06-01', 'Activo'),
    ('Silla Ergonómica Herman Miller', 'Silla ergonómica para oficina', '2022-09-12', 'Activo'),
    ('Bocinas Bose SoundLink', 'Bocinas inalámbricas para audio de calidad', '2023-01-25', 'Activo'),
    ('Escáner Epson Perfection', 'Escáner para digitalización de documentos y fotos', '2021-02-18', 'En Mantenimiento'),
    ('Cargador Anker 20W', 'Cargador rápido USB-C para dispositivos móviles', '2023-07-05', 'Activo'),
    ('Teléfono IP Cisco', 'Teléfono IP para comunicaciones VoIP', '2022-06-30', 'Activo'),
    ('Cámara de Seguridad Arlo Pro', 'Cámara de seguridad para monitoreo remoto', '2022-11-10', 'Activo'),
    ('Dispositivo de Red Ubiquiti UniFi', 'Dispositivo de red para gestionar tráfico empresarial', '2021-08-25', 'Inactivo'),
    ('Router Netgear Nighthawk', 'Router de alto rendimiento para redes domésticas', '2023-04-12', 'Activo');

