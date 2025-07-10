﻿using Ran.Core.Utils.Collections;
using Ran.Core.Utils.Threading;

namespace Ran.Core.Aspects;

/// <summary>
/// 横切关注点
/// </summary>
public static class CrossCuttingConcerns
{
    /// <summary>
    /// 添加已应用的横切关注点
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="concerns"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void AddApplied(object obj, params string[] concerns)
    {
        if (concerns.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(concerns), $"未提供 {nameof(concerns)}!");
        }

        (obj as IAvoidDuplicateCrossCuttingConcerns)?.AppliedCrossCuttingConcerns.AddRange(concerns);
    }

    /// <summary>
    /// 移除已应用的横切关注点
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="concerns"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void RemoveApplied(object obj, params string[] concerns)
    {
        if (concerns.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(concerns), $"未提供 {nameof(concerns)}!");
        }

        if (obj is not IAvoidDuplicateCrossCuttingConcerns crossCuttingEnabledObj)
        {
            return;
        }

        foreach (var concern in concerns)
        {
            _ = crossCuttingEnabledObj.AppliedCrossCuttingConcerns.RemoveAll(c => c == concern);
        }
    }

    /// <summary>
    /// 是否已应用
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="concern"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool IsApplied([NotNull] object obj, [NotNull] string concern)
    {
        return obj is null
            ? throw new ArgumentNullException(nameof(obj))
            : concern is null
                ? throw new ArgumentNullException(nameof(concern))
                : (obj as IAvoidDuplicateCrossCuttingConcerns)?.AppliedCrossCuttingConcerns.Contains(concern) ?? false;
    }

    /// <summary>
    /// 应用
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="concerns"></param>
    /// <returns></returns>
    public static IDisposable Applying(object obj, params string[] concerns)
    {
        AddApplied(obj, concerns);
        return new DisposeAction<ValueTuple<object, string[]>>(static (state) =>
        {
            var (obj, concerns) = state;
            RemoveApplied(obj, concerns);
        }, (obj, concerns));
    }

    /// <summary>
    /// 获取已应用的横切关注点
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string[] GetApplieds(object obj)
    {
        return obj is not IAvoidDuplicateCrossCuttingConcerns crossCuttingEnabledObj
            ? []
            : crossCuttingEnabledObj.AppliedCrossCuttingConcerns.ToArray();
    }
}
