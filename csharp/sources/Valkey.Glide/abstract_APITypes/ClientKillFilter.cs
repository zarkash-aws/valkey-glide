﻿using System;
using System.Collections.Generic;
using System.Net;

namespace Valkey.Glide;

/// <summary>
/// Filter determining which Valkey clients to kill.
/// </summary>
/// <seealso href="https://valkey.io/commands/client-kill/" />
public class ClientKillFilter
{
    /// <summary>
    /// Filter arguments builder for `CLIENT KILL`.
    /// </summary>
    public ClientKillFilter() { }

    /// <summary>
    /// The ID of the client to kill.
    /// </summary>
    public long? Id { get; private set; }

    /// <summary>
    /// The type of client.
    /// </summary>
    public ClientType? ClientType { get; private set; }

    /// <summary>
    /// The authenticated ACL username.
    /// </summary>
    public string? Username { get; private set; }

    /// <summary>
    /// The endpoint to kill.
    /// </summary>
    public EndPoint? Endpoint { get; private set; }

    /// <summary>
    /// The server endpoint to kill.
    /// </summary>
    public EndPoint? ServerEndpoint { get; private set; }

    /// <summary>
    /// Whether to skip the current connection.
    /// </summary>
    public bool? SkipMe { get; private set; }

    /// <summary>
    /// Age of connection in seconds.
    /// </summary>
    public long? MaxAgeInSeconds { get; private set; }

    /// <summary>
    /// Sets client id filter.
    /// </summary>
    /// <param name="id">Id of the client to kill.</param>
    public ClientKillFilter WithId(long? id)
    {
        Id = id;
        return this;
    }

    /// <summary>
    /// Sets client type filter.
    /// </summary>
    /// <param name="clientType">The type of the client.</param>
    public ClientKillFilter WithClientType(ClientType? clientType)
    {
        ClientType = clientType;
        return this;
    }

    /// <summary>
    /// Sets the username filter.
    /// </summary>
    /// <param name="username">Authenticated ACL username.</param>
    public ClientKillFilter WithUsername(string? username)
    {
        Username = username;
        return this;
    }

    /// <summary>
    /// Set the endpoint filter.
    /// </summary>
    /// <param name="endpoint">The endpoint to kill.</param>
    public ClientKillFilter WithEndpoint(EndPoint? endpoint)
    {
        Endpoint = endpoint;
        return this;
    }

    /// <summary>
    /// Set the server endpoint filter.
    /// </summary>
    /// <param name="serverEndpoint">The server endpoint to kill.</param>
    public ClientKillFilter WithServerEndpoint(EndPoint? serverEndpoint)
    {
        ServerEndpoint = serverEndpoint;
        return this;
    }

    /// <summary>
    /// Set the skipMe filter (whether to skip the current connection).
    /// </summary>
    /// <param name="skipMe">Whether to skip the current connection.</param>
    public ClientKillFilter WithSkipMe(bool? skipMe)
    {
        SkipMe = skipMe;
        return this;
    }

    /// <summary>
    /// Set the MaxAgeInSeconds filter.
    /// </summary>
    /// <param name="maxAgeInSeconds">Age of connection in seconds.</param>
    public ClientKillFilter WithMaxAgeInSeconds(long? maxAgeInSeconds)
    {
        MaxAgeInSeconds = maxAgeInSeconds;
        return this;
    }

    internal List<ValkeyValue> ToList(bool withReplicaCommands)
    {
        var parts = new List<ValkeyValue>(15)
        {
            ValkeyLiterals.KILL,
        };
        if (Id != null)
        {
            parts.Add(ValkeyLiterals.ID);
            parts.Add(Id.Value);
        }
        if (ClientType != null)
        {
            parts.Add(ValkeyLiterals.TYPE);
            switch (ClientType.Value)
            {
                case Glide.ClientType.Normal:
                    parts.Add(ValkeyLiterals.normal);
                    break;
                case Glide.ClientType.Replica:
                    parts.Add(withReplicaCommands ? ValkeyLiterals.replica : ValkeyLiterals.slave);
                    break;
                case Glide.ClientType.PubSub:
                    parts.Add(ValkeyLiterals.pubsub);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ClientType));
            }
        }
        if (Username != null)
        {
            parts.Add(ValkeyLiterals.USERNAME);
            parts.Add(Username);
        }
        if (Endpoint != null)
        {
            parts.Add(ValkeyLiterals.ADDR);
            parts.Add((ValkeyValue)Format.ToString(Endpoint));
        }
        if (ServerEndpoint != null)
        {
            parts.Add(ValkeyLiterals.LADDR);
            parts.Add((ValkeyValue)Format.ToString(ServerEndpoint));
        }
        if (SkipMe != null)
        {
            parts.Add(ValkeyLiterals.SKIPME);
            parts.Add(SkipMe.Value ? ValkeyLiterals.yes : ValkeyLiterals.no);
        }
        if (MaxAgeInSeconds != null)
        {
            parts.Add(ValkeyLiterals.MAXAGE);
            parts.Add(MaxAgeInSeconds);
        }
        return parts;
    }
}
