la base de datos la llame "biblioteca" y le cree las siguiente tablas:

//tabla usuarios

CREATE TABLE usuarios (
    id INT AUTO_INCREMENT,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    dni VARCHAR(20) NOT NULL UNIQUE,
    telefono VARCHAR(20) DEFAULT NULL,
    email VARCHAR(100) DEFAULT NULL UNIQUE,
    creado_el TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    actualizado_el TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    estado TINYINT DEFAULT 1,
    PRIMARY KEY(id)
);

//tabla libros

CREATE TABLE libros (
    id INT AUTO_INCREMENT not null,
    nombre_libro VARCHAR(100) NOT NULL,
    autor VARCHAR(100) NOT NULL,
    fecha_lanzamiento DATE DEFAULT NULL,
    id_genero INT DEFAULT NULL,
    creado_el TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    actualizado_el TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    estado TINYINT DEFAULT 1,
    INDEX (id_genero),
    PRIMARY KEY (id),
    CONSTRAINT libros_ibfk_1 FOREIGN KEY (id_genero) REFERENCES generos(id)
) ENGINE=InnoDB;

//tabla prestamos

CREATE TABLE prestamos (
    id INT AUTO_INCREMENT not null,
    usuario_id INT DEFAULT NULL,
    libro_id INT DEFAULT NULL,
    fecha_prestamo DATE DEFAULT NULL,
    fecha_devolucion_estimada DATE DEFAULT NULL,
    fecha_devolucion_real DATE DEFAULT NULL,
    INDEX (usuario_id),
    INDEX (libro_id),
    PRIMARY KEY(id),
    CONSTRAINT prestamos_ibfk_1 FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    CONSTRAINT prestamos_ibfk_2 FOREIGN KEY (libro_id) REFERENCES libros(id)
) ENGINE=InnoDB;

//tabla genero

CREATE TABLE generos (
    id INT AUTO_INCREMENT NOT NULL,
    genero VARCHAR(50) NOT NULL,
    PRIMARY KEY(id)
) ENGINE=InnoDB;

