﻿using System;

namespace Valkey.Glide;

/// <summary>
/// Specifies when to set the expiry for a key.
/// </summary>
public enum ExpireWhen
{
    /// <summary>
    /// Set expiry whether or not there is an existing expiry.
    /// </summary>
    Always,

    /// <summary>
    /// Set expiry only when the new expiry is greater than current one.
    /// </summary>
    GreaterThanCurrentExpiry,

    /// <summary>
    /// Set expiry only when the key has an existing expiry.
    /// </summary>
    HasExpiry,

    /// <summary>
    /// Set expiry only when the key has no expiry.
    /// </summary>
    HasNoExpiry,

    /// <summary>
    /// Set expiry only when the new expiry is less than current one.
    /// </summary>
    LessThanCurrentExpiry,
}

internal static class ExpiryOptionExtensions
{
    internal static ValkeyValue ToLiteral(this ExpireWhen op) => op switch
    {
        ExpireWhen.HasNoExpiry => ValkeyLiterals.NX,
        ExpireWhen.HasExpiry => ValkeyLiterals.XX,
        ExpireWhen.GreaterThanCurrentExpiry => ValkeyLiterals.GT,
        ExpireWhen.LessThanCurrentExpiry => ValkeyLiterals.LT,
        _ => throw new ArgumentOutOfRangeException(nameof(op)),
    };
}
