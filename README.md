Reinforcement-Learning:

Actions:
Two Planes Controlled Independently:
Tilting around their middle axis.
Steps of ±0.1° up to a maximum of ±1.5°.
Discrete Action Space: Shape (2, 3)
First Dimension:
0: No change
1: Tilt up
2: Tilt down
Second Dimension:
0: No change
1: Tilt left
2: Tilt right


States:
Marble State Observation:
Continuous State Space: Shape (3, 3)
First Dimension: Position (x, y, z)
Second Dimension: Angular velocity (x, y, z)
Third Dimension: Velocity (x, y, z)
Tilt Angles Not Included in State: (The angles of the planes are not part of the observed state)


Rewards:
Reward and Penalty System:
-5 for hitting a hole.
+10 for hitting a checkpoint (white bar).
-0.005 per timestep (encouraging efficiency in finding a direct path).


Episodes:
Starting Conditions:
Each episode starts with the marble in the top left corner.
The initial position varies randomly each time within ±0.003 in x- and y-dimension.
Episode Termination Conditions:
Ends if the marble:
Hits a hole.
Hits the target.
Fails to hit the next checkpoint within 6 seconds since the last checkpoint.


Checkpoints:
Checkpoints provide intermediate rewards, encouraging progress steps rather than only rewarding the final target.
This environment sets up a maze navigation problem where the agent controls two planes to guide a marble through a maze while receiving rewards and penalties based on its actions and progress. The agent's goal is to learn a policy that maximizes the cumulative reward by efficiently navigating through the maze, avoiding holes, reaching checkpoints, and ultimately reaching the target while considering the constraints of the episode termination conditions.





