using PuzzleCore.ECS.Common;
using SevenBoldPencil.EasyEvents;
using Temp.SharedData;

namespace PuzzleCore.ECS.SharedData
{
    public class SystemsSharedData
    {
        public EventsBus EventsBus { get; set; }
        
        public InGameData GameData { get; set; }
        
        public SceneData SceneData { get; set; }
    }
}