using UnityEditor;
using UnityEngine;

namespace NaxtorGames.Debugging.EditorScripts
{
    [CustomPropertyDrawer(typeof(DebugPlusCustomTag))]
    public sealed class DebugPlusCustomTagProperty : PropertyDrawer
    {
        private const string ICON_INACTIVE = ".inactive.sml";
        private const string ICON_INFO_NORMAL = "console.infoicon";
        private const string ICON_INFO_INACTIVE = ICON_INFO_NORMAL + ICON_INACTIVE;
        private const string ICON_WARNING_NORMAL = "console.warnicon";
        private const string ICON_WARNING_INACTIVE = ICON_WARNING_NORMAL + ICON_INACTIVE;
        private const string ICON_ERROR_NORMAL = "console.erroricon";
        private const string ICON_ERROR_INACTIVE = ICON_ERROR_NORMAL + ICON_INACTIVE;

        private const string LOG_INFO_TOOLTIP = "Should logs with this tag get printed at 'Info'-Level.";
        private const string LOG_WARNING_TOOLTIP = "Should logs with this tag get printed at 'Warning'-Level.";
        private const string LOG_ERROR_TOOLTIP = "Should logs with this tag get printed at 'Error'-Level.";

        private const string PROPERTY_NAME = "_name";
        private const string PROPERTY_LOG_INFO = "_logInfo";
        private const string PROPERTY_LOG_WARNING = "_logWarning";
        private const string PROPERTY_LOG_ERROR = "_logError";

        private const float SPACING = 2.0f;
        private const float ICON_SPACING = 2.0f;
        private const float TOGGLE_WIDTH = 16.0f;
        private const float LABEL_WIDTH = 24f;
        private const float MIN_NAME_FIELD_WIDTH = 64.0f;
        private const float NAME_LABEL_WIDTH = LABEL_WIDTH;
        private const float ICON_WIDTH = 18.0f;

        private static readonly GUIContent s_infoIcon = new GUIContent(null, null, LOG_INFO_TOOLTIP);
        private static readonly GUIContent s_warnIcon = new GUIContent(null, null, LOG_WARNING_TOOLTIP);
        private static readonly GUIContent s_errorIcon = new GUIContent(null, null, LOG_ERROR_TOOLTIP);
        private static readonly GUIContent s_infoIconInactive = new GUIContent(null, null, LOG_INFO_TOOLTIP);
        private static readonly GUIContent s_warnIconInactive = new GUIContent(null, null, LOG_WARNING_TOOLTIP);
        private static readonly GUIContent s_errorIconInactive = new GUIContent(null, null, LOG_ERROR_TOOLTIP);

        private static readonly GUIContent s_hiddenLabel = GUIContent.none;
        private static readonly GUIContent s_nameLabel = new GUIContent("Tag", "The tag name. Cannot be null, empty or only white spaces.");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ValidateIcons();

            _ = EditorGUI.BeginProperty(position, label, property);

            const float USED_WIDTH = NAME_LABEL_WIDTH + (3.0f * (SPACING + ICON_WIDTH + ICON_SPACING + TOGGLE_WIDTH));

            float remainingSpace = position.width - USED_WIDTH;

            Rect nameLabelRect = new Rect(position.x, position.y, NAME_LABEL_WIDTH, position.height);
            Rect nameFieldRect = new Rect(nameLabelRect.xMax + SPACING, position.y, Mathf.Max(remainingSpace, MIN_NAME_FIELD_WIDTH), position.height);

            Rect infoLabelRect = new Rect(nameFieldRect.xMax + SPACING, position.y, ICON_WIDTH, position.height);
            Rect infoToggleRect = new Rect(infoLabelRect.xMax + ICON_SPACING, position.y, TOGGLE_WIDTH, position.height);

            Rect warnLabelRect = new Rect(infoToggleRect.xMax + SPACING, position.y, ICON_WIDTH, position.height);
            Rect warnToggleRect = new Rect(warnLabelRect.xMax + ICON_SPACING, position.y, TOGGLE_WIDTH, position.height);

            Rect errorLabelRect = new Rect(warnToggleRect.xMax + SPACING, position.y, ICON_WIDTH, position.height);
            Rect errorToggleRect = new Rect(errorLabelRect.xMax + ICON_SPACING, position.y, TOGGLE_WIDTH, position.height);

            SerializedProperty tagNameProperty = property.FindPropertyRelative(PROPERTY_NAME);
            if (string.IsNullOrWhiteSpace(tagNameProperty.stringValue))
            {
                GUI.backgroundColor = Color.red;
            }

            EditorGUI.LabelField(nameLabelRect, s_nameLabel);

            _ = EditorGUI.PropertyField(nameFieldRect, tagNameProperty, s_hiddenLabel);

            GUI.backgroundColor = Color.white;

            DrawToggle(infoLabelRect, infoToggleRect, PROPERTY_LOG_INFO, s_infoIcon, s_infoIconInactive);
            DrawToggle(warnLabelRect, warnToggleRect, PROPERTY_LOG_WARNING, s_warnIcon, s_warnIconInactive);
            DrawToggle(errorLabelRect, errorToggleRect, PROPERTY_LOG_ERROR, s_errorIcon, s_errorIconInactive);

            EditorGUI.EndProperty();

            void DrawToggle(Rect labelRect, Rect propertyRect, string propertyName, GUIContent labelNormal, GUIContent labelInactive)
            {
                SerializedProperty toggleProperty = property.FindPropertyRelative(propertyName);

                GUIContent label = toggleProperty.boolValue ? labelNormal : labelInactive;
                EditorGUI.LabelField(labelRect, label);

                _ = EditorGUI.PropertyField(propertyRect, toggleProperty, s_hiddenLabel);
            }
        }

        private static void ValidateIcons()
        {
            UpdateIcon(s_infoIcon, ICON_INFO_NORMAL);
            UpdateIcon(s_infoIconInactive, ICON_INFO_INACTIVE);
            UpdateIcon(s_warnIcon, ICON_WARNING_NORMAL);
            UpdateIcon(s_warnIconInactive, ICON_WARNING_INACTIVE);
            UpdateIcon(s_errorIcon, ICON_ERROR_NORMAL);
            UpdateIcon(s_errorIconInactive, ICON_ERROR_INACTIVE);

            static void UpdateIcon(GUIContent label, string iconName)
            {
                if (label.image != null)
                {
                    return;
                }

                GUIContent iconContent = EditorGUIUtility.IconContent(iconName);
                if (iconContent != null)
                {
                    label.image = iconContent.image;
                }
            }
        }
    }
}