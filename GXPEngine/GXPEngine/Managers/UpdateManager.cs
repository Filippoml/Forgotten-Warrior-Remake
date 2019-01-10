namespace GXPEngine.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Defines the <see cref="UpdateManager" />
    /// </summary>
    public class UpdateManager
    {
        /// <summary>
        /// The UpdateDelegate
        /// </summary>
        private delegate void UpdateDelegate();

        /// <summary>
        /// Defines the _updateDelegates
        /// </summary>
        private UpdateDelegate _updateDelegates;

        /// <summary>
        /// Defines the _updateReferences
        /// </summary>
        private Dictionary<GameObject, UpdateDelegate> _updateReferences = new Dictionary<GameObject, UpdateDelegate>();

        //------------------------------------------------------------------------------------------------------------------------
        //														UpdateManager()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateManager"/> class.
        /// </summary>
        public UpdateManager()
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
            if (_updateDelegates != null)
                _updateDelegates();
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
            MethodInfo info = gameObject.GetType().GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (info != null)
            {
                UpdateDelegate onUpdate = (UpdateDelegate)Delegate.CreateDelegate(typeof(UpdateDelegate), gameObject, info, false);
                if (onUpdate != null && !_updateReferences.ContainsKey(gameObject))
                {
                    _updateReferences[gameObject] = onUpdate;
                    _updateDelegates += onUpdate;
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
            MethodInfo info = gameObject.GetType().GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (info != null)
            {
                throw new Exception("'Update' function was not binded for '" + gameObject + "'. Please check its case. (capital U?)");
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Contains()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Contains
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        /// <returns>The <see cref="Boolean"/></returns>
        public Boolean Contains(GameObject gameObject)
        {
            return _updateReferences.ContainsKey(gameObject);
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
            if (_updateReferences.ContainsKey(gameObject))
            {
                UpdateDelegate onUpdate = _updateReferences[gameObject];
                if (onUpdate != null) _updateDelegates -= onUpdate;
                _updateReferences.Remove(gameObject);
            }
        }

        /// <summary>
        /// The GetDiagnostics
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public string GetDiagnostics()
        {
            string output = "";
            output += "Number of update delegates: " + _updateReferences.Count + '\n';
            return output;
        }
    }
}
