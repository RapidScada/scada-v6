// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Web.Api;
using Scada.Web.Authorization;
using Scada.Web.Components;
using Scada.Web.Services;
using Scada.Web.Users;
using System;
using System.Collections.Generic;

namespace Scada.Web.Pages
{
    /// <summary>
    /// Represents a page for selecting channels of the configuration database.
    /// <para>Представляет страницу для выбора каналов базы конфигурации.</para>
    /// </summary>
    [Authorize(Policy = PolicyName.Restricted)]
    public class CnlSelectModel : PageModel
    {
        /// <summary>
        /// Represents an item that corresponds to a selectable channel.
        /// </summary>
        public class ChannelItem
        {
            public bool Selected { get; set; }
            public Cnl Cnl { get; set; }
        }

        private readonly IWebContext webContext;
        private readonly IUserContext userContext;
        private readonly dynamic dict;


        public CnlSelectModel(IWebContext webContext, IUserContext userContext)
        {
            this.webContext = webContext;
            this.userContext = userContext;
            dict = Locale.GetDictionary("Scada.Web.Pages.CnlSelect");
        }


        public ModalPostbackArgs PostbackArgs { get; private set; } = null;
        public List<SelectListItem> ObjList { get; private set; } = new();
        public PaginatedList<ChannelItem> ChannelItems { get; private set; } = new();
        public bool FilterIsEmpty => !(ObjNum > 0 || OnlySelected);

        [BindProperty]
        public int ObjNum { get; set; }
        [BindProperty]
        public bool OnlySelected { get; set; }
        [BindProperty]
        public string SelectedCnlNums { get; set; }
        [BindProperty]
        public int PageIndex { get; set; }


        private void FillObjList()
        {
            ObjList.Add(new SelectListItem(dict.SelectObjectItem, "0"));

            foreach (ObjectItem objectItem in userContext.Objects)
            {
                ObjList.Add(new SelectListItem(
                    objectItem.Text, 
                    objectItem.ObjNum.ToString()));
            }
        }

        private void FillChannelItems()
        {
            List<ChannelItem> allChannelItems = new();

            if (ObjNum > 0)
            {
                if (userContext.Rights.GetRightByObj(ObjNum).View)
                {
                    // select channels by object number
                    HashSet<int> selectedCnlNums = ScadaUtils.ParseIntSet(SelectedCnlNums);

                    foreach (Cnl cnl in webContext.ConfigDatabase.CnlTable.Select(new TableFilter("ObjNum", ObjNum), true))
                    {
                        allChannelItems.Add(new ChannelItem
                        {
                            Selected = selectedCnlNums.Contains(cnl.CnlNum),
                            Cnl = cnl
                        });
                    }
                }
            }
            else if (OnlySelected)
            {
                // get selected channels
                int[] selectedCnlNums = ScadaUtils.ParseIntArray(SelectedCnlNums);
                Array.Sort(selectedCnlNums);

                foreach (int cnlNum in selectedCnlNums)
                {
                    if (webContext.ConfigDatabase.CnlTable.GetItem(cnlNum) is Cnl cnl &&
                        userContext.Rights.GetRightByObj(cnl.ObjNum).View)
                    {
                        allChannelItems.Add(new ChannelItem
                        {
                            Selected = true,
                            Cnl = cnl
                        });
                    }
                }
            }

            ChannelItems.Fill(allChannelItems, PageIndex);
        }


        public void OnGet(IdList cnlNums)
        {
            ObjNum = 0;
            OnlySelected = true;
            SelectedCnlNums = cnlNums.ToLongString();

            FillObjList();
            FillChannelItems();
        }

        public void OnPost()
        {
            FillObjList();
            FillChannelItems();
            PostbackArgs = new ModalPostbackArgs { UpdateHeight = true };
        }

        public void OnPostSelect()
        {
            // validate selected channel numbers
            SortedSet<int> selectedCnlNums = new();

            foreach (int cnlNum in ScadaUtils.ParseIntArray(SelectedCnlNums))
            {
                if (webContext.ConfigDatabase.CnlTable.GetItem(cnlNum) is Cnl cnl &&
                    userContext.Rights.GetRightByObj(cnl.ObjNum).View)
                {
                    selectedCnlNums.Add(cnlNum);
                }
            }

            // create modal dialog result
            PostbackArgs = new ModalPostbackArgs 
            { 
                CloseModal = true,
                ModalResult = new { CnlNums = selectedCnlNums.ToLongString() }
            };
        }
    }
}
