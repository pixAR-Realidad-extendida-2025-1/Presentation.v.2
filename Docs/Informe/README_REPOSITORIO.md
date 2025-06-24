# Simulador VR de Presentaciones Orales

## Descripción

Este repositorio contiene los artefactos de implementación del simulador de realidad virtual (VR) para el entrenamiento de habilidades de presentación oral. El proyecto fue desarrollado en Unity 2022.3 LTS utilizando XR Interaction Toolkit.

## Características Principales

- **Tres entornos inmersivos**: Sala de clases, auditorio y sala de conferencias
- **Sistema de carga de PDF**: Soporte para presentaciones en formato PDF
- **Controles VR intuitivos**: Teleportación y navegación de diapositivas
- **Optimización para VR**: Mantiene 90fps mínimo para experiencia fluida
- **Arquitectura modular**: Escalable y reutilizable

## Requisitos del Sistema

### Software

- Unity 2022.3 LTS o superior
- XR Interaction Toolkit 3.1.1
- TextMesh Pro

### Hardware

- Dispositivo VR compatible (Meta Quest, HTC Vive, etc.)
- GPU compatible con VR
- Mínimo 8GB RAM

## Instalación

1. **Clonar el repositorio**

   ```bash
   git clone https://github.com/[usuario]/simulador-vr-presentaciones.git
   cd simulador-vr-presentaciones
   ```

2. **Abrir en Unity**

   - Abrir Unity Hub
   - Agregar proyecto existente
   - Seleccionar la carpeta del proyecto

3. **Configuración inicial**

   - Ejecutar el script de configuración automática
   - Configurar build settings para VR
   - Verificar que XR Interaction Toolkit esté habilitado

4. **Generar escenas**
   - Ejecutar los scripts de generación de escenas
   - Verificar que todas las escenas se hayan creado correctamente

## Uso del Simulador

### Inicio

1. Abrir la escena "Configuration Scene"
2. Seleccionar el escenario deseado (Sala de Clases, Auditorio, o Sala de Conferencias)
3. Cargar archivo PDF de presentación
4. Hacer clic en "Iniciar Presentación"

### Controles VR

- **Teleportación**: Usar controlador para apuntar y teleportarse en el stage
- **Navegación de diapositivas**: Botones virtuales para avanzar/retroceder
- **Interacción**: Selección de objetos con raycast

### Escenarios Disponibles

#### Sala de Clases

- Dimensiones: 8m x 12m
- Capacidad: 20-30 asientos
- Ambiente educativo
- Stage: 3m x 2m

#### Auditorio

- Dimensiones: 15m x 20m
- Capacidad: 100-150 asientos
- Ambiente profesional
- Stage: 6m x 4m

#### Sala de Conferencias

- Dimensiones: 6m x 10m
- Capacidad: 15-20 asientos
- Ambiente ejecutivo
- Stage: 2.5m x 1.5m

## Estructura del Proyecto

```
Assets/
├── VRTemplateAssets/          # Template VR base
├── Scenes/                    # Escenas del simulador
│   ├── ConfigurationScene.unity
│   ├── ClassroomScene.unity
│   ├── AuditoriumScene.unity
│   └── ConferenceScene.unity
├── Scripts/                   # Scripts de configuración
│   ├── Setup/
│   ├── SceneGeneration/
│   └── SceneManagement/
├── Prefabs/                   # Prefabs reutilizables
└── Models/                    # Modelos 3D
```

## Configuración de Build

1. **Configurar Build Settings**

   - File > Build Settings
   - Seleccionar plataforma (Android para Quest, PC para otros)
   - Agregar todas las escenas al build

2. **Configuración VR**

   - Project Settings > XR Plug-in Management
   - Habilitar Oculus (para Quest) o OpenXR
   - Configurar Quality Settings para VR

3. **Build**
   - Build > Build and Run
   - Seguir instrucciones específicas de la plataforma

## Desarrollo

### Scripts Principales

- `UnityConfigurationSetup.cs`: Configuración inicial del proyecto
- `ConfigurationSceneGenerator.cs`: Generación de escena de menú
- `ClassroomSceneGenerator.cs`: Generación de sala de clases
- `AuditoriumSceneGenerator.cs`: Generación de auditorio
- `ConferenceSceneGenerator.cs`: Generación de sala de conferencias

### Assets Utilizados

- **VRTemplateAssets**: Template base con scripts y prefabs VR
- **Gwangju 3D Asset**: Assets de teatro para el auditorio
- **Primitivas Unity**: Cubos, cilindros para elementos básicos

## Optimización

### Rendimiento VR

- LOD (Level of Detail) implementado
- Occlusion culling habilitado
- Texturas optimizadas (máximo 2K)
- Configuración de calidad para 90fps

### Mejores Prácticas

- Uso de prefabs reutilizables
- Scripts de configuración automática
- Documentación completa
- Testing en dispositivos VR reales

## Contribución

Para contribuir al proyecto:

1. Fork del repositorio
2. Crear rama para nueva funcionalidad
3. Implementar cambios siguiendo estándares de Unity
4. Probar en dispositivo VR real
5. Crear Pull Request con documentación

## Licencia

Este proyecto está bajo la licencia [ESPECIFICAR LICENCIA]

## Contacto

- **Equipo de Desarrollo**: Departamento de Informática, Universidad de Chile
- **Email**: [email de contacto]
- **Repositorio**: https://github.com/[usuario]/simulador-vr-presentaciones

## Agradecimientos

- Unity Technologies por XR Interaction Toolkit
- VRTemplateAssets por la base de desarrollo VR
- Gwangju 3D Asset por los assets de teatro

## Estado del Proyecto

- [x] Configuración base del proyecto
- [x] Generación de escenas
- [x] Integración de escenas
- [ ] Sistema de carga de PDF
- [ ] Controles de presentación
- [ ] Testing y optimización final

---

**Nota**: Este proyecto está en desarrollo activo. Para la versión más reciente, consultar el repositorio principal.
