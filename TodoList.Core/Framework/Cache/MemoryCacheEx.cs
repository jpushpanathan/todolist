using System;
using System.Runtime.Caching;
using TodoList.Core.Interfaces;

namespace TodoList.Core.Framework.Cache
{
    public class MemoryCacheEx : IMemoryCacheEx
    {
        public static readonly System.Threading.ReaderWriterLockSlim CacheLock = new System.Threading.ReaderWriterLockSlim();

        public bool Add<T>(string key, T input, int expirationDurationInSeconds)
        {
            if (this.Exists(key))
                return false;

            // Check if we can upgrade from Read to Write mode
            CacheLock.EnterUpgradeableReadLock();
            try
            {
                //check if key already in cache while waiting to EnterUpgradeableReadLock
                if (MemoryCache.Default.Contains(key) == true)
                    return false;

                //all good, enter write lock
                CacheLock.EnterWriteLock();
                try
                {
                    var cip = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expirationDurationInSeconds)
                    };
                    MemoryCache.Default.Add(key, input, cip);
                    return true;
                }
                finally
                {
                    CacheLock.ExitWriteLock();
                }
            }
            finally
            {
                CacheLock.ExitUpgradeableReadLock();
            }
        }

        public T Get<T>(string key)
        {
            //Check if is ready to enter read lock
            CacheLock.EnterReadLock();
            try
            {
                //Returns null if the string does not exist, 
                var cacheItem = MemoryCache.Default.Get(key, null);

                if (cacheItem != null)
                {
                    return (T)cacheItem;
                }
            }
            finally
            {
                CacheLock.ExitReadLock();
            }
            return default(T);
        }
        public bool Exists(string key)
        {
            return MemoryCache.Default.Contains(key);
        }

        /// <summary>
        /// Updates item for given key. if item is not present, it creates it.
        /// </summary>
        public bool Update<T>(string key, T input, int expirationDurationInSeconds)
        {
            // Check if we can upgrade from Read to Write mode
            CacheLock.EnterUpgradeableReadLock();
            try
            {
                //all good, enter write lock
                CacheLock.EnterWriteLock();
                try
                {
                    var cip = new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expirationDurationInSeconds)
                    };
                    MemoryCache.Default.Set(key, input, cip);
                    return true;
                }
                finally
                {
                    CacheLock.ExitWriteLock();
                }
            }
            finally
            {
                CacheLock.ExitUpgradeableReadLock();
            }
        }
        public bool Remove(string key)
        {
            CacheLock.EnterUpgradeableReadLock();
            try
            {
                //all good, enter write lock
                CacheLock.EnterWriteLock();
                try
                {
                    return MemoryCache.Default.Remove(key) != null;
                }
                finally
                {
                    CacheLock.ExitWriteLock();
                }
            }
            finally
            {
                CacheLock.ExitUpgradeableReadLock();
            }
        }

        public bool Add<T>(string key, T input)
        {
            this.Add(key, input, 3600);//TODO: make this configurable
            return true;
        }

        public bool Update<T>(string key, T input)
        {
            this.Update(key, input, 3600); //TODO: make this configurable
            return true;
        }
    }
}
