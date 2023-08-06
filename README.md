
# VenminderBowlingGameExercise
Venminder full stack developer is responsible for understanding the business requirements, working with product and design team to gather acceptance criteria, and work on solving problems. Part of their job is to collaborate with other developers, beyond their team, to bring best solutions for the company.

# How to run the program
## Setup
The project is written in ASP.NET core 6, therefore, it can be ran on Windows or Linux. However, a ASP.NET core 6.0 sdk is required on the operating system. Visual Studio 2022 is strongly recommonded to run this program

## UI component
- Roll score is manually typed in text box, and user need to click button Delivery in order to post the score to the server
- The server returns where the next Roll will be by return next frame number and roll number(so the UI can be status agnostic)
- Once all the rolls are completed, Delivery Button will be disabled and the text will change to Game is Finished. Then the disabled button Get Final Score will be enabled for query the final score from the server.
- If user want to reset the game, then hit Reset the Game button to start over.


# Software design key considerations
## Data Type
Singly Linked Lists can represent bowling score frame appropriately:
- Because the frame score can be affected by the next two frames, therefore a relationship between frames is desired. Everytime when there is a new roll score gets push in. the program will iterate through the entire list to ensure every frame is updated.
- There are some helpful booleans in the data type that represent a frame, which are IsStike and IsSpare. Store those data in each node makes the code easy to understand, because we don't need to create extra logic to figure out if that frame is IsStike or IsSpare.

## software design pattern: 
MVC Pattern:
- BowlingController exposes /bowling endpoints, RESTful api pattern is applied here:
    - Post: Post the newly scored roll score to the server
    - Get: Get the final score when game is over
    - Delete: Delete the "db"(represented by the singly linked list)
- Client App is the view, which is implemented by Angular.js which is similar to Auralia 
- Model: Created the Singly Linked that represent frame score in Model, defined the operations for Linked List like insert a new node to the tail, as well as update node. Each frame's score is calculated in the frame node as well for easy to read.

Repository Pattern:
- Although this is a relative small project, the author still wants the program to be flexible(by lose couping) and extentable in the future
- BowlingScoreRepository contains all the logic that process and manipulate the data, including ProcessFrame, GetTotalScore, and ResetGame
- Two special cases in blowing score game, first frame and last frame. I have created two private function to handle those two scenarios, so the program follows the single responsibility principle
- Repository is a singleton and is injected to controller to follow dependency inversion principle

# Testing strategy
## Unit tests
19 unit tests are implemented for Repository and Model classes, because these two handles most of logics. Made sure edge cases are covered
- Three strikes in a row
- Three spares in a row
- spares after strike 
- strike after spare 
- All stikes 
- All spares 

## Manual testing
testing against the UI, make sure it doesn't break
