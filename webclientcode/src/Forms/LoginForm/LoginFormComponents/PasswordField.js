import ShowPasswordButton from "./ShowPasswordButton";

/**
 * The password field for the log-in form.
 * @returns {JSX.Element}
 */
function PasswordField() {
    return (
        <div className="row mb-3">
            <label htmlFor="login-pass" className="col-2 col-form-label">Password:</label>
            <div className="col-8">
                <input name="password" type="password" id="login-pass" className="form-control"/>
            </div>
            <div className="col-2">
                <ShowPasswordButton props={{passField: "login-pass", id: "toggle-pass"}}/>
            </div>
        </div>
    )
}

export default PasswordField;