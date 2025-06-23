# Roadmap Visual - Simulador VR de Presentaciones

## 🎯 Objetivo MVP

**Un menú funcional que permita seleccionar escenario, cargar PDF y simular presentación con controles VR básicos.**

---

## 📋 Plan de Desarrollo

### 🚀 FASE 1: Configuración Base

```
1. Script de Configuración de Unity
   ├── XR Interaction Toolkit
   ├── Build Settings para VR
   ├── Project Settings
   └── Quality Settings (90fps)
```

### 🏗️ FASE 2: Generación de Escenas

```
2. Scripts de Generación
   ├── 2.1 Configuration Scene (Menú Principal)
   ├── 2.2 Classroom Scene (Sala de Clases)
   ├── 2.3 Auditorium Scene (Auditorio)
   └── 2.4 Conference Scene (Sala de Conferencias)
```

### 🔗 FASE 3: Integración

```
3. Integrar Escenas a Unity
   ├── Scene Management
   ├── Scene Transitions
   └── Scene Loading
```

### ⚙️ FASE 4: Funcionalidades Core

```
4. Integrar Funcionalidades
   ├── Sistema de carga de PDF
   ├── Spawn Point en cada escena
   ├── Sistema de teleportación en stage
   └── Controles de presentación (adelante/atrás)
```

### 🧹 FASE EXTRA: Mantenimiento

```
Extra: Mantenimiento
   ├── Cleanup de escenas existentes
   └── Backup de configuración actual
```

---

## 📁 Estructura de Scripts

```
Scripts/
├── Setup/
│   └── UnityConfigurationSetup.cs
├── SceneGeneration/
│   ├── ConfigurationSceneGenerator.cs
│   ├── ClassroomSceneGenerator.cs
│   ├── AuditoriumSceneGenerator.cs
│   └── ConferenceSceneGenerator.cs
├── SceneManagement/
│   └── SceneManager.cs
├── PDF/
│   └── PDFLoader.cs
├── VR/
│   ├── SpawnPointManager.cs
│   └── TeleportationSystem.cs
└── Presentation/
    └── PresentationController.cs
```

---

## 🎮 Flujo de Usuario (MVP)

```
1. Usuario abre aplicación
   ↓
2. Ve menú principal con 3 opciones de escenario
   ↓
3. Selecciona escenario (Clases/Auditorio/Conferencias)
   ↓
4. Carga archivo PDF
   ↓
5. Presiona "Iniciar Presentación"
   ↓
6. Aparece en spawn point del escenario seleccionado
   ↓
7. Puede teleportarse en el stage
   ↓
8. Controla presentación con controles VR (adelante/atrás)
```

---

## ⚡ Sprints de Desarrollo

| Sprint       | Duración | Objetivo             | Entregable                        |
| ------------ | -------- | -------------------- | --------------------------------- |
| **Sprint 1** | 1-2 días | Configuración Base   | Unity configurado para VR         |
| **Sprint 2** | 2-3 días | Menú Principal       | Escena de configuración funcional |
| **Sprint 3** | 3-4 días | Sala de Clases       | Escena básica con teleportación   |
| **Sprint 4** | 3-4 días | Auditorio            | Escena con assets del teatro      |
| **Sprint 5** | 2-3 días | Sala de Conferencias | Escena con mesa y asientos        |
| **Sprint 6** | 2-3 días | Integración          | Transiciones entre escenas        |
| **Sprint 7** | 4-5 días | Funcionalidades Core | PDF + Controles VR                |
| **Sprint 8** | 2-3 días | Testing              | MVP funcional completo            |

**Tiempo Total Estimado: 19-27 días**

---

## 🎯 Criterios de Éxito MVP

### ✅ Funcionalidades Mínimas

- [ ] Menú principal con selección de escenario
- [ ] Carga de archivo PDF
- [ ] 3 escenas generadas automáticamente
- [ ] Spawn point funcional en cada escena
- [ ] Teleportación dentro del stage
- [ ] Controles de presentación (adelante/atrás)
- [ ] Transiciones entre escenas
- [ ] Experiencia VR fluida (90fps)

### 🚫 Fuera del Alcance MVP

- Decoraciones detalladas
- Efectos visuales avanzados
- Sistema de audio complejo
- Múltiples formatos de presentación
- Configuraciones avanzadas
- Analytics o métricas

---

## 🔧 Consideraciones Técnicas

### Prioridades

1. **Funcionalidad > Estética**
2. **Assets existentes > Crear nuevos**
3. **Dimensiones realistas**
4. **VR First**

### Assets a Usar

- **VRTemplateAssets**: Base VR y primitivas
- **Gwangju Theater**: Auditorio
- **Primitivas**: Estructura básica de todas las escenas

### Optimización

- Texturas máx 2K para VR
- LOD para objetos complejos
- Occlusion culling
- Pooling de objetos

---

## 📝 Notas Importantes

### Antes de Empezar

- Hacer backup de configuración actual
- Verificar compatibilidad de assets
- Documentar estado actual del proyecto

### Durante el Desarrollo

- Probar cada sprint en dispositivo VR real
- Mantener versionado con Git
- Documentar cambios importantes
- Optimizar rendimiento constantemente

### Al Finalizar MVP

- Testing completo en VR
- Documentación de uso
- Guía de instalación
- Plan para próximas iteraciones
