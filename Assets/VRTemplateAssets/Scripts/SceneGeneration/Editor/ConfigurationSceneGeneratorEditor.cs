using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace VRTemplate.SceneGeneration.Editor
{
    /// <summary>
    /// Editor utility para generar la escena de configuración (menú principal) automáticamente
    /// </summary>
    public class ConfigurationSceneGeneratorEditor : EditorWindow
    {
        [MenuItem("VR Simulador/Generar Escena de Configuración", priority = 1)]
        public static void GenerateConfigurationScene()
        {
            CreateConfigurationScene();
        }

        private static void CreateConfigurationScene()
        {
            Debug.Log("🎮 Generando escena de configuración...");

            // Crear nueva escena
            Scene newScene = EditorSceneManager.NewScene(
                NewSceneSetup.DefaultGameObjects,
                NewSceneMode.Single
            );

            // Eliminar la luz direccional por defecto para un control total
            var defaultLight = Object.FindObjectOfType<Light>();
            if (defaultLight != null && defaultLight.type == LightType.Directional)
            {
                Object.DestroyImmediate(defaultLight.gameObject);
            }

            // Configurar cámara principal
            SetupMainCamera();

            // Iluminación direccional
            CreateDirectionalLighting();

            // Crear EventSystem para interacciones
            CreateEventSystem();

            // Crear Canvas y UI
            CreateUI();

            // Guardar la escena
            string scenePath = "Assets/Scenes/ConfigurationScene.unity";
            SaveScene(scenePath);

            // Agregar la escena al Build Settings
            AddSceneToBuildSettings(scenePath);

            Debug.Log("✅ Escena de configuración generada exitosamente en: " + scenePath);

            EditorUtility.DisplayDialog(
                "Escena Generada",
                "La escena de configuración ha sido generada exitosamente.\n\n"
                    + "Ubicación: Assets/Scenes/ConfigurationScene.unity\n\n"
                    + "La escena ha sido agregada automáticamente al Build Settings.",
                "OK"
            );
        }

        private static void CreateEventSystem()
        {
            // Verificar si ya existe un EventSystem
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystemGO = new GameObject("EventSystem");
                eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystemGO.AddComponent<UnityEngine.XR.Interaction.Toolkit.UI.XRUIInputModule>();
                Debug.Log("✅ EventSystem creado");
            }
        }

        private static void CreateUI()
        {
            // Crear Canvas principal
            GameObject canvasGO = new GameObject("MainCanvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            // Agregar CanvasScaler para UI responsiva
            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            // Agregar GraphicRaycaster para interacciones
            canvasGO.AddComponent<GraphicRaycaster>();

            // Crear panel principal
            GameObject panelGO = new GameObject("MainPanel");
            panelGO.transform.SetParent(canvasGO.transform, false);

            UnityEngine.UI.Image panelImage = panelGO.AddComponent<UnityEngine.UI.Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            RectTransform panelRect = panelGO.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            // Crear título
            CreateTitle(panelGO.transform);

            // Crear dropdown de escenarios
            CreateScenarioDropdown(panelGO.transform);

            // Crear botón de cargar PDF
            CreateLoadPDFButton(panelGO.transform);

            // Crear información del PDF
            CreatePDFInfo(panelGO.transform);

            // Crear botón de iniciar presentación
            CreateStartButton(panelGO.transform);

            // Agregar el script de control
            canvasGO.AddComponent<ConfigurationController>();

            Debug.Log("✅ UI creada exitosamente");
        }

        private static void CreateTitle(Transform parent)
        {
            GameObject titleGO = new GameObject("Title");
            titleGO.transform.SetParent(parent, false);

            TMPro.TextMeshProUGUI titleText = titleGO.AddComponent<TMPro.TextMeshProUGUI>();
            titleText.text = "Simulador VR de Presentaciones";
            titleText.fontSize = 36;
            titleText.fontStyle = TMPro.FontStyles.Bold;
            titleText.alignment = TMPro.TextAlignmentOptions.Center;
            titleText.color = Color.white;

            RectTransform titleRect = titleGO.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.5f);
            titleRect.anchorMax = new Vector2(0.5f, 0.5f);
            titleRect.anchoredPosition = new Vector2(0, 200);
            titleRect.sizeDelta = new Vector2(600, 60);
        }

        private static void CreateScenarioDropdown(Transform parent)
        {
            // Crear label
            GameObject labelGO = new GameObject("ScenarioLabel");
            labelGO.transform.SetParent(parent, false);

            TMPro.TextMeshProUGUI labelText = labelGO.AddComponent<TMPro.TextMeshProUGUI>();
            labelText.text = "Seleccionar Escenario:";
            labelText.fontSize = 16;
            labelText.color = Color.white;
            labelText.alignment = TMPro.TextAlignmentOptions.Center;

            RectTransform labelRect = labelGO.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.5f, 0.5f);
            labelRect.anchorMax = new Vector2(0.5f, 0.5f);
            labelRect.anchoredPosition = new Vector2(0, 150);
            labelRect.sizeDelta = new Vector2(400, 30);

            // Cargar y instanciar el prefab de dropdown de VRTemplateAssets
            GameObject dropdownPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/VRTemplateAssets/Prefabs/UI/Dropdown.prefab"
            );
            if (dropdownPrefab != null)
            {
                GameObject dropdownGO =
                    PrefabUtility.InstantiatePrefab(dropdownPrefab) as GameObject;
                dropdownGO.name = "ScenarioDropdown";
                dropdownGO.transform.SetParent(parent, false);

                // Configurar RectTransform
                RectTransform dropdownRect = dropdownGO.GetComponent<RectTransform>();
                dropdownRect.anchorMin = new Vector2(0.5f, 0.5f);
                dropdownRect.anchorMax = new Vector2(0.5f, 0.5f);
                dropdownRect.anchoredPosition = new Vector2(0, 100);
                dropdownRect.sizeDelta = new Vector2(400, 50);

                // Configurar opciones del dropdown
                TMPro.TMP_Dropdown dropdown = dropdownGO.GetComponent<TMPro.TMP_Dropdown>();
                if (dropdown != null)
                {
                    dropdown.options.Clear();
                    dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData("Sala de Clases"));
                    dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData("Auditorio"));
                    dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData("Sala de Conferencias"));
                    dropdown.value = 0;
                }

                Debug.Log("✅ Dropdown prefab instanciado correctamente");
            }
            else
            {
                Debug.LogError(
                    "❌ No se pudo cargar el prefab de dropdown desde VRTemplateAssets/Prefabs/UI/Dropdown.prefab"
                );

                // Fallback: crear un dropdown básico
                GameObject dropdownGO = new GameObject("ScenarioDropdown");
                dropdownGO.transform.SetParent(parent, false);

                TMPro.TMP_Dropdown dropdown = dropdownGO.AddComponent<TMPro.TMP_Dropdown>();

                // Configurar opciones
                dropdown.options.Clear();
                dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData("Sala de Clases"));
                dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData("Auditorio"));
                dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData("Sala de Conferencias"));
                dropdown.value = 0;

                // Configurar RectTransform
                RectTransform dropdownRect = dropdownGO.GetComponent<RectTransform>();
                dropdownRect.anchorMin = new Vector2(0.5f, 0.5f);
                dropdownRect.anchorMax = new Vector2(0.5f, 0.5f);
                dropdownRect.anchoredPosition = new Vector2(0, 100);
                dropdownRect.sizeDelta = new Vector2(400, 50);

                // Configurar imagen del dropdown
                UnityEngine.UI.Image dropdownImage =
                    dropdownGO.AddComponent<UnityEngine.UI.Image>();
                dropdownImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            }
        }

        private static void CreateLoadPDFButton(Transform parent)
        {
            GameObject buttonGO = new GameObject("LoadPDFButton");
            buttonGO.transform.SetParent(parent, false);

            UnityEngine.UI.Button button = buttonGO.AddComponent<UnityEngine.UI.Button>();

            // Configurar imagen del botón
            UnityEngine.UI.Image buttonImage = buttonGO.AddComponent<UnityEngine.UI.Image>();
            buttonImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);

            // Configurar texto del botón
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform, false);

            TMPro.TextMeshProUGUI buttonText = textGO.AddComponent<TMPro.TextMeshProUGUI>();
            buttonText.text = "Cargar PDF";
            buttonText.fontSize = 20;
            buttonText.fontStyle = TMPro.FontStyles.Bold;
            buttonText.color = Color.white;
            buttonText.alignment = TMPro.TextAlignmentOptions.Center;

            RectTransform textRect = textGO.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            // Configurar RectTransform del botón
            RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.anchoredPosition = new Vector2(0, 50);
            buttonRect.sizeDelta = new Vector2(250, 60);
        }

        private static void CreatePDFInfo(Transform parent)
        {
            GameObject infoGO = new GameObject("PDFInfo");
            infoGO.transform.SetParent(parent, false);

            TMPro.TextMeshProUGUI infoText = infoGO.AddComponent<TMPro.TextMeshProUGUI>();
            infoText.text = "Ningún PDF seleccionado";
            infoText.fontSize = 14;
            infoText.color = Color.gray;
            infoText.alignment = TMPro.TextAlignmentOptions.Center;

            RectTransform infoRect = infoGO.GetComponent<RectTransform>();
            infoRect.anchorMin = new Vector2(0.5f, 0.5f);
            infoRect.anchorMax = new Vector2(0.5f, 0.5f);
            infoRect.anchoredPosition = new Vector2(0, 0);
            infoRect.sizeDelta = new Vector2(400, 30);
        }

        private static void CreateStartButton(Transform parent)
        {
            GameObject buttonGO = new GameObject("StartButton");
            buttonGO.transform.SetParent(parent, false);

            UnityEngine.UI.Button button = buttonGO.AddComponent<UnityEngine.UI.Button>();

            // Configurar imagen del botón
            UnityEngine.UI.Image buttonImage = buttonGO.AddComponent<UnityEngine.UI.Image>();
            buttonImage.color = new Color(0.2f, 0.6f, 0.2f, 1f);

            // Configurar texto del botón
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform, false);

            TMPro.TextMeshProUGUI buttonText = textGO.AddComponent<TMPro.TextMeshProUGUI>();
            buttonText.text = "Iniciar Presentación";
            buttonText.fontSize = 20;
            buttonText.fontStyle = TMPro.FontStyles.Bold;
            buttonText.color = Color.white;
            buttonText.alignment = TMPro.TextAlignmentOptions.Center;

            RectTransform textRect = textGO.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            // Configurar RectTransform del botón
            RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
            buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
            buttonRect.anchoredPosition = new Vector2(0, -50);
            buttonRect.sizeDelta = new Vector2(250, 60);

            // Inicialmente deshabilitado
            button.interactable = false;
        }

        private static void SetupMainCamera()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                mainCamera.clearFlags = CameraClearFlags.Skybox;
                mainCamera.backgroundColor = new Color(0.1f, 0.1f, 0.15f);
                mainCamera.orthographic = true;
                mainCamera.orthographicSize = 5f;

                RectTransform cameraRect = mainCamera.GetComponent<RectTransform>();
                if (cameraRect != null)
                    cameraRect.sizeDelta = new Vector2(1, 1);
            }
        }

        private static void CreateDirectionalLighting()
        {
            GameObject mainLight = new GameObject("DirectionalLight_Main");
            Light light1 = mainLight.AddComponent<Light>();
            light1.type = LightType.Directional;
            light1.intensity = 1.0f;
            light1.color = Color.white;
            mainLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static void SaveScene(string scenePath)
        {
            // Asegurar que la carpeta existe
            string directory = System.IO.Path.GetDirectoryName(scenePath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            // Guardar la escena
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);
        }

        private static void AddSceneToBuildSettings(string scenePath)
        {
            // Obtener las escenas actuales del Build Settings
            var buildScenes = EditorBuildSettings.scenes.ToList();

            // Verificar si la escena ya está en el Build Settings
            bool sceneExists = buildScenes.Any(scene => scene.path == scenePath);

            if (!sceneExists)
            {
                // Agregar la nueva escena al Build Settings
                buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                EditorBuildSettings.scenes = buildScenes.ToArray();
                Debug.Log("✅ Escena agregada al Build Settings");
            }
        }
    }

    /// <summary>
    /// Controlador simple para la escena de configuración
    /// </summary>
    public class ConfigurationController : MonoBehaviour
    {
        private TMP_Dropdown scenarioDropdown;
        private Button loadPDFButton;
        private Button startButton;
        private TextMeshProUGUI pdfInfoText;
        private string selectedPDFPath = "";

        private void Start()
        {
            FindUIElements();
            SetupEventListeners();
            LoadSavedConfiguration();
        }

        private void FindUIElements()
        {
            scenarioDropdown = FindObjectOfType<TMP_Dropdown>();
            loadPDFButton = GameObject.Find("LoadPDFButton")?.GetComponent<Button>();
            startButton = GameObject.Find("StartButton")?.GetComponent<Button>();
            pdfInfoText = GameObject.Find("PDFInfo")?.GetComponent<TextMeshProUGUI>();
        }

        private void SetupEventListeners()
        {
            if (loadPDFButton != null)
                loadPDFButton.onClick.AddListener(OnLoadPDFClicked);

            if (startButton != null)
                startButton.onClick.AddListener(OnStartClicked);

            if (scenarioDropdown != null)
                scenarioDropdown.onValueChanged.AddListener(OnScenarioChanged);
        }

        private void LoadSavedConfiguration()
        {
            int savedScenario = PlayerPrefs.GetInt("SelectedScenario", 0);
            if (scenarioDropdown != null && savedScenario < scenarioDropdown.options.Count)
            {
                scenarioDropdown.value = savedScenario;
            }

            string savedPDFPath = PlayerPrefs.GetString("SelectedPDFPath", "");
            if (!string.IsNullOrEmpty(savedPDFPath))
            {
                selectedPDFPath = savedPDFPath;
                UpdatePDFInfo();
                EnableStartButton();
            }
        }

        private void OnLoadPDFClicked()
        {
#if UNITY_EDITOR
            string path = UnityEditor.EditorUtility.OpenFilePanel("Seleccionar PDF", "", "pdf");
            if (!string.IsNullOrEmpty(path))
            {
                selectedPDFPath = path;
                PlayerPrefs.SetString("SelectedPDFPath", path);
                UpdatePDFInfo();
                EnableStartButton();
            }
#endif
        }

        private void OnStartClicked()
        {
            // Guardar configuración
            PlayerPrefs.SetInt("SelectedScenario", scenarioDropdown.value);

            // Solo guardar PDF si existe uno seleccionado
            if (!string.IsNullOrEmpty(selectedPDFPath))
            {
                PlayerPrefs.SetString("SelectedPDFPath", selectedPDFPath);
                Debug.Log($"📄 PDF: {System.IO.Path.GetFileName(selectedPDFPath)}");
            }
            else
            {
                Debug.Log("🚀 Iniciando presentación sin PDF");
            }

            // Definir nombres de escenas
            string[] sceneNames = { "ClassroomScene", "AuditoriumScene", "ConferenceScene" };
            string sceneName = sceneNames[scenarioDropdown.value];

            Debug.Log($"🚀 Iniciando presentación en: {sceneName}");

            // Intentar cargar la escena usando SceneManager
            try
            {
                // Verificar si la escena existe en Build Settings
                bool sceneExists = false;
                for (
                    int i = 0;
                    i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
                    i++
                )
                {
                    string scenePath =
                        UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                    if (scenePath.Contains(sceneName))
                    {
                        sceneExists = true;
                        break;
                    }
                }

                if (sceneExists)
                {
                    // Usar SceneManager.LoadScene
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                    Debug.Log($"✅ Escena {sceneName} cargada exitosamente");
                }
                else
                {
                    Debug.LogError($"❌ La escena {sceneName} no está en el Build Settings");
                    Debug.LogError("💡 Ejecuta: VR Simulador → Generar y Configurar Todo");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Error al cargar la escena {sceneName}: {e.Message}");
            }
        }

        private void OnScenarioChanged(int value)
        {
            PlayerPrefs.SetInt("SelectedScenario", value);
        }

        private void UpdatePDFInfo()
        {
            if (pdfInfoText != null)
            {
                if (!string.IsNullOrEmpty(selectedPDFPath))
                {
                    string fileName = System.IO.Path.GetFileName(selectedPDFPath);
                    pdfInfoText.text = $"PDF seleccionado: {fileName}";
                    pdfInfoText.color = Color.green;
                }
                else
                {
                    pdfInfoText.text = "Ningún PDF seleccionado";
                    pdfInfoText.color = Color.gray;
                }
            }
        }

        private void EnableStartButton()
        {
            if (startButton != null)
            {
                startButton.interactable = !string.IsNullOrEmpty(selectedPDFPath);
            }
        }
    }
}
