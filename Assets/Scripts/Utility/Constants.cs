namespace CaromBilliards3D.Utility
{
    public static class Constants
    {
        // File related
        public const string EXTENSION_SAVE_FILES = ".dat";
        public const string DIRECTORY_PATH_SAVES = "Saves";
        public const string FILE_NAME_SETTINGS = "GameSettings";
        public const string FILE_NAME_LAST_SESSION = "LastSession";

        // Scene related
        public const int SCENE_BUILD_INDEX_INITIALIZATION = 0;
        public const int SCENE_BUILD_INDEX_START_MENU = 1;
        public const int SCENE_BUILD_INDEX_MAIN = 2;
        public const string SCENE_NAME_RUNTIME_PHYSICS_SIMULATION = "BallPhysicsSimulationScene";

        // Event related
        public const string CUE_BALL_HIT_FORCE_PERCENT_CHANGED = "CUE_BALL_HIT_FORCE_PERCENT_CHANGED";
        public const string CUE_BALL_HIT_FORCE_VALUE_CHANGED = "CUE_BALL_HIT_FORCE_VALUE_CHANGED";
        public const string SESSION_DATA_TIME_UPDATED = "SESSION_DATA_TIME_UPDATED";
        public const string SESSION_DATA_SHOTS_UPDATED = "SESSION_DATA_SHOTS_UPDATED";
        public const string SESSION_DATA_SCORE_UPDATED = "SESSION_DATA_SCORE_UPDATED";
        public const string GAME_OVER = "GAME_OVER";
        public const string GUI_REPLAY_BUTTON_CLICKED = "GUI_REPLAY_BUTTON_CLICKED";
        public const string GUI_REPLAY_POSSIBILITY_CHANGED = "GUI_REPLAY_POSSIBILITY_CHANGED";
        public const string GUI_REPLAY_STATE_CHANGED = "GUI_REPLAY_STATE_CHANGED";
        public const string AUDIO_BALL_HIT_BALL = "AUDIO_BALL_HIT_BALL";
        public const string AUDIO_BALL_HIT_WALL = "AUDIO_BALL_HIT_WALL";

        // Gameplay related
        public const int GAMEPLAY_TOTAL_TARGET_BALL_COUNT = 2;
        public const int GAMEPLAY_TOTAL_SCORES_TO_WIN = 3;
    }
}
