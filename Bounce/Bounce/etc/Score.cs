using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct Score
{
    private float totalScore;
    public float TotalScore
    {
        get { return totalScore; }
    }

    private float originalMultiplier;
    private float multiplier;
    public float Multiplier
    {
        get { return multiplier; }
        set { multiplier = value; }
    }

    public Score(float multiplier)
    {
        totalScore = 0;

        if (multiplier <= 0)
            throw new InvalidOperationException("Your multiplier must be greater than zero.");

        this.multiplier = originalMultiplier = multiplier;
    }

    public void Increment(float amount)
    {
        totalScore += amount * multiplier;
    }

    public void ResetMultiplier()
    {
        multiplier = originalMultiplier;
    }

    public override string ToString()
    {
        return totalScore.ToString();
    }
}