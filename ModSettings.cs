using System.Collections.Generic;
using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Input;
using Game.Modding;
using Game.Prefabs;
using Game.Settings;
using Game.UI;
using Unity.Entities;

namespace Cheats
{
    [FileLocation(nameof(Cheats))]
    [SettingsUIGroupOrder(kSection, kExtra, kKeybinds)]
    [SettingsUIShowGroupName(kMilestonesGroup, kMoneyGroup, kDevGroup, kUnlockerGroup, kOptionGroup)]
    [SettingsUIKeyboardAction(name: Mod.kAddMoney, type: ActionType.Button, rebindOptions:RebindOptions.All, modifierOptions: ModifierOptions.Allow, usages: new string[] { Usages.kMenuUsage, "CheatUsage" }, interactions: new string[] { "UIButton" })]
    [SettingsUIKeyboardAction(name: Mod.kSubtractMoney, type: ActionType.Button, rebindOptions: RebindOptions.All, modifierOptions: ModifierOptions.Allow, usages: new string[] { Usages.kMenuUsage, "CheatUsage" }, interactions: new string[] { "UIButton" })]
    [SettingsUIKeyboardAction(name: Mod.kAddDev, type: ActionType.Button, rebindOptions: RebindOptions.All, modifierOptions: ModifierOptions.Allow, usages: new string[] { Usages.kMenuUsage, "CheatUsage" }, interactions: new string[] { "UIButton" })]
    [SettingsUIKeyboardAction(name: Mod.kSubtractDev, type: ActionType.Button, rebindOptions: RebindOptions.All, modifierOptions: ModifierOptions.Allow, usages: new string[] { Usages.kMenuUsage, "CheatUsage" }, interactions: new string[] { "UIButton" })]
    [SettingsUIKeyboardAction(name: Mod.kNextMilestone, type: ActionType.Button, rebindOptions: RebindOptions.All, modifierOptions: ModifierOptions.Allow, usages: new string[] { Usages.kMenuUsage, "CheatUsage" }, interactions: new string[] { "UIButton" })]
    [SettingsUIKeyboardAction(name: Mod.kAllMilestones, type: ActionType.Button, rebindOptions: RebindOptions.All, modifierOptions: ModifierOptions.Allow, usages: new string[] { Usages.kMenuUsage, "CheatUsage" }, interactions: new string[] { "UIButton" })]
    public class ModSettings : ModSetting
    {

        private const int money_scale = Mod.money_scale;

        public const string kSection = "Main";
        public const string kExtra = "Extra";
        public const string kKeybinds = "Keybinds";

        public const string kMilestonesGroup = "Milestone Unlocker";
        public const string kMoneyGroup = "Money";
        public const string kDevGroup = "Development Points";
        public const string kUnlockerGroup = "Prefab Unlocker";
        public const string kOptionGroup = "Options";

        private CheatSystem m_CheatSystem = new CheatSystem();
        public ModSettings(IMod mod) : base(mod)
        {

        }

        [SettingsUISection(kSection, kMilestonesGroup)]
        [SettingsUIButtonGroup("Milestone Buttons")]
        public bool NextMilestoneButton
        {
            set
            {
                m_CheatSystem.NextMilestone();
            }
        }

        [SettingsUISection(kSection, kMilestonesGroup)]
        [SettingsUIButtonGroup("Milestone Buttons")]
        public bool UnlockAllMilestonesButton
        {
            set
            {
                m_CheatSystem.UnlockAllMilestones();
            }
        }

        [SettingsUISlider(min = 1, max = 1000000, step = 1, scalarMultiplier = money_scale, unit = Unit.kMoney)]
        [SettingsUISection(kSection, kMoneyGroup)]
        public int MoneySlider { get; set; } = 500;

        [SettingsUISection(kSection, kMoneyGroup)]
        [SettingsUIButtonGroup("Money Buttons")]
        public bool AddMoneyButton
        {
            set
            {
                m_CheatSystem.GiveMoney(MoneySlider * money_scale, MinToggle);
            }
        }

        [SettingsUISection(kSection, kMoneyGroup)]
        [SettingsUIButtonGroup("Money Buttons")]
        public bool SubtractMoneyButton
        {
            set
            {
                m_CheatSystem.GiveMoney(-MoneySlider * money_scale, MinToggle);
            }
        }

        [SettingsUISection(kSection, kMoneyGroup)]
        [SettingsUIButtonGroup("Money Buttons")]
        public bool SetMoneyButton
        {
            set
            {
                m_CheatSystem.SetMoney(MoneySlider * money_scale);
            }
        }

        [SettingsUISlider(min = 0, max = 20, step = 1, scalarMultiplier = 1, unit = Unit.kInteger)]
        [SettingsUISection(kSection, kDevGroup)]
        public int DevSlider { get; set; } = 1;

        [SettingsUISection(kSection, kDevGroup)]
        [SettingsUIButtonGroup("Development Point Buttons")]
        public bool AddDevButton
        {
            set
            {
                m_CheatSystem.GiveDevPoints(DevSlider, MinToggle);
            }
        }

        [SettingsUISection(kSection, kDevGroup)]
        [SettingsUIButtonGroup("Development Point Buttons")]
        public bool SubtractDevButton
        {
            set
            {
                m_CheatSystem.GiveDevPoints(-DevSlider, MinToggle);
            }
        }

        [SettingsUISection(kSection, kDevGroup)]
        [SettingsUIButtonGroup("Development Point Buttons")]
        public bool SetDevButton
        {
            set
            {
                m_CheatSystem.SetDevPoints(DevSlider);
            }
        }

        [SettingsUISection(kSection, kUnlockerGroup)]
        public MilestoneEnum UnlockMilestonePrefabsDropdown { get; set; } = MilestoneEnum.M1;

        [SettingsUISection(kSection, kUnlockerGroup)]
        public bool UnlockMilestonePrefabsButton
        {
            set
            {
                int milestone = (int)UnlockMilestonePrefabsDropdown;
                m_CheatSystem.UnlockMilestonePrefabs(milestone+1);
            }
        }



        [SettingsUITextInput]
        [SettingsUISection(kExtra, kUnlockerGroup)]
        public string PrefabTypeInput { get; set; } = string.Empty;

        [SettingsUITextInput]
        [SettingsUISection(kExtra, kUnlockerGroup)]
        public string PrefabNameInput { get; set; } = string.Empty;

        [SettingsUISection(kExtra, kUnlockerGroup)]
        public bool UnlockPrefabButton
        {
            set
            {
                PrefabID prefabID = new PrefabID(PrefabTypeInput, PrefabNameInput);
                m_CheatSystem.UnlockPrefabID(prefabID);
            }
        }



        [SettingsUISection(kSection, kOptionGroup)]
        public bool MinToggle { get; set; } = false;


        [SettingsUIKeyboardBinding(BindingKeyboard.None, Mod.kNextMilestone, shift: false)]
        [SettingsUISection(kKeybinds, kMilestonesGroup)]
        public ProxyBinding NextMilestoneBinding { get; set; }

        [SettingsUIKeyboardBinding(BindingKeyboard.None, Mod.kAllMilestones, shift: false)]
        [SettingsUISection(kKeybinds, kMilestonesGroup)]
        public ProxyBinding AllMilestonesBinding { get; set; }

        [SettingsUIKeyboardBinding(BindingKeyboard.None, Mod.kAddMoney, shift: false)]
        [SettingsUISection(kKeybinds, kMoneyGroup)]
        public ProxyBinding AddMoneyBinding { get; set; }

        [SettingsUIKeyboardBinding(BindingKeyboard.None, Mod.kSubtractMoney, shift: false)]
        [SettingsUISection(kKeybinds, kMoneyGroup)]
        public ProxyBinding SubtractMoneyBinding { get; set; }

        [SettingsUIKeyboardBinding(BindingKeyboard.None, Mod.kAddDev, shift: false)]
        [SettingsUISection(kKeybinds, kDevGroup)]
        public ProxyBinding AddDevBinding { get; set; }

        [SettingsUIKeyboardBinding(BindingKeyboard.None, Mod.kSubtractDev, shift: false)]
        [SettingsUISection(kKeybinds, kDevGroup)]
        public ProxyBinding SubtractDevBinding { get; set; }


        [SettingsUISection(kKeybinds, kOptionGroup)]
        public bool ResetBindings
        {
            set
            {
                ResetKeyBindings();
            }
        }

        public override void SetDefaults()
        {
            throw new System.NotImplementedException();
        }

        public enum MilestoneEnum
        {
            M1, M2, M3, M4, M5, M6, M7 ,M8, M9, M10, M11, M12, M13, M14, M15, M16, M17, M18, M19, M20
        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly ModSettings m_Setting;
        public LocaleEN(ModSettings setting)
        {
            m_Setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Cheats" },
                { m_Setting.GetOptionTabLocaleID(ModSettings.kSection), "Main" },
                { m_Setting.GetOptionTabLocaleID(ModSettings.kExtra), "Extra" },
                { m_Setting.GetOptionTabLocaleID(ModSettings.kKeybinds), "Keybinds" },

                { m_Setting.GetOptionGroupLocaleID(ModSettings.kMilestonesGroup), "Milestones" },
                { m_Setting.GetOptionGroupLocaleID(ModSettings.kMoneyGroup), "Money" },
                { m_Setting.GetOptionGroupLocaleID(ModSettings.kDevGroup), "Development Points" },
                { m_Setting.GetOptionGroupLocaleID(ModSettings.kUnlockerGroup), "Prefab Unlocker" },
                { m_Setting.GetOptionGroupLocaleID(ModSettings.kOptionGroup), "Options" },


                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.NextMilestoneButton)), "Unlock Next Milestone" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.NextMilestoneButton)), "Progress to the next milestone." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.UnlockAllMilestonesButton)), "Unlock all Milestones" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.UnlockAllMilestonesButton)), "Progress to Megalopolis." },


                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.AddMoneyButton)), "Add Money" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.AddMoneyButton)), "Add the specified amount of money." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.SubtractMoneyButton)), "Subtract Money" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.SubtractMoneyButton)), "Subtract the specified amount of money." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.SetMoneyButton)), "Set Money" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.SetMoneyButton)), "Set the specified amount of money." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.MoneySlider)), "Money" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.MoneySlider)), "The amount of money." },


                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.DevSlider)), "Development Points" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.DevSlider)), "The amount of development points." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.AddDevButton)), "Add Points" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.AddDevButton)), "Add the specified amount of points." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.SubtractDevButton)), "Subtract Points" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.SubtractDevButton)), "Subtract the specified amount of points." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.SetDevButton)), "Set Points" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.SetDevButton)), "Set the specified amount of points." },



                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.UnlockMilestonePrefabsButton)), "Unlock Milestone Prefabs" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.UnlockMilestonePrefabsButton)), "Unlock the specified milestone's prefabs without unlocking the milestone itself. Prefabs from previous locked milestones will still be locked." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.UnlockMilestonePrefabsDropdown)), "Milestone" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.UnlockMilestonePrefabsDropdown)), "The specified milestone." },


                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.MinToggle)), "Minimum of 0" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.MinToggle)), "If this option is enabled, then you will not be able to set any value below 0 and vice versa." },



                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.PrefabTypeInput)), "Prefab Type" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.PrefabTypeInput)), "The type of the prefab, BuildingPrefab for example." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.PrefabNameInput)), "Prefab Name" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.PrefabNameInput)), "The name of the prefab, BusDepot01 for example." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.UnlockPrefabButton)), "Unlock Prefab" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.UnlockPrefabButton)), "Unlock the specified prefab." },




                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.AddMoneyBinding)), "Add 500,000$" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.AddMoneyBinding)), "" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.SubtractMoneyBinding)), "Subtract 500,000$" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.SubtractMoneyBinding)), "" },


                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.AddDevBinding)), "Add 1 Dev Point" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.AddDevBinding)), "" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.SubtractDevBinding)), "Subtract 1 Dev Point" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.SubtractDevBinding)), "" },


                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.NextMilestoneBinding)), "Unlock Next Milestone" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.NextMilestoneBinding)), "" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.AllMilestonesBinding)), "Unlock All Milestones" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.AllMilestonesBinding)), "" },


                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ResetBindings)), "Reset key bindings" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ResetBindings)), $"Reset all key bindings." },




                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M1), "1 - Tiny Village" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M2), "2 - Small Village" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M3), "3 - Large Village" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M4), "4 - Grand Village" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M5), "5 - Tiny Town" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M6), "6 - Boom Town" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M7), "7 - Busy Town" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M8), "8 - Big Town" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M9), "9 - Great Town" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M10), "10 - Small City" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M11), "11 - Big City" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M12), "12 - Large City" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M13), "13 - Huge City" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M14), "14 - Grand City" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M15), "15 - Metropolis" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M16), "16 - Thriving Metropolis" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M17), "17 - Flourishing Metropolis" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M18), "18 - Expansive Metropolis" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M19), "19 - Massive Metropolis" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.MilestoneEnum.M20), "20 - Megalopolis" },



                { m_Setting.GetBindingMapLocaleID(), "Mod Settings" },
            };
        }

        public void Unload()
        {

        }
    }
}
