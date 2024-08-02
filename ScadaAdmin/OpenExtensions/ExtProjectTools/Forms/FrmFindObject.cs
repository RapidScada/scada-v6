// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtProjectTools.Code;
using Scada.Data.Entities;
using Scada.Forms;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents a form for finding objects.
    /// <para>Представляет форму для поиска объектов.</para>
    /// </summary>
    public partial class FrmFindObject : Form
    {
        /// <summary>
        /// Represents an item to be found.
        /// </summary>
        private class SearchItem
        {
            public required TreeNode TreeNode { get; init; }
            public required Obj Obj { get; init; }
        }

        private readonly TreeView treeView;         // the tree into which the search is performed
        private List<SearchItem> searchItems;       // the linear list for search
        private Dictionary<int, int> searchIndexes; // contains the search item indexes accessed by object number
        private int startSearchIndex;               // the starting position of the search
        private bool foundSomething;                // at least one result found


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmFindObject()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmFindObject(TreeView treeView)
            : this()
        {
            this.treeView = treeView ?? throw new ArgumentNullException(nameof(treeView));
            ResetSearchCache();
            FormTranslator.Translate(this, GetType().FullName);
        }


        /// <summary>
        /// Finds the next match.
        /// </summary>
        private void FindNext()
        {
            InitSearchItems();

            if (searchItems.Count == 0 || treeView.Nodes.Count == 0)
            {
                ScadaUiUtils.ShowInfo(ExtensionPhrases.ObjectNotFound);
                return;
            }

            TreeNode currentNode = treeView.SelectedNode ?? treeView.Nodes[0];
            int currentObjNum = currentNode.Tag is Obj obj ? obj.ObjNum : 0;
            int searchIndex = searchIndexes.TryGetValue(currentObjNum, out int index) ? index : -1;
            bool found = false;
            bool endReached = false;

            if (++searchIndex >= searchItems.Count)
                searchIndex = 0;

            if (startSearchIndex < 0)
                startSearchIndex = searchIndex;

            do
            {
                SearchItem searchItem = searchItems[searchIndex];

                if (IsMatched(searchItem.Obj))
                {
                    found = true;
                    foundSomething = true;
                    treeView.SelectedNode = searchItem.TreeNode;
                }

                if (++searchIndex >= searchItems.Count)
                    searchIndex = 0;

                if (searchIndex == startSearchIndex)
                    endReached = true;

            } while (!found && !endReached);

            if (!found || endReached)
            {
                ScadaUiUtils.ShowInfo(foundSomething
                    ? ExtensionPhrases.SearchCompleted 
                    : ExtensionPhrases.ObjectNotFound);
                ResetSearchPosition();
            }
        }

        /// <summary>
        /// Initializes the linear list for search.
        /// </summary>
        private void InitSearchItems()
        {
            if (searchItems == null)
            {
                searchItems = [];
                searchIndexes = [];
                int index = 0;

                foreach (TreeNode treeNode in treeView.Nodes.IterateNodes())
                {
                    if (treeNode.Tag is Obj obj)
                    {
                        searchItems.Add(new SearchItem
                        {
                            TreeNode = treeNode,
                            Obj = obj
                        });
                        searchIndexes[obj.ObjNum] = index;
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Checks that the object is matched the search condition.
        /// </summary>
        private bool IsMatched(Obj obj)
        {
            if (obj == null)
                return false;

            string filter = txtFind.Text;
            bool ignoreCase = !chkCaseSensitive.Checked;
            bool wholeStringOnly = chkWholeStringOnly.Checked;

            if (chkObjNum.Checked && IsMatched(obj.ObjNum.ToString(), filter, ignoreCase, wholeStringOnly))
                return true;

            if (chkName.Checked && IsMatched(obj.Name, filter, ignoreCase, wholeStringOnly))
                return true;

            if (chkCode.Checked && IsMatched(obj.Code, filter, ignoreCase, wholeStringOnly))
                return true;

            if (chkDescr.Checked && IsMatched(obj.Descr, filter, ignoreCase, wholeStringOnly))
                return true;

            return false;
        }

        /// <summary>
        /// Checks that the field is matched the search condition.
        /// </summary>
        private static bool IsMatched(string field, string filter, bool ignoreCase, bool wholeStringOnly)
        {
            if (wholeStringOnly)
            {
                return string.Compare(field, filter, ignoreCase) == 0;
            }
            else
            {
                return field != null && field.Contains(filter, ignoreCase
                    ? StringComparison.CurrentCultureIgnoreCase
                    : StringComparison.CurrentCulture);
            }
        }

        /// <summary>
        /// Resets the search position.
        /// </summary>
        private void ResetSearchPosition()
        {

            startSearchIndex = -1;
            foundSomething = false;
        }

        /// <summary>
        /// Resets the search cache.
        /// </summary>
        public void ResetSearchCache()
        {
            searchItems = null;
            searchIndexes = null;
            ResetSearchPosition();
        }


        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            btnFindNext.Enabled = txtFind.Text != "";
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
