using System;
using UnityEngine;

namespace NaxtorGames.Debugging
{
    [Serializable]
    public sealed class DebugPlusCustomTag : IEquatable<DebugPlusCustomTag>
    {
        [Tooltip("The name of the tag.")]
        [SerializeField] private string _name = "tag name";
        [Tooltip("Should info messages with this tag be logged?")]
        [SerializeField] private bool _logInfo = true;
        [Tooltip("Should warning messages with this tag be logged?")]
        [SerializeField] private bool _logWarning = true;
        [Tooltip("Should error messages with this tag be logged?")]
        [SerializeField] private bool _logError = true;

        /// <summary>
        /// Gets or sets the name of the tag.
        /// <para>'value' will only be set when its not null, empty or white space.</para>
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? _name : value;
        }
        /// <summary>
        /// Gets or sets if info messages with this tag should be logged.
        /// </summary>
        public bool LogInfo
        {
            get => _logInfo;
            set => _logInfo = value;
        }
        /// <summary>
        /// Gets or sets if warning messages with this tag should be logged.
        /// </summary>
        public bool LogWarning
        {
            get => _logWarning;
            set => _logWarning = value;
        }
        /// <summary>
        /// Gets or sets if error messages with this tag should be logged.
        /// </summary>
        public bool LogError
        {
            get => _logError;
            set => _logError = value;
        }

        /// <summary>
        /// Creates a new custom tag.
        /// </summary>
        /// <param name="tagName">The name of the tag. Is not allowed to be null, empty or withe space.</param>
        /// <param name="logInfo">Should info messages with this tag be logged?</param>
        /// <param name="logWarning">Should warning messages with this tag be logged?</param>
        /// <param name="logError">Should error messages with this tag be logged?</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tagName"/> is null, empty or white space.</exception>
        public DebugPlusCustomTag(string tagName, bool logInfo = true, bool logWarning = true, bool logError = true)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new ArgumentNullException(nameof(tagName), "Tag cannot be null or empty!");
            }

            _name = tagName;
            _logInfo = logInfo;
            _logWarning = logWarning;
            _logError = logError;
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as DebugPlusCustomTag);
        }

        public bool Equals(DebugPlusCustomTag otherCustomTag)
        {
            if (otherCustomTag is null)
            {
                return false;
            }

            return string.Equals(_name, otherCustomTag._name, StringComparison.Ordinal)
                && _logInfo == otherCustomTag._logInfo
                && _logWarning == otherCustomTag._logWarning
                && _logError == otherCustomTag._logError;
        }

        public sealed override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (string.IsNullOrWhiteSpace(_name) ? 0 : _name.GetHashCode());
                hash = (hash * 23) + _logInfo.GetHashCode();
                hash = (hash * 23) + _logWarning.GetHashCode();
                hash = (hash * 23) + _logError.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(DebugPlusCustomTag left, DebugPlusCustomTag right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DebugPlusCustomTag left, DebugPlusCustomTag right)
        {
            return !Equals(left, right);
        }
    }
}