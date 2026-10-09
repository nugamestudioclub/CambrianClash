extends CharacterBody2D

enum States {ACTIVE, VUNERABLE}
var enemy_state = States.ACTIVE
var can_eat: bool = false
var speed: int = 50
var velocity_cap = 150
var gravity_factor: float = 0.1
var stop: bool = false
@export var dir: Vector2 = Vector2.RIGHT
signal hit

@export var point_val: int = 100

@onready var sprite_2d: Sprite2D = $Sprite2D
@onready var area_2d: Area2D = $Area2D

var points = preload("res://scenes/visualEffects/label.tscn").instantiate()
@onready var left_cast: RayCast2D = $leftCast
@onready var right_cast: RayCast2D = $rightCast


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	if !area_2d.get_overlapping_areas().is_empty() and area_2d.get_overlapping_areas()[0].is_in_group("player_hitbox"):
		enemy_state = States.VUNERABLE
		hit.emit()
		#knockback()
		
	if left_cast.is_colliding():
		dir = Vector2.RIGHT
	if right_cast.is_colliding():
		dir = Vector2.LEFT
		
	if enemy_state == States.VUNERABLE:
		velocity += get_gravity() * delta * gravity_factor
		sprite_2d.flip_v = true
		await get_tree().create_timer(0.2).timeout
		can_eat = true
		if is_on_floor():
			stop = true
			await get_tree().create_timer(7.0).timeout
			enemy_state = States.ACTIVE
			stop = false
	elif enemy_state == States.ACTIVE:
		sprite_2d.flip_v = false
		can_eat = false
		position += (dir*speed) * delta
	velocity.y = clampf(velocity.y, -velocity_cap, velocity_cap)
	move_and_slide()

func _on_area_2d_area_entered(area: Area2D) -> void:
	if area.is_in_group("player_hitbox"):
		enemy_state = States.VUNERABLE
		
func _on_area_2d_body_entered(body: Node2D) -> void:
	if body.is_in_group("player") and enemy_state == States.VUNERABLE and can_eat:
		eaten()

func eaten():
	Score.add_points(point_val)
	points.position = position
	points.point_val = point_val
	add_sibling(points)
	queue_free()
