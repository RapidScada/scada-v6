/*
 * Copyright 2022 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Administrator
 * Summary  : Represents a form for transfer configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Admin.App.Code;
using Scada.Admin.Deployment;
using Scada.Admin.Extensions;
using Scada.Admin.Project;
using Scada.Forms;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Admin.App.Forms.Deployment
{
    /// <summary>
    /// Represents a form for transfer configuration.
    /// <para>Представляет форму для передачи конфигурации.</para>
    /// </summary>
    public partial class FrmTransfer : Form, ITransferControl
    {
        private readonly AppData appData;            // the common data of the application
        private readonly ScadaProject project;       // the project under development
        private readonly ProjectInstance instance;   // the affected instance
        private readonly DeploymentProfile profile;  // the deployment profile

        private readonly Action<bool> setCancelEnabledAction;  // wraps the SetCancelEnabled method
        private readonly Action<double> setProgressAction;     // wraps the SetProgress method
        private readonly Action<bool, double> setResultAction; // wraps the SetResult method
        private readonly RichTextBoxHelper logHelper;          // writes to the log text box

        private ExtensionLogic extensionLogic;       // the extension that implements transfer
        private bool uploadMode;                     // determines whether to upload or download
        private bool operationResult;                // the result of the transfer operation
        private CancellationTokenSource tokenSource; // provides a cancellation mechanism


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTransfer()
        {
            InitializeComponent();
            pbUpload.Left = pbSuccess.Left = pbError.Left = pbDownload.Left;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTransfer(AppData appData, ScadaProject project, ProjectInstance instance, DeploymentProfile profile)
            : this()
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.profile = profile ?? throw new ArgumentNullException(nameof(profile));

            setCancelEnabledAction = b => SetCancelEnabled(b);
            setProgressAction = d => SetProgress(d);
            setResultAction = (b, d) => SetResult(b, d);
            logHelper = new RichTextBoxHelper(txtLog);

            extensionLogic = null;
            uploadMode = false;
            operationResult = false;
            tokenSource = null;
        }


        /// <summary>
        /// Gets the token notifying that operations should be canceled.
        /// </summary>
        public CancellationToken CancellationToken => tokenSource == null ? CancellationToken.None : tokenSource.Token;


        /// <summary>
        /// Transfers the configuration.
        /// </summary>
        private bool TransferConfig()
        {
            if (!appData.ExtensionHolder.GetExtension(profile.Extension, out extensionLogic))
            {
                ScadaUiUtils.ShowError(AppPhrases.ExtensionNotFound, profile.Extension);
                return false;
            }
            else if (!extensionLogic.CanDeploy)
            {
                ScadaUiUtils.ShowError(AppPhrases.ExtensionCannotDeploy, profile.Extension);
                return false;
            }
            else
            {
                ShowDialog();
                return operationResult;
            }
        }

        /// <summary>
        /// Prepares the controls for transfer configuration.
        /// </summary>
        private void PrepareTransfer()
        {
            ControlBox = false;
            btnBreak.Visible = false;
            btnClose.Enabled = false;
        }

        /// <summary>
        /// Returns the state of the controls after transfer configuration.
        /// </summary>
        private void FinalizeTransfer()
        {
            ControlBox = true;
            btnBreak.Visible = false;
            btnClose.Enabled = true;
        }

        /// <summary>
        /// Starts a task for downloading configuration.
        /// </summary>
        private async Task StartDownloadTask()
        {
            pbDownload.Visible = true;
            lblStatus.Text = AppPhrases.DownloadProgress;
            tokenSource = new CancellationTokenSource();

            await Task.Run(() =>
            {
                try
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    extensionLogic.DownloadConfig(project, instance, profile, this);
                    SetResult(true, stopwatch.Elapsed.TotalSeconds);
                }
                catch (OperationCanceledException)
                {
                    SetResult(false, 0);
                    WriteError(AppPhrases.OperationCanceled);
                }
                catch (Exception ex)
                {
                    SetResult(false, 0);
                    WriteError(ex.BuildErrorMessage(AppPhrases.DownloadError));
                }
            });
        }

        /// <summary>
        /// Starts a task for uploading configuration.
        /// </summary>
        private async Task StartUploadTask()
        {
            pbUpload.Visible = true;
            lblStatus.Text = AppPhrases.UploadProgress;
            tokenSource = new CancellationTokenSource();

            await Task.Run(() =>
            {
                try
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    extensionLogic.UploadConfig(project, instance, profile, this);
                    SetResult(true, stopwatch.Elapsed.TotalSeconds);
                }
                catch (OperationCanceledException)
                {
                    SetResult(false, 0);
                    WriteError(AppPhrases.OperationCanceled);
                }
                catch (Exception ex)
                {
                    SetResult(false, 0);
                    WriteError(ex.BuildErrorMessage(AppPhrases.UploadError));
                }
            });
        }

        /// <summary>
        /// Sets the transfer result.
        /// </summary>
        private void SetResult(bool successful, double duration)
        {
            if (InvokeRequired)
            {
                Invoke(setResultAction, successful, duration);
            }
            else
            {
                operationResult = successful;
                pbDownload.Visible = false;
                pbUpload.Visible = false;

                if (successful)
                {
                    pbSuccess.Visible = true;
                    lblStatus.Text = string.Format(AppPhrases.OperationCompleted, Math.Round(duration));
                }
                else
                {
                    pbError.Visible = true;
                    lblStatus.Text = CancellationToken.IsCancellationRequested ? 
                        AppPhrases.OperationCanceled : AppPhrases.OperationError;
                }
            }
        }


        /// <summary>
        /// Downloads the configuration.
        /// </summary>
        public bool DownloadConfig()
        {
            uploadMode = false;
            return TransferConfig();
        }

        /// <summary>
        /// Uploads the configuration.
        /// </summary>
        public bool UploadConfig()
        {
            uploadMode = true;
            return TransferConfig();
        }

        /// <summary>
        /// Enables or disables the cancel function.
        /// </summary>
        public void SetCancelEnabled(bool enabled)
        {
            if (InvokeRequired)
                Invoke(setCancelEnabledAction, enabled);
            else
                btnBreak.Visible = enabled;
        }

        /// <summary>
        /// Sets the transfer progress in the range 0 to 1.
        /// </summary>
        public void SetProgress(double value)
        {
            if (InvokeRequired)
            {
                Invoke(setProgressAction, value);
            }
            else
            {
                int val = (int)(value * progressBar.Maximum);
                progressBar.Value = Math.Min(val, progressBar.Maximum);
            }
        }

        /// <summary>
        /// Writes the message to a terminal.
        /// </summary>
        public void WriteMessage(string text)
        {
            logHelper.WriteMessage(text);
        }

        /// <summary>
        /// Writes the error message to a terminal.
        /// </summary>
        public void WriteError(string text)
        {
            logHelper.WriteError(text);
        }

        /// <summary>
        /// Writes an empty line to a terminal.
        /// </summary>
        public void WriteLine()
        {
            logHelper.WriteLine();
        }


        private async void FrmTransfer_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);

            if (extensionLogic != null)
            {
                PrepareTransfer();

                if (uploadMode)
                {
                    Text = AppPhrases.UploadTitle;
                    await StartUploadTask();
                }
                else
                {
                    Text = AppPhrases.DownloadTitle;
                    await StartDownloadTask();
                }

                FinalizeTransfer();
            }
        }

        private void btnBreak_Click(object sender, EventArgs e)
        {
            tokenSource?.Cancel();
        }
    }
}
