
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
namespace Test
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            EventRegistry.subscriber.listen(typeof(Main));

        }
        // so when tower is actually place towerPlace switches to true
        static Il2CppSystem.Action<bool> action2 = (Il2CppSystem.Action<bool>)delegate (bool s)
        {
            towerPlaced = s;
        };
        static bool towerPlaced = false;

        public override void OnUpdate()
        {
            base.OnUpdate();

        }
        static void spawnTower(Vector3 v3, TowerModel towerModel)
        {
            towerPlaced = false;
            int attempts = 0;
            while (!towerPlaced && attempts < 100)
            {
                {
                    for (int i = -50; i < 200; i++)
                    {
                        try
                        {
                            float y = v3.y * -2.3f;

                            var towerPos = new UnityEngine.Vector2(v3.x, y);
                            InGame.instance.bridge.CreateTowerAt(towerPos, towerModel, i, true, action2);
                            //System.Console.WriteLine(x + " " + y);
                            break;
                        }
                        catch// (System.Exception e2)
                        {
                            //System.Console.WriteLine(e2 + "");

                        }
                    }
                    attempts++;
                }
            }
        }
        [HarmonyPatch(typeof(Pet), "Initialise")]
        class petinitpatch
        {
            [HarmonyPostfix]
            static void Postfix()
            {
                var v3 = UnityEngine.Input.mousePosition;
                v3 = InGame.instance.sceneCamera.ScreenToWorldPoint(v3);

                var lazerModel = Game.instance.model.GetTower(TowerType.SuperMonkey, 1, 0, 0);
                var foot = lazerModel.footprint;
                foot.doesntBlockTowerPlacement = true;
                foot.ignoresPlacementCheck = true;
                foot.ignoresTowerOverlap = true;
                lazerModel.footprint = foot;
                spawnTower(v3, lazerModel);
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
                    //mousepos stuff
                    var v3 = UnityEngine.Input.mousePosition;
                    v3 = InGame.instance.sceneCamera.ScreenToWorldPoint(v3);
                    List<TowerToSimulation> towers = InGame.instance.bridge.GetAllTowers();
                    foreach (TowerToSimulation tts in towers)
                    {
                        Tower tower = tts.tower;
                        Pet pet = tower.Pet;
                        TowerModel towerModel = tower.towerModel;
                        float x = v3.x;
                        float y = v3.y * -2.3f;
                        if (pet != null)
                        {
                            Assets.Scripts.Simulation.SMath.Vector2 pos = new Assets.Scripts.Simulation.SMath.Vector2(x, y); // z wonky af
                            PetModel model = pet.petModel;
                            model.isFlying = false;
                            pet.petModel = model;
                            pet.SetPosition(pos);
                        }


                        if (towerModel.tiers[0] == 1 && towerModel.tiers[1] == 0 && towerModel.tiers[2] == 0 && towerModel.baseId == TowerType.SuperMonkey)
                        {
                            //tts.tower.Node.graphic.transform.localScale = new UnityEngine.Vector3(0.001f, 0.001f, 0.001f);
                            if (tts != null && tts.tower != null && tts.tower.Node != null && tts.tower.Node.graphic != null && tts.tower.Node.graphic.transform != null)
                            {
                                tts.tower.Node.graphic.transform.localScale = new UnityEngine.Vector3(0.001f, 0.001f, 0.001f);

                            }


                            tower.PositionTower(new Assets.Scripts.Simulation.SMath.Vector2(x, y), false);
                        }
                    }

                }
            }
        }
    }
}
