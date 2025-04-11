using Ran.Core.Exceptions;
using Ran.Core.Utils.Collections;
using Ran.Core.Utils.Reflections;
using Ran.Core.Utils.System;
using System.Runtime.Loader;

namespace Ran.Core.Modularity.PlugIns;

/// <summary>
/// FolderPlugInSource
/// </summary>
public class FolderPlugInSource : IPlugInSource
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public string Folder { get; }

    /// <summary>
    /// 搜索选项
    /// </summary>
    public SearchOption SearchOption { get; set; }

    /// <summary>
    /// 过滤器
    /// </summary>
    public Func<string, bool>? Filter { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="searchOption"></param>
    public FolderPlugInSource(string folder, SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        _ = CheckHelper.NotNull(folder, nameof(folder));

        Folder = folder;
        SearchOption = searchOption;
    }

    /// <summary>
    /// 获取模块
    /// </summary>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public Type[] GetModules()
    {
        List<Type> modules = [];

        foreach (var assembly in GetAssemblies())
        {
            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (ModuleHelper.IsXiHanModule(type))
                    {
                        _ = modules.AddIfNotContains(type);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"无法从程序集获取模块类型：{assembly.FullName}", innerException: ex);
            }
        }

        return [.. modules];
    }

    /// <summary>
    /// 获取程序集
    /// </summary>
    /// <returns></returns>
    private List<Assembly> GetAssemblies()
    {
        var assemblyFiles = AssemblyHelper.GetAssemblyFiles(Folder, SearchOption);

        if (Filter is not null)
        {
            assemblyFiles = assemblyFiles.Where(Filter);
        }

        return assemblyFiles.Select(AssemblyLoadContext.Default.LoadFromAssemblyPath).ToList();
    }
}
