using System;
using System.Collections;
using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;

using UnityEngine;
using Random = System.Random;

namespace PixelCrew.Creatures.Hero
{
    public class Hero : Creature,ICanAddInventory
    {
        
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private ColliderCheck _wallCheck;
        
        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private CoolDown _throwCoolDown;
        [SerializeField] private RuntimeAnimatorController _armed;
        [SerializeField] private RuntimeAnimatorController _disarmed;

        [Header("Super throw")] [SerializeField]
        private CoolDown _superThrowCoolDown;
        [SerializeField] private int _superThrowParticles;
        [SerializeField] private float _superThrowDelay;
        [SerializeField] private ProbabilityDropComponent _hitDrop;
        [SerializeField] private SpawnComponent _throwSpawner;
        
        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");
        
        private readonly Collider2D[] _ineractionResult = new Collider2D[1];
    
       
        
        private bool _allowDoubleJump;
        private bool _isOnWall;
        private bool _superThrow;
        
        
        private GameSession _session;
        private HealthComponent _health;
        private float _defaultGravityScale;

        private const string SwordId = "Sword";
        
        private int  CoinsCount => _session.Data.Inventory.Count("Coin");
       
        private int SwordCount => _session.Data.Inventory.Count(SwordId);

        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;

        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId)
                    return SwordCount > 1;
                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
                
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        

       
        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChanged +=  OnInventoryChanged;
            
            
            _health.SetHealth(_session.Data.Hp.Value);
            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -=  OnInventoryChanged;
        }

      
        private void OnInventoryChanged(string id, int value)
        {
            if(id == SwordId)
                UpdateHeroWeapon();
        }
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }
        
        protected override void Update()
        {
           base.Update();
           var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
            Animator.SetBool(IsOnWall, _isOnWall);
        }

        

        protected override float CalculateYVelocity()
        {
            
            var isJumpPressing = Direction.y > 0;
            
            if (IsGrounded || _isOnWall) 
            {
                _allowDoubleJump = true;
            }
                if (!isJumpPressing && _isOnWall)
                {
                    return 0f;
                }
                
                return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump && _isOnWall)

        {
            
            _allowDoubleJump = false;
            DoJumpVfx();
            return _jumpSpeed;

        }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public void AddInventory( string id, int value)
        {
           _session.Data.Inventory.Add(id, value); 
        }
        

        
        public override void TakeDamage()
        {
            base.TakeDamage();
            if (CoinsCount > 0)
            {
                SpawnCoins();
            }
           
        }

        private void SpawnCoins()
        {
            
            var numCoinsToDispose = Mathf.Min(CoinsCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

            _hitDrop.SetCount(numCoinsToDispose);
           _hitDrop.CalculateDrop();
        }
        public void Interact()
        {
            _interactionCheck.Check();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
               var contact =  other.contacts[0];
               if (contact.relativeVelocity.y >= _slamDownVelocity)
               {
                   _particles.Spawn("SlawDown");
                  
               }
               
            }
        }

        
        public override void Attack()
        {
            
            if (SwordCount <= 0) return;
            
            base.Attack();
        }

       

       
        private void UpdateHeroWeapon()
        {
            
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
           
        }
        public void OnDoThrow()
        {
            if (_superThrow)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount -1: throwableCount;
                
                var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory();
            }

            _superThrow = false;
        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return  new WaitForSeconds(_superThrowDelay);
            }
        }

        private void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");
            
            
          var throwableId= _session.QuickInventory.SelectedItem.Id;
          var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
          _throwSpawner.SetPrefab(throwableDef.Projectile);
            _throwSpawner.Spawn();
            _session.Data.Inventory.Remove(throwableId, 1);
        }
     

        public void StartThrowing()
        {
            _superThrowCoolDown.Reset();
        }

        public void PerfomThrowing()
        {
            if (!_throwCoolDown.IsReady || !CanThrow) return;

            if (_superThrowCoolDown.IsReady) _superThrow = true;
            
            Animator.SetTrigger(ThrowKey);
            _throwCoolDown.Reset();
        }


        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }
    }
}