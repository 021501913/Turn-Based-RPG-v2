using System.Collections;
using System.Collections.Generic;
using NesScripts.Controls.PathFind;
using UnityEngine;
using TMPro;

public class Player : Actor
{
    void Awake()
    {
        isPlayer = true;
        isTurn = true;
        hasInput = true;
    }

    float angle = 90;
    public Vector2 forward = new Vector2(0, 1);
    public Vector2 right = new Vector2(1, 0);
    public Vector2 left = new Vector2(-1, 0);
    public Vector2 back = new Vector2(0, -1);

    public Quaternion targetRotation;

    private void Start()
    {

    }


    public override void MoveTo(Point pos)
    {
        base.MoveTo(pos);
        //anim.Play("Move");
        anim.Play("Move", -1, 0.0f);

    }

    public override void Update()
    {
        base.Update();

        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, Time.deltaTime * 400);

        if (isTurn && hasInput)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                MasMan.GridMan.ClearMap();
                MasMan.dungeonGenerator.MakeMap();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                angle -= 90;
                forward = Vector2FromAngle(angle);
                back = Vector2FromAngle(angle + 180);
                right = Vector2FromAngle(angle + 90);
                left = Vector2FromAngle(angle - 90);

                targetRotation = Quaternion.Euler(0, angle - 90, 0);

                //this.transform.rotation = Quaternion.Euler(0, angle - 90, 0);

                anim.Play("RotateLeft", -1, 0.0f);

            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                angle += 90;
                forward = Vector2FromAngle(angle);
                back = Vector2FromAngle(angle + 180);
                right = Vector2FromAngle(angle + 90);
                left = Vector2FromAngle(angle - 90);

                targetRotation = Quaternion.Euler(0, angle - 90, 0);

                //this.transform.rotation = Quaternion.Euler(0, angle - 90, 0);

                anim.Play("RotateRight", -1, 0.0f);
            }


            if (Input.GetKey(KeyCode.W))
            {
                Point newPos = new Point(position.x + Mathf.RoundToInt(-forward.x), position.y + Mathf.RoundToInt(forward.y));

                for (int i = 0; i < Actor.actors.Count; i++)
                {
                    if (Actor.actors[i].position == newPos)
                    {
                        if (Actor.actors[i] is BadGuy)
                        {
                            anim.Play("Attack", -1, 0.0f);
                            Actor.actors[i].TakeDamage(10);
                            CompleteTurn(20, 0);

                            return;
                        }
                        else if (Actor.actors[i] is StairsUp)
                        {
                            MasMan.GridMan.ClearMap();
                            MasMan.dungeonGenerator.MakeMap();
                        }
                    }
                }

                MoveInDirection(newPos);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Point newPos = new Point(position.x + Mathf.RoundToInt(-back.x), position.y + Mathf.RoundToInt(back.y));

                MoveInDirection(newPos);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Point newPos = new Point(position.x + Mathf.RoundToInt(-left.x), position.y + Mathf.RoundToInt(left.y));

                MoveInDirection(newPos);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Point newPos = new Point(position.x + Mathf.RoundToInt(-right.x), position.y + Mathf.RoundToInt(right.y));

                MoveInDirection(newPos);
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                CompleteTurn(20, 0);
            }
        }
    }

    public override void MoveInDirection(Point _pos)
    {
        base.MoveInDirection(_pos);
    }

    public override void CompleteTurn(float actionCost, float delay)
    {
        if (MasMan.InventoryManager.loot.open)
            MasMan.InventoryManager.loot.CloseLoot();

        base.CompleteTurn(actionCost, delay);
    }

    public override void TakeDamage(float _damage)
    {
        GameObject damageTextInstance = Instantiate(MasMan.PreMan.playerDamageText, null);
        damageTextInstance.SetActive(true);
        RectTransform rect = damageTextInstance.GetComponent<RectTransform>();
        rect.transform.SetParent(MasMan.MGMan.transform);
        rect.transform.GetComponentInChildren<TextMeshProUGUI>().text = _damage.ToString();
        rect.transform.localPosition = new Vector2(0, -200);
        Destroy(damageTextInstance, 5);

        base.TakeDamage(_damage);
    }

    public void Heal(float healAmount)
    {
        GameObject damageTextInstance = Instantiate(MasMan.PreMan.playerHealthText, null);
        damageTextInstance.SetActive(true);
        RectTransform rect = damageTextInstance.GetComponent<RectTransform>();
        rect.transform.SetParent(MasMan.MGMan.transform);
        rect.transform.GetComponentInChildren<TextMeshProUGUI>().text = healAmount.ToString();
        rect.transform.localPosition = new Vector2(0, -200);
        //damageTextInstance.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        //damageTextInstance.transform.GetComponentInChildren<TextMeshPro>().text = _damage.ToString();
        Destroy(damageTextInstance, 5);

        Player.player.healthPoints += healAmount;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        player = this;
    }
}
