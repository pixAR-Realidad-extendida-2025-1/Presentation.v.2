using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRTemplate.Setup
{
    /// <summary>
    /// Configura automáticamente el sistema de input para VR y teclado/mouse
    /// </summary>
    public class InputSystemSetup : MonoBehaviour
    {
        [Header("Configuración de Input")]
        [SerializeField]
        private bool enableVRInput = true;

        [SerializeField]
        private bool enableKeyboardMouseInput = true;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject vrPlayerPrefab;

        [SerializeField]
        private GameObject keyboardMousePlayerPrefab;

        [Header("Configuración de Movimiento")]
        [SerializeField]
        private float keyboardMoveSpeed = 5f;

        [SerializeField]
        private float mouseLookSensitivity = 2f;

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
            string currentSceneName = UnityEngine
                .SceneManagement.SceneManager.GetActiveScene()
                .name;
            if (currentSceneName == "ConfigurationScene")
            {
                Debug.Log("ℹ️ Escena de configuración detectada - no se creará player");
                return;
            }

            // Detectar si estamos en VR
            isVRMode = XRSettings.isDeviceActive && XRSettings.enabled;

            if (isVRMode && enableVRInput)
            {
                SetupVRInput();
            }
            else if (enableKeyboardMouseInput)
            {
                SetupKeyboardMouseInput();
            }
            else
            {
                Debug.LogWarning("⚠️ No se pudo configurar ningún sistema de input");
            }
        }

        /// <summary>
        /// Configura el input para VR usando el Tutorial Player
        /// </summary>
        private void SetupVRInput()
        {
            Debug.Log("🎮 Configurando input VR...");

            // Cargar el prefab del Tutorial Player si no está asignado
            if (vrPlayerPrefab == null)
            {
                vrPlayerPrefab = Resources.Load<GameObject>(
                    "VRTemplateAssets/Prefabs/TutorialPlayer/Tutorial Player"
                );
                if (vrPlayerPrefab == null)
                {
                    Debug.LogError("❌ No se pudo cargar el prefab del Tutorial Player");
                    return;
                }
            }

            // Instanciar el player VR
            currentPlayer = Instantiate(vrPlayerPrefab, Vector3.zero, Quaternion.identity);
            currentPlayer.name = "VRPlayer";

            // Configurar spawn point si existe
            Transform spawnPoint = FindSpawnPoint();
            if (spawnPoint != null)
            {
                currentPlayer.transform.position = spawnPoint.position;
                currentPlayer.transform.rotation = spawnPoint.rotation;
            }

            Debug.Log("✅ Input VR configurado correctamente");
        }

        /// <summary>
        /// Configura el input para teclado y mouse
        /// </summary>
        private void SetupKeyboardMouseInput()
        {
            Debug.Log("⌨️ Configurando input teclado/mouse...");

            // Crear un player básico para teclado/mouse
            currentPlayer = CreateKeyboardMousePlayer();

            // Configurar spawn point si existe
            Transform spawnPoint = FindSpawnPoint();
            if (spawnPoint != null)
            {
                currentPlayer.transform.position = spawnPoint.position;
                currentPlayer.transform.rotation = spawnPoint.rotation;
            }

            Debug.Log("✅ Input teclado/mouse configurado correctamente");
        }

        /// <summary>
        /// Crea un player básico para teclado y mouse
        /// </summary>
        private GameObject CreateKeyboardMousePlayer()
        {
            GameObject player = new GameObject("KeyboardMousePlayer");

            // Agregar cámara
            Camera playerCamera = player.AddComponent<Camera>();
            playerCamera.tag = "MainCamera";
            playerCamera.clearFlags = CameraClearFlags.Skybox;
            playerCamera.fieldOfView = 60f;
            playerCamera.nearClipPlane = 0.1f;
            playerCamera.farClipPlane = 1000f;

            // Agregar componentes de movimiento
            KeyboardMouseController controller = player.AddComponent<KeyboardMouseController>();
            controller.moveSpeed = keyboardMoveSpeed;
            controller.lookSensitivity = mouseLookSensitivity;

            // Agregar collider para física
            CapsuleCollider collider = player.AddComponent<CapsuleCollider>();
            collider.height = 2f;
            collider.radius = 0.5f;
            collider.center = new Vector3(0, 1f, 0);

            // Agregar rigidbody
            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints =
                RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            return player;
        }

        /// <summary>
        /// Busca un spawn point en la escena
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
        /// Cambia entre modo VR y teclado/mouse
        /// </summary>
        public void ToggleInputMode()
        {
            if (currentPlayer != null)
            {
                Destroy(currentPlayer);
            }

            isVRMode = !isVRMode;
            SetupInputSystem();
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

    /// <summary>
    /// Controlador para movimiento con teclado y mouse
    /// </summary>
    public class KeyboardMouseController : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        public float moveSpeed = 5f;
        public float lookSensitivity = 2f;
        public float jumpForce = 5f;

        private Camera playerCamera;
        private Rigidbody rb;
        private float verticalRotation = 0f;
        private bool isGrounded = true;

        private void Start()
        {
            playerCamera = GetComponent<Camera>();
            rb = GetComponent<Rigidbody>();

            // Bloquear y ocultar el cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMouseLook();
            HandleKeyboardInput();
        }

        private void HandleMouseLook()
        {
            // Rotación horizontal (izquierda/derecha)
            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
            transform.Rotate(Vector3.up * mouseX);

            // Rotación vertical (arriba/abajo)
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }

        private void HandleKeyboardInput()
        {
            // Movimiento
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
            moveDirection.Normalize();

            // Aplicar movimiento
            Vector3 velocity = moveDirection * moveSpeed;
            velocity.y = rb.velocity.y; // Mantener velocidad vertical para gravedad
            rb.velocity = velocity;

            // Salto
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            // Desbloquear cursor con Escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            // Volver a bloquear cursor con clic izquierdo
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            isGrounded = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            isGrounded = false;
        }
    }
}
