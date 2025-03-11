// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Ran.Generators;

internal enum AutoRegistrationType
{
    Scoped,
    Singleton,
    Transient,
    Hosted,
    KeyedScoped,
    KeyedSingleton,
    KeyedTransient,
    TryScoped,
    TrySingleton,
    TryTransient,
    TryKeyedScoped,
    TryKeyedSingleton,
    TryKeyedTransient
}