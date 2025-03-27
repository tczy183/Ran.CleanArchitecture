﻿namespace Ran.Core.Utils.Threading;

/// <summary>
/// 异步锁
/// </summary>
public class AsyncLock : IDisposable
{
    // 控制访问的信号量
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    // 锁释放器
    private readonly Task<IDisposable> _releaser;

    /// <summary>
    /// 创建一个异步锁
    /// </summary>
    public AsyncLock()
    {
        _releaser = Task.FromResult((IDisposable)new Releaser(this));
    }

    /// <summary>
    /// 异步获取锁
    /// </summary>
    /// <returns></returns>
    public async Task<IDisposable> LockAsync()
    {
        await _semaphore.WaitAsync();
        return _releaser.Result;
    }

    /// <summary>
    /// 带超时的异步获取锁
    /// </summary>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    public async Task<IDisposable> LockAsync(TimeSpan timeout)
    {
        return await _semaphore.WaitAsync(timeout) ? _releaser.Result : throw new TimeoutException("未能在指定的超时时间内获取锁。");
    }

    /// <summary>
    /// 同步获取锁
    /// </summary>
    /// <returns></returns>
    public IDisposable Lock()
    {
        _semaphore.Wait();
        return _releaser.Result;
    }

    /// <summary>
    /// 销毁锁对象
    /// </summary>
    public void Dispose()
    {
        _semaphore.Dispose();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 内部释放器类
    /// </summary>
    private sealed class Releaser : IDisposable
    {
        private readonly AsyncLock _lock;

        public Releaser(AsyncLock lockObj)
        {
            _lock = lockObj;
        }

        public void Dispose()
        {
            _ = _lock._semaphore.Release();
        }
    }
}
