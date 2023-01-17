/********** Data definition **********/

CREATE DATABASE rapid_scada;
USE rapid_scada

-- Delete a channel data table if it exists
DROP TABLE IF EXISTS cnl_data;

-- Create a channel data table
CREATE TABLE cnl_data (
  cnl_num    INT NOT NULL,
  time_stamp DATETIME NOT NULL,
  val        DOUBLE NOT NULL,
  stat       INT NOT NULL,
  PRIMARY KEY (cnl_num, time_stamp)
);

CREATE INDEX idx_cnl_data_time_stamp ON cnl_data (time_stamp);

-- Delete an event table if it exists
DROP TABLE IF EXISTS events;

-- Create an event table
CREATE TABLE events (
  event_id      BIGINT NOT NULL,
  time_stamp    DATETIME NOT NULL,
  hidden        TINYINT NOT NULL,
  cnl_num       INT NOT NULL,
  obj_num       INT NOT NULL,
  device_num    INT NOT NULL,
  prev_cnl_val  DOUBLE NOT NULL,
  prev_cnl_stat INT NOT NULL,
  cnl_val       DOUBLE NOT NULL,
  cnl_stat      INT NOT NULL,
  severity      INT NOT NULL,
  ack_required  TINYINT NOT NULL,
  ack           TINYINT NOT NULL,
  ack_timestamp DATETIME NOT NULL,
  ack_user_id   INT NOT NULL,
  text_format   INT NOT NULL,
  event_text    VARCHAR(1000),
  event_data    VARBINARY(1000),
  PRIMARY KEY (event_id)
);

CREATE INDEX idx_events_time_stamp ON events (time_stamp);
CREATE INDEX idx_events_cnl_num ON events (cnl_num);
CREATE INDEX idx_events_obj_num ON events (obj_num);
CREATE INDEX idx_events_device_num ON events (device_num);

-- Delete a command table if it exists
DROP TABLE IF EXISTS commands;

-- Create a command table
CREATE TABLE commands (
  command_id    BIGINT NOT NULL,
  creation_time DATETIME NOT NULL,
  client_name   VARCHAR(100) NOT NULL,
  user_id       INT NOT NULL,
  cnl_num       INT NOT NULL,
  obj_num       INT NOT NULL,
  device_num    INT NOT NULL,
  cmd_num       INT NOT NULL,
  cmd_code      VARCHAR(100) NOT NULL,
  cmd_val       DOUBLE,
  cmd_data      VARBINARY(1000),
  PRIMARY KEY (command_id)
);

CREATE INDEX idx_commands_creation_time ON commands (creation_time);
CREATE INDEX idx_commands_cnl_num ON commands (cnl_num);
CREATE INDEX idx_commands_obj_num ON commands (obj_num);
CREATE INDEX idx_commands_device_num ON commands (device_num);

/********** Data manipulation **********/

-- Insert current data
INSERT INTO cnl_data (cnl_num, time_stamp, val, stat)
VALUES (@cnlNum, @timestamp, @val, @stat)

-- Insert or update historical data
INSERT INTO cnl_data (cnl_num, time_stamp, val, stat)
VALUES (@cnlNum, @timestamp, @val, @stat)
ON DUPLICATE KEY UPDATE val = @val, stat = @stat

-- Insert event
INSERT INTO events (event_id, time_stamp, hidden, cnl_num, obj_num, device_num,
  prev_cnl_val, prev_cnl_stat, cnl_val, cnl_stat, severity,
  ack_required, ack, ack_timestamp, ack_user_id, text_format, event_text, event_data)
VALUES (@eventID, @timestamp, @hidden, @cnlNum, @objNum, @deviceNum, 
  @prevCnlVal, @prevCnlStat, @cnlVal, @cnlStat, @severity,
  @ackRequired, @ack, @ackTimestamp, @ackUserID, @textFormat, @eventText, @eventData)

-- Acknowledge event
UPDATE events
SET ack = 1, ack_timestamp = @ackTimestamp, ack_user_id = @ackUserID
WHERE event_id = @eventID

-- Insert command
INSERT INTO commands (command_id, creation_time, client_name, user_id, 
  cnl_num, obj_num, device_num, cmd_num, cmd_code, cmd_val, cmd_data)
VALUES (@commandID, @creationTime, @clientName, @userID, 
  @cnlNum, @objNum, @deviceNum, @cmdNum, @cmdCode, @cmdVal, @cmdData)
