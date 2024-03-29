﻿using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
namespace HostUtilities
{
    public class FixDoubleServing
    {
        public static Harmony HarmonyInstance { get; set; }
        public static void log(string mes) => _MODEntry.LogInfo(mes);
        public static ConfigEntry<bool> isDoubleServingBanned;
        public static void Awake()
        {
            isDoubleServingBanned = _MODEntry.Instance.Config.Bind<bool>("00-功能开关", "禁止卡盘", true, "禁止卡盘子");
            HarmonyInstance = Harmony.CreateAndPatchAll(MethodBase.GetCurrentMethod().DeclaringType);
            _MODEntry.AllHarmony.Add(HarmonyInstance);
            _MODEntry.AllHarmonyName.Add(MethodBase.GetCurrentMethod().DeclaringType.Name);
        }

        private static bool skipNext = false;

        [HarmonyPatch(typeof(ServerPlateStation), "DeliverCurrentPlate")]
        [HarmonyPrefix]
        private static void DeliverCurrentPlate(ref ServerPlateStation __instance, ref ServerPlate ___m_plate, ref IKitchenOrderHandler ___m_orderHandler)
        {
            if (isDoubleServingBanned.Value && ___m_plate.IsReserved())
            {
                skipNext = true;
            }
        }

        [HarmonyPatch(typeof(ServerKitchenFlowControllerBase), nameof(ServerKitchenFlowControllerBase.FoodDelivered))]
        [HarmonyPrefix]
        private static bool FoodDelivered(ref AssembledDefinitionNode _definition, ref PlatingStepData _plateType, ref ServerPlateStation _station)
        {
            if (skipNext)
            {
                skipNext = false;
                log($"拦截到卡盘子!");
                return false;
            }

            return true;
        }
    }
}
