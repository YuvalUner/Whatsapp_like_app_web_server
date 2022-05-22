import {Component} from "react";
import RegisteredUser from "../../../../Users/RegisteredUser";
import ImageNameContainer from "../../UserProfileContainer/UserProfileContainerComponents/ImageNameContainer";
import $ from "jquery"

/**
 * A container for all of a contact's information.
 */
class ContactContainer extends Component {

    constructor(props) {
        super(props);
        this.state = {
            lastMessageDate: Date.parse(props.lastSeen),
            description: null,
            valid: false
        };
        this.props.connection.on("updateTime", () => this.updateTime());
    }

    /**
     * Updates the contact's last seen on the screen.
     */
    updateTime = () => {
        this.setState(
            {lastMessageDate: Date.parse(this.props.lastSeen)}
        );
    }

    /**
     * Make updates occur every minute.
     */
    async componentDidMount() {
        setInterval(this.updateTime, 60000);
        this.setState({
            nickname: await RegisteredUser.getNickname(this.props.username),
            description: await RegisteredUser.getDescription(this.props.username),
            valid: true
        });
        this.props.connection.on("nicknameChanged", async () => {
            this.setState({
                nickname: await  RegisteredUser.getNickname(this.props.username)
            });
        });
        this.props.connection.on("descriptionChanged", async () => {
            this.setState({
                description: await RegisteredUser.getDescription(this.props.username)
            });
        })
    }

    /**
     * Determine which string to display for the "just seen" portion.
     * @returns {string}
     */
    timeFromLast = () => {
        let timeDelta = Math.floor((new Date() - this.state.lastMessageDate) / 60000);
        if (timeDelta === 0) {
            return "Just now";
            // If less than an hour.
        } else if (timeDelta < 60) {
            return timeDelta + " minutes ago";
        }
        // If less than a day but more than an hour.
        else if (timeDelta < 1440) {
            return Math.floor(timeDelta / 60) + " hours ago";
        } else {
            let date = new Date(this.state.lastMessageDate);
            let day = date.getDate().toString().padStart(2, "0");
            let month = date.getMonth() + 1;
            let year = date.getFullYear();
            return day + "/" + month + "/" + year;
        }
    }

    /**
     * Adds a clear indication to the user that they chose the contact, and changes to that contact.
     */
    focusHandler = async (e) => {
        e.persist();
        let thisItem = $("#contact" + this.props.username);
        thisItem.addClass("active border-primary border-primary border-2");
        await this.props.setConvo(this.props.username);
    }

    blurHandler = () => {
        let thisItem = $("#contact" + this.props.username);
        thisItem.removeClass("active border-primary border-2");
    }


    render() {
        return (
            <li className="d-grid list-group-item bg-light hover-pointer mw-50" id={"contact" + this.props.username}>
                <button onFocus={async (e)=> await this.focusHandler(e)} onBlur={this.blurHandler}
                        className="btn no-effect-button text-start btn-flex justify-content-left break-text">
                    <div className="col">
                        <div className="break-text">
                            {this.state.valid && <ImageNameContainer
                                username={this.props.username}
                                nickname={this.state.nickname}
                                renderNum={false} profilePicture={RegisteredUser.getImage(this.props.username)}/>}
                            <span className="float-end small-text">
                                {this.timeFromLast()}
                            </span>
                            {this.state.valid &&
                                <div className="small-text break-text">
                                    {this.state.description}
                                </div>
                            }
                        </div>
                    </div>
                </button>
            </li>
        )
    }
}

export default ContactContainer;