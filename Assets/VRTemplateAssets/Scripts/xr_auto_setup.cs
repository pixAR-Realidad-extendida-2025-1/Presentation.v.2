using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Helper script para configurar automáticamente las referencias del XR Origin
/// Ejecuta este script una vez para auto-configurar todo
/// </summary>
[System.Serializable]
public class XRAutoSetupHelper : MonoBehaviour
{
    [Header("Auto-Setup Configuration")]
    [SerializeField]
    private bool autoConfigureOnStart = true;

    [SerializeField]
    private bool showDebugLogs = true;

    [Header("Input Action Asset")]
    [SerializeField]
    private InputActionAsset inputActionAsset;

    private XROriginCustomizer customizer;

    private void Start()
    {
        if (autoConfigureOnStart)
        {
            ConfigurarAutomaticamente();
        }
    }

    [ContextMenu("Configurar Automáticamente")]
    public void ConfigurarAutomaticamente()
    {
        customizer = GetComponent<XROriginCustomizer>();
        if (customizer == null)
        {
            LogError("No se encontró XROriginCustomizer en este GameObject");
            return;
        }

        // Buscar Input Action Asset si no está asignado
        if (inputActionAsset == null)
        {
            inputActionAsset = BuscarInputActionAsset();
        }

        if (inputActionAsset == null)
        {
            LogError("No se pudo encontrar el Input Action Asset");
            return;
        }

        // Configurar Input Action References
        ConfigurarInputActionReferences();

        // Auto-detectar controladores
        ConfigurarControladores();

        Log("✅ Configuración automática completada");
    }

    private InputActionAsset BuscarInputActionAsset()
    {
        // Buscar por nombre común
        var asset = Resources.Load<InputActionAsset>("XRI Default Input Actions");
        if (asset != null)
            return asset;

#if UNITY_EDITOR
        // Último recurso: buscar cualquier Input Action Asset en el proyecto (solo en editor)
        var guids = UnityEditor.AssetDatabase.FindAssets("t:InputActionAsset");
        foreach (var guid in guids)
        {
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            var foundAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<InputActionAsset>(path);
            if (foundAsset.name.Contains("XRI") || foundAsset.name.Contains("Default"))
            {
                return foundAsset;
            }
        }
#endif

        return null;
    }

    private void ConfigurarInputActionReferences()
    {
        // Mapeo de acciones comunes del XR Interaction Toolkit
        var mappings = new System.Collections.Generic.Dictionary<string, string>
        {
            { "botonASiguiente", "XRI RightHand/Primary Button" },
            { "botonXAnterior", "XRI RightHand/Secondary Button" },
            { "botonBExplorador", "XRI RightHand/Grip Button" },
            { "botonMenuCronometro", "XRI LeftHand/Menu Button" },
            { "gripResetCronometro", "XRI LeftHand/Grip Button" },
            { "teclaEnterExplorador", "XRI Head/Position" }, // Placeholder, necesitarás crear una acción de teclado
        };

        var customizerType = typeof(XROriginCustomizer);

        foreach (var mapping in mappings)
        {
            var field = customizerType.GetField(
                mapping.Key,
                System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.Instance
            );

            if (field != null)
            {
                var actionReference = BuscarInputActionReference(mapping.Value);
                if (actionReference != null)
                {
                    field.SetValue(customizer, actionReference);
                    Log($"✅ Configurado {mapping.Key} -> {mapping.Value}");
                }
                else
                {
                    LogWarning($"⚠️ No se encontró la acción: {mapping.Value}");
                }
            }
        }
    }

    private InputActionReference BuscarInputActionReference(string actionPath)
    {
        var pathParts = actionPath.Split('/');
        if (pathParts.Length != 2)
            return null;

        var actionMapName = pathParts[0];
        var actionName = pathParts[1];

        var actionMap = inputActionAsset.FindActionMap(actionMapName);
        if (actionMap == null)
            return null;

        var action = actionMap.FindAction(actionName);
        if (action == null)
            return null;

        return InputActionReference.Create(action);
    }

    private void ConfigurarControladores()
    {
        // Esta configuración ya debería estar en el prefab, pero por si acaso
        var leftController = transform
            .Find("Camera Offset/LeftHand Controller")
            ?.GetComponent<XRBaseController>();
        var rightController = transform
            .Find("Camera Offset/RightHand Controller")
            ?.GetComponent<XRBaseController>();

        if (leftController == null || rightController == null)
        {
            LogWarning(
                "⚠️ No se pudieron encontrar todos los controladores en la estructura esperada del prefab"
            );
            return;
        }

        Log("✅ Controladores detectados correctamente");
    }

    #region Logging Helpers
    private void Log(string message)
    {
        if (showDebugLogs)
            Debug.Log($"[XR Auto Setup] {message}");
    }

    private void LogWarning(string message)
    {
        if (showDebugLogs)
            Debug.LogWarning($"[XR Auto Setup] {message}");
    }

    private void LogError(string message)
    {
        Debug.LogError($"[XR Auto Setup] {message}");
    }
    #endregion

    #region Context Menu Actions
    [ContextMenu("Debug - Listar Todas las Acciones Disponibles")]
    private void ListarAccionesDisponibles()
    {
        if (inputActionAsset == null)
        {
            inputActionAsset = BuscarInputActionAsset();
        }

        if (inputActionAsset == null)
        {
            LogError("No se pudo encontrar Input Action Asset");
            return;
        }

        Log("=== ACCIONES DISPONIBLES ===");
        foreach (var actionMap in inputActionAsset.actionMaps)
        {
            Log($"Action Map: {actionMap.name}");
            foreach (var action in actionMap.actions)
            {
                Log($"  - {action.name}");
            }
        }
    }

    [ContextMenu("Debug - Verificar Estructura del Prefab")]
    private void VerificarEstructuraPrefab()
    {
        Log("=== VERIFICANDO ESTRUCTURA DEL PREFAB ===");

        // Verificar componentes principales
        var components = new string[]
        {
            "XROrigin",
            "InputActionManager",
            "XRInteractionManager",
            "TeleportationProvider",
        };

        foreach (var componentName in components)
        {
            var comp = GetComponent(componentName);
            if (comp != null)
                Log($"✅ {componentName} encontrado");
            else
                LogWarning($"⚠️ {componentName} no encontrado");
        }

        // Verificar jerarquía
        var expectedChildren = new string[]
        {
            "Camera Offset",
            "Camera Offset/Main Camera",
            "Camera Offset/LeftHand Controller",
            "Camera Offset/RightHand Controller",
        };

        foreach (var childPath in expectedChildren)
        {
            var child = transform.Find(childPath);
            if (child != null)
                Log($"✅ {childPath} encontrado");
            else
                LogWarning($"⚠️ {childPath} no encontrado");
        }
    }
    #endregion
}
