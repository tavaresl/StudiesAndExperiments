using Godot;
using System;

namespace Scripts.Player;

[Tool]
public partial class Player : CharacterBody2D
{

	private Vector2 _heading = Vector2.Up;
	private float _fieldOfView = 45f;
	private float _viewDistance = 256f;

	[ExportGroup("Vision")]
	[Export]
	public Camera2D VisionCamera { get; set; }

	[Export(PropertyHint.Range, "0,359,0.1")]
	public Vector2 Heading
	{
		get => _heading;
		set => _heading = value.IsNormalized() ? value : value.Normalized();
	}

	[Export(PropertyHint.Range, "1,359,0.1,suffix:deg")]
	public float FieldOfView
	{
		get => _fieldOfView;
		set
		{
			_fieldOfView = value;
			UpdateFieldOfView();
		}
	}

	[Export(PropertyHint.Range, "0.1,1024,0.1,suffix:m,or_greater")]
	public float ViewDistance
	{
		get => _viewDistance;
		set
		{
			_viewDistance = value;
			UpdateViewDistance();
		}
	}

	[Export(PropertyHint.Range, "0,1000,0.1,or_greater,suffix:px")]
	public float CameraPlaneDistance { get; set; } = 100;

	[ExportGroup("Movement")]
	[Export(PropertyHint.Range, "1,5000,0.5,suffix:tiles/s")]
	public float Speed { get; set; }
	[Export(PropertyHint.Range, "0.01,2.0,0.01,or_greater")]
	public float Sensitivity { get; set; } = 0.1f;

	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;


		if (Input.IsKeyPressed(Key.J))
		{
			Heading = Heading.Rotated(-Mathf.DegToRad(.5f));
		}

		if (Input.IsKeyPressed(Key.L))
		{
			Heading = Heading.Rotated(Mathf.DegToRad(.5f));
		}

		Vector2 direction = Vector2.Zero;

		if (Input.IsKeyPressed(Key.W)) direction += Heading;
		if (Input.IsKeyPressed(Key.S)) direction -= Heading;
		if (Input.IsKeyPressed(Key.A)) direction += new Vector2(Heading.Y, -Heading.X);
		if (Input.IsKeyPressed(Key.D)) direction += new Vector2(-Heading.Y, Heading.X);

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMove)
		{
			var viewportWidth = GetViewport().GetVisibleRect().Size.X;
			var angle = Mathf.Remap(mouseMove.Relative.X, 0, viewportWidth, 0, 180);
			Heading = Heading.Rotated(Mathf.DegToRad(angle * Sensitivity));
		}
	}

	private void UpdateFieldOfView()
	{
		var vision = GetNodeOrNull<PlayerVision>("Vision");

		if (vision != null)
		{
			vision.FieldOfView = FieldOfView;
		}
	}

	private void UpdateViewDistance()
	{
		var vision = GetNodeOrNull<PlayerVision>("Vision");

		if (vision != null)
		{
			vision.ViewDistance = ViewDistance;
		}
	}
}
