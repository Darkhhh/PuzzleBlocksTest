using System;

namespace Source.Data
{
    public interface IDataHandler
    {
        public void Load(Action callback = null);

        public bool IsLoaded();

        public UserData GetData();

        public void Save(UserData newData);
    }
}