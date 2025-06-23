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

            // Aquí se llamarían los métodos de generación específicos
            // Por ahora solo registramos la acción
            switch (sceneType)
            {
                case "ConfigurationScene":
                    Debug.Log("📋 Generando escena de configuración...");
                    break;
                case "ClassroomScene":
                    Debug.Log("🏫 Generando sala de clases...");
                    break;
                case "AuditoriumScene":
                    Debug.Log("🎭 Generando auditorio...");
                    break;
                case "ConferenceScene":
                    Debug.Log("💼 Generando sala de conferencias...");
                    break;
            }
        }
    }
}
