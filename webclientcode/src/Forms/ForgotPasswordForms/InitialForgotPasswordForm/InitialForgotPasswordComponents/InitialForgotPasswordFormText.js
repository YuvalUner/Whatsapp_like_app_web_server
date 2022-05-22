/**
 * Basic text for the forgot password initial form.
 * @returns {JSX.Element}
 */
function InitialForgotPasswordFormText() {
    return (
        <div className="row">
            <h3>
                Some verifications...
            </h3>
            <p>
                Before we can reset your password, we would like to confirm your identity.
            </p>
            <p>
                Please enter your username / email, and then choose your security question and its answer.
            </p>
        </div>
    )
}

export default InitialForgotPasswordFormText;