using Patterns;

public class StalkerFSM : FSM
{
    public StalkerFSM() : base() { }

    public void Add(StalkerFSMState state)
    {
        m_states.Add((int)state.ID, state);
    }

    public StalkerFSMState GetState(StalkerFSMStateType key)
    {
        return (StalkerFSMState)GetState((int)key);
    }

    public void SetCurrentState(StalkerFSMStateType stateKey)
    {
        State state = m_states[(int)stateKey];
        if (state != null)
        {
            SetCurrentState(state);
        }
    }
}

public class StalkerFSMState : State
{
    public StalkerFSMStateType ID { get { return _id; } }
    protected Stalker _stalker = null;
    protected StalkerFSMStateType _id;

    public StalkerFSMState(FSM fsm, Stalker stalker) : base(fsm)
    {
        _stalker = stalker;
    }

    public StalkerFSMState(Stalker stalker) : base(fsm: stalker.stalkerFSM)
    {
        _stalker = stalker;
        m_fsm = _stalker.stalkerFSM;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}