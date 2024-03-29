﻿using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using Team17.Online;
using UnityEngine;


namespace HostUtilities
{
    public class PartyKevin
    {
        public static Harmony HarmonyInstance { get; set; }
        public static void log(string mes) => _MODEntry.LogInfo(mes);
        public static ConfigEntry<KeyCode> resetTimer;
        public static ConfigEntry<KeyCode> PlayRandom;
        public static ConfigEntry<bool> kevinEnabled;
        public static bool onlyKevin;
        public static bool notKevin;
        public static bool onlyCarnival3_4;
        public static bool onlyBeach3_4;


        public static void Awake()
        {
            MServerLobbyFlowController.CreateConfigEntries();


            PlayRandom = _MODEntry.Instance.Config.Bind<KeyCode>("01-按键绑定", "08-大厅计时器归零", KeyCode.Alpha6, "4秒后直接开始随机关卡");
            resetTimer = _MODEntry.Instance.Config.Bind<KeyCode>("01-按键绑定", "09-大厅计时器45秒", KeyCode.Alpha7, "重置街机大厅时间为45秒");
            kevinEnabled = _MODEntry.Instance.Config.Bind<bool>("02-修改关卡", "02区域总开关(以下四个选项互斥)", true);
            HarmonyInstance = Harmony.CreateAndPatchAll(MethodBase.GetCurrentMethod().DeclaringType);
            _MODEntry.AllHarmony.Add(HarmonyInstance);
            _MODEntry.AllHarmonyName.Add(MethodBase.GetCurrentMethod().DeclaringType.Name);
        }
        public static void Update()
        {
            //街机凯文
            //开始随机关卡
            if (Input.GetKeyDown(PlayRandom.Value))
            {
                //LobbyManager lobbyManager = new LobbyManager();
                //ServerLobbyFlowController instance = ServerLobbyFlowController.Instance;
                //if (instance != null)
                //{
                //    int num = 0;
                //    while ((long)num < (long)(ulong)OnlineMultiplayerConfig.MaxPlayers)
                //    {
                //        instance.SelectTheme(SceneDirectoryData.LevelTheme.Random, num);
                //        LobbyFlowController.LobbyState? serverLobbyState = lobbyManager.ServerLobbyState;
                //        LobbyFlowController.LobbyState lobbyState = LobbyFlowController.LobbyState.LocalThemeSelection;
                //        if (serverLobbyState.GetValueOrDefault() == lobbyState & serverLobbyState != null)
                //        {
                //            lobbyManager.SetServerLobbyState(LobbyFlowController.LobbyState.LocalThemeSelected);
                //        }
                //        else
                //        {
                //            serverLobbyState = lobbyManager.ServerLobbyState;
                //            lobbyState = LobbyFlowController.LobbyState.OnlineThemeSelection;
                //            if (serverLobbyState.GetValueOrDefault() == lobbyState & serverLobbyState != null)
                //            {
                //                lobbyManager.SetServerLobbyState(LobbyFlowController.LobbyState.OnlineThemeSelected);
                //            }
                //        }
                //        num++;
                //    }
                //}
                MServerLobbyFlowController.ResetServerLobbyTimer(0f);
            }
            //重置街机大厅时间
            else if (Input.GetKeyDown(resetTimer.Value))
            {
                log("重置街机大厅时间");
                MServerLobbyFlowController.ResetServerLobbyTimer(45f);
            }


            bool onlyKevin_ = MServerLobbyFlowController.sceneDisableConfigEntries["只玩凯文和小节关"].Value;
            bool notKevin_ = MServerLobbyFlowController.sceneDisableConfigEntries["不玩凯文和小节关"].Value;
            bool onlyCarnival3_4_ = MServerLobbyFlowController.sceneDisableConfigEntries["只玩麻3-4"].Value;
            bool onlyBeach3_4_ = MServerLobbyFlowController.sceneDisableConfigEntries["只玩海3-4"].Value;

            //凯文,麻,海 选项4选1
            //任意两个为true
            if ((onlyKevin_ && notKevin_) || (onlyKevin_ && onlyCarnival3_4_) || (onlyKevin_ && onlyBeach3_4_) || (notKevin_ && onlyCarnival3_4_) || (notKevin_ && onlyBeach3_4_) || (onlyCarnival3_4_ && onlyBeach3_4_))
            {
                //谁和之前不一样
                if (MServerLobbyFlowController.sceneDisableConfigEntries["只玩麻3-4"].Value != onlyCarnival3_4)
                {
                    //不一样的为true, 其他的为false
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩麻3-4"].Value = true;
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩海3-4"].Value = false;
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩凯文和小节关"].Value = false;
                    MServerLobbyFlowController.sceneDisableConfigEntries["不玩凯文和小节关"].Value = false;
                }
                else if (MServerLobbyFlowController.sceneDisableConfigEntries["只玩海3-4"].Value != onlyBeach3_4)
                {
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩海3-4"].Value = true;
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩麻3-4"].Value = false;
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩凯文和小节关"].Value = false;
                    MServerLobbyFlowController.sceneDisableConfigEntries["不玩凯文和小节关"].Value = false;

                }
                else if (MServerLobbyFlowController.sceneDisableConfigEntries["只玩凯文和小节关"].Value != onlyKevin)
                {
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩凯文和小节关"].Value = true;
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩麻3-4"].Value = false;
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩海3-4"].Value = false;
                    MServerLobbyFlowController.sceneDisableConfigEntries["不玩凯文和小节关"].Value = false;
                }
                else if (MServerLobbyFlowController.sceneDisableConfigEntries["不玩凯文和小节关"].Value != notKevin)
                {
                    MServerLobbyFlowController.sceneDisableConfigEntries["不玩凯文和小节关"].Value = true;
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩麻3-4"].Value = false;
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩海3-4"].Value = false;
                    MServerLobbyFlowController.sceneDisableConfigEntries["只玩凯文和小节关"].Value = false;
                }
            }
            //保存现在的状态供下次使用
            onlyKevin = MServerLobbyFlowController.sceneDisableConfigEntries["只玩凯文和小节关"].Value;
            notKevin = MServerLobbyFlowController.sceneDisableConfigEntries["不玩凯文和小节关"].Value;
            onlyCarnival3_4 = MServerLobbyFlowController.sceneDisableConfigEntries["只玩麻3-4"].Value;
            onlyBeach3_4 = MServerLobbyFlowController.sceneDisableConfigEntries["只玩海3-4"].Value;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ClientTime), "OnTimeSyncReceived")]
        public static void ClientTime_OnTimeSyncReceived_Patch()
        {
            if (MServerLobbyFlowController.sceneDisableConfigEntries["不玩凯文和小节关"].Value == true)
            {
                //不玩凯文时以下所有选项自动关闭
                MServerLobbyFlowController.sceneDisableConfigEntries["01-关闭小节关"].Value = false;
                MServerLobbyFlowController.sceneDisableConfigEntries["02-关闭主线凯文"].Value = false;
                MServerLobbyFlowController.sceneDisableConfigEntries["03-关闭海滩凯文"].Value = false;
                MServerLobbyFlowController.sceneDisableConfigEntries["04-关闭完美露营地凯文"].Value = false;
                MServerLobbyFlowController.sceneDisableConfigEntries["05-关闭恐怖地宫凯文"].Value = false;
                MServerLobbyFlowController.sceneDisableConfigEntries["06-关闭翻滚帐篷凯文"].Value = false;
                MServerLobbyFlowController.sceneDisableConfigEntries["07-关闭咸咸马戏团凯文"].Value = false;
            }
        }

        //选关patch
        [HarmonyPatch(typeof(ServerLobbyFlowController), "PickLevel")]
        [HarmonyPrefix]
        private static bool ServerLobbyFlowController_PickLevel_Prefix(ref ServerLobbyFlowController __instance, SceneDirectoryData.LevelTheme _theme)
        {
            if (!kevinEnabled.Value)
            {
                log("街机凯文未启用,执行原函数");
                return true;
            }
            MServerLobbyFlowController.MPickLevel(__instance, _theme);
            return false;
        }




        private static readonly AccessTools.FieldRef<ServerLobbyFlowController, bool> m_bIsCoop_server = AccessTools.FieldRefAccess<ServerLobbyFlowController, bool>("m_bIsCoop");

    }

    public class MServerLobbyFlowController
    {
        public static Dictionary<string, ConfigEntry<bool>> sceneDisableConfigEntries = new Dictionary<string, ConfigEntry<bool>>();
        public static Dictionary<string, bool> alreadyPlayedSet = new Dictionary<string, bool>();
        public static void CreateConfigEntries()
        {
            CreateConfigEntry("02-修改关卡", "只玩麻3-4", false);
            CreateConfigEntry("02-修改关卡", "只玩海3-4", false);
            CreateConfigEntry("02-修改关卡", "不玩凯文和小节关", true, "此选项打开时,02-禁用主题(凯文)下的所有开关,会被自动关闭.");
            CreateConfigEntry("02-修改关卡", "只玩凯文和小节关", false, "此选项打开时,02-禁用主题(非凯文)下的所有选项会失效,因为不玩普通关卡了.");
            CreateConfigEntry("02-禁用主题(凯文)", "01-关闭小节关");
            CreateConfigEntry("02-禁用主题(凯文)", "02-关闭主线凯文");
            CreateConfigEntry("02-禁用主题(凯文)", "03-关闭海滩凯文");
            CreateConfigEntry("02-禁用主题(凯文)", "04-关闭完美露营地凯文");
            CreateConfigEntry("02-禁用主题(凯文)", "05-关闭恐怖地宫凯文");
            CreateConfigEntry("02-禁用主题(凯文)", "06-关闭翻滚帐篷凯文");
            CreateConfigEntry("02-禁用主题(凯文)", "07-关闭咸咸马戏团凯文");
            CreateConfigEntry("02-禁用主题(非凯文)", "01-关闭世界1");
            CreateConfigEntry("02-禁用主题(非凯文)", "02-关闭世界2");
            CreateConfigEntry("02-禁用主题(非凯文)", "03-关闭世界3");
            CreateConfigEntry("02-禁用主题(非凯文)", "04-关闭世界4");
            CreateConfigEntry("02-禁用主题(非凯文)", "05-关闭世界5");
            CreateConfigEntry("02-禁用主题(非凯文)", "06-关闭世界6");
            CreateConfigEntry("02-禁用主题(非凯文)", "07-关闭节庆大餐");
            CreateConfigEntry("02-禁用主题(非凯文)", "08-关闭王朝餐厅");
            CreateConfigEntry("02-禁用主题(非凯文)", "09-关闭桃子游行", true);
            CreateConfigEntry("02-禁用主题(非凯文)", "10-关闭幸运灯笼", true);
            CreateConfigEntry("02-禁用主题(非凯文)", "11-关闭海滩");
            CreateConfigEntry("02-禁用主题(非凯文)", "12-关闭烧烤度假村");
            CreateConfigEntry("02-禁用主题(非凯文)", "13-关闭完美露营地");
            CreateConfigEntry("02-禁用主题(非凯文)", "14-关闭美味树屋");
            CreateConfigEntry("02-禁用主题(非凯文)", "15-关闭恐怖地宫");
            CreateConfigEntry("02-禁用主题(非凯文)", "16-关闭惊悚庭院");
            CreateConfigEntry("02-禁用主题(非凯文)", "17-关闭凶残城垛", true);
            CreateConfigEntry("02-禁用主题(非凯文)", "18-关闭翻滚帐篷");
            CreateConfigEntry("02-禁用主题(非凯文)", "19-关闭咸咸马戏团");

            CreateConfigEntry("00-功能开关", "街机关卡不重复", true);
        }
        private static ConfigEntry<bool> configEntry;
        private static void CreateConfigEntry(string cls, string key)
        {
            configEntry = _MODEntry.Instance.Config.Bind(cls, key, false);
            sceneDisableConfigEntries.Add(key, configEntry);
        }
        private static void CreateConfigEntry(string cls, string key, bool init)
        {
            configEntry = _MODEntry.Instance.Config.Bind(cls, key, init);
            sceneDisableConfigEntries.Add(key, configEntry);
        }
        private static void CreateConfigEntry(string cls, string key, bool init, string desc)
        {
            configEntry = _MODEntry.Instance.Config.Bind(cls, key, init, desc);
            sceneDisableConfigEntries.Add(key, configEntry);
        }


        public static void ResetServerLobbyTimer(float time = 45f)
        {
            ServerLobbyFlowController.Instance.ResetTimer(time);
        }

        public void PickRandom()
        {
            ServerLobbyFlowController.Instance.PickLevel(SceneDirectoryData.LevelTheme.Random);
        }

        public static void MPickLevel(ServerLobbyFlowController __instance, SceneDirectoryData.LevelTheme _theme)
        {
            _MODEntry.LogInfo($"街机凯文已启用, 选择的世界是{_theme}");
            //Traverse Tvinstance = Traverse.Create(__instance);
            Predicate<SceneDirectoryData.SceneDirectoryEntry> matchOnlyCarnival3_4 = (SceneDirectoryData.SceneDirectoryEntry entry) =>
            {
                return (entry.Label.Contains("DLC08Level12"));
            };
            Predicate<SceneDirectoryData.SceneDirectoryEntry> matchOnlyBeach3_4 = (SceneDirectoryData.SceneDirectoryEntry entry) =>
            {
                return (entry.Label.Contains("DLC02Level12"));
            };
            Predicate<SceneDirectoryData.SceneDirectoryEntry> matchScene = (SceneDirectoryData.SceneDirectoryEntry entry) =>
            {
                if (entry.Label.Contains("ThroneRoom") || entry.Label.Contains("TutorialLevel"))
                {
                    return false;
                }

                if (entry.Theme == _theme || _theme == SceneDirectoryData.LevelTheme.Random)
                {
                    return true;
                }
                return false;
            };
            Predicate<SceneDirectoryData.SceneDirectoryEntry> matchAvailableInLobby = (SceneDirectoryData.SceneDirectoryEntry entry) =>
            {
                return entry.AvailableInLobby;
            };
            Predicate<SceneDirectoryData.SceneDirectoryEntry> matchOnlyKevin = (SceneDirectoryData.SceneDirectoryEntry entry) =>
            {
                return !entry.AvailableInLobby;
            };
            Predicate<SceneDirectoryData.SceneDirectoryEntry> match = (SceneDirectoryData.SceneDirectoryEntry entry) =>
            {

                if (entry.Label.Contains("ThroneRoom") || entry.Label.Contains("TutorialLevel"))
                {
                    return false;
                }

                bool condition1 = !sceneDisableConfigEntries["01-关闭世界1"].Value || !(entry.World == SceneDirectoryData.World.One && entry.AvailableInLobby);
                bool condition2 = !sceneDisableConfigEntries["02-关闭世界2"].Value || !(entry.World == SceneDirectoryData.World.Two && entry.AvailableInLobby);
                bool condition3 = !sceneDisableConfigEntries["03-关闭世界3"].Value || !(entry.World == SceneDirectoryData.World.Three && entry.AvailableInLobby);
                bool condition4 = !sceneDisableConfigEntries["04-关闭世界4"].Value || !(entry.World == SceneDirectoryData.World.Four && entry.AvailableInLobby);
                bool condition5 = !sceneDisableConfigEntries["05-关闭世界5"].Value || !(entry.World == SceneDirectoryData.World.Five && entry.AvailableInLobby);
                bool condition6 = !sceneDisableConfigEntries["06-关闭世界6"].Value || !(entry.World == SceneDirectoryData.World.Six && entry.AvailableInLobby);
                bool condition7 = !sceneDisableConfigEntries["07-关闭节庆大餐"].Value || !(entry.World == SceneDirectoryData.World.DLC3_One);
                bool condition8 = !sceneDisableConfigEntries["07-关闭节庆大餐"].Value || !(entry.World == SceneDirectoryData.World.DLC9_One);
                bool condition9 = !sceneDisableConfigEntries["08-关闭王朝餐厅"].Value || !(entry.World == SceneDirectoryData.World.DLC4_One);
                bool condition10 = !sceneDisableConfigEntries["08-关闭王朝餐厅"].Value || !(entry.World == SceneDirectoryData.World.DLC10_One);
                bool condition11 = !sceneDisableConfigEntries["09-关闭桃子游行"].Value || !(entry.World == SceneDirectoryData.World.DLC11_One);
                bool condition12 = !sceneDisableConfigEntries["10-关闭幸运灯笼"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.ChinaTown3);
                bool condition13 = !sceneDisableConfigEntries["11-关闭海滩"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Beach);
                bool condition14 = !sceneDisableConfigEntries["12-关闭烧烤度假村"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Resort);
                bool condition15 = !sceneDisableConfigEntries["13-关闭完美露营地"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Campsite && entry.AvailableInLobby);
                bool condition16 = !sceneDisableConfigEntries["14-关闭美味树屋"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Treehouse);
                bool condition17 = !sceneDisableConfigEntries["15-关闭恐怖地宫"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Keep && entry.AvailableInLobby);
                bool condition18 = !sceneDisableConfigEntries["16-关闭惊悚庭院"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Courtyard);
                bool condition19 = !sceneDisableConfigEntries["17-关闭凶残城垛"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Battlements);
                bool condition20 = !sceneDisableConfigEntries["18-关闭翻滚帐篷"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Inside && entry.AvailableInLobby);
                bool condition21 = !sceneDisableConfigEntries["19-关闭咸咸马戏团"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Outside && entry.AvailableInLobby);
                bool condition22 = !sceneDisableConfigEntries["01-关闭小节关"].Value || !(entry.World == SceneDirectoryData.World.One || entry.World == SceneDirectoryData.World.Two || entry.World == SceneDirectoryData.World.Three || entry.World == SceneDirectoryData.World.Four || entry.World == SceneDirectoryData.World.Five || entry.World == SceneDirectoryData.World.Six);
                bool condition23 = !sceneDisableConfigEntries["02-关闭主线凯文"].Value || !(entry.World == SceneDirectoryData.World.Seven);
                bool condition24 = !sceneDisableConfigEntries["03-关闭海滩凯文"].Value || !entry.Label.Contains("DLC02HiddenLevel");
                bool condition25 = !sceneDisableConfigEntries["04-关闭完美露营地凯文"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Campsite && !entry.AvailableInLobby);
                bool condition26 = !sceneDisableConfigEntries["05-关闭恐怖地宫凯文"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Keep && !entry.AvailableInLobby);
                bool condition27 = !sceneDisableConfigEntries["06-关闭翻滚帐篷凯文"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Inside && !entry.AvailableInLobby);
                bool condition28 = !sceneDisableConfigEntries["07-关闭咸咸马戏团凯文"].Value || !(entry.Theme == SceneDirectoryData.LevelTheme.Outside && !entry.AvailableInLobby);
                bool shouldIncludeEntry = condition1 && condition2 && condition3 && condition4 && condition5 &&
                      condition6 && condition7 && condition8 && condition9 && condition10 && condition11 && condition12 &&
                      condition13 && condition14 && condition15 && condition16 && condition17 && condition18 && condition19 && condition20 &&
                      condition21 && condition22 && condition23 && condition24 && condition25 && condition26 && condition27 && condition28;

                return shouldIncludeEntry;
            };
            FastList<SceneDirectoryData.SceneDirectoryEntry> fastList = new FastList<SceneDirectoryData.SceneDirectoryEntry>(60);
            //LobbyFlowController m_lobbyFlow = Tvinstance.Field("m_lobbyFlow").GetValue<LobbyFlowController>();
            LobbyFlowController m_lobbyFlow = ServerLobbyFlowController.Instance.m_lobbyFlow;
            SceneDirectoryData[] sceneDirectories = m_lobbyFlow.GetSceneDirectories();

            DLCManager dlcmanager = GameUtils.RequireManager<DLCManager>();
            List<DLCFrontendData> allDlc = dlcmanager.AllDlc;
            bool m_bIsCoop = ServerLobbyFlowController.Instance.m_bIsCoop;
            GameSession.GameType gameType = (!m_bIsCoop) ? GameSession.GameType.Competitive : GameSession.GameType.Cooperative;
            int[] array = new int[sceneDirectories.Length];
            for (int i = 0; i < sceneDirectories.Length; i++)
            {
                DLCFrontendData dlcfrontendData = null;
                int dlcidfromSceneDirIndex = m_lobbyFlow.GetDLCIDFromSceneDirIndex(gameType, i);
                if (_theme == SceneDirectoryData.LevelTheme.Random)
                {
                    for (int j = 0; j < allDlc.Count; j++)
                    {
                        DLCFrontendData dlcfrontendData2 = allDlc[j];
                        if (dlcfrontendData2.m_DLCID == dlcidfromSceneDirIndex)
                        {
                            dlcfrontendData = dlcfrontendData2;
                            break;
                        }
                    }
                }
                if (dlcfrontendData == null || dlcmanager.IsDLCAvailable(dlcfrontendData))
                {
                    fastList.AddRange(sceneDirectories[i].Scenes.FindAll(match));
                    fastList = new FastList<SceneDirectoryData.SceneDirectoryEntry>(fastList.FindAll(matchScene).ToArray());
                    if (sceneDisableConfigEntries["不玩凯文和小节关"].Value)
                    {
                        fastList = new FastList<SceneDirectoryData.SceneDirectoryEntry>(fastList.FindAll(matchAvailableInLobby).ToArray());
                    }
                    else if (sceneDisableConfigEntries["只玩凯文和小节关"].Value)
                    {
                        fastList = new FastList<SceneDirectoryData.SceneDirectoryEntry>(fastList.FindAll(matchOnlyKevin).ToArray());
                    }
                }

                array[i] = fastList.Count;
            }

            //FastList<SceneDirectoryData.SceneDirectoryEntry> fastList_filter = new FastList<SceneDirectoryData.SceneDirectoryEntry>(fastList.FindAll(matchScene).ToArray());
            //for (int kkk = 0; kkk < fastList_filter.Count; kkk++)
            //{
            //    string Message = $"filter: index: {kkk}   Theme: {fastList_filter._items[kkk].Theme}   World: {fastList_filter._items[kkk].World}  label:{fastList_filter._items[kkk].Label}  AvailableInLobby:{fastList_filter._items[kkk].AvailableInLobby}  IsHidden:{fastList_filter._items[kkk].IsHidden}";
            //    if (fastList._items[kkk].AvailableInLobby == false)
            //    {
            //        MODEntryPlugin.LogWarning(Message);
            //    }
            //    else
            //    {
            //        MODEntryPlugin.LogInfo(Message);
            //    }
            //}




            //for (int kkk = 0; kkk < fastList.Count; kkk++)
            //{
            //    string Message = $"index: {kkk}   Theme: {fastList._items[kkk].Theme}   World: {fastList._items[kkk].World}  label:{fastList._items[kkk].Label}  AvailableInLobby:{fastList._items[kkk].AvailableInLobby}  IsHidden:{fastList._items[kkk].IsHidden}";
            //    if (fastList._items[kkk].AvailableInLobby == false)
            //    {
            //        MODEntryPlugin.LogWarning(Message);
            //    }
            //    else
            //    {
            //        MODEntryPlugin.LogInfo(Message);
            //    }
            //}

            if (fastList.Count == 0)
            {
                _MODEntry.LogInfo($"未匹配到{_theme}里的关卡,随机全部关卡(带凯文)");
                for (int i = 0; i < sceneDirectories.Length; i++)
                {
                    //Predicate<SceneDirectoryData.SceneDirectoryEntry> Random = (SceneDirectoryData.SceneDirectoryEntry entry) => !entry.Label.Contains("TutorialLevel") && !entry.Label.Contains("ThroneRoom");
                    fastList.AddRange(sceneDirectories[i].Scenes.FindAll(matchAvailableInLobby));
                    //fastList = new FastList<SceneDirectoryData.SceneDirectoryEntry>(fastList.FindAll(matchAvailableInLobby).ToArray());
                    array[i] = fastList.Count;
                }

            }
            if (sceneDisableConfigEntries["只玩麻3-4"].Value)
            {
                fastList.Clear();
                _MODEntry.LogInfo("只玩麻3-4");
                for (int i = 0; i < sceneDirectories.Length; i++)
                {
                    fastList.AddRange(sceneDirectories[i].Scenes.FindAll(matchOnlyCarnival3_4));
                    array[i] = fastList.Count;
                }
            }
            else if (sceneDisableConfigEntries["只玩海3-4"].Value)
            {
                fastList.Clear();
                _MODEntry.LogInfo("只玩海3-4");
                for (int i = 0; i < sceneDirectories.Length; i++)
                {
                    fastList.AddRange(sceneDirectories[i].Scenes.FindAll(matchOnlyBeach3_4));
                    array[i] = fastList.Count;
                }
            }

            int num = UnityEngine.Random.Range(0, fastList.Count);
            for (int kkk = 0; kkk < fastList.Count; kkk++)
            {
                string Message = $"Filter: index: {kkk}   Theme: {fastList._items[kkk].Theme}   World: {fastList._items[kkk].World}  label:{fastList._items[kkk].Label}  AvailableInLobby:{fastList._items[kkk].AvailableInLobby}  IsHidden:{fastList._items[kkk].IsHidden}";
                if (fastList._items[kkk].AvailableInLobby == false)
                {
                    _MODEntry.LogWarning(Message);
                }
                else
                {
                    _MODEntry.LogInfo(Message);
                }
            }
            if (sceneDisableConfigEntries["街机关卡不重复"].Value && !(sceneDisableConfigEntries["只玩麻3-4"].Value || sceneDisableConfigEntries["只玩海3-4"].Value))
            {
                int count = 0;
                for (int k = 0; k < fastList.Count; k++)
                {
                    if (!alreadyPlayedSet.ContainsKey(fastList._items[k].Label))
                        alreadyPlayedSet.Add(fastList._items[k].Label, false);
                    if (alreadyPlayedSet[fastList._items[k].Label])
                        count++;
                }
                if (count == fastList.Count)
                    for (int k = 0; k < fastList.Count; k++)
                        alreadyPlayedSet[fastList._items[k].Label] = false;
                if (count >= 128)
                {
                    for (int k = 0; k < fastList.Count; k++)
                    {
                        if (!alreadyPlayedSet[fastList._items[k].Label])
                        {
                            num = k;
                            break;
                        }
                    }
                }
                else
                {
                    while (alreadyPlayedSet[fastList._items[num].Label])
                    {
                        _MODEntry.LogInfo($"alreadyPlayed:{fastList._items[num].Label}");
                        num = UnityEngine.Random.Range(0, fastList.Count);
                    }
                }
            }
            alreadyPlayedSet[fastList._items[num].Label] = true;
            _MODEntry.LogInfo($"Picked: index: {num}   Theme: {fastList._items[num].Theme}   World: {fastList._items[num].World}  label:{fastList._items[num].Label}  AvailableInLobby:{fastList._items[num].AvailableInLobby}  IsHidden:{fastList._items[num].IsHidden}");

            int idx = -1;
            for (int k = 0; k < array.Length; k++)
            {
                if (num < array[k])
                {
                    idx = k;
                    break;
                }
            }

            SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = fastList._items[num];
            int dlcidfromSceneDirIndex2 = m_lobbyFlow.GetDLCIDFromSceneDirIndex(gameType, idx);
            SceneDirectoryData.PerPlayerCountDirectoryEntry sceneVarient = sceneDirectoryEntry.GetSceneVarient(ServerUserSystem.m_Users.Count);
            if (sceneVarient == null)
            {
                if (!m_bIsCoop)
                {
                    T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
                    dialog.Initialize("Text.Versus.NotEnoughPlayers.Title", "Text.Versus.NotEnoughPlayers.Message", "Text.Button.Confirm", null, null, T17DialogBox.Symbols.Warning, true, true, false);
                    T17DialogBox t17DialogBox = dialog;
                    t17DialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox.OnConfirm, new T17DialogBox.DialogEvent(delegate ()
                    {
                        ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, delegate (IConnectionModeSwitchStatus _status)
                        {
                            if (_status.GetProgress() == eConnectionModeSwitchProgress.Complete)
                            {
                                ServerGameSetup.Mode = GameMode.OnlineKitchen;
                                ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
                            }
                        });
                    }));
                    dialog.Show();
                }
                return;
            }
            ServerLobbyFlowController.Instance.m_delayedLevelLoad = ServerLobbyFlowController.Instance.StartCoroutine(ServerLobbyFlowController.Instance.DelayedLevelLoad(sceneVarient.SceneName, dlcidfromSceneDirIndex2));
        }
    }

    public class LobbyManager
    {
        private static readonly AccessTools.FieldRef<ServerLobbyFlowController, LobbyFlowController.LobbyState> m_state_server = AccessTools.FieldRefAccess<ServerLobbyFlowController, LobbyFlowController.LobbyState>("m_state");
        private static readonly FastInvokeHandler SetStateServer = MethodInvoker.GetHandler(AccessTools.Method(typeof(ServerLobbyFlowController), "SetState", null, null));
        public unsafe LobbyFlowController.LobbyState? ServerLobbyState
        {
            get
            {
                ServerLobbyFlowController instance = ServerLobbyFlowController.Instance;
                if (instance == null)
                {
                    return null;
                }
                return new LobbyFlowController.LobbyState?(LobbyManager.m_state_server.Invoke(instance));
            }
        }

        public void SetServerLobbyState(LobbyFlowController.LobbyState state)
        {
            ServerLobbyFlowController instance = ServerLobbyFlowController.Instance;
            if (instance == null)
            {
                return;
            }
            LobbyManager.SetStateServer.Invoke(instance, new object[]
            {
                state
            });
        }
    }
}

