﻿using System;
using System.Text.Json;

namespace Sentry
{
    /// <summary>
    /// Snapshot of a session.
    /// </summary>
    public class SessionSnapshot : IJsonSerializable
    {
        /// <summary>
        /// Session.
        /// </summary>
        public Session Session { get; }

        /// <summary>
        /// Whether this is the initial snapshot.
        /// </summary>
        public bool IsInitial { get; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Duration of time since start of the session.
        /// </summary>
        public TimeSpan Duration => Timestamp - Session.Timestamp;

        internal SessionSnapshot(Session session, bool isInitial, DateTimeOffset timestamp)
        {
            Session = session;
            IsInitial = isInitial;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SessionSnapshot"/>.
        /// </summary>
        public SessionSnapshot(Session session, bool isInitial)
            : this(session, isInitial, DateTimeOffset.Now)
        {
        }

        /// <inheritdoc />
        public void WriteTo(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WriteString("sid", Session.Id);

            if (!string.IsNullOrWhiteSpace(Session.DistinctId))
            {
                writer.WriteString("did", Session.DistinctId);
            }

            if (IsInitial)
            {
                writer.WriteBoolean("init", IsInitial);
            }

            writer.WriteString("started", Session.Timestamp);

            writer.WriteNumber("duration", (int)Duration.TotalSeconds);

            writer.WriteNumber("errors", Session.ErrorCount);

            // Attributes
            writer.WriteStartObject("attrs");

            writer.WriteString("release", Session.Release);

            if (!string.IsNullOrWhiteSpace(Session.Environment))
            {
                writer.WriteString("environment", Session.Environment);
            }

            if (!string.IsNullOrWhiteSpace(Session.IpAddress))
            {
                writer.WriteString("ip_address", Session.IpAddress);
            }

            if (!string.IsNullOrWhiteSpace(Session.UserAgent))
            {
                writer.WriteString("user_agent", Session.UserAgent);
            }

            writer.WriteEndObject();

            writer.WriteEndObject();
        }

        /// <summary>
        /// Parses <see cref="SessionSnapshot"/> from JSON.
        /// </summary>
        public static SessionSnapshot FromJson(JsonElement json)
        {
            throw new NotImplementedException();
        }
    }
}