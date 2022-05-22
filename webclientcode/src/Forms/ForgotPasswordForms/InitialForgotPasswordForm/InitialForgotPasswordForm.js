import {Link} from "react-router-dom";
import UsernameField from "../../LoginForm/LoginFormComponents/UsernameField";
import {useState} from "react";
import SecretQuestionsField from "../../SignUpForm/SignUpComponents/SecretQuestionsField";
import SecretQuestionAnswerField from "../../SignUpForm/SignUpComponents/SecretQuestionAnswerField";
import RegisteredUser from "../../../Users/RegisteredUser";
import $ from "jquery"
import InitialForgotPasswordFormText from "./InitialForgotPasswordComponents/InitialForgotPasswordFormText";
import {useNavigate} from "react-router";
import BaseForm from "../../BaseForm";

/**
 * The initial form for resetting password. Verifies basic user info.
 * @param props
 * @returns {JSX.Element}
 */
function InitialForgotPasswordForm({props}) {

    const nav = useNavigate();
    const [questionConfirm, questionConfirmSet] = useState(false);
    const [answerConfirm, answerConfirmSet] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        let val = $("#login-username").val();
        if (questionConfirm && answerConfirm) {
            if (await RegisteredUser.VerifySecretQuestion(val,
                $("#secret-questions").val(), $("#secret-answer").val())) {
                await RegisteredUser.generateVerCode(val);
                nav("/forgot_password/verify");
            } else {
                let warnText = $("#wrong-details-text");
                if ($("#username-radio").is(":checked")) {
                    warnText.text("Error: Incorrect username and / or security question and / or answer")
                    warnText.show();
                } else {
                    warnText.text("Error: Incorrect Email and / or security question and / or answer")
                    warnText.show();
                }
            }
        }
    }

    const clearText = () => {
        props.usernameSetter("");
    }

    return (
        <BaseForm>
            <form id="forgot-password-form" onSubmit={async(e) => handleSubmit(e)}>
                <InitialForgotPasswordFormText/>
                <div className="row">
                    <UsernameField props={{
                        username: props.usernameSetter, toggle: props.toggleSetter, current: props.username,
                        usernameDefault: props.toggle
                    }}/>
                </div>
                <div className="row">
                    <SecretQuestionsField
                        props={{children: null, setConfirm: questionConfirmSet, renderRequired: false}}/>
                </div>
                <div className="row">
                    <SecretQuestionAnswerField props={{setConfirm: answerConfirmSet, renderRequired: false}}/>
                </div>
                <div className="col text-center mb-2">
                    <button type="submit" className="btn btn-primary">Submit</button>
                </div>
                <div id="wrong-details-text" className="row mb-3 error-text"/>
                <div className="row">
                    <Link to="/" onClick={clearText}>I remember my password</Link>
                </div>
            </form>
        </BaseForm>
    )
}

export default InitialForgotPasswordForm;