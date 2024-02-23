using BepInEx.Configuration;
using System.Collections.Generic;
using UnityEngine;
 
namespace LobbyMODS
{
    public static class DisplayKickedUserUI
    {
        private static MyOnScreenDebugDisplayKickedUser onScreenDebugDisplayKickedUser;
        private static MyKickedUserCounter kickedUserCounter = null;
        public static ConfigEntry<bool> ShowKickedUserEnabled;
        public static ConfigEntry<int> defaultFontSize;
        public static ConfigEntry<string> defaultFontColor;


        public static void add_m_Text(string str) => kickedUserCounter?.add_m_Text(str);

        public static void Awake()
        {
            ShowKickedUserEnabled = MODEntry.Instance.Config.Bind<bool>("00-功能开关", "显示自动踢的黑名单玩家", true);
            defaultFontSize = MODEntry.Instance.Config.Bind<int>("00-UI字体", "踢黑名单字体", 20);
            defaultFontColor = MODEntry.Instance.Config.Bind<string>("00-UI字体", "踢黑名单字体颜色(#+6位字母数字组合)", "#000000");
            onScreenDebugDisplayKickedUser = new MyOnScreenDebugDisplayKickedUser();
            onScreenDebugDisplayKickedUser.Awake();
        }

        public static void Update()
        {
            onScreenDebugDisplayKickedUser.Update();

            if (((!MODEntry.IsInLobby) || Input.GetKeyDown(KeyCode.End)) && kickedUserCounter != null)
                RemoveKickedUserCounter();
            else if (((MODEntry.IsInLobby && ShowKickedUserEnabled.Value) || Input.GetKeyDown(KeyCode.Home)) && kickedUserCounter == null)
                AddKickedUserCounter();
        }

        public static void OnGUI() => onScreenDebugDisplayKickedUser.OnGUI();

        private static void AddKickedUserCounter()
        {
            kickedUserCounter = new MyKickedUserCounter();
            onScreenDebugDisplayKickedUser.AddDisplay(kickedUserCounter);
            kickedUserCounter.init_m_Text();
        }

        private static void RemoveKickedUserCounter()
        {
            onScreenDebugDisplayKickedUser.RemoveDisplay(kickedUserCounter);
            kickedUserCounter.OnDestroy();
            kickedUserCounter = null;
        }

        public class MyKickedUserCounter : DebugDisplay
        {
            private bool replaced = false;

            public void init_m_Text() => m_Text = "暂时没有自动移除的账号";

            public void add_m_Text(string str)
            {
                if (replaced) m_Text += "\n" + str;
                else { m_Text = str; replaced = true; }
            }

            public override void OnSetUp() { }

            public override void OnUpdate() { }

            public override void OnDraw(ref Rect rect, GUIStyle style) => base.DrawText(ref rect, style, m_Text);

            private static string m_Text = string.Empty;
        }

        private class MyOnScreenDebugDisplayKickedUser
        {
            private readonly List<DebugDisplay> m_Displays = new List<DebugDisplay>();
            private readonly GUIStyle m_GUIStyle = new GUIStyle();

            public void AddDisplay(DebugDisplay display)
            {
                if (display != null)
                {
                    display.OnSetUp();
                    m_Displays.Add(display);
                }
            }

            public void RemoveDisplay(DebugDisplay display) => m_Displays.Remove(display);

            public void Awake()
            {
                m_GUIStyle.alignment = TextAnchor.UpperLeft;
                m_GUIStyle.fontSize = defaultFontSize.Value;
                try
                {
                    this.m_GUIStyle.normal.textColor = HexToColor(defaultFontColor.Value);
                }
                catch
                {
                    this.m_GUIStyle.normal.textColor = HexToColor("#000000");
                }
            }

            public void Update()
            {
                for (int i = 0; i < m_Displays.Count; i++)
                    m_Displays[i].OnUpdate();
            }

            public void OnGUI()
            {
                Rect rect = new Rect(0f, 0f, Screen.width, m_GUIStyle.fontSize);
                for (int i = 0; i < m_Displays.Count; i++)
                    m_Displays[i].OnDraw(ref rect, m_GUIStyle);
            }
        }
        private static Color HexToColor(string hex)
        {
            Color color = new Color();
            ColorUtility.TryParseHtmlString(hex, out color);
            return color;
        }
    }
}
