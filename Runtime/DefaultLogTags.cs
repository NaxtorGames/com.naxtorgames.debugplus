namespace NaxtorGames.Debugging
{
    /// <summary>
    /// A collection of common premade log tags.
    /// </summary>
    public static class DefaultLogTags
    {
        public const string INIT = "Init";
        public const string CLEANUP = "CleanUp";
        public const string AUDIO = "Audio";
        public const string INPUT = "Input";
        public const string CAMERA = "Camera";
        public const string SCENE_MANAGEMENT = "SceneManagement";
        public const string AI = "AI";
        public const string TERRAIN = "Terrain";
        public const string MISC = "Misc";

        public static readonly System.Collections.Generic.IReadOnlyList<string> Collection = new string[]
        {
            INIT,
            CLEANUP,
            AUDIO,
            INPUT,
            CAMERA,
            SCENE_MANAGEMENT,
            AI,
            TERRAIN,
            MISC
        };
    }
}