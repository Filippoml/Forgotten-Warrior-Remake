using System;
using System.Collections.Generic;
using GXPEngine.Core;

namespace GXPEngine
{
	/// <summary>
	/// GameObject is the base class for all display objects. 
	/// </summary>
	public abstract class GameObject : Transformable
	{
		public string name;
		private Collider _collider;
		
		private List<GameObject> _children = new List<GameObject>();
		private GameObject _parent = null;
		
		public bool visible = true;

		//------------------------------------------------------------------------------------------------------------------------
		//														GameObject()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="GXPEngine.GameObject"/> class.
		/// Since GameObjects contain a display hierarchy, a GameObject can be used as a container for other objects.
		/// Other objects can be added using child commands as AddChild.
		/// </summary>
		public GameObject()
		{
			_collider = createCollider();
			if (Game.main != null) Game.main.Add(this);
		}

		/// <summary>
		/// Return the collider to use for this game object, null is allowed 
		/// </summary>
		protected virtual Collider createCollider () {
			return null;
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														Index
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the index of this object in the parent's hierarchy list.
		/// Returns -1 if no parent is defined.
		/// </summary>
		/// <value>
		/// The index.
		/// </value>
		public int Index {
			get { 
				if (parent == null) return -1;
				return parent._children.IndexOf(this);
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														collider
		//------------------------------------------------------------------------------------------------------------------------
		internal Collider collider {
			get { return _collider; }
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														game
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the game that this object belongs to. 
		/// This is a unique instance throughout the runtime of the game.
		/// Use this to access the top of the displaylist hierarchy, and to retreive the width and height of the screen.
		/// </summary>
		public Game game {
			get {
				return Game.main;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														OnDestroy()
		//------------------------------------------------------------------------------------------------------------------------
		//subclasses can use this call to clean up resources once on destruction
		protected virtual void OnDestroy ()
		{
			//empty
		}
				
		//------------------------------------------------------------------------------------------------------------------------
		//														Destroy()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Destroy this instance, and removes it from the game. To complete garbage collection, you must nullify all 
		/// your own references to this object.
		/// </summary>
		public virtual void Destroy ()
		{
			if (!game.Contains (this)) return;
			OnDestroy();

			//detach all children
			while (_children.Count > 0) {
				GameObject child = _children[0];
				if (child != null) child.Destroy();
			}
			//detatch from parent
			if (parent != null) parent = null;
			//remove from game
			if (Game.main != null) Game.main.Remove (this);
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														Render
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get all a list of all objects that currently overlap this one.
		/// Calling this method will test collisions between this object and all other colliders in the scene.
		/// It can be called mid-step and is included for convenience, not performance.
		/// </summary>
		public GameObject[] GetCollisions ()
		{
			return game.GetGameObjectCollisions(this);
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Render
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// This function is called by the renderer. You can override it to change this object's rendering behaviour.
		/// When not inside the GXPEngine package, specify the parameter as GXPEngine.Core.GLContext.
		/// This function was made public to accomodate split screen rendering. Use SetViewPort for that.
 		/// </summary>
		/// <param name='glContext'>
		/// Gl context, will be supplied by internal caller.
		/// </param>
		public virtual void Render(GLContext glContext) {
			if (visible) {
				glContext.PushMatrix(matrix);
				
				RenderSelf (glContext);
				foreach (GameObject child in GetChildren()) {
					child.Render(glContext);
				}
				
				glContext.PopMatrix();
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														RenderSelf
		//------------------------------------------------------------------------------------------------------------------------
		protected virtual void RenderSelf(GLContext glContext) {
			//if (visible == false) return;
			//glContext.PushMatrix(matrix);
			//glContext.PopMatrix();
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														parent
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the parent GameObject.
		/// When the parent moves, this object moves along.
		/// </summary>
		public GameObject parent {
			get { return _parent; }
			set { 
				if (_parent != null) {
					_parent.removeChild(this);
					_parent = null;
				}
				_parent = value;
				if (value != null) {
					_parent.addChild(this);
				}
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														AddChild()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified GameObject as a child to this one.
		/// </summary>
		/// <param name='child'>
		/// Child object to add.
		/// </param>
		public void AddChild(GameObject child) {
			child.parent = this;	
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														RemoveChild()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the specified child GameObject from this object.
		/// </summary>
		/// <param name='child'>
		/// Child object to remove.
		/// </param>
		public void RemoveChild (GameObject child)
		{
			if (child.parent == this) {
				child.parent = null;
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														removeChild()
		//------------------------------------------------------------------------------------------------------------------------
		private void removeChild(GameObject child) {
			_children.Remove(child);

		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														addChild()
		//------------------------------------------------------------------------------------------------------------------------
		private void addChild(GameObject child) {
			if (child.HasChild(this)) return; //no recursive adding
			_children.Add(child);
			return;
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														AddChildAt()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified GameObject as a child to this object at an specified index. 
		/// This will alter the position of other objects as well.
		/// You can use this to determine the layer order (z-order) of child objects.
		/// </summary>
		/// <param name='child'>
		/// Child object to add.
		/// </param>
		/// <param name='index'>
		/// Index in the child list where the object should be added.
		/// </param>
		public void AddChildAt(GameObject child, int index) {
			if (child.parent != this) {
				AddChild(child);
			}
			if (index < 0) index = 0;
			if (index >= _children.Count) index = _children.Count - 1;
			_children.Remove(child);
			_children.Insert(index, child);			
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														HasChild()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns 'true' if the specified object is a child of this object.
		/// </summary>
		/// <param name='gameObject'>
		/// The GameObject that should be tested.
		/// </param>
		public bool HasChild(GameObject gameObject) {
			GameObject par = gameObject;
			while (par != null) {
				if (par == this) return true;
				par = par.parent;
			}
			return false;
		}
				
		//------------------------------------------------------------------------------------------------------------------------
		//														GetChildren()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a list of all children that belong to this object.
		/// The function returns System.Collections.Generic.List<GameObject>.
		/// </summary>
		public List<GameObject> GetChildren() {
			return _children;
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														SetChildIndex()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts the specified object in this object's child list at given location.
		/// This will alter the position of other objects as well.
		/// You can use this to determine the layer order (z-order) of child objects.
		/// </summary>
		/// <param name='child'>
		/// Child.
		/// </param>
		/// <param name='index'>
		/// Index.
		/// </param>
		public void SetChildIndex(GameObject child, int index) {
			if (child.parent != this) AddChild(child);
			if (index < 0) index = 0;
			if (index >= _children.Count) index = _children.Count - 1;
			_children.Remove(child);
			_children.Insert(index, child);
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														HitTest()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Tests if this object overlaps the one specified. 
		/// </summary>
		/// <returns>
		/// <c>true</c>, if test was hit, <c>false</c> otherwise.
		/// </returns>
		/// <param name='other'>
		/// Other.
		/// </param>
		virtual public bool HitTest(GameObject other) {
			return _collider != null && other._collider != null && _collider.HitTest (other._collider);
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														HitTestPoint()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns 'true' if a 2D point overlaps this object, false otherwise
		/// You could use this for instance to check if the mouse (Input.mouseX, Input.mouseY) is over the object.
		/// </summary>
		/// <param name='x'>
		/// The x coordinate to test.
		/// </param>
		/// <param name='y'>
		/// The y coordinate to test.
		/// </param>
		virtual public bool HitTestPoint(float x, float y) {
			return _collider != null && _collider.HitTestPoint(x, y);
		}		
		
		//------------------------------------------------------------------------------------------------------------------------
		//														TransformPoint()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Transforms the point from local to global space.
		/// If you insert a point relative to the object, it will return that same point relative to the game.
		/// </summary>
		/// <param name='x'>
		/// The x coordinate to transform.
		/// </param>
		/// <param name='y'>
		/// The y coordinate to transform.
		/// </param>
		public override Vector2 TransformPoint(float x, float y) {
			Vector2 ret = base.TransformPoint (x, y);
			if (parent == null) {
				return ret;
			} else {
				return parent.TransformPoint (ret.x, ret.y);
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//												InverseTransformPoint()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Transforms the point from global into local space.
		/// If you insert a point relative to the stage, it will return that same point relative to this GameObject.
		/// </summary>
		/// <param name='x'>
		/// The x coordinate to transform.
		/// </param>
		/// <param name='y'>
		/// The y coordinate to transform.
		/// </param>
		public override Vector2 InverseTransformPoint(float x, float y) {
			Vector2 ret = base.InverseTransformPoint (x, y);
			if (parent == null) {
				return ret;
			} else {
				return parent.InverseTransformPoint (ret.x, ret.y);
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														ToString()
		//------------------------------------------------------------------------------------------------------------------------
		public override string ToString() {
			return "[" + this.GetType().Name + "::" + name + "]";
		}
				
	}
}

