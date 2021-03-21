using UnityEngine;
using UnityEngine.UI;

namespace CaromBilliards3D.UI
{
    // simple helper class to use with a "progress bar" kind of GUI element
    public class UIFillBar : MonoBehaviour
    {
        public Image barFill;

        public void SetFillAmount(float fillAmount)
        {
            barFill.fillAmount = fillAmount;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}