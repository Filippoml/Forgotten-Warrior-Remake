namespace GXPEngine
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    //------------------------------------------------------------------------------------------------------------------------
    //														CollisionManager
    //------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Defines the <see cref="CollisionManager" />
    /// </summary>
    public class CollisionManager
    {
        /// <summary>
        /// The CollisionDelegate
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        private delegate void CollisionDelegate(GameObject gameObject);

        //------------------------------------------------------------------------------------------------------------------------
        //														ColliderInfo
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Defines the <see cref="ColliderInfo" />
        /// </summary>
        private struct ColliderInfo
        {
            /// <summary>
            /// Defines the gameObject
            /// </summary>
            public GameObject gameObject;

            /// <summary>
            /// Defines the onCollision
            /// </summary>
            public CollisionDelegate onCollision;

            //------------------------------------------------------------------------------------------------------------------------
            //														ColliderInfo()
            //------------------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Initializes a new instance of the <see cref=""/> class.
            /// </summary>
            /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
            /// <param name="onCollision">The onCollision<see cref="CollisionDelegate"/></param>
            public ColliderInfo(GameObject gameObject, CollisionDelegate onCollision)
            {
                this.gameObject = gameObject;
                this.onCollision = onCollision;
            }
        }

        /// <summary>
        /// Defines the colliderList
        /// </summary>
        private List<GameObject> colliderList = new List<GameObject>();

        /// <summary>
        /// Defines the activeColliderList
        /// </summary>
        private List<ColliderInfo> activeColliderList = new List<ColliderInfo>();

        /// <summary>
        /// Defines the _collisionReferences
        /// </summary>
        private Dictionary<GameObject, ColliderInfo> _collisionReferences = new Dictionary<GameObject, ColliderInfo>();

        //------------------------------------------------------------------------------------------------------------------------
        //														CollisionManager()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="CollisionManager"/> class.
        /// </summary>
        public CollisionManager()
        {
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Step()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Step
        /// </summary>
        public void Step()
        {
            for (int i = activeColliderList.Count - 1; i >= 0; i--)
            {
                ColliderInfo info = activeColliderList[i];
                for (int j = colliderList.Count - 1; j >= 0; j--)
                {
                    if (j >= colliderList.Count) continue; //fix for removal in loop
                    GameObject other = colliderList[j];
                    if (info.gameObject != other)
                    {
                        if (info.gameObject.HitTest(other))
                        {
                            if (info.onCollision != null)
                            {
                                info.onCollision(other);
                            }
                        }
                    }
                }
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //												 GetCurrentCollisions()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The GetCurrentCollisions
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        /// <returns>The <see cref="GameObject[]"/></returns>
        public GameObject[] GetCurrentCollisions(GameObject gameObject)
        {
            List<GameObject> list = new List<GameObject>();
            for (int j = colliderList.Count - 1; j >= 0; j--)
            {
                if (j >= colliderList.Count) continue; //fix for removal in loop
                GameObject other = colliderList[j];
                if (gameObject != other)
                {
                    if (gameObject.HitTest(other))
                    {
                        list.Add(other);
                    }
                }
            }
            return list.ToArray();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Add()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Add
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        public void Add(GameObject gameObject)
        {
            if (gameObject.collider != null && !colliderList.Contains(gameObject))
            {
                colliderList.Add(gameObject);
            }

            MethodInfo info = gameObject.GetType().GetMethod("OnCollision", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (info != null)
            {

                CollisionDelegate onCollision = (CollisionDelegate)Delegate.CreateDelegate(typeof(CollisionDelegate), gameObject, info, false);
                if (onCollision != null && !_collisionReferences.ContainsKey(gameObject))
                {
                    ColliderInfo colliderInfo = new ColliderInfo(gameObject, onCollision);
                    _collisionReferences[gameObject] = colliderInfo;
                    activeColliderList.Add(colliderInfo);
                }

            }
            else
            {
                validateCase(gameObject);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														validateCase()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The validateCase
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        private void validateCase(GameObject gameObject)
        {
            MethodInfo info = gameObject.GetType().GetMethod("OnCollision", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (info != null)
            {
                throw new Exception("'OnCollision' function was not binded. Please check its case (capital O?)");
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Remove()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Remove
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        public void Remove(GameObject gameObject)
        {
            colliderList.Remove(gameObject);
            if (_collisionReferences.ContainsKey(gameObject))
            {
                ColliderInfo colliderInfo = _collisionReferences[gameObject];
                activeColliderList.Remove(colliderInfo);
                _collisionReferences.Remove(gameObject);
            }
        }

        /// <summary>
        /// The GetDiagnostics
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public string GetDiagnostics()
        {
            string output = "";
            output += "Number of colliders: " + colliderList.Count + '\n';
            output += "Number of active colliders: " + activeColliderList.Count + '\n';
            return output;
        }
    }
}
