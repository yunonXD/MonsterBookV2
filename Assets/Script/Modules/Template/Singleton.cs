using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̱��� ������ �����Ѵ�.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public bool IsDontDestroy = true;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {

                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError("�� ���� " + typeof(T).ToString() + " ��(��) �������� �ʽ��ϴ�.");
                    Debug.Break();
                }

            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            if (IsDontDestroy)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}