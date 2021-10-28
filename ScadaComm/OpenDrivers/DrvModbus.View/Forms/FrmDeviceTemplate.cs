// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using Scada.Comm.Drivers.DrvModbus.View.Properties;
using Scada.Forms;
using Scada.Lang;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvModbus.View.Forms
{
    /// <summary>
    /// Represents a form for editing a device template.
    /// <para>Представляет форму для редактирования шаблона устройства.</para>
    /// </summary>
    public partial class FrmDeviceTemplate : Form
    {
        /// <summary>
        /// Имя файла нового шаблона устройства
        /// </summary>
        private const string NewFileName = "KpModbus_NewTemplate.xml";

        private readonly AppDirs appDirs;   // the application directories
        private readonly CustomUi customUi; // the UI customization object

        private DeviceTemplate template; // редактируемый шаблон устройства
        private bool modified;           // признак изменения шаблона устройства
        private ElemGroupConfig selElemGroup;  // выбранная группа элементов
        private ElemTag selElemInfo;    // информация о выбранном элементе
        private CmdConfig selCmd;        // выбранная команда
        private TreeNode selNode;        // выбранный узел дерева
        private TreeNode grsNode;        // узел дерева "Группы элементов"
        private TreeNode cmdsNode;       // узел дерева "Команды"


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceTemplate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceTemplate(AppDirs appDirs, CustomUi customUi)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.customUi = customUi ?? throw new ArgumentNullException(nameof(customUi));

            template = null;
            modified = false;
            selElemGroup = null;
            selElemInfo = null;
            selCmd = null;
            selNode = null;
            grsNode = treeView.Nodes["grsNode"];
            cmdsNode = treeView.Nodes["cmdsNode"];

            SaveOnly = false;
            FileName = "";
        }


        /// <summary>
        /// Получить или установить признак изменения шаблона устройства
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                SetFormTitle();
                btnSave.Enabled = modified;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether only the save file command is allowed.
        /// </summary>
        public bool SaveOnly { get; set; }

        /// <summary>
        /// Gets or sets the device template file name.
        /// </summary>
        public string FileName { get; set; }


        /// <summary>
        /// Установить заголовок формы
        /// </summary>
        private void SetFormTitle()
        {
            Text = DriverPhrases.TemplateFormTitle + " - " + 
                (FileName == "" ? NewFileName : Path.GetFileName(FileName)) +
                (Modified ? "*" : "");
        }

        /// <summary>
        /// Загрузить шаблон устройства из файла
        /// </summary>
        private void LoadTemplate(string fname)
        {
            template = customUi.CreateDeviceTemplate();
            FileName = fname;
            SetFormTitle();

            if (!template.Load(fname, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            FillTree();
        }

        /// <summary>
        /// Перевести текст основных узлов дерева
        /// </summary>
        private void TranslateTree()
        {
            grsNode.Text = DriverPhrases.ElemGroupsNode;
            cmdsNode.Text = DriverPhrases.CommandsNode;
        }

        /// <summary>
        /// Takes the tree view and loads them into an image list.
        /// </summary>
        private void TakeTreeViewImages()
        {
            // loading images from resources instead of storing in image list prevents them from corruption
            ilTree.Images.Add("cmd.png", Resources.cmd);
            ilTree.Images.Add("cmds.png", Resources.cmds);
            ilTree.Images.Add("elem.png", Resources.elem);
            ilTree.Images.Add("group.png", Resources.group);
            ilTree.Images.Add("group_inactive.png", Resources.group_inactive);

            grsNode.SetImageKey("group.png");
            cmdsNode.SetImageKey("cmds.png");
        }

        /// <summary>
        /// Заполнить дерево в соответствии с шаблоном устройства
        /// </summary>
        private void FillTree()
        {
            // обнуление выбранных объектов и снятие признака изменения
            selElemGroup = null;
            selElemInfo = null;
            selCmd = null;
            selNode = null;
            ShowElemGroupProps(null);
            Modified = false;

            // приостановка отрисовки дерева
            treeView.BeginUpdate();

            // очистка дерева
            grsNode.Nodes.Clear();
            cmdsNode.Nodes.Clear();
            treeView.SelectedNode = grsNode;

            // заполнение узла групп элементов
            foreach (ElemGroupConfig elemGroup in template.ElemGroups)
                grsNode.Nodes.Add(NewElemGroupNode(elemGroup));

            // заполнение узла команд
            foreach (CmdConfig modbusCmd in template.Cmds)
                cmdsNode.Nodes.Add(NewCmdNode(modbusCmd));

            // раскрытие основных узлов дерева
            grsNode.Expand();
            cmdsNode.Expand();

            // возобновление отрисовки дерева
            treeView.EndUpdate();
        }

        /// <summary>
        /// Создать узел группы элементов
        /// </summary>
        private TreeNode NewElemGroupNode(ElemGroupConfig elemGroup)
        {
            string name = elemGroup.Name == "" ? DriverPhrases.UnnamedElemGroup : elemGroup.Name;
            TreeNode grNode = new TreeNode(name + " (" + ModbusUtils.GetDataBlockName(elemGroup.DataBlock) + ")");
            grNode.ImageKey = grNode.SelectedImageKey = elemGroup.Active ? "group.png" : "group_inactive.png";
            grNode.Tag = elemGroup;

            int elemAddr = elemGroup.Address;
            int elemTagNum = elemGroup.StartTagNum;

            foreach (ElemConfig elem in elemGroup.Elems)
            {
                grNode.Nodes.Add(NewElemNode(new ElemTag(template.Options, elemGroup, elem)
                {
                    Address = elemAddr,
                    TagNum = elemTagNum++
                }));
                elemAddr += (ushort)elem.Quantity;
            }

            return grNode;
        }
        
        /// <summary>
        /// Создать узел элемента группы
        /// </summary>
        private TreeNode NewElemNode(ElemTag elemInfo)
        {
            TreeNode elemNode = new TreeNode(elemInfo.NodeText);
            elemNode.ImageKey = elemNode.SelectedImageKey = "elem.png";
            elemNode.Tag = elemInfo;
            return elemNode;
        }

        /// <summary>
        /// Создать узел команды
        /// </summary>
        private TreeNode NewCmdNode(CmdConfig modbusCmd)
        {
            TreeNode cmdNode = new TreeNode(GetCmdCaption(modbusCmd));
            cmdNode.ImageKey = cmdNode.SelectedImageKey = "cmd.png";
            cmdNode.Tag = modbusCmd;
            return cmdNode;
        }

        /// <summary>
        /// Получить обозначение команды в дереве
        /// </summary>
        private string GetCmdCaption(CmdConfig modbusCmd)
        {
            return (string.IsNullOrEmpty(modbusCmd.Name) ? DriverPhrases.UnnamedCommand : modbusCmd.Name) +
                " (" + ModbusUtils.GetDataBlockName(modbusCmd.DataBlock) + ", " +
                ModbusUtils.GetAddressRange(modbusCmd.Address, 
                    modbusCmd.ElemCnt * ModbusUtils.GetQuantity(modbusCmd.ElemType),
                    template.Options.ZeroAddr, template.Options.DecAddr) + ")";
        }

        /// <summary>
        /// Обновить узел выбранной группы элементов
        /// </summary>
        private void UpdateElemGroupNode()
        {
            if (selElemGroup != null)
            {
                selNode.ImageKey = selNode.SelectedImageKey = selElemGroup.Active ? "group.png" : "group_inactive.png";
                selNode.Text = (selElemGroup.Name == "" ? DriverPhrases.UnnamedElemGroup : selElemGroup.Name) + 
                    " (" + ModbusUtils.GetDataBlockName(selElemGroup.DataBlock) + ")";
            }
        }

        /// <summary>
        /// Обновить узлы элементов выбранной группы
        /// </summary>
        private void UpdateElemNodes(TreeNode grNode = null)
        {
            treeView.BeginUpdate();

            if (grNode == null)
                grNode = selNode;

            if (grNode.Tag is ElemGroupConfig elemGroup)
            {
                int elemAddr = elemGroup.Address;
                int elemTagNum = elemGroup.StartTagNum;

                foreach (TreeNode elemNode in grNode.Nodes)
                {
                    ElemTag elemInfo = elemNode.Tag as ElemTag;
                    elemInfo.Address = elemAddr;
                    elemInfo.TagNum = elemTagNum++;
                    elemNode.Text = elemInfo.NodeText;
                    elemAddr += (ushort)elemInfo.Elem.Quantity;
                }
            }

            treeView.EndUpdate();
        }

        /// <summary>
        /// Обновить сигналы КП элементов групп, начиная с заданного узла дерева
        /// </summary>
        private void UpdateSignals(TreeNode startGrNode)
        {
            // проверка корректности заданного узла дерева
            if (!(startGrNode.Tag is ElemGroup))
                return;

            // определение начального индекса тегов КП
            TreeNode prevGrNode = startGrNode.PrevNode;
            ElemGroupConfig prevElemGroup = prevGrNode == null ? null : prevGrNode.Tag as ElemGroupConfig;
            int tagNum = prevElemGroup == null ? 1 : prevElemGroup.StartTagNum + prevElemGroup.Elems.Count;

            // обновление групп и их элементов
            int grNodeCnt = grsNode.Nodes.Count;

            for (int i = startGrNode.Index; i < grNodeCnt; i++)
            {
                TreeNode grNode = grsNode.Nodes[i];
                ElemGroupConfig elemGroup = grNode.Tag as ElemGroupConfig;
                elemGroup.StartTagNum = tagNum;
                int elemTagNum = tagNum;
                tagNum += elemGroup.Elems.Count;

                foreach (TreeNode elemNode in grNode.Nodes)
                {
                    ElemTag elem = elemNode.Tag as ElemTag;
                    elem.TagNum = elemTagNum++;
                }
            }
        }

        /// <summary>
        /// Обновить узел выбранной команды
        /// </summary>
        private void UpdateCmdNode()
        {
            if (selCmd != null)
                selNode.Text = GetCmdCaption(selCmd);
        }


        /// <summary>
        /// Отобразить свойства группы элементов
        /// </summary>
        private void ShowElemGroupProps(ElemGroupConfig elemGroup)
        {
            ctrlElemGroup.Visible = true;
            ctrlElemGroup.TemplateOptions = template.Options;
            ctrlElemGroup.ElemGroup = elemGroup;
            ctrlElem.Visible = false;
            ctrlCmd.Visible = false;
        }

        /// <summary>
        /// Отобразить свойства элемента
        /// </summary>
        private void ShowElemProps(ElemTag elemInfo)
        {
            ctrlElemGroup.Visible = false;
            ctrlElem.Visible = true;
            ctrlElem.ElemTag = elemInfo;
            ctrlCmd.Visible = false;
        }

        /// <summary>
        /// Отобразить свойства команды
        /// </summary>
        private void ShowCmdProps(CmdConfig modbusCmd)
        {
            ctrlElemGroup.Visible = false;
            ctrlElem.Visible = false;
            ctrlCmd.Visible = true;
            ctrlCmd.TemplateOptions = template.Options;
            ctrlCmd.Cmd = modbusCmd;
        }

        /// <summary>
        /// Заблокировать редактирование свойств
        /// </summary>
        private void DisableProps()
        {
            ctrlElemGroup.ElemGroup = null;
            ctrlElem.ElemTag = null;
            ctrlCmd.Cmd = null;
        }

        /// <summary>
        /// Сохраненить изменения
        /// </summary>
        private bool SaveChanges(bool saveAs)
        {
            // определение имени файла
            string newFileName = "";

            if (saveAs || FileName == "")
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    newFileName = saveFileDialog.FileName;
            }
            else
            {
                newFileName = FileName;
            }

            if (newFileName == "")
            {
                return false;
            }
            else
            {
                // сохранение шаблона устройства
                string errMsg;
                if (template.Save(newFileName, out errMsg))
                {
                    FileName = newFileName;
                    Modified = false;
                    return true;
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                    return false;
                }
            }
        }

        /// <summary>
        /// Преверить необходимость сохранения изменений
        /// </summary>
        private bool CheckChanges()
        {
            if (modified)
            {
                DialogResult result = MessageBox.Show(DriverPhrases.SaveTemplateConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        return SaveChanges(false);
                    case DialogResult.No:
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }


        private void FrmDevTemplate_Load(object sender, EventArgs e)
        {
            // перевод формы
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlElemGroup, ctrlElemGroup.GetType().FullName);
            FormTranslator.Translate(ctrlElem, ctrlElem.GetType().FullName);
            FormTranslator.Translate(ctrlCmd, ctrlCmd.GetType().FullName);
            openFileDialog.SetFilter(CommonPhrases.XmlFileFilter);
            saveFileDialog.SetFilter(CommonPhrases.XmlFileFilter);
            TranslateTree();

            // настройка элементов управления
            TakeTreeViewImages();
            openFileDialog.InitialDirectory = appDirs.ConfigDir;
            saveFileDialog.InitialDirectory = appDirs.ConfigDir;
            btnEditOptionsExt.Visible = customUi.ExtendedOptionsAvailable;
            ctrlElem.Top = ctrlCmd.Top = ctrlElemGroup.Top;

            if (SaveOnly)
            {
                btnNew.Visible = false;
                btnOpen.Visible = false;
            }

            if (string.IsNullOrEmpty(FileName))
            {
                saveFileDialog.FileName = NewFileName;
                template = customUi.CreateDeviceTemplate();
                FillTree();
            }
            else
            {
                saveFileDialog.FileName = FileName;
                LoadTemplate(FileName);
            }
        }

        private void FrmDevTemplate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CheckChanges();
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            // создание шаблона устройства
            if (CheckChanges())
            {
                saveFileDialog.FileName = NewFileName;
                template = customUi.CreateDeviceTemplate();
                FileName = "";
                SetFormTitle();
                FillTree();
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // открытие шаблона устройства из файла
            if (CheckChanges())
            {
                openFileDialog.FileName = "";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    saveFileDialog.FileName = openFileDialog.FileName;
                    LoadTemplate(openFileDialog.FileName);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // сохранение шаблона устройства в файл
            SaveChanges(sender == btnSaveAs);
        }


        private void btnAddElemGroup_Click(object sender, EventArgs e)
        {
            // создание группы элементов и добавление в шаблон устройства
            ElemGroupConfig elemGroup = template.CreateElemGroupConfig();
            elemGroup.Elems.Add(elemGroup.CreateElemConfig());
            int ind = selNode != null && selNode.Tag is ElemGroup ? selNode.Index + 1 : template.ElemGroups.Count;
            template.ElemGroups.Insert(ind, elemGroup);

            // создание узла дерева группы элементов
            TreeNode grNode = NewElemGroupNode(elemGroup);
            grsNode.Nodes.Insert(ind, grNode);
            UpdateSignals(grNode);
            grNode.Expand();
            treeView.SelectedNode = grNode;
            ctrlElemGroup.SetFocus();

            // установка признака изменения
            Modified = true;
        }

        private void btnAddElem_Click(object sender, EventArgs e)
        {
            // создание элемента и добавление в шаблон устройства
            ElemGroupConfig elemGroup = selElemGroup == null ? selElemInfo.ElemGroup : selElemGroup;
            int maxElemCnt = elemGroup.MaxElemCnt;

            if (elemGroup.Elems.Count >= maxElemCnt)
            {
                MessageBox.Show(string.Format(DriverPhrases.ElemCntExceeded, maxElemCnt), 
                    CommonPhrases.WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ElemConfig elem = elemGroup.CreateElemConfig();
            elem.ElemType = elemGroup.DefaultElemType;
            int ind = selNode.Tag is ElemTag ? selNode.Index + 1 : elemGroup.Elems.Count;
            elemGroup.Elems.Insert(ind, elem);

            // создание узла дерева элемента
            TreeNode elemNode = NewElemNode(new ElemTag(template.Options, elemGroup, elem));
            TreeNode grNode = selNode.Tag is ElemTag ? selNode.Parent : selNode;
            grNode.Nodes.Insert(ind, elemNode);
            UpdateElemNodes(grNode);
            UpdateSignals(grNode);
            treeView.SelectedNode = elemNode;
            ctrlElem.SetFocus();

            // установка признака изменения
            Modified = true;
        }

        private void btnAddCmd_Click(object sender, EventArgs e)
        {
            // создание команды и добавление в шаблон устройства
            CmdConfig modbusCmd = template.CreateCmdConfig();
            int ind = selNode != null && selNode.Tag is ModbusCmd ? selNode.Index + 1 : template.Cmds.Count;
            template.Cmds.Insert(ind, modbusCmd);

            // создание узла дерева команды
            TreeNode cmdNode = NewCmdNode(modbusCmd);
            cmdsNode.Nodes.Insert(ind, cmdNode);
            treeView.SelectedNode = cmdNode;
            ctrlCmd.SetFocus();

            // установка признака изменения
            Modified = true;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            // перемещение объекта вверх
            TreeNode prevNode = selNode.PrevNode;
            int prevInd = prevNode.Index;

            if (selElemGroup != null)
            {
                // перемещение группы элементов вверх
                ElemGroupConfig prevElemGroup = prevNode.Tag as ElemGroupConfig;

                template.ElemGroups.RemoveAt(prevInd);
                template.ElemGroups.Insert(prevInd + 1, prevElemGroup);

                grsNode.Nodes.RemoveAt(prevInd);
                grsNode.Nodes.Insert(prevInd + 1, prevNode);

                UpdateSignals(selNode);
            }
            else if (selElemInfo != null)
            {
                // перемещение элемента вверх
                ElemTag prevElemInfo = prevNode.Tag as ElemTag;

                selElemInfo.ElemGroup.Elems.RemoveAt(prevInd);
                selElemInfo.ElemGroup.Elems.Insert(prevInd + 1, prevElemInfo.Elem);

                TreeNode grNode = selNode.Parent;
                grNode.Nodes.RemoveAt(prevInd);
                grNode.Nodes.Insert(prevInd + 1, prevNode);

                UpdateElemNodes(grNode);
                ShowElemProps(selElemInfo);
            }
            else if (selCmd != null)
            {
                // перемещение команды вверх
                CmdConfig prevCmd = prevNode.Tag as CmdConfig;

                template.Cmds.RemoveAt(prevInd);
                template.Cmds.Insert(prevInd + 1, prevCmd);

                cmdsNode.Nodes.RemoveAt(prevInd);
                cmdsNode.Nodes.Insert(prevInd + 1, prevNode);
            }

            // установка доступности кнопок
            btnMoveUp.Enabled = selNode.PrevNode != null;
            btnMoveDown.Enabled = selNode.NextNode != null;

            // установка признака изменения
            Modified = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            // перемещение объекта вниз
            TreeNode nextNode = selNode.NextNode;
            int nextInd = nextNode.Index;

            if (selElemGroup != null)
            {
                // перемещение группы элементов вниз
                ElemGroupConfig nextElemGroup = nextNode.Tag as ElemGroupConfig;

                template.ElemGroups.RemoveAt(nextInd);
                template.ElemGroups.Insert(nextInd - 1, nextElemGroup);

                grsNode.Nodes.RemoveAt(nextInd);
                grsNode.Nodes.Insert(nextInd - 1, nextNode);

                UpdateSignals(nextNode);
            }
            else if (selElemInfo != null)
            {
                // перемещение элемента вниз
                ElemTag nextElemInfo = nextNode.Tag as ElemTag;

                selElemInfo.ElemGroup.Elems.RemoveAt(nextInd);
                selElemInfo.ElemGroup.Elems.Insert(nextInd - 1, nextElemInfo.Elem);

                TreeNode grNode = selNode.Parent;
                grNode.Nodes.RemoveAt(nextInd);
                grNode.Nodes.Insert(nextInd - 1, nextNode);

                UpdateElemNodes(grNode);
                ShowElemProps(selElemInfo);
            }
            else if (selCmd != null)
            {
                // перемещение команды вниз
                CmdConfig nextCmd = nextNode.Tag as CmdConfig;

                template.Cmds.RemoveAt(nextInd);
                template.Cmds.Insert(nextInd - 1, nextCmd);

                cmdsNode.Nodes.RemoveAt(nextInd);
                cmdsNode.Nodes.Insert(nextInd - 1, nextNode);
            }

            // установка доступности кнопок
            btnMoveUp.Enabled = selNode.PrevNode != null;
            btnMoveDown.Enabled = selNode.NextNode != null;

            // установка признака изменения
            Modified = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selElemGroup != null)
            {
                // удаление группы элементов
                template.ElemGroups.Remove(selElemGroup);
                grsNode.Nodes.Remove(selNode);
            }
            else if (selElemInfo != null)
            {
                // удаление элемента
                ElemGroupConfig elemGroup = selElemInfo.ElemGroup;
                elemGroup.Elems.Remove(selElemInfo.Elem);
                TreeNode grNode = selNode.Parent;
                grsNode.Nodes.Remove(selNode);
                
                UpdateElemNodes(grNode);
                UpdateSignals(grNode);
                ShowElemProps(selElemInfo);
            }
            else if (selCmd != null)
            {
                // удаление команды
                template.Cmds.Remove(selCmd);
                cmdsNode.Nodes.Remove(selNode);
            }

            // установка признака изменения
            Modified = true;
        }

        private void btnEditSettings_Click(object sender, EventArgs e)
        {
            // редактирование настроек шаблона
            FrmTemplateOptions frmTemplateOptions = new(template.Options);

            if (frmTemplateOptions.ShowDialog() == DialogResult.OK)
            {
                // полное обновление дерева
                FillTree();
                // установка признака изменения
                Modified = true;
            }
        }

        private void btnEditSettingsExt_Click(object sender, EventArgs e)
        {
            // редактирование расширенных настроек
            if (customUi.ShowExtendedOptions(template))
            {
                FillTree();
                Modified = true;
            }
        }


        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // отображение выбранного объекта и его свойств
            selNode = e.Node;
            object tag = selNode.Tag;
            selElemGroup = tag as ElemGroupConfig;
            selElemInfo = tag as ElemTag;
            selCmd = tag as CmdConfig;

            if (selElemGroup != null)
                ShowElemGroupProps(selElemGroup);
            else if (selElemInfo != null)
                ShowElemProps(selElemInfo);
            else if (selCmd != null)
                ShowCmdProps(selCmd);
            else if (selNode == grsNode)
                ShowElemGroupProps(null);
            else if (selNode == cmdsNode)
                ShowCmdProps(null);
            else // не выполняется
                DisableProps();

            // установка доступности кнопок
            btnAddElem.Enabled = selElemGroup != null || selElemInfo != null;
            bool nodeIsOk = selElemGroup != null || selCmd != null ||
                selElemInfo != null && selElemInfo.ElemGroup.Elems.Count > 1 /*последний не удалять*/;
            btnMoveUp.Enabled = nodeIsOk && selNode.PrevNode != null;
            btnMoveDown.Enabled = nodeIsOk && selNode.NextNode != null;
            btnDelete.Enabled = nodeIsOk;
        }

        private void ctrlElemGroup_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            // установка признака изменения конфигурации
            Modified = true;

            // отображение изменений группы элементов в дереве
            TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                UpdateElemGroupNode();

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.ChildNodes))
                UpdateElemNodes();

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.ChildCount))
            {
                treeView.BeginUpdate();
                int oldElemCnt = selNode.Nodes.Count;
                int newElemCnt = selElemGroup.Elems.Count;

                if (oldElemCnt < newElemCnt)
                {
                    // добавление узлов дерева для новых элементов
                    int elemAddr = selElemGroup.Address;

                    for (int elemInd = 0; elemInd < newElemCnt; elemInd++)
                    {
                        ElemConfig elem = selElemGroup.Elems[elemInd];

                        if (elemInd >= oldElemCnt)
                        {
                            selNode.Nodes.Add(NewElemNode(new ElemTag(template.Options, selElemGroup, elem)
                            {
                                Address = elemAddr
                            }));
                        }

                        elemAddr += (ushort)elem.Quantity;
                    }
                }
                else if (oldElemCnt > newElemCnt)
                {
                    // удаление лишних узлов дерева
                    for (int i = newElemCnt; i < oldElemCnt; i++)
                    {
                        selNode.Nodes.RemoveAt(newElemCnt);
                    }
                }

                UpdateSignals(selNode);
                treeView.EndUpdate();
            }
        }

        private void ctrlElem_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            // установка признака изменения конфигурации
            Modified = true;

            // отображение изменений элемента в дереве
            TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                selNode.Text = selElemInfo.NodeText;

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.NextSiblings))
                UpdateElemNodes(selNode.Parent);
        }

        private void ctrlCmd_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            // установка признака изменения конфигурации
            Modified = true;

            // отображение изменений команды в дереве
            TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                UpdateCmdNode();
        }
    }
}
