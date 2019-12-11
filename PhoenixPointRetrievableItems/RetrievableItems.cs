using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using System.Reflection;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Equipments;
namespace PhoenixPointRetrievableItems
{
    public class RetrievableItems
    {
        public static void Init()
        {
            var harmony = HarmonyInstance.Create("io.github.realitymachina.retrievableitems");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

        }

        [HarmonyPatch(typeof(DieAbility), "ShouldDestroyItem")]
        public static class PhoenixPoint_DieAbility_ShouldDestroyItem_Patch
        {
            static bool Prefix(DieAbility  __instance, ref bool __result, ref TacticalItem item)
            {
                if (__instance.TacticalActor.IsControlledByPlayer)
                {
                    return true; // game will let it live...anyway
                }

                __result = false; // never destroy items via RNG;

                return false; // returning false here lets us skip the original method used here
            }
        }
        [HarmonyPatch(typeof(DieAbility), "DropItems")]
        public static class PhoenixPoint_DieAbility_DropItems_Patch
        {

            static bool Prefix(ref DieAbility __instance)
            {
                if (__instance.DieAbilityDef.DestroyItems)
                {
                    __instance.DieAbilityDef.DestroyItems = false;
                }

                return true; // doing this to catch instances where an alien's death may auto destroy items
            }
        }

    }
}
