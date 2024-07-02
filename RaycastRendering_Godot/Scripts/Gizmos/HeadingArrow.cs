using Godot;
using Scripts;
using System;

[Tool]
public partial class HeadingArrow : Node2D
{
	private Polygon2D _polygon2D;
	private Line2D _line2D;

	private Color _color;

	[Export]
	public Color Color
	{
		get => _color;
		set
		{
			_color = value;
			Redraw();
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			Visible = true;
		}
		else
		{
			Visible = false;
		}

		_polygon2D = GetNode<Polygon2D>("Polygon2D");
		_line2D = GetNode<Line2D>("Line2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void Redraw()
	{
		if (_polygon2D != null && _line2D != null)
		{
			_line2D.DefaultColor = Color;
			_polygon2D.Color = Color;
		}
	}
}
