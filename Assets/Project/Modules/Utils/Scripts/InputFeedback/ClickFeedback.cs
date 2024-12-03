using System.Collections;
using UnityEngine;
using Utils;

namespace Game
{
    public class ClickFeedback : MonoBehaviour
    {
        public void OnClick()
        {
            SFX.PlayUI(SFXTypeUI.BUTTON_CLICK);

            Vibration.StartHapticFeedback();
            
            base.StopCoroutine(this.StopVibration());
            base.StartCoroutine(this.StopVibration());
        }

        private IEnumerator StopVibration()
        {
            yield return new WaitForSecondsRealtime(0.1f);

            Vibration.Stop();
        }
    }
}