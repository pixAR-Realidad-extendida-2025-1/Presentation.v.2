using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplaceSeatsWithSittingOnChair : EditorWindow
{
    private GameObject sittingOnChairPrefab;

    [MenuItem("Herramientas/Reemplazar Seats por Sitting on chair (Auditorio)")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceSeatsWithSittingOnChair>("Reemplazar Seats Auditorio");
    }

    private void OnGUI()
    {
        GUILayout.Label("Reemplazo de Seats por Sitting on chair", EditorStyles.boldLabel);

        sittingOnChairPrefab = (GameObject)
            EditorGUILayout.ObjectField(
                "Sitting on chair Prefab",
                sittingOnChairPrefab,
                typeof(GameObject),
                false
            );

        if (GUILayout.Button("Reemplazar en AuditoriumScene"))
        {
            if (sittingOnChairPrefab == null)
            {
                EditorUtility.DisplayDialog("Error", "Asigna el prefab 'Sitting on chair'.", "OK");
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

                // Instanciar el prefab de la persona sentada en la silla
                GameObject newSitting = (GameObject)
                    PrefabUtility.InstantiatePrefab(sittingOnChairPrefab, scene);
                newSitting.transform.position = pos;
                newSitting.transform.rotation = rot;
                newSitting.transform.localScale = scale; // Mantén la escala original del asiento
                newSitting.transform.SetParent(parent);

                // Eliminar el objeto original
                DestroyImmediate(obj);
                count++;
            }
        }
        return count;
    }
}
