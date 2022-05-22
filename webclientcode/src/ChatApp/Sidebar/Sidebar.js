import {Component} from "react";
import UserProfileContainer from "./UserProfileContainer/UserProfileContainer";
import Contacts from "./Contacts/Contacts";
import RegisteredUser from "../../Users/RegisteredUser";

/**
 * The sidebar of the main app.
 */
class Sidebar extends Component {

    constructor(props) {
        super(props);
        // let contactsTemp = RegisteredUser.getContacts(this.props.username);
        this.state = {
            contacts: null,
            filteredContacts: null,
            nickname: null,
            valid: false
        };
    }

    async componentDidMount() {
        let contactsTemp = await RegisteredUser.getContacts(this.props.username);
        this.setState({
            contacts: contactsTemp,
            filteredContacts: contactsTemp,
            nickname: await RegisteredUser.getNickname(this.props.username),
            valid: true
        });
        this.props.connection.on("updateContacts", async () => this.updateContacts());
    }

    /**
     * Updates the user's nickname.
     */
    updateNickname = async () => {
        this.setState({
            nickname: await RegisteredUser.getNickname(this.props.username)
        });
        this.props.connection.invoke("nicknameUpdated", this.props.username);
    }

    /**
     * Filters the contacts upon search.
     * @param val the value to filter by.
     */
    filterContacts = async (val) => {
        let contacts = this.state.contacts;
        this.setState({
            filteredContacts: contacts.filter(
                element => element.name.toLowerCase().includes(val.toLowerCase()))
        });
    }

    /**
     * Updates the user's contacts upon adding a contact.
     */
    updateContacts = async () => {
        let contactsTemp = await RegisteredUser.getContacts(this.props.username);
        this.setState({
            contacts: contactsTemp,
            filteredContacts: contactsTemp,
            shouldUpdate: false
        })
    }


    render() {
        return (
            <div className="col-3 ms-5 mh-75 pe-0" id="sidebar-div">
                {this.state.valid && <>
                    <UserProfileContainer username={this.props.username}
                                          setLogIn={this.props.setLogIn}
                                          renderButtons={true}
                                          renderNum={true}
                                          updateContacts={this.updateContacts}
                                          updateNickname={this.updateNickname}
                                          nickname={this.state.nickname}
                                          connection={this.props.connection}/>
                    <Contacts username={this.props.username}
                              shouldUpdate={this.state.shouldUpdate}
                              contacts={this.state.contacts}
                              filteredContacts={this.state.filteredContacts}
                              filterContacts={this.filterContacts}
                              setConvo={this.props.setConvo}
                              connection={this.props.connection}/>
                </>}
            </div>
        )
    }
}

export default Sidebar;