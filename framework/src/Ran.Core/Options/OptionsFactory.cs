﻿using Ran.Core.Utils.Text;

namespace Ran.Core.Options;

/// <summary>
/// 选项工厂
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public class OptionsFactory<TOptions> : IOptionsFactory<TOptions> where TOptions : class, new()
{
    private readonly IConfigureOptions<TOptions>[] _setups;
    private readonly IPostConfigureOptions<TOptions>[] _postConfigures;
    private readonly IValidateOptions<TOptions>[] _validations;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="setups"></param>
    /// <param name="postConfigures"></param>
    public OptionsFactory(
        IEnumerable<IConfigureOptions<TOptions>> setups,
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigures)
        : this(setups, postConfigures, Array.Empty<IValidateOptions<TOptions>>())
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="setups"></param>
    /// <param name="postConfigures"></param>
    /// <param name="validations"></param>
    public OptionsFactory(
        IEnumerable<IConfigureOptions<TOptions>> setups,
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
        IEnumerable<IValidateOptions<TOptions>> validations)
    {
        _setups = setups as IConfigureOptions<TOptions>[] ?? new List<IConfigureOptions<TOptions>>(setups).ToArray();
        _postConfigures = postConfigures as IPostConfigureOptions<TOptions>[] ??
                          new List<IPostConfigureOptions<TOptions>>(postConfigures).ToArray();
        _validations = validations as IValidateOptions<TOptions>[] ??
                       new List<IValidateOptions<TOptions>>(validations).ToArray();
    }

    /// <summary>
    /// 创建选项
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual TOptions Create(string name)
    {
        var options = CreateInstance(name);

        ConfigureOptions(name, options);
        PostConfigureOptions(name, options);
        ValidateOptions(name, options);

        return options;
    }

    /// <summary>
    /// 配置选项
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    protected virtual void ConfigureOptions(string name, TOptions options)
    {
        foreach (var setup in _setups)
        {
            if (setup is IConfigureNamedOptions<TOptions> namedSetup)
            {
                namedSetup.Configure(name, options);
            }
            else if (name.IsNullOrWhiteSpace())
            {
                setup.Configure(options);
            }
        }
    }

    /// <summary>
    /// 后置配置选项
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    protected virtual void PostConfigureOptions(string name, TOptions options)
    {
        foreach (var post in _postConfigures)
        {
            post.PostConfigure(name, options);
        }
    }

    /// <summary>
    /// 验证选项
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    /// <exception cref="OptionsValidationException"></exception>
    protected virtual void ValidateOptions(string name, TOptions options)
    {
        if (_validations.Length <= 0)
        {
            return;
        }

        List<string> failures = [];
        foreach (var validate in _validations)
        {
            var result = validate.Validate(name, options);
            if (result.Failed)
            {
                failures.AddRange(result.Failures);
            }
        }

        if (failures.Count > 0)
        {
            throw new OptionsValidationException(name, typeof(TOptions), failures);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected virtual TOptions CreateInstance(string name)
    {
        return Activator.CreateInstance<TOptions>();
    }
}
