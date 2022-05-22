import VerifierField from "./EmailVerificationComponents/VerifierField";
import {useRef} from "react";
import PendingUser from "../../Users/PendingUser";
import $ from "jquery";
import VerificationFormText from "./EmailVerificationComponents/VerificationFormText";
import RegisteredUser from "../../Users/RegisteredUser";
import {useNavigate} from "react-router";
import BaseForm from "../BaseForm";
import Tokens from "../../Users/Tokens";

/**
 * Email verification form for when the user is sent a code to their email.
 * @param props
 * @returns {JSX.Element}
 */
function EmailVerificationForm({props}) {

    const textFormRef = useRef("");

    const nav = useNavigate();

    let handleSubmit = async (e) => {
        e.preventDefault();
        let code = textFormRef.current.value;
        let field = $("#verification-code-input");
        let text = $("#format-error");
        // Check code structure validity.
        if (code.length !== 6) {
            field.addClass("border-danger");
            text.show();
        }
        const onError = () => {
            field.addClass("border-danger");
            text.hide();
            $("#verification-error").show();
        }
        // If fromSignup, log the user in.
        if (props.fromSignup) {
            if (await PendingUser.canVerify(props.username, code)) {
                await PendingUser.addUser(props.username);
                props.setFrom(false);
                props.setLogIn(true);
                Tokens.autoRenewTokens(false);
                nav("/")
            } else {
                onError();
            }
            // Otherwise, continue to reset password form.
        } else {
            if (await RegisteredUser.canVerify(props.username, code)) {
                nav("/forgot_password/reset_password");
            } else {
                onError();
            }
        }
    }

    return (
        <BaseForm>
            <form id="verify-form" onSubmit={handleSubmit}>
                <VerificationFormText props={{fromSignup: props.fromSignup}}/>
                <VerifierField props={{textRef: textFormRef, username: props.username, fromSignup: props.fromSignup}}/>
                <div className="col text-center mt-4">
                    <button type="submit" className="btn btn-primary" onClick={handleSubmit}>Submit</button>
                </div>
            </form>
        </BaseForm>
    )
}

export default EmailVerificationForm;