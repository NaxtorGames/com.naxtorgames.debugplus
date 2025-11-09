using UnityEditor;
using UnityEngine;

namespace NaxtorGames.Debugging.EditorScripts
{
    [CustomEditor(typeof(DebugPlusConfigSo))]
    public sealed class DebugPlusConfigSoEditor : Editor
    {
        private const string USE_CLASS_PREFIX_PROPERTY_NAME = "_useClassPrefix";
        private const string REMOVE_CLASS_SUFFIXES_PROPERTY_NAME = "_removeClassSuffixes";
        private const string MISSING_TAG_BEHAVIOR_PROPERTY_NAME = "_missingTagBehavior";
        private const string LOG_INFO_PROPERTY_NAME = "_logInfo";
        private const string LOG_WARNING_PROPERTY_NAME = "_logWarning";
        private const string LOG_ERROR_PROPERTY_NAME = "_logError";
        private const string CUSTOM_TAGS_PROPERTY_NAME = "_customTags";

        private DebugPlusConfigSo _script = null;
        private SerializedProperty _useClassPrefixProperty = null;
        private SerializedProperty _removeClassSuffixesProperty = null;
        private SerializedProperty _missingTagProperty = null;
        private SerializedProperty _logInfoProperty = null;
        private SerializedProperty _logWarningProperty = null;
        private SerializedProperty _logErrorProperty = null;
        private SerializedProperty _customTagsProperty = null;
        private bool _isInValidFolder = false;
        private bool _isValidName = false;

        private void OnEnable()
        {
            if (this.target != _script)
            {
                _script = this.target as DebugPlusConfigSo;
                _useClassPrefixProperty = this.serializedObject.FindProperty(USE_CLASS_PREFIX_PROPERTY_NAME);
                _removeClassSuffixesProperty = this.serializedObject.FindProperty(REMOVE_CLASS_SUFFIXES_PROPERTY_NAME);
                _missingTagProperty = this.serializedObject.FindProperty(MISSING_TAG_BEHAVIOR_PROPERTY_NAME);
                _logInfoProperty = this.serializedObject.FindProperty(LOG_INFO_PROPERTY_NAME);
                _logWarningProperty = this.serializedObject.FindProperty(LOG_WARNING_PROPERTY_NAME);
                _logErrorProperty = this.serializedObject.FindProperty(LOG_ERROR_PROPERTY_NAME);
                _customTagsProperty = this.serializedObject.FindProperty(CUSTOM_TAGS_PROPERTY_NAME);

                _isInValidFolder = IsInResourcesFolder(this.target);
                _isValidName = this.target.name == DebugPlusConfigSo.FILE_NAME;
            }
        }

        private void OnDisable()
        {
            _script = null;
            _useClassPrefixProperty = null;
            _removeClassSuffixesProperty = null;
            _missingTagProperty = null;
            _logInfoProperty = null;
            _logWarningProperty = null;
            _logErrorProperty = null;
            _customTagsProperty = null;
        }

        public sealed override void OnInspectorGUI()
        {
            DrawHeader("Info");

            const string INFO_FILE_REQUIREMENTS = "File requirements:\nName: '" + DebugPlusConfigSo.FILE_NAME + "'\nFolder: 'Resources/Debugging'";
            EditorGUILayout.HelpBox(INFO_FILE_REQUIREMENTS, MessageType.None);
            if (_isInValidFolder && _isValidName)
            {
                const string INFO_ALL_CORRECT = "The name and folder for this asset is correct!";
                EditorGUILayout.HelpBox(INFO_ALL_CORRECT, MessageType.Info);
            }
            else
            {
                if (!_isInValidFolder)
                {
                    const string INFO_INVALID_FOLDER = "This file is not in a 'Resources/Debugging' folder and cannot be found by the debugger.";
                    EditorGUILayout.HelpBox(INFO_INVALID_FOLDER, MessageType.Error);
                }
                if (!_isValidName)
                {
                    const string INFO_INVALID_NAME = "This file has not the correct name of '" + DebugPlusConfigSo.FILE_NAME + "' and cannot be found by the debugger.";
                    EditorGUILayout.HelpBox(INFO_INVALID_NAME, MessageType.Error);
                }
            }

            EditorGUILayout.Space();

            this.serializedObject.Update();

            _ = EditorGUILayout.PropertyField(_useClassPrefixProperty);
            _ = EditorGUILayout.PropertyField(_removeClassSuffixesProperty);

            DrawHeader("Missing Tags");
            _ = EditorGUILayout.PropertyField(_missingTagProperty);
            DrawMissingTagOptions();

            DrawHeader("Log Levels");
            _ = EditorGUILayout.PropertyField(_logInfoProperty);
            _ = EditorGUILayout.PropertyField(_logWarningProperty);
            _ = EditorGUILayout.PropertyField(_logErrorProperty);

            DrawHeader("Tags");
            _ = EditorGUILayout.PropertyField(_customTagsProperty);

            if (GUILayout.Button("Remove Invalid / Duplicated Tags"))
            {
                _script.RemoveInvalidTags();
            }
            if (GUILayout.Button("Add Missing Default Tags"))
            {
                _script.AddMissingDefaultTags();
            }

            _ = this.serializedObject.ApplyModifiedProperties();
        }

        private void DrawMissingTagOptions()
        {
            string infoText = (MissingTagBehaviorType)_missingTagProperty.enumValueIndex switch
            {
                MissingTagBehaviorType.Ignore => "Dose not print any hint or message of missing tags.",
                MissingTagBehaviorType.Info => "Informs about the missing tag at 'Info'-log level. Dose not show the message.",
                MissingTagBehaviorType.Warning => "Informs about the missing tag at 'Warning'-log level. Dose not show the message.",
                MissingTagBehaviorType.Error => "Informs about the missing tag at 'Error'-log level. Dose not show the message.",
                MissingTagBehaviorType.Print => "Informs about the missing tag. Also prints the message. Log level is the original message log level.",
                MissingTagBehaviorType.Add => "Adds the missing tag to the 'Custom Tag'-list.",
                _ => "invalid selection",
            };

            EditorGUILayout.HelpBox(infoText, MessageType.None);
        }

        private static bool IsInResourcesFolder(UnityEngine.Object asset)
        {
            if (asset == null)
            {
                return false;
            }

            string path = AssetDatabase.GetAssetPath(asset);

            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            return path.Contains(DebugPlus.ASSET_FILE_PATH);
        }

        private void DrawHeader(string text)
        {
            EditorGUILayout.Space(3.0f);
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        }
    }
}
