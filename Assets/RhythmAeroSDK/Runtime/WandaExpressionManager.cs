using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlendShapeSetting
{
    [Tooltip("El índice del BlendShape (0, 1, 2...). Podés contar el orden en el SkinnedMeshRenderer.")]
    public int blendShapeIndex;

    [Tooltip("El peso objetivo. Ojo: Unity usa valores de 0 a 100 en el Inspector para esto.")]
    [Range(0f, 150f)]
    public float targetWeight = 100f;
}

[System.Serializable]
public class ExpressionState
{
    public string stateName = "Nueva Expresion";
    public List<BlendShapeSetting> activeBlendShapes = new List<BlendShapeSetting>();
}

public class WandaExpressionManager : MonoBehaviour
{
    [Header("Referencias")]
    public SkinnedMeshRenderer faceRenderer;
    public RhythmManager rhythmManager;

    [Header("Control de Animaciones")]
    [Tooltip("Si está en TRUE, el script deja de forzar la cara y permite que el Animator tome el control total.")]
    public bool pausarExpresiones = false;

    [Header("Configuración de Estados")]
    public float lowHealthThreshold = 30f;
    public float missExpressionDuration = 1.5f;
    public float transitionSpeed = 10f;

    [Header("Configuración de Parpadeo")]
    [Tooltip("Mirá la consola al darle Play para saber qué número es el parpadeo")]
    public int blinkBlendShapeIndex = 0;

    [Tooltip("Índices de BlendShapes que, si superan 50 de peso, evitarán que el personaje parpadee (ej: guiño, ojos apretados).")]
    public List<int> blendShapesQueBloqueanParpadeo = new List<int>();

    public float minBlinkTime = 4f;
    public float maxBlinkTime = 5f;

    [Tooltip("Duración total del parpadeo completo (cerrar y abrir) en segundos.")]
    public float blinkDuration = 0.15f;

    [Header("Tus Expresiones (Templates)")]
    public ExpressionState baseFace;
    public ExpressionState combo20Face;
    public ExpressionState combo50Face;
    public ExpressionState combo100Face;
    public ExpressionState lowHealthFace;
    public ExpressionState missFace;

    // Variables internas expresiones
    private ExpressionState currentTargetFace;
    private float missTimer = 0f;
    private int lastKnownCombo = 0;

    // Variables internas parpadeo
    private float nextBlinkTimer;
    private bool isBlinking = false;
    private float blinkProgress = 0f; // Control en tiempo real (0 a 1)

    void Start()
    {
        currentTargetFace = baseFace;
        nextBlinkTimer = Random.Range(minBlinkTime, maxBlinkTime);
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0f || rhythmManager == null || faceRenderer == null) return;
        if (pausarExpresiones) return;

        CheckLogicAndPriorities();
        HandleBlinkTimer();
        ApplyExpressionSmoothly();
    }

    private void CheckLogicAndPriorities()
    {
        if (ScoreManager.Instance.currentCombo == 0 && lastKnownCombo > 0)
        {
            missTimer = missExpressionDuration;
        }
        lastKnownCombo = ScoreManager.Instance.currentCombo;

        if (missTimer > 0) missTimer -= Time.deltaTime;

        // --- SISTEMA DE PRIORIDADES ---
        if (missTimer > 0)
        {
            currentTargetFace = missFace;
        }
        else if (rhythmManager.currentHealth <= lowHealthThreshold)
        {
            currentTargetFace = lowHealthFace;
        }
        else if (ScoreManager.Instance.currentCombo >= 100)
        {
            currentTargetFace = combo100Face;
        }
        else if (ScoreManager.Instance.currentCombo >= 50)
        {
            currentTargetFace = combo50Face;
        }
        else if (ScoreManager.Instance.currentCombo >= 20)
        {
            currentTargetFace = combo20Face;
        }
        else
        {
            currentTargetFace = baseFace;
        }
    }

    private void HandleBlinkTimer()
    {
        if (!isBlinking)
        {
            if (IsBlinkBlocked()) return;

            nextBlinkTimer -= Time.deltaTime;
            if (nextBlinkTimer <= 0f)
            {
                isBlinking = true;
                blinkProgress = 0f;
            }
        }
        else
        {
            // Avanzamos el progreso de 0 a 1 según la duración elegida
            blinkProgress += Time.deltaTime / Mathf.Max(0.01f, blinkDuration);

            if (blinkProgress >= 1f)
            {
                isBlinking = false;
                blinkProgress = 0f;
                nextBlinkTimer = Random.Range(minBlinkTime, maxBlinkTime);
            }
        }
    }

    private bool IsBlinkBlocked()
    {
        if (GetTargetFromExpression(blinkBlendShapeIndex) > 50f) return true;

        foreach (int index in blendShapesQueBloqueanParpadeo)
        {
            if (GetTargetFromExpression(index) > 50f) return true;
        }

        return false;
    }

    private void ApplyExpressionSmoothly()
    {
        if (faceRenderer == null) return;

        int totalBlendShapes = faceRenderer.sharedMesh.blendShapeCount;

        for (int i = 0; i < totalBlendShapes; i++)
        {
            // 1. Manejo independiente del parpadeo
            if (i == blinkBlendShapeIndex)
            {
                if (isBlinking)
                {
                    float blinkWeight;

                    // De 0.0 a 0.5 del progreso: Ojos cerrándose (0 -> 100)
                    // De 0.5 a 1.0 del progreso: Ojos abriéndose (100 -> 0)
                    if (blinkProgress <= 0.5f)
                    {
                        blinkWeight = Mathf.Lerp(0f, 100f, blinkProgress * 2f);
                    }
                    else
                    {
                        blinkWeight = Mathf.Lerp(100f, 0f, (blinkProgress - 0.5f) * 2f);
                    }

                    faceRenderer.SetBlendShapeWeight(i, blinkWeight);
                    continue; // Pasa directamente al siguiente BlendShape
                }
            }

            // 2. Manejo normal de expresiones
            float target = GetTargetFromExpression(i);
            float currentWeight = faceRenderer.GetBlendShapeWeight(i);
            float currentSpeed = transitionSpeed * 10f;

            if (Mathf.Abs(currentWeight - target) > 0.1f)
            {
                float newWeight = Mathf.MoveTowards(currentWeight, target, Time.deltaTime * currentSpeed);
                faceRenderer.SetBlendShapeWeight(i, newWeight);
            }
            else if (currentWeight != target)
            {
                faceRenderer.SetBlendShapeWeight(i, target);
            }
        }
    }

    private float GetTargetFromExpression(int index)
    {
        if (currentTargetFace != null && currentTargetFace.activeBlendShapes != null)
        {
            for (int i = 0; i < currentTargetFace.activeBlendShapes.Count; i++)
            {
                if (currentTargetFace.activeBlendShapes[i].blendShapeIndex == index)
                {
                    return currentTargetFace.activeBlendShapes[i].targetWeight;
                }
            }
        }
        return 0f;
    }
}