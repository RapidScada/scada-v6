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
 * Module   : ScadaServerCommon
 * Summary  : The phrases used by Server and its modules
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Scada.Lang;

namespace Scada.Server.Lang
{
    /// <summary>
    /// The phrases used by Server and its modules.
    /// <para>Фразы, используемые Сервером и его модулями.</para>
    /// </summary>
    public static class ServerPhrases
    {
        // Engine
        public static string ErrorInArchive { get; private set; }
        public static string ErrorInModule { get; private set; }

        // Archives
        public static string ArchiveMessage { get; private set; }
        public static string NullResultNotAllowed { get; private set; }
        public static string InvalidWritingPeriod { get; private set; }
        public static string ReadOnlyNotSupported { get; private set; }
        public static string WritingOnChangeNotSupported { get; private set; }
        public static string WritingOnChangeIsSlow { get; private set; }
        public static string DeleteOutdatedData { get; private set; }
        public static string ReadingWriteTimeCompleted { get; private set; }
        public static string ReadingTrendsCompleted { get; private set; }
        public static string ReadingTrendCompleted { get; private set; }
        public static string ReadingTimestampsCompleted { get; private set; }
        public static string ReadingSliceCompleted { get; private set; }
        public static string ReadingPointsCompleted { get; private set; }
        public static string WritingSliceCompleted { get; private set; }
        public static string WritingPointsCompleted { get; private set; }
        public static string QueueingPointsCompleted { get; private set; }
        public static string QueueBecameEmpty { get; private set; }
        public static string PointsLost { get; private set; }
        public static string UpdateCompleted { get; private set; }
        public static string ReadingEventsCompleted { get; private set; }
        public static string ReadingEventCompleted { get; private set; }
        public static string WritingEventCompleted { get; private set; }
        public static string QueueingEventCompleted { get; private set; }
        public static string EventLost { get; private set; }
        public static string AckEventCompleted { get; private set; }
        public static string AckEventNotFound { get; private set; }

        // Modules
        public static string StartModule { get; private set; }
        public static string StopModule { get; private set; }
        public static string ModuleMessage { get; private set; }
        public static string ModuleStateLoaded { get; private set; }
        public static string ModuleStateNotExists { get; private set; }
        public static string LoadModuleStateError { get; private set; }
        public static string SaveModuleStateError { get; private set; }
        public static string ReadDbError { get; private set; }
        public static string WriteDbError { get; private set; }
        public static string ReadFileError { get; private set; }
        public static string WriteFileError { get; private set; }

        // Scada.Server.Archives
        public static string UnspecifiedArchiveKind { get; private set; }
        public static string CurrentArchiveKind { get; private set; }
        public static string HistoricalArchiveKind { get; private set; }
        public static string EventsArchiveKind { get; private set; }

        // Scada.Server.Modules
        public static string LoadModuleConfigError { get; private set; }
        public static string SaveModuleConfigError { get; private set; }
        public static string SaveModuleConfigConfirm { get; private set; }

        // Scada.Server.Engine.CoreLogic
        public static string CommandSentBy { get; private set; }
        public static string EmptyCredentials { get; private set; }
        public static string InvalidCredentials { get; private set; }
        public static string AccountDisabled { get; private set; }

        public static void Init()
        {
            // set phrases that are used in the bilingual service logic, depending on the locale
            if (Locale.IsRussian)
            {
                ErrorInArchive = "Ошибка при вызове метода {0} архива {1}";
                ErrorInModule = "Ошибка при вызове метода {0} модуля {1}";

                ArchiveMessage = "Архив {0}: {1}";
                NullResultNotAllowed = "Результат метода не может быть null.";
                InvalidWritingPeriod = "Период записи должен быть положительным.";
                ReadOnlyNotSupported = "Архив не может быть только для чтения.";
                WritingOnChangeNotSupported = "Архив не поддерживает запись данных по изменению.";
                WritingOnChangeIsSlow = "Запись данных по изменению может снизить производительность";
                DeleteOutdatedData = "Удаление устаревших данных из архива {0}, которые старше {1}";
                ReadingWriteTimeCompleted = "Чтение времени последней записи успешно завершено за {0} мс";
                ReadingTrendsCompleted = "Чтение трендов длины {0} успешно завершено за {1} мс";
                ReadingTrendCompleted = "Чтение тренда длины {0} успешно завершено за {1} мс";
                ReadingTimestampsCompleted = "Чтение меток времени длины {0} успешно завершено за {1} мс";
                ReadingSliceCompleted = "Чтение среза длины {0} успешно завершено за {1} мс";
                ReadingPointsCompleted = "Чтение {0} точек данных успешно завершено за {1} мс";
                WritingSliceCompleted = "Запись среза длины {0} успешно завершена за {1} мс";
                WritingPointsCompleted = "Запись {0} точек данных успешно завершена за {1} мс";
                QueueingPointsCompleted = "Постановка в очередь {0} точек данных успешно завершена за {1} мс";
                QueueBecameEmpty = "Очередь данных стала пустой";
                PointsLost = "{0} точек данных были потеряны";
                UpdateCompleted = "Обновление {0} точек данных успешно завершено за {1} мс";
                ReadingEventsCompleted = "Чтение {0} событий успешно завершено за {1} мс";
                ReadingEventCompleted = "Чтение события успешно завершено за {0} мс";
                WritingEventCompleted = "Запись события успешно завершена за {0} мс";
                QueueingEventCompleted = "Постановка события в очередь успешно завершена за {0} мс";
                EventLost = "Событие было потеряно";
                AckEventCompleted = "Квитирование события с ид. {0} успешно завершено за {1} мс";
                AckEventNotFound = "Квитируемое событие с ид. {0} не найдено";

                StartModule = "Модуль {0} {1} запущен";
                StopModule = "Модуль {0} остановлен";
                ModuleMessage = "Модуль {0}: {1}";
                ModuleStateLoaded = "Состояние модуля загружено";
                ModuleStateNotExists = "Файл состояния модуля отсутствует или устарел";
                LoadModuleStateError = "Ошибка при загрузке состояния модуля";
                SaveModuleStateError = "Ошибка при сохранении состояния модуля";
                ReadDbError = "Ошибка при чтении из базы данных";
                WriteDbError = "Ошибка при записи в базу данных";
                ReadFileError = "Ошибка при чтении из файла";
                WriteFileError = "Ошибка при записи в файл";
            }
            else
            {
                ErrorInArchive = "Error calling the {0} method of the {1} archive";
                ErrorInModule = "Error calling the {0} method of the {1} module";

                ArchiveMessage = "Archive {0}: {1}";
                NullResultNotAllowed = "Method result must not be null.";
                InvalidWritingPeriod = "Writing period must be positive.";
                ReadOnlyNotSupported = "Archive cannot be read only.";
                WritingOnChangeNotSupported = "Archive does not support writing data on change.";
                WritingOnChangeIsSlow = "Writing data on change can degrade performance";
                DeleteOutdatedData = "Delete outdated data from the {0} archive older than {1}";
                ReadingWriteTimeCompleted = "Reading last write time completed successfully in {0} ms";
                ReadingTrendsCompleted = "Reading trends of length {0} completed successfully in {1} ms";
                ReadingTrendCompleted = "Reading a trend of length {0} completed successfully in {1} ms";
                ReadingTimestampsCompleted = "Reading timestamps of length {0} completed successfully in {1} ms";
                ReadingSliceCompleted = "Reading a slice of length {0} completed successfully in {1} ms";
                ReadingPointsCompleted = "Reading of {0} data points completed successfully in {1} ms";
                WritingSliceCompleted = "Writing a slice of length {0} completed successfully in {1} ms";
                WritingPointsCompleted = "Writing of {0} data points completed successfully in {1} ms";
                QueueingPointsCompleted = "Enqueueing of {0} data points completed successfully in {1} ms";
                QueueBecameEmpty = "Data queue has become empty";
                PointsLost = "{0} data points were lost";
                UpdateCompleted = "Update of {0} data points completed successfully in {1} ms";
                ReadingEventsCompleted = "Reading of {0} events completed successfully in {1} ms";
                ReadingEventCompleted = "Reading an event completed successfully in {0} ms";
                WritingEventCompleted = "Event writing completed successfully in {0} ms";
                QueueingEventCompleted = "Enqueueing an event completed successfully in {0} ms";
                EventLost = "Event was lost";
                AckEventCompleted = "Acknowledging an event with ID {0} completed successfully in {1} ms";
                AckEventNotFound = "Acknowledged event with ID {0} not found";

                StartModule = "Module {0} {1} started";
                StopModule = "Module {0} is stopped";
                ModuleMessage = "Module {0}: {1}";
                ModuleStateLoaded = "Module state loaded";
                ModuleStateNotExists = "Module state file is missing or outdated";
                LoadModuleStateError = "Error loading module state";
                SaveModuleStateError = "Error saving module state";
                ReadDbError = "Error reading from database";
                WriteDbError = "Error writing to database";
                ReadFileError = "Error reading from file";
                WriteFileError = "Error writing to file";
            }

            // load phrases that are used in the multilingual user interface from dictionaries
            LocaleDict dict = Locale.GetDictionary("Scada.Server.Archives");
            UnspecifiedArchiveKind = dict[nameof(UnspecifiedArchiveKind)];
            CurrentArchiveKind = dict[nameof(CurrentArchiveKind)];
            HistoricalArchiveKind = dict[nameof(HistoricalArchiveKind)];
            EventsArchiveKind = dict[nameof(EventsArchiveKind)];

            dict = Locale.GetDictionary("Scada.Server.Modules");
            LoadModuleConfigError = dict[nameof(LoadModuleConfigError)];
            SaveModuleConfigError = dict[nameof(SaveModuleConfigError)];
            SaveModuleConfigConfirm = dict[nameof(SaveModuleConfigConfirm)];

            dict = Locale.GetDictionary("Scada.Server.Engine.CoreLogic");
            CommandSentBy = dict[nameof(CommandSentBy)];
            EmptyCredentials = dict[nameof(EmptyCredentials)];
            InvalidCredentials = dict[nameof(InvalidCredentials)];
            AccountDisabled = dict[nameof(AccountDisabled)];
        }
    }
}
