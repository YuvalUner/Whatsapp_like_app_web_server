import person from "../Resources/person-circle.svg"
import Tokens from "./Tokens";


/**
 * Temporary class for handling all the "database" related actions for already registered users.
 */
class RegisteredUser {

    /***
     * Checks if a user already exists by their email.
     * @param email
     * @returns {Promise<null|boolean>}
     */
    static async doesUserExistByEmail(email){
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/doesUserExistByEmail/"
            + email, {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            }
        })
        if (res.ok){
            let text = await res.text();
            return text === "true";
        }
        return null;
    }

    /***
     * Checks if a user already exists by their phone number.
     * @param phone
     * @returns {Promise<null|boolean>}
     */
    static async doesUserExistByPhone(phone){
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/doesUserExistByPhone/"
            + phone, {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            }
        })
        if (res.ok){
            let text = await res.text();
            return text === "true";
        }
        return null;
    }

    /**
     * Returns whether a user with that username already exists or not.
     * @param username user to check existence for.
     * @returns {null | string} null if not, the user if yes.
     */
    static async doesUserExistByUsername(username) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/doesUserExistByUsername/"
            + username, {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            }
        })
        if (res.ok){
            let text = await res.text();
            return text === "true";
        }
        return null;
    }

    /**
     * Updates a user's description.
     * @param username The username to update for.
     * @param newDesc The user's new description.
     */
    static async changeDescription(username, newDesc) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/editDescription/"
            + newDesc, {
            method: "PUT",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            }
        })
        return res.ok;
    }

    /**
     *
     * @param username the active user.
     * @param convoWith the user the active user is conversing with.
     * @returns an array containing the user's conversation, sorted chronologically.
     */
    static async getConvo(username, convoWith) {
        let res = await fetch("https://localhost:7031/api/Contacts/" + convoWith + "/messages", {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            }
        })
        if (res.ok){
            return await res.json();
        }
        return [];
    }

    /**
     * Adds a contact to a user, and adds the user as the contact's contact.
     * @param username the active user.
     * @param contact the contact t oadd.
     */
    static async addContactByUsername(username, contact) {
        let res = await fetch("https://localhost:7031/api/Contacts/?local=true", {
            method: "POST",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: contact,
                server: window.location.origin
            })
        })
        return res.ok;
    }

    /***
     * Adds a contact by their email.
     * @param username
     * @param email
     * @returns {Promise<boolean>}
     */
    static async addContactByEmail(username, email) {
        let res = await fetch("https://localhost:7031/api/Contacts/byEmail?local=true", {
            method: "POST",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: email,
                server: window.location.origin
            })
        })
        return res.ok;
    }

    /***
     * Adds a contact by their phone number.
     * @param username
     * @param phone
     * @returns {Promise<boolean>}
     */
    static async addContactByPhone(username, phone) {
        let res = await fetch("https://localhost:7031/api/Contacts/byPhone?local=true", {
            method: "POST",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: phone,
                server: window.location.origin
            })
        })
        return res.ok;
    }

    /**
     * Updates a user's profile picture.
     * Currently disabled.
     * @param username the user to update.
     * @param url a url to their new picture.
     */
    static updateProfileImg(username, url) {
        return false;
    }

    /**
     * Gets the user's profile picture. Currently, basically disabled.
     * @param username the user to get the profile picture for.
     * @returns a link to their profile picture. If the user has no custom picture, the default one is returned.
     */
    static getImage(username) {
        return person;
    }

    /**
     * Adds a message to the conversation between 2 users.
     * @param username the active user.
     * @param convoWith the user the active user is conversing with.
     * @param message the message to add.
     */
    static async addMessageToConvo(username, convoWith, message) {
        let res = await fetch("https://localhost:7031/api/Contacts/" + convoWith + "/messages" , {
            method: "POST",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                content: message.content
            })
        });
        return res.ok;
    }

    /**
     * Returns the user's nickname discriminator.
     * @param username the user to get their nickname discriminator.
     * @returns {string|*} nickname discriminator.
     */
    static async getNickNum(username) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/GetNickNum", {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken
            },
        })
        if (res.ok){
            return await res.text();
        }
        return null;
    }

    /**
     * Updates a user's nickname to a new one chosen by them.
     * @param username
     * @param newNickname
     */
    static async updateNickname(username, newNickname) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/editNickName/"
            + newNickname, {
            method: "PUT",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            }
        })
        return res.ok;
    }

    /**
     * Gets a user's nickname.
     * @param username
     * @returns {string|*}
     */
    static async getNickname(username) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/getNickName/" + username, {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken
            },
        })
        if (res.ok){
            return await res.text();
        }
        return null;
    }

    /**
     * Gets a user's description.
     * @param username
     * @returns {string|*}
     */
    static async getDescription(username) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/getDescription/" + username, {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken
            },
        })
        if (res.ok){
            return await res.text();
        }
        return null;
    }

    /**
     * Returns whether a user's code can be verified.
     * @param username the user.
     * @param input the input from the text box.
     * @returns {boolean}
     */
    static async canVerify(username, input) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/verifyCode/"
            + username + "?verificationCode=" + input, {
            method: "GET"
        })
        if (res.ok){
            Tokens.accessToken = await res.text();
            return true;
        }
        return false;
    }

    /**
     * Returns the user's contacts in an array.
     * @returns {*[]} the user's contacts in an array.
     */
    static async getContacts(username) {

        let res = await fetch("https://localhost:7031/api/Contacts", {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken
            },
        })
        if (res.ok){
            return await res.json();
        }
        return [];
    }

    /**
     * Returns whether a contact is already in the user's contact list.
     * @param username
     * @param contact
     * @returns {T} returns the contact if yes, null otherwise.
     */
    static async isAlreadyContactByUsername(username, contact) {
        let res = await fetch("https://localhost:7031/api/Contacts/alreadyContact/" + contact, {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            }
        })
        if (res.ok){
            let text = await res.text();
            if (text === "true"){
                return true;
            }
            return false;
        }
        return true;
    }

    static async isAlreadyContactByEmail(username, email){
        let res = await fetch("https://localhost:7031/api/Contacts/byEmail/" + email, {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            }
        })
        if (res.ok){
            let text = await res.text();
            if (text === "true"){
                return true;
            }
            return false;
        }
        return true;
    }

    static async isAlreadyContactByPhone(username, phone){
        let res = await fetch("https://localhost:7031/api/Contacts/byPhone/" + phone, {
            method: "GET",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            }
        })
        if (res.ok){
            let text = await res.text();
            if (text === "true"){
                return true;
            }
            return false;
        }
        return true;
    }

    /**
     * Updates a user's password.
     * @param username
     * @param newPassword
     */
    static async updatePassword(username, newPassword) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/editPassword/", {
            method: "PUT",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                password: newPassword
            })
        });
        if (res.ok){
            let text = await res.text();
            if (text === "true"){
                return true;
            }
            return false;
        }
        return true;
    }

    /**
     * Generates the code needed for resetting password.
     * @param username
     */
    static async generateVerCode(username) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/renewVerificationCode/"
            + username,{
            method: "PUT"
        });
        return res.ok;
    }

    /**
     * Verifies if the user's secret question and answer match.
     * @param username
     * @param questionNum the question's number.
     * @param answer
     * @returns {any|boolean} true if yes, false otherwise.
     */
    static async VerifySecretQuestion(username, questionNum, answer) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/secretQuestion/"
            + username + "?question=" + questionNum + "&answer=" + answer,{
            method: "GET",
            headers: {
                'Content-Type': 'application/json',
            }
        });
        if (res.ok){
            let text = await res.text();
            if (text === "true"){
                return true;
            }
            return false;
        }
        return false;
    }

    /***
     * If the user explicitly logged out, have the server erase their refresh token / access cookie.
     * @returns {Promise<boolean>}
     */
    static async logOut(){
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/logOut",{
            method: "PUT",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            }
        });
        return res.ok;
    }

    /**
     * Verifies if a user and password pair match. If yes, the user can be logged in.
     * @param username
     * @param password
     * @param rememberMe
     * @returns {any|boolean} true if yes, false otherwise.
     */
    static async DoUserAndPasswordMatch(username, password) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers",{
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                username: username,
                password: password,
            })
        });
        if (res.ok){
            let tokens = await res.json();
            Tokens.accessToken = tokens.accessToken;
            Tokens.refreshToken = tokens.refreshToken;
            return true;
        }
        return false;
    }

    /**
     * Same as previous one, but this time the username is the email.
     * Frankly, probably a redundant method, but it was used in the beginning of development, and I can't be bothered
     * to refactor this.
     * @param email
     * @param password
     * @returns {any|boolean}
     */
    static async doEmailAndPasswordMatch(email, password) {
        let res = await fetch("https://localhost:7031/api/RegisteredUsers/emailLogIn",{
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                email: email,
                password: password,
            })
        });
        if (res.ok){
            let tokens = await res.json();
            Tokens.accessToken = tokens.accessToken;
            Tokens.refreshToken = tokens.refreshToken;
            return true;
        }
        return false;
    }

}

export default RegisteredUser;