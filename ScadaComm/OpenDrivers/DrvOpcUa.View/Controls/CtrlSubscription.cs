// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Forms;
using System.ComponentModel;

namespace Scada.Comm.Drivers.DrvOpcUa.View.Controls
{
    /// <summary>
    /// Represetns a control for editing a subscription.
    /// <para>Представляет элемент управления для редактирования подписки.</para>
    /// </summary>
    public partial class CtrlSubscription : UserControl
    {
        private SubscriptionConfig subscriptionConfig;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlSubscription()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the edited subscription.
        /// </summary>
        public SubscriptionConfig SubscriptionConfig
        {
            get
            {
                return subscriptionConfig;
            }
            set
            {
                subscriptionConfig = null;
                ShowSubscriptionProps(value);
                subscriptionConfig = value;
            }
        }


        /// <summary>
        /// Shows the subscription properties.
        /// </summary>
        private void ShowSubscriptionProps(SubscriptionConfig subscriptionConfig)
        {
            if (subscriptionConfig != null)
            {
                chkSubscrActive.Checked = subscriptionConfig.Active;
                txtDisplayName.Text = subscriptionConfig.DisplayName;
                numPublishingInterval.SetValue(subscriptionConfig.PublishingInterval);
            }
        }

        /// <summary>
        /// Raises an ObjectChanged event.
        /// </summary>
        private void OnObjectChanged(object changeArgument)
        {
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(subscriptionConfig, changeArgument));
        }

        /// <summary>
        /// Sets the input focus.
        /// </summary>
        public void SetFocus()
        {
            txtDisplayName.Select();
        }


        /// <summary>
        /// Occurs when the edited object changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler<ObjectChangedEventArgs> ObjectChanged;


        private void chkSubscrActive_CheckedChanged(object sender, EventArgs e)
        {
            if (subscriptionConfig != null)
            {
                subscriptionConfig.Active = chkSubscrActive.Checked;
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (subscriptionConfig != null)
            {
                subscriptionConfig.DisplayName = txtDisplayName.Text;
                OnObjectChanged(TreeUpdateTypes.CurrentNode);
            }
        }

        private void numPublishingInterval_ValueChanged(object sender, EventArgs e)
        {
            if (subscriptionConfig != null)
            {
                subscriptionConfig.PublishingInterval = Convert.ToInt32(numPublishingInterval.Value);
                OnObjectChanged(TreeUpdateTypes.None);
            }
        }
    }
}
