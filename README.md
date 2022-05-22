AdvancedProgrammingProjectsServer
### `Submitters`:

Yuval Uner, ID: ***REMOVED***, Github: OddPanda.\
Nadav Elgrabli, ID: ***REMOVED***, Github: ZycleXx.

To run the project, use "npm start".\
The root file is index.js.

### `dependencies frontend`
1. React
2. React-Bootstrap
3. React-router / React-router-dom

### `dependencies backend`
1.JWT Bearer
2. SignalR
3. MariaDB
4. ASP.Net CORE
5. Identity Framework Core
6. Entity Framework Core

### `testing`
To test our project, please create users by the following steps:
1. Go to PendingUsers [POST] in Swagger UI and enter the information of your user. Make sure to enter a real and valid email.
2. go check the email you entered and copy the code that was sent to that email.
3. Now switch to PendingUsers[GET] and enter the username and the code that you copied.
4. Copy the printed token on screen, swipe up to the "Authorize" button on the top right of the Swagger UI screen, and type in "Bearer " and paste the copied token.
5. Switch to RegisteredUsers/signup and press execute.

# Controllers

### `Contacts Controller`
This is the controller in charge of managing everything related to a user's contacts.
This includes adding and deleting contacts from users and adding/deleting messages to conversations between users.

### `Invitations Controller`
A controller for managing invitiations from other servers. 

### `PendingUser Controller`
A controller in charge of managing users that are pending verification.
This includes signing up users as pending users, sending the user a verification email and verifiying the verification code.
Checking if the pending user exists via username\email\phone.

### `Refresh Token Controller`
A controller for managing the renewal of auth tokens.
Provides the ability to renew a user's access and refresh tokens.

### `Registered Users Controller`
