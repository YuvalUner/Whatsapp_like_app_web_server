import AttachMenu from "./InputBarComponents/AttachMenu";
import {OverlayTrigger, Popover} from "react-bootstrap";
import $ from "jquery"
import RegisteredUser from "../../../Users/RegisteredUser";
import RecordMessageModal from "./InputBarComponents/RecordMessageModal";
import {useState} from "react";

/**
 * input bar component of the chat.
 * @param props
 * @returns {JSX.Element}
 */
const InputBar =  (props) => {
    const [modalShow, setModalShow] = useState(false);

    //function sends the text message typed into the input bar, and adds it to the conversation.
    const handleSend = async (e) => {
        e.preventDefault();
        let inputField = $("#message-input");
        await RegisteredUser.addMessageToConvo(props.username, props.convo, {
            sender: true, type: "text", time: new Date(), content: inputField.val()
        })
        inputField.val("");
        props.connection.invoke("messageSent", props.convo);
        props.setConvo();
    }

        return (
            <>
            <form onSubmit={handleSend}>
                <div>
                    <div className="input-group mb-3 input-message-pad">
                        <OverlayTrigger trigger="click" placemnt="top" rootClose={true} overlay={

                            // popover for the modal when trying to record an audio in chat.
                            <Popover id="attach-menu-popover">
                                <Popover.Body>
                                    <AttachMenu openRecordMessageModal={() => setModalShow(true)} props={{username: props.username, convo: props.convo, setConvo: props.setConvo}}/>
                                </Popover.Body>
                            </Popover>
                        }>
                            <button type="button" className="btn btn-success" id="paperclip-click">
                                <i className="bi bi-input bi-paperclip"/>
                            </button>
                        </OverlayTrigger>
                        <input type="text" id="message-input" className="form-control" placeholder="New message here..."
                               aria-label="Example text with button addon"
                               aria-describedby="button-addon1"/>
                        <button type="submit" className="btn btn-success">Send</button>
                    </div>
                </div>
            </form>
                <RecordMessageModal
                    show={modalShow}
                    onHide={() => setModalShow(false)}
                    username={props.username}
                    convo={props.convo}
                    setConvo={props.setConvo}
                />
                </>
        )
}

export default InputBar;