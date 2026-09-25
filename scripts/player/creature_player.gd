extends CharacterBody2D

var cooldown: bool = true
const SPEED = 100.0
const JUMP_VELOCITY = -100.0
var accel = 10.0
@onready var hitbox: Area2D = $hitbox
@onready var up: Marker2D = $up
@onready var down: Marker2D = $down
@onready var right: Marker2D = $right
@onready var left: Marker2D = $left

func _ready() -> void:
	hitbox.visible = false
	
func _physics_process(delta: float) -> void:
	
	var hit_direction = Input.get_vector("a", "d", "w", "s")
	
	if hit_direction == Vector2(0,-1):
		hitbox.position = up.position
	elif hit_direction == Vector2(0,1):
		hitbox.position = down.position
	elif hit_direction == Vector2(1,0):
		hitbox.position = right.position
	elif hit_direction == Vector2(-1,0):
		hitbox.position = left.position
		
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta * 0.1

	# Handle jump.
	if Input.is_action_just_pressed("jump"):
		velocity.y = JUMP_VELOCITY
		
	if Input.is_action_just_pressed("shift") and cooldown:
		cooldown = false
		hitbox.visible = true
		hitbox.add_to_group("player_hitbox")
		await get_tree().create_timer(0.1).timeout
		hitbox.remove_from_group("player_hitbox")
		hitbox.visible = false
		cooldown = true

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction := Input.get_axis("a", "d")
	velocity.x = move_toward(velocity.x, SPEED * direction, accel)

	move_and_slide()
