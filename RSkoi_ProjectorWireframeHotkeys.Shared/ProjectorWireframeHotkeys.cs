using System;
using UnityEngine;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KKAPI.Utilities;

namespace RSkoi_ProjectorWireframeHotkeys
{
    [BepInProcess("CharaStudio.exe")]
    [BepInDependency(KKAPI.KoikatuAPI.GUID, KKAPI.KoikatuAPI.VersionConst)]
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class ProjectorWireframeHotkeys : BaseUnityPlugin
    {
        internal const string PLUGIN_GUID = "RSkoi_ProjectorWireframeHotkeys";
        internal const string PLUGIN_NAME = "RSkoi_ProjectorWireframeHotkeys";
        internal const string PLUGIN_VERSION = "1.0.0";

        internal static ProjectorWireframeHotkeys _instance;
        internal static ManualLogSource _logger;

        private const KeyCode DEFAULT_TOGGLE_MAIN_KEY = KeyCode.P;
        private const KeyCode DEFAULT_TOGGLE_MODIFIER = KeyCode.RightControl;
        internal static ConfigEntry<KeyboardShortcut> ToggleProjectorWireframes { get; private set; }

        internal static ConfigEntry<string> ProjectorShaderName { get; private set; }
        internal static ConfigEntry<string> WireframeShaderName { get; private set; }
        internal static ConfigEntry<string> WireframeShaderColorPropName { get; private set; }

        private Transform _workspace;
        private bool _wireframesHidden = false;

        internal void Awake()
        {
            _instance = this;
            _logger = Logger;

            SetupConfig();
        }

        internal void Start()
        {
            // ¯\_(ツ)_/¯
            _workspace = GameObject.Find("CommonSpace").transform;
        }

        internal void Update()
        {
            if (ToggleProjectorWireframes.Value.IsDown())
                Action();
        }

        private void SetupConfig()
        {
            ToggleProjectorWireframes = Config.Bind(
                "Keyboard Shortcuts",
                "Toggle Wireframes",
                new KeyboardShortcut(DEFAULT_TOGGLE_MAIN_KEY, DEFAULT_TOGGLE_MODIFIER),
                new ConfigDescription("Toggle the visibility of wireframe materials attached to xukmi projectors.",
                null,
                new ConfigurationManagerAttributes { Order = 1 }));

            ProjectorShaderName = Config.Bind(
                "Config",
                "Projector Shader Name",
                "xukmi/Effects/Projector",
                new ConfigDescription("Name of the projector shader to find.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));

            WireframeShaderName = Config.Bind(
                "Config",
                "Wireframe Shader Name",
                "Wireframe-Transparent",
                new ConfigDescription("Name of the wireframe shader to find.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));

            WireframeShaderColorPropName = Config.Bind(
                "Config",
                "Wireframe Shader Color Property Name",
                "_WireColor",
                new ConfigDescription("Name of the color property of the wireframe shader to adjust." +
                " Notice that Material Editor does not display the internal '_' prefix for shader properties.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));
        }

        private void Action()
        {
            if (_workspace == null)
                return;

            bool didAThing = false;
            foreach (var r in _workspace.GetComponentsInChildren<MeshRenderer>())
            {
                var mats = r.materials;

                int projectorIndex = Array.FindIndex(mats, m => m.shader.name.Contains(ProjectorShaderName.Value));
                if (projectorIndex == -1)
                    continue;

                int wireframeIndex = Array.FindIndex(mats, m => m.shader.name.Contains(WireframeShaderName.Value));
                if (wireframeIndex == -1)
                    continue;

                string colorPropName = WireframeShaderColorPropName.Value;
                Color c = mats[wireframeIndex].GetColor(colorPropName);
                float alpha = _wireframesHidden ? 1.0f : 0.0f;
                mats[wireframeIndex].SetColor(colorPropName, new(c.r, c.g, c.b, alpha));

                didAThing = true;
            }

            if (didAThing)
                _wireframesHidden = !_wireframesHidden;
        }
    }
}
