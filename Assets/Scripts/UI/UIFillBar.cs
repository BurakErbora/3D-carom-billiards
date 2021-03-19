using UnityEngine;
using UnityEngine.UI;

namespace CaromBilliards3D.UI
{
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