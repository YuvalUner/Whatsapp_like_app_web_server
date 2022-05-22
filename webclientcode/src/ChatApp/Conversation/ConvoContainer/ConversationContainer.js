import {Component} from "react";
import ChatBubble from "./ChatBubble";
import Utils from "../../../Misc/Utils";
import Hashing from "../../../Misc/Hashing";

/**
 * Class of the container of the entire conversation.
 */
class ConversationContainer extends Component {

    //function creates the bubbles based on all inputs.
    generateChatBubbles =() =>{
        return(
            this.props.convo.map((message)=>
                <ChatBubble key={Hashing.cyrb53(message.content + Utils.generateRandString(128))}
                            content={message.content} time={message.created}
                            sender={message.sent} type={message.type}/>
        ))
    }

    componentDidMount() {
        Utils.scrollToBottom("convo-container");
    }

    componentDidUpdate(prevProps, prevState, snapshot) {
        Utils.scrollToBottom("convo-container");
    }

    render(){
        return(
            <div className="container-fluid chat-container overflow-lesser overflow-auto" id="convo-container">
                <div className="chat-panel">
                    {this.generateChatBubbles()}
                </div>
            </div>
        )
    }

}

export default ConversationContainer;