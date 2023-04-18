using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour, ISaveable
{

    [SerializeField] bool once;
    bool done;

 

    public virtual void Act()
    {

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (done) return;
        if (collision.gameObject.name != "Player") return;
        if (once) done = true;
        Act();
    }

    #region SAVE
    public object CaptureState()
    {
        return new SaveData
        {
            done = done
        };
    }

    public void RestoreState(object state)
    {
        var save = (SaveData)state;
        done = save.done;
    }
    [Serializable]
    struct SaveData
    {
        public bool done;
    }
    #endregion
}
