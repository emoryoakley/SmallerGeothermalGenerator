using Verse;

namespace SmallerGeothermalGenerator
{
    /// <summary>
    /// 表示地熱發電機的模組設定。
    /// </summary>
    public class ModdingGeothermalGenerator : IExposable
    {
        // 代號
        private readonly GeneratorType type;

        /// <summary>
        /// 獲取地熱發電機的類型。
        /// </summary>
        public GeneratorType Type => type;

        // 電力輸出功率(W)
        private float powerOutput;

        /// <summary>
        /// 獲取或設置地熱發電機的電力輸出功率(W)。
        /// </summary>
        public float PowerOutput
        {
            get => powerOutput;
            set => powerOutput = Clamp(value, Constants.MinPowerOutput, Constants.MaxPowerOutput);
        }

        // 建造所需鋼鐵數量
        private float costSteel;

        /// <summary>
        /// 獲取或設置建造地熱發電機所需的鋼鐵數量。
        /// </summary>
        public float CostSteel
        {
            get => costSteel;
            set => costSteel = Clamp(value, Constants.MinCost, Constants.MaxCost);
        }

        // 建造所需零件數量
        private float costComponent;

        /// <summary>
        /// 獲取或設置建造地熱發電機所需的零件數量。
        /// </summary>
        public float CostComponent
        {
            get => costComponent;
            set => costComponent = Clamp(value, Constants.MinCost, Constants.MaxCost);
        }

        /// <summary>
        /// 初始化 <see cref="ModdingGeothermalGenerator"/> 類別的新實例。
        /// </summary>
        public ModdingGeothermalGenerator()
        {
        }

        /// <summary>
        /// 使用指定的地熱發電機類型初始化 <see cref="ModdingGeothermalGenerator"/> 類別的新實例。
        /// </summary>
        /// <param name="type">地熱發電機的類型。</param>
        public ModdingGeothermalGenerator(GeneratorType type)
        {
            this.type = type;
            this.PowerOutput = Constants.DefaultPowerOutputs[type];
            this.CostSteel = Constants.DefaultCostSteels[type];
            this.CostComponent = Constants.DefaultCostComponents[type];
        }

        /// <summary>
        /// 將數據暴露給保存/加載系統。
        /// 這個方法是因為實作 IExposable 介面而需要實作的。
        /// 使用 Scribe_Values.Look 方法進行資料序列化，並使用 Deep 模式存取資料。
        /// </summary>
        public void ExposeData()
        {
            Scribe_Values.Look(ref powerOutput, "powerOutput");
            Scribe_Values.Look(ref costSteel, "costSteel");
            Scribe_Values.Look(ref costComponent, "costComponent");
        }

        /// <summary>
        /// 將值限制在指定的範圍內。
        /// </summary>
        /// <param name="value">要限制的值。</param>
        /// <param name="min">最小值。</param>
        /// <param name="max">最大值。</param>
        /// <returns>限制在指定範圍內的值。</returns>
        private static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
