using Godot;
using System;
using System.Collections.Generic;

namespace Scripts.Player;

[Tool]
public partial class PlayerVision : Node2D
{
	private Player _parent;
	private float _fieldOfView = 45;
	private float _viewDistance = 20f;
	private float _distanceScale = 10f;

	[Export]
	public Camera2D Camera { get; set; }

	[Export]
	public PackedScene WallDetectorRaycast2D { get; set; }

	[Export(PropertyHint.Range, "0,359,0.1,suffix:deg")]
	public float FieldOfView
	{
		get => _fieldOfView;
		set { _fieldOfView = value; SpawnRays(); }
	}

	[Export(PropertyHint.Range, "0,1080,0.1,suffix:m")]
	public float ViewDistance
	{
		get => _viewDistance;
		set { _viewDistance = value; SpawnRays(); }
	}

	[Export(PropertyHint.Range, "0.1, 100,suffix:px/m,or_greater")]
	public float DistanceScale
	{
		get => _distanceScale;
		set { _distanceScale = value; SpawnRays(); }
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_parent = GetParent<Player>();
		Camera ??= _parent.VisionCamera;
		SpawnRays();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Rotation = _parent.Heading.Angle();
		var rays = GetChildren();
		var viewportWidth = GetViewport().GetVisibleRect().Size.X;
		var viewportHeight = GetViewport().GetVisibleRect().Size.Y;
		var rectWidth = viewportWidth / rays.Count;
		var slices = new List<Rect2>();

		for (int i = 0; i < rays.Count; i++)
		{
			if (rays[i] is RayCast2D ray && ray.IsColliding())
			{
				var posX = 0 - (viewportWidth / 2) + (rectWidth * i);
				var collisionPoint = ray.GetCollisionPoint();
				var dist = _parent.Position.DistanceTo(collisionPoint) - _parent.CameraPlaneDistance;
				var rectHeight = .5f * viewportHeight / dist;
				var rect = new Rect2(new Vector2(posX, -rectHeight / 2), new Vector2(rectWidth, rectHeight * 30));

				slices.Add(rect);
			}
		}

		if (Engine.IsEditorHint()) QueueRedraw();

		if (Camera != null)
		{
			var wallsRenderer = Camera.GetNodeOrNull<WallsRenderer>("WallsRenderer");

			if (wallsRenderer != null)
			{
				wallsRenderer.WallSlices = slices;
			}
		}
	}

	public override void _Draw()
	{
		if (Engine.IsEditorHint())
		{
			var planeOrigin = Vector2.FromAngle(0) * _parent.CameraPlaneDistance;
			var minPointY = (float)Mathf.Tan(-Mathf.DegToRad(FieldOfView / 2)) * _parent.CameraPlaneDistance;
			var maxPointY = (float)Mathf.Tan(Mathf.DegToRad(FieldOfView / 2)) * _parent.CameraPlaneDistance;
			var lineStartPoint = new Vector2(planeOrigin.X, minPointY);
			var lineEndPoint = new Vector2(planeOrigin.X, maxPointY);

			DrawCircle(lineStartPoint, radius: 5f, Colors.Black);
			DrawCircle(lineEndPoint, radius: 5f, Colors.Black);
			DrawLine(lineStartPoint, lineEndPoint, Colors.Black, width: 5f);
		}
	}

	private void SpawnRays()
	{
		foreach (var child in GetChildren())
		{
			child.QueueFree();
		}

		if (_parent == null)
		{
			return;
		}

		Vector2 viewport = new(320, 140);
		int halfAngle = (int)Math.Floor(Math.Abs(FieldOfView / 2));
		float angleBetweenRays = FieldOfView / viewport.X;

		for (int i = 0; i < viewport.X; i++)
		{
			var ray = WallDetectorRaycast2D.Instantiate<RayCast2D>();
			ray.CollisionMask = (uint)Math.Pow(2, 1 - 1);
			ray.TargetPosition = Vector2.FromAngle(0).Rotated(Mathf.DegToRad(-halfAngle + angleBetweenRays * i)) * ViewDistance * DistanceScale;
			AddChild(ray);
		}
	}
}
