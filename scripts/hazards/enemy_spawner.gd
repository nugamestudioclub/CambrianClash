extends Marker2D

var can_spawn: bool = true
var PREY = preload("res://scenes/prey/base_prey.tscn")
var prey_list: Array = get_children(true)
var current_prey: int = get_child_count(true)
var total_prey: int
var cooldown: bool = true

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if get_child_count() < 4 and can_spawn and cooldown:
		spawn()
	if total_prey == 6:
		can_spawn = false

func spawn():
	cooldown = false
	var prey_ins = PREY.instantiate()
	prey_ins.position = position
	add_child(prey_ins)
	total_prey += 1
	print("bloop")
	print(current_prey)
	print(prey_ins.position)
	await get_tree().create_timer(10.0).timeout
	cooldown = true
