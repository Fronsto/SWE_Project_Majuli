<div align="center" >
  
![ezgif com-video-to-gif (1)](https://github.com/Fronsto/SWE_Project_Majuli/assets/95305611/8d8041fd-38c2-463e-83ec-e4a90d611454)

### 360-degree images based VR Tour of Majuli Island, created with Unity

</div>

# Table of Contents
-   [Introduction](#introduction)
-   [Background](#background)
-   [Features](#features)
-   [How it works](#how-it-works)

# Introduction
The project is a virtual tour of the Majuli Island in the Brahmaputra River, which is known for its rich culture and biodiversity. Through the use of 360 degree images and information in the form of annotated locations and video playback, the product aims to give a holistic tour of the key parts to the user. The 360 images used to create the tour is not included in this repository, it was taken by the project team at UCNET Lab, IITG. 

# Background
So this was a project we did in our CS346 Software Engineering Lab, requiring us to go through the various necessary steps in software engineering life cycle, including analysing user requirements (based on which the app was divided into 3 separate tours, more on that [below](#features)), writing [SRS Document](./SRS_Document.pdf), building the [prototype](https://www.figma.com/proto/KZicVTB2ShAH26PWCwFcmU/Prototype?node-id=3-196&scaling=scale-down&page-id=4%3A5&starting-point-node-id=3%3A196), and making the final app. With around 300 images and 14 videos used, the final app came out to be around 1.3GB.

# Features

### User Class Separation
The application is split into 3 separate tours - for 3 separate user groups. When the app loads, the user is presented with a choice for which of these 3 tours to take. 

<img width="488" alt="image" src="https://github.com/Fronsto/SWE_Project_Majuli/assets/95305611/e19f6d09-ca9e-4cd0-91a7-8f2f0d75ca43">

1. **Visit the Temples**

   In this mode, various satras and the corresponding videos of the numerous rituals and practices of those satras are shown to the user. The experience is focused heavily towards the religious heritage of the Majuli island.
3. **Explore the Island**

   This mode is tailored for users who like a general tour of the island, exploring not only the satras but also the villages and towns that bear a significant cultural diversity. Various videos are ingrained at key points to enhance the experience of the user.
5. **Learn about the Island**

   This experience is aimed at the people who would like to know about the Majuli Island in depth, offering substantially more information than what a tourist would get. A number of villages and satras are shown to the user with informative text blocks (on the tablet) at numerous places to add to the learning of the user.

### Tablet
The user is provided with a virtual tablet, that serves as the central hub for teleportation, seeking information and accessing the menu in the application. This tablet adds to the features available with the user, with a simple interface. The tablet can be triggered using a pre-defined control, and does not obstruct the view of the user when not in use.

#### Map
The map of the Majuli Island with clickable buttons on the added locations is shown to the user, where his current position is indicated by the blue marker, and the available locations are in green . Clicking on a green location using the virtual stylus will teleport the user to the new location.

<img width="373" alt="image" src="https://github.com/Fronsto/SWE_Project_Majuli/assets/95305611/d2618004-1f06-4eb3-8557-474b0b47481a">

#### Videos/Media content on tablet
To correspond to the three user classes, video and informational text has been added to the tablet according to the locations. Each location has a maximum of one video associated with it, and Video of a cultural festival, playing on the tablet such locations are spread out throughout the virtual Island world. Whenever a particular location has some content associated with it, a notification icon pops on the user’s screen indicating him to open up the tablet to view that information.

<img width="374" alt="image" src="https://github.com/Fronsto/SWE_Project_Majuli/assets/95305611/0980f12b-c73a-444e-9a21-b00f76b92be3">

# How it works

### User Class Separation
In the code we have 4 scenes - one Start, and three for each user group. All the script files are same for all 3 user groups, and they make the distinction of user class by getting the scene name.

### World-Building
The places that are part of this tour are termed as “sites”, with each site containing multiple 360-images. Sites are presented on the map using which the user can teleport there directly, or they are connected to other sites via 360 video “transitions”. Map_Controller.cs script is used to call JumpToSite functions from Scene.cs script which changes the site accordingly.

### Tablet
The Virtual Tablet presented to user is controlled by Tab_Controller.cs script, but in the Start scene (before user selects the user class based tour) there is a separate script Start_Tab_Controller.cs . This script gets the input - which user class tour the user selected, and changes scene to that one.

### Automation
All the content that is presented in the virtual tour, including but not limited to, the data of the places, 360 images, the associated videos or text, places presented on the map for each user group etc. are not hard coded, but rather pulled up from the Resources folder. When the game loads, the Scene.cs scripts reads the json files, extracts all the 360 images from folders and builds the world. So, the world can be entirely customized by altering the files in the Resources folder.

The structure of the files and folders are as follows:
```
Resources
├── <SiteName>
│ └── 360images
│ └── img_x.jpg
│ └── locData.json
├── InfoText
├── Sounds
├── Transitions
├── Videos
├── InitialTexture.jpg
└── metaLocData.json
```
- Every site needs to have its own folder. Within it, there should be a locData.json file which contains the details of the images of that site, more on that later in this documentation. All the images needs to be placed within the 360images folder.

There are 4 folders that need to be present in the Resources folder 
- InfoText , which contains text files for informative text to be displayed on the tablet. This is only going to be shown for the expert user group, the code for this is in the TabController.cs file, if any modification is required
- Sounds , which contains all the ambient audio files.
- Transitions , which contains 360 videos
- Videos , which are normal 2D videos to be played on the tablet. This is only going to be shown for the devotees and travellers user groups, the code for this is in the TabController.cs file, if any modification is required

There are 2 files that need to be present in the Resources folder - InitialTexture.jpg (the first 360 image loaded when the user runs the app, that is displayed on the main menu screen), and metaLocData.json file which contains information about all the sites present in the app.

#### Sample metaLocData.json file
```
{
  "JumpToTraveller":["Auniati", "Kamalabari_Chariali"],
  "JumpToExpert":["Auniati", "Gormur_Gaon"],
  "JumpToDevotee":["Auniati", "Gormur_Gaon"],
  "Sites": [
    {
      "name": "Auniati",
      "data": {
        "initImage": "img_21_5",
        "ambientAudio": "birds_nature"
      }
    },
  ]
}
```
First we have 3 fields JumpTo for each user group - this list specifies the buttons that will be displayed on the map for the corresponding user group, i.e. these are the locations to which that user group can teleport. Buttons on the Map should have the same name as the location they represent, and only those buttons are shown whose id matches one of the values in the list provided here in this JumpTo field.

Then there is a list of “Sites” - each containing an object with the name of the site - this needs to be the same as the corresponding folder name for this site, and then in the data section there are 2 fields -
- initImage <mandatory field> which specifies the first image that will be displayed when the user teleports to this location, and
- ambientAudio <optional field> which specifies some ambient audio to be played for all the images within this location. This ambient audio file must be present in the Sounds folder under the Resources folder. The file name is specified without the extension (.mp3 or .wav, etc).

#### Sample locData.json file
```
{
  "items": [
    {
      "name": "img_21",
      "data": {
        "right": "img_21",
        "up": "img_20",
        "tabText": "1_Auniati",
        "navTextRight": "Towards Kamalabari Chariali",
        "navTextUp": "Towards Satra premises",
        "rotation": 0
      }
    },
    {
      "name": "img_21",
      "data": {
        "transition": true,
        "transVid": "tr1",
        "siteName": "Kamalabari_Chariali",
        "initImage": "img_19"
      }
    }
  ]
}
```
This file contains an array of items, each represents either an image, or a transition ( a 360 video played, followed by teleportation to some location).

Image
- name field should specify the image name that is placed in the 360images folder of the corresponding site.
- Images are not connected by coordinates, rather by explicitly specifying which image/transition comes in each direction - up , down , left and right . These are the only 4 directions. If an image doesn’t have anything in one of its direction, that field should be unspecified. Here in the above example file, the image “img_21” has one image to its up, and another (a transition) to its right.
- rotation field specifies rotation applied on the sphere when displaying the image to correct
the variations in camera angle.
- If there is some content to be played on the tablet at this image in the virtual tour, specify here using tabVideo and tabText fields.
- If the ambient audio to be played on this needs to be different from the one played on the entire site, specify using ambientAudio field.
- Finally, there are navTextUp / navTextDown / navTextLeft / navTextRight attributes. Text specified here will be displayed above the arrows, used for guiding the user through the virtual world.

Transition
- name could be anything.
- transition field should be true - this specifies that this is not a 360 image, but rather a dummy node where the transition video specified by transVid plays and then the user is teleported to the site specified by siteName with the initial image being specified by initImage .
- The transition video is a 360 video in equirectangular format. The video should be placed in the Transitions folder within the Resources folder.


