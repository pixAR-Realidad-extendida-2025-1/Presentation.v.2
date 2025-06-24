using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Diagnostics;

/// <summary>
/// Script para personalizar los controles del Complete XR Origin Set Up Variant
/// Agrega este script al XR Origin y configura las acciones personalizadas
/// </summary>
public class XROriginCustomizer : MonoBehaviour
{
    [Header("Referencias del Prefab")]
    [SerializeField] private XRBaseController leftController;
    [SerializeField] private XRBaseController rightController;
    
    [Header("Configuración de Diapositivas")]
    public GameObject[] diapositivas;
    private int diapositivaActual = 0;
    
    [Header("Configuración del Cronómetro")]
    public UnityEngine.UI.Text tiempoTexto; // Opcional: para mostrar el tiempo
    private System.Diagnostics.Stopwatch cronometro;
    private bool cronometroActivo = false;
    
    [Header("Input Actions Personalizadas")]
    [Tooltip("Botón A del controlador derecho - Siguiente diapositiva")]
    public InputActionReference botonASiguiente;
    
    [Tooltip("Botón X del controlador derecho - Diapositiva anterior")]
    public InputActionReference botonXAnterior;
    
    [Tooltip("Botón B del controlador derecho - Abrir explorador")]
    public InputActionReference botonBExplorador;
    
    [Tooltip("Botón Menu/Start - Alternar cronómetro")]
    public InputActionReference botonMenuCronometro;
    
    [Tooltip("Grip del controlador izquierdo - Reset cronómetro")]
    public InputActionReference gripResetCronometro;
    
    [Tooltip("Tecla Enter del teclado - Abrir explorador")]
    public InputActionReference teclaEnterExplorador;
    
    [Header("Configuración Avanzada")]
    [Tooltip("Desactivar teleport/ray interactor mientras usas controles personalizados")]
    public bool desactivarTeleportTemporalmente = true;
    
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractorDerecho;
    private UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportProvider;
    
    private void Awake()
    {
        // Auto-detectar componentes del prefab si no están asignados
        if (leftController == null)
            leftController = transform.Find("Camera Offset/LeftHand Controller")?.GetComponent<XRBaseController>();
            
        if (rightController == null)
            rightController = transform.Find("Camera Offset/RightHand Controller")?.GetComponent<XRBaseController>();
            
        // Encontrar ray interactor y teleport provider
        rayInteractorDerecho = rightController?.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        teleportProvider = GetComponent<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();
    }
    
    private void Start()
    {
        // Inicializar cronómetro
        cronometro = new System.Diagnostics.Stopwatch();
        
        // Configurar diapositivas
        ConfigurarDiapositivas();
        
        // Habilitar y configurar input actions
        ConfigurarInputActions();
        
        UnityEngine.Debug.Log("XR Origin personalizado configurado correctamente");
    }
    
    private void ConfigurarInputActions()
    {
        // Habilitar todas las acciones
        HabilitarInputAction(botonASiguiente, OnBotonASiguiente);
        HabilitarInputAction(botonXAnterior, OnBotonXAnterior);
        HabilitarInputAction(botonBExplorador, OnBotonBExplorador);
        HabilitarInputAction(botonMenuCronometro, OnBotonMenuCronometro);
        HabilitarInputAction(gripResetCronometro, OnGripResetCronometro);
        HabilitarInputAction(teclaEnterExplorador, OnTeclaEnterExplorador);
    }
    
    private void HabilitarInputAction(InputActionReference actionRef, System.Action<InputAction.CallbackContext> callback)
    {
        if (actionRef?.action != null)
        {
            actionRef.action.Enable();
            actionRef.action.performed += callback;
        }
    }
    
    #region Callbacks de Input Actions
    private void OnBotonASiguiente(InputAction.CallbackContext context)
    {
        SiguienteDiapositiva();
        DesactivarTeleportTemporalmente();
    }
    
    private void OnBotonXAnterior(InputAction.CallbackContext context)
    {
        DiapositivaAnterior();
        DesactivarTeleportTemporalmente();
    }
    
    private void OnBotonBExplorador(InputAction.CallbackContext context)
    {
        AbrirExploradorArchivos();
        DesactivarTeleportTemporalmente();
    }
    
    private void OnBotonMenuCronometro(InputAction.CallbackContext context)
    {
        AlternarCronometro();
    }
    
    private void OnGripResetCronometro(InputAction.CallbackContext context)
    {
        ReiniciarCronometro();
    }
    
    private void OnTeclaEnterExplorador(InputAction.CallbackContext context)
    {
        AbrirExploradorArchivos();
    }
    #endregion
    
    #region Funciones de Control Temporal del Teleport
    private void DesactivarTeleportTemporalmente()
    {
        if (!desactivarTeleportTemporalmente) return;
        
        if (rayInteractorDerecho != null)
        {
            rayInteractorDerecho.enabled = false;
            // Reactivar después de un breve momento
            Invoke(nameof(ReactivarTeleport), 0.5f);
        }
    }
    
    private void ReactivarTeleport()
    {
        if (rayInteractorDerecho != null)
        {
            rayInteractorDerecho.enabled = true;
        }
    }
    #endregion
    
    #region Funciones de Diapositivas
    private void ConfigurarDiapositivas()
    {
        if (diapositivas.Length == 0) return;
        
        for (int i = 0; i < diapositivas.Length; i++)
        {
            diapositivas[i].SetActive(i == 0);
        }
        diapositivaActual = 0;
        
        UnityEngine.Debug.Log($"Configuradas {diapositivas.Length} diapositivas");
    }
    
    public void SiguienteDiapositiva()
    {
        if (diapositivas.Length == 0) return;
        
        diapositivas[diapositivaActual].SetActive(false);
        diapositivaActual = (diapositivaActual + 1) % diapositivas.Length;
        diapositivas[diapositivaActual].SetActive(true);
        
        UnityEngine.Debug.Log($"Diapositiva: {diapositivaActual + 1}/{diapositivas.Length}");
        
        // Feedback háptico opcional
        DarFeedbackHaptico(rightController, 0.1f, 0.3f);
    }
    
    public void DiapositivaAnterior()
    {
        if (diapositivas.Length == 0) return;
        
        diapositivas[diapositivaActual].SetActive(false);
        diapositivaActual = (diapositivaActual - 1 + diapositivas.Length) % diapositivas.Length;
        diapositivas[diapositivaActual].SetActive(true);
        
        UnityEngine.Debug.Log($"Diapositiva: {diapositivaActual + 1}/{diapositivas.Length}");
        
        // Feedback háptico opcional
        DarFeedbackHaptico(rightController, 0.1f, 0.2f);
    }
    #endregion
    
    #region Funciones del Cronómetro
    public void AlternarCronometro()
    {
        if (cronometroActivo)
        {
            PausarCronometro();
        }
        else
        {
            IniciarCronometro();
        }
        
        // Feedback háptico
        DarFeedbackHaptico(leftController, 0.15f, 0.4f);
    }
    
    public void IniciarCronometro()
    {
        cronometro.Start();
        cronometroActivo = true;
        UnityEngine.Debug.Log("⏰ Cronómetro iniciado");
    }
    
    public void PausarCronometro()
    {
        cronometro.Stop();
        cronometroActivo = false;
        UnityEngine.Debug.Log("⏸️ Cronómetro pausado");
    }
    
    public void ReiniciarCronometro()
    {
        cronometro.Reset();
        cronometroActivo = false;
        UnityEngine.Debug.Log("🔄 Cronómetro reiniciado");
        
        // Feedback háptico más fuerte para reset
        DarFeedbackHaptico(leftController, 0.2f, 0.6f);
    }
    #endregion
    
    #region Funciones Auxiliares
    public void AbrirExploradorArchivos()
    {
        try
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            Process.Start("explorer.exe");
#elif UNITY_STANDALONE_OSX
            Process.Start("open", ".");
#elif UNITY_STANDALONE_LINUX
            Process.Start("xdg-open", ".");
#endif
            UnityEngine.Debug.Log("📁 Explorador de archivos abierto");
            
            // Feedback háptico
            DarFeedbackHaptico(rightController, 0.2f, 0.5f);
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError($"❌ Error al abrir explorador: {e.Message}");
        }
    }
    
    private void DarFeedbackHaptico(XRBaseController controller, float duracion, float intensidad)
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(intensidad, duracion);
        }
    }
    #endregion
    
    private void Update()
    {
        // Actualizar display del cronómetro
        if (tiempoTexto != null && cronometro != null)
        {
            var tiempo = cronometro.Elapsed;
            tiempoTexto.text = $"{tiempo.Minutes:00}:{tiempo.Seconds:00}:{tiempo.Milliseconds / 10:00}";
        }
    }
    
    private void OnDestroy()
    {
        // Cleanup de input actions
        DeshabilitarInputAction(botonASiguiente, OnBotonASiguiente);
        DeshabilitarInputAction(botonXAnterior, OnBotonXAnterior);
        DeshabilitarInputAction(botonBExplorador, OnBotonBExplorador);
        DeshabilitarInputAction(botonMenuCronometro, OnBotonMenuCronometro);
        DeshabilitarInputAction(gripResetCronometro, OnGripResetCronometro);
        DeshabilitarInputAction(teclaEnterExplorador, OnTeclaEnterExplorador);
    }
    
    private void DeshabilitarInputAction(InputActionReference actionRef, System.Action<InputAction.CallbackContext> callback)
    {
        if (actionRef?.action != null)
        {
            actionRef.action.performed -= callback;
            actionRef.action.Disable();
        }
    }
    
    #region Métodos de Debug
    [ContextMenu("Debug - Listar Input Actions Disponibles")]
    private void DebugListarInputActions()
    {
        if (leftController != null)
        {
            var leftInputs = leftController.GetComponents<MonoBehaviour>();
            UnityEngine.Debug.Log("=== LEFT CONTROLLER INPUTS ===");
            foreach (var input in leftInputs)
            {
                UnityEngine.Debug.Log($"- {input.GetType().Name}");
            }
        }
        
        if (rightController != null)
        {
            var rightInputs = rightController.GetComponents<MonoBehaviour>();
            UnityEngine.Debug.Log("=== RIGHT CONTROLLER INPUTS ===");
            foreach (var input in rightInputs)
            {
                UnityEngine.Debug.Log($"- {input.GetType().Name}");
            }
        }
    }
    
    [ContextMenu("Debug - Test Siguiente Diapositiva")]
    private void DebugSiguienteDiapositiva()
    {
        SiguienteDiapositiva();
    }
    
    [ContextMenu("Debug - Test Cronómetro")]
    private void DebugCronometro()
    {
        AlternarCronometro();
    }
    #endregion
}