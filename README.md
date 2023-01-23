# Recycle Rocket Race

### Please note that this source repository has all possibly copyrighted material and store assets stripped. It will probebly not compile!

Recycle Rocket Race is a fun puzzle game that lets you create a spaceship with real world objects in order to obtain the highest score.

This project was made for Project Hybrid at the HKU.

## Installation

To build the game you need [Unity 2021.3.11](https://unity.com/releases/editor/whats-new/2021.3.11).

If you just want to play the game, check the [releases tab](https://github.com/streepje8/Hybrid2022/releases).

## Requirements
In order to experience the game you need the following items:
1. A computer capable of running the application with at least 8GB of RAM and 4GB of VRAM.
2. A camera. (required)
3. A microphone. (optional)
4. Building material.
5. A clear, well lit space. (try avoid shadows as much as possible)
6. Speakers. (optional)

## Usage / FAQ
The game itself should be rather straightforward, however in certain scenarios the following instructions could be helpful:
- Make sure your play area has a neutral, evenly lit  background in order for the best results.

- In order to clear the existing spaceships, navigate to "C:\Users\\[UserName]\AppData\LocalLow\Pretendo\RecyleRocketRace" and clear it's continents, or pick the ones you want to keep.

- If you need to recallibrate the background for whatever reason, you can do so with left shift.

- If the player is done with building, you can skip through the timer by 60 seconds with the spacebar. 

- If the color detection isn't working properly, can try to switch to the sobel detection with the "use sobel detection instead of 
color detection" toggle on Assets\Materials\FinalProcessor.mat. This switches it from color detection to edge detection, and may improve the scanning in your scenario. 

- You can change the microphone and camera settings in the options menu.

- Scores are based on a RMI (Rocket Mass Index) generated on your rocket, so it detects if your rocket is overweight or underweight based on it's height.

- This game supports cheering in order to obtain a higher score, there is a slider in the settings which allow you to change the minimum cheering volume, adjust this when you find yourself in a loud environment. 
The louder you cheer, the higher the impact should be.

## Contributing

Once the project is over we don't expect to update the game any further, please refrain from opening issues or making pull requests. 

Instead consider forking the project and making your own version.

## License
You are free to use this project in however way you like, as long as it's non-profit.
