using ColossalFramework;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniqueInstances.Data.Trees
{
    [Serializable]
    public class UniqueTreeInfo
    {
        public TreeInfo.Variation[] m_variations;

        public UniqueTreeInfo(TreeInfo info)
        {

        }
    }
}
