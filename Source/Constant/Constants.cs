using System.Collections.Generic;

namespace SmallerGeothermalGenerator
{
    /// <summary>
    /// 模組提供的地熱發電機的代號
    /// </summary>
    public enum GeneratorType
    {
        Mini,
        Light,
        Medium,
        HighEfficiency
    }

    /// <summary>
    /// 模組提供的地熱發電機相關常數
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// 模組允許調整的電力輸出功率(W)最大值
        /// </summary>
        public const float MaxPowerOutput = 3600f;

        /// <summary>
        /// 模組允許調整的電力輸出功率(W)最小值
        /// </summary>
        public const float MinPowerOutput = 1f;

        /// <summary>
        /// 模組允許調整的資源數量最大值
        /// </summary>
        public const float MaxCost = 100000f;

        /// <summary>
        /// 模組允許調整的資源數量最小值
        /// </summary>
        public const float MinCost = 0f;

        public static readonly Dictionary<GeneratorType, float> DefaultPowerOutputs = new Dictionary<GeneratorType, float>
            {
                { GeneratorType.Mini, 500f },
                { GeneratorType.Light, 1000f },
                { GeneratorType.Medium, 1800f },
                { GeneratorType.HighEfficiency, 2700f }
            };

        public static readonly Dictionary<GeneratorType, float> DefaultCostSteels = new Dictionary<GeneratorType, float>
            {
                { GeneratorType.Mini, 100f },
                { GeneratorType.Light, 120f },
                { GeneratorType.Medium, 200f },
                { GeneratorType.HighEfficiency, 300f }
            };

        public static readonly Dictionary<GeneratorType, float> DefaultCostComponents = new Dictionary<GeneratorType, float>
            {
                { GeneratorType.Mini, 2f },
                { GeneratorType.Light, 3f },
                { GeneratorType.Medium, 5f },
                { GeneratorType.HighEfficiency, 7f }
            };
    }

    /// <summary>
    /// 模組設定畫面的翻譯 key 值
    /// </summary>
    public static class ConstantsView
    {
        /// <summary>
        /// 標題
        /// </summary>
        public const string ModSettingHeaderTitle = "ModSetting.Header.Title";

        /// <summary>
        /// 標頭下方的設定描述
        /// </summary>
        public const string ModSettingHeaderDescription = "ModSetting.Header.Description";

        /// <summary>
        /// 映射 GeneratorType 到各款地熱發電機設定的群組標籤
        /// </summary>
        public static readonly Dictionary<GeneratorType, string> GroupLabels = new Dictionary<GeneratorType, string>
            {
                { GeneratorType.Mini, "MiniGeothermalGenerator.Label" },
                { GeneratorType.Light, "LightGeothermalGenerator.Label" },
                { GeneratorType.Medium, "MediumGeothermalGenerator.Label" },
                { GeneratorType.HighEfficiency, "HighEfficiencyGeothermalGenerator.Label" }
            };

        /// <summary>
        /// 最大電力輸出功率標籤
        /// </summary>
        public const string PowerOutputLabel = "PowerOutput.Label";

        /// <summary>
        /// 建造所需要鋼鐵標籤
        /// </summary>
        public const string CostSteelLabel = "CostSteel.Label";

        /// <summary>
        /// 建造所需要零件標籤
        /// </summary>
        public const string CostComponentLabel = "CostComponent.Label";

        /// <summary>
        /// 預設值標籤
        /// </summary>
        public const string DefaultValueLabel = "DefaultValue.Label";

        /// <summary>
        /// 輸入範圍標籤
        /// </summary>
        public const string AllowedRangeLabel = "AllowedRange.Label";
    }
}