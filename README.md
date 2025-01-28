# AR_Foundation_Task

Main_Scene_2 Hierarchy, GameObject Roles and Scene Setup

![SceenSetup_1](https://github.com/user-attachments/assets/8af54312-2850-4925-a7d1-654a2153c3c0)


1.AR Session : In Unity's AR Foundation, an AR session manages the lifecycle and state of an AR experience. It initializes AR functionality, handles device tracking, and communicates with the underlying AR platform to enable features like plane detection, object tracking, and environmental understanding.

2. AR Session Origin :  In Unity's AR Foundation acts as the root for all AR content. It ensures that tracked AR objects, such as planes or anchors, are properly positioned, scaled, and oriented in relation to the real-world environment detected by the device.
  -> This AR Session Origin has few components on it.
     1. AR session Origin with AR Camera as reference. 
     2.AR Plane Manager : is a component that detects and tracks horizontal and vertical planes in the real world. It automatically creates, updates, and manages ARPlane GameObjects for each detected plane, enabling features like placing objects on flat surfaces.
     3. AR Raycast Manager:allows you to perform raycasts against real-world surfaces detected by the AR device. It is used to determine where a ray, such as a touch input, intersects with features like planes, enabling interactions like object placement or hit detection in AR.

3. The FloatingUI_canvas is a world space canvas that serves as a floating user interface in the AR application. It is placed as a child of the AR Camera to ensure it moves with the user's perspective.

   ![UI_Menu](https://github.com/user-attachments/assets/909182dc-d760-43e3-b2fc-15042eb54c8b)

  Components
  UI_Menu:
  A child of the FloatingUI_canvas containing interactive buttons for user actions.
  Buttons:
    Model_A Button
    Model_B Button
    Reset Button
  
  Functionality
    Model_A and Model_B Buttons:
      OnClick Events:
        Trigger the scaling down of the UI_Menu.
        Update the model_Index variable in the GameManager script to correspond to the selected model.
        Instantiate the respective model in the AR environment based on the updated model_Index.
    Reset Button:
      OnClick Events:
        Clear all instantiated models in the AR environment.
        Reset the spawned models' count to 0.
        Scale down the UI_Menu.
      
  Script Integration
    GameManager Script:
      Manages the model_Index to determine which model is instantiated.
      Handles the instantiation and clearing of models in response to button interactions.
      Ensures proper tracking of the spawned models' count for resetting purposes.
  User Interaction Flow
    User clicks Model_A or Model_B button:
      The UI_Menu scales down, and the selected model is instantiated.
    User clicks the Reset button:
      All instantiated models are cleared, and the spawned models' count is reset to 0.
      The UI_Menu scales down, signaling the reset action is complete.

4. Canvas : This is a screenspace-Overlay canvas which has a Button called UI_Menu_Controller_Button : which on click will scaleup and scale down the UI_Menu of FloatingUI_Menu. and a TextMeshPro Text called Models_Spawn_Count_text which will represent the count of spawned Models.
5. The Game_Manager is an empty GameObject in the Unity scene that acts as the central controller for coordinating and executing all major functionalities within the AR application.

   ![image](https://github.com/user-attachments/assets/d9b0adce-41d5-4832-bde4-d0895cf93e65)


Components
Game Manager Script:
  Core script that handles logic and operations such as model instantiation, UI interactions, and reset functionality.
  Responsible for managing the model_Index to determine which model to instantiate.
  Tracks the number of spawned models and ensures proper handling of reset actions.
  
  Audio Source Component:
   Plays audio cues as needed, such as feedback sounds for button clicks or user interactions.
   Can be triggered by the Game Manager Script to enhance user experience with audio feedback.
   
 Key Responsibilities
  Model Management:
    Instantiate models (e.g., Model_A or Model_B) based on the updated model_Index when triggered by button events from the FloatingUI_canvas.
    Clear all instantiated models and reset the spawned models' count to 0 when the reset action is initiated.
  UI Interaction:
   Works in tandem with the FloatingUI_canvas to respond to user inputs.
   Updates the application state dynamically based on button clicks (Model selection or Reset).
   
  Audio Integration:
   Plays audio feedback during button interactions or other key events, enhancing the interactivity of the application.

6. The Placement_Indicator is a Quad GameObject used to visually indicate a placement area in the AR scene. It serves as a reticle to guide users in placing objects within the environment.

   ![image](https://github.com/user-attachments/assets/f19ecc1a-9df3-4374-9ef3-c0181c332340)
   

  Components : 
    Quad:
     A flat rectangular surface acting as the base for the placement indicator.
     Positioned dynamically in the AR scene to highlight the detected placement area.
   Material:
     Applied to the quad to create a reticle-like appearance, making it visually distinct and intuitive for users.
     Can include transparency, animations, or visual effects to enhance user feedback.
  Functionality : 
   Dynamic Placement:
    The Placement_Indicator moves and aligns itself with the AR surface detected by the device (e.g., AR planes).
    Helps users identify valid placement areas for models in the AR environment.
   User Feedback:
    Provides real-time visual cues, ensuring users understand where objects can be placed.
    Can change appearance (e.g., color or size) based on whether the placement area is valid or invalid.
  Integration : 
    The Placement_Indicator is updated by scripts (e.g., AR Placement scripts) that track and align it with detected surfaces in the AR scene.
    Often works in tandem with AR Foundation's plane detection and raycasting features to ensure precise positioning.

Assests :

![Assest_ICAT_Task](https://github.com/user-attachments/assets/d02e6fc5-5786-4263-9031-73b58a526384)


  ICAT_Task Folder Assets
    1.Animator Controllers: Assigned for Model_A and Model_B to play walk and run animations.
    2.Prefabs: Includes ARPlane, Model_A and Model_B animated characters downloaded from Mixamo, as well as other trail GameObject prefabs.
    3.Models: Contains the origin downloaded Model_A and Model_B avatar prefabs and animation clips.
    4.Materials: Materials for Model_A, Model_B, and the Placement_Indicator.
    5.Audios: Contains audio files for body parts.
    6.Scenes: Holds two scenes, Main_Scene and Main_Scene_2, representing the same functionality with some modifications in ARPlane and the Floating UI Menu.
    7.Scripts: Includes the GameManager and BodyPartDetection scripts used in the application.
    8.Sprites: Contains sprites for Model_A, Model_B, Placement_Indicator, and other button sprites.

Scripts Explanation:

   1. Game Manager : 
      Breakdown of what each function in the GameManager script is doing:
        1. Start()
          Initializes variables at the start of the game.
          Retrieves the saved spawn count (spawnModelCount) from PlayerPrefs and updates the text that displays this count (spawn_count).
          Sets up a listener for the menuButton to call the ToggleScale function when clicked.
          Calls the ScaleUp method to show the UI menu.

        2.ScaleUp()
          Scales up the UI menu (the UI_Menu GameObject) using DOTween over the time defined by scaleDuration.
          Disables interaction with the menuButton while the scaling animation is happening.
          Sets the isScaledUp flag to true when the scaling animation is complete and enables the button interaction.
      
        3.ScaleDown()
          Scales down the UI menu using DOTween over the same scaleDuration time period.
          Disables the button interaction during the scaling animation and sets the isScaledUp flag to false once the animation is complete.
          Sets the pause flag to true to indicate the UI menu is scaled down.
      
        4.UpdatePlacementIndicator()
          If the placement pose is valid (i.e., the AR raycast detected a valid placement position), it shows the placementIndicator (a visual cue of where to place the object).
          The indicator's position and rotation are updated to reflect the placement pose.
      
        5.UpdatePlacementPose()
          Casts an AR ray from the screen's center to detect AR planes.
          If a plane is detected, the placement pose is updated.
          The script checks whether the plane is horizontal or vertical, and adjusts the rotation of the placement indicator accordingly (rotating it 90 degrees for horizontal planes and leaving it as-is for vertical ones).
          The camera's forward direction is used to update the rotation of the placement pose.
      
        6.ResetSpawnedObjects()
          Destroys all previously spawned objects stored in the spawnedObjects list.
          Clears the list and resets the spawn count to 0.
          Saves the updated spawn count to PlayerPrefs and updates the UI text.
      
        7.Update()
          If the game is paused (i.e., pause == true), the function checks for touch input.
          If a touch begins, it checks whether the time between taps is less than the double-tap threshold:
          If it is a double-tap, it cancels any single-tap actions and performs a double-tap action (PerformDoubleTapAction).
          If it is not a double-tap, it schedules the single-tap action (PerformSingleTapAction) to run after the double-tap threshold.
          If the placement indicator is enabled and the game is paused, it updates the placement pose and placement indicator.
      
        8.PerformSingleTapAction()
          Handles single-tap actions.
          If the user tapped on a valid plane, the script spawns an object at the placement pose, adjusts its rotation (flipping it by 180 degrees), and adds the spawned object to the spawnedObjects list.
          The spawn count is updated and saved to PlayerPrefs.
          If the detected plane is vertical, the script calls AdjustSpawnObjectToVerticalPlane to adjust the scale of the object to fit within the vertical plane's size.
      
       9.PerformDoubleTapAction()
        Handles double-tap actions.
        A ray is cast from the camera’s view to detect if any object was tapped.
        If the object hit by the ray has the BodyPartDetection component, it plays a related audio clip and updates the displayed body part name in the UI.
      
       10.AdjustSpawnObjectToVerticalPlane(GameObject spawnObject, ARPlane plane)
          Adjusts the scale of the spawned object to fit within the detected vertical plane.
          It calculates the scale factor by comparing the object's bounds and the plane's size, then applies the scale factor to the spawned object.
      
       11.ToggleScale()
          This function is called when the menuButton is clicked.
          If the UI is scaled up, it calls ScaleDown to scale it down. If it's scaled down, it calls ScaleUp to scale it back up.

  2.BodyPartDetection : script is used to manage the detection of different body parts in the game, display the name of the body part using TMP_Text, and play a specific sound clip associated with the body part when it's detected.
    1. Enum BodyPart
        The BodyPart enum defines the different body parts that the script will handle. These are:
        Right Hand
        Left Hand
        Head
        Body
        Right Leg
        Left Leg
        
   2.Variables
      assignedBodyPart: This variable stores the body part that this particular instance of the BodyPartDetection script is assigned to (e.g., Right Hand, Head).
      bodyPartName: A TMP_Text reference that will be used to display the name of the body part. This could be a TextMeshPro UI element.
      audioClip: An AudioClip that will be played when the body part is detected.
      player: A Transform reference to the player’s object, used to make the text always face the player.
      
   3.Start() Method
      In this method, the script tries to find the player object in the scene by looking for an object tagged with "Player." If it finds such an object, it assigns the player’s Transform to the player variable.
      
   4.Update() Method
      The Update() method checks if bodyPartName and player are assigned.
      If they are, it continuously adjusts the rotation of bodyPartName (the TMP_Text object displaying the body part name) to face the player.
      The LookAt(player) method makes the text face the player's position, and then the text is rotated by 180 degrees on the Y-axis to ensure it faces the player correctly.

Demo Video Explanation :
  Video 1: Application Workflow and Model Interaction
    This video explains the application's startup process and the steps involved in selecting and placing a model.
    Model Selection & Plane Detection:
      After choosing a model, the application takes some time to detect plane surfaces (both horizontal and vertical).
      Once a horizontal plane is detected, the placement indicator turns green.
      When the user taps on the scene, the model spawns in a walking animation.
      Similarly, when vertical planes are detected, the placement indicator turns blue.
      Upon tapping, the model appears in a running animation.
    Model Scaling Based on Plane Size:
      The size of the spawned model is determined by the detected vertical plane's dimensions.
      Since multiple vertical planes are detected, subsequent models have varying sizes.
    Interaction with Models:
      The user can interact with different body parts of the models using a double tap gesture.
      This triggers an audio feedback, announcing the name of the tapped body part.
    Model Count Display:
      The count of spawned models is dynamically updated in the UI.

  Video 2: Reset Functionality & Persistence of Model Count
      This video showcases how the application manages spawned models and persists data after reopening.
    Reset Button Functionality:
      Clicking the Reset button removes all spawned models.
      Once cleared, detected planes are visible again, and the placement indicator resumes changing colors based on detected surfaces.
      The model count resets to 0.
    Persistence of Model Count After Restart:
      After clearing, new models can be spawned again.
      When the application is closed and reopened, the count of previously spawned models is retained.
      If the models are cleared before closing, the count remains 0 upon reopening.



App Functionality and Usage

Notes:
The app currently provides audio feedback for body part interaction due to the lack of characters with separable materials or body parts for visual highlighting.
The plane size is automatically calculated when detecting a vertical plane, ensuring the model fits appropriately.

![UI_Menu](https://github.com/user-attachments/assets/fd6b75fc-b75a-4c80-b177-2d171ca150c2)


  1. Main Screen Overview:
     When the application opens:
       A UI_Menu appears in the center of the scene with a scale-up animation.
       A Menu Button is located in the top-left corner.
       A Count text is displayed at the top-right corner, showing the number of models spawned.

  2. Menu Button Functionality:
     Clicking the Menu Button toggles the UI_Menu:
       Scales up to open.
       Scales down to close.

  3. Choosing a Model:
     Inside the UI_Menu, select one of the available models (e.g., Model_A or Model_B).
     Once a model is selected, the UI_Menu scales down automatically.
     To choose another model, reopen the menu using the Menu Button.

   4.Plane Detection and Placement Indicator:
      Plane Detection begins once the UI_Menu is closed, with visual feedback:
        Green Indicator: Horizontal plane detected (e.g., floor or flat surfaces).
        
  ![Floor_detected_rectile](https://github.com/user-attachments/assets/4989d7a5-0ad1-411a-9b84-dc06b54379d3) 
       Blue Indicator: Vertical plane detected (e.g., walls).
       
  ![Wall_detected_rectile](https://github.com/user-attachments/assets/b25945ef-1d27-4a24-981c-0b5cabe007c0)
  
   The Placement Indicator appears based on the detected plane type.

  5. Spawning Models:
     Single tap on the screen to spawn a model:
       If a Horizontal Plane is detected:
         The model is instantiated with a walking animation.
     
     ![Models_Walking](https://github.com/user-attachments/assets/e9768865-fb31-4b54-9a5a-b47fddc5a9fa)

       If a Vertical Plane is detected:
         The model is instantiated with a running animation.
         The model is resized to fit the scale of the vertical plane.
         You can move around in the real-world environment and spawn multiple models.
     
     ![Same_Model_running_Size fit](https://github.com/user-attachments/assets/cc500feb-2fa5-4830-83db-a0eeb022ee32)


   7. Body Part Interaction:
        Audio Feedback for body part interaction:
          Double tap on a model's body part to hear the name of the body part.
          The name of the body part is displayed above the model.

   8. Reset Functionality:
        Open the UI_Menu and click on the Reset Button to:
          Clear all spawned models.
          Reset the Count to 0.
          If you reopen the application, the Count will reflect the previously spawned models.






