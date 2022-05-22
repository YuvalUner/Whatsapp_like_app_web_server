/**
 * A collection of methods for handling cookies.
 * Used for automatically logging in the user.
 * Basically just stolen from https://www.w3schools.com/js/js_cookies.asp.
 */
class CookieHandling {
    /**
     * Stes a cookie's value.
     * @param cname cookie name.
     * @param cvalue cookie value.
     * @param exdays TTL.
     */
    static setCookie(cname, cvalue, exdays) {
        const d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";";
    }

    /**
     * Returns cookie's value.
     * @param cname The cookie to get.
     * @returns {string} the cookie's value.
     */
    static getCookie(cname) {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) === ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) === 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    /**
     * Deletes a cookie
     * @param cname cookie's name.
     */
    static deleteCookie(cname) {
        const cvalue = CookieHandling.getCookie(cname);
        document.cookie = cname + "=" + cvalue + ";expires=Thu, 01 Jan 1970 00:00:01 GMT"
    }
}

export default CookieHandling;