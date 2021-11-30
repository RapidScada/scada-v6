// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Specifies the connection security policies.
    /// <para>Задает политики безопасности соединения.</para>
    /// </summary>
    public enum SecurityPolicy
    {
        None,
        Basic128Rsa15,
        Basic256,
        Basic256Sha256,
        Aes128_Sha256_RsaOaep,
        Aes256_Sha256_RsaPss,
        Https
    }
}
