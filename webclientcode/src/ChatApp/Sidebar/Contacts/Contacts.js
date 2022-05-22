import {Component} from "react";
import ContactContainer from "./ContactComponents/ContactContainer";
import SearchBar from "./ContactComponents/ContainerComponents/SearchBar";
import Utils from "../../../Misc/Utils";
import Hashing from "../../../Misc/Hashing";

/**
 * Holds and generates all the contact containers, the part seen in the buttom of the sidebar.
 */
class Contacts extends Component {


    createContacts = () => {
        return this.props.filteredContacts.map((contact) =>
            <ContactContainer username={contact.id} user={this.props.username} setConvo={this.props.setConvo}
                              key={contact.id} lastSeen={contact.lastdate} connection={this.props.connection}/>)
    }

    render() {
        return (
            <div className="row border-end border-dark border-1">
                <SearchBar props={{update: this.props.filterContacts}}/>
                <div className="bg-light overflow-auto list-group btn-group overflow-case">
                    {this.createContacts()}
                </div>
            </div>
        )
    }
}

export default Contacts;