import {Link} from "react-router-dom";
import UsernameField from "./LoginFormComponents/UsernameField";
import PasswordField from "./LoginFormComponents/PasswordField";
import RegisteredUser from "../../Users/RegisteredUser";
import $ from "jquery";
import CookieHandling from "../../Misc/CookieHandling";
import {useNavigate} from "react-router";
import BaseForm from "../BaseForm";
import RememberMeCheckbox from "./LoginFormComponents/RememberMeCheckbox";
import Tokens from "../../Users/Tokens";
import PendingUser from "../../Users/PendingUser";

/**
 * The log-in form of the app.
 * @param props
 * @returns {JSX.Element}
 */
function LoginForm({props}) {

    const nav = useNavigate();

    // Verifies the user's identity on submit or other events.
    const handleVerification = async (e, username, password) => {
        e.preventDefault();
        let isUsername = $("#username-radio").is(":checked");
        if (username === "" && password === "") {
            return
        }
        // If user verified successfully, this will be executed.
        const onSuccess = (rememberMe) => {
            wrongDetails.hide();
            props.setLogIn(true);
            if (rememberMe) {
                // Renews cookie for the logged-in user.
                CookieHandling.deleteCookie("rToken");
                CookieHandling.setCookie("rToken", Tokens.refreshToken, 30);
            }
            Tokens.autoRenewTokens(rememberMe);
            nav("/chat");
        }
        let wrongDetails = $("#wrong-details-text");
        if (isUsername) {
            if (await RegisteredUser.DoUserAndPasswordMatch(username, password)) {
                onSuccess($("#remember-me-checkbox").is(":checked"));
            } else {
                if (await PendingUser.checkPendingUserMatch(username, password)){
                    await PendingUser.renewCode(username);
                    props.fromSetter(true);
                    nav("/verify_email");
                }
                else {
                    wrongDetails.text("Incorrect username or password");
                    wrongDetails.show();
                }
            }
        } else {
            // Emails are not case-sensitive, hence the toLowerCase.
            if (await RegisteredUser.doEmailAndPasswordMatch(username.toLowerCase(), password)) {
                onSuccess();
            } else {
                if (await PendingUser.checkPendingUserMatchByEmail(username, password)){
                    await PendingUser.renewCodeByeEmail(username);
                    props.fromSetter(true);
                    nav("/verify_email");
                }
                else {
                    wrongDetails.text("Incorrect Email or password");
                    wrongDetails.show();
                }
            }
        }
    }

    const resetFrom = () => {
        props.fromSetter(false);
    }


    return (
        <BaseForm>
            <div className="text-center">
                <p className="fs-3">Log in</p>
            </div>
            <form onSubmit={event => handleVerification(event, $("#login-username").val(), $("#login-pass").val())}
                  id="log-in-form">
                {/*In case the user just reset their password, include this element.*/}
                {props.passReset && <div className="
            col-8 border border-success rounded mb-4 padding-5 shadow-sm bg-light p-2 text-success">
                    Password reset successfully. You may now log in.
                </div>}
                <UsernameField
                    props={{username: props.username, toggle: props.toggle, current: "", usernameDefault: true}}/>
                <PasswordField/>
                <div id="wrong-details-text" className="mb-3 error-text"/>
                <div className="col text-center">
                    <button type="submit" className="btn btn-primary mb-3 col-2">Log in</button>
                </div>
                <RememberMeCheckbox/>
                <div className="mb-3 float-end">
                    New user?
                    <Link to="/sign_up" onClick={resetFrom}> Sign up here</Link>
                </div>
                <div>
                    <Link to="/forgot_password" onClick={resetFrom}>Forgot my password</Link>
                </div>
            </form>
        </BaseForm>
    )
}


export default LoginForm;