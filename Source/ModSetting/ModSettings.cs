using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace SmallerGeothermalGenerator
{
    /// <summary>
    /// 管理模組設定的資料類別。
    /// - `currentModdings` 是用於存儲用戶設定的地熱發電機屬性（如電力輸出和建造所需材料等）。
    /// - 本類負責設定資料的保存、讀取以及初始化。
    /// </summary>
    public class ModSettings : Verse.ModSettings
    {
        // 用於存儲用戶的地熱發電機屬性設置
        public Dictionary<GeneratorType, ModdingGeothermalGenerator> currentModdings;

        /// <summary>
        /// 負責保存和載入模組設定的數據。
        /// - 使用 Scribe 系統對 `currentModdings` 進行序列化處理。
        /// - 初始化時，補全所有 GeneratorType 的預設值。
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();

            // 使用 Scribe 系統保存/載入用戶的地熱發電機設置
            // 因保存的內容為自訂的類別，故使用 Deep 進行序列化
            Scribe_Collections.Look(ref currentModdings, "currentModdings", LookMode.Value, LookMode.Deep);

            // 如果不存在設置，則初始化字典
            if (currentModdings == null)
            {
                currentModdings = new Dictionary<GeneratorType, ModdingGeothermalGenerator>();
            }

            // 確保所有 GeneratorType 都包含設定值
            foreach (GeneratorType type in (GeneratorType[])System.Enum.GetValues(typeof(GeneratorType)))
            {
                if (!currentModdings.ContainsKey(type))
                {
                    currentModdings[type] = new ModdingGeothermalGenerator(type);
                }
            }
        }
    }

    /// <summary>
    /// 定義模組的主要功能類，負責處理設置界面與用戶互動。
    /// - 提供模組名稱和設置標題。
    /// - 繪製設置頁面，包括地熱發電機的多項屬性設置。
    /// </summary>
    public class Mod : Verse.Mod
    {
        // 模組的靜態設置對象，用於存取保存的資料
        public static ModSettings settings;

        // 定義輸入框名稱基底，用於生成唯一的輸入框名稱
        private const string PowerOutputFieldBase = ".NumberField.PowerOutput";
        private const string CostSteelFieldBase = ".NumberField.CostSteel";
        private const string CostComponentFieldBase = ".NumberField.CostComponent";

        // 滾動區域的位置記錄
        private Vector2 scrollPosition;

        /// <summary>
        /// 初始化模組設定，載入用戶配置數據。
        /// </summary>
        public Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ModSettings>();
        }

        /// <summary>
        /// 返回模組的設置分類標題，用於顯示在 RimWorld 的設置菜單中。
        /// </summary>
        public override string SettingsCategory()
        {
            return ConstantsView.ModSettingHeaderTitle.TranslateOrFallback();
        }

        /// <summary>
        /// 繪製設定頁面的描述內容區域。
        /// - 描述模組功能的用途和設置選項，提升用戶理解。
        /// - 使用自定義的樣式格式化文本內容。
        /// </summary>
        private void DrawDescription(Listing_Standard listing)
        {
            GUIStyle messageStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
            };
            messageStyle.normal.textColor = Color.white;
            string allowedPowerOutputRangeText = $"{ConstantsView.AllowedRangeLabel.TranslateOrFallback(Constants.MinPowerOutput, Constants.MaxPowerOutput)}";
            string allowedRequiredResourcesRangeText = $"{ConstantsView.AllowedRangeLabel.TranslateOrFallback(Constants.MinCost, Constants.MaxCost)}";
            string description = ConstantsView.ModSettingHeaderDescription.TranslateOrFallback($" ({allowedPowerOutputRangeText})", $" ({allowedRequiredResourcesRangeText})");
            Rect messageRect = listing.GetRect(Text.LineHeight * 2f);
            GUI.Label(messageRect, description, messageStyle);
            listing.Gap(16f);
        }

        /// <summary>
        /// 負責繪製地熱發電機屬性設置的群組視圖。
        /// 每個群組包含特定 GeneratorType 的多個屬性設置列。
        /// </summary>
        private void DrawGroupSettingView(Listing_Standard listing, GeneratorType type)
        {
            // 繪製群組標題
            DrawGroupLabel(listing, type);

            // 繪製地熱發電機屬性的每一項設置
            DrawPowerOutputSetting(listing, type);
            DrawCostSteelSetting(listing, type);
            DrawCostComponentSetting(listing, type);

            // 增加群組之間的間距
            listing.GapLine(24f);
        }

        /// <summary>
        /// 繪製地熱發電機群組的標題部分。
        /// - 包含樣式、對齊方式以及群組名稱。
        /// </summary>
        private void DrawGroupLabel(Listing_Standard listing, GeneratorType type)
        {
            string groupLabel = ConstantsView.GroupLabels[type];
            GUIStyle groupLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
            };
            groupLabelStyle.normal.textColor = Color.white;
            Rect groupLabelRect = listing.GetRect(Text.LineHeight * 1.2f);
            GUI.Label(groupLabelRect, groupLabel.TranslateOrFallback(), groupLabelStyle);
            listing.Gap(12f);
        }

        /// <summary>
        /// 繪製地熱發電機的電力輸出設定行。
        /// - 包括標籤、輸入框及預設值顯示。
        /// </summary>
        private void DrawPowerOutputSetting(Listing_Standard listing, GeneratorType type)
        {
            float currentValue = settings.currentModdings[type].PowerOutput;
            DrawSetting(listing, type, ConstantsView.PowerOutputLabel, PowerOutputFieldBase, ref currentValue, Constants.DefaultPowerOutputs[type], Constants.MinPowerOutput, Constants.MaxPowerOutput);
            settings.currentModdings[type].PowerOutput = currentValue;
        }

        /// <summary>
        /// 繪製建造所需鋼鐵設定行。
        /// - 包括標籤、輸入框及預設值顯示。
        /// </summary>
        private void DrawCostSteelSetting(Listing_Standard listing, GeneratorType type)
        {
            float currentValue = settings.currentModdings[type].CostSteel;
            DrawSetting(listing, type, ConstantsView.CostSteelLabel, CostSteelFieldBase, ref currentValue, Constants.DefaultCostSteels[type], Constants.MinCost, Constants.MaxCost);
            settings.currentModdings[type].CostSteel = currentValue;
        }

        /// <summary>
        /// 繪製建造所需零件設定行。
        /// - 包括標籤、輸入框及預設值顯示。
        /// </summary>
        private void DrawCostComponentSetting(Listing_Standard listing, GeneratorType type)
        {
            float currentValue = settings.currentModdings[type].CostComponent;
            DrawSetting(listing, type, ConstantsView.CostComponentLabel, CostComponentFieldBase, ref currentValue, Constants.DefaultCostComponents[type], Constants.MinCost, Constants.MaxCost);
            settings.currentModdings[type].CostComponent = currentValue;
        }

        /// <summary>
        /// 通用方法，用於繪製屬性設定行。
        /// - 包括屬性標籤、數值輸入框及預設值和允許範圍的顯示。
        /// </summary>
        private void DrawSetting(Listing_Standard listing, GeneratorType type, string label, string fieldBase, ref float currentValue, float defaultValue, float minValue, float maxValue)
        {
            Rect rowRect = listing.GetRect(Text.LineHeight * 1.5f);
            float totalWidth = rowRect.width;
            float labelWidth = totalWidth * 0.25f;
            float inputWidth = 90f;
            float defaultLabelWidth = totalWidth - labelWidth - inputWidth - 15f;

            Rect labelRect = new Rect(rowRect.x, rowRect.y, labelWidth, rowRect.height);
            Rect inputRect = new Rect(rowRect.x + labelWidth + 5f, rowRect.y, inputWidth, rowRect.height);
            Rect defaultLabelRect = new Rect(rowRect.x + labelWidth + inputWidth + 20f, rowRect.y, defaultLabelWidth, rowRect.height);

            GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft
            };
            GUI.Label(labelRect, label.TranslateOrFallback() + ": ", centeredLabelStyle);

            string controlName = type + fieldBase;
            GUI.SetNextControlName(controlName);

            string buffer = currentValue.ToString("F0");
            Widgets.TextFieldNumeric(inputRect, ref currentValue, ref buffer, minValue, maxValue);

            string defaultLabelText = $"{ConstantsView.DefaultValueLabel.TranslateOrFallback(defaultValue)}";
            GUI.Label(defaultLabelRect, $"{defaultLabelText}", centeredLabelStyle);
            listing.Gap(12f);
        }

        /// <summary>
        /// 繪製模組設定窗口的內容。
        /// - 包括描述信息的顯示以及地熱發電機屬性設置的交互界面。
        /// - 支持滾動功能以便於顯示多組屬性配置。
        /// </summary>
        /// <param name="inRect">
        /// 窗口的外部矩形範圍，用於限制繪製區域。
        /// </param>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            // 初始化標準界面列表，用於繪製設定項目。
            Listing_Standard listing = new Listing_Standard();

            // 定義滾動區域的尺寸，增加視圖高度以允許滾動。
            Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, inRect.height + 400f);

            // 開始定義可滾動的界面區域。
            Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);

            listing.Begin(viewRect);

            // 繪製描述區域，提供模組設定的簡要信息。
            DrawDescription(listing);

            // 遍歷所有地熱發電機的類型，並為每種類型繪製獨立的設置群組。
            foreach (GeneratorType type in (GeneratorType[])System.Enum.GetValues(typeof(GeneratorType)))
            {
                // 繪製單個群組的設置界面，包括標題和多個屬性配置列。
                DrawGroupSettingView(listing, type);
            }

            // 結束標準列表的繪製。
            listing.End();

            // 結束滾動界面的定義。
            Widgets.EndScrollView();

            // 如果滑鼠點擊事件發生，清除焦點以防止意外輸入。
            if (Event.current.type == EventType.MouseDown)
            {
                // 獲取當前焦點的元件名稱。
                string name = GUI.GetNameOfFocusedControl();

                // 如果焦點元件為空或不是焦點不在輸入框之一，則清除焦點。
                if (name == null ||
                    !name.Contains(PowerOutputFieldBase) ||
                    !name.Contains(CostSteelFieldBase) ||
                    !name.Contains(CostComponentFieldBase))
                {
                    GUI.FocusControl(null);
                }
            }
        }
    }
}
