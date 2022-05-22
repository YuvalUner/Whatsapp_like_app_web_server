/**
 * A basic phone number input field for the modal.
 * @returns {JSX.Element}
 */
function ModalPhoneField() {
    return (
        <div>
            <div className="input-group">
                <span className="input-group-text" id="username-addon"><i className="bi bi-telephone-plus-fill"/></span>
                <input type="tel" placeholder="Contact's phone number"
                       autoComplete="true" id="modal-field" className="form-control"/>
            </div>
        </div>
    )
}

export default ModalPhoneField;