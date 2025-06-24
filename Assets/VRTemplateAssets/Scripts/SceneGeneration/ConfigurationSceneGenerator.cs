using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VRTemplate.SceneGeneration
{
    /// <summary>
    /// Generador de la escena de configuración (menú principal) del simulador VR de presentaciones
    /// Permite seleccionar escenario, cargar PDF e iniciar presentación
    /// </summary>
    public class ConfigurationSceneGenerator : MonoBehaviour
    {
        [Header("Configuración de UI")]
        [SerializeField]
        private Canvas mainCanvas;

        [SerializeField]
        private Transform uiContainer;

        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private GameObject dropdownPrefab;

        [SerializeField]
        private GameObject panelPrefab;

        [Header("Configuración de Escenas")]
        [SerializeField]
        private string[] sceneNames = { "ClassroomScene", "AuditoriumScene", "ConferenceScene" };

        [SerializeField]
        private string[] sceneDisplayNames =
        {
            "Sala de Clases",
            "Auditorio",
            "Sala de Conferencias",
        };

        [Header("Configuración de PDF")]
        [SerializeField]
        private string selectedPDFPath = "";

        [SerializeField]
        private TextMeshProUGUI pdfInfoText;

        [SerializeField]
        private Button loadPDFButton;

        [SerializeField]
        private Button startPresentationButton;

        [Header("Configuración de Navegación")]
        [SerializeField]
        private TMP_Dropdown scenarioDropdown;

        [SerializeField]
        private Button startButton;

        private void Start()
        {
            InitializeUI();
            SetupEventListeners();
            LoadSavedConfiguration();
        }

        /// <summary>
        /// Inicializa la interfaz de usuario del menú principal
        /// </summary>
        private void InitializeUI()
        {
            if (mainCanvas == null)
            {
                mainCanvas = FindObjectOfType<Canvas>();
                if (mainCanvas == null)
                {
                    CreateMainCanvas();
                }
            }

            if (uiContainer == null)
            {
                uiContainer = mainCanvas.transform;
            }

            CreateMainMenu();
        }

        /// <summary>
        /// Crea el canvas principal si no existe
        /// </summary>
        private void CreateMainCanvas()
        {
            GameObject canvasGO = new GameObject("MainCanvas");
            mainCanvas = canvasGO.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            mainCanvas.sortingOrder = 100;

            // Agregar CanvasScaler para UI responsiva
            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            // Agregar GraphicRaycaster para interacciones
            canvasGO.AddComponent<GraphicRaycaster>();

            uiContainer = canvasGO.transform;
        }

        /// <summary>
        /// Crea el menú principal con todos los elementos UI
        /// </summary>
        private void CreateMainMenu()
        {
            // Panel principal
            GameObject mainPanel = CreatePanel("MainPanel", new Vector2(800, 600));
            mainPanel.transform.SetParent(uiContainer, false);

            // Título
            CreateTitle(mainPanel.transform);

            // Dropdown de selección de escenario
            CreateScenarioDropdown(mainPanel.transform);

            // Botón de carga de PDF
            CreateLoadPDFButton(mainPanel.transform);

            // Información del PDF
            CreatePDFInfo(mainPanel.transform);

            // Botón de iniciar presentación
            CreateStartButton(mainPanel.transform);
        }

        /// <summary>
        /// Crea el título del menú
        /// </summary>
        private void CreateTitle(Transform parent)
        {
            GameObject titleGO = new GameObject("Title");
            titleGO.transform.SetParent(parent, false);

            TextMeshProUGUI titleText = titleGO.AddComponent<TextMeshProUGUI>();
            titleText.text = "Simulador VR de Presentaciones";
            titleText.fontSize = 36;
            titleText.fontStyle = FontStyles.Bold;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = Color.white;

            RectTransform titleRect = titleGO.GetComponent<RectTransform>();
            titleRect.anchoredPosition = new Vector2(0, 200);
            titleRect.sizeDelta = new Vector2(600, 60);
        }

        /// <summary>
        /// Crea el dropdown para seleccionar escenario
        /// </summary>
        private void CreateScenarioDropdown(Transform parent)
        {
            GameObject dropdownGO = new GameObject("ScenarioDropdown");
            dropdownGO.transform.SetParent(parent, false);

            TMP_Dropdown dropdown = dropdownGO.AddComponent<TMP_Dropdown>();
            scenarioDropdown = dropdown;

            // Configurar opciones
            dropdown.options.Clear();
            for (int i = 0; i < sceneDisplayNames.Length; i++)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(sceneDisplayNames[i]));
            }
            dropdown.value = 0;

            // Configurar RectTransform
            RectTransform dropdownRect = dropdownGO.GetComponent<RectTransform>();
            dropdownRect.anchoredPosition = new Vector2(0, 100);
            dropdownRect.sizeDelta = new Vector2(400, 50);

            // Crear label
            CreateLabel(parent, "Seleccionar Escenario:", new Vector2(0, 150));
        }

        /// <summary>
        /// Crea el botón para cargar PDF
        /// </summary>
        private void CreateLoadPDFButton(Transform parent)
        {
            GameObject buttonGO = new GameObject("LoadPDFButton");
            buttonGO.transform.SetParent(parent, false);

            Button button = buttonGO.AddComponent<Button>();
            loadPDFButton = button;

            // Configurar imagen del botón
            Image buttonImage = buttonGO.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.6f, 1f, 1f);

            // Configurar texto del botón
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform, false);

            TextMeshProUGUI buttonText = textGO.AddComponent<TextMeshProUGUI>();
            buttonText.text = "Cargar PDF";
            buttonText.fontSize = 18;
            buttonText.color = Color.white;
            buttonText.alignment = TextAlignmentOptions.Center;

            RectTransform textRect = textGO.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            // Configurar RectTransform del botón
            RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(0, 0);
            buttonRect.sizeDelta = new Vector2(200, 50);

            // Crear label
            CreateLabel(parent, "Archivo de Presentación:", new Vector2(0, 50));
        }

        /// <summary>
        /// Crea el área de información del PDF
        /// </summary>
        private void CreatePDFInfo(Transform parent)
        {
            GameObject infoGO = new GameObject("PDFInfo");
            infoGO.transform.SetParent(parent, false);

            TextMeshProUGUI infoText = infoGO.AddComponent<TextMeshProUGUI>();
            pdfInfoText = infoText;
            infoText.text = "Ningún PDF seleccionado";
            infoText.fontSize = 14;
            infoText.color = Color.gray;
            infoText.alignment = TextAlignmentOptions.Center;

            RectTransform infoRect = infoGO.GetComponent<RectTransform>();
            infoRect.anchoredPosition = new Vector2(0, -50);
            infoRect.sizeDelta = new Vector2(500, 40);
        }

        /// <summary>
        /// Crea el botón de iniciar presentación
        /// </summary>
        private void CreateStartButton(Transform parent)
        {
            GameObject buttonGO = new GameObject("StartButton");
            buttonGO.transform.SetParent(parent, false);

            Button button = buttonGO.AddComponent<Button>();
            startPresentationButton = button;
            startButton = button;

            // Configurar imagen del botón
            Image buttonImage = buttonGO.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.8f, 0.2f, 1f);

            // Configurar texto del botón
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform, false);

            TextMeshProUGUI buttonText = textGO.AddComponent<TextMeshProUGUI>();
            buttonText.text = "Iniciar Presentación";
            buttonText.fontSize = 20;
            buttonText.fontStyle = FontStyles.Bold;
            buttonText.color = Color.white;
            buttonText.alignment = TextAlignmentOptions.Center;

            RectTransform textRect = textGO.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            // Configurar RectTransform del botón
            RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(0, -150);
            buttonRect.sizeDelta = new Vector2(250, 60);

            // Inicialmente deshabilitado
            button.interactable = true;
        }

        /// <summary>
        /// Crea un label de texto
        /// </summary>
        private void CreateLabel(Transform parent, string text, Vector2 position)
        {
            GameObject labelGO = new GameObject("Label");
            labelGO.transform.SetParent(parent, false);

            TextMeshProUGUI labelText = labelGO.AddComponent<TextMeshProUGUI>();
            labelText.text = text;
            labelText.fontSize = 16;
            labelText.color = Color.white;
            labelText.alignment = TextAlignmentOptions.Center;

            RectTransform labelRect = labelGO.GetComponent<RectTransform>();
            labelRect.anchoredPosition = position;
            labelRect.sizeDelta = new Vector2(400, 30);
        }

        /// <summary>
        /// Crea un panel UI
        /// </summary>
        private GameObject CreatePanel(string name, Vector2 size)
        {
            GameObject panel = new GameObject(name);
            Image panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.sizeDelta = size;

            return panel;
        }

        /// <summary>
        /// Configura los event listeners para los botones y dropdowns
        /// </summary>
        private void SetupEventListeners()
        {
            if (loadPDFButton != null)
            {
                loadPDFButton.onClick.AddListener(OnLoadPDFClicked);
            }

            if (startPresentationButton != null)
            {
                startPresentationButton.onClick.AddListener(OnStartPresentationClicked);
            }

            if (scenarioDropdown != null)
            {
                scenarioDropdown.onValueChanged.AddListener(OnScenarioChanged);
            }
        }

        /// <summary>
        /// Carga la configuración guardada
        /// </summary>
        private void LoadSavedConfiguration()
        {
            // Cargar escenario seleccionado
            int savedScenario = PlayerPrefs.GetInt("SelectedScenario", 0);
            if (scenarioDropdown != null && savedScenario < scenarioDropdown.options.Count)
            {
                scenarioDropdown.value = savedScenario;
            }

            // Cargar PDF seleccionado
            string savedPDFPath = PlayerPrefs.GetString("SelectedPDFPath", "");
            if (!string.IsNullOrEmpty(savedPDFPath) && File.Exists(savedPDFPath))
            {
                selectedPDFPath = savedPDFPath;
                UpdatePDFInfo();
                EnableStartButton();
            }
        }

        /// <summary>
        /// Maneja el clic en el botón de cargar PDF
        /// </summary>
        private void OnLoadPDFClicked()
        {
#if UNITY_EDITOR
            string path = UnityEditor.EditorUtility.OpenFilePanel("Seleccionar PDF", "", "pdf");
#else
            // En build, usar un sistema de file picker nativo o web
            string path = "";
#endif

            if (!string.IsNullOrEmpty(path))
            {
                selectedPDFPath = path;
                PlayerPrefs.SetString("SelectedPDFPath", path);
                UpdatePDFInfo();
                EnableStartButton();
            }
        }

        /// <summary>
        /// Maneja el clic en el botón de iniciar presentación
        /// </summary>
        private void OnStartPresentationClicked()
        {
            // Guardar configuración
            PlayerPrefs.SetInt("SelectedScenario", scenarioDropdown.value);

            // Solo guardar PDF si existe uno seleccionado
            if (!string.IsNullOrEmpty(selectedPDFPath))
            {
                PlayerPrefs.SetString("SelectedPDFPath", selectedPDFPath);
            }

            // Cargar la escena seleccionada
            string sceneName = sceneNames[scenarioDropdown.value];
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Maneja el cambio de escenario
        /// </summary>
        private void OnScenarioChanged(int value)
        {
            PlayerPrefs.SetInt("SelectedScenario", value);
        }

        /// <summary>
        /// Actualiza la información del PDF en la UI
        /// </summary>
        private void UpdatePDFInfo()
        {
            if (pdfInfoText != null)
            {
                if (!string.IsNullOrEmpty(selectedPDFPath))
                {
                    string fileName = Path.GetFileName(selectedPDFPath);
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

        /// <summary>
        /// Habilita el botón de iniciar presentación
        /// </summary>
        private void EnableStartButton()
        {
            if (startButton != null)
            {
                startButton.interactable = true;
            }
        }

        /// <summary>
        /// Método público para generar la escena programáticamente
        /// </summary>
        [ContextMenu("Generar Escena de Configuración")]
        public void GenerateConfigurationScene()
        {
            Debug.Log("🎮 Generando escena de configuración...");
            InitializeUI();
            SetupEventListeners();
            LoadSavedConfiguration();
            Debug.Log("✅ Escena de configuración generada exitosamente");
        }
    }
}
