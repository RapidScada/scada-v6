// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;

namespace Scada.Server.Modules.ModDbExport.Logic.Replication
{
    /// <summary>
    /// Replicates archives to a target database.
    /// <para>Реплицирует архивы на целевую базу данных.</para>
    /// </summary>
    internal class ArcReplicator
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArcReplicator(IServerContext serverContext, IExporterContext exporterContext)
        {

        }


        /// <summary>
        /// Initializes the replicator.
        /// </summary>
        public void Init()
        {

        }

        /// <summary>
        /// Replicates data to a target database.
        /// </summary>
        public void ReplicateData()
        {

        }

        /// <summary>
        /// Enqueues the export task.
        /// </summary>
        public void EnqueueTask(ArcExportTask task)
        {
            /*if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (!replicatorIsReady)
            {
                gateContext.GateLog.WriteError(Locale.IsRussian ?
                    "Невозможно добавить задачу в очередь. Репликация не готова к работе" :
                    "Unable to add task to the queue. Replication is not ready for operating");
            }
            else if (task.Validate(out string errMsg))
            {
                lock (taskQueue)
                {
                    if (taskQueue.Count >= MaxTaskQueueSize)
                    {
                        gateContext.GateLog.WriteError(Locale.IsRussian ?
                            "Невозможно добавить задачу в очередь. Максимальный размер очереди превышен" :
                            "Unable to add task to the queue. The maximum size of the queue is exceeded");
                    }
                    else
                    {
                        task.State.PositionDT = CorrectPosition(task.StartDT, options.ReadingStep);
                        taskQueue.Enqueue(task);
                    }
                }
            }
            else
            {
                gateContext.GateLog.WriteError(errMsg);
            }*/
        }

        /// <summary>
        /// Clears the task queue.
        /// </summary>
        public void ClearTaskQueue()
        {
            /*lock (taskQueue)
            {
                taskQueue.Clear();
            }*/
        }

        /// <summary>
        /// Appends information to the string builder.
        /// </summary>
        public void AppendInfo(StringBuilder sbInfo)
        {
            /*if (sbInfo == null)
                throw new ArgumentNullException(nameof(sbInfo));

            if (Locale.IsRussian)
            {
                sbInfo
                    .AppendLine("Репликация архива")
                    .AppendLine("-----------------")
                    .Append("Готовность      : ").AppendLine(replicatorIsReady ? "да" : "нет")
                    .Append("Задач в очереди : ").Append(taskQueue.Count)
                    .Append(" из ").Append(MaxTaskQueueSize).AppendLine()
                    .Append("Задача          : ").AppendLine(displayTask == null ? "нет" : displayTask.Name);
            }
            else
            {
                sbInfo
                    .AppendLine("Archive Replication")
                    .AppendLine("-------------------")
                    .Append("Is ready       : ").AppendLine(replicatorIsReady ? "Yes" : "No")
                    .Append("Tasks in queue : ").Append(taskQueue.Count)
                    .Append(" of ").Append(MaxTaskQueueSize).AppendLine()
                    .Append("Task           : ").AppendLine(displayTask == null ? "None" : displayTask.Name);
            }

            if (displayTask != null)
                displayTask.State.AppendInfo(sbInfo);*/
        }
    }
}
