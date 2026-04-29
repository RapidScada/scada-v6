// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Client;
using Scada.Forms;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvOpcUa.View.Forms
{
    /// <summary>
    /// Represents a form for viewing OPC node attributes.
    /// <para>Представляет форму для просмотра атрибутов OPC-узла.</para>
    /// </summary>
    public partial class FrmNodeAttr : Form
    {
        private readonly ISession opcSession; // the OPC session
        private readonly NodeId nodeId;       // the node whose attributes are shown


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmNodeAttr()
        {
            InitializeComponent();
            colName.Name = nameof(colName);
            colValue.Name = nameof(colValue);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmNodeAttr(ISession opcSession, NodeId nodeId)
            : this()
        {
            this.opcSession = opcSession ?? throw new ArgumentNullException(nameof(opcSession));
            this.nodeId = nodeId ?? throw new ArgumentNullException(nameof(nodeId));
        }


        /// <summary>
        /// Reads available attributes from OPC server.
        /// </summary>
        private void ReadAttributes()
        {
            try
            {
                // request attributes
                ReadValueIdCollection nodesToRead = [];

                foreach (uint attributeId in Attributes.GetIdentifiers())
                {
                    nodesToRead.Add(new ReadValueId
                    {
                        NodeId = nodeId,
                        AttributeId = attributeId
                    });
                }

                opcSession.Read(null, 0, TimestampsToReturn.Neither, nodesToRead,
                    out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos);
                ClientBase.ValidateResponse(results, nodesToRead);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

                // display attributes
                for (int i = 0; i < nodesToRead.Count; i++)
                {
                    ReadValueId readValueId = nodesToRead[i];
                    DataValue dataValue = results[i];

                    if (dataValue.StatusCode != StatusCodes.BadAttributeIdInvalid)
                    {
                        listView.Items.Add(new ListViewItem([
                            Attributes.GetBrowseName(readValueId.AttributeId),
                            FormatAttribute(readValueId.AttributeId, dataValue.Value)
                        ]));
                    }
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(DriverPhrases.ReadAttrError + ":" + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Formats the attribute value.
        /// </summary>
        private string FormatAttribute(uint attributeId, object value)
        {
            switch (attributeId)
            {
                case Attributes.NodeClass:
                    return value == null ?
                        "(null)" :
                        Enum.ToObject(typeof(NodeClass), value).ToString();

                case Attributes.DataType:
                    if (value is NodeId dataTypeId)
                    {
                        INode dataType = opcSession.NodeCache.Find(dataTypeId);
                        if (dataType != null)
                            return dataType.DisplayName.Text;
                    }
                    return string.Format("{0}", value);

                case Attributes.ValueRank:
                    if (value is int valueRank)
                    {
                        return valueRank switch
                        {
                            ValueRanks.Scalar => "Scalar",
                            ValueRanks.OneDimension => "OneDimension",
                            ValueRanks.OneOrMoreDimensions => "OneOrMoreDimensions",
                            ValueRanks.Any => "Any",
                            _ => string.Format("{0}", valueRank),
                        };
                    }
                    return string.Format("{0}", value);

                case Attributes.MinimumSamplingInterval:
                    if (value is double minimumSamplingInterval)
                    {
                        return minimumSamplingInterval switch
                        {
                            MinimumSamplingIntervals.Indeterminate => "Indeterminate",
                            MinimumSamplingIntervals.Continuous => "Continuous",
                            _ => string.Format("{0}", minimumSamplingInterval)
                        };
                    }
                    return string.Format("{0}", value);

                case Attributes.AccessLevel:
                case Attributes.UserAccessLevel:
                    byte accessLevel = Convert.ToByte(value);
                    List<string> accessList = [];

                    if ((accessLevel & AccessLevels.CurrentRead) != 0)
                        accessList.Add("Readable");

                    if ((accessLevel & AccessLevels.CurrentWrite) != 0)
                        accessList.Add("Writeable");

                    if ((accessLevel & AccessLevels.HistoryRead) != 0)
                        accessList.Add("History");

                    if ((accessLevel & AccessLevels.HistoryWrite) != 0)
                        accessList.Add("History Update");

                    if (accessList.Count == 0)
                        accessList.Add("No Access");

                    return string.Join(" | ", accessList);

                case Attributes.EventNotifier:
                    byte notifier = Convert.ToByte(value);
                    List<string> bits = [];

                    if ((notifier & EventNotifiers.SubscribeToEvents) != 0)
                        bits.Add("Subscribe");

                    if ((notifier & EventNotifiers.HistoryRead) != 0)
                        bits.Add("History");

                    if ((notifier & EventNotifiers.HistoryWrite) != 0)
                        bits.Add("History Update");

                    if (bits.Count == 0)
                        bits.Add("No Access");

                    return string.Join(" | ", notifier);

                default:
                    return string.Format("{0}", value);
            }
        }


        private void FrmNodeAttr_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, new FormTranslatorOptions { ContextMenus = [cmsAttr] });
        }

        private async void FrmNodeAttr_Shown(object sender, EventArgs e)
        {
            await Task.Run(ReadAttributes);
        }

        private void cmsAttr_Opening(object sender, CancelEventArgs e)
        {
            miCopyName.Enabled = miCopyValue.Enabled = listView.SelectedItems.Count > 0;
        }

        private void miCopyName_Click(object sender, EventArgs e)
        {
            if (listView.GetSelectedItem() is ListViewItem item)
                Clipboard.SetText(item.SubItems[0].Text);
        }

        private void miCopyValue_Click(object sender, EventArgs e)
        {
            if (listView.GetSelectedItem() is ListViewItem item)
                Clipboard.SetText(item.SubItems[1].Text);
        }
    }
}
