/*
 * Copyright 2018 Mikhail Shiryaev
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
 * Module   : Scheme Editor
 * Summary  : About form
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2013
 * Modified : 2018
 */

using Scada.UI;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Utils;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// About form
    /// <para>Форма о программе</para>
    /// </summary>
    public partial class FrmAbout : Form
    {
        private static FrmAbout frmAbout = null;  // форма о программе

        private string exeDir;  // директория исполняемого файла приложения
        private ILog errLog;    // журнал ошибок приложения
        private bool inited;    // форма инициализирована


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private FrmAbout()
        {
            InitializeComponent();

            inited = false;
        }


        /// <summary>
        /// Отобразить форму о программе
        /// </summary>
        public static void ShowAbout(string exeDir, ILog errLog, IWin32Window owner)
        {
            if (exeDir == null)
                throw new ArgumentNullException("exeDir");
            if (errLog == null)
                throw new ArgumentNullException("errLog");

            if (frmAbout == null)
            {
                frmAbout = new FrmAbout();
                frmAbout.exeDir = exeDir;
                frmAbout.errLog = errLog;
            }

            frmAbout.Init();
            frmAbout.ShowDialog(owner);
        }


        /// <summary>
        /// Инициализировать форму
        /// </summary>
        private void Init()
        {
            if (!inited)
            {
                inited = true;
                PictureBox activePictureBox;

                activePictureBox = pbAboutEn;
                lblVersionEn.Text = "Version " + SchemeUtils.SchemeVersion;
            }
        }


        private void FrmAbout_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmAbout_KeyPress(object sender, KeyPressEventArgs e)
        {
            Close();
        }
    }
}