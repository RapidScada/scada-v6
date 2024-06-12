// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Filters;
using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Represents a filter that locks the requested mimic while an action is executed.
    /// <para>Представляет фильтр, который блокирует запрошенную мнемосхему на время выполнения действия.</para>
    /// </summary>
    public class MimicLockFilter(EditorManager editorManager) : IActionFilter
    {
        private Mimic mimic = null;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments["key"] is long key &&
                editorManager.FindMimic(key, out MimicInstance mimicInstance, out _))
            {
                mimic = mimicInstance.Mimic;
                Monitor.Enter(mimic.SyncRoot);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (mimic != null)
                Monitor.Exit(mimic.SyncRoot);
        }
    }
}
