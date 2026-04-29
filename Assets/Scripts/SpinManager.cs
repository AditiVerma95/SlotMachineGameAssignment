using System.Collections;
using UnityEngine;

public class SpinManager : MonoBehaviour
{
    public ReelMoveManager reel1;
    public ReelMoveManager reel2;
    public ReelMoveManager reel3;

    private int spinCount = 0;

    public void SpinButtonPressed()
    {
        SymbolType[] result = SpinInternal();
        StartCoroutine(SpinSequence(result));
        Debug.Log($"🎰 RESULT → {result[0]} | {result[1]} | {result[2]}");
    }

    IEnumerator SpinSequence(SymbolType[] result)
    {
        reel1.SpinTo(result[0]);

        yield return new WaitForSeconds(0.15f);
        reel2.SpinTo(result[1]);

        yield return new WaitForSeconds(0.15f);
        reel3.SpinTo(result[2]);
    }

    private SymbolType[] SpinInternal()
    {
        spinCount++;

        if (spinCount % 10 == 0)
        {
            SymbolType reward = GetRandomSymbol();
            return new SymbolType[] { reward, reward, reward };
        }

        return new SymbolType[]
        {
            GetRandomSymbol(),
            GetRandomSymbol(),
            GetRandomSymbol()
        };
    }

    private SymbolType GetRandomSymbol()
    {
        return (SymbolType)Random.Range(0, 4);
    }
}