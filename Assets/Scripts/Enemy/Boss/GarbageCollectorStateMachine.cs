namespace Assets.Scripts.Enemy.Boss
{
    class GarbageCollectorStateMachine
    {
        public enum State { Intro, General, SuperStart, SuperWait }

        private State currState;

        public GarbageCollectorStateMachine()
        {
            currState = State.Intro;
        }

        public State update(bool animDone, bool superStart)
        {
            switch (currState)
            {
                case State.Intro: currState = Intro(animDone, superStart); break;
                case State.General: currState = General(animDone, superStart); break;
                case State.SuperStart: currState = SuperStart(animDone, superStart); break;
                case State.SuperWait: currState = SuperWait(animDone, superStart); break;
            }
            return currState;
        }


        //The following methods control when and how you can transition between states

        private State Intro(bool animDone, bool superStart)
        {
            if (animDone)
                return State.General;
            return State.Intro;
        }

        private State General(bool animDone, bool superStart)
        {
            if (superStart)
                return State.SuperStart;
            return State.General;
        }

        private State SuperStart(bool animDone, bool superStart)
        {
            if (animDone)
                return State.SuperWait;
            return State.SuperStart;
        }

        private State SuperWait(bool animDone, bool superStart)
        {
            if (animDone)
                return State.General;
            return State.SuperWait;
        }
    }
}
