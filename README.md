# Simulador VR de Presentaciones Orales

## Descripción del Proyecto

Este proyecto es un simulador de realidad virtual (VR) diseñado para ayudar a los usuarios a practicar presentaciones orales en diferentes entornos. El simulador ofrece tres escenarios principales: sala de clases, auditorio y sala de conferencias, cada uno con sus propias características y configuraciones específicas.

## Características Principales

### Escenarios Disponibles

- **Sala de Clases**: Ambiente educativo con capacidad reducida
- **Auditorio**: Espacio amplio para presentaciones a grandes audiencias
- **Sala de Conferencias**: Entorno profesional con mesa y asientos

### Funcionalidades Core

- **Selección de Escenario**: Interfaz de inicio para elegir el entorno deseado
- **Carga de Presentaciones**: Soporte para archivos PDF
- **Controles de Presentación**: Navegación hacia adelante y atrás en las diapositivas
- **Movimiento VR**: Teleportación y navegación natural en el espacio virtual
- **Spawn Point**: Punto de aparición configurado en cada escenario

## Arquitectura del Proyecto

### Estructura de Escenas

Cada escenario sigue un estándar común que incluye:

- 4 paredes (frente, trasera, izquierda, derecha)
- Suelo y techo
- Pantalla de presentación en la pared trasera
- Stage (área de movimiento del usuario)
- Asientos orientados hacia la pared trasera
- Sistema de iluminación completo
- Spawn point en el stage

### Assets Disponibles

- **VRTemplateAssets**: Template base con scripts, prefabs y modelos VR
- **Gwangju 3D Asset**: Assets de teatro que pueden ser reutilizados
- **School Assets**: Elementos educativos
- **Primitivas**: Cubos, cilindros, esferas para construcción básica

## Estrategia de Desarrollo

### Enfoque Paso a Paso

El desarrollo se basa en scripts de configuración automatizada:

1. **Script de Iniciación**: Configuración inicial del proyecto
2. **Script de Build Settings**: Configuración de compilación
3. **Scripts de Generación de Escenas**: Creación automática de cada escenario
4. **Scripts de Configuración**: Ajustes específicos por escenario

### MVP (Minimum Viable Product)

- Funcionalidad básica de presentación con PDF
- Navegación entre diapositivas
- Tres escenarios funcionales
- Controles VR básicos

## Tecnologías Utilizadas

- **Unity**: Motor de desarrollo principal
- **XR Interaction Toolkit**: Framework para interacciones VR
- **TextMesh Pro**: Sistema de texto avanzado
- **VR Template**: Base de desarrollo VR

## Instalación y Configuración

### Requisitos Previos

- Unity 2022.3 LTS o superior
- XR Interaction Toolkit
- Dispositivo VR compatible (Meta Quest, HTC Vive, etc.)

### Pasos de Instalación

1. Clonar el repositorio
2. Abrir el proyecto en Unity
3. Ejecutar el script de configuración inicial
4. Configurar las build settings para VR
5. Generar las escenas base

## Uso del Simulador

1. **Inicio**: Seleccionar escenario desde la pantalla principal
2. **Carga**: Subir archivo PDF de presentación
3. **Configuración**: Ajustar parámetros específicos del escenario
4. **Iniciar**: Comenzar la simulación en el spawn point
5. **Presentación**: Usar controles VR para navegar las diapositivas

## Estructura de Archivos

```
Assets/
├── VRTemplateAssets/     # Template VR base
├── Others/              # Assets adicionales
│   ├── Gwangju_3D asset/ # Assets de teatro
│   └── school/          # Assets educativos
├── Scenes/              # Escenas del proyecto
└── Scripts/             # Scripts de configuración
```

## Documentación Adicional

- [Análisis de Escenas](Docs/ANALISIS_ESCENAS.md): Especificaciones detalladas de cada escenario
- [Guía de Configuración](Docs/CONFIGURACION.md): Pasos para configurar el proyecto
- [Manual de Uso](Docs/MANUAL_USUARIO.md): Guía completa para usuarios

## Estado del Proyecto

- [x] Estructura base del proyecto
- [x] Assets y recursos identificados
- [ ] Scripts de configuración automática
- [ ] Generación de escenas base
- [ ] Sistema de carga de PDF
- [ ] Controles de presentación
- [ ] Testing y optimización

## Contribución

Este proyecto está en desarrollo activo. Para contribuir:

1. Revisar la documentación de análisis
2. Seguir los estándares de Unity y VR
3. Probar en dispositivos VR reales
4. Documentar cambios y mejoras

## Licencia

[Especificar licencia del proyecto]

## Contacto

[Información de contacto del equipo de desarrollo]
