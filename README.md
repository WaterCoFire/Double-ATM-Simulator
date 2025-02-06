# Double-ATM-Simulator
Simulation of two ATM operations implemented in Unity. Part of a Computer Systems course assignment.
## Introduction

Welcome!

The origin of this project was an assignment in a Computer Systems course module designed to simulate data races as well as avoidance measures (so there's like two separate versions). However, only the data-consistent version is provided in this GitHub project (the data race version is not uploaded because it doesn't make much sense if it's no longer an assignment).

This project allows you to set up two 2D ATM UIs in Unity and implement features such as balance enquiries, withdrawals, inter-account transfers, checking transaction history, and more. Depending on the assignment requirements, most of the transactions (e.g. balance withdraw) implement a short artificial delay at the code level to facilitate data inconsistency in the data race version, as the implementation is done by quickly and simultaneously operating the left and right ATMs logged into the same account within a short period of time. This delay feature is still present even though this GitHub project is the data consistent version.

Note that this GitHub project is not a complete Unity project; Only necessary codes and resources are provided. You should create a Unity project and design most of the UI elements by yourself.

This README provides you with important information to personalize your own project, including how to design and manage the various Unity elements, what do they do, and how to properly assign them to C# scripts provided in this project.

## Get Started

Like I said before, you should design most of the UIs by yourself, e.g. the style buttons, input fields, information prompts. You should also design an ATM UI picture. The picture that I personally used for the assignment is provided in Resources/Images so feel free to use if you don't want to use your own. You should place it in Assets/Images in your Unity project directory if you want to use it.

<img src="E:\Project\UnityProject\ATM_simulator_consistency\Assets\Images\atmscreen.jpg" alt="atmscreen"/>

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\753c12d4fb18468eb4bf7c0ff9d655e.png" alt="753c12d4fb18468eb4bf7c0ff9d655e"/>

You should also prepare some sound effects that will be used by the ATM:

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\1faad84de2a0f63cff554d783e8cb4f.png" alt="1faad84de2a0f63cff554d783e8cb4f"/>

- ButtonPress.MP3: played when a button is pressed down
- CashOut.MP3: played when a withdrawal operation is done
- EnterPrompt.MP3: played at the login page, remind the user to enter account number and PIN
- Failure.MP3: played when an error occurs or an invalid operation is made
- Quitting.MP3: played when the user logs out
- Success.MP3: played when a successful operation is made

The sound effects that I personally used is also provided in Resources/Sounds so feel free to use. Please place it in Assets/Resources in your Unity project directory if you want to use it.

## Project Setup

Once opened a new Unity project, create a **UI prefab** and a **data manager** in your canvas. Please also create a virtual element for managing audio (make sure it has an **Audio Source** attribute). Make your project path look like this:

![a7c9364d54aa6dac1604e1a1898a435](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\a7c9364d54aa6dac1604e1a1898a435.png)

Then create all the elements inside the **UI prefab** according to this:

![cd7747efec2074d0a57e177bc89f769](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\cd7747efec2074d0a57e177bc89f769.png)

Explanation:

- ATM Terminal: A virtual element to manage script behaviour.
- ATM screen: Displays the ATM UI (so it should have **an Image attribute** and set to your UI picture).
- Login Page: Login UI. The first UI that the user interacts with after the project starts.
- Record Page: Transaction Record Enquiry UI.
- Main Page: The UI displayed after login. Includes all the buttons for other operations, e.g. balance withdraw, check balance.
- Withdraw Page: Balance withdrawal UI.
- Transfer Page: Balance transfer UI. Transfer means sending some balance to another account.
- Occupied Page: A popup warning, indicating that another transaction is being performed on the other ATM with the same account logged in. This exists precisely because, as I said previously, this is the data-consistent version, so we don't want data races to cause problems.
- Function Button 0-7: All the physical buttons of the ATM, four on each side.
- Number Keyboard Effects:  A feature to enhance the simulation. Contains 10 small translucent squares representing the 0-9 buttons on the numeric keypad. When a number is detected to be pressed, the corresponding small square is automatically displayed, simulating the corresponding number being pressed on the ATM keypad.

Now let's go to the login page. Make it look like this:

![ec199a06fe82e9536fd91137469efca](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\ec199a06fe82e9536fd91137469efca.png)

And for reference, I will show the effect in my personal project: (I will do the same thing for every page UI so don't worry)

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\6ce58efe895a4f38eece42dcfd916d3.png" alt="6ce58efe895a4f38eece42dcfd916d3"/>

For the record page:

![452aaba9520520b8dda0b8ecdfe81d4](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\452aaba9520520b8dda0b8ecdfe81d4.png)

The effect in my project:

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\e1c5137410c74125f1fe7103e7a364b.png" alt="e1c5137410c74125f1fe7103e7a364b"/>

For the main page:

![2cd7c582789816e0b724fc1fdb1d7bb](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\2cd7c582789816e0b724fc1fdb1d7bb.png)

The effect in my project:

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\1ff939a301d497768a6c450e30054ee.png" alt="1ff939a301d497768a6c450e30054ee"/>

Note that in this main page, there are **two subpages** - **balance check page** and **quitting page**. They are subpages because their still exist within the main page.

Make them look like this:

![9c2c562c13b3b5b2dc3fe089eb870ba](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\9c2c562c13b3b5b2dc3fe089eb870ba.png)

The effect in my project:

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\f486f58a8636f40cc756ff8fb748a2c.png" alt="f486f58a8636f40cc756ff8fb748a2c"/>

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\22bc402c0fd2b160c1c31b76378e8b9.png" alt="22bc402c0fd2b160c1c31b76378e8b9"/>

Now, for the withdraw page:

![ec9d78bc2fcdefdcf4a81475451bb62](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\ec9d78bc2fcdefdcf4a81475451bb62.png)

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\b7592a80b38c73b73428565ae44a1a3.png" alt="b7592a80b38c73b73428565ae44a1a3"/>

Note that there are three prompt-related elements: **warning prompt**, **processing prompt** and **success prompt**. You can simply place them in the same place (as at most one will be shown at the same time) where there's space and make sure they are always visible when activated. In my case, I placed it a little bit below the balance display area (below the "Your Balance: 240" in this picture.)

Now turn to the transfer page:

![62928b9a9114504fe0249178c4c6264](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\62928b9a9114504fe0249178c4c6264.png)

You can see that there are actually two components: the page for **selecting the account** and the page for **selecting the amount**. When entering the transfer page from the main page, **the account selection page will always be shown first** (as you have to select the account you wanna transfer money to before you choose how much money you wanna transfer to that account).

Firstly, for the account selection page:

![a959232035da94c7d81be71fc80cd40](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\a959232035da94c7d81be71fc80cd40.png)

And the effect in my project:

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\606918796e1ff825ed7f08072a13873.png" alt="606918796e1ff825ed7f08072a13873"/>

Note: the big "Transfer" title in this picture is exactly the "Transfer Title" element designed inside the transfer page. You can see that it does not belong to either component, because in both pages, this title will always be shown.

Secondly, for the amount selection page:

![e5391514bcd086fa9a0eb66bd8e6ff8](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\e5391514bcd086fa9a0eb66bd8e6ff8.png)

You can see that it's actually pretty similar to the withdraw page, except that there is another text element designed for displaying the receiver (i.e. the person that you're gonna transfer the money to).

And the effect in my project:

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\50ca4dd12aa139f94be5ccc4e5121ee.png" alt="50ca4dd12aa139f94be5ccc4e5121ee"/>

And for the occupied page:

![a210bfca9c4e8955f6cea83e003f859](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\a210bfca9c4e8955f6cea83e003f859.png)

The reason why it is separate and does not belong to any other page is that it could be used in several pages, e.g. withdrawal, transfer.

The effect in my project:

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\cb23745faf891d8fc5acbcfc13d5693.png" alt="cb23745faf891d8fc5acbcfc13d5693"/>

Now let's turn to the number keyboard effects.

![3a837c585b2ad75b96a001235bbaa13](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\3a837c585b2ad75b96a001235bbaa13.png)

Simply design ten small translucent grey squares and place them in the corresponding positions of key 0-9 on the numeric keypad on your ATM UI picture.

For example, for the number 0, the effect would look like this:

![da16819a1680f0a26cee5c2fa28d4fc](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\da16819a1680f0a26cee5c2fa28d4fc.png)

That concludes our description of the project structure and the functionality of all the elements.

After you have designed all the UI elements, it's time to assign all the C# scripts.

First go to the **data manager**, assign the script **AtmDataManager** to it, and assign the **UI prefab** we have designed to the variable AtmUIPrefab:

![319981004a3ce707469034d9c44c115](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\319981004a3ce707469034d9c44c115.png)

Now turn to the UI prefab.

For the ATM terminal, assign the scripts and variables like this:

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\20e96a0f024c33d25bd319a069acf52.png" alt="20e96a0f024c33d25bd319a069acf52"/>

For the login page:

![ed452db9967dfe09306413cd11bf337](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\ed452db9967dfe09306413cd11bf337.png)

For the record page: ("内容" here means content)

![8bd646664d7860c163181ed22a90419](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\8bd646664d7860c163181ed22a90419.png)

For the main page:

![f5dbb65738fa5b406a0ccde5933644f](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\f5dbb65738fa5b406a0ccde5933644f.png)

For the withdraw page:

![dd9b639d82bf69e4cd1579435d1d92a](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\dd9b639d82bf69e4cd1579435d1d92a.png)

For the transfer page:

![cb928e8f3c4d1aaa47620da88418019](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\cb928e8f3c4d1aaa47620da88418019.png)

Finally, go the the audio manager, assign the script **AudioManager** and assign the **ButtonPress** sound effect to it:

![25773d0e6699c29a87025cbe6074ade](E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\25773d0e6699c29a87025cbe6074ade.png)

And then we have finished the setup of the entire project!

## Account Operation

The data about accounts is set in AtmDataManager class:

<img src="E:\WeChat\WeChat Files\wxid_dr8dc1x6tspj31\FileStorage\Temp\09f08d19a8c521a6d58a97944440349.png" alt="09f08d19a8c521a6d58a97944440349"/>

By default, there are three accounts:

- Account 0: account number 111111, PIN 1111, remaining balance: 300
- Account 1: account number 222222, PIN 2222, remaining balance: 750
- Account 2: account number 333333, PIN 3333, remaining balance: 3000

Feel free to modify them to suit your needs.

## End

Have fun!

If encountering any problems, please let me know. I'm glad to help.

