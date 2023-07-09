using System;
using System.Collections;
using System.Collections.Generic;
using Source.Localization;
using UnityEngine;
using Zenject;

namespace Source.UI.Code
{
    public abstract class PageHandler : MonoBehaviour, IPageHandler
    {
        [Inject] protected ILocalizationHandler LocalizationHandler;

        protected Dictionary<string, (string val, int fontSize)> PageStrings;
        
        protected string UIPageTag { get; set; }

        public virtual void Init(string pageTag)
        {
            UIPageTag = pageTag;
        }

        public abstract void OnPageOpen();

        public abstract void OnPageClose();
        public abstract void UpdateTexts();


        protected IEnumerator GetPageStrings(Action callback = null)
        {
            while (!LocalizationHandler.IsLoaded())
            {
                yield return null;
            }

            PageStrings = new Dictionary<string, (string val, int fontSize)>();
            LocalizationHandler.GetPageStrings(UIPageTag, ref PageStrings);
            
            callback?.Invoke();
        }
    }
}