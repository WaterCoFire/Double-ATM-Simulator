# Double-ATM-Simulator
Simulation of two ATM operations implemented in Unity. For demonstrating data consistency. Part of a Computer Systems course assignment.
## Introduction

Welcome!

The origin of this project was an assignment in a Computer Systems course module designed to simulate data races as well as avoidance measures (so there's like two separate versions). However, only the data-consistent version is provided in this GitHub project (the data race version is not uploaded because it doesn't make much sense if it's no longer an assignment).

This project allows you to set up two 2D ATM UIs in Unity and implement features such as balance enquiries, withdrawals, inter-account transfers, checking transaction history, and more. Depending on the assignment requirements, most of the transactions (e.g. balance withdraw) implement a short artificial delay at the code level to facilitate data inconsistency in the data race version, as the implementation is done by quickly and simultaneously operating the left and right ATMs logged into the same account within a short period of time. This delay feature is still present even though this GitHub project is the data consistent version.

Note that this GitHub project is not a complete Unity project; Only necessary codes and resources are provided. You should create a Unity project and design most of the UI elements by yourself.

This README provides you with important information to personalize your own project, including how to design and manage the various Unity elements, what do they do, and how to properly assign them to C# scripts provided in this project.

## Get Started

Like I said before, you should design most of the UIs by yourself, e.g. the style buttons, input fields, information prompts. You should also design an ATM UI picture. The picture that I personally used for the assignment is provided in Resources/Images so feel free to use if you don't want to use your own. You should place it in Assets/Images in your Unity project directory if you want to use it.

The picture provided:

![atmscreen](https://github.com/user-attachments/assets/6d2597db-efd6-48e4-9f3a-cde2c0e2f91a)

And how the UI looks like with this picture:

![image](https://github.com/user-attachments/assets/f7e68a90-1421-4e87-b38b-fd3095c05aea)

You should also prepare some sound effects that will be used by the ATM:

![image](https://github.com/user-attachments/assets/5450aa13-a177-405f-b0f8-1ff21c7ca49a)

- ButtonPress.MP3: played when a button is pressed down
- CashOut.MP3: played when a withdrawal operation is done
- EnterPrompt.MP3: played at the login page, remind the user to enter account number and PIN
- Failure.MP3: played when an error occurs or an invalid operation is made
- Quitting.MP3: played when the user logs out
- Success.MP3: played when a successful operation is made

The sound effects that I personally used is also provided in Resources/Sounds so feel free to use. Please place it in Assets/Resources in your Unity project directory if you want to use it.

## Project Setup

Once opened a new Unity project, create a **UI prefab** and a **data manager** in your canvas. Please also create a virtual element for managing audio (make sure it has an **Audio Source** attribute). Make your project path look like this:

![image](https://github.com/user-attachments/assets/3d70b1d7-5c2b-49b3-887a-18261b325abf)

Then create all the elements inside the **UI prefab** according to this:

![image](https://github.com/user-attachments/assets/cc12b641-0ae6-43fc-8f4f-a327ead0b011)

Explanation:

- ATM Terminal: A virtual element to manage script behaviour.
- ATM screen: Displays the ATM UI (so it should have **an Image attribute** and set to your UI picture).
- Login Page: Login UI. The first UI that the user interacts with after the project starts.
- Record Page: Transaction Record Enquiry UI.
- Main Page: The UI displayed after login. Includes all the buttons for other operations, e.g. balance withdraw, check balance.
- Withdraw Page: Balance withdrawal UI.
- Transfer Page: Balance transfer UI. Transfer means sending some balance to another account.
- Occupied Page: A popup warning, indicating that another transaction is being performed on the other ATM with the same account logged in. This exists precisely because, as I said previously, this is the data-consistent version, so we don't want data races to cause problems.
- Function Button 0-7: All the physical buttons of the ATM, four on each side. Make their No. from top to bottom: Left 0-3 + Right 4-7.
- Number Keyboard Effects:  A feature to enhance the simulation. Contains 10 small translucent squares representing the 0-9 buttons on the numeric keypad. When a number is detected to be pressed, the corresponding small square is automatically displayed, simulating the corresponding number being pressed on the ATM keypad.

Now let's go to the login page. Make it look like this:

![image](https://github.com/user-attachments/assets/f16cba47-e5aa-4ab1-a736-1037bae16294)

And for reference, I will show the effect in my personal project: (I will do the same thing for every page UI so don't worry)

![image](https://github.com/user-attachments/assets/7c9afced-18b6-4268-a34f-0708de6a5d68)

For the record page:

![image](https://github.com/user-attachments/assets/954634a6-d0d5-495b-b716-9bf978a0c72b)

Note: the prefab here represents every single transaction record, stored inside the scroll view.

The effect in my project:

![image](https://github.com/user-attachments/assets/f662b023-fc72-4aed-8cc9-1e3682d4e83a)

For the main page:

![image](https://github.com/user-attachments/assets/e234d0c0-7eb5-48a9-813c-b50fb33bed89)

The effect in my project:

![image](https://github.com/user-attachments/assets/c51a43d0-6ad0-4190-8d25-91135ea74eed)

Note that in this main page, there are **two subpages** - **balance check page** and **quitting page**. They are subpages because their still exist within the main page.

Make them look like this:

![image](https://github.com/user-attachments/assets/f9c6f311-78e5-41ff-bee2-5418399bad31)

The effect in my project:

![image](https://github.com/user-attachments/assets/9818d29f-9f2f-4bf8-bab3-521547662804)

![image](https://github.com/user-attachments/assets/24b4a47f-8931-4af5-8897-1f30a56f5f0e)

Now, for the withdraw page:

![image](https://github.com/user-attachments/assets/a73e3352-a09f-4534-b5db-c14369b9430b)

![image](https://github.com/user-attachments/assets/40023fc1-e27f-4088-a44f-c6eabd488e81)

Note that there are three prompt-related elements: **warning prompt**, **processing prompt** and **success prompt**. You can simply place them in the same place (as at most one will be shown at the same time) where there's space and make sure they are always visible when activated. In my case, I placed it a little bit below the balance display area (below the "Your Balance: 240" in this picture.)

Now turn to the transfer page:

![image](https://github.com/user-attachments/assets/eff5a434-8023-4697-8ff3-1ec0b0835201)

You can see that there are actually two components: the page for **selecting the account** and the page for **selecting the amount**. When entering the transfer page from the main page, **the account selection page will always be shown first** (as you have to select the account you wanna transfer money to before you choose how much money you wanna transfer to that account).

Firstly, for the account selection page:

![image](https://github.com/user-attachments/assets/97285d49-3174-49c0-bfad-dd58f4b3c1f4)

And the effect in my project:

![image](https://github.com/user-attachments/assets/5a0b2345-c106-46e6-b435-198e4a8d5876)

Note: the big "Transfer" title in this picture is exactly the "Transfer Title" element designed inside the transfer page. You can see that it does not belong to either component, because in both pages, this title will always be shown.

Secondly, for the amount selection page:

![image](https://github.com/user-attachments/assets/343401f0-7832-4661-89b0-b8d61ca5985c)

You can see that it's actually pretty similar to the withdraw page, except that there is another text element designed for displaying the receiver (i.e. the person that you're gonna transfer the money to).

And the effect in my project:

![image](https://github.com/user-attachments/assets/80ac681a-9be1-4382-a226-9650c3951b3d)

And for the occupied page:

![image](https://github.com/user-attachments/assets/ac49e170-6a73-4676-9740-c32f68ff13e6)

The reason why it is separate and does not belong to any other page is that it could be used in several pages, e.g. withdrawal, transfer.

The effect in my project:

![image](https://github.com/user-attachments/assets/adb3c260-3d5a-4ac1-a8f2-43c5d3651493)

Now let's turn to the number keyboard effects.

![image](https://github.com/user-attachments/assets/35268479-1b9e-43be-993b-55005c7bad3a)

Simply design ten small translucent grey squares and place them in the corresponding positions of key 0-9 on the numeric keypad on your ATM UI picture.

For example, for the number 0, the effect would look like this:

![image](https://github.com/user-attachments/assets/612ed6e8-81ea-4af1-9ea3-552605d8b323)

That concludes our description of the project structure and the functionality of all the elements.

After you have designed all the UI elements, it's time to assign all the C# scripts.

First go to the **data manager**, assign the script **AtmDataManager** to it, and assign the **UI prefab** we have designed to the variable AtmUIPrefab:

![image](https://github.com/user-attachments/assets/09b89a0f-96a3-492c-8d40-ec70a3a61457)

Now turn to the UI prefab.

For the ATM terminal, assign the scripts and variables like this:

![image](https://github.com/user-attachments/assets/ecf4c65c-28ff-4790-bc5b-0f7e97459b29)

For the login page:

![image](https://github.com/user-attachments/assets/a7410208-eb4d-47dd-a06c-2a180ca430dc)

For the record page: ("内容" here means content)

![image](https://github.com/user-attachments/assets/ebf35bac-3f07-48b7-a237-92e6b2ed1536)

For the main page:

![image](https://github.com/user-attachments/assets/5ccb41a3-c326-41f8-a091-3cd478dc5cd7)

For the withdraw page:

![image](https://github.com/user-attachments/assets/846fbf83-1f30-46d1-bc62-13d075ccfb0a)

For the transfer page:

![image](https://github.com/user-attachments/assets/f8bee3c4-013f-4b87-bea9-ac0f3787d8fd)

Finally, go the the audio manager, assign the script **AudioManager** and assign the **ButtonPress** sound effect to it:

![image](https://github.com/user-attachments/assets/490ac078-0ff2-43e4-97eb-2e3b6f517d1a)

And then we have finished the setup of the entire project!

## Account Operation

The data about accounts is set in AtmDataManager class:

![image](https://github.com/user-attachments/assets/e71c9d4b-905e-4e9d-a09d-763c49d4d954)

By default, there are three accounts:

- Account 0: account number 111111, PIN 1111, remaining balance: 300
- Account 1: account number 222222, PIN 2222, remaining balance: 750
- Account 2: account number 333333, PIN 3333, remaining balance: 3000

Feel free to modify them to suit your needs.

## End

Have fun!

If encountering any problems, please let me know. I'm glad to help.

