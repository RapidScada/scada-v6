/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Administrator
 * Summary  : Specifies data kinds that a column can contain
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2021
 */

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Specifies data kinds that a column can contain.
    /// <para>Задает разновидности данных, которые столбец может содержать.</para>
    /// </summary>
    internal enum ColumnKind
    {
        Unspecified,
        BitMask,
        Button,
        Color,
        MultilineText,
        Password,
        Path,
        PrimaryKey,
        SelectFileButton,
        SelectFolderButton
    }
}
