using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Source.Localization;
using TMPro;
using UnityEngine;

namespace Source.UI.Code
{
    public class SettingsUIHandler : PageHandler
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI musicAndSounds;
        [SerializeField] private TextMeshProUGUI language;
        [SerializeField] private TMP_Dropdown dropdown;

        [Header("Button Elements")] 
        [SerializeField] private GameObject musicOnIcon;
        [SerializeField] private GameObject musicOffIcon;
        
        
        public override void OnPageOpen()
        {
            
        }

        public override void OnPageClose()
        {
            gameObject.SetActive(false);
        }

        public override void Prepare(ILocalizationHandler localizationHandler, Language currentLanguage)
        {
            base.Prepare(localizationHandler, currentLanguage);
            
            dropdown.options.Clear();
            foreach (var item in Enum.GetValues(typeof(Language)).Cast<Language>())
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
            }
            
            musicOnIcon.SetActive(true);
            musicOffIcon.SetActive(false);
            
            StartCoroutine(SetAllTexts(localizationHandler));
        }

        

        private IEnumerator SetAllTexts(ILocalizationHandler localizationHandler)
        {
            while (!localizationHandler.IsLoaded())
            {
                yield return null;
            }

            var strings = new Dictionary<string, (string val, int fontSize)>();
            localizationHandler.GetPageStrings("xml-settings", ref strings);
        
        
            var data = strings["xml-settings-title"];
            title.text = data.val;
            title.fontSize = data.fontSize;
        
            data = strings["xml-settings-music"];
            musicAndSounds.text = data.val;
            musicAndSounds.fontSize = data.fontSize;
        
            data = strings["xml-settings-language"];
            language.text = data.val;
            language.fontSize = data.fontSize;
        }
        
        public void OnBackButtonClick()
        {
            OnPageClose();
        }

        public void OnMusicButtonClick()
        {
            musicOnIcon.SetActive(!musicOnIcon.activeSelf);
            musicOffIcon.SetActive(!musicOffIcon.activeSelf);
        }

        public void OnDropdownValueChange(int val)
        {
            var languages = Enum.GetValues(typeof(Language)).Cast<Language>().ToArray();
            
            GetLocalizationHandler().Load(languages[val]);
            StartCoroutine(SetAllTexts(GetLocalizationHandler()));
        }
    }
}