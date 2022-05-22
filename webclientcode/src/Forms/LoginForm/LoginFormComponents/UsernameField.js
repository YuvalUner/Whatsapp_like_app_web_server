import UsernameEmailRadio from "./UsernameEmailRadio";
import $ from "jquery";

/**
 * The field for inputting the username in the log-in form.
 * @param props
 * @returns {JSX.Element}
 */
function UsernameField({props}) {

    let text = () => {
        if (props.usernameDefault) {
            return "Username";
        } else {
            return "Email";
        }
    }

    return (
        <div className="row mb-3">
            <label htmlFor="login-username" className="col-form-label col-2" id="username-label">{text()}</label>
            <div className="col-8">
                <input name="username" onKeyUp={() => props.username($("#login-username").val())}
                       defaultValue={props.current} type="text" id="login-username"
                       className="form-control"/>
            </div>
            <UsernameEmailRadio props={{toggle: props.toggle, usernameDefault: props.usernameDefault}}/>
        </div>
    )
}

export default UsernameField;