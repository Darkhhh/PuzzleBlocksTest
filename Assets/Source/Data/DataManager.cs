using System;
using BayatGames.SaveGameFree;
using UnityEngine;

namespace Source.Data
{
    public class DataManager : MonoBehaviour, IDataHandler
    {
        private UserData _data = null;

        public void Load(Action callback = null)
        {
            if (SaveGame.Exists("UserData"))
            {
                _data = SaveGame.Load<UserData>("UserData");
            }
            else
            {
                _data = new UserData
                {
                    GameData = new GameData
                    {
                        coinsAmount = 2000,
                        broomstickAmount = 2,
                        canonBallAmount = 2,
                        dynamiteAmount = 2,
                        largeDynamiteAmount = 2
                    },
                    Settings = new SettingsData
                    {
                        MusicOn = true,
                        Lang = "Russian"
                    }
                };
            }
        }

        public bool IsLoaded() => _data is not null;

        public UserData GetData() => _data;

        public void Save(UserData newData)
        {
            if (newData is null) return;
            SaveGame.Save("UserData", newData);
        }
    }
}