using UnityEngine.UI;
using UnityEngine;

public class ReloadBar : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image reloadImageFill;

    public void ActivateReloadBar()
    {
        reloadImageFill.fillAmount = 0;
        canvasGroup.alpha = 1;
    }

    public void DeactivateReloadingBar()
    {
        canvasGroup.alpha = 0;
    }
    public void UpdateReloadingBar(float elapsedTime, float reloadTime)
    {
        reloadImageFill.fillAmount = Mathf.Clamp01(elapsedTime/reloadTime);
    }
}
