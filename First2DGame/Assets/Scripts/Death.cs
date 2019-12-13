using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 动画播放完之后的回调
    /// </summary>
    public void AnimOverCallBack()
    {
        Destroy(gameObject);
    }
}
