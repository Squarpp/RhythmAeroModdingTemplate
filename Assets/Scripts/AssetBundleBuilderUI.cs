using UnityEngine;
using System.IO;

// Envolvemos la librería del Editor para que no rompa si el juego se exporta
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetBundleBuilderUI : MonoBehaviour
{
    /// <summary>
    /// Esta función se debe asignar al evento OnClick() de tu botón en el Canvas.
    /// </summary>
    public void BuildBundles()
    {
#if UNITY_EDITOR
        string assetBundleDirectory = "Assets/ExportedBundles";

        // Creamos la carpeta si no existe
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        // Ejecutamos la compilación
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows64);

        Debug.Log("<color=green>¡ÉXITO!</color> El personaje fue compilado en la carpeta ExportedBundles.");

        // Opcional: Refresca el explorador de archivos de Unity para que el archivo aparezca al instante
        AssetDatabase.Refresh();
#else
        // Este mensaje aparecerá si intentan tocar el botón en un juego ya exportado (.exe)
        Debug.LogError("Los AssetBundles solo se pueden compilar dentro del Unity Editor.");
#endif
    }
}