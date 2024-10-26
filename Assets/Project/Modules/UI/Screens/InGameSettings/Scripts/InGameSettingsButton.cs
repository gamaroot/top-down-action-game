using ScreenNavigation;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class InGameSettingsButton : MonoBehaviour
    {
        [SerializeField] private Animator _btnSettingsAnimator;

        private void OnValidate()
        {
            if (this._btnSettingsAnimator == null)
                this._btnSettingsAnimator = base.GetComponent<Animator>();
        }

        public void OnClick()
        {
            bool visible = !this._btnSettingsAnimator.GetBool(AnimationKeys.VISIBLE);
            this._btnSettingsAnimator.SetBool(AnimationKeys.VISIBLE, visible);

            if (visible)
                SceneNavigator.Instance.LoadAdditiveSceneAsync(SceneID.INGAME_SETTINGS);
            else
                SceneNavigator.Instance.UnloadSceneAsync(SceneID.INGAME_SETTINGS);
        }
    }
}