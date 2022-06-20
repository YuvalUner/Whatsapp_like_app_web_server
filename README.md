## `Note about updates:`
We pushed to master branch from the 10/6/2022 (the pushes were to this branch and not a seperate, unrelated branch by mistake).
These updates had nothing to do with the 2nd assignment, and were updates regarding the 3rd assignment, that is, 
mobile integration.\
These changes were mostly just firebase integration, as well as changes for the additional options in the mobile
options screen - only things that were not implemented or did not exist in the web version.
These affect nothing that regards the API defined in assignment 2 or the web client.
For the version before these changes, refer to the last update made regarding assignment 2, done on 22/05/2022, commit hash 6afbd253e326418973d2b9a01b94a4b11231f9a8

# `Submitters`:
Yuval Uner, Github: OddPanda.\
Nadav Elgrabli, Github: ZycleXx.\
As the repository was made public according to the instructions sent on the 19/06/2020,
our IDs were removed from here and only kept in the submitted txt file.

# `Dependencies`:
### `Frontend`
1. React
2. React-Bootstrap
3. React-router / React-router-dom
4. @microsoft/signalr

### `Backend`
1. JWT Bearer
2. SignalR
3. ASP.Net Core
4. Pomelo Entity framework

In addition, an installation of MariaDB is needed.

# `Configuration and running`

To set up the project via visual studio, open the package manager console, set the active
project in it to "Data", then run update-database.

The projects / servers that need to be run are:
1. AdvancedProgrammingProjectsServer
2. AdvancedProjectWebApi

The first is our server that serves out our views and handles the reviews part\
of the assignment.

The second is the WebAPI we created for running the React app from part 1.

All settings regarding them are stored in their respective appsettings.json\
To change the connection string to one matching your database, it is under the\
ConnectionStrings json object in those files.\
In addition, it may be necessary to change the AllowedOrigins to whatever
ports your machine uses when running the servers.

# `Reviews part`

The reviews part holds up to the all the requirements.\
In addition, it has a link to our React app at its bottom.

# `WebAPI and React app`
## `Testing`
To create users not via the front end, follow these steps:
1. Go to PendingUsers using [POST] and enter the information of your user. Make sure to enter a real and valid email.
2. Check the email you entered and copy the code that was sent to that email.
3. Switch to PendingUsers using [GET] and enter the username and the code that you copied.
4. Copy the access token you received.
5. Go to RegisteredUsers/signUp using [POST], add "Authorization: Bearer" + the token, then execute.

However, we recommend doing this via the frontend React app.

# `Controllers`

### `Contacts Controller`
This is the controller in charge of managing everything related to a user's contacts.
This includes adding and deleting contacts from users and adding/deleting messages to conversations between users.

### `Invitations Controller`
A controller for managing invitations from other servers.

### `PendingUser Controller`
A controller in charge of managing users that are pending verification.
This controller includes functionalities such as:
1. Signing up users as pending users
2. Creating and sending the user a verification email and verifiying the verification code.
3. Checking if the pending user exists via username\email\phone.

### `Refresh Token Controller`
A controller for managing the renewal of auth tokens.
Provides the ability to renew a user's access and refresh tokens.

### `Registered Users Controller`
Controller in charge of managing already registered users.
This controller includes functionalities such as:
1. Allowing the user to log in once the username/email and password match.
2. Log the user out of the app.
3. Adds the user to the database once the signup process is over.
4. Getters for the user's description, nickname, nicknum and secret question.
5. Check existence of user by email/username/phone.
6. Provides the ability to change the user's password/nickname/description.
7. Renews verification code of specific user.

### `Transfer Controller`
A controller in charge of adding messages from other servers.

# `Security`
### `Information security`
Our user's passwords are hashed via the (outdated but comes by default with ASP.net core)
Pbkdf2 hashing algorithm, and salted with 24 character random string.

In addition, our refresh tokens (explanation in following section) are hashed using SHA256.

### `Access control`
We use JWT to give our users access tokens on log-in. These tokens last 5 minutes.\
All methods that relate with giving or changing user specific information require authentication
using this access token.

To renew the access token, the user is also given refresh tokens - 1 use token which is handed
to the server to renew both the access token and refresh token.\
These tokens are 256 characters long, randomly generated, and last a month.\
They are also saved in the browser in the event the user chose "remember me", and are used
to allow the user to log in automatically via cookies.

### `Email verification`
Upon signing up or asking to change password, the user will receive an email to their email address
with a one time code.
Only upon verifying that 1 time code will they be given an access token for performing the action they wish to perform.

In the case of sign-up, the user will only be signed up after verifying their email.\
In the case of password reset, the user will not be allowed to reset their password before verifying their code._

