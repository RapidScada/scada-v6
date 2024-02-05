/********** Data definition **********/

-- Delete a channel data table if it exists
DROP TABLE IF EXISTS CnlData;

-- Create a channel data table
CREATE TABLE CnlData (
  CnlNum    int NOT NULL,
  Timestamp datetime2 NOT NULL,
  Val       float NOT NULL,
  Stat      int NOT NULL,
  PRIMARY KEY (CnlNum, Timestamp)
);

CREATE INDEX idx_CnlData_Timestamp ON CnlData (Timestamp);

-- Delete an event table if it exists
DROP TABLE IF EXISTS Events;

-- Create an event table
CREATE TABLE Events (
  EventID      bigint NOT NULL,
  Timestamp    datetime2 NOT NULL,
  Hidden       bit NOT NULL,
  CnlNum       int NOT NULL,
  ObjNum       int NOT NULL,
  DeviceNum    int NOT NULL,
  PrevCnlVal   float NOT NULL,
  PrevCnlStat  int NOT NULL,
  CnlVal       float NOT NULL,
  CnlStat      int NOT NULL,
  Severity     int NOT NULL,
  AckRequired  bit NOT NULL,
  Ack          bit NOT NULL,
  AckTimestamp datetime2 NOT NULL,
  AckUserID    int NOT NULL,
  TextFormat   int NOT NULL,
  EventText    varchar(1000),
  EventData    varbinary(1000),
  PRIMARY KEY (EventID)
);

CREATE INDEX idx_Events_Timestamp ON Events (Timestamp);
CREATE INDEX idx_Events_CnlNum ON Events (CnlNum);
CREATE INDEX idx_Events_ObjNum ON Events (ObjNum);
CREATE INDEX idx_Events_DeviceNum ON Events (DeviceNum);

-- Delete a command table if it exists
DROP TABLE IF EXISTS Commands;

-- Create a command table
CREATE TABLE Commands (
  CommandID    bigint NOT NULL,
  CreationTime datetime2 NOT NULL,
  ClientName   varchar(100) NOT NULL,
  UserID       int NOT NULL,
  CnlNum       int NOT NULL,
  ObjNum       int NOT NULL,
  DeviceNum    int NOT NULL,
  CmdNum       int NOT NULL,
  CmdCode      varchar(100) NOT NULL,
  CmdVal       float,
  CmdData      varbinary(1000),
  PRIMARY KEY (CommandID)
);

CREATE INDEX idx_Commands_CreationTime ON Commands (CreationTime);
CREATE INDEX idx_Commands_CnlNum ON Commands (CnlNum);
CREATE INDEX idx_Commands_ObjNum ON Commands (ObjNum);
CREATE INDEX idx_Commands_DeviceNum ON Commands (DeviceNum);

/********** Data manipulation **********/

-- Insert current data
INSERT INTO CnlData (CnlNum, Timestamp, Val, Stat)
VALUES (@cnlNum, @timestamp, @val, @stat)

-- Insert or update historical data
MERGE CnlData AS target
USING (SELECT @cnlNum, @timestamp) AS source (CnlNum, Timestamp)
ON (target.CnlNum = source.CnlNum AND target.Timestamp = source.Timestamp)
WHEN MATCHED THEN 
  UPDATE SET Val = @val, Stat = @stat
WHEN NOT MATCHED THEN
  INSERT (CnlNum, Timestamp, Val, Stat)
  VALUES (@cnlNum, @timestamp, @val, @stat);
  
-- Insert event
INSERT INTO Events (EventID, Timestamp, Hidden, CnlNum, ObjNum, DeviceNum,
  PrevCnlVal, PrevCnlStat, CnlVal, CnlStat, Severity,
  AckRequired, Ack, AckTimestamp, AckUserID, TextFormat, EventText, EventData)
VALUES (@eventID, @timestamp, @hidden, @cnlNum, @objNum, @deviceNum, 
  @prevCnlVal, @prevCnlStat, @cnlVal, @cnlStat, @severity,
  @ackRequired, @ack, @ackTimestamp, @ackUserID, @textFormat, @eventText, @eventData)

-- Acknowledge event
UPDATE Events
SET Ack = 1, AckTimestamp = @ackTimestamp, AckUserID = @ackUserID
WHERE EventID = @eventID

-- Insert command
INSERT INTO Commands (CommandID, CreationTime, ClientName, UserID,
  CnlNum, ObjNum, DeviceNum, CmdNum, CmdCode, CmdVal, CmdData)
VALUES (@commandID, @creationTime, @clientName, @userID, 
  @cnlNum, @objNum, @deviceNum, @cmdNum, @cmdCode, @cmdVal, @cmdData)
