import EmailVerificationForm from "../../EmailVerificationForm/EmailVerificationForm";

/**
 * Email verification form for the reset password process.
 * @param props
 * @returns {JSX.Element}
 */
function ForgotPasswordFormVerificationScreen({props}) {
    return (
        <EmailVerificationForm props={{username: props.username, from: "forgot_password"}}/>
    )
}

export default ForgotPasswordFormVerificationScreen;