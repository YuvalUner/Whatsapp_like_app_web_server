import RegisteredUser from "../../../../Users/RegisteredUser";
import {Component} from "react";

/**
 * Contains the user's profile picture, nickname, and if needed also discriminator.
 * @param props
 * @returns {JSX.Element}
 */
class ImageNameContainer extends Component{

    constructor(props) {
        super(props);
        this.state = {
            valid: false,
            nicknum: null
        };
    }

    async componentDidMount(){
        this.setState({
            valid: true,
            nicknum: await RegisteredUser.getNickNum(this.props.username)
        });
    }

    render(){
        return (
            <>
                {this.props.username &&
                    <span className="break-text">
                    <img className="user-profile-img rounded-circle p-1 m-2"
                         src={this.props.profilePicture} alt="User profile image"/>
                    <span className="break-text">
                        {this.props.nickname}
                        {this.props.renderNum && ("#" + this.state.nicknum)}
                    </span>
                </span>
                }
            </>
        )
    }
}

export default ImageNameContainer;