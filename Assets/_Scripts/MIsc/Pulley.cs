using UnityEngine;
using DG.Tweening;

public class Pulley : MonoBehaviour
{
    [Header("References")]
    public SpinManager spinManager;
    public Transform handle;

    [Header("Settings")]
    public float pullAngle = -60f;
    public float duration = 0.2f;

    private bool isBusy = false;

    // 🎯 Assign THIS to Button OnClick()
    public void OnPulleyPressed()
    {
        if (isBusy) return;

        isBusy = true;

        handle.DOLocalRotate(new Vector3(pullAngle, 0, 0), duration)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                isBusy = false;
            });

        spinManager.SpinButtonPressed();
    }
}