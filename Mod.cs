using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Input;
using Game.Modding;
using Game.SceneFlow;

namespace Cheats
{
    public class Mod : IMod
    {
        public static Mod self { get; private set; }

        internal ILog log { get; private set; }

        private ModSettings m_Setting;

        public static ProxyAction m_AddMoney;
        public static ProxyAction m_SubtractMoney;
        public static ProxyAction m_AddDev;
        public static ProxyAction m_SubtractDev;
        public static ProxyAction m_NextMilestone;
        public static ProxyAction m_AllMilestones;

        public const string kAddMoney = "AddMoney";
        public const string kSubtractMoney = "SubtractMoney";
        public const string kAddDev = "AddDev";
        public const string kSubtractDev = "SubtractDev";
        public const string kNextMilestone = "NextMilestone";
        public const string kAllMilestones = "AllMilestones";

        public const int money_scale = 1000;

        public CheatSystem m_CheatSystem;

        public void OnLoad(UpdateSystem updateSystem)
        {
            Mod.self = this;

            this.log = LogManager.GetLogger("Cheats");
            this.log.Info("Loading");

            m_CheatSystem = new CheatSystem();

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                this.log.Info($"Current mod asset at {asset.path}");

            m_Setting = new ModSettings(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));;

            m_Setting.RegisterKeyBindings();

            m_AddMoney = m_Setting.GetAction(kAddMoney);
            m_SubtractMoney = m_Setting.GetAction(kSubtractMoney);
            m_AddDev = m_Setting.GetAction(kAddDev);
            m_SubtractDev = m_Setting.GetAction(kSubtractDev);
            m_NextMilestone = m_Setting.GetAction(kNextMilestone);
            m_AllMilestones = m_Setting.GetAction(kAllMilestones);

            m_AddMoney.shouldBeEnabled = true;
            m_SubtractMoney.shouldBeEnabled = true;
            m_AddDev.shouldBeEnabled = true;
            m_SubtractDev.shouldBeEnabled = true;
            m_NextMilestone.shouldBeEnabled = true;
            m_AllMilestones.shouldBeEnabled = true;

            m_AddMoney.onInteraction += (_, phase) => m_CheatSystem.GiveMoney(500000, m_Setting.MinToggle);
            m_SubtractMoney.onInteraction += (_, phase) => m_CheatSystem.GiveMoney(-500000, m_Setting.MinToggle);
            m_AddDev.onInteraction += (_, phase) => m_CheatSystem.GiveDevPoints(500000, m_Setting.MinToggle);
            m_SubtractDev.onInteraction += (_, phase) => m_CheatSystem.GiveDevPoints(-500000, m_Setting.MinToggle);
            m_NextMilestone.onInteraction += (_, phase) => m_CheatSystem.NextMilestone();
            m_AllMilestones.onInteraction += (_, phase) => m_CheatSystem.UnlockAllMilestones();

            AssetDatabase.global.LoadSettings(nameof(Cheats), m_Setting, new ModSettings(this));
        }

        public void OnDispose()
        {
            this.log.Info("Disposing");
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
            Mod.self = null;
        }
    }
}
