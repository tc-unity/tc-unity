using UnityEngine;
using System.Collections;

//��ʱ��
public class Timer 
{
    //ʱ��
	float m_duration = 0;
    //��ʼʱ��
	float m_startTime = 0;
    //����
	bool m_enable = true;
    //���λ�ѭ��
	bool m_doOnce = true;
    private bool m_isPause = false;
    private float m_progressInPause = -1;

	//����ʱ��
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

	//���õ���ѭ��
	public void SetDoOnce(bool b)
	{
		m_doOnce = b;	
	}

    public void SetPause(bool b)
    {
        if (m_isPause != b)
        {
            m_isPause = b;
            if (b)//������ͣ
            {
                //Debug.Log("�洢�Ľ����� " + m_progressInPause);
                m_progressInPause = GetTimeProgress(); //���¸տ�����ͣ�Ľ���
            }
            else//������ͣ
            {
                m_startTime = Time.time - m_progressInPause;//����Ϊ��ȷ�Ŀ�ʼʱ��
                m_progressInPause = -1;
            }
        }
    }
	
    //�ж�ʱ��ִ�
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
	
    //��ȡ���Ȱٷֱ�
	public float GetTimePercent()
	{
		if (m_enable)
			return (Time.time - m_startTime) / m_duration;
		else
			return 0;
	}
	
    //��ȡ����ʱ��
	public float GetTimeProgress()
	{
        if (m_isPause && m_progressInPause != -1)//��ͣ�У�����ͣ���ȴ���
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
	
    //��ȡʣ��ʱ��
	public float GetTimeLeft()
	{
		if (m_enable)
			return m_duration - (Time.time - m_startTime);
		else
			return 0;
	}
	
    //����
	public void SetEnable(bool b)
	{
		m_enable = b;
	}
	
    //����ʱ��
	public void Add(float dur) {
		m_duration += dur;
	}

    //��ȡ����
    public bool GetEnable()
    {
        return m_enable;
    }
}
