using PuzzleCore.ECS.Common;
using SevenBoldPencil.EasyEvents;

namespace PuzzleCore.ECS.SharedData
{
    public class SystemsSharedData
    {
        public EventsBus EventsBus { get; set; }
        
        public InGameData GameData { get; set; }
    }
}