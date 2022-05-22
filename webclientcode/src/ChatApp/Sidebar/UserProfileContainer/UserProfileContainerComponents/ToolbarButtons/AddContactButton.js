/**
 * A button that opens the add contact modal.
 * @param props
 * @returns {JSX.Element}
 */
function AddContactButton({props}) {

    return (
        <span data-bs-toggle="modal" data-bs-target="#add-contact-modal">
            <button type="button" className="btn no-effect-button btn-lg pe-0"
                    data-bs-toggle="tooltip" data-bs-placement="bottom"
                    title="Add contact" onClick={() => props.setShow(true)}>
                <i className="bi bi-person-plus float-end"/>
            </button>
        </span>
    )
}

export default AddContactButton;