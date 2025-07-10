namespace Ran.Core.Utils.Logging;

/// <summary>
/// 简单的日志输出帮助类
/// </summary>
public static class LogHelper
{
    private static readonly Lock ObjLock = new();

    /// <summary>
    /// 在控制台输出
    /// </summary>
    /// <param name="inputStr">打印文本</param>
    /// <param name="frontColor">前置颜色</param>
    private static void WriteColorLine(string? inputStr, ConsoleColor frontColor)
    {
        lock (ObjLock)
        {
            var currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = frontColor;
            Console.WriteLine(inputStr);
            Console.ForegroundColor = currentForeColor;
        }
    }

    /// <summary>
    /// 正常信息
    /// </summary>
    /// <param name="inputStr"></param>
    /// <param name="frontColor"></param>
    public static void Info(string? inputStr, ConsoleColor frontColor = ConsoleColor.White)
    {
        WriteColorLine(inputStr, frontColor);
    }

    /// <summary>
    /// 成功信息
    /// </summary>
    /// <param name="inputStr"></param>
    /// <param name="frontColor"></param>
    public static void Success(string? inputStr, ConsoleColor frontColor = ConsoleColor.Green)
    {
        WriteColorLine(inputStr, frontColor);
    }

    /// <summary>
    /// 处理、查询信息
    /// </summary>
    /// <param name="inputStr"></param>
    /// <param name="frontColor"></param>
    public static void Handle(string? inputStr, ConsoleColor frontColor = ConsoleColor.Blue)
    {
        WriteColorLine(inputStr, frontColor);
    }

    /// <summary>
    /// 警告、新增、更新信息
    /// </summary>
    /// <param name="inputStr"></param>
    /// <param name="frontColor"></param>
    public static void Warn(string? inputStr, ConsoleColor frontColor = ConsoleColor.Yellow)
    {
        WriteColorLine(inputStr, frontColor);
    }

    /// <summary>
    /// 错误、删除、危险、异常信息
    /// </summary>
    /// <param name="inputStr"></param>
    /// <param name="frontColor"></param>
    public static void Error(string? inputStr, ConsoleColor frontColor = ConsoleColor.Red)
    {
        WriteColorLine(inputStr, frontColor);
    }
}
