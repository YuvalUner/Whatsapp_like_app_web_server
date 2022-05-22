/**
 * Basic text for the verification form.
 * @param props
 * @returns {JSX.Element}
 */
function VerificationFormText({props}) {
    let title = () => {
        if (props.fromSignup) {
            return "Please verify your email to complete your registration";
        } else {
            return "Please verify your identity to reset your password";
        }
    }
    return (
        <div>
            <h4>
                {title()}
            </h4>
            <p>
                A 6 letter verification code has been sent to your email.
            </p>
            <p>
                Please input the code sent to you in the text box below, and then hit submit.
            </p>
            <p>
                While we have implemented this, as this is only for demonstration purposes, you can also input
                111111 to bypass this.
            </p>
        </div>
    );
}

export default VerificationFormText;