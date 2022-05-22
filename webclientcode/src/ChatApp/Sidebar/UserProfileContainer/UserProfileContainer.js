import {Component} from "react";
import RegisteredUser from "../../../Users/RegisteredUser";
import ImageNameContainer from "./UserProfileContainerComponents/ImageNameContainer";
import ButtonsToolbar from "./UserProfileContainerComponents/ButtonsToolbar";
import $ from "jquery";

/**
 * Container for a user's profile.
 */
class UserProfileContainer extends Component {

    constructor(props) {
        super(props);
        this.state = {
            showModal: false, profilePicture: RegisteredUser.getImage(this.props.username),
            nickname: null,
            valid: false
        };
    }

    async componentDidMount() {
        this.setState({
            valid: true,
            nickname: await RegisteredUser.getNickname(this.props.username)
        })
    }

    /**
     * Setter for showing or hiding the add contact modal.
     * @param val
     */
    setShowModal = (val) => {
        this.setState({
            showModal: val
        });
        $("input[name=addContactSelectionRadio][value=username]").prop("checked",true);
    }

    /**
     * Updates the user's profile picture.
     */
    updateProfilePicture = () => {
        this.setState({
            profilePicture: RegisteredUser.getImage(this.props.username)
        });
    }


    /**
     * Determines which classes one of the divs should have.
     * @returns {string}
     */
    determineClasses = () => {
        if (this.props.renderButtons) {
            return "row bg-success pe-3 align-items-center";
        } else {
            return "row bg-success col-12 ms-0";
        }
    }


    render() {
        return (
            <div className={this.determineClasses()}>
                <div className={this.props.renderButtons ? "col-8" : ""}>
                    {this.state.valid && <ImageNameContainer
                        username={this.props.username}
                        renderNum={this.props.renderNum} profilePicture={this.state.profilePicture}
                        nickname={this.props.nickname}/>}
                </div>
                {/*Only render this part if this profile container is for the active user and not a contact*/}
                {this.props.renderButtons &&
                    <div className="col-4">
                        <div className="row justify-content-end me-2" role="toolbar">
                            <ButtonsToolbar setLogIn={this.props.setLogIn}
                                            username={this.props.username}
                                            updateContacts={this.props.updateContacts}
                                            setShow={this.setShowModal}
                                            show={this.state.showModal}
                                            updateProfilePicture={this.updateProfilePicture}
                                            updateNickname={this.props.updateNickname}
                                            connection={this.props.connection}/>
                        </div>
                    </div>}
            </div>
        )
    }
}

export default UserProfileContainer;