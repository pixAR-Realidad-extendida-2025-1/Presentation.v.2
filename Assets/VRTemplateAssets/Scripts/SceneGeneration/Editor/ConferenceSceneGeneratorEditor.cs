using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRTemplate.SceneGeneration.Editor
{
    /// <summary>
    /// Editor utility para generar la escena de sala de conferencias automáticamente
    /// Ambiente profesional para presentaciones ejecutivas
    /// </summary>
    public class ConferenceSceneGeneratorEditor : EditorWindow
    {
        [MenuItem("VR Simulador/Generar Sala de Conferencias", priority = 4)]
        public static void GenerateConferenceScene()
        {
            if (
                EditorUtility.DisplayDialog(
                    "Generar Sala de Conferencias",
                    "¿Deseas generar automáticamente la escena de sala de conferencias?",
                    "Sí",
                    "No"
                )
            )
            {
                CreateConferenceScene();
            }
        }

        private static void CreateConferenceScene()
        {
            Debug.Log("💼 Generando escena de sala de conferencias...");

            // Crear nueva escena
            Scene newScene = EditorSceneManager.NewScene(
                NewSceneSetup.DefaultGameObjects,
                NewSceneMode.Single
            );

            // Eliminar la luz direccional por defecto
            var defaultLight = Object.FindObjectOfType<Light>();
            if (defaultLight != null && defaultLight.type == LightType.Directional)
            {
                Object.DestroyImmediate(defaultLight.gameObject);
            }

            // Parámetros de la sala (dimensiones ejecutivas)
            float roomWidth = 6f; // Ancho: 6 metros
            float roomLength = 10f; // Largo: 10 metros
            float roomHeight = 3.5f; // Altura: 3.5 metros
            float wallThickness = 0.2f; // Grosor de paredes

            // Configuración del stage (más pequeño y elegante)
            float stageWidth = 2.5f; // Ancho del stage: 2.5 metros
            float stageLength = 1.5f; // Largo del stage: 1.5 metros
            float stageHeight = 0.2f; // Altura del stage: 0.2 metros
            float stageDistanceFromFront = 1f; // Distancia desde pared frontal

            // Configuración de la pantalla (alta definición)
            float screenWidth = 2.5f; // Ancho de pantalla: 2.5 metros
            float screenHeight = 1.8f; // Alto de pantalla: 1.8 metros
            float screenHeightFromFloor = 1.3f; // Altura desde el suelo

            // Configuración de la mesa de conferencia
            float tableWidth = 4f; // Ancho de la mesa: 4 metros
            float tableLength = 1.2f; // Largo de la mesa: 1.2 metros
            float tableHeight = 0.75f; // Altura de la mesa: 0.75 metros
            float tableDistanceFromScreen = 3f; // Distancia desde la pantalla

            // Configuración de asientos ejecutivos
            int seatCount = 4; // 4 asientos ejecutivos
            float seatSpacing = 0.8f; // Espaciado entre asientos
            float seatHeight = 0.5f; // Altura de los asientos

            // Configuración de iluminación LED
            int lightCount = 6; // 6 luces LED
            float lightIntensity = 0.8f; // Intensidad moderada

            // Crear paredes (material elegante)
            CreateWall(
                "Wall_Front",
                new Vector3(0, roomHeight / 2, roomLength / 2),
                new Vector3(roomWidth, roomHeight, wallThickness),
                new Color(0.95f, 0.95f, 0.95f)
            );
            CreateWall(
                "Wall_Back",
                new Vector3(0, roomHeight / 2, -roomLength / 2),
                new Vector3(roomWidth, roomHeight, wallThickness),
                new Color(0.95f, 0.95f, 0.95f)
            );
            CreateWall(
                "Wall_Left",
                new Vector3(-roomWidth / 2, roomHeight / 2, 0),
                new Vector3(wallThickness, roomHeight, roomLength),
                new Color(0.95f, 0.95f, 0.95f)
            );
            CreateWall(
                "Wall_Right",
                new Vector3(roomWidth / 2, roomHeight / 2, 0),
                new Vector3(wallThickness, roomHeight, roomLength),
                new Color(0.95f, 0.95f, 0.95f)
            );

            // Iluminación direccional múltiple
            CreateDirectionalLighting();

            // Crear suelo (alfombra ejecutiva)
            CreatePrimitive(
                "Floor",
                PrimitiveType.Cube,
                new Vector3(0, 0, 0),
                new Vector3(roomWidth, 0.05f, roomLength),
                new Color(0.3f, 0.3f, 0.35f)
            );

            // Crear techo (plafón ejecutivo)
            CreatePrimitive(
                "Ceiling",
                PrimitiveType.Cube,
                new Vector3(0, roomHeight, 0),
                new Vector3(roomWidth, 0.1f, roomLength),
                new Color(0.98f, 0.98f, 0.98f)
            );

            // Crear stage (elegante y discreto)
            float stageX = 0;
            float stageY = stageHeight / 2;
            float stageZ = roomLength / 2 - stageDistanceFromFront - stageLength / 2;
            CreatePrimitive(
                "Stage",
                PrimitiveType.Cube,
                new Vector3(stageX, stageY, stageZ),
                new Vector3(stageWidth, stageHeight, stageLength),
                new Color(0.4f, 0.3f, 0.2f)
            );

            // Crear pantalla LCD de alta definición
            float screenX = 0;
            float screenY = screenHeightFromFloor + screenHeight / 2;
            float screenZ = roomLength / 2 - wallThickness / 2;
            CreatePrimitive(
                "Screen",
                PrimitiveType.Cube,
                new Vector3(screenX, screenY, screenZ),
                new Vector3(screenWidth, screenHeight, 0.05f),
                Color.black
            );

            // Crear marco de la pantalla
            CreatePrimitive(
                "ScreenFrame",
                PrimitiveType.Cube,
                new Vector3(screenX, screenY, screenZ),
                new Vector3(screenWidth + 0.1f, screenHeight + 0.1f, 0.1f),
                new Color(0.2f, 0.2f, 0.2f)
            );

            // Crear mesa de conferencia ejecutiva
            float tableX = 0;
            float tableY = tableHeight / 2;
            float tableZ = screenZ - tableDistanceFromScreen;
            CreatePrimitive(
                "ConferenceTable",
                PrimitiveType.Cube,
                new Vector3(tableX, tableY, tableZ),
                new Vector3(tableWidth, tableHeight, tableLength),
                new Color(0.6f, 0.4f, 0.2f)
            );

            // Crear patas de la mesa
            float legWidth = 0.1f;
            float legHeight = tableHeight;
            float legY = legHeight / 2;
            float legZ = tableZ;

            // Patas frontales
            CreatePrimitive(
                "TableLeg_FL",
                PrimitiveType.Cube,
                new Vector3(
                    -tableWidth / 2 + legWidth / 2,
                    legY,
                    legZ + tableLength / 2 - legWidth / 2
                ),
                new Vector3(legWidth, legHeight, legWidth),
                new Color(0.4f, 0.3f, 0.2f)
            );
            CreatePrimitive(
                "TableLeg_FR",
                PrimitiveType.Cube,
                new Vector3(
                    tableWidth / 2 - legWidth / 2,
                    legY,
                    legZ + tableLength / 2 - legWidth / 2
                ),
                new Vector3(legWidth, legHeight, legWidth),
                new Color(0.4f, 0.3f, 0.2f)
            );

            // Patas traseras
            CreatePrimitive(
                "TableLeg_BL",
                PrimitiveType.Cube,
                new Vector3(
                    -tableWidth / 2 + legWidth / 2,
                    legY,
                    legZ - tableLength / 2 + legWidth / 2
                ),
                new Vector3(legWidth, legHeight, legWidth),
                new Color(0.4f, 0.3f, 0.2f)
            );
            CreatePrimitive(
                "TableLeg_BR",
                PrimitiveType.Cube,
                new Vector3(
                    tableWidth / 2 - legWidth / 2,
                    legY,
                    legZ - tableLength / 2 + legWidth / 2
                ),
                new Vector3(legWidth, legHeight, legWidth),
                new Color(0.4f, 0.3f, 0.2f)
            );

            // Crear asientos ejecutivos con ruedas
            for (int i = 0; i < seatCount; i++)
            {
                float seatX = (i - seatCount / 2f + 0.5f) * seatSpacing;
                float seatY = seatHeight / 2;
                float seatZ = tableZ - tableLength / 2 - 0.5f; // Detrás de la mesa

                // Asiento principal
                CreatePrimitive(
                    $"ExecutiveChair_{i}",
                    PrimitiveType.Cube,
                    new Vector3(seatX, seatY, seatZ),
                    new Vector3(0.6f, seatHeight, 0.6f),
                    new Color(0.2f, 0.2f, 0.25f)
                );

                // Respaldo del asiento
                CreatePrimitive(
                    $"ChairBack_{i}",
                    PrimitiveType.Cube,
                    new Vector3(seatX, seatY + seatHeight / 2 + 0.3f, seatZ - 0.2f),
                    new Vector3(0.6f, 0.6f, 0.1f),
                    new Color(0.2f, 0.2f, 0.25f)
                );

                // Brazos del asiento
                CreatePrimitive(
                    $"ChairArmLeft_{i}",
                    PrimitiveType.Cube,
                    new Vector3(seatX - 0.35f, seatY + 0.2f, seatZ),
                    new Vector3(0.1f, 0.2f, 0.6f),
                    new Color(0.15f, 0.15f, 0.2f)
                );
                CreatePrimitive(
                    $"ChairArmRight_{i}",
                    PrimitiveType.Cube,
                    new Vector3(seatX + 0.35f, seatY + 0.2f, seatZ),
                    new Vector3(0.1f, 0.2f, 0.6f),
                    new Color(0.15f, 0.15f, 0.2f)
                );
            }

            // Crear sistema de iluminación LED profesional
            // Luz principal (direccional suave)
            GameObject mainLight = new GameObject("MainLight");
            Light light = mainLight.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.6f;
            light.color = new Color(1f, 0.98f, 0.95f); // Luz cálida pero profesional
            mainLight.transform.rotation = Quaternion.Euler(45f, -30f, 0f);

            // Luces LED de techo (6 luces distribuidas)
            for (int i = 0; i < lightCount; i++)
            {
                GameObject lightGO = new GameObject($"LEDLight_{i}");
                Light l = lightGO.AddComponent<Light>();
                l.type = LightType.Point;
                l.intensity = lightIntensity;
                l.color = new Color(1f, 1f, 0.95f); // Luz LED blanca
                l.range = 6f;

                // Distribuir luces en el techo
                float lightX = (i % 3 - 1) * (roomWidth / 3);
                float lightY = roomHeight - 0.1f;
                float lightZ = (i / 3 - 0.5f) * (roomLength / 2);

                lightGO.transform.position = new Vector3(lightX, lightY, lightZ);
            }

            // Crear luces ambientales adicionales
            CreateAmbientLight(
                "AmbientLight_Left",
                new Vector3(-roomWidth / 2 + 0.5f, roomHeight / 2, 0),
                new Color(0.8f, 0.8f, 0.9f)
            );
            CreateAmbientLight(
                "AmbientLight_Right",
                new Vector3(roomWidth / 2 - 0.5f, roomHeight / 2, 0),
                new Color(0.8f, 0.8f, 0.9f)
            );

            // Crear spawn point (en el stage)
            float spawnX = 0;
            float spawnY = stageHeight + 1.7f;
            float spawnZ = roomLength / 2 - stageDistanceFromFront - stageLength / 2;
            GameObject spawnPoint = new GameObject("SpawnPoint");
            spawnPoint.transform.position = new Vector3(spawnX, spawnY, spawnZ);

            // Crear iluminación profesional
            CreateConferenceLighting(roomWidth, roomLength, roomHeight, tableZ, tableWidth);

            // Crear elementos decorativos ejecutivos
            CreateDecorativeElements(roomWidth, roomLength, roomHeight);

            // Guardar la escena
            string scenePath = "Assets/Scenes/ConferenceScene.unity";
            SaveScene(scenePath);
            AddSceneToBuildSettings(scenePath);

            Debug.Log("✅ Sala de conferencias generada exitosamente en: " + scenePath);
            EditorUtility.DisplayDialog(
                "Escena Generada",
                "La sala de conferencias ha sido generada exitosamente.\n\n"
                    + "Ubicación: Assets/Scenes/ConferenceScene.unity\n\n"
                    + "La escena ha sido agregada automáticamente al Build Settings.",
                "OK"
            );
        }

        private static void CreateWall(string name, Vector3 position, Vector3 scale, Color color)
        {
            CreatePrimitive(name, PrimitiveType.Cube, position, scale, color);
        }

        private static void CreatePrimitive(
            string name,
            PrimitiveType type,
            Vector3 position,
            Vector3 scale,
            Color color
        )
        {
            GameObject obj = GameObject.CreatePrimitive(type);
            obj.name = name;
            obj.transform.position = position;
            obj.transform.localScale = scale;

            // Configurar material usando VRTemplateAssets
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Intentar cargar materiales de VRTemplateAssets
                Material material = null;

                // Buscar material apropiado según el color
                if (color == new Color(0.8f, 0.8f, 0.8f)) // Paredes
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(
                        "Assets/VRTemplateAssets/Materials/Environment/Concrete.mat"
                    );
                }
                else if (color == new Color(0.6f, 0.6f, 0.6f)) // Suelo
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(
                        "Assets/VRTemplateAssets/Materials/Environment/Grey.mat"
                    );
                }
                else if (color == new Color(0.9f, 0.9f, 0.9f)) // Techo
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(
                        "Assets/VRTemplateAssets/Materials/Environment/Concrete.mat"
                    );
                }
                else if (color == new Color(0.6f, 0.4f, 0.2f)) // Stage
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(
                        "Assets/VRTemplateAssets/Materials/Environment/Concrete.mat"
                    );
                }
                else if (color == new Color(0.4f, 0.3f, 0.2f)) // Mesa
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(
                        "Assets/VRTemplateAssets/Materials/Primitive/Interactables.mat"
                    );
                }
                else if (color == new Color(0.2f, 0.2f, 0.25f)) // Sillas
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(
                        "Assets/VRTemplateAssets/Materials/Primitive/Interactables.mat"
                    );
                }
                else if (color == new Color(0.2f, 0.5f, 0.2f)) // Plantas
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(
                        "Assets/VRTemplateAssets/Materials/Environment/Concrete.mat"
                    );
                }

                // Si no se encuentra el material específico, usar uno genérico
                if (material == null)
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(
                        "Assets/VRTemplateAssets/Materials/Environment/Concrete.mat"
                    );
                }

                // Si aún no se encuentra, crear un material temporal
                if (material == null)
                {
                    Debug.LogWarning(
                        $"⚠️ No se pudo cargar material para {name}, creando material temporal"
                    );
                    material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    material.color = color;
                }
                else
                {
                    // Ajustar el color del material si es necesario
                    if (material.HasProperty("_BaseColor"))
                    {
                        material.SetColor("_BaseColor", color);
                    }
                    else if (material.HasProperty("_Color"))
                    {
                        material.SetColor("_Color", color);
                    }
                }

                renderer.material = material;
            }

            // Asegurar que el collider esté configurado correctamente
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;

                // Configurar collider específico según el tipo
                if (name == "Stage")
                {
                    // Para el stage, asegurar que el collider sea sólido
                    if (collider is BoxCollider boxCollider)
                    {
                        boxCollider.size = Vector3.one; // Tamaño normalizado
                        boxCollider.center = Vector3.zero;
                    }
                    Debug.Log($"✅ Collider configurado para {name}");
                }
            }
            else
            {
                Debug.LogError($"❌ No se encontró collider en {name}");
            }
        }

        private static void CreateAmbientLight(string name, Vector3 position, Color color)
        {
            GameObject lightGO = new GameObject(name);
            Light l = lightGO.AddComponent<Light>();
            l.type = LightType.Point;
            l.intensity = 0.3f;
            l.color = color;
            l.range = 4f;
            lightGO.transform.position = position;
        }

        private static void CreateConferenceLighting(
            float roomWidth,
            float roomLength,
            float roomHeight,
            float tableZ,
            float tableWidth
        )
        {
            // Luz principal sobre la mesa de conferencias (2 puntos de luz)
            for (int i = 0; i < 2; i++)
            {
                var tableLightGo = new GameObject($"TableLight_{i}");
                var tableLight = tableLightGo.AddComponent<Light>();
                tableLight.type = LightType.Point;
                tableLight.intensity = 1.0f;
                tableLight.range = roomWidth / 1.5f;
                tableLight.color = new Color(0.9f, 0.95f, 1f); // Tono neutro-frío, profesional
                tableLight.shadows = LightShadows.Soft;

                float lightX = (i == 0) ? -tableWidth / 4 : tableWidth / 4;
                tableLightGo.transform.position = new Vector3(lightX, roomHeight - 0.5f, tableZ);
            }

            // Luz suave sobre el presentador/stage
            var stageLightGo = new GameObject("PresenterLight");
            var stageLight = stageLightGo.AddComponent<Light>();
            stageLight.type = LightType.Spot;
            stageLight.intensity = 1.5f;
            stageLight.range = 10f;
            stageLight.spotAngle = 60f;
            stageLight.color = new Color(1f, 0.95f, 0.9f); // Tono ligeramente cálido para el presentador
            stageLight.shadows = LightShadows.Soft;
            stageLightGo.transform.position = new Vector3(
                0,
                roomHeight - 0.5f,
                roomLength / 2 - 3f
            );
            stageLightGo.transform.rotation = Quaternion.Euler(45, 0, 0);

            // Pequeña luz de relleno ambiental
            var ambientFillGo = new GameObject("AmbientFillLight");
            var ambientFill = ambientFillGo.AddComponent<Light>();
            ambientFill.type = LightType.Point;
            ambientFill.intensity = 0.3f;
            ambientFill.range = roomLength;
            ambientFill.color = new Color(0.8f, 0.85f, 0.9f); // Tono muy suave
            ambientFill.shadows = LightShadows.None;
            ambientFillGo.transform.position = new Vector3(0, roomHeight / 2, 0);
        }

        private static void CreateDecorativeElements(
            float roomWidth,
            float roomLength,
            float roomHeight
        )
        {
            float wallThickness = 0.2f; // Definir wallThickness localmente

            // Plantas decorativas
            CreatePrimitive(
                "Plant_Left",
                PrimitiveType.Cylinder,
                new Vector3(-roomWidth / 2 + 0.5f, 0.5f, -roomLength / 2 + 1f),
                new Vector3(0.3f, 1f, 0.3f),
                new Color(0.2f, 0.5f, 0.2f)
            );
            CreatePrimitive(
                "Plant_Right",
                PrimitiveType.Cylinder,
                new Vector3(roomWidth / 2 - 0.5f, 0.5f, -roomLength / 2 + 1f),
                new Vector3(0.3f, 1f, 0.3f),
                new Color(0.2f, 0.5f, 0.2f)
            );

            // Macetas
            CreatePrimitive(
                "Pot_Left",
                PrimitiveType.Cylinder,
                new Vector3(-roomWidth / 2 + 0.5f, 0.25f, -roomLength / 2 + 1f),
                new Vector3(0.4f, 0.5f, 0.4f),
                new Color(0.6f, 0.4f, 0.3f)
            );
            CreatePrimitive(
                "Pot_Right",
                PrimitiveType.Cylinder,
                new Vector3(roomWidth / 2 - 0.5f, 0.25f, -roomLength / 2 + 1f),
                new Vector3(0.4f, 0.5f, 0.4f),
                new Color(0.6f, 0.4f, 0.3f)
            );

            // Cuadros en las paredes
            CreatePrimitive(
                "Painting_Left",
                PrimitiveType.Cube,
                new Vector3(-roomWidth / 2 + wallThickness / 2, roomHeight / 2, 0),
                new Vector3(0.05f, 1f, 1.5f),
                new Color(0.8f, 0.7f, 0.6f)
            );
            CreatePrimitive(
                "Painting_Right",
                PrimitiveType.Cube,
                new Vector3(roomWidth / 2 - wallThickness / 2, roomHeight / 2, 0),
                new Vector3(0.05f, 1f, 1.5f),
                new Color(0.8f, 0.7f, 0.6f)
            );

            // Marco de los cuadros
            CreatePrimitive(
                "Frame_Left",
                PrimitiveType.Cube,
                new Vector3(-roomWidth / 2 + wallThickness / 2, roomHeight / 2, 0),
                new Vector3(0.1f, 1.1f, 1.6f),
                new Color(0.4f, 0.3f, 0.2f)
            );
            CreatePrimitive(
                "Frame_Right",
                PrimitiveType.Cube,
                new Vector3(roomWidth / 2 - wallThickness / 2, roomHeight / 2, 0),
                new Vector3(0.1f, 1.1f, 1.6f),
                new Color(0.4f, 0.3f, 0.2f)
            );
        }

        private static void CreateDirectionalLighting()
        {
            // Luz principal
            GameObject mainLight = new GameObject("DirectionalLight_Main");
            Light light1 = mainLight.AddComponent<Light>();
            light1.type = LightType.Directional;
            light1.intensity = 0.9f;
            light1.color = new Color(1f, 0.98f, 0.95f);
            mainLight.transform.rotation = Quaternion.Euler(55f, -10f, 0f);

            // Luz de rebote lateral
            GameObject sideLight = new GameObject("DirectionalLight_Side");
            Light light2 = sideLight.AddComponent<Light>();
            light2.type = LightType.Directional;
            light2.intensity = 0.3f;
            light2.color = new Color(0.95f, 0.95f, 1f);
            sideLight.transform.rotation = Quaternion.Euler(0f, 80f, 0f);
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
    }
}
