import RegisteredUser from "../../../Users/RegisteredUser";
import $ from "jquery";
import PendingUser from "../../../Users/PendingUser";

/**
 * username field in the signup form.
 * @param props
 * @returns {JSX.Element}
 */
function UsernameSignupField({props}){

    //function checks validity of username and makes sure it doesn't already exist.
    const handleBlur = async ()=>{
        let usernameField = $("#username-signup-field");
        let text = $("#username-error");

        //if condition checks validity of username.
        if (usernameField.val() === "" || !usernameField.val().match("^[\\w\\s]{1,50}$")){
            usernameField.removeClass("border-success");
            usernameField.addClass("border-danger");
            text.text("Error: Username must be between 1-50 characters and must not contain any special characters")
            text.show();
            props.setConfirm(false);
        }

        //else if condition checks user doesn't already exist, if already exists, pritns error message.
        else if (await PendingUser.doesUserExistByUsername(usernameField.val())){
            usernameField.removeClass("border-success");
            usernameField.addClass("border-danger");
            text.text("Error: Username already exists")
            text.show();
            props.setConfirm(false);
        }

        //username passed all tests and is valid, color border in green and hide error message.
        else{
            usernameField.removeClass("border-danger");
            usernameField.addClass("border-success");
            text.hide();
            props.setConfirm(true);
        }
    }

    return (
        <div className="row mb-3">
            <label htmlFor="signup-username" className="col-4 col-form-label">Username:
                <span className = "required-field"> *</span>
            </label>
            <div className="col-8">
                <input type="text" onBlur={async () => await handleBlur()} id="username-signup-field" className="form-control" placeholder="Example: John Smith" required />
                <div className="error-text" id="username-error"/>
            </div>
        </div>
    )
}

export default UsernameSignupField;