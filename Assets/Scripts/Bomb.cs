﻿using UnityEngine;

public class Bomb : MonoBehaviour
 {
    public float Lifetime;

    public float ExplosionForce;
    public float ExplosionRadius;
    public float PlayerPowerDrain;

    public Material BaseMaterial;
    public Material FlashMaterial;

    private bool _fuseLit = false;
    private float _secondsLit;

    private BombManager _bombManager;
    private GameManager _gameManager;
    private MeshRenderer _meshRenderer;

    private bool _expolded = false;

	void Start () 
	{
        _bombManager = GameObject.Find("Managers").GetComponent<BombManager>();
        _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }
	
	void Update () 
	{
        if (_expolded) return;

		if (_fuseLit)
        {
            _secondsLit += Time.deltaTime;
            if (_secondsLit > Lifetime)
            {
                // Explode!
                Collider[] objects = Physics.OverlapSphere(transform.position, ExplosionRadius);
                foreach (Collider c in objects)
                {
                    Rigidbody rb = c.GetComponent<Rigidbody>();
                    if (rb) rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);

                    if (c.CompareTag("Player"))
                    {
                        PowerManager powerManager = c.GetComponent<PowerManager>();
                        powerManager.OnBombExplosion(this);

                        _gameManager.OnPlayerDamage();
                    }
                }

                _expolded = true;
                _gameManager.OnExplosion();
                _bombManager.OnBombExplosion(this);
            }
            else
            {
                // Start flashing in second half of life
                if (_secondsLit > Lifetime / 2.0f)
                {
                    float flashRate;
                    float lifeRemaining = Lifetime - _secondsLit;
                    // Flash faster in last quarter of life
                    if (lifeRemaining < Lifetime / 4.0f)
                    {
                        flashRate = Lifetime / 16.0f;
                    }
                    else
                    {
                        flashRate = Lifetime / 8.0f;
                    }

                    Material mat = (Mathf.Repeat(lifeRemaining, flashRate) < flashRate / 2.0f) ?
                        FlashMaterial : BaseMaterial;

                    _meshRenderer.material = mat;
                }
            }
        }
	}

    public void LightFuse()
    {
        _fuseLit = true;
    }
}
