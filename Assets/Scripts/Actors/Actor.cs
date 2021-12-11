using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;
using NesScripts.Controls.PathFind;

public class Actor : MonoBehaviour
{
    public static Player player;
    public static StairsUp stairsUp;

    public static List<Actor> actors = new List<Actor>();
    public static bool IsItSomeoneElsesTurn(Actor _actor)
    {
        for (int i = 0; i < Actor.actors.Count; i++)
            if (Actor.actors[i].isTurn)
                if (Actor.actors[i] != _actor)
                    return true;

        return false;
    }

    public float actionPoints = 0; // at 100 turn starts
    public float healthPoints = 100; // at 0 dies

    public bool isTurn = false;
    public bool isPlayer = false;
    public bool hasInput = false;

    [SerializeField]
    public Point position;

    public Slider healthSlider;
    public Slider actionPointsSlider;

    public Vector3 target;
    private Vector3 _vel;

    public Vector2Int startingPosition;

    public Animator anim;

    public virtual void Update()
    {
        CheckForTurnStart();

        this.transform.position = Vector3.SmoothDamp(this.transform.position, target, ref _vel, 0.1f);
    }


    public void MoveToStartingPosition()
    {
        MoveTo(new Point(startingPosition.x, startingPosition.y));
    }

    public void CheckForTurnStart()
    {
        if (!Actor.IsItSomeoneElsesTurn(this) && isTurn == false)
        {
            if (actionPoints > 100)
            {
                isTurn = true;
                hasInput = true;
            }
            else
            {
                actionPoints += Time.deltaTime * 50;
            }
        }
    }

    public virtual void MoveTo(Point pos)
    {

        position = pos;
        target = new Vector3(pos.x * 5, 1.7f, pos.y * 5);
    }

    public virtual void TakeDamage(float _damage)
    {
        healthPoints -= _damage;
    }

    public virtual void OnEnable()
    {
        actors.Add(this);
    }

    public virtual void OnDisable()
    {
        MasMan.GridMan.tilesmap[position.x, position.y] = 1;
        MasMan.GridMan.grid.UpdateGrid(MasMan.GridMan.tilesmap);

        actors.Remove(this);
    }

    public void LateUpdate()
    {
        if (healthSlider != null)
        {
            healthSlider.value = healthPoints / 100;
            actionPointsSlider.value = actionPoints / 100;
        }
    }

    public virtual void CompleteTurn(float actionCost, float delay)
    {
        hasInput = false;
        if (delay > 0)
            StartCoroutine(CompleteTurnCor(actionCost, delay));
        else
            CompleteTurnSimple(actionCost);
    }

    public virtual IEnumerator CompleteTurnCor(float actionCost, float delay)
    {
        yield return new WaitForSeconds(delay);
        CompleteTurnSimple(actionCost);
    }

    private void CompleteTurnSimple(float actionCost)
    {
        hasInput = true;
        actionPoints -= actionCost;

        isTurn = false;
    }

    public Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    

    public virtual void MoveInDirection(NesScripts.Controls.PathFind.Point _pos)
    {
        if (IsWithinBounds(_pos))
        {
            List<Point> path =
            Pathfinding.FindPath(
            MasMan.GridMan.grid,
            position,
            _pos,
            Pathfinding.DistanceType.Manhattan);

            if (path.Count > 0)
            {
                position = _pos;
                MoveTo(_pos);
            }

            CompleteTurn(20, 0);
        }
    }

    public bool IsWithinBounds(Point _pos)
    {
        if ((_pos.x < MasMan.GridMan.tilesmap.GetLength(0) && _pos.x >= 0)
            &&
            (_pos.y < MasMan.GridMan.tilesmap.GetLength(1) && _pos.y >= 0))
        {
            return true;
        }

        Debug.Log(this.gameObject.ToString() + " |" + _pos.x + "," + _pos.y + " | Out of bounds");
        return false;
    }
}
