using UnityEngine;

[CreateAssetMenu(fileName = "Attacker Stats", menuName = "ScriptableObjects/Attacker/Attacker Stats", order = 1)]
public class AttackerStats : ScriptableObject
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int score = 10;

    public float Speed
    {
        get 
        { 
            return speed; 
        }
        set 
        { 
            speed = value; 
        }
    }
    public int Damage
    {
        get 
        { 
            return damage; 
        }
        set 
        { 
            damage = value; 
        }
    }
    public int Score
    {
        get 
        { 
            return score; 
        }
        set 
        { 
            score = value; 
        }
    }
}