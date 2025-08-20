using Ran.Core.Utils.System;

namespace Ran.Core.Modularity.PlugIns;

/// <summary>
/// 插件源列表扩展
/// </summary>
public static class PlugInSourceListExtensions
{
    /// <summary>
    /// 添加文件夹
    /// </summary>
    /// <param name="list"></param>
    /// <param name="folder"></param>
    /// <param name="searchOption"></param>
    public static void AddFolder(
        this PlugInSourceList list,
        string folder,
        SearchOption searchOption = SearchOption.TopDirectoryOnly
    )
    {
        _ = CheckHelper.NotNull(list, nameof(list));

        list.Add(new FolderPlugInSource(folder, searchOption));
    }

    /// <summary>
    /// 添加类型
    /// </summary>
    /// <param name="list"></param>
    /// <param name="moduleTypes"></param>
    public static void AddTypes(this PlugInSourceList list, params Type[] moduleTypes)
    {
        _ = CheckHelper.NotNull(list, nameof(list));

        list.Add(new TypePlugInSource(moduleTypes));
    }

    /// <summary>
    /// 添加文件
    /// </summary>
    /// <param name="list"></param>
    /// <param name="filePaths"></param>
    public static void AddFiles(this PlugInSourceList list, params string[] filePaths)
    {
        _ = CheckHelper.NotNull(list, nameof(list));

        list.Add(new FilePlugInSource(filePaths));
    }
}
