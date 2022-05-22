import React from 'react';

/**
 * Micriphone button to send a voice recording.
 * @param openRecordMessageModal
 * @returns {JSX.Element}
 */
function MicButton({ openRecordMessageModal }) {

    //added a function onClick to open a modal for recording.
       return  <label onClick={openRecordMessageModal} className="padding form-label hover-pointer">
            <i className="bi bi-input bi-mic"/>
        </label>
}

export default MicButton;