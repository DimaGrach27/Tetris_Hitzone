namespace Tetris.StateMachine
{
  public abstract class State
  {
    protected StateMachine StateMachine;

    public State(StateMachine stateMachine)
    {
      StateMachine = stateMachine;
    }
    
    public virtual void Enter() {}
    public virtual void Tick(float deltaTime) {}
    public virtual void Exit() {}
  }
}