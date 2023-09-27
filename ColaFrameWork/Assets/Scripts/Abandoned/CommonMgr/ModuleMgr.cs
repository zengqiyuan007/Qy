//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventType = ColaFramework.EventType;

namespace ColaAbandoned
{
    /// <summary>
    /// Module系统管理控制类
    /// </summary>
    public class ModuleMgr : IEventHandler
    {
        /// <summary>
        /// 存储系统的字典
        /// </summary>
        private Dictionary<int, ModuleBase> modulesList;

        private Modules modules;

        public ModuleMgr()
        {
            modules = new Modules();
            modulesList = new Dictionary<int, ModuleBase>();

            RegisterAllModules();
            InitAllModules();
            GameEventMgr.GetInstance().RegisterHandler(this, EventType.ServerMsg, EventType.SystemMsg);
        }

        /// <summary>
        /// 获取对应类型的系统
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModule<T>() where T : ModuleBase
        {
            foreach (var subSys in modulesList)
            {
                if (typeof(T) == subSys.Value.GetType())
                {
                    return (T)subSys.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据系统类型，返回对应的系统
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public ModuleBase GetModule(ModuleType moduleType)
        {
            if (modulesList.ContainsKey((int)moduleType))
            {
                return modulesList[(int)moduleType];
            }
            return null;
        }

        public bool HandleMessage(GameEvent evt)
        {
            throw new System.NotImplementedException();
        }

        public bool IsHasHandler(GameEvent evt)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="module"></param>
        public void RegisterModule(ModuleBase module)
        {
            if (null != modulesList)
            {
                modulesList.Add((int)module.ModuleType, module);
            }
        }

        /// <summary>
        /// 初始化对应类型的模块
        /// </summary>
        /// <param name="moduleType"></param>
        public void InitModule(ModuleType moduleType)
        {
            ModuleBase module = GetModule(moduleType);
            if (null != module && module.IsInit == false)
            {
                module.Init();
            }
        }

        /// <summary>
        /// 初始化所有的模块
        /// </summary>
        public void InitAllModules()
        {
            if (null != modulesList)
            {
                using (var enumator = modulesList.GetEnumerator())
                {
                    while (enumator.MoveNext())
                    {
                        if (null != enumator.Current.Value && !enumator.Current.Value.IsInit)
                        {
                            enumator.Current.Value.Init();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 注册所有的模块
        /// </summary>
        public void RegisterAllModules()
        {
            if (modules.AllModules != null)
            {
                for (int i = 0; i < modules.AllModules.Count; i++)
                {
                    if (null != modules.AllModules[i])
                    {
                        RegisterModule(modules.AllModules[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 重置所有的模块
        /// </summary>
        public void ResetAllModules()
        {
            if (null != modulesList)
            {
                using (var enumator = modulesList.GetEnumerator())
                {
                    while (enumator.MoveNext())
                    {
                        if (null != enumator.Current.Value)
                        {
                            enumator.Current.Value.Reset();
                        }
                    }
                }
            }
        }
    }
}