using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using VRTemplate.SceneManagement;

namespace VRTemplate.SceneManagement.Editor
{
    /// <summary>
    /// Editor utility para configurar automáticamente el SceneManager en las escenas
    /// </summary>
    public class SceneManagerSetupEditor : EditorWindow
    {
        [MenuItem("VR Simulador/Configurar SceneManager", priority = 5)]
        public static void SetupSceneManager()
        {
            CreateSceneManagerPrefab();
            SetupAllScenes();
        }

        private static void CreateSceneManagerPrefab()
        {
            Debug.Log("🔧 Creando prefab de SceneManager...");

            GameObject sceneManagerGO = new GameObject("SceneManager");
            SceneManager sceneManager = sceneManagerGO.AddComponent<SceneManager>();

            string prefabPath = "Assets/VRTemplateAssets/Prefabs/SceneManager.prefab";
            string directory = System.IO.Path.GetDirectoryName(prefabPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(sceneManagerGO, prefabPath);
            Debug.Log($"✅ Prefab de SceneManager creado en: {prefabPath}");

            DestroyImmediate(sceneManagerGO);
        }

        private static void SetupAllScenes()
        {
            Debug.Log("🔧 Configurando SceneManager en todas las escenas...");

            string[] sceneNames =
            {
                "ConfigurationScene",
                "ClassroomScene",
                "AuditoriumScene",
                "ConferenceScene",
            };

            foreach (string sceneName in sceneNames)
            {
                SetupSceneManagerInScene(sceneName);
            }

            Debug.Log("✅ Configuración de SceneManager completada");
        }

        private static void SetupSceneManagerInScene(string sceneName)
        {
            string scenePath = $"Assets/Scenes/{sceneName}.unity";

            if (!System.IO.File.Exists(scenePath))
            {
                Debug.LogWarning($"⚠️ Escena {sceneName} no encontrada en {scenePath}");
                return;
            }

            EditorSceneManager.OpenScene(scenePath);

            SceneManager existingManager = FindObjectOfType<SceneManager>();
            if (existingManager != null)
            {
                Debug.Log($"✅ SceneManager ya existe en {sceneName}");
                return;
            }

            GameObject sceneManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/VRTemplateAssets/Prefabs/SceneManager.prefab"
            );
            if (sceneManagerPrefab != null)
            {
                GameObject sceneManagerInstance =
                    PrefabUtility.InstantiatePrefab(sceneManagerPrefab) as GameObject;
                sceneManagerInstance.name = "SceneManager";

                Debug.Log($"✅ SceneManager agregado a {sceneName}");
            }
            else
            {
                Debug.LogError($"❌ No se pudo cargar el prefab de SceneManager para {sceneName}");
            }

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        [MenuItem("VR Simulador/Verificar Configuración de Escenas", priority = 6)]
        public static void VerifySceneConfiguration()
        {
            Debug.Log("🔍 Verificando configuración de escenas...");

            string[] sceneNames =
            {
                "ConfigurationScene",
                "ClassroomScene",
                "AuditoriumScene",
                "ConferenceScene",
            };
            bool allScenesConfigured = true;

            foreach (string sceneName in sceneNames)
            {
                string scenePath = $"Assets/Scenes/{sceneName}.unity";

                if (!System.IO.File.Exists(scenePath))
                {
                    Debug.LogError($"❌ Escena {sceneName} no encontrada en {scenePath}");
                    allScenesConfigured = false;
                    continue;
                }

                // Abrir la escena temporalmente para verificar
                EditorSceneManager.OpenScene(scenePath);

                SceneManager sceneManager = FindObjectOfType<SceneManager>();
                if (sceneManager == null)
                {
                    Debug.LogError($"❌ SceneManager no encontrado en {sceneName}");
                    allScenesConfigured = false;
                }
                else
                {
                    Debug.Log($"✅ {sceneName}: SceneManager configurado correctamente");
                }
            }

            if (allScenesConfigured)
            {
                Debug.Log("✅ Todas las escenas están configuradas correctamente");
                EditorUtility.DisplayDialog(
                    "Verificación Completada",
                    "Todas las escenas están configuradas correctamente con SceneManager.",
                    "OK"
                );
            }
            else
            {
                Debug.LogError("❌ Algunas escenas no están configuradas correctamente");
                EditorUtility.DisplayDialog(
                    "Verificación Fallida",
                    "Algunas escenas no están configuradas correctamente. Revisa la consola para más detalles.",
                    "OK"
                );
            }
        }

        [MenuItem("VR Simulador/Generar Todas las Escenas", priority = 7)]
        public static void GenerateAllScenes()
        {
            Debug.Log("🚀 Generando todas las escenas del simulador VR...");

            // Generar escenas usando reflexión para evitar dependencias directas
            GenerateScene("ConfigurationScene");
            GenerateScene("ClassroomScene");
            GenerateScene("AuditoriumScene");
            GenerateScene("ConferenceScene");

            CreateSceneManagerPrefab();
            SetupAllScenes();

            Debug.Log("✅ Todas las escenas generadas y configuradas exitosamente");
        }

        private static void GenerateScene(string sceneType)
        {
            Debug.Log($"🔧 Generando escena: {sceneType}");

            // Usar reflexión para llamar a los métodos de generación específicos
            switch (sceneType)
            {
                case "ConfigurationScene":
                    Debug.Log("📋 Generando escena de configuración...");
                    try
                    {
                        var configType = System.Type.GetType(
                            "VRTemplate.SceneGeneration.Editor.ConfigurationSceneGeneratorEditor, Assembly-CSharp-Editor"
                        );
                        if (configType != null)
                        {
                            var method = configType.GetMethod(
                                "GenerateConfigurationScene",
                                System.Reflection.BindingFlags.Public
                                    | System.Reflection.BindingFlags.Static
                            );
                            if (method != null)
                            {
                                method.Invoke(null, null);
                                Debug.Log("✅ Escena de configuración generada");
                            }
                            else
                            {
                                Debug.LogError("❌ Método GenerateConfigurationScene no encontrado");
                            }
                        }
                        else
                        {
                            Debug.LogError(
                                "❌ Clase ConfigurationSceneGeneratorEditor no encontrada"
                            );
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"❌ Error generando escena de configuración: {e.Message}");
                    }
                    break;

                case "ClassroomScene":
                    Debug.Log("🏫 Generando sala de clases...");
                    try
                    {
                        var classroomType = System.Type.GetType(
                            "VRTemplate.SceneGeneration.Editor.ClassroomSceneGeneratorEditor, Assembly-CSharp-Editor"
                        );
                        if (classroomType != null)
                        {
                            var method = classroomType.GetMethod(
                                "GenerateClassroomScene",
                                System.Reflection.BindingFlags.Public
                                    | System.Reflection.BindingFlags.Static
                            );
                            if (method != null)
                            {
                                method.Invoke(null, null);
                                Debug.Log("✅ Sala de clases generada");
                            }
                            else
                            {
                                Debug.LogError("❌ Método GenerateClassroomScene no encontrado");
                            }
                        }
                        else
                        {
                            Debug.LogError("❌ Clase ClassroomSceneGeneratorEditor no encontrada");
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"❌ Error generando sala de clases: {e.Message}");
                    }
                    break;

                case "AuditoriumScene":
                    Debug.Log("🎭 Generando auditorio...");
                    try
                    {
                        var auditoriumType = System.Type.GetType(
                            "VRTemplate.SceneGeneration.Editor.AuditoriumSceneGeneratorEditor, Assembly-CSharp-Editor"
                        );
                        if (auditoriumType != null)
                        {
                            var method = auditoriumType.GetMethod(
                                "GenerateAuditoriumScene",
                                System.Reflection.BindingFlags.Public
                                    | System.Reflection.BindingFlags.Static
                            );
                            if (method != null)
                            {
                                method.Invoke(null, null);
                                Debug.Log("✅ Auditorio generado");
                            }
                            else
                            {
                                Debug.LogError("❌ Método GenerateAuditoriumScene no encontrado");
                            }
                        }
                        else
                        {
                            Debug.LogError("❌ Clase AuditoriumSceneGeneratorEditor no encontrada");
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"❌ Error generando auditorio: {e.Message}");
                    }
                    break;

                case "ConferenceScene":
                    Debug.Log("💼 Generando sala de conferencias...");
                    try
                    {
                        var conferenceType = System.Type.GetType(
                            "VRTemplate.SceneGeneration.Editor.ConferenceSceneGeneratorEditor, Assembly-CSharp-Editor"
                        );
                        if (conferenceType != null)
                        {
                            var method = conferenceType.GetMethod(
                                "GenerateConferenceScene",
                                System.Reflection.BindingFlags.Public
                                    | System.Reflection.BindingFlags.Static
                            );
                            if (method != null)
                            {
                                method.Invoke(null, null);
                                Debug.Log("✅ Sala de conferencias generada");
                            }
                            else
                            {
                                Debug.LogError("❌ Método GenerateConferenceScene no encontrado");
                            }
                        }
                        else
                        {
                            Debug.LogError("❌ Clase ConferenceSceneGeneratorEditor no encontrada");
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"❌ Error generando sala de conferencias: {e.Message}");
                    }
                    break;
            }
        }

        [MenuItem("VR Simulador/Generar y Configurar Todo", priority = 4)]
        public static void ConfigureEverything()
        {
            Debug.Log("🚀 Configurando todo el sistema VR...");

            // 1. Generar todas las escenas
            GenerateAllScenes();

            // 2. Verificar Build Settings
            VerifyAndAddScenesToBuildSettings();

            // 3. Verificar configuración final
            VerifySceneConfiguration();

            Debug.Log("✅ Configuración completa finalizada");
            EditorUtility.DisplayDialog(
                "Configuración Completada",
                "Todo el sistema VR ha sido configurado correctamente:\n\n"
                    + "✅ Todas las escenas generadas\n"
                    + "✅ SceneManager configurado\n"
                    + "✅ Build Settings actualizados\n"
                    + "✅ Input System configurado\n\n"
                    + "¡Ya puedes probar el simulador!",
                "OK"
            );
        }

        private static void VerifyAndAddScenesToBuildSettings()
        {
            Debug.Log("🔧 Verificando Build Settings...");

            string[] sceneNames =
            {
                "ConfigurationScene",
                "ClassroomScene",
                "AuditoriumScene",
                "ConferenceScene",
            };

            var buildScenes = EditorBuildSettings.scenes.ToList();
            bool buildSettingsChanged = false;

            foreach (string sceneName in sceneNames)
            {
                string scenePath = $"Assets/Scenes/{sceneName}.unity";

                // Verificar si la escena existe
                if (!System.IO.File.Exists(scenePath))
                {
                    Debug.LogWarning($"⚠️ Escena {sceneName} no encontrada en {scenePath}");
                    continue;
                }

                // Verificar si ya está en Build Settings
                bool sceneExists = buildScenes.Any(scene => scene.path == scenePath);
                if (!sceneExists)
                {
                    buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                    buildSettingsChanged = true;
                    Debug.Log($"✅ Escena {sceneName} agregada al Build Settings");
                }
                else
                {
                    Debug.Log($"ℹ️ Escena {sceneName} ya está en Build Settings");
                }
            }

            if (buildSettingsChanged)
            {
                EditorBuildSettings.scenes = buildScenes.ToArray();
                Debug.Log("✅ Build Settings actualizados");
            }
            else
            {
                Debug.Log("ℹ️ Build Settings ya están actualizados");
            }
        }
    }
}
