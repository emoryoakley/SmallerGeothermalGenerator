using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using Verse;

namespace SmallerGeothermalGenerator.Patch
{
    public class ModPatch
    {
        /// <summary>
        /// 補丁修改函式
        /// </summary>
        public static void Patch()
        {
            // 獲取模組設定
            ModSettings settings = Mod.settings;

            // 遍歷所有地熱發電機類型，進行補丁修改
            foreach (GeneratorType type in (GeneratorType[])System.Enum.GetValues(typeof(GeneratorType)))
            {
                string defName = type + "GeothermalGenerator";
                // 獲取地熱發電機的 ThingDef
                ThingDef targetDef = DefDatabase<ThingDef>.GetNamed(defName, false);
                // 補丁修改建造成本列表
                PatchCostList(targetDef, settings.currentModdings[type]);
                // 補丁修改電力消耗(最大電力輸出功率)
                PatchPowerConsumption(targetDef, settings.currentModdings[type]);
            }
        }

        /// <summary>
        /// 補丁修改建造成本列表。
        /// </summary>
        /// <param name="targetDef">目標 ThingDef。</param>
        /// <param name="setting">地熱發電機的模組設定。</param>
        private static void PatchCostList(ThingDef targetDef, ModdingGeothermalGenerator setting)
        {
            targetDef.costList = new List<ThingDefCountClass>();
            AddCostList(ref targetDef.costList, "Steel", (int)setting.CostSteel);
            AddCostList(ref targetDef.costList, "ComponentIndustrial", (int)setting.CostComponent);
        }

        /// <summary>
        /// 向建造成本列表中添加項目。
        /// </summary>
        /// <param name="costList">建造成本列表。</param>
        /// <param name="name">項目名稱。</param>
        /// <param name="cost">項目成本。</param>
        private static void AddCostList(ref List<ThingDefCountClass> costList, string name, int cost)
        {
            if (cost > 0)
            {
                costList.Add(new ThingDefCountClass(ThingDef.Named(name), cost));
            }
        }

        /// <summary>
        /// 補丁修改電力消耗。
        /// </summary>
        /// <param name="targetDef">目標 ThingDef。</param>
        /// <param name="modding">地熱發電機的模組設定。</param>
        private static void PatchPowerConsumption(ThingDef targetDef, ModdingGeothermalGenerator modding)
        {
            // 找到 CompProperties_Power 並更新基礎功率
            CompProperties_Power power = targetDef.comps.Find(c => c is CompProperties_Power) as CompProperties_Power;
            if (power != null)
            {
                var field = typeof(CompProperties_Power).GetField("basePowerConsumption", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    // 設置電力輸出功率為負值，表示輸出功率
                    field.SetValue(power, -1 * ((int)modding.PowerOutput));
                }
            }
        }
    }
}
