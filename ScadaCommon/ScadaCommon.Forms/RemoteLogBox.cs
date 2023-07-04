// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Agent;
using Scada.Protocol;

namespace Scada.Forms
{
    /// <summary>
    /// Provides displaying and updating both local and remote log.
    /// <para>Обеспечивает отображение и обновление как локального, так и удалённого журнала.</para>
    /// </summary>
    public class RemoteLogBox : LogBox
    {
        /// <summary>
        /// The client of the Agent service.
        /// </summary>
        protected IAgentClient agentClient;
        /// <summary>
        /// The remote path of the log.
        /// </summary>
        protected RelativePath logPath;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RemoteLogBox(ListBox listBox, bool colorize = false)
            : base(listBox, colorize)
        {
            agentClient = null;
            logPath = new RelativePath();
        }


        /// <summary>
        /// Gets or sets the client of the Agent service.
        /// </summary>
        public IAgentClient AgentClient
        {
            get 
            { 
                return agentClient; 
            }
            set
            {
                agentClient = value;
                logFileAge = DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets or sets the remote path of the log.
        /// </summary>
        public RelativePath LogPath
        {
            get
            {
                return logPath;
            }
            set
            {
                logPath = value;
                logFileAge = DateTime.MinValue;
            }
        }


        /// <summary>
        /// Sets the first line of the list box.
        /// </summary>
        public override void SetFirstLine(string s)
        {
            if (!listBox.IsDisposed)
                base.SetFirstLine(s);
        }

        /// <summary>
        /// Sets the list box lines.
        /// </summary>
        public override void SetLines(ICollection<string> lines)
        {
            if (AgentClient != null && !listBox.IsDisposed)
                base.SetLines(lines);
        }

        /// <summary>
        /// Refresh log content with Agent.
        /// </summary>
        public void RefreshWithAgent()
        {
            try
            {
                if (AgentClient is IAgentClient agentClient)
                {
                    ICollection<string> lines;

                    lock (agentClient.SyncRoot)
                    {
                        if (FullLogView)
                            agentClient.ReadTextFile(logPath, ref logFileAge, out lines);
                        else
                            agentClient.ReadTextFile(logPath, LogViewSize, ref logFileAge, out lines);
                    }

                    if (lines != null)
                        SetLines(lines);
                }
            }
            catch (Exception ex)
            {
                SetFirstLine(ex.Message);
                logFileAge = DateTime.MinValue;
            }
        }
    }
}
