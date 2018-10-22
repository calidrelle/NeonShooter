﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeonShooter {
    static class EntityManager {
        static bool isUpdating;
        static List<Entity> entities = new List<Entity>();
        static List<Entity> addedEntities = new List<Entity>();

        static List<Enemy> enemies = new List<Enemy>();
        static List<Bullet> bullets = new List<Bullet>();

        public static int Count { get { return entities.Count; } }

        public static void Add(Entity entity) {
            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
        }

        private static void AddEntity(Entity entity) {
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            else if (entity is Enemy)
                enemies.Add(entity as Enemy);
        }

        private static bool IsColliding(Entity a, Entity b) {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        // UPDATE & DRAW

        public static void Update() {
            isUpdating = true;

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);

            addedEntities.Clear();

            // remove any expired entities.
            entities = entities.Where(x => !x.IsExpired).ToList();

            bullets = bullets.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();
        }

        public static void Draw(SpriteBatch spriteBatch) {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }
    }
}
