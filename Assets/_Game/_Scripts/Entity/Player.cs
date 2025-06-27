namespace Game.Entity
{
    public interface IPlayer
    {
        IAliveEntity AliveEntity { get; }
    }

    public sealed class Player : BaseAliveEntity, IPlayer
    {
        public IAliveEntity AliveEntity => this;
    }
}