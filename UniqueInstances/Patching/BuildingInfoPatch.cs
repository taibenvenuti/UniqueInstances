using Harmony;

namespace UniqueInstances
{
    [HarmonyPatch(typeof(LoadingManager), "LoadLevelComplete")]
    public class BuildingInfoPatch
    {
        static bool Prefix()
        {

        }
    }
}
