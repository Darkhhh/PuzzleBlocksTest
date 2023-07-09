using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Source.Localization
{
    public class LocalizationHandler : MonoBehaviour, ILocalizationHandler
    {
        [SerializeField] private List<LanguageAsset> assets;
        private Language _currentLanguage;

        private readonly Dictionary<Language, AssetReference> _languagesTextFiles = new();
        private XmlDocument _xmlData = null;
        private AsyncOperationHandle<TextAsset> _handle;

        public void Init()
        {
            foreach (var item in assets)
            {
                _languagesTextFiles.Add(item.language, item.addressableTextAsset);
            }
        }


        public void Load(Language language, Action callbackOnCompleted = null)
        {
            if (!_languagesTextFiles.TryGetValue(language, out var xmlTextAsset))
                throw new Exception($"LocalizationHandler do not have this language: {language.ToString()}");
            
            if (_handle.IsValid())
            {
                Addressables.Release(_handle);
            }
            
            xmlTextAsset.LoadAssetAsync<TextAsset>().Completed += handle =>
            {
                _xmlData = new XmlDocument();
                _xmlData.LoadXml(handle.Result.text);
                _handle = handle;
                callbackOnCompleted?.Invoke();
            };
            _currentLanguage = language;
        }

        public void GetPageStrings(string pageTag, ref Dictionary<string, (string val, int fontSize)> strings)
        {
            if (_xmlData.DocumentElement != null)
            {
                var node = _xmlData.DocumentElement.SelectSingleNode($"Page[@id='{pageTag}']");
                if (node is null) throw new Exception($"Not available page with tag: {pageTag}");
                var elements = node.SelectNodes("Str");
                if (elements is null) return;
                
                foreach (XmlNode e in elements)
                    strings.Add(e!.Attributes!["id"].Value, (e.InnerText, Convert.ToInt32(e.Attributes["FontSize"].Value)));
            }
            else
            {
                throw new Exception("XML File not available");
            }
        }

        private void OnDestroy()
        {
            if (_handle.IsValid())
            {
                Addressables.Release(_handle);
            }
        }

        public bool IsLoaded() => _handle.IsValid();

        public Language GetCurrentLanguage() => _currentLanguage;
    }
}