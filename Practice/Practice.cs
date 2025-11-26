using Exiled.API.Enums;
using Exiled.API.Features;
using Practice.Helpers;
using System;

namespace Practice
{
    public class Practice : Plugin<Config>
    {
        public override string Name => "SCP:SL Practice";
        public override string Author => "Vretu";
        public override string Prefix { get; } = "Practice";
        public override Version Version => new Version(1, 1, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 10, 0);
        public override PluginPriority Priority { get; } = PluginPriority.High;
        public static Practice Instance { get; private set; }

        public override void OnEnabled()
        {
            Instance = this;

            EventHandlers.RegisterEvents();
            Overlay.EnableOverlay();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;

            EventHandlers.UnregisterEvents();
            Overlay.DisableOverlay();

            base.OnDisabled();
        }
    }
}