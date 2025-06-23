# Sprint 6: Integración y Gestión de Escenas

## Objetivo

Implementar un sistema de gestión de escenas que permita transiciones fluidas entre las diferentes escenas del simulador VR, con persistencia de datos y loading screens.

## Componentes Implementados

### 1. SceneManager.cs

**Ubicación:** `Assets/VRTemplateAssets/Scripts/SceneManagement/SceneManager.cs`

**Funcionalidades:**

- Gestión de transiciones entre escenas
- Carga asíncrona de escenas con loading screen
- Persistencia de datos entre escenas (PDF path, selección de escenario)
- Configuración de escenas por defecto
- Sistema de loading con barra de progreso

**Características principales:**

```csharp
// Configuración de escenas
[SerializeField] private string configurationSceneName = "ConfigurationScene";
[SerializeField] private string classroomSceneName = "ClassroomScene";
[SerializeField] private string auditoriumSceneName = "AuditoriumScene";
[SerializeField] private string conferenceSceneName = "ConferenceScene";

// Configuración de transiciones
[SerializeField] private float transitionDuration = 1f;
[SerializeField] private bool useLoadingScreen = true;

// Persistencia de datos
[SerializeField] private bool persistPDFPath = true;
[SerializeField] private bool persistScenarioSelection = true;
```

**Métodos principales:**

- `LoadScene(string sceneName)`: Carga una escena con loading screen
- `LoadSceneAsync(string sceneName)`: Carga asíncrona con barra de progreso
- `SetPDFPath(string path)`: Guarda la ruta del PDF para persistencia
- `GetPDFPath()`: Recupera la ruta del PDF guardada
- `SetSelectedScenario(string scenario)`: Guarda el escenario seleccionado
- `GetSelectedScenario()`: Recupera el escenario guardado

### 2. SceneManagerSetupEditor.cs

**Ubicación:** `Assets/VRTemplateAssets/Scripts/SceneManagement/Editor/SceneManagerSetupEditor.cs`

**Funcionalidades:**

- Creación automática del prefab de SceneManager
- Configuración automática del SceneManager en todas las escenas
- Generación completa de todas las escenas del simulador
- Verificación de configuración de escenas

**Menús de Unity:**

- `VR Simulador/Configurar SceneManager`: Configura el SceneManager en todas las escenas
- `VR Simulador/Generar Todas las Escenas`: Genera todas las escenas y configura el SceneManager

## Flujo de Integración

### 1. Generación de Escenas

```csharp
// Generar todas las escenas automáticamente
ConfigurationSceneGeneratorEditor.GenerateConfigurationScene();
ClassroomSceneGeneratorEditor.GenerateClassroomScene();
AuditoriumSceneGeneratorEditor.GenerateAuditoriumScene();
ConferenceSceneGeneratorEditor.GenerateConferenceScene();
```

### 2. Configuración del SceneManager

```csharp
// Crear prefab del SceneManager
CreateSceneManagerPrefab();

// Configurar en todas las escenas
SetupAllScenes();
```

### 3. Transiciones entre Escenas

```csharp
// Desde la escena de configuración
SceneManager.Instance.LoadScene("ClassroomScene");

// Con persistencia de datos
SceneManager.Instance.SetPDFPath("ruta/al/archivo.pdf");
SceneManager.Instance.SetSelectedScenario("Classroom");
```

## Estructura de Datos Persistentes

### Datos Guardados

- **PDF Path**: Ruta del archivo PDF seleccionado
- **Selected Scenario**: Escenario seleccionado (Classroom, Auditorium, Conference)
- **User Preferences**: Configuraciones del usuario

### Persistencia

- Los datos se mantienen entre transiciones de escenas
- Se pueden configurar qué datos persistir desde el inspector
- Sistema flexible para agregar nuevos datos persistentes

## Loading Screen

### Características

- Barra de progreso visual
- Texto de progreso numérico
- Nombre de la escena que se está cargando
- Duración configurable de transición
- Opción de habilitar/deshabilitar

### Configuración

```csharp
[Header("UI de Loading")]
[SerializeField] private Canvas loadingCanvas;
[SerializeField] private Slider progressBar;
[SerializeField] private TextMeshProUGUI progressText;
[SerializeField] private TextMeshProUGUI sceneNameText;
```

## Configuración Automática

### Prefab del SceneManager

- Se crea automáticamente en `Assets/VRTemplateAssets/Prefabs/SceneManager.prefab`
- Configuración por defecto optimizada para VR
- Fácil personalización desde el inspector

### Integración en Escenas

- Se agrega automáticamente a todas las escenas generadas
- Verificación de duplicados
- Configuración consistente en todas las escenas

## Próximos Pasos

### Sprint 7: Sistema de Presentaciones

- Integración con PDFs
- Sistema de slides
- Controles de presentación
- Interacciones VR

### Sprint 8: Optimización y Testing

- Optimización de rendimiento
- Testing en dispositivos VR
- Corrección de bugs
- Documentación final

## Archivos Creados/Modificados

### Nuevos Archivos

- `Assets/VRTemplateAssets/Scripts/SceneManagement/SceneManager.cs`
- `Assets/VRTemplateAssets/Scripts/SceneManagement/Editor/SceneManagerSetupEditor.cs`

### Archivos Modificados

- Ninguno en este sprint

## Estado del Sprint

✅ **COMPLETADO**

### Funcionalidades Implementadas

- [x] SceneManager con gestión de transiciones
- [x] Carga asíncrona de escenas
- [x] Loading screen con barra de progreso
- [x] Persistencia de datos entre escenas
- [x] Configuración automática del SceneManager
- [x] Generación completa de todas las escenas
- [x] Menús de Unity para configuración

### Pruebas Realizadas

- [x] Compilación sin errores
- [x] Generación de prefab del SceneManager
- [x] Configuración automática en escenas
- [x] Integración con scripts de generación existentes

## Notas Técnicas

### Consideraciones de Rendimiento

- Carga asíncrona para evitar bloqueos
- Transiciones suaves entre escenas
- Optimización de memoria con persistencia selectiva

### Compatibilidad VR

- Configuración optimizada para dispositivos VR
- Transiciones que mantienen la inmersión
- Loading screens adaptadas a VR

### Escalabilidad

- Sistema modular para agregar nuevas escenas
- Configuración flexible de datos persistentes
- Fácil extensión para nuevas funcionalidades
