using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game
{
    [RequireComponent(typeof(Button))]
    public class ButtonFeedback : MonoBehaviour
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