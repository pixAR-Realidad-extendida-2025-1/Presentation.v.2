# Simulador VR de Presentaciones Orales

## Descripción

Simulador de realidad virtual para practicar presentaciones orales en diferentes entornos. Permite cargar presentaciones PDF y presentarlas en tres escenarios: sala de clases, auditorio y sala de conferencias.

## Características

- **3 Escenarios**: Sala de clases, auditorio y sala de conferencias
- **Carga de PDF**: Soporte para presentaciones en formato PDF
- **Controles VR**: Navegación con controles VR y teclado
- **Timer**: Cronómetro para controlar tiempo de presentación
- **Interfaz intuitiva**: Menú de configuración simple

## Requisitos

### Software

- Unity 2022.3 LTS o superior
- Dispositivo VR (Meta Quest, HTC Vive, etc.)

### Hardware

- PC compatible con VR
- Dispositivo VR conectado

## Instalación

1. **Descargar** el proyecto desde el repositorio
2. **Abrir Unity Hub** y agregar el proyecto
3. **Esperar** a que Unity cargue todos los assets
4. **Ejecutar** el menú "VR Simulador" → "Generar y Configurar Todo"

## Uso

### Configuración Inicial

1. Abrir la escena `ConfigurationScene`
2. Seleccionar el escenario deseado
3. Hacer clic en "Iniciar Presentación"

### Controles VR

- **Botón A**: Siguiente diapositiva
- **Botón X**: Diapositiva anterior
- **Botón B**: Abrir explorador de archivos
- **Enter**: Abrir explorador de archivos (teclado)

### Timer

- **Start**: Iniciar cronómetro
- **Pause**: Pausar cronómetro
- **Reset**: Reiniciar cronómetro

## Estructura del Proyecto

```
Assets/
├── Scenes/              # Escenas del simulador
├── VRTemplateAssets/    # Template VR base
├── Others/              # Assets adicionales
│   └── mupdf-1.26.2-windows/ # Conversor PDF
└── Jo/                  # Scripts de presentación
```

## Solución de Problemas

### "No se encontró mutool"

- El proyecto incluye MuPDF en `Assets/Others/mupdf-1.26.2-windows/`
- Si persiste, descargar MuPDF desde https://mupdf.com/downloads/

### "No se detectó dispositivo VR"

- Verificar conexión del dispositivo VR
- En modo editor funciona con teclado y mouse

### Mouse visible en VR

- El mouse se oculta automáticamente en presentaciones
- Solo visible en menú de configuración

## Tecnologías

- Unity 2022.3 LTS
- XR Interaction Toolkit
- MuPDF (conversión PDF)
