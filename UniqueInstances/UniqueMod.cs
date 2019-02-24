using ICities;
using UnityEngine;
using static UnityEngine.Object;
using UniqueInstances.Controller;
using UniqueInstances.Serialization;

namespace UniqueInstances
{
    public class UniqueMod : LoadingExtensionBase, IUserMod
    {
        public string Name => "Unique Instances";
        public string Description => "Allows customization of buildings and trees on a per-instance basis.";
        private const string UniqueName = "UniqueInstancesMod";
        private GameObject GameObject { get; set; }
        private bool Initialized;
        private static Xml _settings;
        public static Xml Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Xml.Load();
                    if (_settings == null)
                    {
                        _settings = new Xml();
                        _settings.Save();
                    }
                }
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        private void Load()
        {
            if (Initialized || !LoadingManager.instance.m_loadingComplete) return;
            if (GameObject != null) Unload();
            GameObject = new GameObject(UniqueName);
            UniqueController.Instance = GameObject.AddComponent<UniqueController>();
            Debug.LogWarning("UniqueLoading");
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

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
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
