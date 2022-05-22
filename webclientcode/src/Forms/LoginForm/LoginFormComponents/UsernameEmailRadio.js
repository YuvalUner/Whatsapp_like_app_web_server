import $ from "jquery"

/**
 * Radio buttons for switching between inputting username and inputting email.
 * @param props which checkbox to toggle by default.
 * @returns {JSX.Element}
 */
function UsernameEmailRadio({props}) {

    const onChange = (event, text, val) => {
        $("#username-label").text(text);
        props.toggle(val);
    }

    return (
        <div>
            <div className="form-check form-check-inline">
                <input className="form-check-input-sm" type="radio" name="inlineRadioOptions" id="username-radio"
                       value="1" onChange={event => onChange(event, "Username:", true)}
                       defaultChecked={props.usernameDefault}/>
                <label className="form-check-label" htmlFor="username-radio">Username</label>
            </div>
            <div className="form-check form-check-inline">
                <input className="form-check-input-sm" type="radio" name="inlineRadioOptions" id="email-radio"
                       value="2" onChange={event => onChange(event, "Email:", false)}
                       defaultChecked={!props.usernameDefault}/>
                <label className="form-check-label" htmlFor="email-radio">Email</label>
            </div>
        </div>
    )
}

export default UsernameEmailRadio;