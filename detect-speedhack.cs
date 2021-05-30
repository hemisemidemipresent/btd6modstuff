
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Bridge;
using MelonLoader;
using UnityEngine;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Models.Towers;
using System.Collections.Generic;
using Assets.Scripts.Models.Towers.Behaviors;
using System.Drawing;
using System;
using Assets.Scripts.Unity.Analytics;
using Harmony;
using Assets.Scripts.Unity.UI_New.Popups;

namespace slons
{
    enum placment
    {

    };
    public class Main : MelonMod
    {
        public override void OnApplicationStart() { base.OnApplicationStart(); }
        private static double delta;
        private const double EPSILON = 5; // max max max max max max normal deviations
        static Il2CppSystem.Action<string> action = (Il2CppSystem.Action<string>)delegate (string s)
        {// do literally nothing lmao
        };

        [HarmonyPatch(typeof(InGame), "Update")]
        public class Update_Patch
        {
            [HarmonyPostfix]
            public static void Postfix() // happens the moment any game starts
            {
                // sets the delta at the start of the game
                // the delta will gradually widen if they speedhack
                AnalyticsManager analyticsManager = new AnalyticsManager();
                delta = analyticsManager.GetServerDelta(Il2CppSystem.DateTime.Now);
            }
        }
        public override void OnUpdate()
        {

            base.OnUpdate();
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                try
                {
                    // this is the checking function. It is up to you how frequently you want to run it
                    AnalyticsManager analyticsManager = new AnalyticsManager();
                    double newdelta = analyticsManager.GetServerDelta(Il2CppSystem.DateTime.Now);
                    if (Math.Abs(newdelta - delta) > EPSILON)
                    {

                        PopupScreen.instance.ShowSetNamePopup("vrej", "stop using cheat engine/speedhacking", action, "idk how to make a popup because im bad at making mods lmao");

                    }
                    else
                    {
                        log($"{delta.ToString()} | {newdelta.ToString()}");
                    }
                }
                catch (Exception e)
                {
                    PopupScreen.instance.ShowInternetPopup(1);
                }


            }
        }

        public void log(string s)
        {
            MelonLogger.Msg(s);
        }

    }
}