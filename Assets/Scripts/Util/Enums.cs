
namespace Assets.Scripts.Util
{
    public class Enums
    {
        public enum Direction { Up, Down, Left, Right, None };
        public enum PlayerState { Idle = 0, Moving, Hit, Dead, Attack, Jump };
        public enum GameStates { Running, CutScene, Paused };
        public enum BulletTypes { Player = 0, Enemy };
    }
}
