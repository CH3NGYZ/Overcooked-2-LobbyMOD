using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
using Team17.Online;
using UnityEngine;

namespace LobbyMODS
{
    internal class ForceHost
    {
        public static void log(string mes) => MODEntry.LogInfo(mes);
        public static ConfigEntry<string> ValueList;
        private static string[] strList = {
            "游戏默认逻辑",
            "强制主机",
            //"强制客机"
        };
        public static void Awake()
        {
            ValueList = MODEntry.Instance.Config.Bind<string>("00-功能开关", "切换默认主机/客机角色:", strList[0], new ConfigDescription("选择状态", new AcceptableValueList<string>(strList)));

            Harmony.CreateAndPatchAll(typeof(ForceHost));
        }
        public static void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                ClientLobbyFlowController lobbyFlowController = ClientLobbyFlowController.Instance;

                if (lobbyFlowController != null)
                {
                    lobbyFlowController.TryJoinGame();
                    log("尝试以客机身份加入游戏");
                }
                else
                {
                    log("未找到 ClientLobbyFlowController 实例");
                }
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                ClientLobbyFlowController lobbyFlowController = ClientLobbyFlowController.Instance;

                if (lobbyFlowController != null)
                {
                    lobbyFlowController.HostGame();
                    log("强制以主机身份主持游戏");
                }
                else
                {
                    log("未找到 ClientLobbyFlowController 实例");
                }
            }

        }

        //[HarmonyPatch(typeof(ClientLobbyFlowController), "HostGame")]
        //[HarmonyPrefix]
        //private static bool ClientLobbyFlowController_HostGame_Prefix(ClientLobbyFlowController __instance)
        //{
        //    //if (ClientUserSystem.m_Users.Count > 1)
        //    //{
        //    //    return false;
        //    //};
        //    //return true;

        //    //MODEntry.IsHost = true;
        //    //bool flag = ForceHost.ValueList.Value.Equals("强制客机");
        //    //if (flag)
        //    //{
        //    //    log("强制客机已生效, TryJoinGame");
        //    //    Traverse.Create(__instance).Method("TryJoinGame", new object[0]).GetValue();
        //    //    MODEntry.IsHost = false;
        //    //}
        //}

        [HarmonyPatch(typeof(ClientLobbyFlowController), "TryJoinGame")]
        [HarmonyPrefix]
        private static bool ClientLobbyFlowController_TryJoinGame_Prefix(ClientLobbyFlowController __instance)
        {
            //MODEntry.IsHost = false;
            bool flag = ForceHost.ValueList.Value.Equals("强制主机");
            bool result = true;
            if (flag)
            {
                log("强制主机已生效");
                __instance.HostGame();
                result = false;
            }
            return result;
        }

        //private static readonly MethodInfo HostGame = AccessTools.Method(typeof(ClientLobbyFlowController), "HostGame", null, null);
        //// Token: 0x040000D6 RID: 214
        //private static readonly MethodInfo SetState = AccessTools.Method(typeof(ClientLobbyFlowController), "SetState", null, null);
    }
}
