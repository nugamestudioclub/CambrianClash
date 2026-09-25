extends CharacterBody2D

var knockbackPower = 150
@onready var enemy: Area2D = $enemy

func _ready() -> void:
	enemy.hit.connect(knockback)

func _physics_process(delta: float) -> void:
	pass
	
func knockback():
	velocity = Vector2(-1,0)
	var knockbackDirection = velocity.normalized() * knockbackPower
	velocity = knockbackDirection
	move_and_slide()
