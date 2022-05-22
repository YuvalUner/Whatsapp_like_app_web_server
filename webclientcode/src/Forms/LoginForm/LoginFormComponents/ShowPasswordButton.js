import $ from "jquery"

/**
 * A general button for showing passwords for password input fields.
 * @param props the id for the button and the id of the password field to affect.
 * @returns {JSX.Element}
 */
function ShowPasswordButton({props}) {

    // Turns the password field onto a text field on check, and back to a password field on uncheck.
    const showPassword = () => {
        let passField = $("#" + props.passField);
        if ($("#" + props.id).is(":checked")) {
            passField.get(0).type = "text";
        } else {
            passField.get(0).type = "password";
        }
    }

    return (
        <div>
            <input onChange={showPassword} className="form-check-input me-1" type="checkbox" value="" id={props.id}/>
            <label htmlFor="show-password-toggle" className="form-check-label small-text">Show</label>
        </div>
    )
}

export default ShowPasswordButton;