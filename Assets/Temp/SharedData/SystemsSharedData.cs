using SevenBoldPencil.EasyEvents;

namespace Temp.SharedData
{
    public class SystemsSharedData
    {
        public EventsBus EventsBus { get; set; }
        
        public InGameData GameData { get; set; }
        
        public SceneData SceneData { get; set; }
    }
}