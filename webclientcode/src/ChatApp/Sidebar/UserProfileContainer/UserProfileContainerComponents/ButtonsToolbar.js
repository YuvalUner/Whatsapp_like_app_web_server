import {Component} from "react";
import LogOutButton from "./ToolbarButtons/LogOutButton";
import AddContactButton from "./ToolbarButtons/AddContactButton";
import AddContactModal from "./AddContactModal";
import MoreOptionsDropdown from "./ToolbarButtons/MoreOptionsDropdown";


/**
 * A toolbar containing various button.
 * A class because it used to be more complex, but we did not want to re-write it after changing it.
 */
class ButtonsToolbar extends Component {
    render() {
        return (
            <div className="btn-group me-1 pe-3" role="group">
                <div>
                    <AddContactButton props={{setShow: this.props.setShow}}/>
                    <AddContactModal show={this.props.show} username={this.props.username}
                                     updateContacts={this.props.updateContacts}
                                     setShow={this.props.setShow} connection={this.props.connection}/>
                </div>
                <div className="me-2">
                    <LogOutButton props={{setLogIn: this.props.setLogIn}}/>
                </div>
                <div className="text-center">
                    <MoreOptionsDropdown props={{
                        username: this.props.username,
                        updateProfilePicture: this.props.updateProfilePicture,
                        updateNickname: this.props.updateNickname, connection: this.props.connection
                    }}/>
                </div>
            </div>
        )
    }
}

export default ButtonsToolbar;