using Ran.Core.Exceptions;
using Ran.Core.Utils.Collections;
using System.Runtime.Loader;

namespace Ran.Core.Modularity.PlugIns;

/// <summary>
/// 文件插件源
/// </summary>
public class FilePlugInSource : IPlugInSource
{
    /// <summary>
    /// 文件路径
    /// </summary>
    public string[] FilePaths { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="filePaths"></param>
    public FilePlugInSource(params string[]? filePaths)
    {
        FilePaths = filePaths ?? [];
    }

    /// <summary>
    /// 获取模块
    /// </summary>
    /// <returns></returns>
    /// <exception cref="XiHanException"></exception>
    public Type[] GetModules()
    {
        List<Type> modules = [];

        foreach (var filePath in FilePaths)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(filePath);

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
}
