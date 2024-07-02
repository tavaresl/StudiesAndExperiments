using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class WallsRenderer : Node2D
{
	private List<Rect2> _wallSlices = new List<Rect2>();
	public List<Rect2> WallSlices
	{
		get => _wallSlices;
		set
		{
			_wallSlices = value;
			QueueRedraw();
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Draw()
	{
		if (!Engine.IsEditorHint())
		{
			var viewportSize = GetViewport().GetVisibleRect().Size;

			foreach (var wallSlice in WallSlices)
			{
				var color = Color.FromOkHsl(Mathf.Remap(240, 0, 359, 0, 1), 1f, Mathf.Remap(wallSlice.Size.Y, 0.2f * viewportSize.Y, viewportSize.Y, .2f, .5f));
				var posX = wallSlice.Position.X - wallSlice.Size.X / 2;

				DrawLine(new Vector2(posX, -viewportSize.Y / 2), new Vector2(posX, -wallSlice.Size.Y / 2), Colors.DarkSlateGray, width: wallSlice.Size.X);
				DrawLine(new Vector2(posX, -wallSlice.Size.Y / 2), new Vector2(posX, wallSlice.Size.Y / 2), color, width: wallSlice.Size.X);
				DrawLine(new Vector2(posX, wallSlice.Size.Y / 2), new Vector2(posX, viewportSize.Y / 2), Colors.DarkGray, width: wallSlice.Size.X);
			}
		}
	}
}
