using System;
using System.Collections.Generic;
using UniqueInstances.Extensions;
using UnityEngine;

namespace UniqueInstances.Data.Buildings
{
    public class UniqueBuildingsData : MonoBehaviour
    {
        public const ushort MAX_UNIQUE_COUNT = ushort.MaxValue;

        public HashSet<UniqueBuilding> Buffer { get; private set; }

        private void Awake()
        {
            Buffer = new HashSet<UniqueBuilding>();
        }
    }
}
