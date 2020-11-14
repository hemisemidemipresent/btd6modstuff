using Assets.Scripts.Simulation.Objects;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Unity.UI_New.InGame;
using Harmony;
using Il2CppSystem.Collections.Generic;
using MelonLoader;

namespace speed
{
    public class Main : MelonMod
    {}
   
    [HarmonyPatch(typeof(InGame), "RoundStart")]

    public class InGameRoundStart_Patch
    {
        [HarmonyPostfix]

        public static void Postfix()
        {
            List<TowerToSimulation> towers = InGame.instance.bridge.GetAllTowers();
            foreach (TowerToSimulation towerToSimulation in towers)
            {
                BehaviorMutator rateBuffModel = new Assets.Scripts.Simulation.Towers.Behaviors.DamageBasedAttackSpeed.RateMutator("69", 10);
                towerToSimulation.tower.AddMutator(rateBuffModel, 300, true, true, false, true, false, false);
            }
        }
    }
}
