using System;
using System.Runtime.CompilerServices;
using Colossal.Entities;
using Colossal.Logging;
using Game;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.UI.InGame;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Cheats
{
    public class CheatSystem
    {

        private ILog log = Mod.self.log;

        private readonly World world = World.DefaultGameObjectInjectionWorld;



        public void NextMilestone()
        {

            XPSystem xpSystem = world.GetExistingSystemManaged<XPSystem>();
            MilestoneSystem milestoneSystem = world.GetExistingSystemManaged<MilestoneSystem>();

            JobHandle handle = new JobHandle();

            NativeQueue<XPGain> queue = xpSystem.GetQueue(out handle);

            var requiredXP = milestoneSystem.requiredXP;

            if (requiredXP <= 0)
            {
                log.Debug("Skipping unlock due to required xp being <= 0");
                return;
            }

            queue.Enqueue(new XPGain
            {
                amount = requiredXP,
                entity = Entity.Null,
                reason = XPReason.Unknown
            });

            log.Debug($"Increased XP value by {requiredXP}");
        }

        public void UnlockAllMilestones()
        {

            XPSystem xpSystem = world.GetExistingSystemManaged<XPSystem>();

            JobHandle handle = new JobHandle();
            NativeQueue<XPGain> queue = xpSystem.GetQueue(out handle);

            queue.Enqueue(new XPGain
            {
                amount = 2000000000,
                entity = Entity.Null,
                reason = XPReason.Unknown
            });

            log.Debug("Unlocked all milestones");
        }


        public void GiveMoney(int amount, bool min)
        {

            CitySystem citySystem = world.GetExistingSystemManaged<CitySystem>();

            PlayerMoney playerMoney = world.EntityManager.GetComponentData<PlayerMoney>(citySystem.City);

            playerMoney.Add(amount);

            if (min == true)
            {
                if (playerMoney.money < 0)
                {
                    playerMoney.Add(Math.Abs(playerMoney.money));
                }
            }

            world.EntityManager.SetComponentData<PlayerMoney>(citySystem.City, playerMoney);
            citySystem.Update();

            log.Debug($"Added {amount} money");
        }

        public void SetMoney(int amount)
        {

            CitySystem citySystem = world.GetExistingSystemManaged<CitySystem>();

            PlayerMoney playerMoney = world.EntityManager.GetComponentData<PlayerMoney>(citySystem.City);

            playerMoney.Add(amount - playerMoney.money);

            world.EntityManager.SetComponentData<PlayerMoney>(citySystem.City, playerMoney);
            citySystem.Update();

            log.Debug($"Added {amount} money");
        }

        public void GiveDevPoints(int amount, bool min)
        {

            DevTreeSystem devTreeSystem = world.GetExistingSystemManaged<DevTreeSystem>();

            devTreeSystem.points += amount;

            if (min == true)
            {
                if (devTreeSystem.points < 0)
                {
                    devTreeSystem.points = 0;
                }
            }

            log.Debug($"Added {amount} dev points");

        }

        public void SetDevPoints(int amount)
        {

            DevTreeSystem devTreeSystem = world.GetExistingSystemManaged<DevTreeSystem>();

            devTreeSystem.points = amount;

            log.Debug($"Set dev points to {amount}");

        }

        public void UnlockPrefabID(PrefabID prefabID)
        {

            PrefabSystem prefabSystem = world.GetExistingSystemManaged<PrefabSystem>();

            if (prefabSystem.TryGetPrefab(prefabID, out PrefabBase prefabBase) && prefabBase is not null && prefabSystem.TryGetEntity(prefabBase, out Entity prefabEntity))
            {
                Unlock(prefabEntity);
                log.Debug($"Unlocked entity {prefabID.GetName()}");
                return;
            }
            log.Error("PrefabID is invalid");
        }

        public void UnlockMilestonePrefabs(int milestone)
        {

            PrefabSystem prefabSystem = world.GetExistingSystemManaged<PrefabSystem>();

            EntityQuery milestoneQuery = world.EntityManager.CreateEntityQuery(typeof(MilestoneData));

            log.Info($"Trying to unlock milestone {milestone} entities");
            log.Info($"Got {milestoneQuery.CalculateEntityCount()} entities from all milestones");

            foreach (Entity entity in milestoneQuery.ToEntityArray(Allocator.Temp))
            {
                log.Info(entity);
                MilestoneData milestoneData;
                if (EntitiesExtensions.TryGetComponent<MilestoneData>(world.EntityManager, entity, out milestoneData) && milestoneData.m_Index == milestone)
                {
                    Unlock(entity);
                    log.Info($"Unlocked milestone {milestone} entity {entity}");
                }
            }
        }
        public void Unlock(Entity entity)
        {
            EntityArchetype entityArchetype = world.EntityManager.CreateArchetype(new ComponentType[]
            {
                ComponentType.ReadWrite<Event>(),
                ComponentType.ReadWrite<Unlock>()
            });
            Entity entity2 = world.EntityManager.CreateEntity(entityArchetype);
            world.EntityManager.AddComponentData<Unlock>(entity2, new Unlock(entity));
        }

    }
}