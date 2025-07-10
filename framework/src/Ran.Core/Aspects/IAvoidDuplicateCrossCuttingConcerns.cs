namespace Ran.Core.Aspects;

/// <summary>
/// 避免重复横切关注点
/// </summary>
public interface IAvoidDuplicateCrossCuttingConcerns
{
    /// <summary>
    /// 应用的横切关注点
    /// </summary>
    List<string> AppliedCrossCuttingConcerns { get; }
}
