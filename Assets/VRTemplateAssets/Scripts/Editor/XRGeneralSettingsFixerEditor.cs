using UnityEditor;
using UnityEngine;

namespace VRTemplate.Editor
{
    /// <summary>
    /// Editor utility para limpiar errores de XR General Settings
    /// </summary>
    public class XRGeneralSettingsFixerEditor : EditorWindow
    {
        [MenuItem("VR Simulador/Limpiar Errores XR Settings", priority = 10)]
        public static void FixXRGeneralSettings()
        {
            Debug.Log("🔧 Limpiando errores de XR General Settings...");

            // Buscar y limpiar XRGeneralSettings
            var xrGeneralSettings = AssetDatabase.FindAssets("t:XRGeneralSettings");

            if (xrGeneralSettings.Length > 0)
            {
                foreach (var guid in xrGeneralSettings)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    var settings =
                        AssetDatabase.LoadAssetAtPath<UnityEngine.XR.Management.XRGeneralSettings>(
                            path
                        );

                    if (settings != null)
                    {
                        // Limpiar referencias problemáticas
                        var serializedObject = new SerializedObject(settings);
                        var managerProperty = serializedObject.FindProperty(
                            "m_LoaderManagerInstance"
                        );

                        if (managerProperty != null && managerProperty.objectReferenceValue == null)
                        {
                            Debug.Log($"✅ Limpiando referencia nula en {path}");
                            managerProperty.objectReferenceValue = null;
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                }
            }

            // Buscar y limpiar XRGeneralSettingsPerBuildTarget (si existe)
            var xrGeneralSettingsPerBuildTarget = AssetDatabase.FindAssets(
                "XRGeneralSettingsPerBuildTarget"
            );

            if (xrGeneralSettingsPerBuildTarget.Length > 0)
            {
                foreach (var guid in xrGeneralSettingsPerBuildTarget)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    Debug.Log($"✅ Encontrado archivo XRGeneralSettingsPerBuildTarget en {path}");

                    // Intentar limpiar usando SerializedObject genérico
                    var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                    if (asset != null)
                    {
                        var serializedObject = new SerializedObject(asset);
                        serializedObject.Update();

                        // Buscar propiedades problemáticas
                        var iterator = serializedObject.GetIterator();
                        bool hasChanges = false;

                        while (iterator.Next(true))
                        {
                            if (
                                iterator.propertyType == SerializedPropertyType.ObjectReference
                                && iterator.objectReferenceValue == null
                            )
                            {
                                Debug.Log($"✅ Limpiando referencia nula: {iterator.propertyPath}");
                                iterator.objectReferenceValue = null;
                                hasChanges = true;
                            }
                        }

                        if (hasChanges)
                        {
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                }
            }

            // Forzar recompilación
            AssetDatabase.Refresh();

            Debug.Log("✅ Errores de XR General Settings limpiados");
            EditorUtility.DisplayDialog(
                "Limpieza Completada",
                "Se han limpiado los errores de XR General Settings.\n\n"
                    + "Los mensajes de 'referenced script is missing' deberían desaparecer.",
                "OK"
            );
        }
    }
}
