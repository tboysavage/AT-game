Reinforcement-Learning:

actions
The two planes can be controlled by the ML agent independantly, i.e. they are tilted around their middle axis in steps of ±0.1° up to ±1.5° max. The action space is discrete, shape (2, 3):

1st dimension: 0 = no change, 1 = tilt up, 2 = tilt down
2nd dimension: 0 = no change, 1 = tilt left, 2 = tilt right
states
The ML Agent receives the state of the marble. The state space is continuous, shape (3, 3):

1st dimension: position (x, y, z)
2nd dimension: angular velocity (x, y, z)
3rd dimension: velocity (x, y, z)
Note, that the tilt angle of the planes is not part of a state.

rewards
The ML Agent receives rewards / penalties

-5 for hitting a hole
+10 for hitting a checkpoint (white bar)
-0.005 per timestep
Note: the penalty per timestep encourages the ML Agent to find a fast and direct path through the maze.

episodes
Each episode starts with the marble in the top left corner. The start position is randomly changed each time up to ±0.003 in x- and y-dimension. An episode ends if,

the marble hits a hole
the marble hits the target
the marble has not hit the next checkpoint within 6 seconds since the last checkpoint
Note: the checkpoints are necessary to reward small progress steps rather than rewarding the final target only.
