using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
     [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class AnimationController : KinematicObject
    {
        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;
        public Vector2 move;
        public bool jump;
        public bool stopJump;

        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        private bool isDead = false;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void ComputeVelocity()
        {
            if (isDead) return;

            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        /// <summary>
        /// Plays the death animation and stops movement.
        /// </summary>
        public void PlayDeath()
        {
            isDead = true;
            move = Vector2.zero;
            targetVelocity = Vector2.zero;
            velocity = Vector2.zero;

            if (animator != null)
            {
                animator.SetTrigger("die");
            }

            // Optional: disable collider or logic scripts after death animation
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }

        /// <summary>
        /// Optionally play other triggers like hurt, attack, etc.
        /// </summary>
        public void PlayTrigger(string triggerName)
        {
            if (!isDead && animator != null)
                animator.SetTrigger(triggerName);
        }
    }
}