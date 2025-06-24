using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRTemplate.Setup
{
    /// <summary>
    /// Configura automáticamente el sistema de input para VR
    /// </summary>
    public class InputSystemSetup : MonoBehaviour
    {
        [Header("Configuración de Input")]
        [SerializeField]
        private bool enableVRInput = true;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject vrPlayerPrefab;

        private GameObject currentPlayer;
        private bool isVRMode = false;

        private void Start()
        {
            SetupInputSystem();
        }

        /// <summary>
        /// Configura el sistema de input según el dispositivo disponible
        /// </summary>
        public void SetupInputSystem()
        {
            Debug.Log("🔧 Configurando sistema de input...");

            // Verificar si estamos en la escena de configuración
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName == "ConfigurationScene")
            {
                Debug.Log("ℹ️ Escena de configuración detectada - no se creará player");
                return;
            }

            // Siempre configurar VR - crear XR Origin
            if (enableVRInput)
            {
                SetupVRInput();
            }
            else
            {
                Debug.LogWarning(
                    "⚠️ VR input deshabilitado - no se configurará ningún sistema de input"
                );
            }
        }

        /// <summary>
        /// Configura el input para VR usando el Tutorial Player
        /// </summary>
        /// <summary>
        /// Configura el input para VR usando el prefab VR completo
        /// </summary>
        private void SetupVRInput()
        {
            Debug.Log("�� Configurando input VR...");

            // Buscar spawn point
            Transform spawnPoint = FindSpawnPoint();

            // Cargar y usar el prefab VR completo
            GameObject vrPlayerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/VRTemplateAssets/Prefabs/Setup/Complete XR Origin Set Up Variant.prefab"
            );

            if (vrPlayerPrefab != null)
            {
                // Instanciar el prefab VR
                currentPlayer = PrefabUtility.InstantiatePrefab(vrPlayerPrefab) as GameObject;

                if (currentPlayer != null)
                {
                    // Posicionar en el spawn point
                    if (spawnPoint != null)
                    {
                        currentPlayer.transform.position = spawnPoint.position;
                        currentPlayer.transform.rotation = spawnPoint.rotation;
                    }
                    else
                    {
                        // Posición por defecto
                        currentPlayer.transform.position = new Vector3(0, 1.7f, 0);
                    }

                    Debug.Log("✅ XR Origin VR configurado correctamente");
                }
                else
                {
                    Debug.LogError("❌ No se pudo instanciar el prefab VR");
                }
            }
            else
            {
                Debug.LogError(
                    "❌ No se encontró el prefab VR: Complete XR Origin Set Up Variant.prefab"
                );
            }
        }

        /// <summary>
        /// Busca el spawn point en la escena
        /// </summary>
        private Transform FindSpawnPoint()
        {
            // Buscar por nombre primero
            GameObject spawnPoint = GameObject.Find("SpawnPoint");
            if (spawnPoint != null)
            {
                return spawnPoint.transform;
            }

            // Buscar por tag solo si existe
            try
            {
                spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
                if (spawnPoint != null)
                {
                    return spawnPoint.transform;
                }
            }
            catch (System.Exception)
            {
                Debug.Log("ℹ️ Tag 'SpawnPoint' no definido, continuando con búsqueda por nombre");
            }

            // Buscar en el stage
            GameObject stage = GameObject.Find("Stage");
            if (stage != null)
            {
                return stage.transform;
            }

            Debug.Log("⚠️ No se encontró spawn point, usando posición por defecto");
            return null;
        }

        /// <summary>
        /// Obtiene el player actual
        /// </summary>
        public GameObject GetCurrentPlayer()
        {
            return currentPlayer;
        }

        /// <summary>
        /// Verifica si estamos en modo VR
        /// </summary>
        public bool IsVRMode()
        {
            return isVRMode;
        }
    }
}
