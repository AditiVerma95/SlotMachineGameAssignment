using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReelMoveManager : MonoBehaviour
{
    public RectTransform content;

    [Header("Config")]
    public float spinSpeed = 2000f;
    public float spinDuration = 1.5f;
    public float symbolStep = 107f;

    [Header("New Boundaries")]
    
    public float topPosition = -160f; 
    public float bottomPosition = 160f; 

    public List<SymbolType> symbols; // [Cherry, Bell, Seven, Bar]
    private bool isSpinning;

    public void SpinTo(SymbolType targetSymbol)
    {
        if (isSpinning) return;
        StartCoroutine(SpinRoutine(targetSymbol));
    }

    IEnumerator SpinRoutine(SymbolType targetSymbol)
    {
        isSpinning = true;

        float timer = 0f;
        // Total range is 320 (160 - (-160))
        float totalRange = Mathf.Abs(bottomPosition - topPosition); 

        // --- PHASE 1: SPINNING ---
        while (timer < spinDuration)
        {
        
            content.anchoredPosition -= new Vector2(0, spinSpeed * Time.deltaTime);

            // Wrap logic: If we go past -160, loop back to +160
            if (content.anchoredPosition.y < topPosition)
            {
                content.anchoredPosition += new Vector2(0, totalRange);
            }
            
            timer += Time.deltaTime;
            yield return null;
        }

        // --- PHASE 2: CALCULATE TARGET ---
        int targetIndex = symbols.IndexOf(targetSymbol);
        
        // Since Top is -160 and we are moving through the list, 
        // the next symbol is likely at -160 + 107 = -53
        float targetY = topPosition + (targetIndex * symbolStep);

        // --- PHASE 3: THE PRECISION LANDING ---
        float currentY = content.anchoredPosition.y;

       
        while (targetY > currentY)
        {
            targetY -= totalRange;
        }

        yield return content.DOAnchorPosY(targetY, 0.6f)
            .SetEase(Ease.OutBack)
            .WaitForCompletion();

        // --- PHASE 4: FINAL SNAP ---
        float finalVisualY = topPosition + (targetIndex * symbolStep);
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, finalVisualY);

        isSpinning = false;
    }
}