extends Area2D

enum States {ACTIVE, VUNERABLE}
var enemy_state
var speed: int = 50
var stop: bool = false
signal hit

@onready var sprite_2d: Sprite2D = $Sprite2D
var points = preload("res://scenes/visualEffects/label.tscn").instantiate()

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if !get_overlapping_areas().is_empty() and get_overlapping_areas()[0].is_in_group("player_hitbox"):
		enemy_state = States.VUNERABLE
		hit.emit()
		#knockback()
	if enemy_state == States.VUNERABLE:
		if stop == false:
			position += (Vector2.DOWN*speed) * delta
		sprite_2d.flip_v = true
	elif enemy_state == States.ACTIVE:
		sprite_2d.flip_v = false
		position += (Vector2.UP*speed) * delta
		
func _on_area_entered(area: Area2D) -> void:
	if area.is_in_group("player_hitbox"):
		#queue_free()
		enemy_state = States.VUNERABLE

func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("floor"):
		stop = true
		await get_tree().create_timer(7.0).timeout
		enemy_state = States.ACTIVE
		stop = false
	if body.is_in_group("player") and enemy_state == States.VUNERABLE:
		points.position = position
		add_sibling(points)
		queue_free()
		
