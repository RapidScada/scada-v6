// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Admin.Extensions.ExtCommConfig.Code
{
    /// <summary>
    /// Specifies the types of Communicator tree nodes.
    /// <para>Задаёт типы узлов Коммуникатора.</para>
    /// </summary>
    public static class CommNodeType
    {
        public const string GeneralOptions = nameof(GeneralOptions);
        public const string Drivers = nameof(Drivers);
        public const string DataSources = nameof(DataSources);
        public const string Lines = nameof(Lines);
        public const string Line = nameof(Line);
        public const string LineOptions = nameof(LineOptions);
        public const string LineStats = nameof(LineStats);
        public const string Device = nameof(Device);
        public const string Logs = nameof(Logs);
    }
}
