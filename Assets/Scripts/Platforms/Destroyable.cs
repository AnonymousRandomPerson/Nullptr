namespace Assets.Scripts.Platforms
{
    /// <summary>
    /// An object that can be destroyed by a destroyer bullet.
    /// </summary>
    public interface Destroyable
    {

        /// <summary>
        /// Triggers an event when the object is destroyed.
        /// </summary>
        void Destroy();
    }
}