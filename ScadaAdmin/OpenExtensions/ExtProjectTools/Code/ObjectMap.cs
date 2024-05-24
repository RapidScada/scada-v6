// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;
using System.Text;

namespace Scada.Admin.Extensions.ExtProjectTools.Code
{
    /// <summary>
    /// Generates an object map of the configuration database.
    /// <para>Генерирует карту объектов базы конфигурации.</para>
    /// </summary>
    internal class ObjectMap
    {
        /// <summary>
        /// The file name of newly created maps.
        /// </summary>
        public const string MapFileName = "ScadaAdmin_ObjectMap.txt";

        private readonly ILog log;                      // the application log
        private readonly ConfigDatabase configDatabase; // the configuration database


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ObjectMap(ILog log, ConfigDatabase configDatabase)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
        }


        /// <summary>
        /// Writes objects recursively.
        /// </summary>
        private static void WriteChildObjects(StreamWriter writer, ITableIndex parentObjIndex,
            int parentObjNum, int level)
        {
            // infinite recursion is not possible for objects
            foreach (Obj childObj in parentObjIndex.SelectItems(parentObjNum))
            {
                if (level > 0)
                    writer.Write(new string('-', level * 2) + " ");

                writer.WriteLine(string.Format(CommonPhrases.EntityCaption, childObj.ObjNum, childObj.Name));
                WriteChildObjects(writer, parentObjIndex, childObj.ObjNum, level + 1);
            }
        }

        /// <summary>
        /// Generates an object map.
        /// </summary>
        public void Generate(string mapFileName)
        {
            try
            {
                using (StreamWriter writer = new(mapFileName, false, Encoding.UTF8))
                {
                    writer.WriteLine(ExtensionPhrases.ObjectMapTitle);
                    writer.WriteLine(new string('-', ExtensionPhrases.ObjectMapTitle.Length));

                    if (configDatabase.ObjTable.ItemCount == 0)
                        writer.WriteLine(ExtensionPhrases.NoObjects);
                    else if (configDatabase.ObjTable.TryGetIndex("ParentObjNum", out ITableIndex parentObjIndex))
                        WriteChildObjects(writer, parentObjIndex, 0, 0);
                    else
                        throw new ScadaException(CommonPhrases.IndexNotFound);
                }

                ScadaUiUtils.StartProcess(mapFileName);
            }
            catch (Exception ex)
            {
                log.HandleError(ex, ExtensionPhrases.GenerateObjectMapError);
            }
        }
    }
}
