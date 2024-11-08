using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Tetr4lab.Utilities {

    /// <summary>汎用ユーティリティ</summary>
    public static partial class Tetr4labUtility {

		/// <summary>ストリーミングアセットからデータを読み込んで返す</summary>
        /// <param name="filename"></param>
        /// <returns></returns>
		public static byte [] LoadStreamingData (this string filename) {
			string sourcePath = Path.Combine (Application.streamingAssetsPath, filename);
			var gz = filename.EndsWith (".gz");
			if (sourcePath.Contains ("://")) { // Android
				using (var www = UnityWebRequest.Get (gz ? sourcePath.Substring (0, sourcePath.Length - 3) : sourcePath)) {
					www.SendWebRequest ();
					while (www.result == UnityWebRequest.Result.InProgress) { }
					if (www.result == UnityWebRequest.Result.Success) {
						return www.downloadHandler.data;
					}
				}
			} else if (File.Exists (sourcePath)) { // Mac, Windows, iPhone
				if (gz) {
					using (var data = File.OpenRead (sourcePath))
					using (var compresed = new GZipStream (data, CompressionMode.Decompress))
					using (var decompressed = new MemoryStream ()) {
						compresed.CopyTo (decompressed);
						return decompressed.ToArray ();
					}
				} else {
					return File.ReadAllBytes (sourcePath);
				}
			}
			return null;
		}

        /// <summary>ストリーミングアセットからテキストを読み込んで返す</summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string LoadStreamingText (this string filename) {
			string sourcePath = Path.Combine (Application.streamingAssetsPath, filename);
			var gz = filename.EndsWith (".gz");
			if (sourcePath.Contains ("://")) { // Android
				using (var www = UnityWebRequest.Get (gz ? sourcePath.Substring (0, sourcePath.Length - 3) : sourcePath)) {
					www.SendWebRequest ();
					while (www.result == UnityWebRequest.Result.InProgress) { }
					if (www.result == UnityWebRequest.Result.Success) {
						return www.downloadHandler.text;
					}
				}
			} else if (File.Exists (sourcePath)) { // Mac, Windows, iPhone
				if (gz) {
					using (var data = File.OpenRead (sourcePath))
					using (var compresed = new GZipStream (data, CompressionMode.Decompress))
					using (var text = new MemoryStream ()) {
						compresed.CopyTo (text);
						return Encoding.UTF8.GetString (text.ToArray ());
					}
				} else {
					return File.ReadAllText (sourcePath);
				}
			}
			return null;
		}

		/// <summary>設定上のネット接続の有効性 (実際に接続できるかどうかは別)</summary>
		public static bool IsNetworkAvailable => (Application.internetReachability != NetworkReachability.NotReachable);

		/// <summary>テキストファイルへ保存する</summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
		public static void SaveTextFile (string name, string text) {
			var path = System.IO.Path.Combine (Application.persistentDataPath, name);
			System.IO.File.WriteAllBytes (path, System.Text.Encoding.UTF8.GetBytes (text));
		}

		/// <summary>オブジェクトが最前かどうかを判定</summary>
        /// <param name="child"></param>
        /// <returns></returns>
		public static bool IsLastSibling (this GameObject child) {
			return (child.transform.GetSiblingIndex () == child.transform.parent.childCount - 1);
		}

	}

}
