import CameraButton from "./CameraButton";
import VideoButton from "./VideoButton";
import MicButton from "./MicButton";

/**
 * attach menu for the button clicked to attach picture, video or voice recording.
 * @param props
 * @param openRecordMessageModal
 * @returns {JSX.Element}
 */
function AttachMenu({props, openRecordMessageModal}) {
    return (
        <span>
            <CameraButton props={{username: props.username, convo: props.convo, setConvo: props.setConvo}}/>
            <VideoButton props={{username: props.username, convo: props.convo, setConvo: props.setConvo}}/>
            <MicButton openRecordMessageModal={openRecordMessageModal}/>
        </span>
    )
}

export default AttachMenu;