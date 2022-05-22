import {Component} from "react";
import ModalSelectionButtons from "./AddContactModalComponents/ModalSelectionButtons";
import {Modal, Button} from "react-bootstrap";
import ModalEmailField from "./AddContactModalComponents/ModalEmailField";
import ModalUsernameField from "./AddContactModalComponents/ModalUsernameField";
import ModalPhoneField from "./AddContactModalComponents/ModalPhoneField";
import RegisteredUser from "../../../../Users/RegisteredUser";
import $ from "jquery";

/**
 * A modal for adding a contact.
 */
class AddContactModal extends Component {
    constructor(props) {
        super(props);
        this.state = {
            radioValue: "username"
        };
    }

    /**
     * Determines which input field to render.
     * @returns {JSX.Element}
     */
    renderField = () => {
         if (this.state.radioValue === "username") {
            return (<ModalUsernameField/>)
        } else if (this.state.radioValue === "email") {
            return (<ModalEmailField/>)
        } else {
            return (<ModalPhoneField/>)
        }
    }

    /**
     * Changes the state of which radio button is chosen.
     * @param val
     */
    setRadioVal = (val) => {
        this.setState({
            radioValue: val
        });
        $("#modal-field-error").hide();
    }

    /**
     * Handles submission once the user submits the contact's info.
     * @param e
     */
    handleSubmit = async (e) => {
        e.preventDefault();
        let val = $("#modal-field").val();
        if (this.state.radioValue === "username") {
            // If contact exists and is not already the user's contact, add them.
            if (await RegisteredUser.doesUserExistByUsername(val)) {
                if (await RegisteredUser.isAlreadyContactByUsername(this.props.username, val) === false) {
                    await RegisteredUser.addContactByUsername(this.props.username, val);
                    await this.props.updateContacts();
                    this.props.connection.invoke("addedContactByUsername", val);
                    this.props.setShow(false);
                } else {
                    let errText = $("#modal-field-error");
                    errText.text("Error: " + val + " is already your contact");
                    errText.show();
                }
            } else {
                let errText = $("#modal-field-error");
                errText.text("Error: A user with this " + this.state.radioValue + " does not exist")
                errText.show();
            }
        }
        else if (this.state.radioValue === "email"){
            if (await RegisteredUser.doesUserExistByEmail(val)) {
                if (await RegisteredUser.isAlreadyContactByEmail(this.props.username, val) === false) {
                    await RegisteredUser.addContactByEmail(this.props.username, val);
                    await this.props.updateContacts();
                    this.props.connection.invoke("addedContactByEmail", val);
                    this.props.setShow(false);
                } else {
                    let errText = $("#modal-field-error");
                    errText.text("Error: " + val + " is already your contact");
                    errText.show();
                }
            } else {
                let errText = $("#modal-field-error");
                errText.text("Error: A user with this " + this.state.radioValue + " does not exist")
                errText.show();
            }
        }
        else{
            if (await RegisteredUser.doesUserExistByPhone(val)) {
                if (await RegisteredUser.isAlreadyContactByPhone(this.props.username, val) === false) {
                    await RegisteredUser.addContactByPhone(this.props.username, val);
                    await this.props.updateContacts();
                    this.props.connection.invoke("addedContactByPhone", val);
                    this.props.setShow(false);
                } else {
                    let errText = $("#modal-field-error");
                    errText.text("Error: " + val + " is already your contact");
                    errText.show();
                }
            } else {
                let errText = $("#modal-field-error");
                errText.text("Error: A user with this " + this.state.radioValue + " does not exist")
                errText.show();
            }
        }
    }


    render() {
        return (
            <Modal
                show={this.props.show}
                size="lg"
                aria-labelledby="add-contact-modal"
                centered
            >
                <Modal.Header closeButton onClick={() => {
                    this.props.setShow(false)
                }}>
                    <Modal.Title id="add-contact-modal">
                        Add contact
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <form onSubmit={this.handleSubmit}>
                        <div>
                            <ModalSelectionButtons props={{set: this.setRadioVal}}/>
                            <div className="m-5">
                                {this.renderField()}
                                <div className="error-text" id="modal-field-error"/>
                            </div>
                        </div>
                    </form>
                </Modal.Body>
                <Modal.Footer>
                    <Button className="btn-danger" onClick={() => this.props.setShow(false)}>Close</Button>
                    <Button onClick={async (e) => await this.handleSubmit(e)}>Add contact</Button>
                </Modal.Footer>
            </Modal>
        )
    }
}

export default AddContactModal;