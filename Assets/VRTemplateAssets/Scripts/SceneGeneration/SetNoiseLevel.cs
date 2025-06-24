using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPresentationTrainer.Core.Data;
using TMPro;

public class SetNoiseLevel : MonoBehaviour
{
    public VRConfigSettings config;

    public void SetLevel(string level)
    {
        int intLevel = Mathf.RoundToInt(float.Parse(level));
        Debug.Log("Valor recibido del slider: " + intLevel);
        config.environment.noiseLevel = (VRConfigSettings.EnvironmentSettings.NoiseLevel)intLevel;
        Debug.Log("Nivel de ruido actualizado a: " + config.environment.noiseLevel);

    }

    // Funcion para obtener el nivel de ruido actual desde un dropdown
    public void SetLevelFromDropdown(int level)
    {
        Debug.Log("Valor recibido del dropdown: " + level);
        config.environment.noiseLevel = (VRConfigSettings.EnvironmentSettings.NoiseLevel)level;
        Debug.Log("Nivel de ruido actualizado a: " + config.environment.noiseLevel);
    }

    public void SetLevelSwitch(int index)
    {
        switch (index)
        {
            case 0:
                config.environment.noiseLevel = VRConfigSettings.EnvironmentSettings.NoiseLevel.Ninguno;
                break;
            case 1:
                config.environment.noiseLevel = VRConfigSettings.EnvironmentSettings.NoiseLevel.Bajo;
                break;
            case 2:
                config.environment.noiseLevel = VRConfigSettings.EnvironmentSettings.NoiseLevel.Medio;
                break;
            case 3:
                config.environment.noiseLevel = VRConfigSettings.EnvironmentSettings.NoiseLevel.Alto;
                break;
            default:
                Debug.LogWarning("Índice de nivel de ruido no válido: " + index);
                break;
        }
        Debug.Log("Nivel de ruido actualizado a: " + config.environment.noiseLevel);
    }
}
