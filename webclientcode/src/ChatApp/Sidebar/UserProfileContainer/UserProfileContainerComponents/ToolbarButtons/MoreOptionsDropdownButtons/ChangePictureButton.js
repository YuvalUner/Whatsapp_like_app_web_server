import {Component} from "react";
import RegisteredUser from "../../../../../../Users/RegisteredUser";

/**
 * A button for changing the user's profile picture.
 */
class ChangePictureButton extends Component {

    // Needed because this is contained in the dropdown menu.
    handleClick = (e) => {
        e.stopPropagation();
    }

    // Update the user's profile picture to the new one.
    handleChange = (e) => {
        const url = URL.createObjectURL(e.target.files[0]);
        RegisteredUser.updateProfileImg(this.props.username, url);
        this.props.updateProfilePicture();
    }

    render() {
        return (
            <div>
                <label htmlFor="profile-image-input" className="form-label padding">Change profile picture</label>
                <span>
            <label htmlFor="profile-image-input" className="padding form-label hover-pointer"> <i
                className="bi bi-input bi-image"/></label>
            <input accept="image/*" onClick={this.handleClick} onChange={this.handleChange}
                   type="file" className="hide-stuff filename" id="profile-image-input"/>
        </span>
            </div>
        )
    }
}

export default ChangePictureButton;