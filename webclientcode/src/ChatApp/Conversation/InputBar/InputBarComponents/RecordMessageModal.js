import {useState} from "react";
import RegisteredUser from "../../../../Users/RegisteredUser";
import {Button, Modal} from "react-bootstrap";

let mediaRecorder;
let mediaStream;

/**
 * a modal that pops up for clicking the audio recording button.
 * @param props
 * @returns {JSX.Element}
 */
function RecordMessageModal(props) {

    const [isRecording, setIsRecording] = useState(false);

    //if condition that stops recording if we click the stop button
    function onClick() {
        if (isRecording) {
            // stop record
            mediaRecorder.stop()
            mediaStream.getTracks()[0].stop();
            setIsRecording(false);

            //else condition that keeps the recording going if user doesn't push the stop button.
        } else {
            navigator.mediaDevices.getUserMedia({audio: true})
                .then(stream => {
                    navigator.mediaDevices.getUserMedia({audio: true})
                        .then(stream => {
                            mediaStream = stream;
                            mediaRecorder = new MediaRecorder(stream);

                            //while we record, we store the audio data in chunks.
                            if (mediaRecorder) {
                                setIsRecording(true);
                                mediaRecorder.start();
                                const audioChunks = [];
                                mediaRecorder.addEventListener("dataavailable", event => {
                                    audioChunks.push(event.data);
                                });

                                //Convert the audio data chunks to a single audio data blob
                                mediaRecorder.addEventListener("stop", async () => {
                                    const audioBlob = new Blob(audioChunks);

                                    //Create a URL for that single audio data blob
                                    const audioUrl = URL.createObjectURL(audioBlob);
                                    await RegisteredUser.addMessageToConvo(props.username, props.convo, {
                                        sender: true, type: "audio", time: new Date(), content: audioUrl
                                    });
                                    mediaRecorder = null;
                                    mediaStream = null;
                                    props.onHide();
                                    props.setConvo();
                                });
                            }
                        });
                });
        }
    }

    return (
        <Modal
            show={props.show}
            size="sm"
            aria-labelledby="record-modal"
            centered
        >
            <Modal.Header closeButton onClick={props.onHide}>
                <Modal.Title id="record-modal">
                    Record message
                </Modal.Title>
            </Modal.Header>
            <Modal.Body className="text-center">
                <Button onClick={onClick}>{isRecording ? 'Stop' : 'Record'}</Button>
            </Modal.Body>
        </Modal>
    );
}

export default RecordMessageModal;