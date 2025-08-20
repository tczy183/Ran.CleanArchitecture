﻿namespace Ran.Core.DependencyInjection;

/// <summary>
/// 属性或字段自动装配服务处理器
/// </summary>
/// <remarks>由此启发：<see href="https://www.cnblogs.com/loogn/p/10566510.html"/></remarks>
public class AutowiredServiceHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, Action<object, IServiceProvider>> _autowiredActions = [];

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider"></param>
    public AutowiredServiceHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 装配属性和字段
    /// </summary>
    /// <param name="service"></param>
    public void Autowired(object service)
    {
        Autowired(service, _serviceProvider);
    }

    /// <summary>
    /// 装配属性和字段
    /// </summary>
    /// <param name="service"></param>
    /// <param name="serviceProvider"></param>
    private void Autowired(object service, IServiceProvider serviceProvider)
    {
        var serviceType = service.GetType();
        if (_autowiredActions.TryGetValue(serviceType, out var act))
        {
            act(service, serviceProvider);
        }
        else
        {
            //参数
            var objParam = Expression.Parameter(typeof(object), "obj");
            var spParam = Expression.Parameter(typeof(IServiceProvider), "sp");
            var obj = Expression.Convert(objParam, serviceType);
            var getService = typeof(IServiceProvider).GetMethod("GetService");

            List<Expression> setList = [];
            if (getService is not null)
            {
                // 字段赋值
                setList.AddRange(
                    from field in serviceType.GetFields(BindingFlags.Instance | BindingFlags.Public)
                    let autowiredAttr = field.GetCustomAttribute<AutowiredServiceAttribute>()
                    where autowiredAttr is not null
                    let fieldExp = Expression.Field(obj, field)
                    let createService = Expression.Call(
                        spParam,
                        getService,
                        Expression.Constant(field.FieldType)
                    )
                    select Expression.Assign(
                        fieldExp,
                        Expression.Convert(createService, field.FieldType)
                    )
                );
                // 属性赋值
                setList.AddRange(
                    from property in serviceType.GetProperties(
                        BindingFlags.Instance | BindingFlags.Public
                    )
                    let autowiredAttr = property.GetCustomAttribute<AutowiredServiceAttribute>()
                    where autowiredAttr is not null
                    let propExp = Expression.Property(obj, property)
                    let createService = Expression.Call(
                        spParam,
                        getService,
                        Expression.Constant(property.PropertyType)
                    )
                    select Expression.Assign(
                        propExp,
                        Expression.Convert(createService, property.PropertyType)
                    )
                );
            }

            var bodyExp = Expression.Block(setList);
            var setAction = Expression
                .Lambda<Action<object, IServiceProvider>>(bodyExp, objParam, spParam)
                .Compile();
            _autowiredActions[serviceType] = setAction;
            setAction(service, serviceProvider);
        }
    }
}
