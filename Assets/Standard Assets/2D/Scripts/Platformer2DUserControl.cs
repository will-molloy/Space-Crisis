using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]

    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        public String jumpKey;
        public String horizontalKey;
        public KeyCode ctrlKey;

        private bool swinging = false;
        private bool canSwing = true;

        public Lever lever
        {
            get;
            set;
        }


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown(jumpKey);
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            if(crouch && lever != null)
            {
                lever.activate();
            }
            if (swinging == false)
            {
                
                float h = CrossPlatformInputManager.GetAxis(horizontalKey);
                // Pass all parameters to the character control script.
                m_Character.Move(h, crouch, m_Jump);
                m_Jump = false;
            } else
            {
                float h = CrossPlatformInputManager.GetAxis(horizontalKey);
                if (m_Jump)
                {
                    GetComponent<BoxCollider2D>().enabled = false;

                    swinging = false;
                    Destroy(GetComponent<HingeJoint2D>());
                    
                    m_Character.Move(h, crouch, m_Jump);
                    m_Jump = false;
                    StartCoroutine(Wait());
                } else
                {
                    m_Character.Move(h, crouch, m_Jump);
                }

                
            }

        }

        private IEnumerator Wait()
        {

            yield return new WaitForSeconds(0.5f);
            GetComponent<BoxCollider2D>().enabled = true;
            canSwing = true;

        }

        void OnCollisionStay2D(Collision2D collider)
        {
            Debug.Log(collider);
            if (collider.gameObject.tag == "Rope" && canSwing == true)
            {

                canSwing = false;
                swinging = true;

                HingeJoint2D hinge = gameObject.AddComponent<HingeJoint2D>() as HingeJoint2D;
                hinge.connectedBody = collider.gameObject.GetComponent<Rigidbody2D>();
            }
            /*
            Debug.Log(collider);
            Debug.Log(collider.gameObject.layer);
            Debug.Log(m_WhatIsGround.value);
            if (collider.gameObject.layer == m_WhatIsGround)
            {
                m_Grounded = true;
                m_Anim.SetBool("Ground", m_Grounded);
            }
            */
            //CheckIfGrounded();
        }

    }
}
