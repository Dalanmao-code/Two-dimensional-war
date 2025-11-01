using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMonoBase<T>:MonoBehaviour where T:SingleMonoBase<T>
{
    public static T INSTANCE; //实例

    protected virtual void Awake()
    {
        if (INSTANCE != null)
            Debug.LogError(name + "不符合单例模式");
        INSTANCE = (T)this;
    }

    protected virtual void OnDestroy()
    {
        INSTANCE = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
