using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplaceSeatsWithProbability : EditorWindow
{
    private GameObject sittingOnChairPrefab;
    private GameObject chairPrefab;
    private float chairScale = 0.003f;
    private float occupiedProbability = 0.3f;

    [MenuItem("Herramientas/Reemplazar Seats con probabilidad (Auditorio)")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceSeatsWithProbability>("Reemplazar Seats Auditorio");
    }

    private void OnGUI()
    {
        GUILayout.Label("Reemplazo de Seats con probabilidad", EditorStyles.boldLabel);

        sittingOnChairPrefab = (GameObject)
            EditorGUILayout.ObjectField(
                "Sitting on chair Prefab",
                sittingOnChairPrefab,
                typeof(GameObject),
                false
            );
        chairPrefab = (GameObject)
            EditorGUILayout.ObjectField(
                "Chair Prefab (vacía)",
                chairPrefab,
                typeof(GameObject),
                false
            );
        chairScale = EditorGUILayout.FloatField("Escala de la silla", chairScale);
        occupiedProbability = EditorGUILayout.Slider(
            "Probabilidad de ocupado",
            occupiedProbability,
            0f,
            1f
        );

        if (GUILayout.Button("Reemplazar en AuditoriumScene"))
        {
            if (sittingOnChairPrefab == null || chairPrefab == null)
            {
                EditorUtility.DisplayDialog("Error", "Asigna ambos prefabs.", "OK");
                return;
            }
            ReplaceSeatsInAuditorium();
        }
    }

    private void ReplaceSeatsInAuditorium()
    {
        string scenePath = "Assets/Scenes/AuditoriumScene.unity";
        if (!System.IO.File.Exists(scenePath))
        {
            EditorUtility.DisplayDialog("Error", "No se encontró la escena de auditorio.", "OK");
            return;
        }

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            int replaced = ReplaceSeatsInScene(scene);
            EditorSceneManager.SaveScene(scene);
            EditorUtility.DisplayDialog(
                "Listo",
                $"¡Reemplazo completado en AuditoriumScene!\nObjetos reemplazados: {replaced}",
                "OK"
            );
        }
    }

    private int ReplaceSeatsInScene(Scene scene)
    {
        var allObjects = scene.GetRootGameObjects();
        Regex seatPattern = new Regex(@"^Seat_R\d+_C\d+$");
        int count = 0;

        foreach (var obj in allObjects)
        {
            if (seatPattern.IsMatch(obj.name))
            {
                Vector3 pos = obj.transform.position;
                Quaternion rot = obj.transform.rotation;
                Vector3 scale = obj.transform.localScale;
                Transform parent = obj.transform.parent;

                GameObject prefabToUse =
                    (Random.value < occupiedProbability) ? sittingOnChairPrefab : chairPrefab;

                GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefabToUse, scene);
                newObj.transform.position = pos;
                newObj.transform.rotation = rot;
                newObj.transform.localScale = Vector3.one * chairScale;
                newObj.transform.SetParent(parent);

                DestroyImmediate(obj);
                count++;
            }
        }
        return count;
    }
}
