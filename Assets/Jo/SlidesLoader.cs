using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEngine.InputSystem;



public class SlideLoader : MonoBehaviour
{
    private string tempOutputFolder; // carpeta donde se guardan las imágenes convertidas

    [Header("Configuración Visual")]
    public Renderer pantalla; // Arrastra tu Quad aquí
    public float transitionSpeed = 1f;

    [Header("Controles")]
    public KeyCode nextKey = KeyCode.RightArrow;
    public KeyCode prevKey = KeyCode.LeftArrow;
    public KeyCode openKey = KeyCode.Return;

    private List<Texture2D> slides = new List<Texture2D>();
    private int currentIndex = 0;
    private bool isTransitioning = false;



    void Start()
    {
        UnityEngine.Debug.Log("=== PRUEBA DE CONFIGURACIÓN ===");

        if (pantalla == null) // Cambié "Pantalla" a "targetScreen" para coincidir con tu script
        {
            UnityEngine.Debug.LogError("No hay pantalla asignada!");
        }
        else
        {
            UnityEngine.Debug.Log("Pantalla asignada correctamente: " + pantalla.name);

            // Test visual
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.cyan);
            tex.Apply();
            pantalla.material.SetTexture("_BaseMap", tex);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(openKey)) OpenFileBrowser();

        if (!isTransitioning)
        {
            // Teclado
            if (Input.GetKeyDown(nextKey)) NextSlide();
            if (Input.GetKeyDown(prevKey)) PreviousSlide();

            // Controles de Quest (Input System)
            if (Gamepad.current != null)
            {
                if (Gamepad.current.buttonSouth.wasPressedThisFrame) // A
                    NextSlide();
                if (Gamepad.current.buttonWest.wasPressedThisFrame) // X
                    PreviousSlide();
                if (Gamepad.current.buttonEast.wasPressedThisFrame)  // B
                    OpenFileBrowser();
                if (Gamepad.current.rightShoulder.wasPressedThisFrame)
                    UnityEngine.Debug.Log("Grip derecho presionado");
                if (Gamepad.current.rightTrigger.wasPressedThisFrame)
                    UnityEngine.Debug.Log("Trigger derecho presionado");
                if (Gamepad.current.leftStickButton.wasPressedThisFrame)
                    UnityEngine.Debug.Log("Joystick izquierdo clicado");
            }
        }
    }


    void ConfigureScreen()
    {
        if (pantalla != null)
        {
            // pantalla.material = new Material(Shader.Find("Unlit/Texture"));
            UnityEngine.Debug.Log("Pantalla configurada correctamente");
        }
        else UnityEngine.Debug.LogError("Asigna el Quad en el Inspector!");
    }

    public void OpenFileBrowser()
    {
        FileBrowser.ShowLoadDialog(
            (paths) =>
            {
                string path = paths[0];

                if (path.ToLower().EndsWith(".pdf"))
                {
                    ConvertPDFToImages(path);
                }
                else if (Directory.Exists(path))
                {
                    LoadFolder(path);
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Archivo no soportado.");
                }
            },
            () => { UnityEngine.Debug.Log("Operación cancelada"); },
            FileBrowser.PickMode.FilesAndFolders,
            false,
            null,
            null,
            "Selecciona carpeta o PDF",
            "Abrir"
        );
    }


    void LoadFolder(string folderPath)
    {
        StartCoroutine(LoadImagesCoroutine(folderPath));
    }

    IEnumerator LoadImagesCoroutine(string folderPath)
    {
        slides.Clear();

        var imageFiles = Directory.GetFiles(folderPath)
            .Where(f => f.ToLower().EndsWith(".jpg") ||
                       f.ToLower().EndsWith(".png") ||
                       f.ToLower().EndsWith(".jpeg"))
            .OrderBy(f => f)
            .ToArray();

        if (imageFiles.Length == 0)
        {
            UnityEngine.Debug.LogWarning("No hay imágenes en: " + folderPath);
            yield break;
        }

        foreach (var file in imageFiles)
        {
            Texture2D tex = new Texture2D(2, 2);
            byte[] bytes = File.ReadAllBytes(file);

            if (tex.LoadImage(bytes))
            {
                slides.Add(tex);
                UnityEngine.Debug.Log("Cargada: " + Path.GetFileName(file));
            }
            yield return null;
        }

        if (slides.Count > 0) ShowSlide(0);
    }

    void ShowSlide(int index)
    {
        currentIndex = (index + slides.Count) % slides.Count;

        // 🔄 Esto es lo que sí funciona en URP:
        pantalla.material.SetTexture("_BaseMap", slides[currentIndex]);

        UnityEngine.Debug.Log($"Slide {currentIndex + 1}/{slides.Count}");
    }


    public void NextSlide()
    {
        if (slides.Count == 0) return;
        StartCoroutine(TransitionSlide(1));
    }

    public void PreviousSlide()
    {
        if (slides.Count == 0) return;
        StartCoroutine(TransitionSlide(-1));
    }

    IEnumerator TransitionSlide(int direction)
    {
        isTransitioning = true;
        // Efecto de transición opcional (puedes eliminarlo si quieres cambio instantáneo)
        float fadeTime = 0.5f;
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            pantalla.material.color = Color.Lerp(Color.white, new Color(0.5f, 0.5f, 0.5f), elapsed / fadeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ShowSlide(currentIndex + direction);

        elapsed = 0f;
        while (elapsed < fadeTime)
        {
            pantalla.material.color = Color.Lerp(new Color(0.5f, 0.5f, 0.5f), Color.white, elapsed / fadeTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isTransitioning = false;
    }

    void ConvertPDFToImages(string pdfPath)
    {
        // Carpeta temporal para las imágenes
        tempOutputFolder = Path.Combine(Application.temporaryCachePath, "pdf_slides");

        // Borra la carpeta si ya existe
        if (Directory.Exists(tempOutputFolder))
            Directory.Delete(tempOutputFolder, true);

        Directory.CreateDirectory(tempOutputFolder);

        string outputPattern = Path.Combine(tempOutputFolder, "slide_%d.png");

        // Configurar el proceso
        string mutoolPath = FindMutoolPath();
        if (string.IsNullOrEmpty(mutoolPath))
        {
            UnityEngine.Debug.LogError("mutool no está disponible, no se puede continuar.");
            return;
        }

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = mutoolPath,

            Arguments = $"convert -o \"{outputPattern}\" \"{pdfPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process proc = new Process();
        proc.StartInfo = psi;
        proc.Start();

        string output = proc.StandardOutput.ReadToEnd();
        string error = proc.StandardError.ReadToEnd();

        proc.WaitForExit();

        if (proc.ExitCode == 0)
        {
            UnityEngine.Debug.Log("PDF convertido correctamente.");
            StartCoroutine(WaitForImagesThenLoad(tempOutputFolder));
        }
        else
        {
            UnityEngine.Debug.LogError("Error al convertir PDF: " + error);
        }
    }

    IEnumerator WaitForImagesThenLoad(string folder)
    {
        // Esperar medio segundo para asegurar que las imágenes estén listas
        yield return new WaitForSeconds(0.5f);
        LoadFolder(folder);
    }

    string FindMutoolPath()
    {
        try
        {
            string mutoolPath = null;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "where",
            Arguments = "mutool.exe",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = System.Diagnostics.Process.Start(psi);
        mutoolPath = process.StandardOutput.ReadToEnd().Trim().Split('\n').FirstOrDefault();
        process.WaitForExit();

        // Fallback manual (por si no está en el PATH)
        string[] fallbackPaths = {
            @"C:\Program Files\MuPDF\mutool.exe",
            @"C:\MuPDF\mutool.exe"
        };

        if (string.IsNullOrWhiteSpace(mutoolPath))
        {
            foreach (var path in fallbackPaths)
            {
                if (System.IO.File.Exists(path))
                {
                    mutoolPath = path;
                    break;
                }
            }
        }

#else // macOS / Linux
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \"which mutool\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = System.Diagnostics.Process.Start(psi);
            mutoolPath = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            if (string.IsNullOrWhiteSpace(mutoolPath))
            {
                string[] fallbackPaths = {
                "/opt/homebrew/bin/mutool",
                "/usr/local/bin/mutool",
                "/usr/bin/mutool"
            };

                foreach (var path in fallbackPaths)
                {
                    if (System.IO.File.Exists(path))
                    {
                        mutoolPath = path;
                        break;
                    }
                }
            }
#endif

            if (!string.IsNullOrWhiteSpace(mutoolPath) && System.IO.File.Exists(mutoolPath))
            {
                UnityEngine.Debug.Log("mutool encontrado en: " + mutoolPath);
                return mutoolPath;
            }

            UnityEngine.Debug.LogError("No se encontró mutool en el sistema.");
            return null;
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("Error buscando mutool: " + ex.Message);
            return null;
        }
    }





}