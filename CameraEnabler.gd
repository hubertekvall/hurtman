extends Node3D

@export var player_target : Node3D
@export var cameras : Array[PhantomCamera3D]

func _process(delta: float) -> void:
	if not player_target or cameras.is_empty():
		return
	
	var closest_camera : PhantomCamera3D = null
	var closest_dist : float = INF

	for camera in cameras:
		var dist = camera.global_position.distance_to(player_target.global_position)
		if dist < closest_dist:
			closest_dist = dist
			closest_camera = camera

	# Set priorities
	for camera in cameras:
		if camera == closest_camera:
			camera.priority = 1
		else:
			camera.priority = 0
