using PatchQuest;
using MelonLoader;
using HarmonyLib;

namespace Difficulty20
{
    public class ModMain : MelonMod
    {
        public MelonPreferences_Category Preferences;

        public MelonPreferences_Entry<float> Preferences_Difficulty;

        public static float Difficulty;

        /// <summary>
        /// Getting the difficulty value the user has specified
        /// </summary>
        public override void OnInitializeMelon()
        {
            // Creating preferences category
            Preferences = MelonPreferences.CreateCategory("Difficulty20");

            // Creating preferences entry
            Preferences_Difficulty = Preferences.CreateEntry<float>("difficulty", 20f, "Difficulty", 
                "Sets the difficulty to any value. Keep in mind that it will not work if the game's difficulty level is set to 0.");

            // Setting the Difficulty variable to the value found in the config.
            Difficulty = Preferences_Difficulty.Value;

            // Saving the file, so that any changes made (e.g. setting up the category and entry) is saved.
            Preferences.SaveToFile();
        }
    }


    
    /// <summary>
    /// Patches the method that sets the game's difficulty level.
    /// Gets a reference of the hardModeLevel float from the actual method and then sets it to whatever value the user wants
    /// </summary>
    [HarmonyPatch(typeof(Scaling))]
    [HarmonyPatch(nameof(Scaling.ResetAsHard))]
    [HarmonyPatch(MethodType.Normal)]
    public class DifficultyPatch
    {
        public static void Prefix(ref float hardModeLevel)
        {
            hardModeLevel = ModMain.Difficulty;
            MelonLogger.Msg("Setting game difficulty to level " + hardModeLevel);
        }
    }
}
