﻿namespace Ran.Core.Utils.Maths;

/// <summary>
/// 金额扩展方法
/// </summary>
public static class MoneyFormatExtensions
{
    /// <summary>
    /// 格式化金额(由千位转万位，如 12,345,678.90=>1234,5678.90 )
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string FormatMoneyToString(this decimal num)
    {
        var numStr = num.ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
        string numRes;
        var numDecimal = string.Empty;
        if (numStr.Contains('.'))
        {
            var numInt = numStr.Split('.')[0];
            numDecimal = "." + numStr.Split('.')[1];
            numRes = FormatMoneyStringComma(numInt);
        }
        else
        {
            numRes = FormatMoneyStringComma(numStr);
        }

        return numRes + numDecimal;
    }

    /// <summary>
    /// 金额字符串加逗号格式化
    /// </summary>
    /// <param name="numInt"></param>
    /// <returns></returns>
    private static string FormatMoneyStringComma(string numInt)
    {
        if (numInt.Length <= 4)
        {
            return numInt;
        }

        var numNoFormat = numInt[..^4];
        var numFormat = numInt.Substring(numInt.Length - 4, 4);
        return numNoFormat.Length > 4
            ? FormatMoneyStringComma(numNoFormat) + "," + numFormat
            : numNoFormat + "," + numFormat;
    }
}
