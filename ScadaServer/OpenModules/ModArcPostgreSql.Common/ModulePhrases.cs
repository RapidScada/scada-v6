// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Server.Modules.ModArcPostgreSql
{
    /// <summary>
    /// The phrases used by the module.
    /// <para>Фразы, используемые модулем.</para>
    /// </summary>
    public static class ModulePhrases
    {
        static ModulePhrases()
        {
            if (Locale.IsRussian)
            {
                CreationPartitionCompleted = "Проверка существования и создание секции {0} завершено за {1} мс";
                CreatePartitionError = "Ошибка при создании секции";
                PartitionDeleted = "Секция {0} удалена за {1} мс";
            }
            else
            {
                CreationPartitionCompleted = "Checking existence and creation of partition {0} completed in {1} ms";
                CreatePartitionError = "Error creating partition";
                PartitionDeleted = "Partition {0} deleted in {1} ms";
            }
        }

        public static string CreationPartitionCompleted { get; private set; }
        public static string CreatePartitionError { get; private set; }
        public static string PartitionDeleted { get; private set; }
    }
}
