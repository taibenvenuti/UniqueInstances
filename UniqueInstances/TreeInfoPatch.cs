using ColossalFramework;
using Harmony;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniqueInstances
{
    [HarmonyPatch(typeof(TreeInfo), "InitializePrefab")]
    public class TreeInfoPatch
    {
        static void Postfix(TreeInfo __instance)
        {

        }
    }
}
