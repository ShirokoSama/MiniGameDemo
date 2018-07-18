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
    public bool originalVisible;
    public Vector2 endPosition;
    public float endRotation;
    public float endScale = 1.0f;
    public float duration = 0.0f;
    public List<Key.KeyTrigger> triggers = null;
    public int transferCrystalTrigger = 0;
    public ShiftCrystal.ShiftCrystalTrigger shiftCrystalTrigger = new ShiftCrystal.ShiftCrystalTrigger(0.0f, 0.0f, false);

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
    public float targetScaleRatio;
    //几秒后达到目标缩放
    public float scaleCountDown;

    //当前可见性
    public bool visible;
    //是否可触发
    public bool triggerable;
    //是否可加载
    public bool loadable;

    //和最初加载的数据相比，是否有改变，将决定是否存档变化
    private bool changed = false;

    public MapPiece(MapType type, int index, string fileName, Vector2 position, float rotation, Vector2 scale, bool visible, 
        Vector2 positionEnd, float rotationEnd, float scaleEnd, float duration, List<Key.KeyTrigger> triggers, int transferTrigger, ShiftCrystal.ShiftCrystalTrigger shiftTrigger)
    {
        this.type = type;
        this.index = index;
        this.fileName = fileName;
        this.originalPosition = position;
        this.originalRotation = rotation;
        this.originalScale = scale;
        this.originalVisible = visible;
        this.endPosition = positionEnd;
        this.endRotation = rotationEnd;
        this.endScale = scaleEnd;
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
        targetScaleRatio = 1.0f;
        scaleCountDown = 0.0f;
        this.visible = visible;
        triggerable = true;
        loadable = true;

    }

    public void LoadArchive(Vector2 currentPosition, Vector2 targetPositionOffset, float moveCountDown, float currentRotation, float targetRotationOffset,
        float rotationCountDown, Vector2 currentScale, float targetRatio, float scaleCountDown, bool visible, bool triggerable, bool loadable)
    {
        this.currentPosition = currentPosition;
        this.targetPositionOffset = targetPositionOffset;
        this.moveCountDown = moveCountDown;
        this.currentRotation = currentRotation;
        this.targetRotationOffset = targetRotationOffset;
        this.rotationCountDown = rotationCountDown;
        this.currentScale = currentScale;
        this.targetScaleRatio = targetRatio;
        this.scaleCountDown = scaleCountDown;
        this.visible = visible;
        this.triggerable = triggerable;
        this.loadable = loadable;

        changed = true;
    }

    public JSONClass GenerateArchivePiece()
    {
        JSONClass node = new JSONClass();
        node.Add("Index", new JSONData(index));
        node.Add("CurrentPosition", new JSONArray
        {
            new JSONData(currentPosition.x),
            new JSONData(currentPosition.y)
        });
        node.Add("TargetPositionOffset", new JSONArray()
        {
            new JSONData(targetPositionOffset.x),
            new JSONData(targetPositionOffset.y)
        });
        node.Add("MoveCountDown", new JSONData(moveCountDown));
        node.Add("CurrentRotation", new JSONData(currentRotation));
        node.Add("TartgetRotationOffset", new JSONData(targetRotationOffset));
        node.Add("RotationCountDown", new JSONData(rotationCountDown));
        node.Add("CurrentScale", new JSONArray()
        {
            new JSONData(currentScale.x),
            new JSONData(currentScale.y)
        });
        node.Add("TargetScaleRatio", new JSONData(targetScaleRatio));
        node.Add("ScaleCountDown", new JSONData(scaleCountDown));
        node.Add("Visible", new JSONData(visible));
        node.Add("Triggerable", new JSONData(triggerable));
        node.Add("Loadable", new JSONData(loadable));
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
                currentScale *= targetScaleRatio;
                targetScaleRatio = 1.0f;
                scaleCountDown = 0.0f;
            }
            else
            {
                float ratio = Mathf.Lerp(1.0f, targetScaleRatio, Time.deltaTime / scaleCountDown);
                currentScale *= ratio;
                targetScaleRatio /= ratio;
                scaleCountDown -= Time.deltaTime;
            }
        }
    }

    public void SetNewMoveOffset(Vector2 offset, float duration)
    {
        targetPositionOffset += offset;
        moveCountDown = duration;
        changed = true;
    }

    public void SetNewRotationOffset(float offset, float duration)
    {
        targetRotationOffset += offset;
        rotationCountDown = duration;
        changed = true;
    }

    public void SetNewScaleRatio(float ratio, float duration)
    {
        targetScaleRatio *= ratio;
        scaleCountDown = duration;
        changed = true;
    }

}
