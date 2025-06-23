using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using VRTemplate.Setup;

namespace VRTemplate.Setup.Editor
{
    public class InputSystemSetupEditor : EditorWindow
    {
        [MenuItem("VR Simulador/Configurar Sistema de Input", priority = 6)]
        public static void SetupInputSystem()
        {
            CreateInputSystemPrefab();
            SetupAllScenes();
        }

        private static void CreateInputSystemPrefab()
        {
            Debug.Log("🔧 Creando prefab de InputSystem...");

            GameObject inputSystemGO = new GameObject("InputSystem");
            InputSystemSetup inputSystem = inputSystemGO.AddComponent<InputSystemSetup>();

            // Configurar parámetros por defecto
            inputSystem.name = "InputSystem";

            string prefabPath = "Assets/VRTemplateAssets/Prefabs/InputSystem.prefab";
            string directory = System.IO.Path.GetDirectoryName(prefabPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(inputSystemGO, prefabPath);
            Debug.Log($"✅ Prefab de InputSystem creado en: {prefabPath}");

            DestroyImmediate(inputSystemGO);
        }

        private static void SetupAllScenes()
        {
            Debug.Log("🔧 Configurando sistema de input en todas las escenas...");

            string[] sceneNames =
            {
                "ConfigurationScene",
                "ClassroomScene",
                "AuditoriumScene",
                "ConferenceScene",
            };

            foreach (string sceneName in sceneNames)
            {
                SetupInputSystemInScene(sceneName);
            }

            Debug.Log("✅ Configuración de sistema de input completada");
        }

        private static void SetupInputSystemInScene(string sceneName)
        {
            string scenePath = $"Assets/Scenes/{sceneName}.unity";

            if (!System.IO.File.Exists(scenePath))
            {
                Debug.LogWarning($"⚠️ Escena {sceneName} no encontrada en {scenePath}");
                return;
            }

            EditorSceneManager.OpenScene(scenePath);

            InputSystemSetup existingInputSystem = FindObjectOfType<InputSystemSetup>();
            if (existingInputSystem != null)
            {
                Debug.Log($"✅ InputSystem ya existe en {sceneName}");
                return;
            }

            // Cargar el prefab del InputSystem
            GameObject inputSystemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/VRTemplateAssets/Prefabs/InputSystem.prefab"
            );
            if (inputSystemPrefab != null)
            {
                GameObject inputSystemInstance =
                    PrefabUtility.InstantiatePrefab(inputSystemPrefab) as GameObject;
                inputSystemInstance.name = "InputSystem";

                Debug.Log($"✅ InputSystem agregado a {sceneName}");
            }
            else
            {
                Debug.LogError($"❌ No se pudo cargar el prefab de InputSystem para {sceneName}");
            }

            // Solo crear spawn point en escenas de presentación (no en ConfigurationScene)
            if (sceneName != "ConfigurationScene")
            {
                CreateSpawnPointIfNeeded(sceneName);
            }
            else
            {
                Debug.Log(
                    $"ℹ️ Saltando creación de SpawnPoint en {sceneName} (escena de configuración)"
                );
            }

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        private static void CreateSpawnPointIfNeeded(string sceneName)
        {
            // Buscar spawn point existente por nombre primero
            GameObject existingSpawnPoint = GameObject.Find("SpawnPoint");
            if (existingSpawnPoint != null)
            {
                Debug.Log($"✅ SpawnPoint ya existe en {sceneName}");
                return;
            }

            // Buscar por tag solo si existe
            try
            {
                existingSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
                if (existingSpawnPoint != null)
                {
                    Debug.Log($"✅ SpawnPoint ya existe en {sceneName}");
                    return;
                }
            }
            catch (System.Exception)
            {
                Debug.Log("ℹ️ Tag 'SpawnPoint' no definido, continuando con búsqueda por nombre");
            }

            // Buscar stage para usar como referencia
            GameObject stage = GameObject.Find("Stage");
            Vector3 spawnPosition = Vector3.zero;
            Quaternion spawnRotation = Quaternion.identity;

            if (stage != null)
            {
                // Posicionar spawn point en el stage
                spawnPosition = stage.transform.position + Vector3.up * 0.1f;
                spawnRotation = stage.transform.rotation;
            }
            else
            {
                // Posición por defecto según el tipo de escena
                switch (sceneName)
                {
                    case "ClassroomScene":
                        spawnPosition = new Vector3(0, 1.7f, -2f);
                        break;
                    case "AuditoriumScene":
                        spawnPosition = new Vector3(0, 1.7f, -3f);
                        break;
                    case "ConferenceScene":
                        spawnPosition = new Vector3(0, 1.7f, -1.5f);
                        break;
                    default:
                        spawnPosition = new Vector3(0, 1.7f, 0);
                        break;
                }
            }

            // Crear spawn point
            GameObject spawnPoint = new GameObject("SpawnPoint");
            spawnPoint.transform.position = spawnPosition;
            spawnPoint.transform.rotation = spawnRotation;

            // Intentar asignar el tag solo si existe
            try
            {
                spawnPoint.tag = "SpawnPoint";
            }
            catch (System.Exception)
            {
                Debug.Log("ℹ️ Tag 'SpawnPoint' no definido, usando solo nombre del objeto");
            }

            Debug.Log($"✅ SpawnPoint creado en {sceneName} en posición {spawnPosition}");
        }

        [MenuItem("VR Simulador/Configurar Todo (SceneManager + Input)", priority = 8)]
        public static void SetupEverything()
        {
            Debug.Log("🚀 Configurando todo el sistema...");

            // Configurar SceneManager
            VRTemplate.SceneManagement.Editor.SceneManagerSetupEditor.SetupSceneManager();

            // Configurar InputSystem
            SetupInputSystem();

            Debug.Log("✅ Todo configurado exitosamente");
            EditorUtility.DisplayDialog(
                "Configuración Completada",
                "Se ha configurado exitosamente:\n"
                    + "✅ SceneManager en todas las escenas\n"
                    + "✅ Sistema de Input (VR + Teclado/Mouse)\n"
                    + "✅ SpawnPoints en todas las escenas\n\n"
                    + "¡Ya puedes probar el simulador!",
                "OK"
            );
        }

        [MenuItem("VR Simulador/Verificar Configuración Completa", priority = 9)]
        public static void VerifyCompleteSetup()
        {
            Debug.Log("🔍 Verificando configuración completa...");

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
                    Debug.LogError($"❌ Escena {sceneName} no encontrada");
                    allScenesConfigured = false;
                    continue;
                }

                EditorSceneManager.OpenScene(scenePath);

                // Verificar SceneManager
                VRTemplate.SceneManagement.SceneManager sceneManager =
                    FindObjectOfType<VRTemplate.SceneManagement.SceneManager>();
                if (sceneManager == null)
                {
                    Debug.LogError($"❌ SceneManager no encontrado en {sceneName}");
                    allScenesConfigured = false;
                }

                // Verificar InputSystem
                InputSystemSetup inputSystem = FindObjectOfType<InputSystemSetup>();
                if (inputSystem == null)
                {
                    Debug.LogError($"❌ InputSystem no encontrado en {sceneName}");
                    allScenesConfigured = false;
                }

                // Verificar SpawnPoint
                GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
                if (spawnPoint == null)
                {
                    Debug.LogError($"❌ SpawnPoint no encontrado en {sceneName}");
                    allScenesConfigured = false;
                }

                if (sceneManager != null && inputSystem != null && spawnPoint != null)
                {
                    Debug.Log($"✅ {sceneName}: Todo configurado correctamente");
                }
            }

            if (allScenesConfigured)
            {
                Debug.Log("✅ Todas las escenas están completamente configuradas");
                EditorUtility.DisplayDialog(
                    "Verificación Completada",
                    "✅ Todas las escenas están completamente configuradas:\n"
                        + "• SceneManager\n"
                        + "• InputSystem (VR + Teclado/Mouse)\n"
                        + "• SpawnPoints\n\n"
                        + "¡El simulador está listo para usar!",
                    "OK"
                );
            }
            else
            {
                Debug.LogError("❌ Algunas escenas no están completamente configuradas");
                EditorUtility.DisplayDialog(
                    "Verificación Fallida",
                    "❌ Algunas escenas no están completamente configuradas.\n"
                        + "Revisa la consola para más detalles.\n\n"
                        + "Usa 'Configurar Todo' para solucionarlo.",
                    "OK"
                );
            }
        }
    }
}
