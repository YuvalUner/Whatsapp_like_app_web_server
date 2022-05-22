import $ from "jquery";

/**
 * Nickname field in the signup form.
 * @param props
 * @returns {JSX.Element}
 */
function NicknameField({props}){

    //function checks validity of nickname, if not valid, colors in red and prints error message.
    const handleBlur = ()=>{
        let text = $("#nickname-error");
        let nicknameField = $("#nickname-signup-field");
        if (nicknameField.val() === "" || !nicknameField.val().match("^[\\w\\s]{1,10}$")){
            nicknameField.removeClass("border-success");
            nicknameField.addClass("border-danger");
            text.text("Error: Nickname must be between 1-10 characters and can not contain any special characters")
            text.show();
            props.setConfirm(false);
        }
        //else condition occurs if nickname is valid, and color the field in green.
        else{
            nicknameField.removeClass("border-danger");
            nicknameField.addClass("border-success");
            text.hide();
            props.setConfirm(true);
        }
    }

    return (
        <div className="row mb-3">
            <label htmlFor="nickname" className="col-3 col-form-label">Nick name:
                <span className = "required-field"> *</span>
            </label>
            <div className="col-9">
                <input type="text" onBlur={handleBlur} id="nickname-signup-field" className="form-control" placeholder="Example: Mr. Bond007"/>
                <div className="error-text" id="nickname-error"/>
            </div>
        </div>
    )
}

export default NicknameField;