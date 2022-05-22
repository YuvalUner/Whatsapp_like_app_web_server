/**
 * A set of radio buttons for selecting the input type in the modal.
 * @param props
 * @returns {JSX.Element}
 */
function ModalSelectionButtons({props}) {
    return (
        <div className="btn-group" role="group" aria-label="Basic radio toggle button group">
            <input type="radio" className="btn-check" name="addContactSelectionRadio" id="btnradio2"
                   autoComplete="off" defaultChecked={true}
                   value="username" onChange={() => props.set("username")}/>
            <label className="btn btn-outline-primary" htmlFor="btnradio2">Username</label>

            <input type="radio" className="btn-check" name="addContactSelectionRadio" id="btnradio3"
                   autoComplete="off" value="email" onChange={() => props.set("email")}/>
            <label className="btn btn-outline-primary" htmlFor="btnradio3">Email</label>

            <input type="radio" className="btn-check" name="addContactSelectionRadio" id="btnradio4"
                   autoComplete="off" value="phone" onChange={() => props.set("phone number")}/>
            <label className="btn btn-outline-primary" htmlFor="btnradio4">Phone number</label>
        </div>
    )
}

export default ModalSelectionButtons;