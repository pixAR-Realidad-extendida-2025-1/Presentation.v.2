using UnityEditor;
using UnityEngine;

namespace VRTemplate.Editor
{
    /// <summary>
    /// Editor utility para corregir materiales que aparecen negros
    /// </summary>
    public class MaterialFixerEditor : EditorWindow
    {
        [MenuItem("VR Simulador/Corregir Materiales Negros", priority = 11)]
        public static void FixBlackMaterials()
        {
            Debug.Log("🎨 Corrigiendo materiales negros...");

            // Lista de materiales a verificar y corregir
            string[] materialPaths =
            {
                "Assets/VRTemplateAssets/Materials/Environment/Concrete.mat",
                "Assets/VRTemplateAssets/Materials/Environment/Grey.mat",
                "Assets/VRTemplateAssets/Materials/Primitive/Interactables.mat",
                "Assets/VRTemplateAssets/Materials/Environment/Concrete Blue.mat",
                "Assets/VRTemplateAssets/Materials/Environment/Concrete Grey.mat",
                "Assets/VRTemplateAssets/Materials/Environment/Wall Default.mat",
            };

            int fixedCount = 0;

            foreach (string materialPath in materialPaths)
            {
                Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                if (material != null)
                {
                    bool wasFixed = FixMaterial(material, materialPath);
                    if (wasFixed)
                    {
                        fixedCount++;
                    }
                }
                else
                {
                    Debug.LogWarning($"⚠️ Material no encontrado: {materialPath}");
                }
            }

            // Verificar materiales en la escena actual
            Renderer[] renderers = FindObjectsOfType<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer.material != null)
                {
                    bool wasFixed = FixMaterialInScene(renderer);
                    if (wasFixed)
                    {
                        fixedCount++;
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"✅ Materiales corregidos: {fixedCount}");
            EditorUtility.DisplayDialog(
                "Materiales Corregidos",
                $"Se han corregido {fixedCount} materiales que aparecían negros.\n\n"
                    + "Los materiales ahora deberían verse con colores apropiados.",
                "OK"
            );
        }

        private static bool FixMaterial(Material material, string materialPath)
        {
            bool wasFixed = false;

            // Verificar si el material tiene color base asignado
            if (material.HasProperty("_BaseColor"))
            {
                Color baseColor = material.GetColor("_BaseColor");
                if (baseColor.r < 0.1f && baseColor.g < 0.1f && baseColor.b < 0.1f)
                {
                    // Asignar color apropiado según el tipo de material
                    Color newColor = GetAppropriateColor(materialPath);
                    material.SetColor("_BaseColor", newColor);
                    wasFixed = true;
                    Debug.Log($"✅ Corregido material: {materialPath} - Color: {newColor}");
                }
            }
            else if (material.HasProperty("_Color"))
            {
                Color color = material.GetColor("_Color");
                if (color.r < 0.1f && color.g < 0.1f && color.b < 0.1f)
                {
                    Color newColor = GetAppropriateColor(materialPath);
                    material.SetColor("_Color", newColor);
                    wasFixed = true;
                    Debug.Log($"✅ Corregido material: {materialPath} - Color: {newColor}");
                }
            }

            // Verificar si el material tiene textura principal
            if (material.HasProperty("_MainTex") && material.GetTexture("_MainTex") == null)
            {
                // Asignar textura por defecto si es necesario
                Texture2D defaultTexture = CreateDefaultTexture();
                material.SetTexture("_MainTex", defaultTexture);
                wasFixed = true;
                Debug.Log($"✅ Asignada textura por defecto a: {materialPath}");
            }

            return wasFixed;
        }

        private static bool FixMaterialInScene(Renderer renderer)
        {
            bool wasFixed = false;
            Material material = renderer.material;

            // Verificar si el material es negro
            if (material.HasProperty("_BaseColor"))
            {
                Color baseColor = material.GetColor("_BaseColor");
                if (baseColor.r < 0.1f && baseColor.g < 0.1f && baseColor.b < 0.1f)
                {
                    // Asignar color basado en el nombre del objeto
                    Color newColor = GetColorByObjectName(renderer.gameObject.name);
                    material.SetColor("_BaseColor", newColor);
                    wasFixed = true;
                    Debug.Log(
                        $"✅ Corregido material en escena: {renderer.gameObject.name} - Color: {newColor}"
                    );
                }
            }

            return wasFixed;
        }

        private static Color GetAppropriateColor(string materialPath)
        {
            if (materialPath.Contains("Concrete"))
            {
                return new Color(0.7f, 0.7f, 0.7f); // Gris claro
            }
            else if (materialPath.Contains("Grey"))
            {
                return new Color(0.6f, 0.6f, 0.6f); // Gris medio
            }
            else if (materialPath.Contains("Interactables"))
            {
                return new Color(0.8f, 0.8f, 0.8f); // Gris claro
            }
            else if (materialPath.Contains("Wall"))
            {
                return new Color(0.8f, 0.8f, 0.8f); // Gris claro
            }
            else
            {
                return new Color(0.7f, 0.7f, 0.7f); // Gris por defecto
            }
        }

        private static Color GetColorByObjectName(string objectName)
        {
            if (objectName.Contains("Wall"))
            {
                return new Color(0.8f, 0.8f, 0.8f); // Gris claro
            }
            else if (objectName.Contains("Floor") || objectName.Contains("Ceiling"))
            {
                return new Color(0.6f, 0.6f, 0.6f); // Gris medio
            }
            else if (objectName.Contains("Stage"))
            {
                return new Color(0.6f, 0.4f, 0.2f); // Marrón
            }
            else if (objectName.Contains("Seat") || objectName.Contains("Chair"))
            {
                return new Color(0.3f, 0.3f, 0.3f); // Gris oscuro
            }
            else
            {
                return new Color(0.7f, 0.7f, 0.7f); // Gris por defecto
            }
        }

        private static Texture2D CreateDefaultTexture()
        {
            // Crear una textura simple de 1x1 píxel
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            return texture;
        }
    }
}
