using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class KernelHandStateMachine
    {
        public enum State
        {
            Intro,
            Wait, Hover, Slam, Drop,
            Hit, Dead
        }

        private State currState;

        public KernelHandStateMachine()
        {
            currState = State.Intro;
        }

        public State update(int health, bool animDone, bool hit, bool revive)
        {
            switch (currState)
            {
                case State.Intro: currState = Intro(animDone); break;
                case State.Wait: currState = Wait(animDone, hit); break;
                case State.Hover: currState = Hover(animDone); break;
                case State.Slam: currState = Slam(animDone); break;
                case State.Drop: currState = Drop(animDone); break;
                case State.Hit: currState = Hit(health, animDone); break;
                case State.Dead: currState = Dead(revive); break;
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
                    return State.Drop;
                if (r < .60f)
                    return State.Slam;
                return State.Hover;
            }
            return State.Wait;
        }

        private State Hover(bool animDone)
        {
            if (animDone)
            {
                float r = Random.Range(0f, 1f);
                if (r < .40f)
                    return State.Drop;
                return State.Slam;
            }
            return State.Hover;
        }

        private State Slam(bool animDone)
        {
            if (animDone)
                return State.Wait;
            return State.Slam;
        }

        private State Drop(bool animDone)
        {
            if (animDone)
                return State.Wait;
            return State.Drop;
        }

        private State Hit(int health, bool animDone)
        {
            if (animDone)
            {
                if (health <= 0)
                    return State.Dead;
                float r = Random.Range(0f, 1f);
                if (r < .40f)
                    return State.Wait;
                return State.Hover;
            }
            return State.Hit;
        }

        private State Dead(bool revive)
        {
            if (revive)
                return State.Wait;
            return State.Dead;
        }
    }
}