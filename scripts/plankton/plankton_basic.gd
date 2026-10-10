extends Area2D

var d := 0.0
var radius := 100.0
var speed := 2.0
var point_val: int = 100
var dir_x: float = 0.5
var dir_y: float = 0.5

var points = preload("res://scenes/visualEffects/label.tscn").instantiate()

@export var X_OFF:float = position.x
@export var Y_OFF:float = position.y

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	d += delta
	position = Vector2(sin(d * speed) * radius, cos(d* speed)* radius) + Vector2(X_OFF,Y_OFF)
	X_OFF += dir_x
	Y_OFF += dir_y
func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		eaten()
	if body.is_in_group("floor"):
		dir_y = -dir_y
	if body.is_in_group("wall"):
		dir_x = -dir_x
		
func eaten():
	Score.add_points(point_val)
	points.position = position
	points.PointVal = point_val
	add_sibling(points)
	queue_free()
