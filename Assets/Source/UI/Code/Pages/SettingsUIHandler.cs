using System;
using System.Linq;
using Source.Localization;
using TMPro;
using UnityEngine;

namespace Source.UI.Code.Pages
{
    public class SettingsUIHandler : PageHandler
    {
        #region Const Strings

        private const string TitleTag = "xml-settings-title";
        private const string MusicTag = "xml-settings-music";
        private const string LanguagesTag = "xml-settings-language";

        #endregion
        
        
        #region Serialized Fields

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI musicAndSounds;
        [SerializeField] private TextMeshProUGUI language;
        [SerializeField] private TMP_Dropdown dropdown;

        [Header("Button Elements")] 
        [SerializeField] private GameObject musicOnIcon;
        [SerializeField] private GameObject musicOffIcon;

        #endregion


        public Action<Language> ChangeLanguage;
        public Action<bool> ChangeMusicStatus;
        public Action ReturnBack;


        public override void OnPageOpen()
        {
            dropdown.options.Clear();
            var currentLanguage = LocalizationHandler.GetCurrentLanguage();
            foreach (var item in LocalizationExtensions.GetAllLanguages())
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
                if (item == currentLanguage) dropdown.value = dropdown.options.Count - 1;
            }
            UpdateTexts();
        }

        public void SetMusicValue(bool val)
        {
            musicOnIcon.SetActive(val);
            musicOffIcon.SetActive(!val);
        }

        public override void OnPageClose() { }
        
        public void OnBackButtonClick() => ReturnBack?.Invoke();

        public void OnMusicButtonClick()
        {
            musicOnIcon.SetActive(!musicOnIcon.activeSelf);
            musicOffIcon.SetActive(!musicOffIcon.activeSelf);
            
            ChangeMusicStatus?.Invoke(musicOnIcon.activeSelf);
        }

        public void OnDropdownValueChange(int val)
        {
            var languages = LocalizationExtensions.GetAllLanguages();
            ChangeLanguage?.Invoke(languages[val]);
        }


        public override void UpdateTexts()
        {
            StartCoroutine(GetPageStrings(() =>
            {
                var data = PageStrings[TitleTag];
                title.text = data.val;
                title.fontSize = data.fontSize;
        
                data = PageStrings[MusicTag];
                musicAndSounds.text = data.val;
                musicAndSounds.fontSize = data.fontSize;
        
                data = PageStrings[LanguagesTag];
                language.text = data.val;
                language.fontSize = data.fontSize;
            }));
        }
    }
}