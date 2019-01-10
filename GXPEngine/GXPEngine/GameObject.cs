namespace GXPEngine
{
    using System.Collections.Generic;
    using GXPEngine.Core;

    /// <summary>
    /// GameObject is the base class for all display objects.
    /// </summary>
    public abstract class GameObject : Transformable
    {
        /// <summary>
        /// Defines the name
        /// </summary>
        public string name;

        /// <summary>
        /// Defines the _collider
        /// </summary>
        private Collider _collider;

        /// <summary>
        /// Defines the _children
        /// </summary>
        private List<GameObject> _children = new List<GameObject>();

        /// <summary>
        /// Defines the _parent
        /// </summary>
        private GameObject _parent = null;

        /// <summary>
        /// Defines the visible
        /// </summary>
        public bool visible = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        public GameObject()
        {
            _collider = createCollider();
            if (Game.main != null) Game.main.Add(this);
        }

        /// <summary>
        /// Return the collider to use for this game object, null is allowed
        /// </summary>
        /// <returns>The <see cref="Collider"/></returns>
        protected virtual Collider createCollider()
        {
            return null;
        }

        /// <summary>
        /// Gets the index of this object in the parent's hierarchy list.
		/// Returns -1 if no parent is defined.
        /// </summary>
        public int Index
        {
            get
            {
                if (parent == null) return -1;
                return parent._children.IndexOf(this);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														collider
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the collider
        /// </summary>
        internal Collider collider
        {
            get { return _collider; }
        }

        /// <summary>
        /// Gets the game that this object belongs to. 
		/// This is a unique instance throughout the runtime of the game.
		/// Use this to access the top of the displaylist hierarchy, and to retreive the width and height of the screen.
        /// </summary>
        public Game game
        {
            get
            {
                return Game.main;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														OnDestroy()
        //------------------------------------------------------------------------------------------------------------------------
        //subclasses can use this call to clean up resources once on destruction
        /// <summary>
        /// The OnDestroy
        /// </summary>
        protected virtual void OnDestroy()
        {
        }

        /// <summary>
        /// Destroy this instance, and removes it from the game. To complete garbage collection, you must nullify all 
		/// your own references to this object.
        /// </summary>
        public virtual void Destroy()
        {
            if (!game.Contains(this)) return;
            OnDestroy();

            //detach all children
            while (_children.Count > 0)
            {
                GameObject child = _children[0];
                if (child != null) child.Destroy();
            }
            //detatch from parent
            if (parent != null) parent = null;
            //remove from game
            if (Game.main != null) Game.main.Remove(this);
        }

        /// <summary>
        /// Get all a list of all objects that currently overlap this one.
		/// Calling this method will test collisions between this object and all other colliders in the scene.
		/// It can be called mid-step and is included for convenience, not performance.
        /// </summary>
        /// <returns>The <see cref="GameObject[]"/></returns>
        public GameObject[] GetCollisions()
        {
            return game.GetGameObjectCollisions(this);
        }

        /// <summary>
        /// This function is called by the renderer. You can override it to change this object's rendering behaviour.
		/// When not inside the GXPEngine package, specify the parameter as GXPEngine.Core.GLContext.
		/// This function was made public to accomodate split screen rendering. Use SetViewPort for that.
        /// </summary>
        /// <param name="glContext">The glContext<see cref="GLContext"/></param>
        public virtual void Render(GLContext glContext)
        {
            if (visible)
            {
                glContext.PushMatrix(matrix);

                RenderSelf(glContext);
                foreach (GameObject child in GetChildren())
                {
                    child.Render(glContext);
                }

                glContext.PopMatrix();
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														RenderSelf
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The RenderSelf
        /// </summary>
        /// <param name="glContext">The glContext<see cref="GLContext"/></param>
        protected virtual void RenderSelf(GLContext glContext)
        {
        }

        /// <summary>
        /// Gets or sets the parent GameObject.
		/// When the parent moves, this object moves along.
        /// </summary>
        public GameObject parent
        {
            get { return _parent; }
            set
            {
                if (_parent != null)
                {
                    _parent.removeChild(this);
                    _parent = null;
                }
                _parent = value;
                if (value != null)
                {
                    _parent.addChild(this);
                }
            }
        }

        /// <summary>
        /// Adds the specified GameObject as a child to this one.
        /// </summary>
        /// <param name="child">The child<see cref="GameObject"/></param>
        public void AddChild(GameObject child)
        {
            child.parent = this;
        }

        /// <summary>
        /// Removes the specified child GameObject from this object.
        /// </summary>
        /// <param name="child">The child<see cref="GameObject"/></param>
        public void RemoveChild(GameObject child)
        {
            if (child.parent == this)
            {
                child.parent = null;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														removeChild()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The removeChild
        /// </summary>
        /// <param name="child">The child<see cref="GameObject"/></param>
        private void removeChild(GameObject child)
        {
            _children.Remove(child);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														addChild()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The addChild
        /// </summary>
        /// <param name="child">The child<see cref="GameObject"/></param>
        private void addChild(GameObject child)
        {
            if (child.HasChild(this)) return; //no recursive adding
            _children.Add(child);
            return;
        }

        /// <summary>
        /// Adds the specified GameObject as a child to this object at an specified index. 
		/// This will alter the position of other objects as well.
		/// You can use this to determine the layer order (z-order) of child objects.
        /// </summary>
        /// <param name="child">The child<see cref="GameObject"/></param>
        /// <param name="index">The index<see cref="int"/></param>
        public void AddChildAt(GameObject child, int index)
        {
            if (child.parent != this)
            {
                AddChild(child);
            }
            if (index < 0) index = 0;
            if (index >= _children.Count) index = _children.Count - 1;
            _children.Remove(child);
            _children.Insert(index, child);
        }

        /// <summary>
        /// Returns 'true' if the specified object is a child of this object.
        /// </summary>
        /// <param name="gameObject">The gameObject<see cref="GameObject"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool HasChild(GameObject gameObject)
        {
            GameObject par = gameObject;
            while (par != null)
            {
                if (par == this) return true;
                par = par.parent;
            }
            return false;
        }

        /// <summary>
        /// Returns a list of all children that belong to this object.
		/// The function returns System.Collections.Generic.List<GameObject>.
        /// </summary>
        /// <returns>The <see cref="List{GameObject}"/></returns>
        public List<GameObject> GetChildren()
        {
            return _children;
        }

        /// <summary>
        /// Inserts the specified object in this object's child list at given location.
		/// This will alter the position of other objects as well.
		/// You can use this to determine the layer order (z-order) of child objects.
        /// </summary>
        /// <param name="child">The child<see cref="GameObject"/></param>
        /// <param name="index">The index<see cref="int"/></param>
        public void SetChildIndex(GameObject child, int index)
        {
            if (child.parent != this) AddChild(child);
            if (index < 0) index = 0;
            if (index >= _children.Count) index = _children.Count - 1;
            _children.Remove(child);
            _children.Insert(index, child);
        }

        /// <summary>
        /// Tests if this object overlaps the one specified.
        /// </summary>
        /// <param name="other">The other<see cref="GameObject"/></param>
        /// <returns>The <see cref="bool"/></returns>
        virtual public bool HitTest(GameObject other)
        {
            return _collider != null && other._collider != null && _collider.HitTest(other._collider);
        }

        /// <summary>
        /// Returns 'true' if a 2D point overlaps this object, false otherwise
		/// You could use this for instance to check if the mouse (Input.mouseX, Input.mouseY) is over the object.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <returns>The <see cref="bool"/></returns>
        virtual public bool HitTestPoint(float x, float y)
        {
            return _collider != null && _collider.HitTestPoint(x, y);
        }

        /// <summary>
        /// Transforms the point from local to global space.
		/// If you insert a point relative to the object, it will return that same point relative to the game.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public override Vector2 TransformPoint(float x, float y)
        {
            Vector2 ret = base.TransformPoint(x, y);
            if (parent == null)
            {
                return ret;
            }
            else
            {
                return parent.TransformPoint(ret.x, ret.y);
            }
        }

        /// <summary>
        /// Transforms the point from global into local space.
		/// If you insert a point relative to the stage, it will return that same point relative to this GameObject.
        /// </summary>
        /// <param name="x">The x<see cref="float"/></param>
        /// <param name="y">The y<see cref="float"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public override Vector2 InverseTransformPoint(float x, float y)
        {
            Vector2 ret = base.InverseTransformPoint(x, y);
            if (parent == null)
            {
                return ret;
            }
            else
            {
                return parent.InverseTransformPoint(ret.x, ret.y);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														ToString()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return "[" + this.GetType().Name + "::" + name + "]";
        }
    }
}
