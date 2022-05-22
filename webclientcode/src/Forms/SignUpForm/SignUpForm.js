import {Link} from "react-router-dom";
import EmailField from "./SignUpComponents/EmailField";
import UsernameSignupField from "./SignUpComponents/UsernameSignupField";
import PasswordSignupField from "./SignUpComponents/PasswordSignupField";
import NicknameField from "./SignUpComponents/NicknameField";
import PhoneNumberField from "./SignUpComponents/PhoneNumberField";
import SecretQuestionsField from "./SignUpComponents/SecretQuestionsField";
import {useState} from "react";
import SecretQuestionDescriptor from "./SignUpComponents/SecretQuestionDescriptor";
import SecretQuestionAnswerField from "./SignUpComponents/SecretQuestionAnswerField";
import TermOfServiceField from "./SignUpComponents/TermsOfServiceField";
import PrivacyPolicyField from "./SignUpComponents/PrivacyPolicyField";
import $ from "jquery";
import PendingUser from "../../Users/PendingUser";
import {useNavigate} from "react-router";
import BaseForm from "../BaseForm";

/**
 * Sign up form of the app.
 * @param props
 * @returns {JSX.Element}
 */
function SignUpForm({props}) {

    // Complete abuse of states. Likely to be deleted when we have a server.
    const [userConfirm, userConfirmSet] = useState(false);
    const [passConfirm, passConfirmSet] = useState(false);
    const [passConfirmationConfirm, passConfirmationConfirmSet] = useState(false);
    const [emailConfirm, emailConfirmSet] = useState(false);
    const [nicknameConfirm, nicknameConfirmSet] = useState(false);
    const [secretQuestionConfirm, secretQuestionConfirmSet] = useState(false);
    const [secretAnswerConfirm, secretAnswerConfirmSet] = useState(false);
    const [phoneConfirm, phoneConfirmSet] = useState(true);


    const nav = useNavigate();

    //This function prevents then loss of info in refresh once we submit new user
    const handleSubmit = async (event) => {
        event.preventDefault();
        let isTos = $("#tos-radio-check").is(":checked");
        let isPrivacyPolicy = $("#privacy-policy-radio-check").is(":checked");

        //Checks that user agreed to terms of service and privacy policy, and all inputs are correct
        if (isTos && isPrivacyPolicy) {
            if (userConfirm && passConfirm && passConfirmationConfirm
                && emailConfirm && nicknameConfirm && secretQuestionConfirm && secretAnswerConfirm
                && phoneConfirm) {
                let username = $("#username-signup-field").val();
                let email = $("#email-signup-field").val().toLowerCase();
                let password = $("#new-pass1").val();
                let nickname = $("#nickname-signup-field").val();
                let phone = $("#phone-signup-field").val();
                let secretQuestion = $("#secret-questions").val();
                let secretAnswer = $("#secret-answer").val();
                //if all inputs are correct, create user
                let pendingUser = new PendingUser({
                    username: username, password: password,
                    email: email, phone: phone, nickname: nickname,
                    secretQuestion: {question: secretQuestion, answer: secretAnswer}
                });
                if (await PendingUser.signUp(pendingUser)) {
                    props.setUser(username);
                    props.from(true);
                    nav("/verify_email");
                }
                else{
                    $("#something-went-wrong").show();
                }
            }
            //If user didn't agree to terms of service or privacy policy, mark them as red.
        } else {
            if (!isTos) {
                $("#invalid-tos").show();
            }
            if (!isPrivacyPolicy) {
                $("#invalid-pp").show();
            }
        }
    }
    return (
        <BaseForm>
            <form onSubmit={handleSubmit}>
                <div>
                    <EmailField props={{setConfirm: emailConfirmSet}}/>
                    <UsernameSignupField props={{setConfirm: userConfirmSet}}/>
                    <PasswordSignupField props={{
                        setConfirmPass: passConfirmSet,
                        setConfirmationConfirm: passConfirmationConfirmSet,
                        renderRequired: true
                    }}/>
                    <NicknameField props={{setConfirm: nicknameConfirmSet}}/>
                    <PhoneNumberField props={{setConfirm: phoneConfirmSet}}/>
                    <SecretQuestionsField props={{
                        setConfirm: secretQuestionConfirmSet,
                        children: <SecretQuestionDescriptor/>,
                        renderRequired: true
                    }}/>
                    <SecretQuestionAnswerField props={{setConfirm: secretAnswerConfirmSet, renderRequired: true}}/>
                    <TermOfServiceField/>
                    <PrivacyPolicyField/>
                    <div className="d-grid gap-2 col-6 mx-auto mb-3">
                        <button className="btn btn-primary">Submit</button>
                    </div>
                    <div className="mt-2 error-text" id="something-went-wrong">
                        Oops, something went wrong. Please verify your details and try again.
                    </div>
                </div>
                <div>
                    <Link to="/">I already have an account</Link>
                </div>
            </form>
        </BaseForm>
    )
}

export default SignUpForm;