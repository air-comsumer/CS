using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    public Dictionary<string, UIBehaviour> comDic = new Dictionary<string, UIBehaviour>();
    // Start is called before the first frame update
    void Start()
    {
        FindCompoments<Button>();
    }

    // Update is called once per frame
    public void FindCompoments<T>() where T : UIBehaviour
    {
        T[] components = GetComponentsInChildren<T>();
        foreach (var component in components)
        {
            comDic.Add(component.gameObject.name, component);
        }
    }
    void Update()
    {
        
    }
}
