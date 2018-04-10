# ARKit Monoscopic Portals
###### *Using Unity 2017.3 and ARKit 1.0.14*

---

## Features:
![Features](https://i.imgur.com/FLGv2qX.gif)
![Features](https://i.imgur.com/Pk56HIX.gif)
* Portals are **bidirectional shaders**.
* By assigning a **cubemap** image to a **Portal** prefab, each portal will have its own unique skybox portal instance. 
* Based on Camera gaze, the skybox is changed to provide respective portal experience.
* Multiple AR Portals may be laid out in a scene and invoked based on user camera gaze.
* ARKit uses a focus square animation (provided in ARKit Unity API) to analyze plane. 
* When dashed lines are used, point clouds are being analyzed to create a plane. When plane is detected, the square follows the surface as an enclosed set of edges.
---

### References:
#### Summarized Portal Tutorial:
[![FullPortalTutorial](https://img.youtube.com/vi/Z5AmqMuNi08/0.jpg)](https://www.youtube.com/watch?v=Z5AmqMuNi08)

All-in-one tutorial by Matthew Halberg explaining how to create a portal environment. For the sake of this framework, creating the portal prefab object has been based off this tutorial.

#### Shader Tutorial and Bidirectional Portal Collision Detection:
[![ShaderTutorial](https://img.youtube.com/vi/-9Fcoo1mVuo/0.jpg)](https://www.youtube.com/watch?v=-9Fcoo1mVuo)

Tutorial series by Pirates Just AR. This tutorial series explains how to utilize custom shader scripts to mask off the real world and augmented portal world. The shaders are applied to the skybox, and the portal quad plane. The shader is explained in one of the scripts referenced below. Additionally, collision detection for creating bidirectional rendering of the portal is explained in this tutorial series.

---
---


## Configuration:

### *WHEN IMPORTING PACKAGE, MAKE SURE iOS BUILD PLATFORM IS SET.*

## Adding 360 Images to Project:
All custom files, scripts, media content, and scene files are stored in the **Resources** Folder. Images are stored in ***Resources/360Images/.***

When adding images to the project, simply drag/drop the file into the 360Images folder using File Explorer/Finder.
### **MAKE SURE TO PROPERLY CONFIGURE IMAGE SETTINGS.** 

View the imported image in the editor inspector, and change the texture shape from **2D** to **cubemap.**

![2D_Cubemap](https://i.imgur.com/ZCCYVXK.gif)
---

## Adding Portal Instances to AR Scene:
![AddPortal](https://imgur.com/ZEzzep0.jpg)

* The **PlaneTapScene** is the horizontal-surface-instantiated AR scene. 
* The **PortalScene** object will contain all pertaining AR Portals. 
* For any **Portal** Object added, make them a child of the **PortalScene** object. 
#### The reason is because:
* *When app loads, the user must analyze surfaces for a horizontal plane AR instance.*
* *The user will tap to invoke the portals, this tap is what activates the AR scene; the portal objects. By default, the objects are disabled. This is to prevent them from randomly being instantiated in blank space as the user analyzes the environment for a flat surface.*
* **PlaneTapScene** *is responsible for creating a AR plane instance in the background via ARKit API.* **PlaneTapScene** *will allow PortalScene to instantiate the pertinent AR objects for the scene.*
---

## Setting Up Portal Prefab:
![SetUp_Portal](https://i.imgur.com/2aapOTe.gif)

Once adding Portal prefab to scene, click on the **Portal Transporter** child object. 
This plane is responsible for detecting collisions with user camera, and assigning skybox Portal.
You must specify the image to use from the 360 images folder, by default it’s using the Bladerunner 360 image.
Simply drag/drop the image into the **Image Skybox** field for the **Portal Transporter** object.

---
---



# How it works:

## Scripts:

#### *IdentifyPortal.cs*
---
This script is assigned to the **Main Camera** object. It is responsible for identifying which portal the user is staring at. Should it be the case there are multiple portals in a scene, each one needs to properly assign the skybox rendering. There can only be one skybox in a scene, but it can be dynamically changed. Therefore, based on the portal the user is gazing at, the skybox is set to correspond to the portal being explored.

This is done by shooting a raycast out from the camera. So imagine an invisible straight line shooting from the center of the camera, when it hits a collision with another game object, it checks if the collided-with object is tagged as a *‘Portal’*, if it is it references that portal to assign the skybox to the scene.

#### *InterdimensionalTransporter.cs*
---
This script is assigned to the **Portal Transporter** object, which is a child of the **Portal** object. It is responsible to detecting when a collision occurs between the user’s camera and the portal transporter object. Based on collision, the state of the users reality is checked. The importance of this is to allow for bidirectional portal travel. Furthermore, when a collision is invoked, the shaders are toggled from **Equal** to **Not Equal** and vic versa, hence rendering the appropriate content for the users line of sight. 

The script takes the assigned image, and creates a skybox material out of it. It assigns the **Custom/SkyboxCube** shader to create the masked content rendering described in **2nd cited YouTube series**.

---

## Shaders:
### *SkyboxCube/Skybox*
---
This shader contains a enumerate stencil that toggles between ***‘Equal’*** and ***‘Not Equal’***. 
***‘Equal’*** denotes a state in which the user is within a Portal Reality, and the Portal Window should render the camera feed.
***‘Not Equal’*** denotes a state in which the user is outside a Portal, and the Portal Window should render the skybox scene.

##### In depth explanation given:
https://www.youtube.com/watch?v=0eFo4ialKKQ&list=PLKIKuXdn4ZMhwJmPnYI0e7Ixv94ZFPvEP&index=6&t=342s

### *PortalWindow*
---
This shader is responsible for rendering the **Portal Transporter** Plane. A stencil is used for choosing which objects are rendered in users line of sight. 

##### In depth explanation given:
https://www.youtube.com/watch?v=b9xUO0zHNXw&list=PLKIKuXdn4ZMhwJmPnYI0e7Ixv94ZFPvEP&index=3&t=0s

---
---

### Author

**Alireza Bahremand**

* [github/TheWiselyBearded](https://github.com/TheWiselyBearded)
* [twitter/lirezaBahremand](https://twitter.com/lirezabahremand)

### License

Copyright © 2018, [Alireza Bahremand](https://github.com/TheWiselyBearded).
Released under the [MIT license](LICENSE).
