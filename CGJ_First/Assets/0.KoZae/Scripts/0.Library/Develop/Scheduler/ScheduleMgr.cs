using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace KZLib.Develop
{
    #region Scheduler
    public static class Scheduler
    {
        private static List<string> names = ScheduleMgr.In.names;
        private static DictValue<ScheduleData> schedules = ScheduleMgr.In.schedules;

        #region Exist    
        public static bool IsExistSchedule(ScheduleData _schedule)
        {
            return IsExistSchedule(_schedule.tag);
        }

        public static bool IsExistSchedule(string _tag)
        {
            return ScheduleMgr.In.schedules.ContainsKey(_tag);
        }
        #endregion

        #region Add
        public static bool AddSchedule(ScheduleData _data)
        {
            if(IsExistSchedule(_data))
            {
                return false;
            }
            else
            {
                names.Add(_data.tag);
                schedules.Add(_data.tag,_data);

                return true;
            }
        }
        #endregion

        #region Remove
        public static bool RemoveSchedule(ScheduleData _data)
        {
            return RemoveSchedule(_data.tag);
        }

        public static bool RemoveSchedule(string _tag)
        {
            if(IsExistSchedule(_tag))
            {
                names.Remove(_tag);
                return schedules.RemoveSafe(_tag);
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
    #endregion

    #region ScheduleMgr
    internal class ScheduleMgr : SingletonMono<ScheduleMgr>
    {
        // 외부에서 체크 하는 용도
        public List<string> names = new List<string>();
        public DictValue<ScheduleData> schedules = new DictValue<ScheduleData>();

        void Start()
        {
            gameObject.name = "Scheduler";
        }

        void Update()
        {
            if(schedules.Count != 0)
            {
                // 처음 프레임 생략
                if(Time.unscaledTime.Equals(Time.unscaledDeltaTime))
                {
                    return;
                }

                for(int i=0;i<schedules.Count;i++)
                {
                    var pair = schedules.GetPairByIdx(i);
                    var schedule = pair.Value;

                    schedule.onUpdate?.Invoke(schedule.nowTime/schedule.finishTime);

                    schedule.nowTime += Time.unscaledDeltaTime;

                    if( (schedule.stepTime > 0.0f) && (schedule.nowTime >= schedule.stepTime*schedule.stepCount) )
                    {
                        schedule.onStepEnd?.Invoke();
                        schedule.stepCount++;
                    }

                    if(schedule.nowTime >= schedule.finishTime)
                    {
                        schedule.onUpdate?.Invoke(1.0f);
                        schedule.onComplete?.Invoke();

                        schedule.nowTime = 0.0f;
                        schedule.stepCount = 0;

                        if(!schedule.isLoop)
                        {
                            schedules.RemoveSafe(pair.Key);
                            names.Remove(pair.Key);
                        }
                    }                    
                }
            }
        }
    }
    #endregion

    #region ScheduleData
    /// <summary>
    ///        |--------|--------|--------|--------|--------|
    /// <para>now     step      step     step     step    finish(+step)</para>
    /// <para>Dont Save Funcs To Json</para>
    /// </summary>
    public class ScheduleData
    {
        public string tag;

        public float nowTime;
        public float finishTime;

        public bool isLoop;

        public float stepTime;
        public int stepCount;

        [JsonIgnore] public System.Action        onComplete;
        [JsonIgnore] public System.Action        onStepEnd;
        [JsonIgnore] public System.Action<float> onUpdate;

        public ScheduleData() { }

        public ScheduleData(string _tag,float _now,float _finish,bool _loop = false,float _step = 0.0f)
        {
            tag = _tag;
            nowTime = _now;
            finishTime = _finish;
            isLoop = _loop;
            stepTime = _step;
        }
    }
    #endregion

    #region ScheduleDataExtensions
    public static class ScheduleDataExtensions
    {
        public static ScheduleData OnComplete(this ScheduleData _data,System.Action _onComplete)
        {
            _data.onComplete += _onComplete;

            return _data;
        }

        public static ScheduleData OnStepEnd(this ScheduleData _data,System.Action _onStepEnd)
        {
            _data.onStepEnd += _onStepEnd;

            return _data;
        }

        public static ScheduleData OnUpdate(this ScheduleData _data,System.Action<float> _onUpdate)
        {
            _data.onUpdate += _onUpdate;

            return _data;
        }
    }
    #endregion
}