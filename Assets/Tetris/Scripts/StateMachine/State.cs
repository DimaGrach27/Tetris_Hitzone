namespace Tetris.StateMachine
{
  public abstract class State
  {
    public virtual void Enter() {}
    public virtual void Tick(float deltaTime) {}
    public virtual void Exit() {}
  }
}