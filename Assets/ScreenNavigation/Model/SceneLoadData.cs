using UnityEngine.SceneManagement;

namespace ScreenNavigation
{
	public struct SceneLoadData
    {
        public SceneID ID;
        public LoadSceneMode Mode;
		public object NewSceneParams;
	}
}