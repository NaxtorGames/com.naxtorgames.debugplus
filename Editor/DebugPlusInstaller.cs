using System;
using UnityEditor;
using UnityEditor.Build;

namespace NaxtorGames.Debugging.EditorScripts
{
    static class DebugPlusInstaller
    {
        private const string DEFINE_NAME = "DEBUG_PLUS";

#if UNITY_2021_2_OR_NEWER
        private static readonly NamedBuildTarget[] s_buildTargets = new[]
        {
            NamedBuildTarget.Standalone,
            NamedBuildTarget.Android,
            NamedBuildTarget.iOS,
            NamedBuildTarget.WebGL,
            NamedBuildTarget.Server,
        };
#else
        private static readonly BuildTargetGroup[] s_targetGroups = new[]
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
            BuildTargetGroup.WebGL,
        };
#endif

#if !DEBUG_PLUS
        [InitializeOnLoadMethod()]
        private static void OnLoad()
        {
#if UNITY_2021_2_OR_NEWER
            AddDefine(s_buildTargets, DEFINE_NAME);
#else
            AddDefine(s_targetGroups, DEFINE_NAME);
#endif
        }
#endif

#if UNITY_2021_2_OR_NEWER
        private static void AddDefine(NamedBuildTarget[] buildTargets, string defineName)
        {
            if (buildTargets == null || buildTargets.Length == 0 || string.IsNullOrWhiteSpace(defineName))
            {
                return;
            }

            foreach (NamedBuildTarget buildTarget in buildTargets)
            {
                try
                {
                    PlayerSettings.GetScriptingDefineSymbols(buildTarget, out string[] defines);

                    bool hasDefine = false;
                    if (defines == null)
                    {
                        defines = Array.Empty<string>();
                    }
                    else
                    {
                        foreach (string define in defines)
                        {
                            if (define == defineName)
                            {
                                hasDefine = true;
                                break;
                            }
                        }
                    }

                    if (!hasDefine)
                    {
                        string[] newDefines = new string[defines.Length + 1];
                        Array.Copy(defines, newDefines, defines.Length);
                        defines = newDefines;
                        defines[defines.Length - 1] = defineName;

                        PlayerSettings.SetScriptingDefineSymbols(buildTarget, defines);

                        UnityEngine.Debug.Log($"Define: '{defineName}' was added to '{buildTarget.TargetName}'.");
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogWarning($"Could not add define '{defineName}'. Exception: {e}");
                }
            }
        }
#else
        private static void AddDefine(BuildTargetGroup[] targetGroups, string defineName)
        {
            if (targetGroups == null || targetGroups.Length == 0 || string.IsNullOrWhiteSpace(defineName))
            {
                return;
            }

            const char SEPERATOR = ';';

            foreach (BuildTargetGroup targetGroup in targetGroups)
            {
                try
                {
                    string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
                    System.Collections.Generic.List<string> defineList = new System.Collections.Generic.List<string>(defines.Split(SEPERATOR, StringSplitOptions.RemoveEmptyEntries));

                    if (defineList.Contains(defineName))
                    {
                        continue;
                    }

                    defineList.Add(defineName);

                    defines = string.Join(SEPERATOR, defineList);

                    PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);

                    UnityEngine.Debug.Log($"Define: '{defineName}' was added to '{targetGroup}'.");
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogWarning($"Could not add define '{defineName}'. Exception: {e}");
                }
            }
        }
#endif
    }
}