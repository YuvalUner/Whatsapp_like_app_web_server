import InputBar from "./InputBar/InputBar";
import ConversationContainer from "./ConvoContainer/ConversationContainer";
import UserProfileContainer from "../Sidebar/UserProfileContainer/UserProfileContainer";
import {Component} from "react";
import RegisteredUser from "../../Users/RegisteredUser";

/**
 * class of the conversation in chat.
 */
class Conversation extends Component {

    /**
     * function renders all components of the chat conversation, including profile container, conversation container and input abr.
     * @returns {JSX.Element}
     */
    render() {
        return (
            <>
                {this.props.convo &&
                    <div className="col-8" id="conversation">
                        <UserProfileContainer nickname={this.props.nickname}
                                              username={this.props.convo}
                                              renderButtons={false}
                                              renderNum={false}/>
                        <ConversationContainer convo={this.props.convoContent}/>
                        <InputBar username={this.props.username}
                                  convo={this.props.convo}
                                  setConvo={this.props.setConvo}
                                  connection={this.props.connection}/>
                    </div>
                }
            </>
        )
    }

}

export default Conversation;