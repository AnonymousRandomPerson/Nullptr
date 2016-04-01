using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class STSStateMachine
    {
        public enum State
        {
            Intro,
            Wait, Step, Jump, GroundAttack,
            Hit
        }

        private State currState;
        private int stage;

        public STSStateMachine()
        {
            currState = State.Intro;
            stage = 0;
        }

        public State update(int health, bool animDone, bool hit, bool onGround)
        {
            switch (currState)
            {
                case State.Intro: currState = Intro(animDone); break;
                case State.Wait: currState = Wait(animDone, hit); break;
                case State.Step: currState = Step(animDone); break;
                case State.Jump: currState = Jump(animDone); break;
                case State.GroundAttack: currState = GroundAttack(animDone); break;
                case State.Hit: currState = Hit(health, animDone, onGround); break;
            }
            return currState;
        }


        //The following methods control when and how you can transition between states

        private State Intro(bool animDone)
        {
            if (animDone)
                return State.Wait;
            return State.Intro;
        }

        private State Wait(bool animDone, bool hit)
        {
            if (hit)
                return State.Hit;
            if (animDone)
            {
                float r = Random.Range(0f, 1f);
                if (r < .40f)
                    return State.GroundAttack;
                if (r < .60f)
                    return State.Jump;
                return State.Step;
            }
            return State.Wait;
        }

        private State Step(bool animDone)
        {
            if (animDone)
                return State.Wait;
            return State.Step;
        }

        private State Jump(bool animDone)
        {
            if (animDone)
                return State.Wait;
            return State.Jump;
        }

        private State GroundAttack(bool animDone)
        {
            if (animDone)
                return State.Wait;
            return State.GroundAttack;
        }

        private State Hit(int health, bool animDone, bool onGround)
        {
            if (animDone)
            {
                float r = Random.Range(0f, 1f);
                if (!onGround || r < .40f)
                    return State.GroundAttack;
                return State.Jump;
            }
            return State.Hit;
        }
    }
}
