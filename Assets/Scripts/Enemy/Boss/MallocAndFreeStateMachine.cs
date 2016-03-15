using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class MallocAndFreeStateMachine
    {
        public enum State
        {
            Intro,
            GoToGround, GoToAir,
            Wait, Step, Jump, GroundAttack, Move, AirAttack,
            Stage2, 
            Hit
        }

        private State currState;
        private int stage;

        public MallocAndFreeStateMachine()
        {
            currState = State.Intro;
            stage = 0;
        }

        public State update(int health, int limit, bool animDone, bool goFirst, bool hit, bool waitOnPartner, bool sigWaitForAttack, bool sigDone, bool sigYourTurn)
        {
            switch (currState)
            {
                case State.Intro: currState = Intro(animDone, goFirst); break;
                case State.GoToGround: currState = GotoGround(animDone); break;
                case State.GoToAir: currState = GoToAir(animDone); break;
                case State.Wait: currState = Wait(animDone, hit, sigWaitForAttack); break;
                case State.Step: currState = Step(animDone); break;
                case State.Jump: currState = Jump(animDone); break;
                case State.GroundAttack: currState = GroundAttack(animDone); break;
                case State.Move: currState = Move(animDone, sigYourTurn); break;
                case State.AirAttack: currState = AirAttack(animDone); break;
                case State.Stage2: currState = Stage2(waitOnPartner); break;
                case State.Hit: currState = Hit(health, limit, animDone); break;
            }
            return currState;
        }


        //The following methods control when and how you can transition between states

        private State Intro(bool animDone, bool goFirst)
        {
            if (animDone)
            {
                if (goFirst)
                    return State.GoToGround;
                else
                    return State.GoToAir;
            }
            return State.Intro;
        }

        private State GotoGround(bool animDone)
        {
            if (animDone)
                return State.Wait;
            return State.GoToGround;
        }

        private State GoToAir(bool animDone)
        {
            if (animDone)
                return State.Move;
            return State.GoToAir;
        }

        private State Wait(bool animDone, bool hit, bool sigWaitForAttack)
        {
            if (hit)
                return State.Hit;
            if(animDone && !sigWaitForAttack)
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

        private State Move(bool animDone, bool sigYourTurn)
        {
            if (sigYourTurn)
                return State.GoToGround;
            if (animDone)
            {
                float r = Random.Range(0f, 1f);
                if (r < .20f)
                    return State.AirAttack;
            }
            return State.Move;
        }

        private State AirAttack(bool animDone)
        {
            if (animDone)
                return State.Move;
            return State.AirAttack;
        }

        private State Stage2(bool waitOnPartner)
        {
            stage = 1;
            if (waitOnPartner)
                return State.Stage2;
            return State.GoToGround;
        }

        private State Hit(int health, int limit, bool animDone)
        {
            if (animDone)
            {
                if (health == 0)
                {
                    if (stage == 1)
                        return State.Hit;
                    else
                        return State.Stage2;
                }
                if (health % limit == 0)
                    return State.GoToAir;
                return State.Wait;
            }
            return State.Hit;
        }
    }
}
