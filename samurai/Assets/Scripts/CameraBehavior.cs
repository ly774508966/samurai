using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public static CameraBehavior Instance;

    public Transform    owner;
    public Camera       ownerCamera;
    public Transform    target;
    public Vector3      offset = new Vector3(0, 8, -6.4f);

    float _curFovTime;
    float _fovTime;
    float _fovStart;
    float _fovEnd;    
    bool _fovOK;

    float _curSloTime;
    float _sloTime;
    float _timeScaleStart;
    float _timeScaleEnd;
    float _curTimeScale = 1;
    bool  _sloOK = true;
    public float _baseFov;

    // Use this for initialization
    void Start ()
    {
        _curFovTime = 0;
        _fovTime = 0;
        _fovStart = 0;
        _fovEnd = 0;
        if (ownerCamera != null)
        {
            _baseFov = ownerCamera.fieldOfView;
        }        
        _fovOK = true;
        Instance = this;
    }
	
	// Update is called once per frame
	void LateUpdate ()  
    {
        if (owner == null)
        {
            return;
        }

        if (Game.Instance.IsPause())
        {
            return;
        }

        UpdateFov();
        UpdateSloMotion();

        Vector3 destPos = GetDestPos();
        owner.position = Vector3.Lerp(owner.position, destPos, Time.deltaTime * 4);

        Vector3 dir = owner.forward;
        dir.y = 0;
        dir.Normalize();
        Vector3 t = target.position;
        t += dir * 0.7f;

        Vector3 lookAt = t - owner.position;
        lookAt.Normalize();

        owner.forward = Vector3.Lerp(owner.forward, lookAt, Time.deltaTime * 4);
    }

    Vector3 GetDestPos()
    {
        if (owner == null)
        {
            return Vector3.zero;
        }

        if (target == null)
        {
            return owner.position;
        }

        return target.position + offset * 0.9f;
    }

    void UpdateSloMotion()
    {
        if (_sloOK == false)
        {
            _curSloTime += Time.deltaTime;

            if (_curSloTime > _sloTime)
            {
                _curSloTime = _sloTime;
                _sloOK = true;
            }

            if (_curSloTime >= 0)
                _curTimeScale = Mathfx.Hermite(_timeScaleStart, _timeScaleEnd, _curSloTime / _sloTime);
        }
        Time.timeScale = _curTimeScale;
    }

    void UpdateFov()
    {
        if (_fovOK == false)
        {
            _curFovTime += Time.deltaTime;

            if (_curFovTime > _fovTime)
            {
                _curFovTime = _fovTime;
                _fovOK = true;
            }

            if (_curFovTime >= 0)
                ownerCamera.fieldOfView = Mathfx.Hermite(_fovStart, _fovEnd, _curFovTime / _fovTime);
        }        
    }

    public void RestoreTimeScaleAndFov()
    {
        CancelInvoke("RestoreTimeScaleAndFov");
        ChangeTimeScale(1, 0.4f);
        ChangeFov(_baseFov, 0.4f);       
    }

    public void ChangeFov(float newFov, float inTime)
    {
        CancelInvoke("RestoreTimeScaleAndFov");
        _curFovTime = 0;
        _fovTime = inTime;
        _fovStart = ownerCamera.fieldOfView;
        _fovEnd = newFov;
        _fovOK = false;
    }

    public void ChangeTimeScale(float newTimeScale, float inTime)
    {
        CancelInvoke("RestoreTimeScaleAndFov");        
        _curSloTime = 0;
        _sloTime = inTime;
        _timeScaleStart = Time.timeScale;
        _timeScaleEnd = newTimeScale;
        _sloOK = false;        
    }
}
