using ColossalFramework;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniqueInstances.Data.Trees
{
    public class UniqueTreesData : MonoBehaviour
    {
        public const ushort MAX_UNIQUE_COUNT = ushort.MaxValue;

        public HashSet<UniqueTree> Buffer { get; private set; }

        private void Awake()
        {
            Buffer = new HashSet<UniqueTree>();
        }
    }
}
