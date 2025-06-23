# Escena de Configuración (Menú Principal) - Sprint 2 Completado

## Resumen

El Sprint 2 del plan de desarrollo se ha completado exitosamente. Se ha implementado un sistema de generación automática de la escena de configuración (menú principal) que permite a los usuarios seleccionar escenarios, cargar PDFs e iniciar presentaciones.

## Archivos Implementados

### Script de Generación de Escena

- **Ubicación**: `Assets/VRTemplateAssets/Scripts/SceneGeneration/ConfigurationSceneGenerator.cs`
- **Tipo**: Script de Runtime (funciona en tiempo de ejecución)
- **Funcionalidad**: Genera automáticamente la UI del menú principal

### Script de Editor para Generación Automática

- **Ubicación**: `Assets/VRTemplateAssets/Scripts/SceneGeneration/Editor/ConfigurationSceneGeneratorEditor.cs`
- **Tipo**: Script de Editor (solo funciona en Unity Editor)
- **Funcionalidad**: Genera automáticamente la escena completa desde el menú de Unity

## Cómo Usar la Generación Automática

### 1. Acceder al Menú de Generación

En Unity, ve al menú superior:

```
VR Simulador → Generar Escena de Configuración
```

### 2. Confirmar la Generación

Se abrirá un diálogo de confirmación:

- **Mensaje**: "¿Deseas generar automáticamente la escena del menú principal?"
- **Opciones**: "Sí" / "No"

### 3. Resultado de la Generación

Al aceptar, se ejecutarán automáticamente:

- Creación de nueva escena
- Configuración de cámara principal
- Configuración de iluminación básica
- Generación de UI completa
- Guardado de escena en `Assets/Scenes/ConfigurationScene.unity`
- Adición automática al Build Settings

### 4. Confirmación de Completado

Se mostrará un mensaje con la ubicación de la escena generada.

## Elementos de la UI Generada

### Panel Principal

- **Dimensiones**: 800x600 píxeles
- **Color de fondo**: Gris oscuro semi-transparente
- **Posición**: Centrado en pantalla

### Título

- **Texto**: "Simulador VR de Presentaciones"
- **Tamaño de fuente**: 36px
- **Estilo**: Negrita
- **Color**: Blanco
- **Posición**: Parte superior del panel

### Dropdown de Selección de Escenario

- **Opciones**:
  - "Sala de Clases"
  - "Auditorio"
  - "Sala de Conferencias"
- **Tamaño**: 400x50 píxeles
- **Posición**: Centro del panel
- **Funcionalidad**: Guarda la selección en PlayerPrefs

### Botón de Carga de PDF

- **Texto**: "Cargar PDF"
- **Color**: Azul (#3366FF)
- **Tamaño**: 200x50 píxeles
- **Funcionalidad**: Abre file picker para seleccionar archivo PDF
- **Compatibilidad**: Funciona en Editor y Build (con implementación nativa)

### Información del PDF

- **Estado inicial**: "Ningún PDF seleccionado" (gris)
- **Estado con PDF**: "PDF seleccionado: [nombre]" (verde)
- **Posición**: Debajo del botón de carga

### Botón de Iniciar Presentación

- **Texto**: "Iniciar Presentación"
- **Color**: Verde (#33CC33)
- **Tamaño**: 250x60 píxeles
- **Estado inicial**: Deshabilitado
- **Habilitación**: Solo cuando se ha seleccionado un PDF
- **Funcionalidad**: Carga la escena seleccionada

## Funcionalidades Implementadas

### Persistencia de Datos

- **Escenario seleccionado**: Guardado en PlayerPrefs
- **PDF seleccionado**: Guardado en PlayerPrefs
- **Carga automática**: Al iniciar la escena, se restauran las selecciones previas

### Validación de Archivos

- **Verificación de existencia**: Comprueba que el PDF seleccionado existe
- **Restauración automática**: Si el PDF ya no existe, se limpia la selección

### Navegación entre Escenas

- **Carga de escenas**: Usa SceneManager.LoadScene()
- **Nombres de escenas**: Configurables en el inspector
- **Transición**: Directa a la escena seleccionada

### UI Responsiva

- **Canvas Scaler**: Configurado para diferentes resoluciones
- **Referencia**: 1920x1080
- **Escalado**: Adaptativo según el tamaño de pantalla

## Configuración Técnica

### Canvas Principal

- **Render Mode**: Screen Space Overlay
- **Sorting Order**: 100
- **Canvas Scaler**: Scale With Screen Size
- **Reference Resolution**: 1920x1080
- **Screen Match Mode**: Match Width Or Height (0.5)

### Cámara Principal

- **Clear Flags**: Solid Color
- **Background Color**: Gris oscuro (#1A1A1A)
- **Projection**: Orthographic
- **Position**: (0, 0, -10)
- **Orthographic Size**: 5

### Iluminación

- **Luz Direccional**: Intensidad 1.0, color blanco
- **Rotación**: (50°, -30°, 0°)
- **Ambient Mode**: Trilight
- **Colores ambientales**: Configurados para ambiente neutro

## Estructura de Archivos Generada

```
Assets/
└── Scenes/
    └── ConfigurationScene.unity
```

## Integración con Build Settings

La escena generada se agrega automáticamente al Build Settings de Unity, lo que permite:

- Compilación directa del proyecto
- Inclusión en builds finales
- Configuración de orden de escenas

## Próximos Pasos

Con el Sprint 2 completado, el proyecto está listo para:

- ✅ Configuración automática de Unity para VR
- ✅ Generación automática de escena de configuración
- 🔄 Sprint 3: Generación de escenas (Classroom Scene)
- 🔄 Sprint 4: Generación de escenas (Auditorium Scene)
- 🔄 Sprint 5: Generación de escenas (Conference Scene)

## Notas de Implementación

### Compatibilidad

- **Unity 2022.3 LTS**: Compatible con la versión del proyecto
- **TextMesh Pro**: Requerido para los elementos de texto
- **UI Toolkit**: Usa el sistema de UI tradicional de Unity

### Optimización

- **UI Dinámica**: Se genera solo cuando es necesario
- **Memoria**: Elementos UI creados bajo demanda
- **Rendimiento**: Configuración optimizada para VR

### Extensibilidad

- **Escenarios**: Fácil agregar nuevos escenarios
- **UI Elements**: Estructura modular para agregar elementos
- **Configuración**: Parámetros ajustables desde el inspector

### Testing

- **Editor**: Funciona completamente en modo Editor
- **Build**: Requiere implementación de file picker nativo para builds
- **VR**: Preparado para integración con XR Interaction Toolkit

---

**Estado**: ✅ Sprint 2 Completado  
**Fecha**: [Fecha actual]  
**Responsable**: Equipo de Desarrollo VR
