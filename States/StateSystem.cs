using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastLoopExample
{
    public class StateSystem : IGameObject
    {
        //Dictionary键与值的结合
        Dictionary<string, IGameObject> _stateStore = new Dictionary<string, IGameObject>();
        IGameObject _currentState = null;                        //当前状态

        
        public void Update(double elapsedTime)
        {
            if (_currentState == null)
                return;
            _currentState.Update(elapsedTime);

        }

        public void Render()
        {
            if (_currentState == null)
                return;
            _currentState.Render();
        }

        public void AddState(string stateId, IGameObject state)
        {
            System.Diagnostics.Debug.Assert(Exists(stateId) == false);
            _stateStore.Add(stateId, state);
        }

        public void ChangeState(string stateId)
        {
            System.Diagnostics.Debug.Assert(Exists(stateId));
            _stateStore[stateId].Start();            //切换场景时执行一次start
            _currentState = _stateStore[stateId];
            Input.InitKeys();                        //每切换一次场景，键位复位一次
        }

        public bool Exists(string stateId)
        {
            return _stateStore.ContainsKey(stateId);
        }


        public void Start()
        {
            
        }
    }
}
