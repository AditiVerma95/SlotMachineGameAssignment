using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SpinManager : MonoBehaviour
{
    [Header("Reels")]
    public ReelMoveManager reel1;
    public ReelMoveManager reel2;
    public ReelMoveManager reel3;

    [Header("Pop-Up UI")]
    public CanvasGroup mainPopUpGroup; // Add CanvasGroup to the Parent PopUp
    public GameObject winText;
    public GameObject niceTryText;
    public GameObject loseText;

    private bool isSpinning = false;

    private void Start()
    {
        // Hide pop-up at start
        mainPopUpGroup.alpha = 0;
        mainPopUpGroup.gameObject.SetActive(false);
    }

    public void SpinButtonPressed()
    {
        if (isSpinning) return;

        // 1. Play Click Immediately
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);

        HidePopUp();
        SymbolType[] result = SpinInternal();
        StartCoroutine(SpinSequence(result));
    }

    IEnumerator SpinSequence(SymbolType[] result)
    {
        isSpinning = true;

        // 2. Play Spin Sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.spinSound);

        reel1.SpinTo(result[0]);
        yield return new WaitForSeconds(0.15f);
        reel2.SpinTo(result[1]);
        yield return new WaitForSeconds(0.15f);
        reel3.SpinTo(result[2]);

        // 3. Wait for reels to stop (1.5s spin + 0.6s landing + 0.1 delay)
        yield return new WaitForSeconds(2.3f);

        DetermineResult(result);
        isSpinning = false;
    }

    private void DetermineResult(SymbolType[] res)
    {
        // Turn off all text children
        winText.SetActive(false);
        niceTryText.SetActive(false);
        loseText.SetActive(false);

        // Logic for sound and which text to enable
        if (res[0] == res[1] && res[1] == res[2])
        {
            winText.SetActive(true);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.winSound);
        }
        else if (res[0] == res[1] || res[1] == res[2] || res[0] == res[2])
        {
            niceTryText.SetActive(true);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.loseSound);
        }
        else
        {
            loseText.SetActive(true);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.loseSound);
        }

        ShowPopUp();
    }

    private void ShowPopUp()
    {
        mainPopUpGroup.gameObject.SetActive(true);
        mainPopUpGroup.transform.localScale = Vector3.one * 0.7f;
        
        mainPopUpGroup.DOFade(1f, 0.4f);
        mainPopUpGroup.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
    }

    private void HidePopUp()
    {
        mainPopUpGroup.DOFade(0f, 0.2f).OnComplete(() => {
            mainPopUpGroup.gameObject.SetActive(false);
        });
    }

    private SymbolType[] SpinInternal()
    {
        return new SymbolType[] { GetRandomSymbol(), GetRandomSymbol(), GetRandomSymbol() };
    }

    private SymbolType GetRandomSymbol() => (SymbolType)Random.Range(0, 4);
}