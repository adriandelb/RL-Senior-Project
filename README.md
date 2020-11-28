# RL-Senior-Project
Repository for UTRGV senior project 2020. Project used Unity Ml agents to create environments


## File Info
### Included Files
* UnitySeniorProject - This file contains the unity  environments
* Phases - Supporting files needed for training
### Additional Files
* ml-agents-release_7 - This files needs to be downloaded from [ML Agents Github](https://github.com/Unity-Technologies/ml-agents/tree/release_7)

## Requirements
* Unity 2018.4 or later
* Python 3.6.1 or higher
* ml-agents-release_7 file

## Setting up Environment
### PIP Installs
1. `pip install mlagents==0.20.0 --use-feature=2020-resolver`
2. `pip install mlagents_envs==0.20.0 --use-feature=2020-resolver`
3. `pip install gym_unity==0.20.0 --use-feature=2020-resolver`
### Unity Configuration
1. Open unity hub, add a project and select the folder**UnitySeniorProject**
2. Once in the project go to assets -> add from disk -> navigate to ml-agents-release_7 folder -> com.unity.ml-agents -> select **package.JSON**

## Navigation
Once you have set up your environment you will see several folders in the assets where console is located. If you open scenes you will be able to open different scenes. 
## Training
Follow the steps to set up training for the scene:
1. locate the phases folder in the main RL-Senior-Project folder
2. You will see three folders labeled phase_1, phase_2, phase_3. Select the phase that you are interested in.
3. In the folder you will see a .YAML file. 
	 >This file needs to be added to the config folder in the ml-agents-release_7 folder.
4. Copy the YAML file and navigate to ml-agents-release_7 folder
5.  ml-agents-release_7 -> config -> ppo. Paste YAML in ppo folder. 
6. To start training open a command line window and navigate to the base ml-agents-release_7 directory.
7. Once you are in the directory type the following `mlagents-learn config/ppo/SimpleEnvironment.yaml --run-id=SErun1`.
	> You will need to change the file name of the yaml you want to run as well as the run name
8. Go back to unity and press play. Make sure the agent is not in heuristic. Change to default if in heuristic. 
9. Open another command line window and navigate to the base ml-agents-release_7 directory.
10. Type in the following for *tensorboard*; `tensorboard --logdir results --port 6006` 