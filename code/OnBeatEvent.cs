using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnBeatEvent : MonoBehaviour
{
    [SerializeField] Dread dread;
    [SerializeField] Qin qin;

    Hashtable attackTable = new Hashtable();
    int trueAttackIdx = 0;
    bool oddOdds = false; // on every trigger this flips, so the attacks alternate between the first and the second of each segment

    [Header("Spectrum1")]
    public float[] spectrum1;

    [Header("Spectrum2")]
    public float[] spectrum2;

    [Header("Spectrum3")]
    public float[] spectrum3;

    [Header("Spectrum4")]
    public float[] spectrum4;

    [Header("Spectrum5")]
    public float[] spectrum5;

    private void SetUpAttackTable(float[] odds) // can't have 2 equal odds!
    {
        for(int i = 1; i <= odds.Length; i++)
        {
            attackTable[odds[i - 1]] = i;

        }
    }

    private int WhichAttack(float[] passedOdds)
    {
        var odds = (float[])passedOdds.Clone();

        SetUpAttackTable(odds);

        float rnd = UnityEngine.Random.Range(0f, 100f);

        Array.Sort(odds);
        

        float totalOdds = odds[0];

        if(rnd < totalOdds)
        {
            return (int)attackTable[odds[0]];
        }
        else 
        {
            totalOdds += odds[1];
            if (rnd < totalOdds)
                return (int)attackTable[odds[1]];
            else 
            {
                totalOdds += odds[2];
                if (rnd < totalOdds)
                    return (int)attackTable[odds[2]];
                else 
                {
                    totalOdds += odds[3];
                    if (rnd < totalOdds)
                    {
                        if (oddOdds)
                            return (int)attackTable[odds[4]];
                        else
                            return (int)attackTable[odds[3]];
                    } 
                    else 
                    {
                        totalOdds += odds[4];
                        if (rnd < totalOdds)
                        {
                            if (oddOdds)
                                return (int)attackTable[odds[3]];
                            else
                                return (int)attackTable[odds[4]];
                        }
                        else
                        {
                            return 0; // error case, should not happen
                        }
                    }
                }
            }
        }
    }

    public void OnBeat(int spectrumIdx)
    {
        // oddOdds = !oddOdds;

        switch(spectrumIdx)
        {
            case 1:
                {
                    trueAttackIdx = WhichAttack(spectrum1);
                    break;
                }

            case 2:
                {
                    trueAttackIdx = WhichAttack(spectrum2);
                    break;
                }

            case 3:
                {
                    trueAttackIdx = WhichAttack(spectrum3);
                    break;
                }

            case 4:
                {
                    trueAttackIdx = WhichAttack(spectrum4);
                    break;
                }

            case 5:
                {
                    trueAttackIdx = WhichAttack(spectrum5);
                    break;
                }
        }

        switch(qin.Attack1.enabled)
        {
            case true: // qin is not controlled by player
                {
                    qin.callAttack(trueAttackIdx); // we want the coroutine to start in qin, not in here
                    dread.callDefenseOnBeat();
                    break;
                }

            case false: // interesting problem: how to get rid of the doubled switch without inheritance. (was a bigger problem when the cases had 25 lines)
                {
                    dread.callAttack(trueAttackIdx);
                    qin.callDefenseOnBeat();
                    break;
                }
        }
    }
}
