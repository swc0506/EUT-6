﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public interface IUnitCache
    {

    }

    public class UnitCache : Entity, IAwake, IDestroy
    {
        public string key;

        public Dictionary<long, Entity> CacheComponentsDictionary = new Dictionary<long, Entity>();
    }
}
