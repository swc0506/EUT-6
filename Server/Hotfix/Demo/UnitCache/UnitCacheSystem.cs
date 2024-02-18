﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class UnitCacheDestroySystem : DestroySystem<UnitCache>
    {
        public override void Destroy(UnitCache self)
        {
            foreach (Entity entity in self.CacheComponentsDictionary.Values)
            {
                entity.Dispose();
            }
            self.CacheComponentsDictionary.Clear();
            self.key = null;
        }
    }


    [FriendClass(typeof(UnitCache))]
    public static class UnitCacheSystem
    {
        public static void AddOrUpdate(this UnitCache self, Entity entity)
        {
            if (entity == null)
            {
                return;
            }

            if (self.CacheComponentsDictionary.TryGetValue(entity.Id, out Entity oldEntity))
            {
                if (entity !=  oldEntity)
                {
                    oldEntity.Dispose();
                }
                self.CacheComponentsDictionary.Remove(entity.Id);
            }

            self.CacheComponentsDictionary.Add(entity.Id, entity);
        }

        public static async ETTask<Entity> Get(this UnitCache self, long unitId)
        {
            Entity entity = null;
            if (!self.CacheComponentsDictionary.TryGetValue(unitId, out entity))
            {
                entity = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<Entity>(unitId, self.key);
                if (entity != null)
                {
                    self.AddOrUpdate(entity);
                }
            }
            return entity;
        }


        public static void Delete(this UnitCache self, long unitId)
        {
            if (self.CacheComponentsDictionary.TryGetValue(unitId, out Entity entity))
            {
                entity.Dispose();
                self.CacheComponentsDictionary.Remove(unitId);
            }
        }
    }
}
