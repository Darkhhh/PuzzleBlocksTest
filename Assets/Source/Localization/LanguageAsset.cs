using System;
using UnityEngine.AddressableAssets;

namespace Source.Localization
{
    [Serializable]
    public class LanguageAsset
    {
        public Language language;

        public AssetReference addressableTextAsset;
    }
}