using UnityEditor;
using UnityEngine;
#if UNITY_XR_MANAGEMENT
using UnityEditor.XR.Management;
using UnityEngine.XR.Management;
#endif

namespace VRTemplate.Setup.Editor
{
    /// <summary>
    /// Editor utility para configurar el proyecto VR de presentaciones desde el menú de Unity.
    /// </summary>
    public class VRProjectConfiguratorEditor : EditorWindow
    {
        [MenuItem("VR Simulador/Configurar Proyecto VR", priority = 0)]
        public static void ConfigureVRProject()
        {
            if (
                EditorUtility.DisplayDialog(
                    "Configurar Proyecto VR",
                    "¿Deseas configurar automáticamente XR, calidad y settings para VR?",
                    "Sí",
                    "No"
                )
            )
            {
                ConfigureXR();
                ConfigureQuality();
                ConfigureProjectSettings();
                EditorUtility.DisplayDialog(
                    "Configuración Completa",
                    "¡Proyecto VR configurado exitosamente!",
                    "OK"
                );
            }
        }

        private static void ConfigureXR()
        {
#if UNITY_XR_MANAGEMENT
            Debug.Log("Configurando XR Plugin Management...");
            XRGeneralSettings generalSettings =
                XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(
                    BuildTargetGroup.Standalone
                );
            if (generalSettings != null)
            {
                generalSettings.InitManagerOnStart = true;
                EditorUtility.SetDirty(generalSettings);
            }
            // Nota: La activación de loaders específicos (Oculus, OpenXR) debe hacerse manualmente desde el XR Plugin Management UI.
            Debug.Log(
                "XR Plugin Management configurado. Verifica loaders manualmente si es necesario."
            );
#else
            Debug.LogWarning(
                "XR Management package no está instalado. Instálalo desde Package Manager."
            );
#endif
        }

        private static void ConfigureQuality()
        {
            Debug.Log("Configurando Quality Settings para VR...");
            QualitySettings.antiAliasing = 4; // MSAA 4x
            QualitySettings.vSyncCount = 0;
            QualitySettings.shadowDistance = 50f;
            QualitySettings.shadowResolution = ShadowResolution.High;
            QualitySettings.shadowCascades = 4;
            QualitySettings.shadowProjection = ShadowProjection.CloseFit;
            QualitySettings.softParticles = true;
            QualitySettings.realtimeReflectionProbes = true;
            QualitySettings.billboardsFaceCameraPosition = true;
            Debug.Log("Quality Settings configurados para VR.");
        }

        private static void ConfigureProjectSettings()
        {
            Debug.Log("Configurando Project Settings...");
            // Física
            Physics.defaultContactOffset = 0.01f;
            Physics.defaultSolverIterations = 6;
            Physics.defaultSolverVelocityIterations = 1;
            // Audio
            AudioConfiguration config = AudioSettings.GetConfiguration();
            config.sampleRate = 48000;
            config.speakerMode = AudioSpeakerMode.Stereo;
            config.dspBufferSize = 256;
            AudioSettings.Reset(config);
            // Input
            Input.simulateMouseWithTouches = false;
            Debug.Log("Project Settings configurados.");
        }
    }
}
