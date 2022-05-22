import PasswordSignupField from "../../SignUpForm/SignUpComponents/PasswordSignupField";
import ForgotPasswordFormResetPasswordText
    from "./ForgotPasswordFormResetPasswordComponents/ForgotPasswordFormResetPasswordText";
import {useState} from "react";
import {useNavigate} from "react-router";
import RegisteredUser from "../../../Users/RegisteredUser";
import $ from "jquery";
import BaseForm from "../../BaseForm";

/**
 * The reset password form.
 * @param props
 * @returns {JSX.Element}
 */
function ForgotPasswordFormResetPassword({props}) {

    // Using states to have the fields confirm validity.
    const [passwordConfirm, passwordConfirmSet] = useState(false);
    const [passwordConfirmationConfirm, passwordConfirmationConfirmSet] = useState(false);
    const nav = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (passwordConfirm && passwordConfirmationConfirm) {
            props.setter(true);
            await RegisteredUser.updatePassword(props.username, $("#new-pass1").val());
            nav("/");
        }
    }

    return (
        <BaseForm>
            <form onSubmit={async (e) => handleSubmit(e)}>
                <ForgotPasswordFormResetPasswordText/>
                <PasswordSignupField props={{
                    setConfirmPass: passwordConfirmSet,
                    setConfirmationConfirm: passwordConfirmationConfirmSet, renderRequired: false
                }}/>
                <div className="col text-center mt-2">
                    <button className="btn btn-primary" type="submit">Submit</button>
                </div>
            </form>
        </BaseForm>
    )
}

export default ForgotPasswordFormResetPassword;