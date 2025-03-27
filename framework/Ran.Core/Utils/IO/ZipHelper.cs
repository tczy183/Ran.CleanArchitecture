using System.IO.Compression;

namespace Ran.Core.Utils.IO;

/// <summary>
/// 压缩帮助类
/// </summary>
public static class ZipHelper
{
    /// <summary>
    /// 解压
    /// </summary>
    /// <param name="archivePath">存档路径</param>
    /// <param name="extractPath">提取路径</param>
    /// <exception cref="FileNotFoundException"></exception>
    public static void Extract(string archivePath, string extractPath)
    {
        if (!File.Exists(archivePath))
        {
            throw new FileNotFoundException("没有找到文件。", archivePath);
        }

        if (!Directory.Exists(extractPath))
        {
            _ = Directory.CreateDirectory(extractPath);
        }

        ZipFile.ExtractToDirectory(archivePath, extractPath);
    }

    /// <summary>
    /// 压缩
    /// </summary>
    /// <param name="sourceDirectory">源目录</param>
    /// <param name="archivePath">存档路径</param>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public static void Compress(string sourceDirectory, string archivePath)
    {
        if (!Directory.Exists(sourceDirectory))
        {
            throw new DirectoryNotFoundException("找不到源目录。");
        }

        ZipFile.CreateFromDirectory(sourceDirectory, archivePath, CompressionLevel.Optimal, false);
    }
}
