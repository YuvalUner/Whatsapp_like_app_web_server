/**
 * A simple checkbox for the user to choose if to remember them or not.
 * @returns {JSX.Element}
 */
function RememberMeCheckbox() {
    return (
        <div className="mb-3">
            <input defaultChecked={true} type="checkbox" className="form-check-input" id="remember-me-checkbox"/>
            <label className="form-check-label" htmlFor="remember-me-checkbox">Remember me</label>
        </div>
    )
}

export default RememberMeCheckbox;