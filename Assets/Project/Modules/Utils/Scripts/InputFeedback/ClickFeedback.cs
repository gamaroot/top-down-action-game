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
            
            string invokeMethod = nameof(this.StopVibration);
            base.CancelInvoke(invokeMethod);
            base.Invoke(invokeMethod, 0.1f);
        }

        private void StopVibration()
        {
            Vibration.Stop();
        }
    }
}