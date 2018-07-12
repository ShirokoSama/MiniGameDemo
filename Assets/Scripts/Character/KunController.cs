using UnityEngine;

namespace HaruScene
{
    public class KunController : MonoBehaviour
    {

        //玩家输入状态控制
        private bool isTouch = false;
        private Vector2 touchPosition;

        enum KunMoveState
        {
            Float,            //上浮
            Swim,             //水平游动
            Idle,             //待机
            Tired             //因疲劳而待机，状态上也是待机
        }

        public GameObject character;
        //public float accelaration = 1;
        //public float resistance = 0.2f;
        //private Vector3 speed;

        
        //手指与最初触控屏幕的位置的偏移
        private Vector2 touchOffset;
        //鲲浮起（向上游）的速度
        public float floatingSpeed = 1.0f;
        //鲲水平游动速度
        public float swimSpeed = 2.0f;
        //鲲下沉速度
        public float sinkSpeed = 1.0f;

        //采用相对鲲点击位置确认移动方式时，判断如果为点击在鲲身上则上浮，其中的判定范围
        public float floatJudge = 1.0f;
        public EnergyUIManager energyUIManager;

        //鲲的最大精力
        public float maxEnergy = 100.0f;
        //鲲的当前精力
        private float energy;
        //鲲精力的消耗速度
        public float energyFadeSpeed = 20.0f;
        //鲲精力的回复速度
        public float energyRecoverSpeed = 15.0f;

        //当前鲲的状态
        private KunMoveState moveState = KunMoveState.Idle;

        // Use this for initialization
        private void Start()
        {

            character.transform.position.Set(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                0
                );
            //speed = new Vector3(0f, 0f, 0f);

            energy = maxEnergy;
            energyUIManager.maxEnergy = maxEnergy;
            energyUIManager.SetUIEnergy(energy);
        }

        // Update is called once per frame
        private void Update()
        {
            /*
             * 仅适用于触控类似于摇杆
            if (Input.touchCount <= 0)
            {
                moveState = KunMoveState.Idle;
                transform.Translate(new Vector3(0.0f, -sinkSpeed * Time.deltaTime, 0.0f));
                return;
            }
            if (Input.touchCount > 0)
            {
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        Debug.Log("TouchPhase.Begin");
                        touchStartPos = Input.GetTouch(0).position;
                        break;

                    case TouchPhase.Stationary:
                        Debug.Log("TouchPhase.Stationary");
                        transform.Translate(new Vector3(0.0f, floatingSpeed * Time.deltaTime, 0.0f));
                        moveState = KunMoveState.Float;
                        break;

                    case TouchPhase.Moved:
                        touchOffset = Input.GetTouch(0).position - touchStartPos;
                        touchOffset.y = Mathf.Min(touchOffset.x, touchOffset.y);
                        transform.Translate(touchOffset.normalized * swimSpeed * Time.deltaTime);
                        moveState = KunMoveState.Swim;
                        break;
                        
                }
            }
            */


            //当点击结束，将鲲的运动状态恢复为待命
            if (!isTouch) moveState = KunMoveState.Idle;

            //当鲲得到移动指令且仍有精力时
            if (isTouch && moveState != KunMoveState.Tired)
            {
                Vector3 mousePostion = Camera.main.ScreenToWorldPoint(touchPosition);
                Vector3 dir = mousePostion - character.transform.position;
                touchOffset = (Vector2)dir;
                if (touchOffset.magnitude <= floatJudge)
                {
                    moveState = KunMoveState.Float;
                    transform.Translate(0.0f, floatingSpeed * Time.deltaTime, 0.0f);
                    
                }
                else
                {
                    moveState = KunMoveState.Swim;
                    touchOffset.y = Mathf.Clamp(touchOffset.y, -Mathf.Abs(touchOffset.x), Mathf.Abs(touchOffset.x));
                    transform.Translate(touchOffset.normalized * swimSpeed * Time.deltaTime);
                }
                energy -= energyFadeSpeed * Time.deltaTime;
            }
            else
            {
                transform.Translate(new Vector3(0.0f, -sinkSpeed * Time.deltaTime, 0.0f));
                energy += energyRecoverSpeed * Time.deltaTime;
            }

            if (energy <= 0.0f)
            {
                moveState = KunMoveState.Tired;
                energy = 0.0f;
            }
            energy = Mathf.Min(energy, maxEnergy);

            energyUIManager.SetUIEnergy(energy);
        }

        private void PlayerTouchDown(Vector2 position)
        {
            isTouch = true;
            touchPosition = position;
        }

        private void PlayerTouchUp()
        {
            isTouch = false;
        }
    }
}
