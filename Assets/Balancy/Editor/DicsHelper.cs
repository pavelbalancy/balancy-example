﻿using System;
using System.Collections;
using UnityEditor;
using Balancy.Dictionaries;

namespace Balancy.Editor {

    public class DicsHelper : EditorWindow {

        private static Loader _loader;
        private static string _apiGameId;
        
        public static void LoadDocs(BaseAppConfig settings, Action<LoaderResponseData> onCompleted, Action<string, float> onProgress, int version = 0)
        {

            _apiGameId = settings.ApiGameId;
            _loader = new Loader(settings, true, SaveDataObjectsInResources);

            _loader.Load(responseData =>
            {
                AssetDatabase.Refresh();
                onCompleted?.Invoke(responseData);
            }, onProgress, version);
        }
        
        private static void SaveDataObjectsInResources(DataObjects dataObjects, Action doneCallback)
        {
            var helper = EditorCoroutineHelper.Create();
            helper.StartCoroutine(LoadImages(_apiGameId, dataObjects, doneCallback));
        }

        static IEnumerator LoadImages(string apiGameId, DataObjects dataObjects, Action doneCallback)
        {
            var fileName = apiGameId + "_Cache/";
            var remotePath = dataObjects.Meta.RootPath + '/';
            int loadingTextures = 0;
            for (int i = 0; i < dataObjects.Objects.Length; i++)
            {
                var obj = dataObjects.Objects[i];
                if (obj.ShouldSaveInResources())
                {
                    var remote = remotePath + obj.Path;
                    loadingTextures++;
                    
                    var cor = Balancy.ObjectsLoader.LoadSpriteFromUrl(remote, loadedSprite =>
                    {
                        var path = fileName + obj.Path;
                        path = path.Replace('/', '-');
                        FileHelper.SaveSprite(path, loadedSprite, true);
                        loadingTextures--;
                    });

                    var helper = EditorCoroutineHelper.Create();
                    helper.LaunchCoroutine(cor);
                }
            }

            while (loadingTextures > 0)
                yield return null;
            
            doneCallback?.Invoke();
        }
    }
}