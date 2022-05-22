import {Routes} from "react-router";
import {Route} from "react-router-dom";
import LoginForm from "../Forms/LoginForm/LoginForm";
import SignUpForm from "../Forms/SignUpForm/SignUpForm";
import InitialForgotPasswordForm
    from "../Forms/ForgotPasswordForms/InitialForgotPasswordForm/InitialForgotPasswordForm";
import EmailVerificationForm from "../Forms/EmailVerificationForm/EmailVerificationForm";
import ForgotPasswordFormVerificationScreen
    from "../Forms/ForgotPasswordForms/ForgotPasswordFormVerificationScreen/ForgotPasswordFormVerificationScreen";
import ForgotPasswordFormResetPassword
    from "../Forms/ForgotPasswordForms/ForgotPasswordFormResetPassword/ForgotPasswordFormResetPassword";
import MainApp from "../ChatApp/MainApp";
import React, {useState} from "react";

/**
 * A hack used for putting the whole app in one place and maintaining session storage, as well as controlling where
 * users can go when logged in and when not logged in.
 * @param props
 * @returns {JSX.Element}
 */
function Router({props}) {

    const [toggle, setToggle] = useState(true);
    const [from, setFrom] = useState(false);

    const auth = () => {
        // If user is not logged in, limit access to only the forms.
        if (!props.loggedIn) {
            return (
                <>
                    <Route path="/log_in" element={<LoginForm props={{
                        username: props.setUsername, toggle: setToggle,
                        fromSetter: setFrom, passReset: from, setLogIn: props.setLoggedIn
                    }}/>}/>
                    <Route path="/sign_up" element={<SignUpForm props={{setUser: props.setUsername, from: setFrom}}/>}/>
                    <Route path="/forgot_password" element={<InitialForgotPasswordForm
                        props={{
                            username: props.username,
                            usernameSetter: props.setUsername,
                            toggle: toggle,
                            toggleSetter: setToggle,
                        }}/>}/>
                    <Route path="*" element={<LoginForm props={{
                        username: props.setUsername, toggle: setToggle,
                        fromSetter: setFrom, passReset: from, setLogIn: props.setLoggedIn
                    }}/>}/>
                    <Route path="/verify_email" element={
                        <EmailVerificationForm props={{
                            username: props.username, fromSignup: from,
                            setFrom: setFrom, setLogIn: props.setLoggedIn
                        }}/>}/>
                    <Route path="/forgot_password/verify" element={
                        <ForgotPasswordFormVerificationScreen props={{username: props.username}}/>}/>
                    <Route path="/forgot_password/reset_password" element={
                        <ForgotPasswordFormResetPassword props={{username: props.username, setter: setFrom}}/>}/>
                </>
            )
        }
        // Otherwise, limit access to only the chat.
        else {
            return (
                <>
                    <Route path="*" element={<MainApp setLogIn={props.setLoggedIn} username={props.username}/>}/>
                </>
            )
        }
    }

    return (
        <Routes>
            {auth()}
        </Routes>
    )
}

export default Router;