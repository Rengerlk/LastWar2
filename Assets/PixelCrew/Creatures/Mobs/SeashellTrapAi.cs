using System;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Utils;
using TMPro;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class ShootingTrapAi : MonoBehaviour
    {
        [SerializeField] private ColliderCheck _vision;

        [Header("Melee")]
        [SerializeField] private CoolDown _meleeCoolDown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private ColliderCheck _meleeCanAttack;

        [Header("Rnage")]
        [SerializeField] private CoolDown _rangeCoolDown;
        [SerializeField] private SpawnComponent _rangeAttack;
        
        private static readonly int Melee = Animator.StringToHash("melee");
        private static readonly int Range = Animator.StringToHash("range");
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCoolDown.IsReady)
                        MeleeAttack();
                    return;
                }

                if (_rangeCoolDown.IsReady)
                    RangeAttack();
            }
        }

        private void RangeAttack()
        {
            _rangeCoolDown.Reset();
           _animator.SetTrigger(Range);
        }

        private void MeleeAttack()
        {
            _meleeCoolDown.Reset();
            _animator.SetTrigger(Melee);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void onRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}