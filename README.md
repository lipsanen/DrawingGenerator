# Description
For drawing binary images with the AR2 or the SMG for HL2 TASes. Takes in:
* Image path
* Output path
* Image position on map
* Player position on map
* Right vector (translates x coordinate on image to map coordinates)
* Down vector (translates y coordinate on image to map coordinates, usually 0 0 -1)
* Width in hammer units
* Weapon name
* Point sorting type (Random, Scan or Connected) (optional, default is Scan)

And generates framebulks into the output path that create the image. Arguments can be passed either in this order as command-line arguments or one by one in interactive mode.