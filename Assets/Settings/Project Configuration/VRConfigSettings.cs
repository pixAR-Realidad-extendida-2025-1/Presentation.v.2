using System;
using UnityEngine;

namespace VRPresentationTrainer.Core.Data
{
    [CreateAssetMenu(fileName = "VRConfigSettings", menuName = "VR Presentation Trainer/Config Settings")]
    public class VRConfigSettings : ScriptableObject
    {
        [Serializable]
        public class VRCoreSettings
        {
            public int targetFrameRate = 90;
            public float renderScale = 1.0f;
            public bool enableHandTracking = true;
            public string trackingMode = "controller_and_hand";
            
            [Serializable]
            public class InteractionSettings
            {
                public float grabThreshold = 0.15f;
                public float hapticStrength = 0.5f;
                public float touchSensitivity = 0.8f;
            }
            
            public InteractionSettings interactionSettings = new InteractionSettings();
        }
        
        [Serializable]
        public class PresentationSettings
        {
            public float slideTransitionTime = 0.5f;
            public float pointerSize = 0.02f;
            public Color highlightColor = new Color(0.26f, 0.52f, 0.96f); // #4285F4
            public string defaultFont = "Roboto-Regular";
        }
        
        [Serializable]
        public class AudienceSettings
        {
            public int maxMembers = 20;
            public float behaviorUpdateRate = 0.5f;
            public float reactionThreshold = 0.7f;
        }
        
        [Serializable]
        public class AnalyticsSettings
        {
            public int sampleRate = 100;
            public int cacheDuration = 3600;
            public bool enableDetailedLogging = true;
            public string[] metricsToTrack = new string[] {
                "eyeContact", "speechPace", "gestures", "positioning"
            };
        }

        [Serializable]
        public class EnvironmentSettings
        {
            public NoiseLevel noiseLevel = NoiseLevel.Ninguno;

            public enum NoiseLevel
            {
                Ninguno,
                Bajo,
                Medio,
                Alto,
            }
        }

        public VRCoreSettings vrCore = new VRCoreSettings();
        public PresentationSettings presentation = new PresentationSettings();
        public AudienceSettings audience = new AudienceSettings();
        public AnalyticsSettings analytics = new AnalyticsSettings();
        public EnvironmentSettings environment = new EnvironmentSettings();

        // Para guardar la configuración como JSON
        public string ToJson()
        {
            return JsonUtility.ToJson(this, true);
        }
        
        // Para cargar desde JSON
        public static VRConfigSettings FromJson(string json)
        {
            VRConfigSettings settings = CreateInstance<VRConfigSettings>();
            JsonUtility.FromJsonOverwrite(json, settings);
            return settings;
        }
    }
}
