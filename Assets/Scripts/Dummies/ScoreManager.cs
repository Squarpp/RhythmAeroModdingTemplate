using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Este es el Singleton que WandaExpressionManager está pidiendo a gritos (línea 86)
    public static ScoreManager Instance;

    [Header("Variables expuestas para el SDK (Sin lógica)")]

    // Dejo estas variables listas por si tu WandaExpressionManager reacciona al combo o al Star Power
    public int currentScore;
    public int currentCombo;
    public int currentMultiplier;
    public bool isPowerActive;
    public float powerCharge;

    void Awake()
    {
        // En el Template simulamos el Singleton para que no tire error en Play Mode (aunque el modder no va a jugar acá)
        if (Instance == null) Instance = this;
    }

    // Funciones vacías comunes
    public void AddScore(int baseScore, bool addCombo = true) { }
    public void ResetComboAndMultiplier() { }
}