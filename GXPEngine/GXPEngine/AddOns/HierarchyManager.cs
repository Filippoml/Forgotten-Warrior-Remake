namespace GXPEngine
{
    using System.Collections.Generic;

    /// <summary>
    /// If you are getting strange bugs because you are calling Destroy during the Update loop, 
	/// you can use this class to do this more cleanly: when using 
	/// HierarchyManager.Instance.LateDestroy,
	/// all these hierarchy changes will be made after the update loop is finished.
	/// Similarly, you can use HierarchyManager.Instance.LateCall to postpone a certain method call until 
	/// after the update loop.
    /// </summary>
    internal class HierarchyManager
    {
        /// <summary>
        /// The DelayedMethod
        /// </summary>
        public delegate void DelayedMethod();

        /// <summary>
        /// Gets the Instance
        /// </summary>
        public static HierarchyManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HierarchyManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// Defines the instance
        /// </summary>
        private static HierarchyManager instance;

        /// <summary>
        /// Defines the toDestroy
        /// </summary>
        private List<GameObject> toDestroy;

        /// <summary>
        /// Defines the toCall
        /// </summary>
        private List<DelayedMethod> toCall;

        // Don't construct these yourself - get the one HierarchyManager using HierarchyManager.Instance
        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyManager"/> class.
        /// </summary>
        HierarchyManager()
        {
            Game.main.OnAfterStep += UpdateHierarchy;
            toDestroy = new List<GameObject>();
            toCall = new List<DelayedMethod>();
        }

        /// <summary>
        /// The LateDestroy
        /// </summary>
        /// <param name="obj">The obj<see cref="GameObject"/></param>
        public void LateDestroy(GameObject obj)
        {
            toDestroy.Add(obj);
        }

        /// <summary>
        /// The IsOnDestroyList
        /// </summary>
        /// <param name="obj">The obj<see cref="GameObject"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool IsOnDestroyList(GameObject obj)
        {
            return toDestroy.Contains(obj);
        }

        /// <summary>
        /// The LateCall
        /// </summary>
        /// <param name="meth">The meth<see cref="DelayedMethod"/></param>
        public void LateCall(DelayedMethod meth)
        {
            toCall.Add(meth);
        }

        /// <summary>
        /// The UpdateHierarchy
        /// </summary>
        public void UpdateHierarchy()
        {
            foreach (GameObject obj in toDestroy)
            {
                obj.Destroy();
            }
            toDestroy.Clear();

            foreach (DelayedMethod method in toCall)
            {
                method();
            }
            toCall.Clear();
        }
    }
}
