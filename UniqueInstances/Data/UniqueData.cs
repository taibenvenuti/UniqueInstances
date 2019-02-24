using UnityEngine;
using UniqueInstances.Data.Trees;
using UniqueInstances.Data.Buildings;

namespace UniqueInstances.Data
{
    public class UniqueData : MonoBehaviour
    {
        public UniqueTreesData Trees { get; set; }
        public UniqueBuildingsData Buildings { get; set; }

        private void Awake()
        {
            Trees = gameObject.AddComponent<UniqueTreesData>();
            Buildings = gameObject.AddComponent<UniqueBuildingsData>();
        }
    }
}
