import $ from "jquery";
import PendingUser from "../../../Users/PendingUser";

/**
 * Phone number field in signup form.
 * @returns {JSX.Element}
 */
function PhoneNumberField({props}){

    const handleBlur = async ()=>{
        let phoneField = $("#phone-signup-field");
        let current = phoneField.val();
        let text = $("#phone-error");
        //if condition checks validity of email.
        if(!current.match("^[0-9]*$") || current.length > 300){
            phoneField.removeClass("border-success");
            phoneField.addClass("border-danger");
            text.text("Error: Invalid phone number");
            text.show();
            props.setConfirm(false);
        }
        //if condition checks if the email already exists in system, if so mark as red and alert.
        else if (await PendingUser.doesUserExistByPhoneNumber(current)){
            phoneField.removeClass("border-success");
            phoneField.addClass("border-danger");
            text.text("Error: Phone number already exists")
            text.show();
            props.setConfirm(false);
        }
        //else condition marks that email is good and colors it in green
        else{
            phoneField.removeClass("border-danger");
            phoneField.addClass("border-success");
            text.hide();
            props.setConfirm(true);
        }
    }

    return (
        <div>
            <div className="row mb-0">
            <label htmlFor="phone" className="col-3 col-form-label">Phone number:</label>
            <div className="col-9">
                <input type="tel" id="phone-signup-field" className="form-control"
                       onBlur={async () => handleBlur()} placeholder="Example: 05-1234-5678"/>
                <div className="error-text" id="phone-error"/>
            </div>
        </div>
            <div className="row mb-2">
                        <span id="phone" className="form-text">Phone number must include only numbers.</span>
            </div>
        </div>
    )
}

export default PhoneNumberField;