# Whack-A-Homer

A Tangible Augmented Reality videogame that challenges your concentration and memory.
Being based on the popular arcade game Whack-A-Mole, you are presented Homers with
colored shirts. You must hit the one that matches the color presented before on the board.

![gameplay](https://raw.github.com/alculquicondor/Whack-a-Homer/master/doc/gameplay.jpg)

Have fun and challenge your friends to beat your record!

In order to play, you need a cellphone,
the [Google Cardboard](https://www.google.com/get/cardboard/),
and this target (just print it):

![target](https://raw.github.com/alculquicondor/Whack-a-Homer/master/doc/boardtarget.png)

You can use your hands or a plastic hammer to hit the Homers.

## Authors

This game was created at San Pablo Catholic University (Peru) for the
Human Computer Interaction class (spring 2015) by:

- [Aldo Culquicondor](https://github.com/alculquicondor)
- [Adolfo Tamayo](https://github.com/adolfo1994)

## Building

### Requirements

- [Unity 5.2.x](https://unity3d.com/es/get-unity/download/archive), the game engine.
- [Vuforia 5.0.x](https://developer.vuforia.com/downloads/sdk), an Augmented Reality SDK.
- [Cardboard SDK 0.5.0](https://github.com/googlesamples/cardboard-unity/releases), to render on the Google Cardboard and implement head-tracking.

### Do it yourself

If you want to do a similar project, here are some recommendations:

- Get familiar with Unity. Get used to program with the refresh loop. Learn how to implement
sounds, how to change colors and textures at runtime, hiding and showing up elements, moving
elements relative to others, etc.
- Get familiar with Unity and Vuforia SDK. [Here](https://developer.vuforia.com/downloads/samples) you can run.
- Get familiar with Unity and Cardboard SDK. [Here](https://developers.google.com/cardboard/unity/) is the documentation.
- Integrate Vuforia with Cardboard SDK. [Here](https://developer.vuforia.com/library/articles/Solution/Integrating-Cardboard-SDK-050) are the steps. Make sure you are using Cardboard SDK
version listed on the post (0.5.0 at the time of this writing).
- Design your [targets](https://developer.vuforia.com/library/articles/Best_Practices/Attributes-of-an-Ideal-Image-Target) carefully. Use images with sharp edges. We found out that images
composed by stars are easy to track and contain good amount of features. Make sure your
images get the maximum punctuation on Vuforia's Target Manager.
- Learn how to design [virtual buttons](https://developer.vuforia.com/library/articles/Solution/How-To-Implement-Virtual-Buttons). Test the sensibility according to your needs.
Documentation is not good enough about implementing virtual buttons on Unity, so go ahead and read our code on `Assets/Scripts/HitScript`.
