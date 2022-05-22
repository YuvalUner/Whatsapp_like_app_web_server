import $ from "jquery";
import ResendCodeButton from "./ResendCodeButton";

/**
 * Field for inputting the verifier code sent to the user.
 * @param props
 * @returns {JSX.Element}
 */
function VerifierField({props}) {

    const verifyCodeStructure = () => {
        let userInput = props.textRef.current.value;
        let field = $("#verification-code-input");
        let text = $("#format-error");
        // Performs basic validity check on the verifier code structure.
        $("#verification-error").hide();
        if (userInput.length > 6 || userInput.length === 0 || !userInput.match("^[0-9a-zA-Z]*$")) {
            field.addClass("border-danger");
            text.show();
        } else {
            field.removeClass("border-danger");
            text.hide();
        }
    }

    return (
        <div className="row g-3r">
            <label htmlFor="verification-code-input" className="col-form-label col-4">Verification code:</label>
            <div className="col-4">
                <input ref={props.textRef} type="text" name="verification-code" className="form-control"
                       id="verification-code-input" onChange={verifyCodeStructure}/>
                <span id="format-error" className="error-text">Verification code must be
                        6 digits long and only contain letters and numbers</span>
                <span id="verification-error" className="error-text">Error - incorrect code</span>
            </div>
            <div className="col-4">
                <ResendCodeButton props={{username: props.username, fromSignup: props.fromSignup}}/>
            </div>
        </div>
    )
}

export default VerifierField;