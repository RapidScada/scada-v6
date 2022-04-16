// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.AB.Forms.Lang;
using Scada.AB.Forms.Properties;
using Scada.Forms;
using Scada.Lang;
using System.Collections;
using System.Net.Mail;

namespace Scada.AB.Forms
{
    /// <summary>
    /// Represents an address book form.
    /// <para>Представляет форму адресной книги.</para>
    /// </summary>
    public partial class FrmAddressBook : Form
    {
        /// <summary>
        /// Specifies the image keys.
        /// </summary>
        private static class ImageKey
        {
            public const string Book = "book.png";
            public const string Contact = "contact.png";
            public const string Email = "email.png";
            public const string FolderClosed = "folder_closed.png";
            public const string FolderOpen = "folder_open.png";
            public const string Phone = "phone.png";
        }

        private static bool dictLoaded = false;   // indicates that the address book dictionary is loaded

        private readonly AppDirs appDirs;         // the application directories
        private readonly AddressBook addressBook; // the address book being edited
        private readonly TreeNode rootNode;       // the root node of the tree view
        private readonly string fileName;         // the address book file name
        private bool modified;                    // indicates that the address book is modified


        /// <summary>
        /// Конструктор, ограничивающий создание формы без параметров
        /// </summary>
        private FrmAddressBook()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmAddressBook(AppDirs appDirs)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            addressBook = new AddressBook();
            rootNode = treeView.Nodes[0];
            rootNode.Tag = addressBook;
            fileName = Path.Combine(appDirs.ConfigDir, AddressBook.DefaultFileName);
            modified = false;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the address book is modified.
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
                btnSave.Enabled = modified;
            }
        }


        /// <summary>
        /// Loads dictionaries and applies localization to the form.
        /// </summary>
        private void LocalizeForm()
        {
            if (!dictLoaded)
            {
                if (Locale.LoadDictionaries(appDirs.LangDir, "AddressBook", out string errMsg))
                    dictLoaded = true;
                else
                    ScadaUiUtils.ShowError(errMsg);

                AddressBookPhrases.Init();
            }

            FormTranslator.Translate(this, GetType().FullName);
            rootNode.Text = AddressBookPhrases.AddressBookNode;
        }

        /// <summary>
        /// Takes the tree view and loads them into an image list.
        /// </summary>
        private void TakeTreeViewImages()
        {
            // loading images from resources instead of storing in image list prevents them from corruption
            imageList.Images.Add(ImageKey.Book, Resources.book);
            imageList.Images.Add(ImageKey.Contact, Resources.contact);
            imageList.Images.Add(ImageKey.Email, Resources.email);
            imageList.Images.Add(ImageKey.FolderClosed, Resources.folder_closed);
            imageList.Images.Add(ImageKey.FolderOpen, Resources.folder_open);
            imageList.Images.Add(ImageKey.Phone, Resources.phone);
        }

        /// <summary>
        /// Fills the tree view according to the address book content.
        /// </summary>
        private void FillTree()
        {
            try
            {
                treeView.BeginUpdate();
                rootNode.Nodes.Clear();

                foreach (ContactGroup contactGroup in addressBook.ContactGroups)
                {
                    rootNode.Nodes.Add(CreateContactGroupNode(contactGroup));
                }

                if (rootNode.Nodes.Count > 0)
                    treeView.SelectedNode = rootNode.Nodes[0];
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Creates a node that represents the specified contact group.
        /// </summary>
        private static TreeNode CreateContactGroupNode(ContactGroup contactGroup)
        {
            TreeNode contactGroupNode = TreeViewExtensions.CreateNode(contactGroup, ImageKey.FolderClosed);

            foreach (Contact contact in contactGroup.Contacts)
            {
                contactGroupNode.Nodes.Add(CreateContactNode(contact));
            }

            return contactGroupNode;
        }

        /// <summary>
        /// Creates a node that represents the specified contact.
        /// </summary>
        private static TreeNode CreateContactNode(Contact contact)
        {
            TreeNode contactNode = TreeViewExtensions.CreateNode(contact, ImageKey.Contact);

            foreach (AddressBookItem contactItem in contact.ContactItems)
            {
                if (contactItem is PhoneNumber phoneNumber)
                    contactNode.Nodes.Add(CreatePhoneNumberNode(phoneNumber));
                else if (contactItem is Email email)
                    contactNode.Nodes.Add(CreateEmailNode(email));
            }

            return contactNode;
        }

        /// <summary>
        /// Creates a node that represents the specified phone number.
        /// </summary>
        private static TreeNode CreatePhoneNumberNode(PhoneNumber phoneNumber)
        {
            return TreeViewExtensions.CreateNode(phoneNumber, ImageKey.Phone);
        }

        /// <summary>
        /// Creates a node that represents the specified email.
        /// </summary>
        private static TreeNode CreateEmailNode(Email email)
        {
            return TreeViewExtensions.CreateNode(email, ImageKey.Email);
        }

        /// <summary>
        /// Finds the index to insert an element keeping the list sort order.
        /// </summary>
        private static int FindInsertIndex<T>(List<T> list, int currentIndex, out bool duplicated)
        {
            if (list.Count <= 1)
            {
                duplicated = false;
                return currentIndex;
            }
            else
            {
                T item = list[currentIndex];

                list.RemoveAt(currentIndex);
                int newIndex = list.BinarySearch(item);
                list.Insert(currentIndex, item);

                if (newIndex >= 0)
                {
                    duplicated = true;
                    return newIndex;
                }
                else
                {
                    duplicated = false;
                    return ~newIndex;
                }
            }
        }

        /// <summary>
        /// Validates the email.
        /// </summary>
        private static bool ValidateEmail(string email)
        {
            return MailAddress.TryCreate(email, out _);
        }

        /// <summary>
        /// Enables or disables the buttons.
        /// </summary>
        private void SetButtonsEnabled()
        {
            if (treeView.SelectedNode?.Parent == null)
            {
                btnAddContact.Enabled = btnEdit.Enabled = btnDelete.Enabled =
                    btnAddPhoneNumber.Enabled = btnAddEmail.Enabled = false;
            }
            else
            {
                object selectedObject = treeView.SelectedNode.Tag;
                object parentObject = treeView.SelectedNode.Parent.Tag;

                btnAddContact.Enabled = btnEdit.Enabled = btnDelete.Enabled =
                    selectedObject is AddressBookItem;
                btnAddPhoneNumber.Enabled = btnAddEmail.Enabled =
                    selectedObject is Contact || parentObject is Contact;
            }
        }


        private void FrmAddressBook_Load(object sender, EventArgs e)
        {
            LocalizeForm();

            if (File.Exists(fileName) && !addressBook.Load(fileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            Modified = false;
            TakeTreeViewImages();
            FillTree();
            SetButtonsEnabled();
        }

        private void FrmAddressBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(AddressBookPhrases.SaveAddressBookConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!addressBook.Save(fileName, out string errMsg))
                        {
                            ScadaUiUtils.ShowError(errMsg);
                            e.Cancel = true;
                        }
                        break;

                    case DialogResult.No:
                        break;

                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }


        private void btnAddContactGroup_Click(object sender, EventArgs e)
        {
            // add contact group
            ContactGroup contactGroup = new(AddressBookPhrases.NewContactGroup);
            TreeNode contactGroupNode = CreateContactGroupNode(contactGroup);

            treeView.Add(rootNode, contactGroupNode);
            contactGroupNode.BeginEdit();
            Modified = true;
        }

        private void btnAddContact_Click(object sender, EventArgs e)
        {
            // add contact
            if (treeView.SelectedNode?.FindClosest(typeof(ContactGroup)) is TreeNode contactGroupNode)
            {
                Contact contact = new(AddressBookPhrases.NewContact);
                TreeNode contactNode = CreateContactNode(contact);

                treeView.Add(contactGroupNode, contactNode);
                contactNode.BeginEdit();
                Modified = true;
            }
        }

        private void btnAddPhoneNumber_Click(object sender, EventArgs e)
        {
            // add phone number
            if (treeView.SelectedNode?.FindClosest(typeof(Contact)) is TreeNode contactNode)
            {
                PhoneNumber phoneNumber = new(AddressBookPhrases.NewPhoneNumber);
                TreeNode phoneNumberNode = CreatePhoneNumberNode(phoneNumber);

                treeView.Add(contactNode, phoneNumberNode);
                phoneNumberNode.BeginEdit();
                Modified = true;
            }
        }

        private void btnAddEmail_Click(object sender, EventArgs e)
        {
            // add email
            if (treeView.SelectedNode?.FindClosest(typeof(Contact)) is TreeNode contactNode)
            {
                Email email = new(AddressBookPhrases.NewEmail);
                TreeNode emailNode = CreateEmailNode(email);

                treeView.Add(contactNode, emailNode);
                emailNode.BeginEdit();
                Modified = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // switch editing mode of the selected node
            if (treeView.SelectedNode is TreeNode selectedNode)
            {
                if (selectedNode.IsEditing)
                    selectedNode.EndEdit(false);
                else
                    selectedNode.BeginEdit();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // delete the selected item
            if (treeView.GetSelectedObject() is AddressBookItem)
            {
                treeView.RemoveSelectedNode();
                Modified = true;
            }
        }


        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetButtonsEnabled();
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // show open folder icon
            if (e.Node.Tag is ContactGroup)
                e.Node.SetImageKey(ImageKey.FolderOpen);
        }

        private void treeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            // show closed folder icon
            if (e.Node.Tag is ContactGroup)
                e.Node.SetImageKey(ImageKey.FolderClosed);
        }

        private void treeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // cancel editing the root node
            if (e.Node == rootNode)
                e.CancelEdit = true;
        }

        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // retrieve changes after node editing
            if (e.Label != null /*editing not canceled*/ &&
                e.Node.Tag as AddressBookItem is AddressBookItem bookItem)
            {
                string oldVal = bookItem.Value;
                string newVal = e.Label;

                if (newVal == "")
                {
                    e.CancelEdit = true;
                    ScadaUiUtils.ShowError(AddressBookPhrases.EmptyValueNotAllowed);
                    e.Node.BeginEdit();
                }
                else if (!oldVal.Equals(newVal, StringComparison.Ordinal))
                {
                    // set new item value
                    bookItem.Value = newVal;

                    // define insertion index and validate value
                    IList list = bookItem.Parent.Children;
                    int curInd = e.Node.Index;
                    int newInd = curInd;
                    bool duplicated;
                    string errMsg = "";

                    if (bookItem is ContactGroup)
                    {
                        newInd = FindInsertIndex((List<ContactGroup>)list, curInd, out duplicated);

                        if (duplicated)
                            errMsg = AddressBookPhrases.ContactGroupExists;
                    }
                    else if (bookItem is Contact)
                    {
                        newInd = FindInsertIndex((List<Contact>)list, curInd, out duplicated);

                        if (duplicated)
                            errMsg = AddressBookPhrases.ContactExists;
                    }
                    else if (bookItem is PhoneNumber || bookItem is Email)
                    {
                        newInd = FindInsertIndex((List<AddressBookItem>)list, curInd, out duplicated);

                        if (bookItem is PhoneNumber)
                        {
                            if (duplicated)
                                errMsg = AddressBookPhrases.PhoneNumberExists;
                        }
                        else
                        {
                            if (duplicated)
                                errMsg = AddressBookPhrases.EmailExists;

                            if (!ValidateEmail(newVal))
                                errMsg = AddressBookPhrases.IncorrectEmail;
                        }
                    }

                    if (errMsg != "")
                    {
                        // revert old value
                        bookItem.Value = newVal;
                        e.CancelEdit = true;
                        ScadaUiUtils.ShowError(errMsg);
                        e.Node.BeginEdit();
                    }
                    else if (newInd != curInd)
                    {
                        // move node to keep sort order
                        BeginInvoke(new Action(() => { treeView.MoveSelectedNode(newInd); }));
                    }

                    Modified = true;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (addressBook.Save(fileName, out string errMsg))
                Modified = false;
            else
                ScadaUiUtils.ShowError(errMsg);
        }
    }
}
