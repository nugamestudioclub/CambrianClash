extends Area2D

@export var left_side: bool = true

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		if left_side == true:
			body.position.x = 1160.0
		elif left_side == false:
			body.position.x = -8.0
