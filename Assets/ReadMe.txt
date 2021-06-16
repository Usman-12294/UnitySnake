
Snake game - The Game Boss - Hyper casual game prototype

Setting up game:

1. Open and run gameplay scene
2. You can also choose to play your desired level by mentioning desired level number in GameManager script in Gameplay scene
3. If you don't want to play your desired level and want to go sequentially from level 1 onwards then leave a 0 in desired level field in GameManager script
3.1. In case step 3 doesn't work, you can clear your preferences by selecting "Edit" Menu and then "Clear All PlayerPrefs". A Confirmation box will appear. Press "Yes"
4. For now, we have two playable levels so stick to desired level method and change it to 1 or 2 accordingly
5. GameManager has the collectibles field where you can change the name and image of the collectibles
6. SumUp field has prefabs which shows up whenever snake gets collectable and it works on object pooling mechanism
7. Collectibles count is a text holder which holds the information of collectibles consumed by snake
8. Start level and end level are the level number displayers at the start and end of the level respectively
9. Win panel and fail panel shows up on level complete and level fail respectively
10. Controls field has those elements which are to be turned on/off when game starts or fails

How to play:

1. Tap to start game
2. Click/Touch anywhere on screen and use joystick to move the snake
3. Follow the instructions and reach the finishpoint to complete the level

