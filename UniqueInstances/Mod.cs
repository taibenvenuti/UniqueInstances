using ICities;
using UnityEngine;
using static UnityEngine.Object;

namespace UniqueInstances
{
    public class Mod : LoadingExtensionBase, IUserMod
    {
        public string Name => "Unique";
        public string Description => "Allows customization of prefabs on a per-instance basis.";
        private const string UniqueName = "UniqueMod";
        private GameObject GameObject { get; set; }
        private bool Initialized;

        private void Load()
        {
            if (Initialized) return;
            if (GameObject != null) Unload();
            GameObject = new GameObject(UniqueName);
            GameObject.AddComponent<Manager>();
            Initialized = true;
        }

        private void Unload()
        {
            if (GameObject != null)
            {
                Destroy(GameObject);
                GameObject = null;
            }
            Initialized = false;
        }

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            if (loading.currentMode == AppMode.Game)
                Load();
        }

        public override void OnReleased()
        {
            Unload();
            base.OnReleased();
        }

        public void OnEnabled()
        {
            Load();
        }

        public void OnDisabled()
        {
            Unload();
        }
    }
}
