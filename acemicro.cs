
using Assets.Scripts.Models.Pets;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Simulation.Pets;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Unity.UI_New.InGame;
using Harmony;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using NKHook6.Api.Events;
using NKHook6.Api.Extensions;
using UnhollowerBaseLib;
using UnityEngine;
using Assets.Scripts.Simulation.Display;
using NKHook6.Api.Events._MainMenu;

namespace Test
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            EventRegistry.subscriber.listen(typeof(Main));

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        static List<TargetType> targetsList = new List<TargetType>();

        [EventAttribute("MainMenuLoadedEvent")]
        public static void onMainMenuLoaded(ref MainMenuEvents.LoadedEvent e)
        {
            var towerModel = Game.instance.model.GetTower(TowerType.MonkeyAce);
            var ttypes = towerModel.targetTypes;
            for (int i = 0; i < ttypes.Length; i++)
            {
                var id = ttypes[i].id;
                if (id != "Wingmonkey" && id != "Centered")
                {
                    targetsList.Add(ttypes[i]);
                }
            }
        }

        [HarmonyPatch(typeof(InGame), "Update")]
        public class Update_Patch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                bool inAGame = InGame.instance != null && InGame.instance.bridge != null;

                if (inAGame)
                {
                    foreach (TowerToSimulation towerToSimulation in InGame.instance.bridge.GetAllTowers())
                    {
                        Tower tower = towerToSimulation.tower;
                        TowerModel towerModel = tower.towerModel;
                        if (towerModel.name.Contains("MonkeyAce"))
                        {
                            Il2CppReferenceArray<TargetType> targets = new Il2CppReferenceArray<TargetType>(targetsList.ToArray());
                            towerModel.targetTypes = targets;
                            tower.towerModel = towerModel;
                        }
                    }
                }
            }
        }
    }
}
