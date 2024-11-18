using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using SpinMatch.Items;

namespace SpinMatch.Jobs
{
    public class JobsExecutor : IDisposable
    {
        private static List<Job> _jobs;
        private static Dictionary<int, List<ItemsFallJob>> _fallJobPairs;

        public JobsExecutor()
        {
            _jobs = new List<Job>();
            _fallJobPairs = new();
        }

        public async UniTask ExecuteJobsAsync()
        {
            ChainFallJobs();

            ItemStateManager.SetAllItemsState();
            
             await UniTask.WhenAll(_jobs.Select(job => job.ExecuteAsync()));

            ClearJobs();
        }

        public static void AddJob(Job job)
        {
            _jobs.Add(job);
        }

        public static void AddFallJob(ItemsFallJob fallJob, int columnIndex)
        {
            if (_fallJobPairs.ContainsKey(columnIndex))
            {
                _fallJobPairs[columnIndex].Add(fallJob);
            }
            else
            {
                _fallJobPairs[columnIndex] = new() { fallJob };
            }
        }

        private void ChainFallJobs()
        {
            foreach (var fallJobPerColumn in _fallJobPairs)
            {
                List<ItemsFallJob> fallJobList = fallJobPerColumn.Value;

                AddJob(fallJobList[0]);

                for (int i = 0; i < fallJobList.Count; i++)
                {
                    if(i + 1 < fallJobList.Count)
                    {
                        fallJobList[i].SetNextFallJob(fallJobList[i + 1]);
                    }             
                }
            }
        }

        private void ClearJobs()
        {
            _jobs.Clear();
            _fallJobPairs.Clear();
        }

        public void Dispose()
        {
            ClearJobs();
        }
    }
}