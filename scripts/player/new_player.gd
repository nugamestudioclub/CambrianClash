extends CharacterBody2D

var cooldown: bool = true
var SPEED = 200.0
var JUMP_VELOCITY = -5.0
var accel = 5.0
var gravity_factor: float = 0.1
var velocity_cap = 250
@onready var hitbox: Area2D = $hitbox
@onready var up: Marker2D = $up
@onready var down: Marker2D = $down
@onready var right: Marker2D = $right
@onready var left: Marker2D = $left
@onready var sprite: Sprite2D = $Sprite2D

func _ready() -> void:
	hitbox.visible = false
	
func _physics_process(delta: float) -> void:
	
	var hit_direction = Input.get_vector("a", "d", "w", "s")
	
	if hit_direction == Vector2(0,-1):
		hitbox.position = up.position
		sprite.rotation = 0
	elif hit_direction == Vector2(0,1):
		hitbox.position = down.position
		sprite.rotation = PI
	elif hit_direction == Vector2(1,0):
		hitbox.position = right.position
		sprite.rotation = PI/2
	elif hit_direction == Vector2(-1,0):
		hitbox.position = left.position
		sprite.rotation = -PI/2
		
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta * gravity_factor
		SPEED = 200.0
		#accel = 5.0
	else:
		SPEED = 150.0
		accel = 10.0

	# Handle jump.
	if Input.is_action_pressed("jump"):
		velocity.y += JUMP_VELOCITY 
		accel = 10.0
	if Input.is_action_just_released("jump"):
		accel = 1.0
		
	if Input.is_action_pressed("s"):
		gravity_factor = 0.2
		velocity_cap = 300
	else:
		gravity_factor = 0.1
		velocity_cap = 250
		
		
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

	velocity.y = clampf(velocity.y, -velocity_cap, velocity_cap)
	move_and_slide()
	print(velocity)
	
