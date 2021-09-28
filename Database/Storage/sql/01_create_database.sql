-- DROP DATABASE IF EXISTS rapid_scada;

CREATE DATABASE rapid_scada
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8';

COMMENT ON DATABASE rapid_scada
    IS 'http://rapidscada.org';
