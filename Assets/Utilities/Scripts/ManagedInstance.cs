using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
#if USING_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif

namespace Tetr4lab {

    /// <summary>プレハブのインスタンス数を管理するクラス</summary>
    /// <remarks>
    /// # 概要
    ///     - 管理対象のクラスをTとします。
    ///		    - TはMonoBehaviourを継承しなければなりません。
    ///		- デフォルトでTと同じ名前のプレハブを使います。
    ///		    - クラスがプレハブにあらかじめアタッチされていなくても、自動的にインスタンスにアタッチされます。
    ///		- 例えば、MaxInstances=1、AutoDelete=true だと、常に最後のひとつだけが存在するようになります。
    ///	# 使い方
    ///	    - `MonoBehaviour`を継承したクラスを`T`として定義して使います。
    ///	    ```csharp:ExampleClass.cs
    ///		/// &gt;summary&lt;管理対象クラス&gt;/summary&lt;
    ///		public sealed class ExampleClass : MonoBehaviour {
    ///		    /// &gt;summary&lt;管理クラス&gt;/summary&lt;
    ///		    private static ManagedInstance&gt;ExampleClass&lt; ManagedInstance { get; }
    ///		        = new ManagedInstance&gt;ExampleClass&lt; (3, true);
    ///		    /// &gt;summary&lt;インスタンス生成&gt;/summary&lt;
    ///		    /// &gt;param name="parent"&lt;コンテナ&gt;/param&lt;
    ///		    /// &gt;returns&lt;生成されたインスタンス&gt;/returns&lt;
    ///		    public static async Task&gt;ExampleClass&lt; CreateAsync (GameObject parent) =&lt; await ManagedInstance.CreateAsync (parent);
    ///		    /// &gt;summary&lt;破棄&gt;/summary&lt;
    ///		    private void OnDestroy () =&lt; ManagedInstance.Instances.Remove (this);
    ///		}
    ///		```
    /// </remarks>
    public class ManagedInstance<T> : IDisposable where T : MonoBehaviour {

		/// <summary>インスタンス数</summary>
		public int Count => Instances.Count;
		
		/// <summary>最大インスタンス数 (0で無制限)</summary>
		public int MaxInstances { get; protected set; }
		
		/// <summary>上限数を超えたら最古を破棄する</summary>
		public bool AutoDelete { get; protected set; }
		
		/// <summary>インスタンス一覧</summary>
		public List<T> Instances { get; protected set; }
		
		/// <summary>最新のインスタンス</summary>
		public T LastInstance => (Instances == null || Count <= 0) ? null : Instances [Count - 1];

        /// <summary>インスタンスのインデックス</summary>
        /// <param name="instance">インスタンス</param>
        /// <returns>インデックス</returns>
        public int IndexOf (T instance) => Instances.IndexOf (instance);

		/// <summary>ひとつ以上生成されている</summary>
		public bool OnMode {
			get { return (Instances != null && Count > 0); }
			set {
				if (!value) { // 全インスタンス破棄
					foreach (var instance in Instances) {
						if (instance != null) { GameObject.Destroy (instance.gameObject); }
					}
				}
			}
		}

		/// <summary>デフォルトプレハブ</summary>
		public async Task<GameObject> GetPrefabAsync () {
			if (!prefab) {
#if USING_ADDRESSABLES
                prefab = await Addressables.LoadAssetAsync<GameObject> ($"Prefabs/{typeof (T).Name}.prefab").Task;
#else
                var request = Resources.LoadAsync<GameObject> ($"Prefabs/{typeof (T).Name}");
                await TaskEx.DelayUntil (() => request.isDone);
                prefab = (GameObject) request.asset;
#endif
			}
			return prefab;
		}
		private GameObject prefab;

		/// <summary>コンストラクタ</summary>
		/// <param name="max">最大インスタンス数 (0で無制限)</param>
		/// <param name="autoDelete">上限数を超えたら最古を破棄する</param>
		public ManagedInstance (int max = 0, bool autoDelete = false) {
			if (Instances == null) {
				MaxInstances = max;
				AutoDelete = autoDelete;
				Instances = new List<T> { };
				_ = GetPrefabAsync (); // ブロックしない
			}
		}

		/// <summary>
		/// uGUIオブジェクト生成
		/// </summary>
		/// <param name="parent">ヒエラルキー上の親</param>
		/// <param name="prefab">プレハブ</param>
		/// <param name="control">管理下 (除外可能)</param>
		/// <returns>生成したオブジェクトの制御ルーチン</returns>
		public async Task<T> CreateAsync (GameObject parent, GameObject prefab = null, bool control = true) {
			if (!preCreate ()) { return null; }
			try {
				return postCreate (GameObject.Instantiate (prefab ?? await GetPrefabAsync (), parent.transform), control);
			}
			catch {
				return null;
			}
		}

		/// <summary>3Dオブジェクト生成</summary>
		public async Task<T> CreateAsync (Vector3 position, Quaternion rotation, GameObject prefab = null, bool control = true) {
			if (!preCreate ()) { return null; }
			try {
				return postCreate (GameObject.Instantiate (prefab ?? await GetPrefabAsync (), position, rotation), control);
			}
			catch {
				return null;
			}
		}

		/// <summary>生成前処理</summary>
		private bool preCreate () {
			if (MaxInstances > 0 && Count >= MaxInstances) { // 制限数オーバー
				if (AutoDelete) { // 最古を破棄して成り代わり
					GameObject.Destroy (Instances [0].gameObject);
				} else {
					return false; // 生成忌避
				}
			}
			return true;
		}

		/// <summary>生成後処理</summary>
		private T postCreate (GameObject obj, bool control) {
			var instance = obj.GetComponent<T> () ?? obj.AddComponent<T> ();
			if (instance != null) {
				if (control) {
					Instances.Add (instance);
				}
			} else {
				GameObject.Destroy (obj);
			}
			return instance;
		}

		/// <summary>全インスタンス破棄</summary>
		public void Destroy () {
			foreach (var instance in Instances) {
				if (instance != null) { GameObject.Destroy (instance.gameObject); }
			}
		}

		/// <summary>後始末</summary>
		public void Dispose () {
			OnMode = false;
			if (prefab) {
				Resources.UnloadAsset (prefab);
				prefab = null;
			}
		}

	}

}

