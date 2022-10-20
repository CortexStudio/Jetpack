using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Scores
    public delegate void CollectionScore(int score);
    public static event CollectionScore OnCollectionScore;

    private int scores;

    public int Scores { get => scores; set => scores = value; }

    void Start()
    {
        
    }

    public void AddScore(int scores)
    {
        this.scores += scores;

        if (OnCollectionScore != null)
            OnCollectionScore(this.scores);
    }

}
