using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRTemplate.SceneGeneration.Editor
{
    /// <summary>
    /// Editor utility para generar la escena de sala de clases automáticamente
    /// </summary>
    public class ClassroomSceneGeneratorEditor : EditorWindow
    {
        [MenuItem("VR Simulador/Generar Sala de Clases", priority = 2)]
        public static void GenerateClassroomScene()
        {
            if (
                EditorUtility.DisplayDialog(
                    "Generar Sala de Clases",
                    "¿Deseas generar automáticamente la escena de sala de clases?",
                    "Sí",
                    "No"
                )
            )
            {
                CreateClassroomScene();
            }
        }

        private static void CreateClassroomScene()
        {
            Debug.Log("🏫 Generando escena de sala de clases...");

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

            // Parámetros de la sala
            float roomWidth = 8f;
            float roomLength = 12f;
            float roomHeight = 3.5f;
            float wallThickness = 0.2f;
            float stageWidth = 3f;
            float stageLength = 2f;
            float stageHeight = 0.3f;
            float stageDistanceFromFront = 1.5f;
            float screenWidth = 3f;
            float screenHeight = 2f;
            float screenHeightFromFloor = 1.2f;
            int seatRows = 4;
            int seatColumns = 6;
            float seatSpacingX = 0.6f;
            float seatSpacingZ = 0.8f;
            float firstRowDistance = 2.5f;

            // Crear paredes
            CreateWall(
                "Wall_Front",
                new Vector3(0, roomHeight / 2, roomLength / 2),
                new Vector3(roomWidth, roomHeight, wallThickness)
            );
            CreateWall(
                "Wall_Back",
                new Vector3(0, roomHeight / 2, -roomLength / 2),
                new Vector3(roomWidth, roomHeight, wallThickness)
            );
            CreateWall(
                "Wall_Left",
                new Vector3(-roomWidth / 2, roomHeight / 2, 0),
                new Vector3(wallThickness, roomHeight, roomLength)
            );
            CreateWall(
                "Wall_Right",
                new Vector3(roomWidth / 2, roomHeight / 2, 0),
                new Vector3(wallThickness, roomHeight, roomLength)
            );

            // Crear suelo
            CreatePrimitive(
                "Floor",
                PrimitiveType.Cube,
                new Vector3(0, 0, 0),
                new Vector3(roomWidth, 0.1f, roomLength),
                new Color(0.6f, 0.6f, 0.6f)
            );
            // Crear techo
            CreatePrimitive(
                "Ceiling",
                PrimitiveType.Cube,
                new Vector3(0, roomHeight, 0),
                new Vector3(roomWidth, 0.1f, roomLength),
                new Color(0.9f, 0.9f, 0.9f)
            );

            // Crear stage
            float stageX = 0;
            float stageY = stageHeight / 2;
            float stageZ = roomLength / 2 - stageDistanceFromFront - stageLength / 2;
            CreatePrimitive(
                "Stage",
                PrimitiveType.Cube,
                new Vector3(stageX, stageY, stageZ),
                new Vector3(stageWidth, stageHeight, stageLength),
                new Color(0.6f, 0.4f, 0.2f)
            );

            // Crear pantalla
            float screenX = 0;
            float screenY = screenHeightFromFloor + screenHeight / 2;
            float screenZ = roomLength / 2 - wallThickness / 2;
            CreatePrimitive(
                "Screen",
                PrimitiveType.Cube,
                new Vector3(screenX, screenY, screenZ),
                new Vector3(screenWidth, screenHeight, 0.1f),
                Color.black
            );

            // Crear asientos
            for (int row = 0; row < seatRows; row++)
            {
                for (int col = 0; col < seatColumns; col++)
                {
                    float seatX = (col - seatColumns / 2f + 0.5f) * seatSpacingX;
                    float seatY = 0.4f;
                    float seatZ = -firstRowDistance - row * seatSpacingZ;
                    CreatePrimitive(
                        $"Seat_R{row}_C{col}",
                        PrimitiveType.Cube,
                        new Vector3(seatX, seatY, seatZ),
                        new Vector3(0.5f, 0.8f, 0.5f),
                        new Color(0.3f, 0.3f, 0.3f)
                    );
                }
            }

            // Crear iluminación de la sala
            CreateClassroomLighting(roomWidth, roomLength, roomHeight);

            // Crear spawn point (solo objeto vacío)
            float spawnX = 0;
            float spawnY = stageHeight + 1.7f;
            float spawnZ = roomLength / 2 - stageDistanceFromFront - stageLength / 2;
            GameObject spawnPoint = new GameObject("SpawnPoint");
            spawnPoint.transform.position = new Vector3(spawnX, spawnY, spawnZ);

            // Iluminación direccional múltiple
            CreateDirectionalLighting();

            // Guardar la escena
            string scenePath = "Assets/Scenes/ClassroomScene.unity";
            SaveScene(scenePath);
            AddSceneToBuildSettings(scenePath);

            Debug.Log("✅ Sala de clases generada exitosamente en: " + scenePath);
            EditorUtility.DisplayDialog(
                "Escena Generada",
                "La sala de clases ha sido generada exitosamente.\n\nUbicación: Assets/Scenes/ClassroomScene.unity\n\nLa escena ha sido agregada automáticamente al Build Settings.",
                "OK"
            );
        }

        private static void CreateClassroomLighting(
            float roomWidth,
            float roomLength,
            float roomHeight
        )
        {
            // Colocar 2 filas de 2 luces de punto para una iluminación uniforme
            int lightRows = 2;
            int lightCols = 2;
            float lightIntensity = 1.2f;
            float lightRange = Mathf.Max(roomWidth, roomLength) / 2.5f;

            for (int r = 0; r < lightRows; r++)
            {
                for (int c = 0; c < lightCols; c++)
                {
                    GameObject lightGo = new GameObject($"CeilingLight_R{r}_C{c}");
                    Light light = lightGo.AddComponent<Light>();
                    light.type = LightType.Point;
                    light.intensity = lightIntensity;
                    light.range = lightRange;
                    light.color = new Color(1f, 0.95f, 0.85f); // Tono cálido
                    light.shadows = LightShadows.Soft;

                    float lightX = (c - (lightCols - 1) / 2f) * (roomWidth / lightCols);
                    float lightY = roomHeight - 0.3f;
                    float lightZ = (r - (lightRows - 1) / 2f) * (roomLength / lightRows);
                    lightGo.transform.position = new Vector3(lightX, lightY, lightZ);
                }
            }
        }

        private static void CreateDirectionalLighting()
        {
            // Luz principal
            GameObject mainLight = new GameObject("DirectionalLight_Main");
            Light light1 = mainLight.AddComponent<Light>();
            light1.type = LightType.Directional;
            light1.intensity = 1.0f;
            light1.color = new Color(1f, 0.98f, 0.95f);
            mainLight.transform.rotation = Quaternion.Euler(60f, -20f, 0f);

            // Luz de rebote
            GameObject fillLight = new GameObject("DirectionalLight_Fill");
            Light light2 = fillLight.AddComponent<Light>();
            light2.type = LightType.Directional;
            light2.intensity = 0.3f;
            light2.color = new Color(0.9f, 0.95f, 1f);
            fillLight.transform.rotation = Quaternion.Euler(320f, 20f, 0f);
        }

        private static void CreateWall(string name, Vector3 position, Vector3 scale)
        {
            CreatePrimitive(name, PrimitiveType.Cube, position, scale, new Color(0.8f, 0.8f, 0.8f));
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
                else if (color == Color.black) // Pantalla
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(
                        "Assets/VRTemplateAssets/Materials/Environment/Concrete.mat"
                    );
                }
                else if (color == new Color(0.3f, 0.3f, 0.3f)) // Asientos
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(
                        "Assets/VRTemplateAssets/Materials/Primitive/Interactables.mat"
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
