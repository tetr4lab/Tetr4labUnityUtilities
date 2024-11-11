using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tetr4lab.UnityEngine {

    /// <summary>プレハブで画面を作るクラスの基礎 (同種の画面は同時に一つだけ)</summary>
    /// <example>public class NewClass : ScreenMode<NewClass>, IScreenMode { }</example>
    public abstract class ScreenMode<T> : MonoBehaviour where T : MonoBehaviour, IScreenMode {

        #region Static

		protected static GameObject modePrefab; // 画面のプレハブ
		protected static bool modeStarted; // 初期化の開始状態
		protected static bool modeInited; // 初期化済み
		protected static GameObject singleton; // シングルトン

		/// <summary>既存</summary>
		public static bool OnMode => singleton != null;

        /// <summary>初期化 先行可能</summary>
		public static async Task InitAsync () {
            if (modeInited) return;
			if (modeStarted) {
				await TaskEx.DelayWhile (() => !modeInited);
			} else {
				modeStarted = true;
				modePrefab = await Addressables.LoadAssetAsync<GameObject> ($"Prefabs/{typeof (T).Name}.prefab").Task;
                modeInited = true;
			}
		}

		/// <summary>画面の生成</summary>
		public static async Task<GameObject> CreateAsync (GameObject parent, params object [] args) {
            if (singleton) return null;
			await InitAsync ();
			singleton = Instantiate (modePrefab, parent.transform);
            var instance = singleton.GetComponent<T> () ?? singleton.AddComponent<T> ();
            if (singleton && (await instance.InitAsync (parent, args)) != true) {
				singleton = null;
			}
			return singleton;
		}

        #endregion

		/// <summary>破棄</summary>
		private void OnDestroy () {
			singleton = null;
		}
	}

	/// <summary>画面のインターフェイス</summary>
	public interface IScreenMode {
        /// <summary>画面の初期化</summary>
        /// <param name="parent">コンテナ</param>
        /// <param name="args">初期化の引数</param>
        /// <returns>成否</returns>
        public abstract Task<bool> InitAsync (GameObject parent, params object [] args);
    }

}
