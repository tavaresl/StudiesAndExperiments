using Godot;
using System;

namespace Scripts.Player;

public partial class PlayerVisionRayCast : RayCast2D
{
	[Signal]
	public delegate void CollidedEventHandler(RayCastCollision col);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsColliding())
		{
			EmitSignal(SignalName.Collided, new RayCastCollision
			{
				Collider = GetCollider(),
				ColliderRid = GetColliderRid(),
				Point = GetCollisionPoint(),
				Normal = GetCollisionNormal(),
			});
		}
	}
}

public partial class RayCastCollision : GodotObject
{
	public GodotObject Collider { get; set; }
	public Rid ColliderRid { get; set; }
	public Vector2 Point { get; set; }
	public Vector2 Normal { get; set; }
}
