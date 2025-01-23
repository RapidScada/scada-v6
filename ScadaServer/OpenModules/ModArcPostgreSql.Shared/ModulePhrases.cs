// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Server.Modules.ModArcPostgreSql
{
    /// <summary>
    /// The phrases used by the module.
    /// <para>Фразы, используемые модулем.</para>
    /// </summary>
    internal static class ModulePhrases
    {
        // Scada.Server.Modules.ModArcPostgreSql.Logic
        public static string CreationPartitionCompleted { get; private set; }
        public static string CreatePartitionError { get; private set; }
        public static string PartitionDeleted { get; private set; }
        public static string NoPartitionFound { get; private set; }

        static ModulePhrases()
        {
            if (Locale.IsRussian)
            {
                CreationPartitionCompleted = "Проверка существования и создание секции {0} завершено за {1} мс";
                CreatePartitionError = "Ошибка при создании секции";
                PartitionDeleted = "Секция {0} удалена за {1} мс";
                NoPartitionFound = "Не найдена секция для точки данных с меткой времени {0}, канал {1}";
            }
            else
            {
                CreationPartitionCompleted = "Checking existence and creation of partition {0} completed in {1} ms";
                CreatePartitionError = "Error creating partition";
                PartitionDeleted = "Partition {0} deleted in {1} ms";
                NoPartitionFound = "No partition found for data point with timestamp {0}, channel {1}";
            }
        }
    }
}
