#if !DEBUG_PLUS_NO_LOGGING
using System;
using UnityEngine; 
#endif

using Object = UnityEngine.Object;
using Type = System.Type;

namespace NaxtorGames.Debugging
{
    public static class DebugPlus
    {
        public const string LOG_TAG = "[<color=" + CORE_HEX_COLOR + ">DebugPlus</color>]";
        public const string ASSET_FILE_PATH = "Debugging/DebugPlusConfig";

        private const string CORE_HEX_COLOR = "#6495ED";
#if !DEBUG_PLUS_NO_LOGGING
        private const string MESSAGE_HEX_COLOR = "#98FF98";

        private static DebugPlusConfigSo s_debugConfig = null;

        private static DebugPlusConfigSo DebugConfig
        {
            get
            {
                if (s_debugConfig == null)
                {
                    s_debugConfig = Resources.Load<DebugPlusConfigSo>(ASSET_FILE_PATH);

                    if (s_debugConfig == null)
                    {
                        s_debugConfig = ScriptableObject.CreateInstance<DebugPlusConfigSo>();
                        Debug.LogWarning($"{LOG_TAG} No config file found. Fallback instantiated. A 'DebugPlusConfig' has to exist in any resource folder at '{ASSET_FILE_PATH}'");
                    }

                    s_debugConfig.BuildLookup();
                }

                return s_debugConfig;
            }
        }

        [RuntimeInitializeOnLoadMethod()]
        private static void InitializeOnLoad()
        {
            s_debugConfig = null;
        } 
#endif

        public static void LogInfo(string message, Object context, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            if (DebugConfig.ShouldLogInfo == LogType.None)
            {
                return;
            }

            UpdateMessage(ref message, classType);

            Debug.Log(message, context);
#endif
        }

        public static void LogInfo(string message, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogInfo(message, context: null, classType);
#endif
        }

        public static void LogInfo(string message, string tag, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogCustomTag(message, tag, LogLevelType.Info, classType); 
#endif
        }

        public static void LogInfo(string message, Object context, string tag, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogCustomTag(message, context, tag, LogLevelType.Info, classType); 
#endif
        }

        public static void LogWarning(string message, Object context, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            if (DebugConfig.ShouldLogWarning == LogType.None)
            {
                return;
            }

            UpdateMessage(ref message, classType);

            Debug.LogWarning(message, context); 
#endif
        }

        public static void LogWarning(string message, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogWarning(message, context: null, classType); 
#endif
        }

        public static void LogWarning(string message, string tag, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogCustomTag(message, tag, LogLevelType.Warning, classType); 
#endif
        }

        public static void LogWarning(string message, Object context, string tag, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogCustomTag(message, context, tag, LogLevelType.Warning, classType); 
#endif
        }

        public static void LogError(string message, Object context, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            if (DebugConfig.ShouldLogError == LogType.None)
            {
                return;
            }

            UpdateMessage(ref message, classType);

            Debug.LogError(message, context); 
#endif
        }

        public static void LogError(string message, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogError(message, context: null, classType); 
#endif
        }

        public static void LogError(string message, string tag, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogCustomTag(message, tag, LogLevelType.Error, classType); 
#endif
        }

        public static void LogError(string message, Object context, string tag, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogCustomTag(message, context, tag, LogLevelType.Error, classType); 
#endif
        }

        private static void LogCustomTag(string message, Object context, string customTag, LogLevelType logLevel, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogResult result = DebugConfig.ShouldLogWithTag(customTag, logLevel);

            if (result == LogResult.Log)
            {
                UpdateMessage(ref message, classType);

                switch (logLevel)
                {
                    case LogLevelType.Info:
                        if (DebugConfig.ShouldLogInfo == LogType.All)
                        {
                            Debug.Log(message, context);
                        }
                        break;
                    case LogLevelType.Warning:
                        if (DebugConfig.ShouldLogWarning == LogType.All)
                        {
                            Debug.LogWarning(message, context);
                        }
                        break;
                    case LogLevelType.Error:
                        if (DebugConfig.ShouldLogError == LogType.All)
                        {
                            Debug.LogError(message, context);
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (result == LogResult.Missing)
            {
                if (DebugConfig.MissingTagBehavior == MissingTagBehaviorType.Add)
                {
                    if (DebugConfig.AddTag(customTag))
                    {
                        Debug.Log($"{LOG_TAG} Tag {customTag} was added.");

                        UpdateMessage(ref message, classType);

                        switch (logLevel)
                        {
                            case LogLevelType.Info:
                                Debug.Log(message, context);
                                break;
                            case LogLevelType.Warning:
                                Debug.LogWarning(message, context);
                                break;
                            case LogLevelType.Error:
                                Debug.LogError(message, context);
                                break;
                            default:
                                break;
                        }
                        return;
                    }
                }

                if (DebugConfig.MissingTagBehavior == MissingTagBehaviorType.Print)
                {
                    UpdateMessage(ref message, classType);
                    message = $"{LOG_TAG} Tag '{customTag}' is not available. Original Message below:\n{message}";

                    switch (logLevel)
                    {
                        case LogLevelType.Info:
                            Debug.Log(message, context);
                            break;
                        case LogLevelType.Warning:
                            Debug.LogWarning(message, context);
                            break;
                        case LogLevelType.Error:
                            Debug.LogError(message, context);
                            break;
                        default:
                            break;
                    }
                    return;
                }

                if (DebugConfig.MissingTagBehavior != MissingTagBehaviorType.Ignore)
                {
                    string missingTagMessage = $"Tag '{customTag}' dose not exist.";
                    UpdateMessage(ref missingTagMessage, typeof(DebugPlus));

                    switch (DebugConfig.MissingTagBehavior)
                    {
                        case MissingTagBehaviorType.Info:
                            Debug.Log(missingTagMessage);
                            break;
                        case MissingTagBehaviorType.Warning:
                            Debug.LogWarning(missingTagMessage);
                            break;
                        case MissingTagBehaviorType.Error:
                            Debug.LogError(missingTagMessage);
                            break;
                        default:
                            break;
                    }
                }
                return;
            } 
#endif
        }

        private static void LogCustomTag(string message, string customTag, LogLevelType logLevel, Type classType = null)
        {
#if !DEBUG_PLUS_NO_LOGGING
            LogCustomTag(message, null, customTag, logLevel, classType); 
#endif
        }

        private static void UpdateMessage(ref string message, Type classType)
        {
#if !DEBUG_PLUS_NO_LOGGING
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "missing message";
            }

            if (DebugConfig.UseClassPrefix && classType != null)
            {
                string className = classType.Name;

                if (DebugConfig.RemoveClassSuffixes)
                {
                    const string MONO_SUFFIX = "Mono";
                    const string SO_SUFFIX = "So";
                    const string SO_SUFFIX_CAPITAL = "SO";

                    RemoveSuffixes(ref className, MONO_SUFFIX, SO_SUFFIX, SO_SUFFIX_CAPITAL);
                }

                message = $"[<color={MESSAGE_HEX_COLOR}>{className}</color>] {message}";
            } 
#endif
        }

        /// <summary>
        /// Removes the first matching suffix from the input string, if any of the provided suffixes are found at the end.
        /// </summary>
        /// <param name="value">The string to process. Will be updated if a matching suffix is removed.</param>
        /// <param name="suffixes">A list of suffix candidates to check against the end of the string. Only the first match is removed.</param>
        private static void RemoveSuffixes(ref string value, params string[] suffixes)
        {
#if !DEBUG_PLUS_NO_LOGGING
            if (suffixes == null || suffixes.Length == 0)
            {
                return;
            }

            ReadOnlySpan<char> valueSpan = value.AsSpan();

            foreach (string suffix in suffixes)
            {
                if (string.IsNullOrEmpty(suffix))
                {
                    continue;
                }

                if (valueSpan.EndsWith(suffix.AsSpan()))
                {
                    valueSpan = valueSpan.Slice(0, valueSpan.Length - suffix.Length);
                    break;
                }
            }

            value = valueSpan.ToString(); 
#endif
        }
    }
}