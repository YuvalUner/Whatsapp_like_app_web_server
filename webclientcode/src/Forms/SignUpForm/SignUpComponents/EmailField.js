import $ from "jquery";
import RegisteredUser from "../../../Users/RegisteredUser";
import PendingUser from "../../../Users/PendingUser";

/**
 * The field of email in the signup form.
 * @param props
 * @returns {JSX.Element}
 */
function EmailField({props}){

    const handleBlur = async ()=>{
        let emailField = $("#email-signup-field");
        let current = emailField.val();
        let text = $("#email-error");
        //if condition checks validity of email.
        if(current.length < 3 || !current.includes("@") || current.length > 300){
            emailField.removeClass("border-success");
            emailField.addClass("border-danger");
            text.text("Error: Invalid email address");
            text.show();
            props.setConfirm(false);
        }
        //if condition checks if the email already exists in system, if so mark as red and alert.
        else if (await PendingUser.doesUserExistByEmail(current)){
            emailField.removeClass("border-success");
            emailField.addClass("border-danger");
            text.text("Error: Email already exists")
            text.show();
            props.setConfirm(false);
        }
        //else condition marks that email is good and colors it in green
        else{
            emailField.removeClass("border-danger");
            emailField.addClass("border-success");
            text.hide();
            props.setConfirm(true);
        }
    }
    return (
        <div className="row mb-3">
            <label htmlFor="email-signup-field" className="col-2 col-form-label">
                Email:
                <span className = "required-field"> *</span>
            </label>
            <div className="col-10">
                <input type="email" onBlur={async () => await handleBlur()}
                       id="email-signup-field" className="form-control" placeholder="name@example.com" required/>
                <div className="error-text" id="email-error"/>
            </div>
        </div>
    )
}

export default EmailField;