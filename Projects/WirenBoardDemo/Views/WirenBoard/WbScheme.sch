<?xml version="1.0" encoding="utf-8"?>
<SchemeView title="WB 7 Scheme" xmlns:basic="urn:rapidscada:scheme:basic">
  <Scheme>
    <Version>5.3.1.1</Version>
    <Size>
      <Width>1150</Width>
      <Height>700</Height>
    </Size>
    <BackColor>White</BackColor>
    <BackImageName />
    <Font>
      <Name>Arial</Name>
      <Size>18</Size>
      <Bold>false</Bold>
      <Italic>false</Italic>
      <Underline>false</Underline>
    </Font>
    <ForeColor>Black</ForeColor>
    <Title>WB 7 Scheme</Title>
    <CnlFilter />
  </Scheme>
  <Components>
    <StaticText>
      <BackColor />
      <BorderColor>DarkCyan</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>1</ID>
      <Name />
      <Location>
        <X>30</X>
        <Y>60</Y>
      </Location>
      <Size>
        <Width>350</Width>
        <Height>250</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>2</ID>
      <Name />
      <Location>
        <X>50</X>
        <Y>80</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>100</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Font>
        <Name>Arial</Name>
        <Size>18</Size>
        <Bold>true</Bold>
        <Italic>false</Italic>
        <Underline>false</Underline>
      </Font>
      <Text>Buzzer</Text>
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>true</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>3</ID>
      <Name />
      <Location>
        <X>50</X>
        <Y>120</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>Включен</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <basic:Toggle>
      <BackColor>Status</BackColor>
      <BorderColor>Status</BorderColor>
      <BorderWidth>2</BorderWidth>
      <ToolTip />
      <ID>4</ID>
      <Name />
      <Location>
        <X>200</X>
        <Y>120</Y>
      </Location>
      <Size>
        <Width>50</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <LeverColor>White</LeverColor>
      <Padding>0</Padding>
      <Action>SendCommandNow</Action>
      <InCnlNum>304</InCnlNum>
      <CtrlCnlNum>304</CtrlCnlNum>
    </basic:Toggle>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>5</ID>
      <Name />
      <Location>
        <X>50</X>
        <Y>160</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>Частота</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <DynamicText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>6</ID>
      <Name />
      <Location>
        <X>180</X>
        <Y>160</Y>
      </Location>
      <Size>
        <Width>70</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Right</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
      <BackColorOnHover />
      <BorderColorOnHover />
      <ForeColorOnHover />
      <UnderlineOnHover>true</UnderlineOnHover>
      <Action>None</Action>
      <ShowValue>ShowWithUnit</ShowValue>
      <InCnlNum>307</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </DynamicText>
    <basic:Button>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>7</ID>
      <Name />
      <Location>
        <X>265</X>
        <Y>160</Y>
      </Location>
      <Size>
        <Width>90</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Font>
        <Name>Arial</Name>
        <Size>14</Size>
        <Bold>false</Bold>
        <Italic>false</Italic>
        <Underline>false</Underline>
      </Font>
      <ImageName />
      <ImageSize>
        <Width>16</Width>
        <Height>16</Height>
      </ImageSize>
      <Text>Установить</Text>
      <Action>SendCommand</Action>
      <BoundProperty>None</BoundProperty>
      <InCnlNum>0</InCnlNum>
      <CtrlCnlNum>307</CtrlCnlNum>
    </basic:Button>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>8</ID>
      <Name />
      <Location>
        <X>50</X>
        <Y>200</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>Громкость</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <DynamicText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>9</ID>
      <Name />
      <Location>
        <X>180</X>
        <Y>200</Y>
      </Location>
      <Size>
        <Width>70</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Right</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
      <BackColorOnHover />
      <BorderColorOnHover />
      <ForeColorOnHover />
      <UnderlineOnHover>true</UnderlineOnHover>
      <Action>None</Action>
      <ShowValue>ShowWithUnit</ShowValue>
      <InCnlNum>310</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </DynamicText>
    <basic:Button>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>10</ID>
      <Name />
      <Location>
        <X>265</X>
        <Y>200</Y>
      </Location>
      <Size>
        <Width>90</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Font>
        <Name>Arial</Name>
        <Size>14</Size>
        <Bold>false</Bold>
        <Italic>false</Italic>
        <Underline>false</Underline>
      </Font>
      <ImageName />
      <ImageSize>
        <Width>16</Width>
        <Height>16</Height>
      </ImageSize>
      <Text>Установить</Text>
      <Action>SendCommand</Action>
      <BoundProperty>None</BoundProperty>
      <InCnlNum>0</InCnlNum>
      <CtrlCnlNum>310</CtrlCnlNum>
    </basic:Button>
    <DynamicPicture>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>12</ID>
      <Name />
      <Location>
        <X>200</X>
        <Y>235</Y>
      </Location>
      <Size>
        <Width>50</Width>
        <Height>50</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ImageName>melody.png</ImageName>
      <ImageStretch>Fill</ImageStretch>
      <BackColorOnHover />
      <BorderColorOnHover />
      <ImageOnHoverName />
      <Action>SendCommandNow</Action>
      <Conditions />
      <InCnlNum>0</InCnlNum>
      <CtrlCnlNum>311</CtrlCnlNum>
    </DynamicPicture>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>13</ID>
      <Name />
      <Location>
        <X>50</X>
        <Y>250</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>Мелодия</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor>DarkCyan</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>14</ID>
      <Name />
      <Location>
        <X>400</X>
        <Y>60</Y>
      </Location>
      <Size>
        <Width>350</Width>
        <Height>250</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>15</ID>
      <Name />
      <Location>
        <X>420</X>
        <Y>80</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>100</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Font>
        <Name>Arial</Name>
        <Size>18</Size>
        <Bold>true</Bold>
        <Italic>false</Italic>
        <Underline>false</Underline>
      </Font>
      <Text>Power Status</Text>
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>true</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>16</ID>
      <Name />
      <Location>
        <X>420</X>
        <Y>120</Y>
      </Location>
      <Size>
        <Width>200</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>Входное напряжение</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <DynamicText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>17</ID>
      <Name />
      <Location>
        <X>630</X>
        <Y>120</Y>
      </Location>
      <Size>
        <Width>70</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Right</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
      <BackColorOnHover />
      <BorderColorOnHover />
      <ForeColorOnHover />
      <UnderlineOnHover>true</UnderlineOnHover>
      <Action>DrawDiagram</Action>
      <ShowValue>ShowWithUnit</ShowValue>
      <InCnlNum>504</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </DynamicText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>18</ID>
      <Name />
      <Location>
        <X>420</X>
        <Y>160</Y>
      </Location>
      <Size>
        <Width>200</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>От батареи</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <basic:Led>
      <BackColor>Silver</BackColor>
      <BorderColor>Black</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>19</ID>
      <Name />
      <Location>
        <X>665</X>
        <Y>157</Y>
      </Location>
      <Size>
        <Width>30</Width>
        <Height>30</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <BorderOpacity>30</BorderOpacity>
      <Action>None</Action>
      <Conditions>
        <Condition>
          <CompareOperator1>LessThanEqual</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Red</Color>
        </Condition>
        <Condition>
          <CompareOperator1>GreaterThan</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Green</Color>
        </Condition>
      </Conditions>
      <InCnlNum>507</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </basic:Led>
    <StaticText>
      <BackColor />
      <BorderColor>DarkCyan</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>20</ID>
      <Name />
      <Location>
        <X>769</X>
        <Y>60</Y>
      </Location>
      <Size>
        <Width>350</Width>
        <Height>250</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>21</ID>
      <Name />
      <Location>
        <X>789</X>
        <Y>80</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>100</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Font>
        <Name>Arial</Name>
        <Size>18</Size>
        <Bold>true</Bold>
        <Italic>false</Italic>
        <Underline>false</Underline>
      </Font>
      <Text>HW Monitor</Text>
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>true</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>22</ID>
      <Name />
      <Location>
        <X>790</X>
        <Y>120</Y>
      </Location>
      <Size>
        <Width>200</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>Температура платы</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <DynamicText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>23</ID>
      <Name />
      <Location>
        <X>1000</X>
        <Y>120</Y>
      </Location>
      <Size>
        <Width>70</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Right</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
      <BackColorOnHover />
      <BorderColorOnHover />
      <ForeColorOnHover />
      <UnderlineOnHover>true</UnderlineOnHover>
      <Action>DrawDiagram</Action>
      <ShowValue>ShowWithUnit</ShowValue>
      <InCnlNum>604</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </DynamicText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>24</ID>
      <Name />
      <Location>
        <X>790</X>
        <Y>160</Y>
      </Location>
      <Size>
        <Width>200</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>Температура ЦП</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <DynamicText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>25</ID>
      <Name />
      <Location>
        <X>1000</X>
        <Y>160</Y>
      </Location>
      <Size>
        <Width>70</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Right</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
      <BackColorOnHover />
      <BorderColorOnHover />
      <ForeColorOnHover />
      <UnderlineOnHover>true</UnderlineOnHover>
      <Action>DrawDiagram</Action>
      <ShowValue>ShowWithUnit</ShowValue>
      <InCnlNum>607</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </DynamicText>
    <StaticText>
      <BackColor />
      <BorderColor>DarkCyan</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>26</ID>
      <Name />
      <Location>
        <X>30</X>
        <Y>330</Y>
      </Location>
      <Size>
        <Width>350</Width>
        <Height>250</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>27</ID>
      <Name />
      <Location>
        <X>50</X>
        <Y>350</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>100</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Font>
        <Name>Arial</Name>
        <Size>18</Size>
        <Bold>true</Bold>
        <Italic>false</Italic>
        <Underline>false</Underline>
      </Font>
      <Text>ADCs</Text>
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>true</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>28</ID>
      <Name />
      <Location>
        <X>340</X>
        <Y>10</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>100</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Font>
        <Name>Arial</Name>
        <Size>28</Size>
        <Bold>true</Bold>
        <Italic>false</Italic>
        <Underline>false</Underline>
      </Font>
      <Text>Rapid SCADA + Wiren Board Demo</Text>
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>true</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>29</ID>
      <Name />
      <Location>
        <X>50</X>
        <Y>390</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>A1</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <DynamicText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>30</ID>
      <Name />
      <Location>
        <X>180</X>
        <Y>390</Y>
      </Location>
      <Size>
        <Width>70</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Right</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
      <BackColorOnHover />
      <BorderColorOnHover />
      <ForeColorOnHover />
      <UnderlineOnHover>true</UnderlineOnHover>
      <Action>DrawDiagram</Action>
      <ShowValue>ShowWithUnit</ShowValue>
      <InCnlNum>1004</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </DynamicText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>31</ID>
      <Name />
      <Location>
        <X>50</X>
        <Y>430</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>A2</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <DynamicText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>32</ID>
      <Name />
      <Location>
        <X>180</X>
        <Y>430</Y>
      </Location>
      <Size>
        <Width>70</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Right</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
      <BackColorOnHover />
      <BorderColorOnHover />
      <ForeColorOnHover />
      <UnderlineOnHover>true</UnderlineOnHover>
      <Action>DrawDiagram</Action>
      <ShowValue>ShowWithUnit</ShowValue>
      <InCnlNum>1007</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </DynamicText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>38</ID>
      <Name />
      <Location>
        <X>50</X>
        <Y>470</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>A3</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>39</ID>
      <Name />
      <Location>
        <X>50</X>
        <Y>510</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>Vin</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <DynamicText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>40</ID>
      <Name />
      <Location>
        <X>180</X>
        <Y>510</Y>
      </Location>
      <Size>
        <Width>70</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Right</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
      <BackColorOnHover />
      <BorderColorOnHover />
      <ForeColorOnHover />
      <UnderlineOnHover>true</UnderlineOnHover>
      <Action>DrawDiagram</Action>
      <ShowValue>ShowWithUnit</ShowValue>
      <InCnlNum>1013</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </DynamicText>
    <DynamicText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>41</ID>
      <Name />
      <Location>
        <X>180</X>
        <Y>470</Y>
      </Location>
      <Size>
        <Width>70</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Right</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
      <BackColorOnHover />
      <BorderColorOnHover />
      <ForeColorOnHover />
      <UnderlineOnHover>true</UnderlineOnHover>
      <Action>DrawDiagram</Action>
      <ShowValue>ShowWithUnit</ShowValue>
      <InCnlNum>1010</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </DynamicText>
    <StaticText>
      <BackColor />
      <BorderColor>DarkCyan</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>42</ID>
      <Name />
      <Location>
        <X>400</X>
        <Y>330</Y>
      </Location>
      <Size>
        <Width>719</Width>
        <Height>250</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text />
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>43</ID>
      <Name />
      <Location>
        <X>420</X>
        <Y>350</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>100</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Font>
        <Name>Arial</Name>
        <Size>18</Size>
        <Bold>true</Bold>
        <Italic>false</Italic>
        <Underline>false</Underline>
      </Font>
      <Text>Relays &amp; FETs</Text>
      <HAlign>Left</HAlign>
      <VAlign>Top</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>true</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>44</ID>
      <Name />
      <Location>
        <X>420</X>
        <Y>390</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>A1 out</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>45</ID>
      <Name />
      <Location>
        <X>420</X>
        <Y>430</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>A2 out</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>46</ID>
      <Name />
      <Location>
        <X>420</X>
        <Y>470</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>A3 out</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>47</ID>
      <Name />
      <Location>
        <X>420</X>
        <Y>510</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>D1 out</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <basic:Toggle>
      <BackColor>Status</BackColor>
      <BorderColor>Status</BorderColor>
      <BorderWidth>2</BorderWidth>
      <ToolTip />
      <ID>48</ID>
      <Name />
      <Location>
        <X>520</X>
        <Y>390</Y>
      </Location>
      <Size>
        <Width>50</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <LeverColor>White</LeverColor>
      <Padding>0</Padding>
      <Action>SendCommandNow</Action>
      <InCnlNum>1104</InCnlNum>
      <CtrlCnlNum>1104</CtrlCnlNum>
    </basic:Toggle>
    <basic:Toggle>
      <BackColor>Status</BackColor>
      <BorderColor>Status</BorderColor>
      <BorderWidth>2</BorderWidth>
      <ToolTip />
      <ID>49</ID>
      <Name />
      <Location>
        <X>520</X>
        <Y>430</Y>
      </Location>
      <Size>
        <Width>50</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <LeverColor>White</LeverColor>
      <Padding>0</Padding>
      <Action>SendCommandNow</Action>
      <InCnlNum>1107</InCnlNum>
      <CtrlCnlNum>1107</CtrlCnlNum>
    </basic:Toggle>
    <basic:Toggle>
      <BackColor>Status</BackColor>
      <BorderColor>Status</BorderColor>
      <BorderWidth>2</BorderWidth>
      <ToolTip />
      <ID>50</ID>
      <Name />
      <Location>
        <X>520</X>
        <Y>470</Y>
      </Location>
      <Size>
        <Width>50</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <LeverColor>White</LeverColor>
      <Padding>0</Padding>
      <Action>SendCommandNow</Action>
      <InCnlNum>1110</InCnlNum>
      <CtrlCnlNum>1110</CtrlCnlNum>
    </basic:Toggle>
    <basic:Toggle>
      <BackColor>Status</BackColor>
      <BorderColor>Status</BorderColor>
      <BorderWidth>2</BorderWidth>
      <ToolTip />
      <ID>51</ID>
      <Name />
      <Location>
        <X>520</X>
        <Y>510</Y>
      </Location>
      <Size>
        <Width>50</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <LeverColor>White</LeverColor>
      <Padding>0</Padding>
      <Action>SendCommandNow</Action>
      <InCnlNum>1113</InCnlNum>
      <CtrlCnlNum>1113</CtrlCnlNum>
    </basic:Toggle>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>52</ID>
      <Name />
      <Location>
        <X>650</X>
        <Y>390</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>A1 in</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>53</ID>
      <Name />
      <Location>
        <X>650</X>
        <Y>430</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>A2 in</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>54</ID>
      <Name />
      <Location>
        <X>650</X>
        <Y>470</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>A3 in</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <basic:Led>
      <BackColor>Silver</BackColor>
      <BorderColor>Black</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>55</ID>
      <Name />
      <Location>
        <X>755</X>
        <Y>387</Y>
      </Location>
      <Size>
        <Width>30</Width>
        <Height>30</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <BorderOpacity>30</BorderOpacity>
      <Action>None</Action>
      <Conditions>
        <Condition>
          <CompareOperator1>LessThanEqual</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Red</Color>
        </Condition>
        <Condition>
          <CompareOperator1>GreaterThan</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Green</Color>
        </Condition>
      </Conditions>
      <InCnlNum>1116</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </basic:Led>
    <basic:Led>
      <BackColor>Silver</BackColor>
      <BorderColor>Black</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>56</ID>
      <Name />
      <Location>
        <X>755</X>
        <Y>427</Y>
      </Location>
      <Size>
        <Width>30</Width>
        <Height>30</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <BorderOpacity>30</BorderOpacity>
      <Action>None</Action>
      <Conditions>
        <Condition>
          <CompareOperator1>LessThanEqual</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Red</Color>
        </Condition>
        <Condition>
          <CompareOperator1>GreaterThan</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Green</Color>
        </Condition>
      </Conditions>
      <InCnlNum>1119</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </basic:Led>
    <basic:Led>
      <BackColor>Silver</BackColor>
      <BorderColor>Black</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>57</ID>
      <Name />
      <Location>
        <X>755</X>
        <Y>467</Y>
      </Location>
      <Size>
        <Width>30</Width>
        <Height>30</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <BorderOpacity>30</BorderOpacity>
      <Action>None</Action>
      <Conditions>
        <Condition>
          <CompareOperator1>LessThanEqual</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Red</Color>
        </Condition>
        <Condition>
          <CompareOperator1>GreaterThan</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Green</Color>
        </Condition>
      </Conditions>
      <InCnlNum>1122</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </basic:Led>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>58</ID>
      <Name />
      <Location>
        <X>892</X>
        <Y>388</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>5V out</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>59</ID>
      <Name />
      <Location>
        <X>892</X>
        <Y>428</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>V out</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <basic:Toggle>
      <BackColor>Status</BackColor>
      <BorderColor>Status</BorderColor>
      <BorderWidth>2</BorderWidth>
      <ToolTip />
      <ID>60</ID>
      <Name />
      <Location>
        <X>992</X>
        <Y>388</Y>
      </Location>
      <Size>
        <Width>50</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <LeverColor>White</LeverColor>
      <Padding>0</Padding>
      <Action>SendCommandNow</Action>
      <InCnlNum>1125</InCnlNum>
      <CtrlCnlNum>1125</CtrlCnlNum>
    </basic:Toggle>
    <basic:Toggle>
      <BackColor>Status</BackColor>
      <BorderColor>Status</BorderColor>
      <BorderWidth>2</BorderWidth>
      <ToolTip />
      <ID>61</ID>
      <Name />
      <Location>
        <X>992</X>
        <Y>428</Y>
      </Location>
      <Size>
        <Width>50</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <LeverColor>White</LeverColor>
      <Padding>0</Padding>
      <Action>SendCommandNow</Action>
      <InCnlNum>1128</InCnlNum>
      <CtrlCnlNum>1128</CtrlCnlNum>
    </basic:Toggle>
    <StaticText>
      <BackColor />
      <BorderColor />
      <BorderWidth>0</BorderWidth>
      <ToolTip />
      <ID>62</ID>
      <Name />
      <Location>
        <X>650</X>
        <Y>510</Y>
      </Location>
      <Size>
        <Width>100</Width>
        <Height>25</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <ForeColor />
      <Text>D1 in</Text>
      <HAlign>Left</HAlign>
      <VAlign>Center</VAlign>
      <WordWrap>false</WordWrap>
      <AutoSize>false</AutoSize>
    </StaticText>
    <basic:Led>
      <BackColor>Silver</BackColor>
      <BorderColor>Black</BorderColor>
      <BorderWidth>3</BorderWidth>
      <ToolTip />
      <ID>63</ID>
      <Name />
      <Location>
        <X>755</X>
        <Y>507</Y>
      </Location>
      <Size>
        <Width>30</Width>
        <Height>30</Height>
      </Size>
      <ZIndex>0</ZIndex>
      <BorderOpacity>30</BorderOpacity>
      <Action>None</Action>
      <Conditions>
        <Condition>
          <CompareOperator1>LessThanEqual</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Red</Color>
        </Condition>
        <Condition>
          <CompareOperator1>GreaterThan</CompareOperator1>
          <CompareArgument1>0</CompareArgument1>
          <CompareOperator2>LessThan</CompareOperator2>
          <CompareArgument2>0</CompareArgument2>
          <LogicalOperator>None</LogicalOperator>
          <Color>Green</Color>
        </Condition>
      </Conditions>
      <InCnlNum>1131</InCnlNum>
      <CtrlCnlNum>0</CtrlCnlNum>
    </basic:Led>
  </Components>
  <Images>
    <Image>
      <Name>melody.png</Name>
      <Data>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</Data>
    </Image>
  </Images>
</SchemeView>