using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    [Header("Variables expuestas para el SDK (Sin lógica)")]

    // Esta es la variable que WandaExpressionManager necesita leer para saber si el jugador está por perder
    public float currentHealth = 100f;

    // Otras variables que vimos antes que suelen ser leídas por otros scripts:
    public float actualBpm;
    public float songPitch = 1f;
    public static string levelFolderName;
    public static string difficulty;

    // Funciones vacías por si algún script las llama
    public void StartResumeRewind() { }
    public void CancelRewind() { }
}