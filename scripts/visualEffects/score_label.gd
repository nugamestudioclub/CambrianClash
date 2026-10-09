extends Label

@export var point_val: int

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	text = str(point_val)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	position.y -= 1
	await get_tree().create_timer(1.0).timeout
	queue_free()
