# Sergio's Camera Shaker
A simple and easy-to-use Unity component to add juice and game feel to your games. It was made following the tips of this amazing talk by Squirrel Eiserloh, and I've been perfecting it for my latest projects:
https://www.youtube.com/watch?v=tu-Qe66AvtY&list=PLBJR0H4rFtvNiYaPg23POOJRNl3oXZZJ0&index=4&t=295s

Checkout how the shaking looks here: https://youtu.be/AElnVwOUQMw

This repository includes the needed scripts for the component to work and a sample project that lets you test out in real time some of the possibilities that it offers.

Once you've downloaded the files to your project, you can add the component by clicking *Add Component/Camera Shake/Sergio's Cam Shaker* on the Unity inspector. The difference between the regular and 2D versions is that the regular version only rotates the camera, while the 2D one moves the camera in the X and Y axis and rotates it in the Z. Both of them rotate/move the camera in local space. So if you want to move or rotate it with other components you should make the camera children of an empty game object and move/rotate that one instead.

### Contents:
* [Inspector](#Inspector-Variables)
* [Public Methods](#Public-Methods)
* [Defined Type: Distance Shake](#Defined-Types)
* [Public Variables](#Public-Variables)
* [Contact](#Contact)
* [License](#License)

### Inspector Variables
| Variable |  |
| ------ | ------ |
| **Max Shake Values** | The max amount of displacement for each axis. |
| **Magnitude Reduction** | Reduction of shake per frame. Multiplied by delta time. |
| **Reduction Method** | How the shake is reduced: Linear, Square or Cube. |
| **Noise Speed** | Speed at which the perlin noise moves per frame. |

![Inspector_Image](https://drive.google.com/uc?export=view&id=18tfsXHcsREbGAp92oOf4LWadyn3twUzf)

### Public Methods
Of the *CameraShaker* component that you can call from your code to manipulate the shake of the camera.
##### > `` AddShake (float magnitude) ``
Increase the amount of camera shake by ``magnitude``. ``Magnitud`` should be between 0 and 1 in all these functions. If not, it would be clamped.
##### > ``AddClampShake (float magnitude, float max)``
Increase the cam shake by ``magnitude``, only if the current shake is less than ``max``.
If the difference between ``max`` and the current shake is less than ``magnitude``, only the amount needed to reach ``max`` is added.
##### > ``AddLimitedShake (float magnitude)``
Works as AddClampShake() but using ``magnitude`` itself as max. Setting the camera shake to ``magnitude``, only if it is not already more than that.
##### > ``SetShake (float magnitude)``
Set the camera shake to ``magnitude``.
##### > ``AddShakeByDistance (Vector3 _origin_, Distanceshake distanceShake)``
Shake the camera based on its distance to an ``origin`` point and what is indicated by ``distanceShake``.
This function evaluates the ``distanceShake.falloffCurve`` to add the indicated magnitude at the current distance, using ``AddLimitedShake()`` to do so.

### Defined Types
##### > ``DistanceShake``
You can create in any of your scripts a _DistanceShake_ variable. It's a struct that contains information used by the AddShakeByDistance() method:
- ``Vector2 minMaxDis``. Where X is the min and Y the max distance to be had in count. Any distance below or above these values would be clamped between them by the ``AddShakeByDistance()`` function.
- ``Float minMagnitude`` & ``Float maxMagnitude``. Lower and greater shake magnitudes that can be added.
- ``AnimationCurve falloffCurve``. Defines how much shake is added based on the distance.
The X-axis represents the distance, where 0 is ``minDis`` and 1 is ``maxDis``.
The Y-axis is the shake magnitude to be added at a given distance. 0 been ``minMagnitude`` and 1 been ``maxMagnitude``.
```cs
DistanceShake distanceShake = new DistanceShake(minDis, maxDis, minMagnitude, maxMagnitude);
// The falloffCurve is set to a linear decrease by default.
```
![DistanceShake_Image](https://drive.google.com/uc?export=view&id=1brLp8T8uJE0Ko8N6lc4x5kCcAcqo-Xy8)

### Public Variables
| Variable |  |
| ------ | ------ |
| ``float magnitude`` | A read only variable that indicates the current amount of camera shake. At 0 the camera is not shaking, at 1 it is shaking at the max values. |
| ``float verMaxShake`` | Max shake in the X axis. |
| ``float horMaxShake`` | Max shake in the Y axis. |
| ``float rollMaxShake`` | Max shake in the Z axis. |
| ``Vector3 maxShake`` | Get or Set all the axles' maxShake values at the same time as a Vector3. |
| ``float magnitudeReduction`` | Reduction of magnitude per frame. |
| ``float noiseSpeed`` | Speed at which the perlin noise moves per frame. |

### How it works
Internally, the component has a ‘_trauma_’ value that defines the current amount of camera shake. It’s always between 0 and 1 (the value is always clamped after been modified by the above functions). At 0 the camera is not shaking, at 1 the camera is been displaced at the max shaking values.
Whenever one of the add() functions is called, if the camera is already shaking the _trauma_ is simply changed. If there is no shake, a new shaking coroutine is started.
This coroutine moves or rotates the camera every frame using, instead of just a random number between 1 and -1, the value extracted from a Perlin noise map. This is done in order to make each shake feel more connected to one another, and the shake overall more fiscal and organic.
The camera displacement for each axis is calculated by multiplying the _maxShake_ values 
by the -1 to 1 Perlin noise value to define the direction of the displacement
by the trauma, trauma square or trama cube, depending on the _reductionMethod_. By default, it's set to square because in that way the shake is reduced tracing a smoother curve than linear. Feeling, again, better and more organic.
Then the Perlin noise coordinates are moved and the trauma reduced for the next frame. When the _trauma_ reaches cero, the coroutine ends.

### Contact
* Issues: *https://github.com/S-LucasSerrano/CameraShaker/issues*
* Email: *Lucas.ss.Serrano@Gmail.com*
* Portfolio: *https://sergiolucasserrano.wixsite.com/website*

### License
MIT License
Copyright (c) 2021 Sergio Lucas Serrano
More on the _LICENSE.txt_ file.
