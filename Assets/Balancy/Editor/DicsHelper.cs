using System;
using System.Collections;
using UnityEditor;
using Balancy.Dictionaries;

namespace Balancy.Editor {

    public class DicsHelper : EditorWindow {

        private static Loader _loader;
        private static IEnumerator _coroutine;

        public static void LoadDocs(AppConfig settings, Action<ResponseData> onCompleted, Action<string, float> onProgress, int version) {
            
            _loader = new Loader(settings, true);

            var helper = EditorCoroutineHelper.Create();
            
            _coroutine = _loader.Load(helper, responseData =>
            {
                AssetDatabase.Refresh();
                onCompleted?.Invoke(responseData);
            }, onProgress, version);

            helper.LaunchCoroutine(_coroutine);
        }
    }
}