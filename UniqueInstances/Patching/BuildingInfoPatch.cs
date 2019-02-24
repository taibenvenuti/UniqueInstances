using Harmony;

namespace UniqueInstances
{
    [HarmonyPatch(typeof(LoadingManager), "LoadLevelCoroutine")]
    public class LoadingManagerPatch
    {
        static bool Prefix()
        {

            return true;
        }
    }
}
