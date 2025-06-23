# Análisis de Escenas - Simulador VR de Presentaciones

## Resumen Ejecutivo

Este documento define las especificaciones técnicas y dimensionales para las tres escenas principales del simulador VR de presentaciones orales: Sala de Clases, Auditorio y Sala de Conferencias. Cada escena sigue un estándar común pero con configuraciones específicas según su propósito.

## Estándar Común de Escenas

### Elementos Base (Obligatorios)

- **4 Paredes**: Frente, trasera, izquierda, derecha
- **Suelo**: Base horizontal de la sala
- **Techo**: Cubierta superior
- **Pantalla de Presentación**: Ubicada en la pared trasera
- **Stage**: Área de movimiento del usuario
- **Asientos**: Orientados hacia la pared trasera
- **Sistema de Iluminación**: Cobertura completa de la sala
- **Spawn Point**: Punto de aparición del usuario en el stage

### Dimensiones Base

- **Altura de Paredes**: 3.5 metros (estándar)
- **Altura de Techo**: 4 metros
- **Grosor de Paredes**: 0.2 metros
- **Altura de Stage**: 0.3 metros sobre el suelo

## 1. Sala de Clases

### Propósito

Ambiente educativo para presentaciones a grupos pequeños (15-30 personas)

### Dimensiones Específicas

- **Ancho**: 8 metros
- **Largo**: 12 metros
- **Área Total**: 96 m²
- **Capacidad**: 20-30 asientos

### Configuración de Elementos

#### Stage

- **Dimensiones**: 3m x 2m
- **Posición**: Centro-frontal, 1.5m desde la pared frontal
- **Altura**: 0.3m sobre el suelo
- **Material**: Madera o linóleo (apariencia educativa)

#### Pantalla de Presentación

- **Dimensiones**: 3m x 2m
- **Posición**: Centrada en la pared trasera
- **Altura desde el suelo**: 1.2m
- **Tipo**: Pizarra digital o proyector

#### Asientos

- **Tipo**: Sillas individuales con paletas
- **Disposición**: 4 filas x 6 columnas (24 asientos)
- **Espaciado**: 0.8m entre filas, 0.6m entre asientos
- **Distancia desde pantalla**: 2.5m (primera fila)
- **Orientación**: Hacia la pared trasera

#### Iluminación

- **Tipo**: Fluorescente o LED
- **Distribución**: 4 filas de luces paralelas
- **Intensidad**: Moderada (para ambiente educativo)
- **Control**: Luz general + iluminación focal en pantalla

### Assets Recomendados

- **VRTemplateAssets/Models/Primitives/Cube.fbx**: Para paredes y stage
- **VRTemplateAssets/Models/Environment/Template Environment.fbx**: Elementos base
- **Gwangju 3D asset**: Elementos de mobiliario si son compatibles

## 2. Auditorio

### Propósito

Espacio amplio para presentaciones a grandes audiencias (50-200 personas)

### Dimensiones Específicas

- **Ancho**: 15 metros
- **Largo**: 20 metros
- **Área Total**: 300 m²
- **Capacidad**: 100-150 asientos

### Configuración de Elementos

#### Stage

- **Dimensiones**: 6m x 4m
- **Posición**: Centro-frontal, 2m desde la pared frontal
- **Altura**: 0.5m sobre el suelo
- **Material**: Madera noble o alfombra profesional

#### Pantalla de Presentación

- **Dimensiones**: 6m x 3.5m
- **Posición**: Centrada en la pared trasera
- **Altura desde el suelo**: 1.5m
- **Tipo**: Pantalla gigante o sistema de proyección múltiple

#### Asientos

- **Tipo**: Butacas de auditorio
- **Disposición**: 8 filas x 12 columnas (96 asientos) + balcón superior
- **Espaciado**: 1m entre filas, 0.7m entre asientos
- **Distancia desde pantalla**: 4m (primera fila)
- **Orientación**: Hacia la pared trasera con inclinación gradual

#### Iluminación

- **Tipo**: Sistema profesional de iluminación escénica
- **Distribución**: Luces principales + luces de escena + luces de emergencia
- **Intensidad**: Variable (dimmable)
- **Control**: Sistema de iluminación teatral

### Assets Recomendados

- **Gwangju 3D asset/26_GwangjuTheater/Prefabs/GwangjuTheater_Interior.prefab**: Base del auditorio
- **Gwangju 3D asset/26_GwangjuTheater/Meshes/GwangjuTheater_Interior.fbx**: Geometría del teatro
- **VRTemplateAssets/Models/Primitives**: Elementos estructurales

## 3. Sala de Conferencias

### Propósito

Entorno profesional para presentaciones ejecutivas (10-25 personas)

### Dimensiones Específicas

- **Ancho**: 6 metros
- **Largo**: 10 metros
- **Área Total**: 60 m²
- **Capacidad**: 15-20 asientos

### Configuración de Elementos

#### Stage

- **Dimensiones**: 2.5m x 1.5m
- **Posición**: Centro-frontal, 1m desde la pared frontal
- **Altura**: 0.2m sobre el suelo
- **Material**: Alfombra ejecutiva o madera elegante

#### Pantalla de Presentación

- **Dimensiones**: 2.5m x 1.8m
- **Posición**: Centrada en la pared trasera
- **Altura desde el suelo**: 1.3m
- **Tipo**: Pantalla LCD de alta definición

#### Mesa de Conferencia

- **Dimensiones**: 4m x 1.2m
- **Forma**: Ovalada o rectangular con esquinas redondeadas
- **Altura**: 0.75m
- **Material**: Madera ejecutiva o vidrio templado
- **Posición**: Centro de la sala, paralela a la pantalla

#### Asientos

- **Tipo**: Sillas ejecutivas con ruedas
- **Disposición**: Del lado contrario a la pantalla, 4 asientos
- **Espaciado**: 0.8m entre asientos
- **Orientación**: Hacia la pared trasera

#### Iluminación

- **Tipo**: LED de alta eficiencia
- **Distribución**: Luces principales + iluminación focal + luces ambientales
- **Intensidad**: Ajustable según necesidad
- **Control**: Sistema de control inteligente

### Assets Recomendados

- **VRTemplateAssets/Models/Primitives/Cube.fbx**: Estructura básica
- **VRTemplateAssets/Models/Primitives/Cylinder.fbx**: Elementos decorativos
- **Assets de mobiliario ejecutivo**: Si están disponibles

## Especificaciones Técnicas Comunes

### Materiales y Texturas

- **Paredes**: Textura neutra, color claro (blanco, beige, gris claro)
- **Suelo**: Material apropiado al contexto (linóleo, alfombra, madera)
- **Techo**: Plafón acústico o techo plano
- **Stage**: Material antideslizante y duradero

### Acústica

- **Absorción**: Materiales que reduzcan la reverberación
- **Aislamiento**: Paredes con propiedades acústicas
- **Sistema de audio**: Altavoces integrados según el tamaño de la sala

### Iluminación

- **Temperatura de color**: 4000K-5000K (luz neutra)
- **Índice de reproducción cromática**: >80
- **Control de intensidad**: Dimmable en todas las salas
- **Iluminación de emergencia**: Cumplir estándares de seguridad

## Consideraciones de Rendimiento (BAJA PRIORIDAD)

### Optimización

- **LOD (Level of Detail)**: Diferentes niveles de detalle según distancia
- **Occlusion Culling**: Optimización de renderizado
- **Texturas**: Resolución apropiada para VR (2K máximo)

### Escalabilidad

- **Modularidad**: Componentes reutilizables entre escenas
- **Configuración**: Parámetros ajustables sin recompilar
- **Assets**: Uso eficiente de memoria

## Próximos Pasos

1. **Validación de Assets**: Verificar compatibilidad de los assets identificados
2. **Prototipado**: Crear versiones básicas de cada escena
3. **Testing**: Probar en dispositivos VR reales
4. **Optimización**: Ajustar rendimiento según resultados
5. **Documentación**: Crear guías de implementación específicas

## Notas de Implementación

- Todas las dimensiones están en metros del mundo real
- Las proporciones deben mantenerse para una experiencia VR realista
- Considerar la escala del usuario VR (altura promedio 1.7m)
- Los assets del Gwangju Theater pueden requerir adaptación para otros escenarios
- Mantener consistencia visual entre las tres escenas
