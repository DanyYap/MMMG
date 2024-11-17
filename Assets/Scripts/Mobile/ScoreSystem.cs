using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private bool isPutOutFire;
    private int m_hostageCount = 0;
    private int m_assetCount = 0;
    private int m_dieCount = 0;
    private int m_starCount = 0;


    private void Update()
    {
        ScoreCalculate();
    }

    public void OnPutOutFire()
    {
        isPutOutFire = true;
    }

    public void OnAssetBurned()
    {
        m_assetCount++;
    }
    
    public void OnHostageRescue()
    {
        m_hostageCount++;
    }

    public void OnPlayerDieCount()
    {
        m_dieCount++;
    }

    void ScoreCalculate()
    {
        if (isPutOutFire)
        {
            m_starCount++;
        }

        if (m_assetCount <= 10)
        {
            m_starCount++;
        }
        
        if (m_assetCount <= 20)
        {
            m_starCount++;
        }
        
        if (m_hostageCount == 4)
        {
            m_starCount++;
        }
        
        if (m_dieCount < 4)
        {
            m_starCount++;
        }
    }
    
}
