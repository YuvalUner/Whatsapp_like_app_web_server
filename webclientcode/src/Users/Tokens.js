import CookieHandling from "../Misc/CookieHandling";

/***
 * A class for handling all token related operations.
 */
class Tokens{
    static accessToken;
    static refreshToken;

    /***
     * Renews a user's tokens.
     * @param token a current refresh token.
     * @param login whether to use this to log in.
     * @param save Whether to save the refresh token as a cookie or not.
     * @returns {Promise<boolean|*>}
     */
    static async renewTokens(token, login, save){
        try {
            let res = await fetch("https://localhost:7031/api/RefreshToken?login=" + login, {
                method: "PUT",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Token: token,
                })
            });
            if (res.ok) {
                let tokens = await res.json();
                Tokens.accessToken = tokens.accessToken;
                Tokens.refreshToken = tokens.refreshToken;
                if (save) {
                    CookieHandling.setCookie("rToken", Tokens.refreshToken, 30);
                }
                if (login) {
                    return tokens.username;
                }
                return true;
            }
        }
        // If the fetch request itself failed due to a network error, start trying again every 8 seconds.
        catch(error){
            setTimeout(await Tokens.renewTokens(token, login, save), 8000);
            return false;
        }
        return false;
    }

    /***
     * Begins auto renewing the tokens every 4.75 minutes.
     * @param save whether to save the refresh token as a cookie or not.
     */
    static autoRenewTokens(save){
        setInterval(async () => await Tokens.renewTokens(Tokens.refreshToken, false, save), 285000);
    }
}

export default Tokens;