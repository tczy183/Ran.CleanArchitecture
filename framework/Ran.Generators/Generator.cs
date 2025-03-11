﻿using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Ran.Generators;

/// <summary>
/// The main generator class that will be responsible for generating the source code.
/// </summary>
[Generator]
public class Generator : IIncrementalGenerator
{
    private const string SCOPED_ATTRIBUTE_NAME = "RegisterScopedAttribute";
    private const string SINGLETON_ATTRIBUTE_NAME = "RegisterSingletonAttribute";
    private const string TRANSIENT_ATTRIBUTE_NAME = "RegisterTransientAttribute";
    private const string HOSTED_SERVICE_ATTRIBUTE_NAME = "RegisterHostedServiceAttribute";
    
    private const string KEYED_SCOPED_ATTRIBUTE_NAME = "RegisterKeyedScopedAttribute";
    private const string KEYED_SINGLETON_ATTRIBUTE_NAME = "RegisterKeyedSingletonAttribute";
    private const string KEYED_TRANSIENT_ATTRIBUTE_NAME = "RegisterKeyedTransientAttribute";
    
    private const string TRY_SCOPED_ATTRIBUTE_NAME = "TryRegisterScopedAttribute";
    private const string TRY_SINGLETON_ATTRIBUTE_NAME = "TryRegisterSingletonAttribute";
    private const string TRY_TRANSIENT_ATTRIBUTE_NAME = "TryRegisterTransientAttribute";
    
    private const string TRY_KEYED_SCOPED_ATTRIBUTE_NAME = "TryRegisterKeyedScopedAttribute";
    private const string TRY_KEYED_SINGLETON_ATTRIBUTE_NAME = "TryRegisterKeyedSingletonAttribute";
    private const string TRY_KEYED_TRANSIENT_ATTRIBUTE_NAME = "TryRegisterKeyedTransientAttribute";

    private const string ONLY_REGISTER_AS = "onlyRegisterAs";
    private const string SERVICE_KEY = "serviceKey"; 
    
    private static readonly string KeyedServiceExceptionFormattedMessage = "{0} requires a service key to be passed as an argument. Service Key argument was null, empty, or whitespace.";

    private static readonly Dictionary<string, AutoRegistrationType> RegistrationTypes = new()
    {
        [SCOPED_ATTRIBUTE_NAME] = AutoRegistrationType.Scoped,
        [SINGLETON_ATTRIBUTE_NAME] = AutoRegistrationType.Singleton,
        [TRANSIENT_ATTRIBUTE_NAME] = AutoRegistrationType.Transient,
        [HOSTED_SERVICE_ATTRIBUTE_NAME] = AutoRegistrationType.Hosted,
        
        [KEYED_SCOPED_ATTRIBUTE_NAME] = AutoRegistrationType.KeyedScoped,
        [KEYED_SINGLETON_ATTRIBUTE_NAME] = AutoRegistrationType.KeyedSingleton,
        [KEYED_TRANSIENT_ATTRIBUTE_NAME] = AutoRegistrationType.KeyedTransient,
        
        [TRY_SCOPED_ATTRIBUTE_NAME] = AutoRegistrationType.TryScoped,
        [TRY_SINGLETON_ATTRIBUTE_NAME] = AutoRegistrationType.TrySingleton,
        [TRY_TRANSIENT_ATTRIBUTE_NAME] = AutoRegistrationType.TryTransient,
        
        [TRY_KEYED_SCOPED_ATTRIBUTE_NAME] = AutoRegistrationType.TryKeyedScoped,
        [TRY_KEYED_SINGLETON_ATTRIBUTE_NAME] = AutoRegistrationType.TryKeyedSingleton,
        [TRY_KEYED_TRANSIENT_ATTRIBUTE_NAME] = AutoRegistrationType.TryKeyedTransient,
    };
    
    private static readonly string[] IgnoredInterfaces =
    [
        "System.IDisposable",
        "System.IAsyncDisposable"
    ];
    
    public void Initialize(IncrementalGeneratorInitializationContext initialisationContext)
    {
        initialisationContext.RegisterPostInitializationOutput((i) =>
        {
            i.AddSource("AutoRegisterInject.Attributes.g.cs",
                SourceText.From(SourceConstants.GENERATE_ATTRIBUTE_SOURCE, Encoding.UTF8));
        });

        var autoRegistered = initialisationContext
            .SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
                transform: static (ctx, _) => GetAutoRegisteredClassDeclarations(ctx))
            .Where(autoRegisteredClass => autoRegisteredClass != null);

        var compilationModel = initialisationContext.CompilationProvider.Combine(autoRegistered.Collect());

        initialisationContext.RegisterSourceOutput(compilationModel, static (sourceContext, source) =>
        {
            Execute(source.Left, source.Right, sourceContext);
        });
    }

    private static AutoRegisteredClass GetAutoRegisteredClassDeclarations(GeneratorSyntaxContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        foreach (var attributeSyntax in classDeclaration.AttributeLists.SelectMany(attributeListSyntax => attributeListSyntax.Attributes))
        {
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
            {
                continue;
            }

            var fullyQualifiedAttributeName = attributeSymbol.ContainingType.ToString();

            if (!RegistrationTypes.TryGetValue(fullyQualifiedAttributeName, out var registrationType))
            {
                continue;
            }

            var symbol = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(classDeclaration)!;
            var typeName = symbol.ToDisplayString();

            var attributeData = symbol.GetFirstAutoRegisterAttribute(fullyQualifiedAttributeName);

            string[] registerAs;
            var serviceKey = string.Empty;

            if (attributeData?.AttributeConstructor?.Parameters.Length > 0 &&
                attributeData?.AttributeConstructor?.Parameters.Any(a => a.Name == SERVICE_KEY) is true)
            {
                serviceKey = attributeData?.ConstructorArguments.First().Value?.ToString()!;
            }

            if (attributeData?.AttributeConstructor?.Parameters.Length > 0 && attributeData.GetIgnoredTypeNames(ONLY_REGISTER_AS) is { Length: > 0 } onlyRegisterAs)
            {
                registerAs = symbol!
                    .AllInterfaces
                    .Select(x => x.ToDisplayString())
                    .Where(x => onlyRegisterAs.Contains(x))
                    .ToArray();
            }
            else
            {
                registerAs = symbol!
                    .Interfaces
                    .Select(x => x.ToDisplayString())
                    .Where(x => !IgnoredInterfaces.Contains(x))
                    .ToArray();
            }

            return new AutoRegisteredClass(
                typeName,
                registrationType,
                registerAs,
                serviceKey);
        }

        return default!;
    }

    private static void Execute(Compilation compilation, ImmutableArray<AutoRegisteredClass> classes, SourceProductionContext context)
    {
        var assemblyNameForMethod = compilation
            .AssemblyName!
            .Replace(".", string.Empty)
            .Replace(" ", string.Empty)
            .Trim();

        // Group by name and type because we want to avoid any partial
        // declarations from popping up twice. Especially true if you
        // use another source generator that makes a partial class/file
        // TODO: Refactor below into more readable code, not everything should be one line of code!
        var registrations = classes
            .GroupBy(x => new { x.ClassName, x.RegistrationType, x.ServiceKey })
            .Select(x => GetRegistration(x.Key.RegistrationType, x.Key.ClassName, x.SelectMany(d => d.Interfaces).ToArray(), x.Key.ServiceKey));

        var formatted = string.Join(Environment.NewLine, registrations);
        var output = SourceConstants.GENERATE_CLASS_SOURCE.Replace("{0}", assemblyNameForMethod).Replace("{1}", formatted);
        context.AddSource("AutoRegisterInject.ServiceCollectionExtension.g.cs", SourceText.From(output, Encoding.UTF8));
        return;

        string GetRegistration(AutoRegistrationType type, string className, string[] interfaces, string serviceKey)
        {
            var hasInterfaces = interfaces.Any();
            var isServiceKeyEmptyOrNull = string.IsNullOrWhiteSpace(serviceKey);

            return type switch
            {
                AutoRegistrationType.Scoped when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_SCOPED_SOURCE, className),
                AutoRegistrationType.Scoped
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_SCOPED_INTERFACE_SOURCE, d, className))),

                AutoRegistrationType.Singleton when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_SINGLETON_SOURCE, className),
                AutoRegistrationType.Singleton
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_SINGLETON_INTERFACE_SOURCE, d, className))),

                AutoRegistrationType.Transient when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_TRANSIENT_SOURCE, className),
                AutoRegistrationType.Transient
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_TRANSIENT_INTERFACE_SOURCE, d, className))),
                
                AutoRegistrationType.TryTransient when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_TRY_TRANSIENT_SOURCE, className),
                AutoRegistrationType.TryTransient
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_TRY_TRANSIENT_INTERFACE_SOURCE, d, className))),
                
                AutoRegistrationType.TryScoped when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_TRY_SCOPED_SOURCE, className),
                AutoRegistrationType.TryScoped
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_TRY_SCOPED_INTERFACE_SOURCE, d, className))),
                
                AutoRegistrationType.TrySingleton when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_TRY_SINGLETON_SOURCE, className),
                AutoRegistrationType.TrySingleton
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_TRY_SINGLETON_INTERFACE_SOURCE, d, className))),
                
                AutoRegistrationType.KeyedScoped when isServiceKeyEmptyOrNull 
                    => throw new ArgumentException(string.Format(KeyedServiceExceptionFormattedMessage, nameof(AutoRegistrationType.KeyedScoped))),
                AutoRegistrationType.KeyedScoped when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_KEYED_SCOPED_SOURCE, className, serviceKey),
                AutoRegistrationType.KeyedScoped
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_KEYED_SCOPED_INTERFACE_SOURCE, d, className, serviceKey))),
                
                AutoRegistrationType.KeyedSingleton when isServiceKeyEmptyOrNull 
                    => throw new ArgumentException(string.Format(KeyedServiceExceptionFormattedMessage, nameof(AutoRegistrationType.KeyedSingleton))),
                AutoRegistrationType.KeyedSingleton when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_KEYED_SINGLETON_SOURCE, className, serviceKey),
                AutoRegistrationType.KeyedSingleton
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_KEYED_SINGLETON_INTERFACE_SOURCE, d, className, serviceKey))),
                
                AutoRegistrationType.KeyedTransient when isServiceKeyEmptyOrNull 
                    => throw new ArgumentException(string.Format(KeyedServiceExceptionFormattedMessage, nameof(AutoRegistrationType.KeyedTransient))),
                AutoRegistrationType.KeyedTransient when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_KEYED_TRANSIENT_SOURCE, className, serviceKey),
                AutoRegistrationType.KeyedTransient
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_KEYED_TRANSIENT_INTERFACE_SOURCE, d, className, serviceKey))),
                
                AutoRegistrationType.TryKeyedScoped when isServiceKeyEmptyOrNull 
                    => throw new ArgumentException(string.Format(KeyedServiceExceptionFormattedMessage, nameof(AutoRegistrationType.TryKeyedScoped))),
                AutoRegistrationType.TryKeyedScoped when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_TRY_KEYED_SCOPED_SOURCE, className, serviceKey),
                AutoRegistrationType.TryKeyedScoped
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_TRY_KEYED_SCOPED_INTERFACE_SOURCE, d, className, serviceKey))),
                
                AutoRegistrationType.TryKeyedSingleton when isServiceKeyEmptyOrNull 
                    => throw new ArgumentException(string.Format(KeyedServiceExceptionFormattedMessage, nameof(AutoRegistrationType.TryKeyedSingleton))),
                AutoRegistrationType.TryKeyedSingleton when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_TRY_KEYED_SINGLETON_SOURCE, className, serviceKey),
                AutoRegistrationType.TryKeyedSingleton
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_TRY_KEYED_SINGLETON_INTERFACE_SOURCE, d, className, serviceKey))),
                
                AutoRegistrationType.TryKeyedTransient when isServiceKeyEmptyOrNull 
                    => throw new ArgumentException(string.Format(KeyedServiceExceptionFormattedMessage, nameof(AutoRegistrationType.TryKeyedTransient))),
                AutoRegistrationType.TryKeyedTransient when !hasInterfaces
                    => string.Format(SourceConstants.GENERATE_TRY_KEYED_TRANSIENT_SOURCE, className, serviceKey),
                AutoRegistrationType.TryKeyedTransient
                    => string.Join(Environment.NewLine, interfaces.Select(d => string.Format(SourceConstants.GENERATE_TRY_KEYED_TRANSIENT_INTERFACE_SOURCE, d, className, serviceKey))),

                AutoRegistrationType.Hosted // Hosted services do not support interfaces at this time
                    => string.Format(SourceConstants.GENERATE_HOSTED_SERVICE_SOURCE, className),

                _ => throw new NotImplementedException("Auto registration type not set up to output"),
            };
        }
    }
}