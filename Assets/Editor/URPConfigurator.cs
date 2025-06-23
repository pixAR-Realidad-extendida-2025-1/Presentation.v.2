using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class URPConfigurator : EditorWindow
{
    [MenuItem("Herramientas/Configurar URP para Iluminación Interior")]
    public static void ConfigureURP()
    {
        // Busca el asset de URP en el proyecto
        string[] guids = AssetDatabase.FindAssets("t:UniversalRenderPipelineAsset");
        if (guids.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "URP Config",
                "No se encontró ningún UniversalRenderPipelineAsset en el proyecto.",
                "OK"
            );
            return;
        }

        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var urpAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(path);

        if (urpAsset == null)
        {
            EditorUtility.DisplayDialog(
                "URP Config",
                "No se pudo cargar el UniversalRenderPipelineAsset.",
                "OK"
            );
            return;
        }

        // Solo se pueden cambiar estas propiedades por script
        urpAsset.maxAdditionalLightsCount = 8; // Puedes subirlo si tienes muchas luces
        urpAsset.supportsCameraDepthTexture = true;
        urpAsset.supportsCameraOpaqueTexture = true;
        urpAsset.supportsHDR = true;
        urpAsset.shadowDistance = 50f;

        EditorUtility.SetDirty(urpAsset);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog(
            "URP Config",
            "¡Configuración de URP aplicada!\n\n- Additional Lights: 8\n- HDR: Activado\n- Sombras: Activadas\n\nRecuerda: El modo Per Pixel de las luces se debe ajustar manualmente en el Inspector del asset URP.",
            "OK"
        );
    }
}
