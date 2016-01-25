
namespace Assets.Scripts.Managers
{
    public interface Callback
    {
        /// <summary> Callback function to let handlers know of entity death. </summary>
        /// <param name="entity"> The entity that died. </param>
        void entityDied(Entity entity);
    }
}
