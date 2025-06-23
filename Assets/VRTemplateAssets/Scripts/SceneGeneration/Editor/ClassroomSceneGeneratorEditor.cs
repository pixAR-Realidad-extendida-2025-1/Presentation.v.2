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
            int lightRows = 4;
            float lightIntensity = 1f;

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

            // Crear iluminación
            GameObject mainLight = new GameObject("MainLight");
            Light light = mainLight.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.8f;
            light.color = Color.white;
            mainLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            for (int row = 0; row < lightRows; row++)
            {
                GameObject lightGO = new GameObject($"CeilingLight_Row{row}");
                Light l = lightGO.AddComponent<Light>();
                l.type = LightType.Point;
                l.intensity = lightIntensity;
                l.color = new Color(1f, 0.95f, 0.8f);
                l.range = 8f;
                float lightX = 0;
                float lightY = roomHeight - 0.2f;
                float lightZ = (row - lightRows / 2f + 0.5f) * (roomLength / lightRows);
                lightGO.transform.position = new Vector3(lightX, lightY, lightZ);
            }

            // Crear spawn point (solo objeto vacío)
            float spawnX = 0;
            float spawnY = stageHeight + 1.7f;
            float spawnZ = roomLength / 2 - stageDistanceFromFront - stageLength / 2;
            GameObject spawnPoint = new GameObject("SpawnPoint");
            spawnPoint.transform.position = new Vector3(spawnX, spawnY, spawnZ);

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
            obj.GetComponent<Renderer>().material.color = color;
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
