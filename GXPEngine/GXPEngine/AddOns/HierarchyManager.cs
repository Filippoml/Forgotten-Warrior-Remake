using GXPEngine;
using System.Collections.Generic;

namespace GXPEngine {
	/// <summary>
	/// If you are getting strange bugs because you are calling Destroy during the Update loop, 
	/// you can use this class to do this more cleanly: when using 
	/// HierarchyManager.Instance.LateDestroy,
	/// all these hierarchy changes will be made after the update loop is finished.
	/// Similarly, you can use HierarchyManager.Instance.LateCall to postpone a certain method call until 
	/// after the update loop.
	/// </summary>
	class HierarchyManager {
		public delegate void DelayedMethod();

		public static HierarchyManager Instance {
			get {
				if (instance == null) {
					instance = new HierarchyManager ();
				}				
				return instance;
			}
		}
		private static HierarchyManager instance; 

		private List<GameObject> toDestroy;
		private List<DelayedMethod> toCall;

		// Don't construct these yourself - get the one HierarchyManager using HierarchyManager.Instance
		HierarchyManager() {
			Game.main.OnAfterStep += UpdateHierarchy;
			toDestroy = new List<GameObject> ();
			toCall = new List<DelayedMethod> ();
		}

		public void LateDestroy(GameObject obj) {
			toDestroy.Add (obj);
		}

		public bool IsOnDestroyList(GameObject obj) {
			return toDestroy.Contains (obj);
		}

		public void LateCall(DelayedMethod meth) {
			toCall.Add (meth);
		}

		public void UpdateHierarchy() {
			foreach (GameObject obj in toDestroy) {
				obj.Destroy ();
			}
			toDestroy.Clear ();

			foreach (DelayedMethod method in toCall) {
				method ();
			}
			toCall.Clear ();
		}
	}
}
