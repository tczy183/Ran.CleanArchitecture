﻿using Ran.Core.Utils.Collections;
using Ran.Core.Utils.System;

namespace Ran.Core.Modularity;

/// <summary>
/// 模块描述器
/// </summary>
public class ModuleDescriptor : IModuleDescriptor
{
    private readonly List<IModuleDescriptor> _dependencies;

    /// <summary>
    /// 模块类
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// 模块的主程序集
    /// </summary>
    public Assembly Assembly { get; }

    /// <summary>
    /// 模块的所有组件
    /// 包括在模块 Type 上使用 AdditionalAssemblyAttribute 属性标记的主程序集和其他已定义的程序集
    /// </summary>
    public Assembly[] AllAssemblies { get; }

    /// <summary>
    /// 模块类的实例(单例)
    /// </summary>
    public IModule Instance { get; }

    /// <summary>
    /// 该模块是否作为插件加载
    /// </summary>
    public bool IsLoadedAsPlugIn { get; }

    /// <summary>
    /// 此模块所依赖的模块
    /// 一个模块可以通过<see cref="DependsOnAttribute"/>属性依赖于另一个模块
    /// </summary>
    public IReadOnlyList<IModuleDescriptor> Dependencies => [.. _dependencies];

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="instance"></param>
    /// <param name="isLoadedAsPlugIn"></param>
    /// <exception cref="ArgumentException"></exception>
    public ModuleDescriptor(Type type, IModule instance, bool isLoadedAsPlugIn)
    {
        _ = CheckHelper.NotNull(type, nameof(type));
        _ = CheckHelper.NotNull(instance, nameof(instance));
        ModuleHelper.CheckXiHanModuleType(type);

        if (!type.GetTypeInfo().IsInstanceOfType(instance))
        {
            throw new ArgumentException(
                $"模块实例({instance.GetType().AssemblyQualifiedName})不是模块类型{type.AssemblyQualifiedName}的实例！");
        }

        Type = type;
        Assembly = type.Assembly;
        AllAssemblies = ModuleHelper.GetAllAssemblies(type);
        Instance = instance;
        IsLoadedAsPlugIn = isLoadedAsPlugIn;

        _dependencies = [];
    }

    /// <summary>
    /// 添加依赖项
    /// </summary>
    /// <param name="descriptor"></param>
    public void AddDependency(IModuleDescriptor descriptor)
    {
        _ = _dependencies.AddIfNotContains(descriptor);
    }

    /// <summary>
    /// 字符串表示形式
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"[ModuleDescriptor {Type.FullName}]";
    }
}
