using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRTemplate.SceneGeneration.Editor
{
    /// <summary>
    /// Editor utility para generar la escena de auditorio automáticamente
    /// Usa assets del Gwangju Theater para crear un auditorio profesional
    /// </summary>
    public class AuditoriumSceneGeneratorEditor : EditorWindow
    {
        [MenuItem("VR Simulador/Generar Auditorio", priority = 3)]
        public static void GenerateAuditoriumScene()
        {
            if (
                EditorUtility.DisplayDialog(
                    "Generar Auditorio",
                    "¿Deseas generar automáticamente la escena de auditorio usando los assets del Gwangju Theater?",
                    "Sí",
                    "No"
                )
            )
            {
                CreateAuditoriumScene();
            }
        }

        private static void CreateAuditoriumScene()
        {
            Debug.Log("🎭 Generando escena de auditorio...");

            // Crear nueva escena
            Scene newScene = EditorSceneManager.NewScene(
                NewSceneSetup.DefaultGameObjects,
                NewSceneMode.Single
            );

            // Eliminar la luz direccional por defecto para evitar conflictos
            var defaultLight = Object.FindObjectOfType<Light>();
            if (defaultLight != null && defaultLight.type == LightType.Directional)
            {
                Object.DestroyImmediate(defaultLight.gameObject);
            }

            // Parámetros del auditorio (dimensiones realistas)
            float roomWidth = 15f; // Ancho: 15 metros
            float roomLength = 20f; // Largo: 20 metros
            float roomHeight = 3.5f; // Altura: 3.5 metros
            float wallThickness = 0.3f; // Grosor de paredes: 0.3 metros
            float stageWidth = 6f; // Ancho del stage: 6 metros
            float stageLength = 4f; // Largo del stage: 4 metros
            float stageHeight = 0.5f; // Altura del stage: 0.5 metros
            float stageDistanceFromFront = 2f; // Distancia desde pared frontal
            float screenWidth = 6f; // Ancho de pantalla: 6 metros
            float screenHeight = 3.5f; // Alto de pantalla: 3.5 metros
            float screenHeightFromFloor = 1.5f; // Altura desde el suelo

            // Configuración de asientos (8 filas x 12 columnas = 96 asientos)
            int seatRows = 8;
            int seatColumns = 12;
            float seatSpacingX = 0.7f; // Espaciado entre asientos
            float seatSpacingZ = 1f; // Espaciado entre filas
            float firstRowDistance = 4f; // Distancia primera fila desde pantalla
            float seatInclination = 5f; // Inclinación gradual de las filas (grados)

            // Crear estructura básica del auditorio
            CreateAuditoriumStructure(roomWidth, roomLength, roomHeight, wallThickness);

            // Iluminación direccional múltiple
            CreateDirectionalLighting();

            // Crear stage profesional
            CreateProfessionalStage(
                stageWidth,
                stageLength,
                stageHeight,
                stageDistanceFromFront,
                roomLength
            );

            // Crear pantalla gigante
            CreateLargeScreen(
                screenWidth,
                screenHeight,
                screenHeightFromFloor,
                roomLength,
                wallThickness
            );

            // Crear asientos de auditorio con inclinación
            CreateAuditoriumSeats(
                seatRows,
                seatColumns,
                seatSpacingX,
                seatSpacingZ,
                firstRowDistance,
                seatInclination
            );

            // Crear sistema de iluminación profesional
            CreateProfessionalLighting(roomWidth, roomLength, roomHeight, stageWidth, stageLength);

            // Crear elementos decorativos del teatro
            CreateTheaterDecorations(roomWidth, roomLength, roomHeight);

            // Crear spawn point
            CreateSpawnPoint(stageHeight, stageDistanceFromFront, stageLength, roomLength);

            // Guardar la escena
            string scenePath = "Assets/Scenes/AuditoriumScene.unity";
            SaveScene(scenePath);
            AddSceneToBuildSettings(scenePath);

            Debug.Log("✅ Auditorio generado exitosamente en: " + scenePath);
            EditorUtility.DisplayDialog(
                "Auditorio Generado",
                "El auditorio ha sido generado exitosamente usando los assets del Gwangju Theater.\n\n"
                    + "Ubicación: Assets/Scenes/AuditoriumScene.unity\n\n"
                    + "Características:\n"
                    + "• 96 asientos con inclinación gradual\n"
                    + "• Stage profesional de 6x4 metros\n"
                    + "• Pantalla gigante de 6x3.5 metros\n"
                    + "• Sistema de iluminación teatral\n"
                    + "• Materiales del Gwangju Theater",
                "OK"
            );
        }

        private static void CreateAuditoriumStructure(
            float width,
            float length,
            float height,
            float wallThickness
        )
        {
            // Paredes principales con materiales del teatro
            CreateWall(
                "Wall_Front",
                new Vector3(0, height / 2, length / 2),
                new Vector3(width, height, wallThickness),
                "GtTheaterConc"
            );
            CreateWall(
                "Wall_Back",
                new Vector3(0, height / 2, -length / 2),
                new Vector3(width, height, wallThickness),
                "GtTheaterConc"
            );
            CreateWall(
                "Wall_Left",
                new Vector3(-width / 2, height / 2, 0),
                new Vector3(wallThickness, height, length),
                "GtTheaterSide"
            );
            CreateWall(
                "Wall_Right",
                new Vector3(width / 2, height / 2, 0),
                new Vector3(wallThickness, height, length),
                "GtTheaterSide"
            );

            // Suelo con material de teatro
            CreatePrimitive(
                "Floor",
                PrimitiveType.Cube,
                new Vector3(0, 0, 0),
                new Vector3(width, 0.1f, length),
                "GtFloorInner2nd"
            );

            // Techo con material de teatro
            CreatePrimitive(
                "Ceiling",
                PrimitiveType.Cube,
                new Vector3(0, height, 0),
                new Vector3(width, 0.1f, length),
                "GtWhiteConc"
            );
        }

        private static void CreateProfessionalStage(
            float width,
            float length,
            float height,
            float distanceFromFront,
            float roomLength
        )
        {
            // Stage principal
            float stageX = 0;
            float stageY = height / 2;
            float stageZ = roomLength / 2 - distanceFromFront - length / 2;
            CreatePrimitive(
                "Stage",
                PrimitiveType.Cube,
                new Vector3(stageX, stageY, stageZ),
                new Vector3(width, height, length),
                "GtTheaterWooden"
            );

            // Bordes del stage con acabados
            CreatePrimitive(
                "Stage_Trim_Front",
                PrimitiveType.Cube,
                new Vector3(0, height + 0.05f, stageZ + length / 2),
                new Vector3(width, 0.1f, 0.2f),
                "GtWoodTrim"
            );
            CreatePrimitive(
                "Stage_Trim_Left",
                PrimitiveType.Cube,
                new Vector3(-width / 2, height + 0.05f, stageZ),
                new Vector3(0.2f, 0.1f, length),
                "GtWoodTrim"
            );
            CreatePrimitive(
                "Stage_Trim_Right",
                PrimitiveType.Cube,
                new Vector3(width / 2, height + 0.05f, stageZ),
                new Vector3(0.2f, 0.1f, length),
                "GtWoodTrim"
            );
        }

        private static void CreateLargeScreen(
            float width,
            float height,
            float heightFromFloor,
            float roomLength,
            float wallThickness
        )
        {
            // Pantalla principal
            float screenX = 0;
            float screenY = heightFromFloor + height / 2;
            float screenZ = roomLength / 2 - wallThickness / 2;
            CreatePrimitive(
                "Screen",
                PrimitiveType.Cube,
                new Vector3(screenX, screenY, screenZ),
                new Vector3(width, height, 0.1f),
                "Screen"
            );

            // Marco de la pantalla
            CreatePrimitive(
                "Screen_Frame",
                PrimitiveType.Cube,
                new Vector3(screenX, screenY, screenZ - 0.05f),
                new Vector3(width + 0.5f, height + 0.5f, 0.2f),
                "GtWoodTrim"
            );
        }

        private static void CreateAuditoriumSeats(
            int rows,
            int columns,
            float spacingX,
            float spacingZ,
            float firstRowDistance,
            float inclination
        )
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    // Calcular posición con inclinación gradual
                    float seatX = (col - columns / 2f + 0.5f) * spacingX;
                    float seatY = 0.4f + (row * inclination * 0.01f); // Inclinación gradual
                    float seatZ = -firstRowDistance - row * spacingZ;

                    // Crear asiento individual
                    GameObject seat = CreatePrimitive(
                        $"Seat_R{row}_C{col}",
                        PrimitiveType.Cube,
                        new Vector3(seatX, seatY, seatZ),
                        new Vector3(0.6f, 0.9f, 0.6f),
                        "GtChair"
                    );

                    // Aplicar material de silla del teatro
                    ApplyTheaterMaterial(seat, "GtChair");
                }
            }
        }

        private static void CreateProfessionalLighting(
            float roomWidth,
            float roomLength,
            float roomHeight,
            float stageWidth,
            float stageLength
        )
        {
            // Luz ambiental general para todo el auditorio
            var ambientLightGo = new GameObject("AmbientAuditoriumLight");
            ambientLightGo.transform.position = new Vector3(0, roomHeight - 1, 0);
            var ambientLight = ambientLightGo.AddComponent<Light>();
            ambientLight.type = LightType.Point;
            ambientLight.intensity = 0.8f;
            ambientLight.range = roomWidth * 1.5f;
            ambientLight.color = new Color(1f, 0.9f, 0.8f); // Tono cálido
            ambientLight.shadows = LightShadows.Soft;

            // Luces de escenario (Spotlights)
            float lightHeight = roomHeight - 0.5f;
            float lightZPos = roomLength / 2 - 6f; // Un poco más atrás del stage

            // Foco central
            CreateStageLight(
                "Center_Spotlight",
                new Vector3(0, lightHeight, lightZPos),
                new Vector3(45f, 0, 0),
                2.5f,
                45f
            );

            // Foco izquierdo
            CreateStageLight(
                "Left_Spotlight",
                new Vector3(-stageWidth / 3, lightHeight, lightZPos),
                new Vector3(45f, -20f, 0),
                2.0f,
                40f
            );

            // Foco derecho
            CreateStageLight(
                "Right_Spotlight",
                new Vector3(stageWidth / 3, lightHeight, lightZPos),
                new Vector3(45f, 20f, 0),
                2.0f,
                40f
            );
        }

        private static void CreateStageLight(
            string name,
            Vector3 position,
            Vector3 rotation,
            float intensity,
            float spotAngle
        )
        {
            var lightGo = new GameObject(name);
            lightGo.transform.position = position;
            lightGo.transform.rotation = Quaternion.Euler(rotation);

            var light = lightGo.AddComponent<Light>();
            light.type = LightType.Spot;
            light.intensity = intensity;
            light.range = 20f;
            light.spotAngle = spotAngle;
            light.color = new Color(0.9f, 0.9f, 1f); // Tono ligeramente frío para el escenario
            light.shadows = LightShadows.Soft;
        }

        private static void CreateTheaterDecorations(float width, float length, float height)
        {
            // Cortinas laterales
            CreatePrimitive(
                "Curtain_Left",
                PrimitiveType.Cube,
                new Vector3(-width / 2 + 0.5f, height / 2, 0),
                new Vector3(0.1f, height - 1f, length / 2),
                "GtRedFabric"
            );
            CreatePrimitive(
                "Curtain_Right",
                PrimitiveType.Cube,
                new Vector3(width / 2 - 0.5f, height / 2, 0),
                new Vector3(0.1f, height - 1f, length / 2),
                "GtRedFabric"
            );

            // Acabados de pared con madera
            CreatePrimitive(
                "WallTrim_Front",
                PrimitiveType.Cube,
                new Vector3(0, height / 2, length / 2 - 0.1f),
                new Vector3(width, 0.3f, 0.1f),
                "GtWoodTrim"
            );
            CreatePrimitive(
                "WallTrim_Back",
                PrimitiveType.Cube,
                new Vector3(0, height / 2, -length / 2 + 0.1f),
                new Vector3(width, 0.3f, 0.1f),
                "GtWoodTrim"
            );

            // Pasillos laterales
            CreatePrimitive(
                "Aisle_Left",
                PrimitiveType.Cube,
                new Vector3(-width / 4, 0.01f, 0),
                new Vector3(1f, 0.02f, length - 2f),
                "GtFloorInnerTrim1st"
            );
            CreatePrimitive(
                "Aisle_Right",
                PrimitiveType.Cube,
                new Vector3(width / 4, 0.01f, 0),
                new Vector3(1f, 0.02f, length - 2f),
                "GtFloorInnerTrim1st"
            );
        }

        private static void CreateSpawnPoint(
            float stageHeight,
            float stageDistanceFromFront,
            float stageLength,
            float roomLength
        )
        {
            float spawnX = 0;
            float spawnY = stageHeight + 1.7f;
            float spawnZ = roomLength / 2 - stageDistanceFromFront - stageLength / 2;
            GameObject spawnPoint = new GameObject("SpawnPoint");
            spawnPoint.transform.position = new Vector3(spawnX, spawnY, spawnZ);
        }

        private static void CreateWall(
            string name,
            Vector3 position,
            Vector3 scale,
            string materialName
        )
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.position = position;
            wall.transform.localScale = scale;
            ApplyTheaterMaterial(wall, materialName);
        }

        private static GameObject CreatePrimitive(
            string name,
            PrimitiveType type,
            Vector3 position,
            Vector3 scale,
            string materialName
        )
        {
            GameObject obj = GameObject.CreatePrimitive(type);
            obj.name = name;
            obj.transform.position = position;
            obj.transform.localScale = scale;
            ApplyTheaterMaterial(obj, materialName);
            return obj;
        }

        private static void ApplyTheaterMaterial(GameObject obj, string materialName)
        {
            string materialPath =
                $"Assets/Others/Gwangju_3D asset/26_GwangjuTheater/Materials/Inner/{materialName}.mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            if (material != null)
            {
                obj.GetComponent<Renderer>().sharedMaterial = material;
            }
            else
            {
                // Material por defecto si no se encuentra
                Debug.LogWarning($"Material {materialName} no encontrado en {materialPath}");
                obj.GetComponent<Renderer>().sharedMaterial.color = Color.gray;
            }

            // Configurar collider correctamente
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;

                // Configuración específica para el stage
                if (obj.name == "Stage")
                {
                    if (collider is BoxCollider boxCollider)
                    {
                        boxCollider.size = Vector3.one; // Tamaño normalizado
                        boxCollider.center = Vector3.zero;
                    }
                    Debug.Log($"✅ Collider configurado para {obj.name}");
                }
            }
            else
            {
                Debug.LogError($"❌ No se encontró collider en {obj.name}");
            }
        }

        private static void SaveScene(string scenePath)
        {
            string directory = System.IO.Path.GetDirectoryName(scenePath);
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);
        }

        private static void AddSceneToBuildSettings(string scenePath)
        {
            var buildScenes = EditorBuildSettings.scenes.ToList();
            bool sceneExists = buildScenes.Exists(scene => scene.path == scenePath);
            if (!sceneExists)
            {
                buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                EditorBuildSettings.scenes = buildScenes.ToArray();
            }
        }

        private static void CreateDirectionalLighting()
        {
            // Luz principal (simula luz solar)
            GameObject mainLight = new GameObject("DirectionalLight_Main");
            Light light1 = mainLight.AddComponent<Light>();
            light1.type = LightType.Directional;
            light1.intensity = 1.0f;
            light1.color = new Color(1f, 0.97f, 0.92f);
            mainLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            // Luz secundaria (simula rebote desde el suelo)
            GameObject fillLight = new GameObject("DirectionalLight_Fill");
            Light light2 = fillLight.AddComponent<Light>();
            light2.type = LightType.Directional;
            light2.intensity = 0.4f;
            light2.color = new Color(0.8f, 0.9f, 1f);
            fillLight.transform.rotation = Quaternion.Euler(340f, 30f, 0f);

            // Luz ambiente lateral (simula rebote lateral)
            GameObject sideLight = new GameObject("DirectionalLight_Side");
            Light light3 = sideLight.AddComponent<Light>();
            light3.type = LightType.Directional;
            light3.intensity = 0.3f;
            light3.color = new Color(1f, 0.95f, 0.8f);
            sideLight.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }
}
