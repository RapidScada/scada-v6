/********** Data definition **********/

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

-- Delete an event table if it exists
DROP TABLE IF EXISTS mod_db_export.events;

-- Create an event table
CREATE TABLE mod_db_export.events (
  event_id      bigint NOT NULL,
  time_stamp    timestamp with time zone NOT NULL,
  hidden        smallint NOT NULL,
  cnl_num       integer NOT NULL,
  obj_num       integer NOT NULL,
  device_num    integer NOT NULL,
  prev_cnl_val  double precision NOT NULL,
  prev_cnl_stat integer NOT NULL,
  cnl_val       double precision NOT NULL,
  cnl_stat      integer NOT NULL,
  severity      integer NOT NULL,
  ack_required  smallint NOT NULL,
  ack           smallint NOT NULL,
  ack_timestamp timestamp with time zone NOT NULL,
  ack_user_id   integer NOT NULL,
  text_format   integer NOT NULL,
  event_text    character varying,
  event_data    bytea,
  PRIMARY KEY (event_id)
);

CREATE INDEX ON mod_db_export.events (time_stamp);
CREATE INDEX ON mod_db_export.events (cnl_num);
CREATE INDEX ON mod_db_export.events (obj_num);
CREATE INDEX ON mod_db_export.events (device_num);

-- Delete a command table if it exists
DROP TABLE IF EXISTS mod_db_export.commands;

-- Create a command table
CREATE TABLE mod_db_export.commands (
  command_id    bigint NOT NULL,
  creation_time timestamp with time zone NOT NULL,
  client_name   character varying NOT NULL,
  user_id       integer NOT NULL,
  cnl_num       integer NOT NULL,
  obj_num       integer NOT NULL,
  device_num    integer NOT NULL,
  cmd_num       integer NOT NULL,
  cmd_code      character varying NOT NULL,
  cmd_val       double precision,
  cmd_data      bytea,
  PRIMARY KEY (command_id)
);

CREATE INDEX ON mod_db_export.commands (creation_time);
CREATE INDEX ON mod_db_export.commands (cnl_num);
CREATE INDEX ON mod_db_export.commands (obj_num);
CREATE INDEX ON mod_db_export.commands (device_num);

/********** Data manipulation **********/

-- Insert current data
INSERT INTO mod_db_export.cnl_data (cnl_num, time_stamp, val, stat)
VALUES (@cnlNum, @timestamp, @val, @stat)

-- Insert or update historical data
INSERT INTO mod_db_export.cnl_data (cnl_num, time_stamp, val, stat)
VALUES (@cnlNum, @timestamp, @val, @stat)
ON CONFLICT (cnl_num, time_stamp) DO UPDATE
SET val = EXCLUDED.val, stat = EXCLUDED.stat

-- Insert event
INSERT INTO mod_db_export.events (event_id, time_stamp, hidden, cnl_num, obj_num, device_num,
  prev_cnl_val, prev_cnl_stat, cnl_val, cnl_stat, severity,
  ack_required, ack, ack_timestamp, ack_user_id, text_format, event_text, event_data)
VALUES (@eventID, @timestamp, @hidden, @cnlNum, @objNum, @deviceNum, 
  @prevCnlVal, @prevCnlStat, @cnlVal, @cnlStat, @severity,
  @ackRequired, @ack, @ackTimestamp, @ackUserID, @textFormat, @eventText, @eventData)

-- Acknowledge event
UPDATE mod_db_export.events
SET ack = 1, ack_timestamp = @ackTimestamp, ack_user_id = @ackUserID
WHERE event_id = @eventID

-- Insert command
INSERT INTO mod_db_export.commands (command_id, creation_time, client_name, user_id,
  cnl_num, obj_num, device_num, cmd_num, cmd_code, cmd_val, cmd_data)
VALUES (@commandID, @creationTime, @clientName, @userID, 
  @cnlNum, @objNum, @deviceNum, @cmdNum, @cmdCode, @cmdVal, @cmdData)
