/**
 * A basic email input field for the modal.
 * @param props
 * @returns {JSX.Element}
 */
function ModalEmailField({props}) {
    return (
        <div className="input-group">
            <span className="input-group-text" id="username-addon"><i className="bi bi-envelope-plus-fill"/></span>
            <input type="email" placeholder="Contact's email"
                   autoComplete="true" id="modal-field" className="form-control"/>
        </div>
    )
}

export default ModalEmailField;