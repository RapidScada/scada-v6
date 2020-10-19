/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Summary  : The phrases used by the server and its modules
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Server
{
    /// <summary>
    /// The phrases used by the server and its modules.
    /// <para>Фразы, используемые сервером и его модулями.</para>
    /// </summary>
    public static class ServerPhrases
    {
        // Archives
        public static string DeleteOutdatedData { get; private set; }
        public static string ReadingTrendsCompleted { get; private set; }
        public static string ReadingTrendCompleted { get; private set; }
        public static string ReadingTimestampsCompleted { get; private set; }
        public static string ReadingSliceCompleted { get; private set; }
        public static string WritingSliceCompleted { get; private set; }
        public static string UpdateCompleted { get; private set; }
        public static string ReadingEventsCompleted { get; private set; }
        public static string ReadingEventCompleted { get; private set; }
        public static string WritingEventCompleted { get; private set; }
        public static string AckEventCompleted { get; private set; }

        public static void Init()
        {
            // load phrases from dictionaries

            // set phrases depending on locale
            if (Locale.IsRussian)
            {
                DeleteOutdatedData = "Удаление устаревших данных из архива {0}, которые старше {1}";
                ReadingTrendsCompleted = "Чтение трендов длины {0} успешно завершено за {1} мс";
                ReadingTrendCompleted = "Чтение тренда длины {0} успешно завершено за {1} мс";
                ReadingTimestampsCompleted = "Чтение меток времени длины {0} успешно завершено за {1} мс";
                ReadingSliceCompleted = "Чтение среза длины {0} успешно завершено за {1} мс";
                WritingSliceCompleted = "Запись среза длины {0} успешно завершена за {1} мс";
                UpdateCompleted = "Обновление данных успешно завершено за {0} мс";
                ReadingEventsCompleted = "Чтение событий в количестве {0} успешно завершено за {1} мс";
                ReadingEventCompleted = "Чтение события успешно завершено за {0} мс";
                WritingEventCompleted = "Запись события успешно завершена за {0} мс";
                AckEventCompleted = "Квитирование события успешно завершено за {0} мс";
            }
            else
            {
                DeleteOutdatedData = "Delete outdated data from the {0} archive older than {1}";
                ReadingTrendsCompleted = "Reading trends of length {0} completed successfully in {1} ms";
                ReadingTrendCompleted = "Reading a trend of length {0} completed successfully in {1} ms";
                ReadingTimestampsCompleted = "Reading timestamps of length {0} completed successfully in {1} ms";
                ReadingSliceCompleted = "Reading a slice of length {0} completed successfully in {1} ms";
                WritingSliceCompleted = "Writing a slice of length {0} completed successfully in {1} ms";
                UpdateCompleted = "Data update completed successfully in {0} ms";
                ReadingEventsCompleted = "Reading events in the amount of {0} completed successfully in {1} ms";
                ReadingEventCompleted = "Reading an event completed successfully in {0} ms";
                WritingEventCompleted = "Event writing completed successfully in {0} ms";
                AckEventCompleted = "Event acknowledgment completed successfully in {0} ms";
            }
        }
    }
}
