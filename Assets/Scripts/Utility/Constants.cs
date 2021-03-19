using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaromBilliards3D.Utility
{
    public static class Constants
    {
        // File related
        public const string EXTENSION_SAVE_FILES = ".dat";
        public const string DIRECTORY_PATH_SAVES = "Saves";
        public const string FILE_NAME_SETTINGS = "GameSettings";

        // Scene related
        public const int SCENE_BUILD_INDEX_INITIALIZATION = 0;
        public const int SCENE_BUILD_INDEX_START_MENU = 1;
        public const int SCENE_BUILD_INDEX_MAIN = 2;

        // Event related
        public const string CUE_BALL_HIT_FORCE_PERCENT_CHANGED = "CUE_BALL_HIT_FORCE_PERCENT_CHANGED";
        public const string SESSION_DATA_TIME_UPDATED = "SESSION_DATA_TIME_UPDATED";
        public const string SESSION_DATA_SHOTS_UPDATED = "SESSION_DATA_SHOTS_UPDATED";
        public const string GUI_REPLAY_BUTTON_CLICKED = "GUI_REPLAY_BUTTON_CLICKED";
        public const string GUI_REPLAY_DISABLED = "GUI_REPLAY_DISABLED";
        public const string GUI_REPLAY_ENABLED = "GUI_REPLAY_ENABLED";
    }
}
