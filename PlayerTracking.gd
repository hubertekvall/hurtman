extends PhantomCamera3D


@export var player_node : Node3D

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	super._process(delta)
	
	
	
	if global_position.distance_to(player_node) <
