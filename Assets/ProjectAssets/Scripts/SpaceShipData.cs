using UnityEngine;

[CreateAssetMenu(fileName = "SpaceShip Data", menuName = "ScriptableObjects/Player Skins/SpaceShip Data", order = 1)]
public class SpaceShipData : ScriptableObject
{
    [Header("Visuals")]
    [SerializeField] private Sprite shipSprite;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float handling = 5;
    [SerializeField] private int scoreSpeed = 1;

    [Header("Combat")]
    [SerializeField] private int projectileDamage = 1;

    [Header("Firing")]
    [SerializeField] private float fireRate = 0.5f;


    public Sprite ShipSprite
    {
        get
        {
            return shipSprite;
        }
        set
        {
            shipSprite = value;
        }
    }
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }
    public float Handling
    {
        get
        {
            return handling;
        }
        set
        {
            handling = value;
        }
    }
    public int ScoreSpeed
    {
        get
        {
            return scoreSpeed;
        }
        set
        {
            scoreSpeed = value;
        }
    }

    public int ProjectileDamage
    {
        get
        {
            return projectileDamage;
        }
        set
        {
            projectileDamage = value;
        }
    }

    public float FireRate
    {
        get
        {
            return fireRate;
        }
        set
        {
            fireRate = value;
        }
    }
}