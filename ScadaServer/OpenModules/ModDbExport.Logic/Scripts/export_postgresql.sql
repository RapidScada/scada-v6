CREATE SCHEMA IF NOT EXISTS mod_db_export
    AUTHORIZATION postgres;

-- Delete a channel data table if it exists
DROP TABLE IF EXISTS mod_db_export.cnl_data;

-- Create a channel data table
CREATE TABLE mod_db_export.cnl_data (
  cnl_num    integer NOT NULL,
  time_stamp timestamp with time zone NOT NULL,
  val        double precision NOT NULL,
  stat       integer NOT NULL,
  PRIMARY KEY (cnl_num, time_stamp)
);

CREATE INDEX ON mod_db_export.cnl_data (time_stamp);

-- Delete an events table if it exists
DROP TABLE IF EXISTS mod_db_export.events;

-- Insert current data
INSERT INTO mod_db_export.cnl_data (time_stamp, cnl_num, val, stat)
VALUES (@timestamp, @cnlNum, @val, @stat)

-- Insert or update historical data
INSERT INTO mod_db_export.cnl_data (time_stamp, cnl_num, val, stat)
VALUES (@timestamp, @cnlNum, @val, @stat)
ON CONFLICT (cnl_num, time_stamp) DO UPDATE
SET val = EXCLUDED.val, stat = EXCLUDED.stat
