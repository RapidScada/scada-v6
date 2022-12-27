/********** Data definition **********/

-- Delete a channel data table
DROP TABLE cnl_data;

-- Create a channel data table
CREATE TABLE cnl_data (
  cnl_num    INTEGER NOT NULL,
  time_stamp TIMESTAMP NOT NULL,
  val        FLOAT NOT NULL,
  stat       INTEGER NOT NULL,
  PRIMARY KEY (cnl_num, time_stamp)
);

CREATE INDEX idx_cnl_data_time_stamp ON cnl_data (time_stamp);

-- Delete an event table
DROP TABLE events;

-- Create an event table
CREATE TABLE events (
  event_id      INTEGER NOT NULL,
  time_stamp    TIMESTAMP NOT NULL,
  hidden        INTEGER NOT NULL,
  cnl_num       INTEGER NOT NULL,
  obj_num       INTEGER NOT NULL,
  device_num    INTEGER NOT NULL,
  prev_cnl_val  FLOAT NOT NULL,
  prev_cnl_stat INTEGER NOT NULL,
  cnl_val       FLOAT NOT NULL,
  cnl_stat      INTEGER NOT NULL,
  severity      INTEGER NOT NULL,
  ack_required  INTEGER NOT NULL,
  ack           INTEGER NOT NULL,
  ack_timestamp TIMESTAMP NOT NULL,
  ack_user_id   INTEGER NOT NULL,
  text_format   INTEGER NOT NULL,
  event_text    VARCHAR2(1000),
  event_data    RAW(1000),
  PRIMARY KEY (event_id)
);

CREATE INDEX idx_events_time_stamp ON events (time_stamp);
CREATE INDEX idx_events_cnl_num ON events (cnl_num);
CREATE INDEX idx_events_obj_num ON events (obj_num);
CREATE INDEX idx_events_device_num ON events (device_num);

-- Delete a command table
DROP TABLE commands;

-- Create a command table
CREATE TABLE commands (
  command_id    INTEGER NOT NULL,
  creation_time TIMESTAMP NOT NULL,
  client_name   VARCHAR2(100) NOT NULL,
  user_id       INTEGER NOT NULL,
  cnl_num       INTEGER NOT NULL,
  obj_num       INTEGER NOT NULL,
  device_num    INTEGER NOT NULL,
  cmd_num       INTEGER NOT NULL,
  cmd_code      VARCHAR2(100) NOT NULL,
  cmd_val       FLOAT,
  cmd_data      RAW(1000),
  PRIMARY KEY (command_id)
);

CREATE INDEX idx_commands_creation_time ON commands (creation_time);
CREATE INDEX idx_commands_cnl_num ON commands (cnl_num);
CREATE INDEX idx_commands_obj_num ON commands (obj_num);
CREATE INDEX idx_commands_device_num ON commands (device_num);

/********** Data manipulation **********/

-- The order and number of query parameters must be exactly the same as in this example.

-- Insert current and historical data
INSERT INTO cnl_data (time_stamp, cnl_num, val, stat)
VALUES (:1, :2, :3, :4)

-- Insert event
INSERT INTO events (event_id, time_stamp, hidden, cnl_num, obj_num, device_num,
  prev_cnl_val, prev_cnl_stat, cnl_val, cnl_stat, severity,
  ack_required, ack, ack_timestamp, ack_user_id, text_format, event_text, event_data)
VALUES (:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11, :12, :13, :14, :15, :16, :17, :18)

-- Insert command
INSERT INTO commands (command_id, creation_time, client_name, user_id,
  cnl_num, obj_num, device_num, cmd_num, cmd_code, cmd_val, cmd_data)
VALUES (:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11)
