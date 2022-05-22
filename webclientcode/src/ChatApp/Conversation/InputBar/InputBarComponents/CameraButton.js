import RegisteredUser from "../../../../Users/RegisteredUser";

/**
 * camera button to send picture in chat.
 * @param props
 * @returns {JSX.Element}
 */
function CameraButton({props}) {

    //function receives the picture and adds it as message to the conversation.
    const handleChange = async (e)=>{
        let url = URL.createObjectURL(e.target.files[0]);
        await RegisteredUser.addMessageToConvo(props.username, props.convo, {
            sender: true, type: "img", time: new Date(), content: url
        });
        props.setConvo();
    }

    return (
        <span>
            <label htmlFor="image-input" className="padding form-label hover-pointer"> <i className="bi bi-input bi-image"/></label>
            <input accept="image/*" type="file" onChange={handleChange} className="hide-stuff filename" id="image-input"/>
        </span>
    )
}

export default CameraButton;