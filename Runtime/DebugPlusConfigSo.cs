using System.Collections.Generic;
using UnityEngine;

namespace NaxtorGames.Debugging
{
    [CreateAssetMenu(fileName = FILE_NAME, menuName = "Debug Plus/" + "Debug Plus Configuration")]
    public sealed class DebugPlusConfigSo : ScriptableObject
    {
        public const string FILE_NAME = "DebugPlusConfig";

        private const string CONTEXT_MENU_TAGS = "Tags/";
        private const string CONTEXT_MENU_LOOKUP = "Lookup/";

        private readonly Dictionary<string, DebugPlusCustomTag> _tagLookup = new Dictionary<string, DebugPlusCustomTag>();

        [Tooltip("If enabled, messages will be prefixed with the class name (e.g. '[ClassName] My Message').")]
        [SerializeField] private bool _useClassPrefix = true;
        [Tooltip("If enabled, removes class suffixes like 'Mono' or 'So'/'SO'.")]
        [SerializeField] private bool _removeClassSuffixes = true;

        [Tooltip("Specifies what the debugger should do when a tag is missing.")]
        [SerializeField] private MissingTagBehaviorType _missingTagBehavior = MissingTagBehaviorType.Ignore;

        [SerializeField] private LogType _logInfo = LogType.All;
        [SerializeField] private LogType _logWarning = LogType.All;
        [SerializeField] private LogType _logError = LogType.All;

        [SerializeField] private List<DebugPlusCustomTag> _customTags = new List<DebugPlusCustomTag>();

        private bool _isLookupBuild = false;

        /// <summary>
        /// If enabled, messages will be prefixed with the class name (e.g. '[ClassName] My Message').
        /// </summary>
        public bool UseClassPrefix
        {
            get => _useClassPrefix;
            set => _useClassPrefix = value;
        }
        /// <summary>
        /// If enabled, removes class suffixes like 'Mono' or 'So'/'SO'.
        /// </summary>
        public bool RemoveClassSuffixes
        {
            get => _removeClassSuffixes;
            set => _removeClassSuffixes = value;
        }
        /// <summary>
        /// Specifies what the debugger should do when a tag is missing.
        /// </summary>
        public MissingTagBehaviorType MissingTagBehavior
        {
            get => _missingTagBehavior;
            set => _missingTagBehavior = value;
        }
        public LogType ShouldLogInfo
        {
            get => _logInfo;
            set => _logInfo = value;
        }
        public LogType ShouldLogWarning
        {
            get => _logWarning;
            set => _logWarning = value;
        }
        public LogType ShouldLogError
        {
            get => _logError;
            set => _logError = value;
        }

        private void OnEnable()
        {
            _tagLookup.Clear();
            _isLookupBuild = false;
        }

        public LogResult ShouldLogWithTag(string tag, LogLevelType logLevel)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return LogResult.DoNotLog;
            }

            if (!_isLookupBuild)
            {
                BuildLookup();
            }

            if (_tagLookup.TryGetValue(tag, out DebugPlusCustomTag customConfig))
            {
                bool shouldLog = logLevel switch
                {
                    LogLevelType.Info => customConfig.LogInfo,
                    LogLevelType.Warning => customConfig.LogWarning,
                    LogLevelType.Error => customConfig.LogError,
                    _ => false,
                };

                return shouldLog ? LogResult.Log : LogResult.DoNotLog;
            }
            else
            {
                return LogResult.Missing;
            }
        }

        public bool AddTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return false;
            }

            return AddTag(new DebugPlusCustomTag(tag));
        }

        public bool AddTag(DebugPlusCustomTag customTag)
        {
            if (customTag == null
                || _customTags.Contains(customTag))
            {
                return false;
            }

            _customTags.Add(customTag);

            if (_isLookupBuild)
            {
                _tagLookup.Add(customTag.Name, customTag);
            }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
            return true;
        }

        public bool RemoveTag(string tag)
        {
            bool removedFromTags = false;
            for (int i = 0; i < _customTags.Count; i++)
            {
                if (_customTags[i].Name == tag)
                {
                    _customTags.RemoveAt(i);
                    removedFromTags = true;
                    break;
                }
            }

#if UNITY_EDITOR
            if (removedFromTags)
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
            bool removedFromLookup = _tagLookup.Remove(tag);

            return removedFromTags || removedFromLookup;
        }

        [ContextMenu(CONTEXT_MENU_LOOKUP + "Rebuild", false)]
        public void BuildLookup()
        {
            _tagLookup.Clear();

            for (int i = 0; i < _customTags.Count; i++)
            {
                DebugPlusCustomTag config = _customTags[i];
                if (_tagLookup.ContainsKey(config.Name))
                {
                    Debug.LogError($"{DebugPlus.LOG_TAG} '{config.Name}' already exists. Item '{i}' should be removed.");
                }
                else
                {
                    _tagLookup.Add(config.Name, config);
                }
            }

            _isLookupBuild = true;
        }

        [ContextMenu(CONTEXT_MENU_TAGS + "Remove Invalid or Duplicated")]
        public void RemoveInvalidTags()
        {
            if (_customTags == null || _customTags.Count == 0)
            {
                return;
            }

#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "Removed invalid tags");
#endif

            HashSet<string> existingTags = new HashSet<string>();

            for (int i = 0; i < _customTags.Count;)
            {
                DebugPlusCustomTag customTag = _customTags[i];
                if (IsValid(customTag))
                {
                    i++;
                }
                else
                {
                    _customTags.RemoveAt(i);
                }
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif

            bool IsValid(DebugPlusCustomTag customTag)
            {
                return customTag != null
                    && !string.IsNullOrWhiteSpace(customTag.Name)
                    && existingTags.Add(customTag.Name);
            }
        }

        [ContextMenu(CONTEXT_MENU_LOOKUP + "Clear")]
        public void ClearLookup()
        {
            _tagLookup.Clear();
            _isLookupBuild = false;
        }

        [ContextMenu(CONTEXT_MENU_TAGS + "Add Missing Defaults")]
        public void AddMissingDefaultTags()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "Added missing default tags");
#endif

            HashSet<string> existingTags = new HashSet<string>();

            foreach (DebugPlusCustomTag customTag in _customTags)
            {
                if (!string.IsNullOrWhiteSpace(customTag.Name))
                {
                    existingTags.Add(customTag.Name);
                }
            }

            foreach (string tag in DefaultLogTags.Collection)
            {
                if (!existingTags.Contains(tag))
                {
                    _customTags.Add(new DebugPlusCustomTag(tag));
                }
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        [ContextMenu(CONTEXT_MENU_LOOKUP + "Rebuild", true)]
        private bool BuildLookup_Validate() => Application.isPlaying;
    }
}