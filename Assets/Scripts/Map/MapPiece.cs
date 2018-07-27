using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class MapPiece {

    public enum MapType
    {
        FixObject = 0,
        Key = 1,
        ShiftCrystal = 2,
        TransferCrystal = 3
    }


    //对应游戏对象
    public GameObject gameObject;
    
    public MapType type = MapType.FixObject;
    public int index;
    public string fileName;
    public Vector2 originalPosition;
    public float originalRotation;
    public Vector2 originalScale;
    public List<int> children;
    public float duration = 0.0f;
    public bool originalVisible;
    public List<Key.KeyTrigger> triggers = null;
    public int transferCrystalTrigger = 0;
    public ShiftCrystal.ShiftCrystalTrigger shiftCrystalTrigger = new ShiftCrystal.ShiftCrystalTrigger(new List<int>(), true);

    //存档中会保存以下状态，同时也是管理对应地图部分的状态（不管游戏对象有没有生成），
    //会根据这里记录的当前位置等决定要不要生成或者移动到视野内，因此根据CountDown如果触发了移动等每帧都会先修改这个然后MapManager发现位置出现偏差再移动
    //当前位置,规约到0-8640
    public Vector2 currentPosition;
    //即将移动到的目标位置与当前位置的偏移
    public Vector2 targetPositionOffset;
    //几秒后移动到目标位置
    public float moveCountDown;

    //当前旋转
    public float currentRotation;
    //即将旋转到的角度与当前的度数差
    public float targetRotationOffset;
    //几秒后旋转到目标角度
    public float rotationCountDown;

    //当前缩放
    public Vector2 currentScale;
    //目标缩放与当前缩放的比例
    public Vector2 targetScaleOffset;
    //几秒后达到目标缩放
    public float scaleCountDown;

    //当前可见性
    private bool visible = false;
    public bool Visible
    {
        get { return visible; }
        set
        {
            visible = value;
            changed = true;
        }
    }
    //是否可触发
    private bool triggerable;
    public bool Triggerable
    {
        get { return triggerable; }
        set
        {
            triggerable = value;
            changed = true;
        }
    }
    //几秒后切换触发状态
    public float triggerableCountDown;
    //是否可加载
    private bool loadable;
    public bool Loadable
    {
        get { return loadable; }
        set
        {
            loadable = value;
            changed = true;
        }
    }

    //和最初加载的数据相比，是否有改变，将决定是否存档变化
    public bool changed = false;

    public MapPiece(MapType type, int index, string fileName, Vector2 position, float rotation, Vector2 scale, bool visible, 
        List<int> children, float duration, List<Key.KeyTrigger> triggers, int transferTrigger, ShiftCrystal.ShiftCrystalTrigger shiftTrigger, 
        bool triggerable, bool load)
    {
        this.type = type;
        this.index = index;
        this.fileName = fileName;
        this.originalPosition = position;
        this.originalRotation = rotation;
        this.originalScale = scale;
        this.originalVisible = visible;
        this.children = children;
        this.duration = duration;
        this.triggers = triggers;
        this.transferCrystalTrigger = transferTrigger;
        this.shiftCrystalTrigger = shiftTrigger;

        currentPosition = originalPosition;
        targetPositionOffset = new Vector2(0.0f, 0.0f);
        moveCountDown = 0.0f;
        currentRotation = originalRotation;
        targetRotationOffset = 0.0f;
        rotationCountDown = 0.0f;
        currentScale = originalScale;
        targetScaleOffset = new Vector2(0.0f, 0.0f);
        scaleCountDown = 0.0f;
        this.visible = visible;
        this.triggerable = triggerable;
        triggerableCountDown = 0.0f;
        loadable = load;

    }

    public void LoadArchive(Vector2 currentPosition, Vector2 targetPositionOffset, float moveCountDown, float currentRotation, float targetRotationOffset,
        float rotationCountDown, Vector2 currentScale, Vector2 targetScaleOffset, float scaleCountDown, bool visible, bool triggerable,float triggerableCountDown, bool loadable)
    {
        this.currentPosition = currentPosition;
        this.targetPositionOffset = targetPositionOffset;
        this.moveCountDown = moveCountDown;
        this.currentRotation = currentRotation;
        this.targetRotationOffset = targetRotationOffset;
        this.rotationCountDown = rotationCountDown;
        this.currentScale = currentScale;
        this.targetScaleOffset = targetScaleOffset;
        this.scaleCountDown = scaleCountDown;
        this.visible = visible;
        this.triggerable = triggerable;
        this.triggerableCountDown = triggerableCountDown;
        this.loadable = loadable;

        changed = true;
    }

    public JSONClass GenerateArchivePiece()
    {
        JSONClass node = new JSONClass
        {
            { "Index", new JSONData(index) },
            { "CurrentPosition", new JSONArray
                {
                    new JSONData(currentPosition.x),
                    new JSONData(currentPosition.y)
                }
            },
            { "TargetPositionOffset", new JSONArray()
                {
                    new JSONData(targetPositionOffset.x),
                    new JSONData(targetPositionOffset.y)
                }
            },
            { "MoveCountDown", new JSONData(moveCountDown) },
            { "CurrentRotation", new JSONData(currentRotation) },
            { "TartgetRotationOffset", new JSONData(targetRotationOffset) },
            { "RotationCountDown", new JSONData(rotationCountDown) },
            { "CurrentScale", new JSONArray()
                {
                    new JSONData(currentScale.x),
                    new JSONData(currentScale.y)
                }
            },
            { "TargetScaleOffset", new JSONArray()
                {
                    new JSONData(targetScaleOffset.x),
                    new JSONData(targetScaleOffset.y)
                }
            },
            { "ScaleCountDown", new JSONData(scaleCountDown) },
            { "Visible", new JSONData(Visible) },
            { "Triggerable", new JSONData(Triggerable) },
            { "TriggerableCountDown", new JSONData(triggerableCountDown) },
            { "Loadable", new JSONData(Loadable) }
        };
        return node;
    }

    //每帧刷新的内容，主要是移动旋转缩放部分
    public void RefreshPerFrame()
    {
        if (moveCountDown > 0.0f) {
            if (moveCountDown <= Time.deltaTime)
            {
                currentPosition += targetPositionOffset;
                targetPositionOffset = Vector2.zero;
                moveCountDown = 0.0f;
            }
            else
            {
                Vector2 offset = targetPositionOffset * Time.deltaTime / moveCountDown;
                currentPosition += offset;
                targetPositionOffset -= offset;
                moveCountDown -= Time.deltaTime;
            }
            currentPosition = new Vector2((currentPosition.x + 8640.0f) % 8640.0f, currentPosition.y);
        }
        if (rotationCountDown > 0.0f)
        {
            if (rotationCountDown < Time.deltaTime)
            {
                currentRotation += targetRotationOffset;
                targetRotationOffset = 0.0f;
                rotationCountDown = 0.0f;
            }
            else
            {
                float offset = targetRotationOffset * Time.deltaTime / rotationCountDown;
                currentRotation += offset;
                targetRotationOffset -= offset;
                rotationCountDown -= Time.deltaTime;
            }
        }
        if (scaleCountDown > 0.0f)
        {
            if (scaleCountDown < Time.deltaTime)
            {
                currentScale += targetScaleOffset;
                targetScaleOffset = Vector2.zero;
                scaleCountDown = 0.0f;
            }
            else
            {
                Vector2 offset = targetScaleOffset * Time.deltaTime / scaleCountDown;
                currentScale += offset;
                targetScaleOffset -= offset;
                scaleCountDown -= Time.deltaTime;
            }
        }
        if (triggerableCountDown > 0.0f)
        {
            if (triggerableCountDown < Time.deltaTime)
            {
                SwitchTriggerable();
                triggerableCountDown = 0.0f;
            }
            else
            {
                triggerableCountDown -= Time.deltaTime;
            }
        }
    }

    public void SetNewMoveOffset(Vector2 offset, float duration)
    {
        targetPositionOffset += offset;
        moveCountDown = duration;
        changed = true;
        foreach (int childIndex in children)
        {
            MapPiece piece = MapManager.instance.Get(childIndex);
            piece.SetNewMoveOffset(offset, duration);
        }
    }

    public void SetNewRotationOffset(float offset, float duration)
    {
        targetRotationOffset += offset;
        rotationCountDown = duration;
        changed = true;
    }

    public void SetNewScaleRatio(Vector2 offset, float duration)
    {
        targetScaleOffset += offset;
        scaleCountDown = duration;
        changed = true;
    }

    public void TriggerBreak(float time)
    {
        SwitchTriggerable();
        triggerableCountDown = time;
    }

    public void SwitchTriggerable()
    {
        if (Triggerable)
        {
            Triggerable = false;
        }
        else
        {
            Triggerable = true;
        }
    }

}
