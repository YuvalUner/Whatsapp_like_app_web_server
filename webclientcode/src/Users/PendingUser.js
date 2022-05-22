import RegisteredUser from "./RegisteredUser";
import Tokens from "./Tokens";

class PendingUser {

    constructor(user) {
        this.username = user.username;
        this.password = user.password;
        this.email = user.email;
        this.phone = user.phone;
        this.nickname = user.nickname;
        this.secretQuestion = user.secretQuestion;
    }

    static async checkPendingUserMatch(username, password){
        let res = await fetch("https://localhost:7031/api/PendingUsers/match", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username,
                password: password
            })
        })
        if (res.ok){
            let text =  await res.text();
            if (text === "true"){
                return true;
            }
            return false;
        }
        return false;
    }

    static async checkPendingUserMatchByEmail(email, password){
        let res = await fetch("https://localhost:7031/api/PendingUsers/matchEmail", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                email: email,
                password: password
            })
        })
        if (res.ok){
            let text =  await res.text();
            if (text === "true"){
                return true;
            }
            return false;
        }
        return false;
    }

    /***
     * Signs up a user as a pending user.
     * @param pendingUser
     * @returns {Promise<boolean>}
     */
    static async signUp(pendingUser){
        let response = await fetch("https://localhost:7031/api/PendingUsers", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(pendingUser),
        })
        return response.ok;
    }

    /***
     * Checks if a user exists (either pending or registered) by their username.
     * @param username
     * @returns {Promise<null|boolean>}
     */
    static async doesUserExistByUsername(username) {
        let res = await fetch("https://localhost:7031/api/PendingUsers/doesPendingUserExistByUsername/"
            + username, {
            method: "GET",
        })
        if (res.ok){
            let text = await res.text();
            return text === "true";
        }
        return null;
    }

    /***
     * Checks if a user exists (either pending or registered) by their email.
     * @param email
     * @returns {Promise<null|boolean>}
     */
    static async doesUserExistByEmail(email){
        let res = await fetch("https://localhost:7031/api/PendingUsers/doesPendingUserExistByEmail/"
            + email, {
            method: "GET",
        })
        if (res.ok){
            let text = await res.text();
            return text === "true";
        }
        return null;
    }

    /***
     * Checks if a user exists (either pending or registered) by their phone number.
     * @param phoneNumber
     * @returns {Promise<null|boolean>}
     */
    static async doesUserExistByPhoneNumber(phoneNumber){
        let res = await fetch("https://localhost:7031/api/PendingUsers/doesPendingUserExistByPhone/"
            + phoneNumber, {
            method: "GET",
        })
        if (res.ok){
            let text = await res.text();
            return text === "true";
        }
        return null;
    }

    /***
     * Renews a user's verification code.
     * @param username
     * @returns {Promise<boolean>}
     */
    static async renewCode(username) {
        let res = await fetch("https://localhost:7031/api/PendingUsers/" + username,{
            method: "PUT"
        });
        return res.ok;
    }

    static async renewCodeByeEmail(email) {
        let res = await fetch("https://localhost:7031/api/PendingUsers/renew/" + email,{
            method: "PUT"
        });
        return res.ok;
    }

    /***
     * Checks if a user's verification code can be verified.
     * @param username
     * @param userInput
     * @returns {Promise<boolean>}
     */
    static async canVerify(username, userInput) {
        let res = await fetch("https://localhost:7031/api/PendingUsers/"
            + username + "?verificationCode=" + userInput);
        if (res.ok){
            Tokens.accessToken = await res.text();
            return true;
        }
        return false;
    }

    /***
     * Adds a pending user as a registered user.
     * @returns {Promise<boolean>}
     */
    static async addUser() {
        let response = await fetch("https://localhost:7031/api/RegisteredUsers/signUp", {
            method: "POST",
            headers: {
                'Authorization': 'Bearer ' + Tokens.accessToken,
            },
        })
        return response.ok;
    }
}

export default PendingUser;