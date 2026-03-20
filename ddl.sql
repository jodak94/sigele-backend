-- ============================================================
-- DDL - Padrón Electoral
-- ============================================================

-- Tabla de seccionales electorales
CREATE TABLE seccionales (
    codigo_dep   SMALLINT      NOT NULL,
    n_depart     VARCHAR(25),
    codigo_dis   SMALLINT      NOT NULL,
    n_distrito   VARCHAR(30),
    zona         SMALLINT,
    codigo_sec   SMALLINT      NOT NULL,
    descripcio   VARCHAR(30),
    w_seccio     VARCHAR(35),
    direccion    VARCHAR(45),
    CONSTRAINT pk_seccionales PRIMARY KEY (codigo_dep, codigo_dis, codigo_sec)
);

-- Tabla de locales de votación (lugar físico: escuela, colegio, etc.)
CREATE TABLE locales (
    secc_loc     INTEGER       NOT NULL,
    nombre_loc   VARCHAR(80),
    direccion    VARCHAR(80),
    recibido     CHAR(1),
    CONSTRAINT pk_locales PRIMARY KEY (secc_loc)
);

-- Tabla de relación entre seccionales y locales
CREATE TABLE secc_locales (
    codigo_dep   SMALLINT      NOT NULL,
    codigo_dis   SMALLINT      NOT NULL,
    codigo_sec   SMALLINT      NOT NULL,
    codigo_loc   SMALLINT      NOT NULL,
    cod_local    SMALLINT,
    secc_loc     INTEGER       NOT NULL,
    CONSTRAINT pk_secc_locales PRIMARY KEY (codigo_dep, codigo_dis, codigo_sec, codigo_loc),
    CONSTRAINT fk_secc_locales_seccionales FOREIGN KEY (codigo_dep, codigo_dis, codigo_sec)
        REFERENCES seccionales (codigo_dep, codigo_dis, codigo_sec),
    CONSTRAINT fk_secc_locales_locales FOREIGN KEY (secc_loc)
        REFERENCES locales (secc_loc)
);

-- Tabla de electores del padrón electoral
CREATE TABLE electores (
    cod_dpto     SMALLINT,
    cod_dist     SMALLINT,
    sec_ant      SMALLINT,
    codigo_sec   SMALLINT,
    slocal       SMALLINT,
    mesa         SMALLINT,
    orden        SMALLINT,
    numero_ced   INTEGER       NOT NULL,
    apellido     VARCHAR(30),
    nombre       VARCHAR(30),
    direccion    VARCHAR(60),
    fecha_naci   DATE,
    fecha_afil   DATE,
    anio         SMALLINT,
    codigo_sex   SMALLINT,
    sec_loc      INTEGER,
    key_dd       CHAR(4),
    ced_ape_nom  VARCHAR(15),
    CONSTRAINT pk_electores PRIMARY KEY (numero_ced),
    CONSTRAINT fk_electores_locales FOREIGN KEY (sec_loc)
        REFERENCES locales (secc_loc)
);

-- ============================================================
-- Índices (crearlos DESPUÉS de la carga de datos)
-- ============================================================

-- Búsqueda por apellido/nombre
CREATE INDEX idx_electores_apellido  ON electores (apellido);
CREATE INDEX idx_electores_nombre    ON electores (nombre);

-- Búsqueda por ubicación geográfica
CREATE INDEX idx_electores_dpto_dist ON electores (cod_dpto, cod_dist);
CREATE INDEX idx_electores_sec_mesa  ON electores (codigo_sec, mesa);

-- Navegación local → seccional
CREATE INDEX idx_secc_locales_secc_loc ON secc_locales (secc_loc);
