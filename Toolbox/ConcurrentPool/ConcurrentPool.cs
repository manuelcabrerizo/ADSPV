using System;
using System.Collections.Concurrent;

// TODO: change Resetteable for Resettable 

namespace Rexar.Toolbox.Pool
{
    public sealed class ConcurrentPool
    {
        private readonly ConcurrentDictionary<Type, ConcurrentStack<IResetteable>> concurrentPool = 
            new ConcurrentDictionary<Type, ConcurrentStack<IResetteable>>();

        public ResetteableType Get<ResetteableType>(params object[] parameters) where ResetteableType : IResetteable
        {
            Type resettableType = typeof(ResetteableType);
            if(!concurrentPool.ContainsKey(resettableType))
            {
                concurrentPool.TryAdd(resettableType, new ConcurrentStack<IResetteable>());
            }

            ResetteableType val;
            if(concurrentPool[resettableType].Count > 0)
            {
                concurrentPool[resettableType].TryPop(out IResetteable resettable);
                val = (ResetteableType)resettable;
            }
            else
            {
                val = (ResetteableType)Activator.CreateInstance(resettableType);
            }

            val.Assign(parameters);
            return val;
        }

        public void Release<ResetteableType>(ResetteableType resettable) where ResetteableType : IResetteable
        {
            resettable.Reset();
            concurrentPool[typeof(ResetteableType)].Push(resettable);
        }
    }
}
