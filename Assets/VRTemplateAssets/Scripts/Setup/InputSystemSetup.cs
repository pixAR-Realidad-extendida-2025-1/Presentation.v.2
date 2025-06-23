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

        private Camera playerCamera;
        private CharacterController characterController;

        // Input System
        private UnityEngine.InputSystem.PlayerInput playerInput;
        private UnityEngine.InputSystem.InputAction moveAction;
        private UnityEngine.InputSystem.InputAction lookAction;
        private UnityEngine.InputSystem.InputAction jumpAction;

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
        /// Configura el input para teclado y mouse usando el prefab VR
        /// </summary>
        private void SetupKeyboardMouseInput()
        {
            Debug.Log("⌨️ Configurando input teclado/mouse...");

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

                    // Configurar para modo desktop
                    ConfigureForDesktopMode(currentPlayer);

                    Debug.Log("✅ Player VR configurado para modo desktop");
                }
                else
                {
                    Debug.LogError("❌ No se pudo instanciar el prefab VR");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ No se encontró el prefab VR, creando player básico");
                CreateBasicKeyboardMousePlayer(spawnPoint);
            }
        }

        /// <summary>
        /// Configura el player VR para modo desktop
        /// </summary>
        private void ConfigureForDesktopMode(GameObject player)
        {
            // Buscar la cámara principal
            var camera = player.GetComponentInChildren<Camera>();
            if (camera != null)
            {
                camera.tag = "MainCamera";

                // Ajustar posición de la cámara para desktop
                var cameraTransform = camera.transform;
                if (cameraTransform.parent != null)
                {
                    // Ajustar altura para desktop
                    Vector3 localPos = cameraTransform.parent.localPosition;
                    localPos.y = 1.7f;
                    cameraTransform.parent.localPosition = localPos;
                }
            }

            // Agregar controlador de teclado/mouse si no existe
            var keyboardController = player.GetComponentInChildren<KeyboardMouseController>();
            if (keyboardController == null && camera != null)
            {
                keyboardController = camera.gameObject.AddComponent<KeyboardMouseController>();
                keyboardController.moveSpeed = keyboardMoveSpeed;
                keyboardController.lookSensitivity = mouseLookSensitivity;
            }

            Debug.Log("✅ Player VR configurado para modo desktop");
        }

        /// <summary>
        /// Crea un player básico como fallback
        /// </summary>
        private void CreateBasicKeyboardMousePlayer(Transform spawnPoint)
        {
            GameObject player = new GameObject("KeyboardMousePlayer");

            // Agregar cámara
            Camera playerCamera = player.AddComponent<Camera>();
            playerCamera.tag = "MainCamera";
            playerCamera.clearFlags = CameraClearFlags.Skybox;
            playerCamera.fieldOfView = 60f;
            playerCamera.nearClipPlane = 0.1f;
            playerCamera.farClipPlane = 1000f;

            // Agregar CharacterController para mejor control de colisiones
            CharacterController characterController = player.AddComponent<CharacterController>();
            characterController.height = 2f;
            characterController.radius = 0.5f;
            characterController.center = new Vector3(0, 1f, 0);
            characterController.slopeLimit = 45f;
            characterController.stepOffset = 0.3f;

            // Agregar componentes de movimiento
            KeyboardMouseController controller = player.AddComponent<KeyboardMouseController>();
            controller.moveSpeed = keyboardMoveSpeed;
            controller.lookSensitivity = mouseLookSensitivity;

            // Posicionar en spawn point
            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.position;
                player.transform.rotation = spawnPoint.rotation;
            }
            else
            {
                player.transform.position = new Vector3(0, 1.7f, 0);
            }

            currentPlayer = player;
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
        private CharacterController characterController;

        // Input System
        private UnityEngine.InputSystem.PlayerInput playerInput;
        private UnityEngine.InputSystem.InputAction moveAction;
        private UnityEngine.InputSystem.InputAction lookAction;
        private UnityEngine.InputSystem.InputAction jumpAction;

        private void Start()
        {
            playerCamera = GetComponent<Camera>();
            characterController = GetComponent<CharacterController>();

            // Configurar Input System
            SetupInputActions();

            // Bloquear y ocultar el cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SetupInputActions()
        {
            // Crear acciones de input manualmente
            moveAction = new UnityEngine.InputSystem.InputAction(
                "Move",
                UnityEngine.InputSystem.InputActionType.Value,
                "<Keyboard>/wasd"
            );
            lookAction = new UnityEngine.InputSystem.InputAction(
                "Look",
                UnityEngine.InputSystem.InputActionType.Value,
                "<Mouse>/delta"
            );
            jumpAction = new UnityEngine.InputSystem.InputAction(
                "Jump",
                UnityEngine.InputSystem.InputActionType.Button,
                "<Keyboard>/space"
            );

            // Habilitar las acciones
            moveAction.Enable();
            lookAction.Enable();
            jumpAction.Enable();
        }

        private void Update()
        {
            HandleMouseLook();
            HandleKeyboardInput();
        }

        private void HandleMouseLook()
        {
            // Rotación horizontal (izquierda/derecha) - rotar todo el player
            Vector2 lookDelta = lookAction.ReadValue<Vector2>();
            float mouseX = lookDelta.x * lookSensitivity;
            transform.Rotate(Vector3.up * mouseX);

            // Rotación vertical (arriba/abajo) - solo la cámara
            float mouseY = lookDelta.y * lookSensitivity;
            if (playerCamera != null)
            {
                Vector3 currentRotation = playerCamera.transform.localEulerAngles;
                float newXRotation = currentRotation.x - mouseY;

                // Limitar rotación vertical entre -90 y 90 grados
                if (newXRotation > 180f)
                    newXRotation -= 360f;
                newXRotation = Mathf.Clamp(newXRotation, -90f, 90f);

                playerCamera.transform.localRotation = Quaternion.Euler(newXRotation, 0f, 0f);
            }
        }

        private void HandleKeyboardInput()
        {
            // Movimiento usando Input System
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            float moveX = moveInput.x;
            float moveZ = moveInput.y;

            // Calcular dirección de movimiento relativa a la rotación del player
            Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
            moveDirection.Normalize();

            // Aplicar movimiento horizontal
            Vector3 horizontalVelocity =
                new Vector3(moveDirection.x, 0, moveDirection.z) * moveSpeed;

            // Aplicar gravedad vertical
            Vector3 verticalVelocity = Vector3.zero;
            if (!characterController.isGrounded)
            {
                verticalVelocity.y = Physics.gravity.y * Time.deltaTime;
            }

            // Combinar velocidades y aplicar movimiento
            Vector3 totalVelocity = horizontalVelocity + verticalVelocity;
            characterController.Move(totalVelocity * Time.deltaTime);

            // Salto
            if (jumpAction.WasPressedThisFrame() && characterController.isGrounded)
            {
                Vector3 jumpVelocity =
                    Vector3.up * Mathf.Sqrt(2 * jumpForce * Mathf.Abs(Physics.gravity.y));
                characterController.Move(jumpVelocity * Time.deltaTime);
            }

            // Desbloquear cursor con Escape
            if (
                UnityEngine.InputSystem.Keyboard.current != null
                && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame
            )
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            // Volver a bloquear cursor con clic izquierdo
            if (
                UnityEngine.InputSystem.Mouse.current != null
                && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame
            )
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OnDestroy()
        {
            // Limpiar acciones de input
            if (moveAction != null)
                moveAction.Disable();
            if (lookAction != null)
                lookAction.Disable();
            if (jumpAction != null)
                jumpAction.Disable();
        }
    }
}
