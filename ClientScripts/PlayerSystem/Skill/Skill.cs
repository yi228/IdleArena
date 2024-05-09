using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skill : MonoBehaviour
{
    public string skillName;
    public float attackMultiple;
    public float duration;
    public int unlockLevel;
    public int price;
    public Sprite icon;
    public string soundKey;

    public float appliedDamage {  get; private set; }

    private float activateTime = 0.2f;
    private Collider2D coll;

    public enum DirType
    {
        Center,
        LR
    }
    public DirType dirType;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainScene")
        {
            coll = GetComponent<Collider2D>();
            StartCoroutine(CoSetCollider());
        }
        Destroy(gameObject, duration);
    }
    private IEnumerator CoSetCollider()
    {
        coll.enabled = false;
        yield return new WaitForSeconds(activateTime);
        coll.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{skillName} to {collision.name}");
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if (collision.CompareTag("Monster"))
            {
                appliedDamage = (GameManager.instance.player.stat.attack + GameManager.instance.player.extraAttack) * attackMultiple;
                GameManager.instance.player.InstantiateDamageText(appliedDamage, true);

                collision.GetComponentInParent<GoblinController>().OnDamaged(appliedDamage);
            }
        } 
    }
}
