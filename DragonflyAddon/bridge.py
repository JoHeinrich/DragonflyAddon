def my_function(path):
	with open (path, "r") as file:
		exec(file.read())