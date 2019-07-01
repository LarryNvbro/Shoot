using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Reflection;


public abstract class StateMachineBase : MonoBehaviour
{
	[HideInInspector]
	public Transform localTransform;

	[HideInInspector]
	public new Transform transform;

	[HideInInspector]
	public new GameObject gameObject;

	[HideInInspector]
	public StateMachineBase stateMachine;

	private float timeEnteredState;

	public float TimeInCurrentState
	{
		get
		{
			return Time.time - timeEnteredState;
		}
	}

	void Awake()
	{
		state.executingStateMachine = stateMachine = this;
		localTransform = base.transform;
		
		Represent(base.gameObject);

		OnAwake();
	}

	void Represent(GameObject gameObjectToRepresent)
	{
		gameObject = gameObjectToRepresent;
		transform = gameObject.transform;

		OnRepresent();
	}

	public void Refresh() { Represent(gameObject); }

	protected virtual void OnAwake() { }

	protected virtual void OnRepresent() { }

	protected virtual void OnChangeState() { }

	static IEnumerator DoNothingCoroutine() { yield break; }

	static void DoNothing() { }

	static void DoNothingCollider(Collider other) { }
	
	static void DoNothingCollision(Collision other) { }

	public class State
	{
		public Action DoUpdate = DoNothing;
		public Action DoLateUpdate = DoNothing;
		public Action DoFixedUpdate = DoNothing;

		public System.Func<IEnumerator> DoUpdateCoroutine = DoNothingCoroutine;
		public System.Func<IEnumerator> DoLateUpdateCoroutine = DoNothingCoroutine;
		public System.Func<IEnumerator> DoFixedUpdateCoroutine = DoNothingCoroutine;

		public Action<Collider> DoOnTriggerEnter = DoNothingCollider;
		public Action<Collider> DoOnTriggerStay = DoNothingCollider;
		public Action<Collider> DoOnTriggerExit = DoNothingCollider;

		public Action<Collision> DoOnCollisionEnter = DoNothingCollision;
		public Action<Collision> DoOnCollisionStay = DoNothingCollision;
		public Action<Collision> DoOnCollisionExit = DoNothingCollision;

		public Action DoOnMouseEnter = DoNothing;
		public Action DoOnMouseUp = DoNothing;
		public Action DoOnMouseDown = DoNothing;
		public Action DoOnMouseOver = DoNothing;
		public Action DoOnMouseExit = DoNothing;
		public Action DoOnMouseDrag = DoNothing;
		public Action DoOnGUI = DoNothing;

		public System.Func<IEnumerator> enterState = DoNothingCoroutine;
		public System.Func<IEnumerator> exitState = DoNothingCoroutine;

		public Enum currentState;
		public StateMachineBase executingStateMachine;
	}

	//[HideInInspector]
	public State state = new State();

	public Enum CurrentState
	{
		get
		{
			return state.currentState;
		}
		set
		{
			if (stateMachine != this) {
				stateMachine.CurrentState = value;
			} else {
				string currentStateName = state.currentState == null ? "" : state.currentState.ToString();
				string nextStateName = value == null ? "" : value.ToString();
				
				if (state.currentState != value && !string.Equals(currentStateName, nextStateName)) {
					Debug.Log(string.Format("****** Change State : {0} ---> {1} ******", state.currentState, value), this);
					
					ChangingState();

					state.currentState = value;
					state.executingStateMachine.state.currentState = value;

					ConfigureCurrentState();
				}
			}
		}
	}

	[HideInInspector]
	public Enum lastState;

	[HideInInspector]
	public StateMachineBase lastStateMachineBehaviour;

	public void SetState(Enum stateToActivate, StateMachineBase useStateMachine)
	{
		if (state.executingStateMachine == useStateMachine && stateToActivate == state.currentState) {
			return;
		}

		ChangingState();
		state.currentState = stateToActivate;
		state.executingStateMachine = useStateMachine;

		if (useStateMachine != this) {
			useStateMachine.stateMachine = this;
		}
		state.executingStateMachine.state.currentState = stateToActivate;
		useStateMachine.Represent(gameObject);
		ConfigureCurrentState();
	}


	/// <summary>
	/// Caches previous states
	/// </summary>
	void ChangingState()
	{
		lastState = state.currentState;
		lastStateMachineBehaviour = state.executingStateMachine;
		timeEnteredState = Time.time;
	}

	void ConfigureCurrentState()
	{
		if (state.exitState != null) {
			stateMachine.StartCoroutine(state.exitState());
		}

		//Now we need to configure all of the methods
		state.DoUpdate = state.executingStateMachine.ConfigureDelegate<Action>("Update", DoNothing);
		state.DoOnGUI = state.executingStateMachine.ConfigureDelegate<Action>("OnGUI", DoNothing);
		state.DoLateUpdate = state.executingStateMachine.ConfigureDelegate<Action>("LateUpdate", DoNothing);
		state.DoFixedUpdate = state.executingStateMachine.ConfigureDelegate<Action>("FixedUpdate", DoNothing);
		
		state.DoUpdateCoroutine = state.executingStateMachine.ConfigureDelegate<System.Func<IEnumerator>>("UpdateCoroutine", DoNothingCoroutine);
		state.DoLateUpdateCoroutine = state.executingStateMachine.ConfigureDelegate<System.Func<IEnumerator>>("LateUpdateCoroutine", DoNothingCoroutine);
		state.DoFixedUpdateCoroutine = state.executingStateMachine.ConfigureDelegate<System.Func<IEnumerator>>("FixedUpdateCoroutine", DoNothingCoroutine);

		state.DoOnMouseUp = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseUp", DoNothing);
		state.DoOnMouseDown = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseDown", DoNothing);
		state.DoOnMouseEnter = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseEnter", DoNothing);
		state.DoOnMouseExit = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseExit", DoNothing);
		state.DoOnMouseDrag = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseDrag", DoNothing);
		state.DoOnMouseOver = state.executingStateMachine.ConfigureDelegate<Action>("OnMouseOver", DoNothing);
		state.DoOnTriggerEnter = state.executingStateMachine.ConfigureDelegate<Action<Collider>>("OnTriggerEnter", DoNothingCollider);
		state.DoOnTriggerExit = state.executingStateMachine.ConfigureDelegate<Action<Collider>>("OnTriggerExit", DoNothingCollider);
		state.DoOnTriggerStay = state.executingStateMachine.ConfigureDelegate<Action<Collider>>("OnTriggerEnter", DoNothingCollider);
		state.DoOnCollisionEnter = state.executingStateMachine.ConfigureDelegate<Action<Collision>>("OnCollisionEnter", DoNothingCollision);
		state.DoOnCollisionExit = state.executingStateMachine.ConfigureDelegate<Action<Collision>>("OnCollisionExit", DoNothingCollision);
		state.DoOnCollisionStay = state.executingStateMachine.ConfigureDelegate<Action<Collision>>("OnCollisionStay", DoNothingCollision);
		state.enterState = state.executingStateMachine.ConfigureDelegate<System.Func<IEnumerator>>("Enter", DoNothingCoroutine);
		state.exitState = state.executingStateMachine.ConfigureDelegate<System.Func<IEnumerator>>("Exit", DoNothingCoroutine);

		EnableGUI();

		if (state.enterState != null) {
			stateMachine.StartCoroutine(state.enterState());
		}

		OnChangeState();
	}

	Dictionary<Enum, Dictionary<string, Delegate>> _cache = new Dictionary<Enum, Dictionary<string, Delegate>>();

	T ConfigureDelegate<T>(string methodRoot, T Default) where T : class
	{

		Dictionary<string, Delegate> lookup;
		if (!_cache.TryGetValue(state.currentState, out lookup)) {
			_cache[state.currentState] = lookup = new Dictionary<string, Delegate>();
		}
		Delegate returnValue;
		if (!lookup.TryGetValue(methodRoot, out returnValue)) {

			var mtd = GetType().GetMethod(state.currentState.ToString() + methodRoot, System.Reflection.BindingFlags.Instance
				| System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod);

			if (mtd != null) {
				returnValue = Delegate.CreateDelegate(typeof(T), this, mtd);
			} else {
				returnValue = Default as Delegate;
			}
			lookup[methodRoot] = returnValue;
		}
		return returnValue as T;

	}

	protected void EnableGUI()
	{
		useGUILayout = state.DoOnGUI != DoNothing;
	}

	void Update()
	{
		state.DoUpdate();

		if (state.DoUpdateCoroutine != null) {
			stateMachine.StartCoroutine(state.DoUpdateCoroutine());
		}
	}

	void LateUpdate()
	{
		state.DoLateUpdate();

		if (state.DoLateUpdateCoroutine != null) {
			stateMachine.StartCoroutine(state.DoLateUpdateCoroutine());
		}
	}

	void FixedUpdate()
	{
		state.DoFixedUpdate();

		if (state.DoFixedUpdateCoroutine != null) {
			stateMachine.StartCoroutine(state.DoFixedUpdateCoroutine());
		}
	}

	void OnMouseEnter()
	{
		state.DoOnMouseEnter();
	}

	void OnMouseUp()
	{
		state.DoOnMouseUp();
	}

	void OnMouseDown()
	{
		state.DoOnMouseDown();
	}

	void OnMouseExit()
	{
		state.DoOnMouseExit();
	}

	void OnMouseDrag()
	{
		state.DoOnMouseDrag();
	}

	void OnTriggerEnter(Collider other)
	{
		state.DoOnTriggerEnter(other);
	}
	void OnTriggerExit(Collider other)
	{
		state.DoOnTriggerExit(other);
	}
	void OnTriggerStay(Collider other)
	{
		state.DoOnTriggerStay(other);
	}
	void OnCollisionEnter(Collision other)
	{
		state.DoOnCollisionEnter(other);
	}
	void OnCollisionExit(Collision other)
	{
		state.DoOnCollisionExit(other);
	}
	void OnCollisionStay(Collision other)
	{
		state.DoOnCollisionStay(other);
	}
	void OnGUI()
	{
		state.DoOnGUI();
	}
}
