using System;
using VoiceControl;

namespace DragonflyAddon
{
    public abstract class CheckSolve : ICheckSolve
    {
        public event Action Changed;
        private CheckState state;
        public CheckState State { get => state; private set { state = value; Changed?.Invoke(); } }
        public string ErrorMessage { get; protected set; }
        public string ManualOptions { get; protected set; }
        public string AutomatedAction { get; protected set; }
        public abstract bool InternalCheck();
        public abstract void InternalSolve();
        public CheckSolve(string error, string action)
        {
            ErrorMessage = error;
            AutomatedAction = ManualOptions = action;
        }
        public bool Check()
        {
            var result = InternalCheck();
            State = result ? CheckState.Good : CheckState.Bad;
            return result;
        }

        public void Solve()
        {
            State = CheckState.Working;
            InternalSolve();
            var result = InternalCheck();
            State = result ? CheckState.Good : CheckState.Failed;
        }
    }
}

