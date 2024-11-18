using Cysharp.Threading.Tasks;
using SpinMatch.Enums;

namespace SpinMatch.Jobs
{
    public abstract class Job
    {
        public virtual int ExecutionOrder { get; } = (int) ExecutionOrders.DEFAULT;

        public virtual bool IsParallel => false;

        public bool JobCompleted { get; protected set; } = false;

        public abstract UniTask ExecuteAsync();
    } 
}