//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: This object can be set on fire
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	public class FireSource : MonoBehaviour
	{
		public GameObject fireParticlePrefab;
		public bool startActive;
		protected GameObject fireObject;

		public ParticleSystem customParticles;

		public bool isBurning;

		public float burnTime;
		public float ignitionDelay = 0;
		protected float ignitionTime;

		private Hand hand;

		public AudioSource ignitionSound;

		public bool canSpreadFromThisSource = true;
        public bool isDisabled = false;

		//-------------------------------------------------
		void Start()
		{
			if ( startActive )
			{
				StartBurning();
			}
		}


		//-------------------------------------------------
		void Update()
		{
			if ( ( burnTime != 0 ) && ( Time.time > ( ignitionTime + burnTime ) ) && isBurning )
			{
				isBurning = false;
				if ( customParticles != null )
				{
					customParticles.Stop();
				}
				else
				{
					Destroy( fireObject );
				}
			}
		}


		//-------------------------------------------------
		void OnTriggerEnter( Collider other )
		{
			if ( isBurning && canSpreadFromThisSource )
			{
				other.SendMessageUpwards( "FireExposure", SendMessageOptions.DontRequireReceiver );
			}
            //else if (isBurning)
            //{
            //    other.gameObject.SendMessageUpwards("TookDamage", 1);
            //}
		}

        //-------------------------------------------------
        protected void FireExposure()
		{
            if (isDisabled)
            {
                return;
            }

            if ( fireObject == null )
			{
				Invoke( "StartBurning", ignitionDelay );
			}

			if ( hand = GetComponentInParent<Hand>() )
			{
				hand.TriggerHapticPulse( 1000 );
			}
		}


		//-------------------------------------------------
		protected void StartBurning()
		{
            if (isDisabled)
            {
                return;
            }

			isBurning = true;
			ignitionTime = Time.time;

			// Play the fire ignition sound if there is one
			if ( ignitionSound != null )
			{
				ignitionSound.Play();
			}

			if ( customParticles != null )
			{
				customParticles.Play();
			}
			else
			{
				if ( fireParticlePrefab != null )
				{
                    fireObject = Instantiate( fireParticlePrefab, transform.position, transform.rotation ) as GameObject;
					fireObject.transform.parent = transform;
				}
			}
		}
	}
}
