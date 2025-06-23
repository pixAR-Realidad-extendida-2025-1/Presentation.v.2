using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VRTemplate.SceneManagement
{
    /// <summary>
    /// Sistema de gestión de escenas para el simulador VR de presentaciones
    /// Maneja transiciones, carga asíncrona y persistencia de datos
    /// </summary>
    public class SceneManager : MonoBehaviour
    {
        [Header("Configuración de Escenas")]
        [SerializeField]
        private string configurationSceneName = "ConfigurationScene";

        [SerializeField]
        private string classroomSceneName = "ClassroomScene";

        [SerializeField]
        private string auditoriumSceneName = "AuditoriumScene";

        [SerializeField]
        private string conferenceSceneName = "ConferenceScene";

        [Header("Configuración de Transiciones")]
        [SerializeField]
        private float transitionDuration = 1f;

        [SerializeField]
        private bool useLoadingScreen = true;

        [SerializeField]
        private GameObject loadingScreenPrefab;

        [Header("Configuración de Persistencia")]
        [SerializeField]
        private bool persistPDFPath = true;

        [SerializeField]
        private bool persistScenarioSelection = true;

        [Header("UI de Loading")]
        [SerializeField]
        private Canvas loadingCanvas;

        [SerializeField]
        private Slider progressBar;

        [SerializeField]
        private TextMeshProUGUI progressText;

        [SerializeField]
        private TextMeshProUGUI sceneNameText;

        // Datos persistentes entre escenas
        private static string selectedPDFPath = "";
        private static int selectedScenarioIndex = 0;
        private static bool isTransitioning = false;

        // Singleton pattern
        public static SceneManager Instance { get; private set; }

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadPersistentData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Configurar loading screen si no existe
            if (loadingCanvas == null && useLoadingScreen)
            {
                CreateLoadingScreen();
            }
        }

        /// <summary>
        /// Carga la escena de configuración (menú principal)
        /// </summary>
        public void LoadConfigurationScene()
        {
            LoadScene(configurationSceneName);
        }

        /// <summary>
        /// Carga la escena de sala de clases
        /// </summary>
        public void LoadClassroomScene()
        {
            selectedScenarioIndex = 0;
            SavePersistentData();
            LoadScene(classroomSceneName);
        }

        /// <summary>
        /// Carga la escena de auditorio
        /// </summary>
        public void LoadAuditoriumScene()
        {
            selectedScenarioIndex = 1;
            SavePersistentData();
            LoadScene(auditoriumSceneName);
        }

        /// <summary>
        /// Carga la escena de sala de conferencias
        /// </summary>
        public void LoadConferenceScene()
        {
            selectedScenarioIndex = 2;
            SavePersistentData();
            LoadScene(conferenceSceneName);
        }

        /// <summary>
        /// Carga una escena específica por nombre
        /// </summary>
        public void LoadScene(string sceneName)
        {
            if (isTransitioning)
            {
                Debug.LogWarning("Ya hay una transición en progreso");
                return;
            }

            StartCoroutine(LoadSceneAsync(sceneName));
        }

        /// <summary>
        /// Carga una escena de forma asíncrona con loading screen
        /// </summary>
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            isTransitioning = true;

            // Mostrar loading screen
            if (useLoadingScreen && loadingCanvas != null)
            {
                ShowLoadingScreen(sceneName);
            }

            // Iniciar carga asíncrona
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
                sceneName
            );
            asyncLoad.allowSceneActivation = false;

            // Actualizar progreso
            while (asyncLoad.progress < 0.9f)
            {
                float progress = asyncLoad.progress / 0.9f;
                UpdateLoadingProgress(progress);
                yield return null;
            }

            // Simular tiempo mínimo de carga
            yield return new WaitForSeconds(transitionDuration);

            // Completar carga
            asyncLoad.allowSceneActivation = true;

            // Esperar a que la escena esté completamente cargada
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Ocultar loading screen
            if (useLoadingScreen && loadingCanvas != null)
            {
                HideLoadingScreen();
            }

            // Configurar spawn point si es necesario
            SetupSpawnPoint();

            isTransitioning = false;
            Debug.Log($"✅ Escena {sceneName} cargada exitosamente");
        }

        /// <summary>
        /// Muestra la pantalla de carga
        /// </summary>
        private void ShowLoadingScreen(string sceneName)
        {
            if (loadingCanvas != null)
            {
                loadingCanvas.gameObject.SetActive(true);
                if (sceneNameText != null)
                {
                    sceneNameText.text = GetSceneDisplayName(sceneName);
                }
                if (progressBar != null)
                {
                    progressBar.value = 0f;
                }
                if (progressText != null)
                {
                    progressText.text = "0%";
                }
            }
        }

        /// <summary>
        /// Oculta la pantalla de carga
        /// </summary>
        private void HideLoadingScreen()
        {
            if (loadingCanvas != null)
            {
                loadingCanvas.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Actualiza el progreso de carga
        /// </summary>
        private void UpdateLoadingProgress(float progress)
        {
            if (progressBar != null)
            {
                progressBar.value = progress;
            }
            if (progressText != null)
            {
                progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
            }
        }

        /// <summary>
        /// Crea la pantalla de carga si no existe
        /// </summary>
        private void CreateLoadingScreen()
        {
            GameObject loadingGO = new GameObject("LoadingCanvas");
            loadingCanvas = loadingGO.AddComponent<Canvas>();
            loadingCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            loadingCanvas.sortingOrder = 1000;

            // Canvas Scaler
            CanvasScaler scaler = loadingGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            // Graphic Raycaster
            loadingGO.AddComponent<GraphicRaycaster>();

            // Panel de fondo
            GameObject panelGO = new GameObject("LoadingPanel");
            panelGO.transform.SetParent(loadingGO.transform, false);
            Image panelImage = panelGO.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.9f);

            RectTransform panelRect = panelGO.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            // Texto de título
            GameObject titleGO = new GameObject("Title");
            titleGO.transform.SetParent(panelGO.transform, false);
            TextMeshProUGUI titleText = titleGO.AddComponent<TextMeshProUGUI>();
            titleText.text = "Cargando Simulador VR";
            titleText.fontSize = 36;
            titleText.color = Color.white;
            titleText.alignment = TextAlignmentOptions.Center;

            RectTransform titleRect = titleGO.GetComponent<RectTransform>();
            titleRect.anchoredPosition = new Vector2(0, 100);
            titleRect.sizeDelta = new Vector2(600, 50);

            // Nombre de la escena
            GameObject sceneNameGO = new GameObject("SceneName");
            sceneNameGO.transform.SetParent(panelGO.transform, false);
            sceneNameText = sceneNameGO.AddComponent<TextMeshProUGUI>();
            sceneNameText.fontSize = 24;
            sceneNameText.color = Color.yellow;
            sceneNameText.alignment = TextAlignmentOptions.Center;

            RectTransform sceneNameRect = sceneNameGO.GetComponent<RectTransform>();
            sceneNameRect.anchoredPosition = new Vector2(0, 50);
            sceneNameRect.sizeDelta = new Vector2(600, 30);

            // Barra de progreso
            GameObject progressBarGO = new GameObject("ProgressBar");
            progressBarGO.transform.SetParent(panelGO.transform, false);
            progressBar = progressBarGO.AddComponent<Slider>();
            progressBar.minValue = 0f;
            progressBar.maxValue = 1f;
            progressBar.value = 0f;

            RectTransform progressBarRect = progressBarGO.GetComponent<RectTransform>();
            progressBarRect.anchoredPosition = new Vector2(0, -50);
            progressBarRect.sizeDelta = new Vector2(400, 20);

            // Texto de progreso
            GameObject progressTextGO = new GameObject("ProgressText");
            progressTextGO.transform.SetParent(panelGO.transform, false);
            progressText = progressTextGO.AddComponent<TextMeshProUGUI>();
            progressText.fontSize = 18;
            progressText.color = Color.white;
            progressText.alignment = TextAlignmentOptions.Center;

            RectTransform progressTextRect = progressTextGO.GetComponent<RectTransform>();
            progressTextRect.anchoredPosition = new Vector2(0, -80);
            progressTextRect.sizeDelta = new Vector2(200, 30);

            // Ocultar inicialmente
            loadingCanvas.gameObject.SetActive(false);
        }

        /// <summary>
        /// Obtiene el nombre de visualización de una escena
        /// </summary>
        private string GetSceneDisplayName(string sceneName)
        {
            switch (sceneName)
            {
                case "ConfigurationScene":
                    return "Menú Principal";
                case "ClassroomScene":
                    return "Sala de Clases";
                case "AuditoriumScene":
                    return "Auditorio";
                case "ConferenceScene":
                    return "Sala de Conferencias";
                default:
                    return sceneName;
            }
        }

        /// <summary>
        /// Configura el spawn point en la escena cargada
        /// </summary>
        private void SetupSpawnPoint()
        {
            GameObject spawnPoint = GameObject.Find("SpawnPoint");
            if (spawnPoint != null)
            {
                // Aquí se puede configurar el XR Origin si es necesario
                Debug.Log("✅ Spawn point encontrado en la escena");
            }
            else
            {
                Debug.LogWarning("⚠️ No se encontró SpawnPoint en la escena");
            }
        }

        /// <summary>
        /// Guarda datos persistentes
        /// </summary>
        private void SavePersistentData()
        {
            if (persistPDFPath)
            {
                PlayerPrefs.SetString("SelectedPDFPath", selectedPDFPath);
            }
            if (persistScenarioSelection)
            {
                PlayerPrefs.SetInt("SelectedScenario", selectedScenarioIndex);
            }
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Carga datos persistentes
        /// </summary>
        private void LoadPersistentData()
        {
            if (persistPDFPath)
            {
                selectedPDFPath = PlayerPrefs.GetString("SelectedPDFPath", "");
            }
            if (persistScenarioSelection)
            {
                selectedScenarioIndex = PlayerPrefs.GetInt("SelectedScenario", 0);
            }
        }

        /// <summary>
        /// Establece la ruta del PDF seleccionado
        /// </summary>
        public void SetSelectedPDFPath(string pdfPath)
        {
            selectedPDFPath = pdfPath;
            if (persistPDFPath)
            {
                PlayerPrefs.SetString("SelectedPDFPath", pdfPath);
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// Obtiene la ruta del PDF seleccionado
        /// </summary>
        public string GetSelectedPDFPath()
        {
            return selectedPDFPath;
        }

        /// <summary>
        /// Verifica si hay un PDF seleccionado
        /// </summary>
        public bool HasSelectedPDF()
        {
            return !string.IsNullOrEmpty(selectedPDFPath) && File.Exists(selectedPDFPath);
        }

        /// <summary>
        /// Obtiene el índice del escenario seleccionado
        /// </summary>
        public int GetSelectedScenarioIndex()
        {
            return selectedScenarioIndex;
        }

        /// <summary>
        /// Obtiene el nombre del escenario seleccionado
        /// </summary>
        public string GetSelectedScenarioName()
        {
            switch (selectedScenarioIndex)
            {
                case 0:
                    return classroomSceneName;
                case 1:
                    return auditoriumSceneName;
                case 2:
                    return conferenceSceneName;
                default:
                    return classroomSceneName;
            }
        }

        /// <summary>
        /// Verifica si hay una transición en progreso
        /// </summary>
        public bool IsTransitioning()
        {
            return isTransitioning;
        }

        /// <summary>
        /// Método público para cargar escena desde UI
        /// </summary>
        [ContextMenu("Cargar Escena de Configuración")]
        public void LoadConfigurationSceneFromMenu()
        {
            LoadConfigurationScene();
        }

        /// <summary>
        /// Método público para cargar escena desde UI
        /// </summary>
        [ContextMenu("Cargar Sala de Clases")]
        public void LoadClassroomSceneFromMenu()
        {
            LoadClassroomScene();
        }

        /// <summary>
        /// Método público para cargar escena desde UI
        /// </summary>
        [ContextMenu("Cargar Auditorio")]
        public void LoadAuditoriumSceneFromMenu()
        {
            LoadAuditoriumScene();
        }

        /// <summary>
        /// Método público para cargar escena desde UI
        /// </summary>
        [ContextMenu("Cargar Sala de Conferencias")]
        public void LoadConferenceSceneFromMenu()
        {
            LoadConferenceScene();
        }
    }
}
