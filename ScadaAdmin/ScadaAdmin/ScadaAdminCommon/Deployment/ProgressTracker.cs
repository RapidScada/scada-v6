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
 * Module   : ScadaAdminCommon
 * Summary  : Calculates percentage of process completion
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;

namespace Scada.Admin.Deployment
{
    /// <summary>
    /// Calculates percentage of process completion.
    /// <para>Вычисляет процент выполнения процесса.</para>
    /// </summary>
    public class ProgressTracker
    {
        private readonly ITransferControl transferControl; // shows progress
        private int taskIndex;    // the current task index
        private int taskCount;    // the task count
        private int subtaskIndex; // the current subtask index
        private int subtaskCount; // the subtask count


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProgressTracker(ITransferControl transferControl)
        {
            this.transferControl = transferControl ?? throw new ArgumentNullException(nameof(transferControl));
            taskIndex = 0;
            taskCount = 0;
            subtaskIndex = 0;
            subtaskCount = 0;
        }


        /// <summary>
        /// Gets or sets the current task index.
        /// </summary>
        public int TaskIndex
        {
            get
            {
                return taskIndex;
            }
            set
            {
                taskIndex = value;
                subtaskIndex = 0;
                subtaskCount = 0;
                UpdateProgress();
            }
        }

        /// <summary>
        /// Gets or sets the task count.
        /// </summary>
        public int TaskCount
        {
            get
            {
                return taskCount;
            }
            set
            {
                taskCount = value;
                UpdateProgress();
            }
        }

        /// <summary>
        /// Gets or sets the current subtask index.
        /// </summary>
        public int SubtaskIndex
        {
            get
            {
                return subtaskIndex;
            }
            set
            {
                subtaskIndex = value;
                UpdateProgress();
            }
        }

        /// <summary>
        /// Gets or sets the subtask count.
        /// </summary>
        public int SubtaskCount
        {
            get
            {
                return subtaskCount;
            }
            set
            {
                subtaskCount = value;
                UpdateProgress();
            }
        }


        /// <summary>
        /// Updates the process progress.
        /// </summary>
        private void UpdateProgress()
        {
            if (taskCount <= 0)
            {
                transferControl.SetProgress(0.0);
            }
            else if (taskIndex >= taskCount)
            {
                transferControl.SetProgress(1.0);
            }
            else
            {
                transferControl.SetProgress((double)taskIndex / taskCount + 
                    (subtaskCount > 0 ? (double)subtaskIndex / subtaskCount / taskCount : 0));
            }
        }

        /// <summary>
        /// Skips a task, increasing progress.
        /// </summary>
        public void SkipTask(int taskCount = 1)
        {
            TaskIndex += taskCount;
        }

        /// <summary>
        /// Updates the subtask.
        /// </summary>
        public void UpdateSubtask(int subtaskIndex, int subtaskCount)
        {
            this.subtaskIndex = subtaskIndex;
            this.subtaskCount = subtaskCount;
            UpdateProgress();
        }

        /// <summary>
        /// Marks progress as complete.
        /// </summary>
        public void SetCompleted()
        {
            TaskIndex = TaskCount;
        }
    }
}
