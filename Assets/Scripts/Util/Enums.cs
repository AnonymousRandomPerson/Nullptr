
namespace Assets.Scripts.Util
{
    public class Enums
    {
        public enum Direction { Up, Down, Left, Right, None };
        public enum PlayerState { Idle = 0, Moving, Hit, Dead, Attack, Jump };
        public enum GameStates { Menu, Running, CutScene, Paused, Dead };
        public enum BulletTypes {
            Pistol = 0, Destroyer, Malloc, Free, MachineGun, Player6, Player7, Player8,
            Enemy1, Enemy2, Enemy3, Enemy4, Beam 
        };
        public enum EnemiesLevel01 { Walkers, Flyers };
    }
}
