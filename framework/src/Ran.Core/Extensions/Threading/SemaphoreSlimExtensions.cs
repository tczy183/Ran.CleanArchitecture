﻿using System.Runtime.CompilerServices;
using Ran.Core.Utils.Threading;

namespace Ran.Core.Extensions.Threading;

/// <summary>
/// 信号量扩展
/// </summary>
public static class SemaphoreSlimExtensions
{
    /// <summary>
    /// 异步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim)
    {
        await semaphoreSlim.WaitAsync();
        return GetDispose(semaphoreSlim);
    }

    /// <summary>
    /// 异步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<IDisposable> LockAsync(
        this SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken
    )
    {
        await semaphoreSlim.WaitAsync(cancellationToken);
        return GetDispose(semaphoreSlim);
    }

    /// <summary>
    /// 异步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="millisecondsTimeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<IDisposable> LockAsync(
        this SemaphoreSlim semaphoreSlim,
        int millisecondsTimeout
    )
    {
        return await semaphoreSlim.WaitAsync(millisecondsTimeout)
            ? GetDispose(semaphoreSlim)
            : throw new TimeoutException();
    }

    /// <summary>
    /// 异步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="millisecondsTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<IDisposable> LockAsync(
        this SemaphoreSlim semaphoreSlim,
        int millisecondsTimeout,
        CancellationToken cancellationToken
    )
    {
        return await semaphoreSlim.WaitAsync(millisecondsTimeout, cancellationToken)
            ? GetDispose(semaphoreSlim)
            : throw new TimeoutException();
    }

    /// <summary>
    /// 异步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<IDisposable> LockAsync(
        this SemaphoreSlim semaphoreSlim,
        TimeSpan timeout
    )
    {
        return await semaphoreSlim.WaitAsync(timeout)
            ? GetDispose(semaphoreSlim)
            : throw new TimeoutException();
    }

    /// <summary>
    /// 异步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="timeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async ValueTask<IDisposable> LockAsync(
        this SemaphoreSlim semaphoreSlim,
        TimeSpan timeout,
        CancellationToken cancellationToken
    )
    {
        return await semaphoreSlim.WaitAsync(timeout, cancellationToken)
            ? GetDispose(semaphoreSlim)
            : throw new TimeoutException();
    }

    /// <summary>
    /// 同步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDisposable Lock(this SemaphoreSlim semaphoreSlim)
    {
        semaphoreSlim.Wait();
        return GetDispose(semaphoreSlim);
    }

    /// <summary>
    /// 同步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDisposable Lock(
        this SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken
    )
    {
        semaphoreSlim.Wait(cancellationToken);
        return GetDispose(semaphoreSlim);
    }

    /// <summary>
    /// 同步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="millisecondsTimeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout)
    {
        return semaphoreSlim.Wait(millisecondsTimeout)
            ? GetDispose(semaphoreSlim)
            : throw new TimeoutException();
    }

    /// <summary>
    /// 同步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="millisecondsTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDisposable Lock(
        this SemaphoreSlim semaphoreSlim,
        int millisecondsTimeout,
        CancellationToken cancellationToken
    )
    {
        return semaphoreSlim.Wait(millisecondsTimeout, cancellationToken)
            ? GetDispose(semaphoreSlim)
            : throw new TimeoutException();
    }

    /// <summary>
    /// 同步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
    {
        return semaphoreSlim.Wait(timeout)
            ? GetDispose(semaphoreSlim)
            : throw new TimeoutException();
    }

    /// <summary>
    /// 同步锁
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <param name="timeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IDisposable Lock(
        this SemaphoreSlim semaphoreSlim,
        TimeSpan timeout,
        CancellationToken cancellationToken
    )
    {
        return semaphoreSlim.Wait(timeout, cancellationToken)
            ? GetDispose(semaphoreSlim)
            : throw new TimeoutException();
    }

    /// <summary>
    /// 获取释放信号量的操作
    /// </summary>
    /// <param name="semaphoreSlim"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DisposeAction<SemaphoreSlim> GetDispose(this SemaphoreSlim semaphoreSlim)
    {
        return new DisposeAction<SemaphoreSlim>(
            static (semaphoreSlim) =>
            {
                _ = semaphoreSlim.Release();
            },
            semaphoreSlim
        );
    }
}
