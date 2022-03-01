using UnityEngine;
using System.Collections;

//计时器
public class Timer 
{
    //时长
	float m_duration = 0;
    //开始时间
	float m_startTime = 0;
    //开关
	bool m_enable = true;
    //单次或循环
	bool m_doOnce = true;
    private bool m_isPause = false;
    private float m_progressInPause = -1;

	//设置时长
	public void Set(float dur)
	{
		m_duration = dur;	
		m_startTime = Time.time;
		m_enable = true;
	    if (m_isPause)
	    {
	        m_isPause = false;
	        m_progressInPause = -1;
	    }
	}

	//设置单次循环
	public void SetDoOnce(bool b)
	{
		m_doOnce = b;	
	}

    public void SetPause(bool b)
    {
        if (m_isPause != b)
        {
            m_isPause = b;
            if (b)//开启暂停
            {
                //Debug.Log("存储的进度是 " + m_progressInPause);
                m_progressInPause = GetTimeProgress(); //记下刚开启暂停的进度
            }
            else//结束暂停
            {
                m_startTime = Time.time - m_progressInPause;//重置为正确的开始时间
                m_progressInPause = -1;
            }
        }
    }
	
    //判断时间抵达
	public bool IsTimeOut()
	{
		if (m_enable)
		{
		    if (m_isPause)
		    {
		        return false;
		    }
		    else
		    {
			if (Time.time - m_startTime > m_duration)
			{
				if (!m_doOnce)
					m_startTime = Time.time;
				
				return true;			
			}
			else
				return false;
		    }
		}
		else
			return true;
	}
    public void ReCount()
    {
        m_startTime = Time.time;
    }
	
    //获取进度百分比
	public float GetTimePercent()
	{
		if (m_enable)
			return (Time.time - m_startTime) / m_duration;
		else
			return 0;
	}
	
    //获取进度时间
	public float GetTimeProgress()
	{
        if (m_isPause && m_progressInPause != -1)//暂停中，且暂停进度储存
        {
            return m_progressInPause;
        }
        else
        {
            if (m_enable)
                return (Time.time - m_startTime);
            else
                return 0;
        }
	}
	
    //获取剩余时间
	public float GetTimeLeft()
	{
		if (m_enable)
			return m_duration - (Time.time - m_startTime);
		else
			return 0;
	}
	
    //开关
	public void SetEnable(bool b)
	{
		m_enable = b;
	}
	
    //增加时间
	public void Add(float dur) {
		m_duration += dur;
	}

    //获取开关
    public bool GetEnable()
    {
        return m_enable;
    }
}
