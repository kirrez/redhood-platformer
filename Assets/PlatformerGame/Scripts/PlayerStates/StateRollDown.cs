//using UnityEngine;

//namespace Platformer.PlayerStates
//{
//    public class StateRollDown : IState
//    {
//        private Player Model;

//        public StateRollDown(Player model)
//        {
//            Model = model;
//        }

//        public void Update()
//        {
//            //if (Timer <= 0)
//            //{
//            //    Model.GetInput();
//            //}
//        }

//        public void HealthChanged()
//        {
//            Model.ChangeHealthUI();
//            Model.SetState(Model.StateDamageTaken);
//        }

//        public void OnEnable(float time = 0f)
//        {
//            Model.Timer = time;

//            Model.SitDown();

//            Model.Rigidbody.velocity = Vector2.zero; // slips anyway, but quite slowly

//            var x = Model.Facing == EFacing.Right ? 1 : -1;
//            Model.Rigidbody.AddForce(new Vector2(x * Model.RollDownForce, 0f));
//        }

//        public void FixedUpdate()
//        {
//            Model.SetDeltaY();
//            Model.UpdateAttackTimers();

//            Model.Timer -= Time.fixedDeltaTime;

//            if (Model.Timer <= 0)
//            {
//                // Idle and Walk
//                if (!Model.Ceiled(LayerMasks.Solid) && Model.Vertical > -1)
//                {
//                    if (Model.Horizontal == 0)
//                    {
//                        Model.Animations.Idle();
//                        Model.SetState(Model.StateIdle);
//                    }
//                    else if (Model.Horizontal != 0)
//                    {
//                        Model.Animations.Walk();
//                        Model.SetState(Model.StateWalk);
//                    }
//                }

//                // Sit and Crouch
//                if (Model.Ceiled(LayerMasks.Solid) || Model.Vertical == -1)
//                {
//                    if (Model.Horizontal == 0)
//                    {
//                        Model.Animations.Sit();
//                        Model.SetState(Model.StateSit);
//                    }
//                    else if (Model.Horizontal != 0)
//                    {
//                        Model.Animations.Crouch();
//                        Model.SetState(Model.StateSitCrouch);
//                    }
//                }
//            }

//            // State Jump Rising without hitting "Jump" button ))
//            if (Model.DeltaY > 0 && !Model.Grounded(LayerMasks.Walkable))
//            {
//                Model.Timer = 0f;
//                //Model.UpdateInAir(true);
//                Model.Animations.JumpRising();
//                Model.SetState(Model.StateJumpRising);
//            }

//            // State Jump Falling, something disappeared right beneath your feet or you slided down from a solid surface
//            if (Model.DeltaY < 0 && !Model.Grounded(LayerMasks.Walkable))
//            {
//                Model.Timer = 0f;
//                //Model.UpdateInAir(true);
//                Model.Animations.JumpFalling();
//                Model.SetState(Model.StateJumpFalling, 0.75f);
//            }
//        }
//    }
//}