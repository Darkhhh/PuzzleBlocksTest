using System;
using System.Collections.Generic;
using System.Xml;
using Source.Code.Common.Utils;
using Source.UI.Code.Menu.Pages;
using Source.UI.Code.Menu.Pages.Market;
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
        
        
        public void GetMarketItems(ref Dictionary<string, MarketItemInfo> strings)
        {
            if (_xmlData.DocumentElement != null)
            {
                var node = _xmlData.DocumentElement.SelectSingleNode("MarketItems");
                if (node is null) throw new Exception($"Not available data with tag: MarketItems");
                var elements = node.SelectNodes("MarketItem");
                if (elements is null) return;

                foreach (XmlNode e in elements)
                {
                    var itemTag = e!.Attributes!["id"].Value;
                    var descriptionNode = e.SelectSingleNode("Description");
                    if (descriptionNode is null) throw new Exception("Can not get description from item");
                    var description = new TextInfo
                    {
                        Text = descriptionNode.InnerText,
                        FontSize = Convert.ToInt32(descriptionNode.Attributes!["FontSize"].Value)
                    };
                    var titleNode = e.SelectSingleNode("Title");
                    if (titleNode is null) throw new Exception("Can not get title from item");
                    var title = new TextInfo
                    {
                        Text = titleNode.InnerText, FontSize = Convert.ToInt32(titleNode.Attributes!["FontSize"].Value)
                    };

                    strings.Add(itemTag, new MarketItemInfo
                    {
                        Description =  description, Name = title
                    });
                }
            }
            else
            {
                throw new Exception("XML File not available");
            }
        }
    }
}