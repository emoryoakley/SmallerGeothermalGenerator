using RimWorld.Planet;
using SmallerGeothermalGenerator.Patch;
using System.Linq;
using System.Reflection;
using Verse;

namespace SmallerGeothermalGenerator
{
    /// <summary>
    /// 自定義的世界組件，用於在 RimWorld 啟動完成後或在遊戲讀取完成後進行補丁修改。
    /// </summary>
    [StaticConstructorOnStartup]
    public class StartUpWorld : WorldComponent
    {
        /// <summary>
        /// 靜態建構函式，在 RimWorld 啟動完成後執行。
        /// </summary>
        static StartUpWorld()
        {
            // 獲取當前執行的dll組件，印出組件建置編號以及釋出版本號
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Log.Message($"[SmallerGeothermalGenerator] Current assembly build version is {executingAssembly.GetName().Version}.");

            // 使用反射獲取 AssemblyInformationalVersion
            var informationalVersionAttribute = executingAssembly
                .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)
                .OfType<AssemblyInformationalVersionAttribute>()
                .FirstOrDefault();

            string productVersion = informationalVersionAttribute?.InformationalVersion ?? "";

            if (!string.IsNullOrEmpty(productVersion))
            {
                Log.Message($"[SmallerGeothermalGenerator] Current assembly product version is {productVersion}.");
            }

            // 執行補丁邏輯
            ModPatch.Patch();
        }

        /// <summary>
        /// 一般建構函式，在創建 WorldComponent 實例時執行。
        /// </summary>
        /// <param name="world">當前的世界實例。</param>
        public StartUpWorld(World world) : base(world)
        {
        }

        /// <summary>
        /// 在遊戲讀取完成後執行補丁修改。
        /// </summary>
        public override void FinalizeInit()
        {
            base.FinalizeInit();
            // 執行補丁修改
            ModPatch.Patch();
        }
    }
}
