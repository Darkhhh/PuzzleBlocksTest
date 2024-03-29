﻿using Source.Localization;

namespace Source.UI.Code
{
    public interface IPageHandler
    {
        public void Init(string pageTag);

        public void OnPageOpen();

        public void OnPageClose();

        public void UpdateTexts();
    }
}